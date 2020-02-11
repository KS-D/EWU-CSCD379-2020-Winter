using AutoMapper;
using SecretSanta.Data;
using System.Reflection;

namespace SecretSanta.Business
{
    public class AutomapperConfigurationProfile : Profile
    {
        public AutomapperConfigurationProfile()
        {
            CreateMap<Gift, Dto.Gift>();
            CreateMap<Dto.GiftInput, Gift>();

            CreateMap<User, Dto.User>();
            CreateMap<Dto.UserInput, User>();
            
            CreateMap<Group, Dto.Group>();
            CreateMap<Dto.GroupInput, Group>();
        }

        public static IMapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            return mapperConfiguration.CreateMapper();
        }
    }
}
