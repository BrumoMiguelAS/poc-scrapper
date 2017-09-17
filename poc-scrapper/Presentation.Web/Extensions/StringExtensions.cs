namespace Scrapper.Presentation.Web.Extensions
{
    using System;

    public static class StringExtensions
    {
        public static bool IsValidUrl(this string source)
        {
            Uri uriResult;
            return Uri.TryCreate(source, UriKind.Absolute, out uriResult);
        }
    }
}
