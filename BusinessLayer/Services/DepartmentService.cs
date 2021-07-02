using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class DepartmentService:Repository<Department>, IDepartmentService
    {
        private readonly IConfiguration _configuration;
        public readonly string baseUrl;

        public DepartmentService(ELearnContext context, IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
            baseUrl = configuration.GetValue<string>("Url:root");
            
        }

    }
}
