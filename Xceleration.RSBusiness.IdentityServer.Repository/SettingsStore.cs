using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;
using Xceleration.RSBusiness.IdentityServer.Contracts.Store;
using Xceleration.RSBusiness.IdentityServer.Stores.DbContexts;
using Xceleration.RSBusiness.IdentityServer.Stores.Entities;
using Xceleration.RSBusiness.IdentityServer.Stores.Extensions;

namespace Xceleration.RSBusiness.IdentityServer.Stores;

public class SettingsStore : ISettingsStore
{
    private readonly PcConfigurationDbContext _context;
    private readonly ILogger<InternalClientStore> _logger;
    private readonly IMapper _mapper;

    public SettingsStore(PcConfigurationDbContext context, ILogger<InternalClientStore> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<SettingsDto> GetSettingsByClientId(string clientId, CancellationToken cancellationToken)
    {
        var query = _context.ClientOrganizationSettings
            .Join(_context.Clients, s => s.ClientId, c => c.Id, (s, c) => new
            {
                Setting = s,
                Client = c
            })
            .Join(_context.OrganizationSettings, s => s.Setting.OrganizationSettingId, os => os.Id, (s, os) =>
                new ClientOrganizationSetting
                {
                    Client = s.Client,
                    OrganizationSetting = os
                })
            .Where(os => os.Client.ClientId == clientId);

        _logger.LogQuery(query);
        var setting = await query.FirstOrDefaultAsync(cancellationToken);

        if (setting != null) return _mapper.Map<SettingsDto>(setting);

        _logger.LogError("Unable to find settings for {clientId}", clientId);
        return new SettingsDto();
    }
}