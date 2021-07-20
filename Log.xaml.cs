using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Shell_City_Mod_Loader
{
    public partial class Log : Window
    {
        public Log()
        {
            InitializeComponent();
            InstallTask(((MainWindow)Application.Current.MainWindow).ModDir.Text, Path.Combine(((MainWindow)Application.Current.MainWindow).InstallDir.Text, "files"));
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!Finish.IsEnabled)
            {
                base.OnClosing(e);
                e.Cancel = true;
            }
        }

        public async Task InstallTask(string src, string dest)
        {
            // i hate c# i/o operations so much
            if (!Directory.Exists(dest))
                Directory.CreateDirectory(dest);
            foreach (string file in Directory.EnumerateFiles(src))
            {
                using FileStream source = File.Open(file, FileMode.Open);
                using FileStream destination = File.Create(Path.Combine(dest, Path.GetFileName(file)));
                await source.CopyToAsync(destination);
                LogBox.Inlines.Add($"{Environment.NewLine}Copied {source.Name} to {destination.Name}.");
                logScroll.ScrollToEnd();
            }
            foreach (string directory in Directory.EnumerateDirectories(src))
            {
                string destDir = Path.Combine(dest, Path.GetFileName(directory));
                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);
                foreach (string file in Directory.EnumerateFiles(directory))
                {
                    using FileStream source = File.Open(file, FileMode.Open);
                    using FileStream destination = File.Create(Path.Combine(destDir, Path.GetFileName(file)));
                    await source.CopyToAsync(destination);
                    LogBox.Inlines.Add($"{Environment.NewLine}Copied {source.Name} to {destination.Name}.");
                    logScroll.ScrollToEnd();
                }
            }
            LogBox.Inlines.Add($"{Environment.NewLine}{((MainWindow)Application.Current.MainWindow).ModName.Content} successfully installed!");
            logScroll.ScrollToEnd();
            Finish.IsEnabled = true;
        }
    }
}
