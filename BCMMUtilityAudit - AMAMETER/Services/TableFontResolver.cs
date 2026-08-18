using PdfSharpCore.Fonts;
using System.IO;
using System.Reflection;

namespace BCMMUtilityAudit___AMAMETER.Services
{
    public class TableFontResolver : IFontResolver
    {
        // Add this line to satisfy the IFontResolver interface
        public string DefaultFontName => "OpenSans_Regular";

        public byte[] GetFont(string faceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = "BCMMUtilityAudit___AMAMETER.Resources.Fonts.OpenSans_Regular.ttf";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new FileNotFoundException($"Could not find embedded font resource: {resourceName}");

                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            return new FontResolverInfo("OpenSans_Regular");
        }
    }
}