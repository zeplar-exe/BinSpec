using System;
using System.IO;

namespace BinSpec.Avalonia.Resources;

public static class Settings
{
    private static string SettingsFilePath => Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "BinSpec/settings");

    public static T? Get<T>(string key)
    {
        // ConfigLoader.TryGet<T>(key, out var value);

        return default;
    }

    public static void Set(string key, object value)
    {
        // ConfigLoader.Container.Add(key, value);
    }

    public static void Load()
    {
        // ConfigLoader.TryLoadFile(SettingsFilePath);
    }

    public static void Write()
    {
        // File.WriteAllText(SettingsFilePath, ConfigLoader.Container.ToString());
    }
}