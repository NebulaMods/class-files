# class-files
Random class files
Auto updating class file, just add it to your project and reference it like this inside your program.cs.
You can specify a Exe, Rar, Zip, Dll for download types, you can expand the enum list as well.

static void Main()
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    AutoUpdater.Update.UPDATE("https://autoupdater.com/version", "https://autoupdater.com/version.exe", AutoUpdater.Update.EnumFileType.Exe);    
    Application.Run(new Developer());
}
