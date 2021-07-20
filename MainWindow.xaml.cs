using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

using Ookii.Dialogs.Wpf;

using Shell_City_Mod_Loader.Classes;
using Shell_City_Mod_Loader.Handlers;

namespace Shell_City_Mod_Loader
{
    public partial class MainWindow : Window
    {
        ModIni mod = new ModIni();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenIni(object sender, RoutedEventArgs e)
        {
            var iniDiag = new VistaOpenFileDialog();
            if (iniDiag.ShowDialog() == true)
            {
                if (!iniDiag.FileName.EndsWith(".ini"))
                {
                    MessageBox.Show("This is not a valid .ini configuration! Double check your selection or contact the mod creator.", "Error!");
                    return;
                }
                mod = IniParse.Parse(iniDiag.FileName);
                mod.Directory = Path.Combine(Directory.GetParent(mod.Source).FullName, mod.Directory);
                mod.Image = Path.Combine(Directory.GetParent(mod.Source).FullName, mod.Image);
                if (!Directory.Exists(mod.Directory) || !File.Exists(mod.Image))
                {
                    MessageBox.Show("This is not a valid .ini configuration! Double check your selection or contact the mod creator.", "Error!");
                    return;
                }
                Update(mod);
            }
        }

        private void Update(ModIni mod)
        {
            Name.Text = mod.Name;
            Game.Text = mod.Game;
            Description.Text = mod.Description;
            Authors.Text = mod.Authors;
            ModDir.Text = mod.Directory;
            Cover.Source = new BitmapImage(new Uri(mod.Image));
        }

        private void InstallMod(object sender, RoutedEventArgs e)
        {
            Install.IsEnabled = false;
            InstallTask(ModDir.Text, Path.Combine(InstallDir.Text, "files"));
        }

        public async Task InstallTask(string src, string dest)
        {
            // i hate c# i/o operations so much
            if (Directory.Exists(src) && Directory.Exists(InstallDir.Text))
            {
                if (!Directory.Exists(dest))
                    Directory.CreateDirectory(dest);
                foreach (string file in Directory.EnumerateFiles(src))
                {
                    using FileStream source = File.Open(file, FileMode.Open);
                    using FileStream destination = File.Create(Path.Combine(dest, Path.GetFileName(file)));
                    await source.CopyToAsync(destination);
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
                    }
                }
                MessageBox.Show($"{mod.Name} was installed correctly! This application will now close.");
                Environment.Exit(1);
            }
            else
            {
                MessageBox.Show("Your directories are invalid and the mod cannot currently be installed. Please fix the issue and try again.", "Error!");
            }
        }

        private void BrowseFolder(object sender, RoutedEventArgs e)
        {
            var folderDiag = new VistaFolderBrowserDialog();
            if (folderDiag.ShowDialog() == true)
            {
                if (!Directory.Exists(Path.Combine(folderDiag.SelectedPath, "files")) || !Directory.Exists(Path.Combine(folderDiag.SelectedPath, "sys")))
                {
                    MessageBoxResult result = MessageBox.Show($"This doesn't look like a valid extraction of a {(string.IsNullOrEmpty(mod.Game) ? "Heavy Iron GameCube" : mod.Game)} disc! Are you sure you want to continue?", "Error!", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                        return;
                }
                InstallDir.Text = folderDiag.SelectedPath;
            }
        }
    }
}
