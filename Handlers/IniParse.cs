using Shell_City_Mod_Loader.Classes;

using System.Windows;

using IniParser;
using IniParser.Model;

namespace Shell_City_Mod_Loader.Handlers
{
    class IniParse
    {
        public static ModIni Parse(string path)
        {
            var parser = new FileIniDataParser();
            var ini = new ModIni();
            try
            {
                IniData modData = parser.ReadFile(path);
                ini.Source = path;
                ini.Game = modData["Installer"]["Game"];
                ini.Name = modData["Installer"]["Name"];
                ini.Authors = modData["Installer"]["Authors"];
                ini.Description = modData["Installer"]["Description"];
                ini.Directory = modData["Installer"]["Directory"];
                ini.Image = modData["Installer"]["Image"];
            }
            catch
            {
                MessageBox.Show("This is not a valid .ini configuration! Double check your selection or contact the mod creator.", "Error!");
                return null;
            }

            return ini;
        }
    }
}

