using System;
using System.IO;
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
            var iniDiag = new VistaOpenFileDialog()
            {
                DefaultExt = "ini",
                Filter = "INI files (*.ini)|*.ini|All files (*.*)|*.*"
            };

            if (iniDiag.ShowDialog() == true)
            {
                if (!iniDiag.FileName.EndsWith(".ini"))
                {
                    MessageBox.Show("This is not a valid .ini configuration! Double check your selection or contact the mod creator.", 
                        "Error", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                    return;
                }
                mod = IniParse.Parse(iniDiag.FileName);
                mod.Directory = Path.Combine(Directory.GetParent(mod.Source).FullName, mod.Directory);
                mod.Image = Path.Combine(Directory.GetParent(mod.Source).FullName, mod.Image);
                if (!Directory.Exists(mod.Directory))
                {
                    MessageBox.Show("This is not a valid .ini configuration! Double check your selection or contact the mod creator.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
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
            if (File.Exists(mod.Image))
                Cover.Source = new BitmapImage(new Uri(mod.Image));
            else
                Cover.Source = new BitmapImage();
        }

        private void InstallMod(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(ModDir.Text) && Directory.Exists(InstallDir.Text))
            {
                Install.IsEnabled = false;
                Log log = new Log();
                log.Owner = this;
                log.ShowDialog();
                MessageBox.Show("The mod has been installed successfully! This application will now close.");
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
