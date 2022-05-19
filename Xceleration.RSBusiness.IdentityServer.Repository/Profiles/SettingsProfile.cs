using AutoMapper;
using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;
using Xceleration.RSBusiness.IdentityServer.Stores.Entities;

namespace Xceleration.RSBusiness.IdentityServer.Stores.Profiles;

public class SettingsProfile : Profile
{
    public SettingsProfile()
    {
        CreateMap<OrganizationSetting, SettingsDto>();
        CreateMap<ClientOrganizationSetting, SettingsDto>()
            .ConstructUsing((s, ctx) => ctx.Mapper.Map<SettingsDto>(s.OrganizationSetting));
    }
}