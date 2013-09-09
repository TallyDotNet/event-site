using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Raven.Client;

namespace CodeCamp.Domain.Infrastructure {
    public static class SlugConverter {
        public static string ToUrlSafeFile(string fileName) {
            fileName = RemoveDiacritics(fileName);
            fileName = ReplaceNonWordWithDashes(fileName, true);
            return fileName.Trim(' ', '-');
        }

        public static string UniquifySlug<T>(IDocumentSession session, string slug, Func<string, string> getId)
            where T : class {
            var id = getId(slug);
            var exisiting = session.Load<T>(id);

            var counter = 2;
            var originalSlug = slug;
            while(exisiting != null) {
                slug = originalSlug + "-" + counter;
                id = getId(slug);
                exisiting = session.Load<T>(id);
                counter++;
            }

            return slug;
        }

        public static string ToSlug(string title) {
            // 2 - Strip diacritical marks using Michael Kaplan's function or equivalent
            title = RemoveDiacritics(title);

            // 3 - Lowercase the string for canonicalization
            title = title.ToLowerInvariant();

            // 4 - Replace all the non-word characters with dashes
            title = ReplaceNonWordWithDashes(title);

            // 1 - Trim the string of leading/trailing whitespace
            title = title.Trim(' ', '-');

            return title;
        }

        public static string ToCaseSensitiveSlug(string title) {
            title = RemoveDiacritics(title);
            title = ReplaceNonWordWithDashes(title);
            return title.Trim(' ', '-');
        }

        // http://blogs.msdn.com/michkap/archive/2007/05/14/2629747.aspx
        /// <summary>
        ///   Strips the value from any non English character by replacing those with their English equivalent.
        /// </summary>
        /// <param name = "value">The string to normalize.</param>
        /// <returns>A string where all characters are part of the basic English ANSI encoding.</returns>
        /// <seealso cref = "http://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net" />
        static string RemoveDiacritics(string value) {
            var stFormD = value.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            for(var ich = 0; ich < stFormD.Length; ich++) {
                var uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if(uc != UnicodeCategory.NonSpacingMark) {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        static string ReplaceNonWordWithDashes(string title, bool doNotReplacePeriod = false) {
            // Remove Apostrophe Tags
            title = Regex.Replace(title, "[’'“”\"&]{1,}", "", RegexOptions.None);

            // Replaces all non-alphanumeric character by a space
            var builder = new StringBuilder();

            for(var i = 0; i < title.Length; i++) {
                var current = title[i];

                if(doNotReplacePeriod && current == '.') {
                    builder.Append(current);
                } else {
                    builder.Append(Char.IsLetterOrDigit(current) ? current : ' ');
                }
            }

            title = builder.ToString();

            // Replace multiple spaces into a single dash
            title = Regex.Replace(title, "[ ]{1,}", "-", RegexOptions.None);

            return title;
        }
    }
}