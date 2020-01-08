using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ManagementLibrary.API.Controllers.Authentication
{
    public class ManagerUsersController : ApiController
    {
        private readonly IUsersService _Userservice;

        private readonly IMapper _mapper;

        public ManagerUsersController(IUsersService Userservice, IMapper mapper)
        {
            this._Userservice = Userservice;
            this._mapper = mapper;
        }
    }
}
