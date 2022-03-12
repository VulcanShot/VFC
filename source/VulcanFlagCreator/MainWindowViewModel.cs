using System;
using System.IO;
using System.Net.Http;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using VulcanFlagCreator.Extensions;
using VulcanFlagCreator.FlagTemplates;

namespace VulcanFlagCreator
{
    public class MainWindowViewModel : ObservableClass
    {
        public MainWindowViewModel()
        {
            CreateFlagCommand = new CommandHandler(
                delegate ()
                {
                    switch (SelectedGameTag)
                    {
                        case "hoi4":
                            new Hoi4Flag(SourceFlagPath, _httpClient, OutputFolder, FileName, Suffix).CreateFlag();
                            break;
                        default:
                            new GenericFlag(SourceFlagPath, _httpClient, OutputFolder, FileName, Suffix).CreateFlag();
                            break;
                    }
                    System.Diagnostics.Process.Start("explorer.exe", OutputFolder);
                },
                () => HasCompletedFillingInformation()
            );

            PreviewFlagCommand = new CommandHandler(
                delegate ()
                {
                    switch (SelectedGameTag)
                    {
                        case "hoi4":
                            var bitmapSources = new Hoi4Flag(SourceFlagPath, _httpClient).GetFlagBitmapSources();
                            LargeFlag = bitmapSources[FlagSize.Large];
                            MediumFlag = bitmapSources[FlagSize.Medium];
                            SmallFlag = bitmapSources[FlagSize.Small];
                            break;
                        default:
                            LargeFlag = new GenericFlag(SourceFlagPath, _httpClient).GetFlagBitmapSources()[FlagSize.Large];
                            break;
                    }
                },
                () => SourceFlagPathExists()
            );
        }

        private BitmapSource largeFlag;
        private BitmapSource mediumFlag;
        private BitmapSource smallFlag;
        private static readonly HttpClient _httpClient = new();

        public string SelectedGameTag { get; set; }
        public string SourceFlagPath { get; set; }
        public string OutputFolder { get; set; }
        public string FileName { get; set; }
        public string Suffix { get; set; }
        public BitmapSource LargeFlag
        {
            get => largeFlag;
            set { largeFlag = value; OnPropertyChanged(nameof(LargeFlag)); }
        }
        public BitmapSource MediumFlag
        {
            get => mediumFlag;
            set { mediumFlag = value; OnPropertyChanged(nameof(MediumFlag)); }
        }
        public BitmapSource SmallFlag
        {
            get => smallFlag;
            set { smallFlag = value; OnPropertyChanged(nameof(SmallFlag)); }
        }
        public ICommand CreateFlagCommand { get; private set; }
        public ICommand PreviewFlagCommand { get; private set; }

        private bool HasCompletedFillingInformation()
        {
            return !string.IsNullOrEmpty(SelectedGameTag) &&
                   !string.IsNullOrEmpty(FileName) &&
                   Directory.Exists(OutputFolder) &&
                   SourceFlagPathExists();
        }

        private bool SourceFlagPathExists()
        {
            if (File.Exists(SourceFlagPath))
                return true;

            if (Uri.IsWellFormedUriString(SourceFlagPath, UriKind.Absolute))
            {
                HttpRequestMessage request = new(HttpMethod.Get, new Uri(SourceFlagPath));
                var response = _httpClient.Send(request);

                if (response.IsSuccessStatusCode && response.Content.Headers.ContentType.MediaType.CaseInsensitiveContains("Image"))
                    return true;
            }

            return false;
        }
    }
}
