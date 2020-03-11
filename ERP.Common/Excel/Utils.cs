using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using OfficeOpenXml;

namespace ERP.Common.Excel
{
    public class ExcelImport
    {
        public static DataSet ImportExcelXLS(HttpPostedFileWrapper file, bool hasHeaders)
        {
            string fileName = Path.GetTempFileName();
            file.SaveAs(fileName);

            return ImportExcelXLS(fileName, hasHeaders);
        }

        public static DataSet ImportExcelXLS(string FileName, bool hasHeaders)
        {

            string HDR = hasHeaders ? "Yes" : "No";


            string strConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=1\"";
            DataSet output = new DataSet();

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                conn.Open();

                DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                foreach (DataRow row in dt.Rows)
                {
                    string sheet = row["TABLE_NAME"].ToString();

                    OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheet + "]", conn);
                    cmd.CommandType = CommandType.Text;

                    DataTable outputTable = new DataTable(sheet);
                    output.Tables.Add(outputTable);
                    new OleDbDataAdapter(cmd).Fill(outputTable);
                }
            }
            return output;
        }

        struct ColumnType
        {
            public Type type;
            private string name;
            public ColumnType(Type type) { this.type = type; this.name = type.ToString().ToLower(); }
            public object ParseString(string input)
            {
                if (String.IsNullOrEmpty(input))
                    return DBNull.Value;
                switch (type.ToString())
                {
                    case "system.datetime":
                        return DateTime.Parse(input);
                    case "system.decimal":
                        return decimal.Parse(input);
                    case "system.boolean":
                        return bool.Parse(input);
                    default:
                        return input;
                }
            }
        }

        public static DataSet ImportExcelXML(HttpPostedFile file, bool hasHeaders, bool autoDetectColumnType)
        {
            return ImportExcelXML(file.InputStream, hasHeaders, autoDetectColumnType);
        }

        public static DataSet ImportExcelXML(Stream inputFileStream, bool hasHeaders, bool autoDetectColumnType)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new XmlTextReader(inputFileStream));
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);

            nsmgr.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
            nsmgr.AddNamespace("x", "urn:schemas-microsoft-com:office:excel");
            nsmgr.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");

            DataSet ds = new DataSet();

            foreach (XmlNode node in doc.DocumentElement.SelectNodes("//ss:Worksheet", nsmgr))
            {
                DataTable dt = new DataTable(node.Attributes["ss:Name"].Value);
                ds.Tables.Add(dt);
                XmlNodeList rows = node.SelectNodes("ss:Table/ss:Row", nsmgr);
                if (rows.Count > 0)
                {
                    List<ColumnType> columns = new List<ColumnType>();
                    int startIndex = 0;
                    if (hasHeaders)
                    {
                        foreach (XmlNode data in rows[0].SelectNodes("ss:Cell/ss:Data", nsmgr))
                        {
                            columns.Add(new ColumnType(typeof(string)));//default to text
                            dt.Columns.Add(data.InnerText, typeof(string));
                        }
                        startIndex++;
                    }
                    if (autoDetectColumnType && rows.Count > 0)
                    {
                        XmlNodeList cells = rows[startIndex].SelectNodes("ss:Cell", nsmgr);
                        int actualCellIndex = 0;
                        for (int cellIndex = 0; cellIndex < cells.Count; cellIndex++)
                        {
                            XmlNode cell = cells[cellIndex];
                            if (cell.Attributes["ss:Index"] != null)
                                actualCellIndex = int.Parse(cell.Attributes["ss:Index"].Value) - 1;

                            ColumnType autoDetectType = getType(cell.SelectSingleNode("ss:Data", nsmgr));

                            if (actualCellIndex >= dt.Columns.Count)
                            {
                                dt.Columns.Add("Column" + actualCellIndex.ToString(), autoDetectType.type);
                                columns.Add(autoDetectType);
                            }
                            else
                            {
                                dt.Columns[actualCellIndex].DataType = autoDetectType.type;
                                columns[actualCellIndex] = autoDetectType;
                            }

                            actualCellIndex++;
                        }
                    }
                    for (int i = startIndex; i < rows.Count; i++)
                    {
                        DataRow row = dt.NewRow();
                        XmlNodeList cells = rows[i].SelectNodes("ss:Cell", nsmgr);
                        int actualCellIndex = 0;
                        for (int cellIndex = 0; cellIndex < cells.Count; cellIndex++)
                        {
                            XmlNode cell = cells[cellIndex];
                            if (cell.Attributes["ss:Index"] != null)
                                actualCellIndex = int.Parse(cell.Attributes["ss:Index"].Value) - 1;

                            XmlNode data = cell.SelectSingleNode("ss:Data", nsmgr);

                            if (actualCellIndex >= dt.Columns.Count)
                            {
                                for (int j = dt.Columns.Count; j < actualCellIndex; j++)
                                {
                                    dt.Columns.Add("Column" + actualCellIndex.ToString(), typeof(string));
                                    columns.Add(getDefaultType());
                                }
                                ColumnType autoDetectType = getType(cell.SelectSingleNode("ss:Data", nsmgr));
                                dt.Columns.Add("Column" + actualCellIndex.ToString(), typeof(string));
                                columns.Add(autoDetectType);
                            }
                            if (data != null)
                                row[actualCellIndex] = data.InnerText;

                            actualCellIndex++;
                        }

                        dt.Rows.Add(row);
                    }
                }
            }
            return ds;

            //<?xml version="1.0"?>
            //<?mso-application progid="Excel.Sheet"?>
            //<Workbook>
            // <Worksheet ss:Name="Sheet1">
            //  <Table>
            //   <Row>
            //    <Cell><Data ss:Type="String">Item Number</Data></Cell>
            //    <Cell><Data ss:Type="String">Description</Data></Cell>
            //    <Cell ss:StyleID="s21"><Data ss:Type="String">Item Barcode</Data></Cell>
            //   </Row>
            // </Worksheet>
            //</Workbook>
        }

        private static ColumnType getDefaultType()
        {
            return new ColumnType(typeof(String));
        }

        private static ColumnType getType(XmlNode data)
        {
            string type = null;
            if (data.Attributes["ss:Type"] == null || data.Attributes["ss:Type"].Value == null)
                type = "";
            else
                type = data.Attributes["ss:Type"].Value;

            switch (type)
            {
                case "DateTime":
                    return new ColumnType(typeof(DateTime));
                case "Boolean":
                    return new ColumnType(typeof(Boolean));
                case "Number":
                    return new ColumnType(typeof(Decimal));
                case "":
                    decimal test2;
                    if (data == null || String.IsNullOrEmpty(data.InnerText) || decimal.TryParse(data.InnerText, out test2))
                    {
                        return new ColumnType(typeof(Decimal));
                    }
                    else
                    {
                        return new ColumnType(typeof(String));
                    }
                default://"String"
                    return new ColumnType(typeof(String));
            }
        }
    }
    public class ExcelExport
    {
        static int i = 1;
        public static void ExportToExcel(DataSet oDs, string strFileName)
        {
            try
            {
                FileInfo newFile = new FileInfo(strFileName);

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Do the export stuff here..
                    int i = 0;
                    foreach (DataTable odt in oDs.Tables)
                    {


                        i++;
                        string sheetname = null == odt.TableName || odt.TableName.Equals(string.Empty) ? "Sheet" + i.ToString() : odt.TableName;
                        AddSheetsToWorkBookFromDataTable(xlPackage, odt, sheetname);

                    }
                    xlPackage.Save();
                    i = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // i = 0;
        }

        public static void ApplyFormattingToARangeByDataType(ExcelRange oRange, DataColumn oDC)
        {

            if (IsDate(oDC))
            {
                oRange.Style.Numberformat.Format = @"dd/mm/yyyy hh:mm:ss AM/PM";
            }
            else if (IsInteger(oDC))
            {
                //Do Nothing
            }
            else if (IsNumeric(oDC))
            {
                oRange.Style.Numberformat.Format = @"#.##";
            }
            oRange.AutoFitColumns();
        }

        public static void AddSheetsToWorkBookFromDataTable(ExcelPackage oPack, DataTable oDT, string SheetName)
        {
            try
            {
                ExcelWorksheet oWs = oPack.Workbook.Worksheets.Add(null == oDT.TableName || oDT.TableName.Equals(string.Empty) ? "Sheet" + i.ToString() : oDT.TableName);
                oWs.Cells.Style.Font.Name = "Calibiri";
                oWs.Cells.Style.Font.Size = 10;


                int ColCnt =
                            oDT.Columns.Count, RowCnt = oDT.Rows.Count;


                //Export each row..
                oWs.Cells["A1"].LoadFromDataTable(oDT, true);
                //Format the header
                using (ExcelRange oRange = oWs.Cells["A1:" + GetColumnAlphabetFromNumber(ColCnt) + "1"])
                {
                    oRange.Style.Font.Bold = true;
                    oRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    oRange.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(229, 229, 229));
                }
                int CurrentColCount = 1;
                foreach (DataColumn oDC in oDT.Columns)
                {
                    using (ExcelRange oRange = oWs.Cells[GetColumnAlphabetFromNumber(CurrentColCount) + "1:" + GetColumnAlphabetFromNumber(CurrentColCount) + RowCnt.ToString()])
                    {
                        ApplyFormattingToARangeByDataType(oRange, oDC);
                    }
                    CurrentColCount++;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsInteger(DataColumn col)
        {
            if (col == null)
                return false;

            var numericTypes = new[] { typeof(Byte),
                                                typeof(Int16), typeof(Int32), typeof(Int64), typeof(SByte),
                                                 typeof(UInt16), typeof(UInt32), typeof(UInt64)};
            return numericTypes.Contains(col.DataType);
        }

        public static bool IsNumeric(DataColumn col)
        {
            if (col == null)
                return false;
            var numericTypes = new[] {  typeof(Decimal), typeof(Double),
                                                typeof(Single)};
            return numericTypes.Contains(col.DataType);
        }

        public static bool IsDate(DataColumn col)
        {
            if (col == null)
                return false;
            var numericTypes = new[] { typeof(DateTime), typeof(TimeSpan) };
            return numericTypes.Contains(col.DataType);
        }

        public static string GetColumnAlphabetFromNumber(int iColCount)
        {
            string strColAlpha = string.Empty;

            try
            {
                int iloop = iColCount, icount1 = 0, icount2 = 0;
                Char chr = ' ';

                while (iloop > 676)
                {
                    iloop -= 676;
                    icount1++;
                }

                if (icount1 != 0)
                {
                    chr = (Char)(64 + icount1);
                    strColAlpha = chr.ToString();
                }
                while (iloop > 26)
                {
                    iloop -= 26;
                    icount2++;
                }
                if (icount2 != 0)
                {
                    chr = (Char)(64 + icount2);
                    strColAlpha = strColAlpha + chr.ToString();
                }
                chr = (Char)(64 + iloop);
                strColAlpha = strColAlpha + chr.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strColAlpha;
        }

        public static void ExportToExcel(SqlDataReader oReader, string strFileName)
        {
            throw new NotImplementedException();
        }

        public static void ExportToExcel(DataTable oDT, string strFileName)
        {
            try
            {
                FileInfo newFile = new FileInfo(strFileName);

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Do the export stuff here..
                    string sheetname = null == oDT.TableName || oDT.TableName.Equals(string.Empty) ? "Sheet1" : oDT.TableName;
                    AddSheetsToWorkBookFromDataTable(xlPackage, oDT, sheetname);
                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExportToExcel(DataTable oDT, Dictionary<string, string> dicColumnNames, string strFileName, string tableName = "")
        {
            var dt = ConstructDataTable(oDT, dicColumnNames);

            if (!string.IsNullOrEmpty(tableName))
                dt.TableName = tableName;
            ExportToExcel(dt, strFileName);
        }

        public static void ExportToExcelFromList<T>(List<T> oDT, Dictionary<string, string> dicColumnNames, string strFileName, string tableName = "")
        {
            var dt = ConstructDataTable(oDT, dicColumnNames);

            if (!string.IsNullOrEmpty(tableName))
                dt.TableName = tableName;
            ExportToExcel(dt, strFileName);
        }

        private static DataTable ConstructDataTable(DataTable dt, Dictionary<string, string> dicColumnNames)
        {


            DataTable dtRet = new DataTable();

            foreach (var col in dicColumnNames.Keys)
            {
                dtRet.Columns.Add(dicColumnNames[col], dt.Columns[col].DataType);
            }

            foreach (DataRow dr in dt.Rows)
            {
                DataRow nDr = dtRet.NewRow();

                foreach (var col in dicColumnNames.Keys)
                {
                    nDr[dicColumnNames[col]] = dr[col];
                }

                dtRet.Rows.Add(nDr);
            }


            return dtRet;



        }

        private static DataTable ConstructDataTable<T>(List<T> dt, Dictionary<string, string> dicColumnNames)
        {


            DataTable dtRet = new DataTable();
            
            foreach (var col in dicColumnNames.Keys)
            {
                dtRet.Columns.Add(dicColumnNames[col], typeof(string));
            }

            foreach (T dr in dt)
            {
                DataRow nDr = dtRet.NewRow();

                foreach (var col in dicColumnNames.Keys)
                {
                    foreach (var prop in dr.GetType().GetProperties())
                    {
                        //nDr[dicColumnNames[col]] = dr.GetType().GetProperties().GetValue(dr, null);
                        if (prop.Name.Equals(col, StringComparison.InvariantCultureIgnoreCase))
                        {
                            nDr[dicColumnNames[col]] = prop.GetValue(dr, null);
                        }
                    }
                }

                dtRet.Rows.Add(nDr);
            }


            return dtRet;



        }
    }

    public class DataTableCmUtils
    {
        public static List<T> ToListof<T>(DataTable dt)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns .Cast<DataColumn>()
                .Select(c => c.ColumnName.ToUpper())
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Skip(1).Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name.ToUpper()) && dataRow[properties.Name.ToUpper()] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, ChangeType(dataRow[properties.Name], properties.PropertyType), null);
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }

        public static List<T> DtToList<T>(DataTable dt) where T : class, new()
        {
            List<T> lst = new List<T>();

            foreach (var row in dt.AsEnumerable())
            {
                T obj = new T();

                foreach (var prop in obj.GetType().GetProperties())
                {
                    PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                    propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                }

                lst.Add(obj);
            }

            return lst;
        }

        public static DataTable ToDataTable<T>(IList<T> data, string dtName)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable(dtName);
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }
    }
}
