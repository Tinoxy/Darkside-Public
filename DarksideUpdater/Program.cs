using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;

[DllImport("kernel32.dll")]
static extern IntPtr GetConsoleWindow();

[DllImport("user32.dll")]
static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

const int SW_HIDE = 0;
const int SW_SHOW = 5;

var path = Path.Combine(Directory.GetCurrentDirectory(), "DS.zip");
var logPath = Path.Combine(Directory.GetCurrentDirectory(), "update.log");
var url = new Uri("http://syssc.karmann-photo.de/darkside/app/Darkside.zip");
var handle = GetConsoleWindow();

ShowWindow(handle, SW_HIDE);

try
{
    using HttpClient client = new();

    Console.WriteLine("Updater Started...");

    var bytesFromServer = 0L;
    var bytesLocal = 0L;
    var startTime = DateTime.Now;

    var stream = await client.GetStreamAsync(url);

    using (var serverFileStream = File.Create(path))
    {
        stream.CopyTo(serverFileStream);
    }

    bytesFromServer = File.ReadAllBytes(path).Length;

    Console.WriteLine("Zip created...");

    using var fileStream = File.Open(path, FileMode.OpenOrCreate);
    using ZipArchive zip = new(fileStream);

    bytesLocal = fileStream.Length;
    foreach (var entry in zip.Entries)
    {
        if (entry.Name.Contains("DarksideUpdater"))
        {
            continue;
        }
        entry.ExtractToFile(Path.Combine(Directory.GetCurrentDirectory(), entry.Name), true);
    }

    fileStream.Close();

    Console.WriteLine("Updated contents...");

    File.Delete(path);

    File.WriteAllText(logPath, "#Updater Log Begin=" + startTime.ToLongTimeString() + "\n"
                                                                                    + "#Bytes downloaded=" + bytesFromServer + "\n"
                                                                                    + "#Bytes unpacked=" + bytesLocal + "\n"
                                                                                    + "#Total Update Duration=" + (DateTime.Now - startTime).TotalSeconds + "Seconds\n"
                                                                                    + "#Updater Log Finish=" + DateTime.Now.ToLongTimeString());

    Process.Start(Path.Combine(Directory.GetCurrentDirectory(), "Darkside.exe"));
    Process.GetCurrentProcess().Kill();
}
catch (Exception ex)
{
    File.WriteAllText(logPath, "#Updater Log error=" + ex);
    Console.ReadKey();
}