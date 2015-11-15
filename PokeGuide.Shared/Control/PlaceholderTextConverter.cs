using System;
using System.Text.RegularExpressions;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace PokeGuide.Control
{
    static class PlaceholderTextConverter
    {
        internal static void ConvertTextWithPlaceholders(TextBlock textBlock, string fullText)
        {
            textBlock.Inlines.Clear();

            if (String.IsNullOrWhiteSpace(fullText))
                return;

            string nameRegexString = @"(?<text>\[[\w\s]*\])(?<identifier>{\w*:\w*-*\w*})";
            var regex = new Regex(nameRegexString);
            int textIndex = 0;

            foreach (Match match in regex.Matches(fullText))
            {
                string identifier = match.Groups["identifier"].Value;
                int identifierStart = match.Groups["identifier"].Index;
                int identifierEnd = match.Groups["identifier"].Index + identifier.Length;

                string text = match.Groups["text"].Value;
                int text_start = match.Groups["text"].Index;
                int text_end = match.Groups["text"].Index + text.Length;

                string trimmedText = text.Trim(new char[] { '[', ']' });
                string trimmedIdentifier = identifier.Trim(new char[] { '{', '}' });

                string content = fullText.Substring(textIndex, text_start - textIndex);
                textBlock.Inlines.Add(new Run { Text = content });

                var hyperLink = new Hyperlink()
                {
                    NavigateUri = new Uri(trimmedIdentifier)
                };
                hyperLink.Inlines.Add(new Run { Text = trimmedText });
                textBlock.Inlines.Add(hyperLink);
                textIndex = identifierEnd;
            }

            string lastContent = fullText.Substring(textIndex, fullText.Length - textIndex);
            textBlock.Inlines.Add(new Run { Text = lastContent });
        }
    }
}