using Domain.DTOs;
using System;
using AutoMapper;

using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Domain.Entities;

namespace Domain.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<TestDTO, Test>().ReverseMap();
        

        }
    }

}
