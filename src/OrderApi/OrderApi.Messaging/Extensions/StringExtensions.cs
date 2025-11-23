using System.Text.RegularExpressions;

namespace OrderApi.Messaging.Extensions;

public static partial class StringExtensions
{
    public static string PascalToKebabCase(this string str)
    {
        return PascalToKebabCaseRegex().Replace(str, "-$1")
            .ToLower();
    }

    [GeneratedRegex(@"(?<!^)(?<!-)((?<=\p{Ll})\p{Lu}|\p{Lu}(?=\p{Ll}))")]
    private static partial Regex PascalToKebabCaseRegex();
}