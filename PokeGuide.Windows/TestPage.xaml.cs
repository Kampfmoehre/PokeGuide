using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using PokeGuide.Model;
using PokeGuide.ViewModel;
using PokeGuide.ViewModel.Interface;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PokeGuide
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class TestPage : Page
    {
        TestViewModel _dataContext;

        public TestPage()
        {
            this.InitializeComponent();
            DataContextChanged += TestPage_DataContextChanged;
        }

        void TestPage_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            TestViewModel vm = DataContext as TestViewModel;
            _dataContext = vm;
            if (vm == null)
                vm.PropertyChanged -= Vm_PropertyChanged;
            else
                vm.PropertyChanged += Vm_PropertyChanged;
        }

        void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedAbility" && _dataContext.SelectedAbility != null)
            {
                //SetAbilityText(txtShortEffect, _dataContext.SelectedAbility.Description);
                //SetAbilityText(txtEffect, _dataContext.SelectedAbility.Effect);
                //SetAbilityText(txtEffectChange, _dataContext.SelectedAbility.EffectChange);
            }

        }

        void SetAbilityText(TextBlock textBlock, string fullText)
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

                Hyperlink hyperLink = new Hyperlink()
                {
                    NavigateUri = new Uri(trimmedIdentifier)
                };
                hyperLink.Inlines.Add(new Run { Text = trimmedText } );
                textBlock.Inlines.Add(hyperLink);
                textIndex = identifierEnd;
            }

            string lastContent = fullText.Substring(textIndex, fullText.Length - textIndex);
            textBlock.Inlines.Add(new Run { Text = lastContent });
        }
    }
}
