using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Xceleration.RSBusiness.IdentityServer.Stores.Extensions;

public static class QueryableExtensions
{
    public static void LogQuery(this ILogger logger, IQueryable query, [CallerMemberName] string caller = null)
    {
        logger.LogDebug("Query in {method} - {query}", caller, query.ToQueryString());
    }
}