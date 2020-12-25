using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace AutoUpdater
{
    class Update
    {
        //Usage: AutoUpdater.Update.UPDATE("https://autoupdater.com/version", "https://autoupdater.com/version.exe", AutoUpdater.Update.EnumFileType.Exe);
        //Created By Nebula: https://nebulamods.ca : Give Credit Where It's Due.

        public static void UPDATE(string VersionLink, string DownloadLink, EnumFileType Type)
        {
            string ApplicationName = Process.GetCurrentProcess().ProcessName;//Grabs the applications name by process
            try
            {
                if (string.IsNullOrEmpty(VersionLink) || string.IsNullOrEmpty(DownloadLink))
                    goto Error;//Wanted to try a goto
                string TypeOfFile = FileType(Type);
                WebClient Client = new WebClient() { Proxy = null };
                if (VersionLink.Contains(Application.ProductVersion) || (VersionLink == Application.ProductVersion))
                    Console.WriteLine($"{ApplicationName} is on the latest version.");
                else
                {
                    if (TypeOfFile == ".exe" || TypeOfFile == ".dll")
                    {
                        Directory.CreateDirectory($@"{Application.StartupPath}\{ApplicationName}");
                        Client.DownloadFile(DownloadLink, $@"{Application.StartupPath}\{ApplicationName}\{ApplicationName}{TypeOfFile}");
                    }
                    else
                        Client.DownloadFile(DownloadLink, $@"{Application.StartupPath}\{ApplicationName}{TypeOfFile}");
                    //File done downloading
                    Updater(ApplicationName, Type);
                }
                Error: MessageBox.Show($"The developer of this application did not define the one of the update paths.", ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                //Error Updating Display Message
                MessageBox.Show($"There was an error updating {ApplicationName}, please try again.", ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);//Default MessageBox
                Process.GetCurrentProcess().Kill();
            }
        }
        private static void Updater(string ApplicationName, EnumFileType Type)
        {
            string CurrentFolder = Environment.CurrentDirectory;
            string UpdatedFolder = $@"{Environment.CurrentDirectory}\{ApplicationName}";
            string MainArgs = $"/C choice /C Y /N /D Y /T 5 & Del \"{Application.ExecutablePath}\" & move \"{UpdatedFolder}\\{ApplicationName}.{FileType(Type)}\" \"{CurrentFolder}\\{ApplicationName}.{FileType(Type)}\" & rmdir \"{UpdatedFolder}\"";
            string ExeArgs = $" & start \"{ApplicationName}\" \"{CurrentFolder}\\{ApplicationName}.{FileType(Type)}\"";
            if (Type == EnumFileType.Exe)
                MainArgs += ExeArgs;
            Process.Start(new ProcessStartInfo
            {
                CreateNoWindow = false,
                WorkingDirectory = Environment.CurrentDirectory,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = MainArgs
            });
            Process.GetCurrentProcess().Kill();
        }

        #region FileType

        public enum EnumFileType : int
        {
            Exe = 0,
            Rar = 1,
            Zip = 2,
            Dll = 3
        }
        private static string FileType(EnumFileType Type)
        {
            switch (Type)
            {
                case EnumFileType.Exe:
                    return ".exe";
                case EnumFileType.Rar:
                    return ".rar";
                case EnumFileType.Zip:
                    return ".zip";
                case EnumFileType.Dll:
                    return ".dll";
                default:
                    return null;
            }
        }

        #endregion
    }
}
