using AutoMapper;
using SecretSanta.Web.ApiModels;

namespace SecretSanta.Web.Models
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<UserViewModel, UserInputViewModel>();
            CreateMap<GroupViewModel, GroupInputViewModel>();
        }
    }
}