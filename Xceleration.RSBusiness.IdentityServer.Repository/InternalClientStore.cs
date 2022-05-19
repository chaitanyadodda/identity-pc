using AutoMapper;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xceleration.RSBusiness.IdentityServer.Contracts.Store;
using Xceleration.RSBusiness.IdentityServer.Stores.DbContexts;
using Xceleration.RSBusiness.IdentityServer.Stores.Extensions;

namespace Xceleration.RSBusiness.IdentityServer.Stores;

public class InternalClientStore : IInternalClientStore
{
    private readonly PcConfigurationDbContext _context;
    private readonly ILogger<InternalClientStore> _logger;
    private readonly IMapper _mapper;

    public InternalClientStore(PcConfigurationDbContext context, ILogger<InternalClientStore> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }


    public async Task<Client> GetClientByOrigin(string origin, CancellationToken cancellationToken)
    {
        var query = _context.Clients
            .Include(x => x.AllowedScopes)
            .Include(x => x.AllowedGrantTypes)
            .Join(_context.ClientCorsOrigins,
                c => c.Id,
                o => o.ClientId,
                (c, o) => new { Client = c, Origin = o })
            .Where(c => c.Origin.Origin == origin)
            .Select(c => c.Client);

        _logger.LogQuery(query);

        var client = (await query.ToArrayAsync(cancellationToken))
            .FirstOrDefault();

        _logger.LogDebug("{origin} found in database {originFound}", origin, client != null);
        return client?.ToModel();
    }
}