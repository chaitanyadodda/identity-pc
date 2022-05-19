using System.Text;
using AutoMapper;
using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;
using Xceleration.RSBusiness.IdentityServer.Stores.Entities;

namespace Xceleration.RSBusiness.IdentityServer.Stores.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<MemberDetail, UserDto>()
            .ForMember(m => m.IsActive,
                opt => opt.MapFrom(src => (src.CorporateMemberIsDeActive ?? 0) == 0));

        CreateMap<LoginMaster, UserDto>()
            .ForMember(m => m.Password, opt => opt.MapFrom(src => src.WebPassword))
            .ForMember(m => m.SubjectId, opt => opt.MapFrom(src => src.WebUserId))
            .ForMember(x => x.Name, opt => opt.MapFrom(src => src.EntityName));

        CreateMap<MemberMaster, UserDto>()
            .IncludeMembers(m => m.MemberDetails, m => m.LoginMaster, m => m.AddressMaster)
            .ForMember(m => m.Email, opt => opt.MapFrom(src => src.EmailId))
            .ForMember(m => m.DateOfBirth, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth)))
            .ForMember(m => m.Phone, opt => opt.MapFrom(src => src.MobileNo));

        CreateMap<AddressMaster, UserDto>()
            .IncludeMembers(m => m.CityMaster, m => m.DistrictMaster, m => m.CountryMaster)
            .ForMember(m => m.StreetAddress, opt => opt.MapFrom<StreetAddressResolver>());

        CreateMap<CityMaster, UserDto>()
            .IncludeMembers(m => m.StateMaster)
            .ForMember(m => m.City, opt => opt.MapFrom(src => src.CityName));

        CreateMap<DistrictMaster, UserDto>()
            .ForMember(m => m.District, opt => opt.MapFrom(src => src.DistrictName));

        CreateMap<StateMaster, UserDto>()
            .ForMember(m => m.State, opt => opt.MapFrom(src => src.StateName));

        CreateMap<CountryMaster, UserDto>()
            .ForMember(m => m.Country, opt => opt.MapFrom(src => src.CountryName));
    }

    internal class StreetAddressResolver : IValueResolver<AddressMaster, UserDto, string>
    {
        public string Resolve(AddressMaster source, UserDto destination, string destMember,
            ResolutionContext context)
        {
            var streetAddressBuilder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(source.Address1))
                streetAddressBuilder.Append($"{source.Address1} ");

            if (!string.IsNullOrWhiteSpace(source.Address2))
                streetAddressBuilder.Append($"{source.Address2} ");

            if (!string.IsNullOrWhiteSpace(source.Address3))
                streetAddressBuilder.Append($"{source.Address3} ");

            return streetAddressBuilder.ToString().Trim();
        }
    }
}