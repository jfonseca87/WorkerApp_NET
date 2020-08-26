using AutoMapper;
using WorkerApp.Domain;
using WorkerApp.MVC.Models;

namespace WorkerApp.MVC.Utilities
{
    public class MapperSetup
    {
        public IMapper CreateMapperProfile()
        {
            var config = new MapperConfiguration(cnf =>
            {
                cnf.CreateMap<File, FileViewModel>().ReverseMap();
                cnf.CreateMap<Person, PersonViewModel>().ReverseMap();
                cnf.CreateMap<FileAttached, FileAttachedViewModel>().ReverseMap();
                cnf.CreateMap<FileAttachedInsertViewModel, FileAttached>();
                cnf.CreateMap<LoginViewModel, User>();
                cnf.CreateMap<RegisterViewModel, User>();
            });

            return config.CreateMapper();
        }
    }
}