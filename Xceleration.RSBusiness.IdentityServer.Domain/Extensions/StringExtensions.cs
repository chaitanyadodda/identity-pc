using System.Globalization;
using System.Text.RegularExpressions;

namespace Xceleration.RSBusiness.IdentityServer.Domain.Extensions;

public static class StringExtensions
{
    public static string ToCamelCase(this string str)
    {
        var pattern = new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+");
        var matches = pattern.Matches(str);
        return new string(
            new CultureInfo("en-US", false)
                .TextInfo
                .ToTitleCase(
                    string.Join(" ", matches
                        .Select(m => m.Value).ToArray()).ToLower()
                )
                .Replace(@" ", "")
                .Select((x, i) => i == 0 ? char.ToLower(x) : x)
                .ToArray()
        );
    }
}