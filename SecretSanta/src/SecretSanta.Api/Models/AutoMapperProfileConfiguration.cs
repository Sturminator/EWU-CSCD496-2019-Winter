using AutoMapper;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Api.Models
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<Gift, GiftViewModel>();
            CreateMap<GiftViewModel, Gift>();

            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
            CreateMap<UserInputViewModel, User>();

            CreateMap<Group, GroupViewModel>();
            CreateMap<GroupViewModel, Group>();
            CreateMap<GroupInputViewModel, Group>();
        }
    }
}
