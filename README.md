# ERP20

## Requiment
  ### Visual studio 2019
  ### Sql server version > 2014
1. Full code from develop
2. Gen database from ERP20/Database/data.sql
3. Change connectionStrings from /ERP.API/Web.config , Data Source="your data server"
4. Rebuild source code

## Step to create one api

1. Create Interface IResponsitory
2. Create Class Responsitory
3. Create Interface IService
4. Create Class Service
5. Create class Model from /ERP.API/Models to Mapp data from client to server.
6. Add class want mapper from client to server : ERP.API/AutoMapper/MappingConfig.cs
7. Create Controller
8. If there are other functions that need to be modified or deleted, it needs to be redefined from Service file to Repository file and the business will be processed in the Repository file.
