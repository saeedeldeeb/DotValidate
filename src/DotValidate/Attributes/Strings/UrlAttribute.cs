namespace DotValidate.Attributes.Strings;

/// <summary>
/// Specifies that a string property must be a valid URL (http or https).
/// </summary>
public sealed class UrlAttribute : ValidationAttribute
{
    protected override string DefaultErrorMessage => "{0} must be a valid URL.";

    public override bool IsValid(object? value)
    {
        if (value is null) return true;
        if (value is not string str) return false;
        if (string.IsNullOrWhiteSpace(str)) return true;

        return Uri.TryCreate(str, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
