using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.APIs.DTOs.Project;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()//you will never need ReverseMap cause you give the user info about Product not take info from him maybe if in admin role need to reverse  map
                .ForMember(D => D.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
                .ForMember(D => D.ProductType, O => O.MapFrom(S => S.ProductType.Name))
                .ForMember(D => D.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());//he understand he will make resolve cause the Interface is come with lib automapper  , so the new version of mapfrom<take clas implements IResolver>

            CreateMap<Project, ProjectToReturnDto>();

            CreateMap<Tasky, TaskDto>()
                .ForMember(D => D.DeveloperName, O => O.MapFrom(S => S.Developer.UserName))
                .ForMember(D => D.ProjectName, O => O.MapFrom(S => S.Project.Name))
                .ForMember(D=>D.comments,O=>O.MapFrom(S=>S.Comments))
                ;

            CreateMap<TaskToCreateDto, Tasky>();
            CreateMap<Tasky,TaskWithAttach>()
                .ForMember(D => D.UploadedAttachment, O => O.MapFrom<TaskFileUrl>());



            CreateMap<Comment, CommentDto>()
                .ForMember(D => D.Id, O => O.MapFrom(S => S.Id))
                .ForMember(D => D.Name, O => O.MapFrom(S => S.Name))
                .ForMember(D => D.Role, O => O.MapFrom(S => S.Role))
                .ForMember(D => D.Text, O => O.MapFrom(S => S.Text))
                .ReverseMap();
                

            CreateMap<CreateComment, Comment>();



            



            CreateMap<Users, DeveloperDto>();

            CreateMap<ProjectToCreateDto, Project>();
            CreateMap<ProjectToUpdateDto, Project>();



            CreateMap<UpdateTaskStatus, Tasky>();




        }
    }
}
