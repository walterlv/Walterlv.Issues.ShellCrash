using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.WindowsAPICodePack.Shell;

using Walterlv.Issues.ShellCrash.Utils;

namespace Walterlv.Issues.ShellCrash
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentRendered += OnContentRendered;
        }

        private async void OnContentRendered(object sender, EventArgs e)
        {
            ShowDesktopIcons();
            ShowInjectedDlls();
        }

        private void ShowInjectedDlls()
        {
            foreach (ProcessModule module in Process.GetCurrentProcess().Modules.FindOutNotTrustfulModules())
            {
                DebugListBox.Items.Add(Path.GetFileName(module.FileName));
            }
        }

        private void ShowDesktopIcons()
        {
            var deskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            foreach (var item in new DirectoryInfo(deskPath).EnumerateFileSystemInfos())
            {
                using (var sf = ShellObject.FromParsingName(item.FullName))
                {
                    sf.Thumbnail.FormatOption = ShellThumbnailFormatOption.IconOnly;
                    var bitmapSource = sf.Thumbnail.LargeBitmapSource;
                    bitmapSource?.Freeze();
                    DebugPanel.Children.Add(new Image
                    {
                        Source = bitmapSource,
                        Stretch = Stretch.None,
                    });
                }
            }
        }
    }
}
