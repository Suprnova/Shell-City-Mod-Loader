using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Windows;

namespace Shell_City_Mod_Loader.Tests
{
    class GenerateMD5
    {
        // this doesn't need to be async because it's a test and not something used by the end-user.
        public static void Generate(string path)
        {
            if (!Directory.Exists(Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location, "MD5")))
                Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "MD5"));
            FileStream write = File.Create(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "MD5", Path.GetFileName(path) + ".md5"));
            using (var md5 = MD5.Create())
            {
                using FileStream bootVerify = File.Open(Path.Combine(path, "sys", "boot.bin"), FileMode.Open);
                AddText(write, $"{BitConverter.ToString(md5.ComputeHash(bootVerify)).Replace("-", "").ToLower()} boot");
                path = Path.Combine(path, "files");
                foreach (string file in Directory.EnumerateFiles(path))
                {
                    using FileStream read = File.Open(file, FileMode.Open);
                    var hash = BitConverter.ToString(md5.ComputeHash(read)).Replace("-", "").ToLower();
                    AddText(write, $"\r\n{hash} {Path.GetFileName(file)}");
                }
                foreach (string directory in Directory.EnumerateDirectories(path))
                {
                    foreach (string file in Directory.EnumerateFiles(directory))
                    {
                        using FileStream read = File.Open(file, FileMode.Open);
                        var hash = BitConverter.ToString(md5.ComputeHash(read)).Replace("-", "").ToLower();
                        AddText(write, $"\r\n{hash} {Path.Combine(Path.GetFileName(directory), Path.GetFileName(file))}");
                    }
                }
            }
            MessageBox.Show($"MD5 output saved to {write.Name}.");
        }

        //stolen from microsoft docs dont ask dont tell
        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
    }
}
