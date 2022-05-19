using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;
using Xceleration.RSBusiness.IdentityServer.Contracts.Store;
using Xceleration.RSBusiness.IdentityServer.Stores.DbContexts;
using Xceleration.RSBusiness.IdentityServer.Stores.Extensions;

namespace Xceleration.RSBusiness.IdentityServer.Stores;

public class UserStore : IUserStore
{
    private readonly UserDbContext _context;
    private readonly ILogger<UserStore> _logger;
    private readonly IMapper _mapper;

    public UserStore(ILogger<UserStore> logger, UserDbContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> GetUserByUserId(int corporateAccountId, string userId,
        CancellationToken cancellationToken)
    {
        var query = _context.MemberMasters
            .Include(x => x.AddressMaster)
            .ThenInclude(x => x.CityMaster)
            .ThenInclude(x => x.StateMaster)
            .Include(x => x.AddressMaster)
            .ThenInclude(x => x.CountryMaster)
            .Include(x => x.AddressMaster)
            .ThenInclude(x => x.DistrictMaster)
            .Include(x => x.MemberDetails)
            .Include(m => m.LoginMaster)
            .Where(m => (m.MemberDetails.CorporateMemberIsDeActive == null ||
                         m.MemberDetails.CorporateMemberIsDeActive == 0) &&
                        m.LoginMaster.WebUserId == userId &&
                        m.MemberDetails.CorporateAccountId == corporateAccountId)
            .Take(2);
        _logger.LogQuery(query);
        var users = await query.ToArrayAsync(cancellationToken);

        _logger.LogDebug("{corporateAccountId}:{userId} found in database {userFound}", corporateAccountId, userId,
            users.Any());

        if (users.Length > 1)
        {
            _logger.LogDebug("{corporateAccountId}:{userId} had more than one record.", corporateAccountId, userId);
            return null;
        }

        var user = users.FirstOrDefault();

        return _mapper.Map<UserDto>(user);
    }
}