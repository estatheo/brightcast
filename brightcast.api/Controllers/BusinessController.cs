using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using brightcast.Helpers;
using brightcast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brightcast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessController : ControllerBase
    {
        private IUserProfileService _userProfileService;
        private IBusinessService _businessService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public BusinessController(
            IUserProfileService userProfileService,
            IBusinessService businessService,
            IMapper mapper,
            AppSettings appSettings
        )
        {
            _userProfileService = userProfileService;
            _businessService = businessService;
            _mapper = mapper;
            _appSettings = appSettings;
        }
    }
}