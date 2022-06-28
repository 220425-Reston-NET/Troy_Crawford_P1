namespace StoreApp.Extensions;

public static class StringExtensions
{
    public static string? Truncate(this string? value, int maxLength, string truncationSuffix = "...")
    {
        return value?.Length > maxLength
            ? string.Concat(value.AsSpan(0, maxLength), truncationSuffix)
            : value;
    }

    public static int[] GetIds(this string value)
    {
        return value
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(val => int.Parse(val.Trim()))
            .ToArray();
    }

    public static string ToDBString(this IEnumerable<int> ids)
    {
        return string.Join(',', ids);
    }
}

