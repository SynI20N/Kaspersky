using System.Text.RegularExpressions;
using Kasp.Test.Interfaces;

namespace Kasp.Test.Classes
{
    public class ContentReplacementService : IContentReplacementService
    {
        public string ReplacePasswordPattern(string content)
        {
            string passwordPattern = @"password\s*[:=]\s*%(.*?)%";
            return Regex.Replace(content, passwordPattern, "password=***PASSWORD***", RegexOptions.IgnoreCase);
        }

        public string ReplaceLicenseKeyPattern(string content)
        {
            string licenseKeyPattern = @"\b[A-Z0-9]{5}-[A-Z0-9]{5}-[A-Z0-9]{5}-[A-Z0-9]{5}\b";
            return Regex.Replace(content, licenseKeyPattern, "***LICENCE KEY***");
        }

        public string ReplaceForbiddenWords(string content)
        {
            var replacements = new (string forbidden, string legal)[]
            {
            ("master", "primary"),
            ("slave", "secondary"),
            ("whitelist", "allowlist"),
            ("blacklist", "denylist"),
            ("manhours", "personhours"),
            ("grandfathered", "legacy"),
            ("dummy", "stub"),
            ("sanitycheck", "integritycheck"),
            ("masterpassword", "mainpassword")
            };

            foreach (var (forbidden, legal) in replacements)
            {
                content = content.Replace(forbidden, legal, StringComparison.OrdinalIgnoreCase);
            }

            return content;
        }
    }
}
