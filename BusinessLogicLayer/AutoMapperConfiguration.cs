using AutoMapper;
using BusinessLogicLayer.Models;
using DBUser = DataAccessLayer.Entities.User;
using DBTransaction = DataAccessLayer.Entities.Transaction;

namespace BusinessLogicLayer
{
    public static class AutoMapperConfiguration
    {
        static AutoMapperConfiguration()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, DBUser>(MemberList.Source)
                    .ForSourceMember(source => source.PasswordRepeat, dest => dest.DoNotValidate())
                    .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
                cfg.CreateMap<DBUser, User>(MemberList.Destination)
                    .ForMember(source => source.PasswordRepeat, dest => dest.Ignore());
                cfg.CreateMap<Transaction, DBTransaction>(MemberList.Source)
                    .ForSourceMember(source => source.ReceiverUserName, dest => dest.DoNotValidate())
                    .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
                //cfg.CreateMap<DBTransaction, Transaction>(MemberList.Destination);
            });
            Mapper = MapperConfiguration.CreateMapper();
            MapperConfiguration.AssertConfigurationIsValid();
        }

        public static IMapper Mapper { get; private set; }

        public static MapperConfiguration MapperConfiguration { get; private set; }
    }
}