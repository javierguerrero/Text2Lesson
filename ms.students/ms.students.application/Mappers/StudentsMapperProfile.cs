using AutoMapper;
using ms.students.application.Responses;
using ms.students.domain.Entities;

namespace ms.students.application.Mappers
{
    public class StudentsMapperProfile : Profile
    {
        public StudentsMapperProfile()
        {
            CreateMap<Student, StudentResponse>().ReverseMap();
        }
    }
}