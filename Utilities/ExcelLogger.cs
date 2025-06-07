// This file is part of the LiveGPS project.
using System;
using System.IO;
using System.Text;

public static class ExcelLogger
{
    private static readonly string filePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        $"LiveGPSExport_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
    );
    private static readonly object fileLock = new object();
    private static bool fileInitialized = false;

    private static void InitCsvIfNeeded()
    {
        if (fileInitialized) return;
        lock (fileLock)
        {
            if (!fileInitialized)
            {
                if (!File.Exists(filePath))
                {
                    var header = "Timestamp,Latitude,Longitude,Altitude (Baro)";
                    File.WriteAllText(filePath, header + Environment.NewLine, Encoding.UTF8);
                }
                fileInitialized = true;
            }
        }
    }

    public static void Log(double lat, double lng, double altBaro)
    {
        InitCsvIfNeeded();
        lock (fileLock)
        {
            string timestamp = DateTime.UtcNow.ToString("u");
            string line = $"{timestamp},{lat},{lng},{altBaro}";
            File.AppendAllText(filePath, line + Environment.NewLine, Encoding.UTF8);
            
        }
    }
}