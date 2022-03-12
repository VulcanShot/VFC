using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Input;
using Microsoft.Win32;

namespace VulcanFlagCreator
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        private void GameIcons_Click(object sender, MouseButtonEventArgs args)
        {
            Image selectedGame = sender as Image;
            GameIconsStackPanel.Children.Cast<Image>().ToList().ForEach(x => x.Opacity = 0.5f);
            selectedGame.Opacity = 1f;
            (DataContext as MainWindowViewModel).SelectedGameTag = selectedGame.Tag as string;
        }

        private void OnClickSetSourcePath(object sender, MouseButtonEventArgs args)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Set Source Path",
                Filter = "Supported Image Types|*.jpg;*.png;*.tiff;*.bmp;*.gif;*.exif"
            };

            if ((bool)dialog.ShowDialog())
            {
                SourcePathTextBox.Text = dialog.FileName;
            }
        }

        private void OnClickSetOutputFolder(object sender, MouseButtonEventArgs args)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                EnsureFileExists = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                OutputFolderTextBox.Text = dialog.FileName;
            }
        }
    }
}
