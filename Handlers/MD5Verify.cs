using System;
using System.IO;
using System.Security.Cryptography;

namespace Shell_City_Mod_Loader.Handlers
{
    class MD5Verify
    {
        // todo: make async and output log
        public static Tuple<bool, string> Verify(string path)
        {
            var md5 = MD5.Create();
            foreach (string file in Directory.EnumerateFiles(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "MD5")))
            {
                var compare = File.ReadAllLines(file);
                foreach (string line in compare)
                {
                    string checksum = line.Split(' ')[0];
                    string filePath = line.Replace(checksum, "").Trim();

                    if (filePath == "boot")
                        filePath = Path.Combine(path, "sys", "boot.bin");
                    else
                        filePath = Path.Combine(path, "files", filePath);
                    try
                    {
                        using FileStream read = File.Open(filePath, FileMode.Open);
                        var hash = BitConverter.ToString(md5.ComputeHash(read)).Replace("-", "").ToLower();
                        if (hash != checksum)
                            goto next;
                    }
                    catch
                    {
                        // if code reaches here, it means that it (most likely) couldn't find the file in question, so we should continue to the next md5 file
                        goto next;
                    }

                }
                md5.Dispose();
                return Tuple.Create(true, Path.GetFileNameWithoutExtension(file).Replace(" 2", ""));
            next:;

            }
            md5.Dispose();
            return Tuple.Create(false, "N/A");
        }
    }
}
