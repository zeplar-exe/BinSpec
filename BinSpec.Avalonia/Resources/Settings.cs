using System;
using System.IO;

using SimpleConfig;

namespace BinSpec.Avalonia.Resources;

public static class Settings
{
    private static string SettingsFilePath => Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "BinSpec/settings");
    
    private static FileConfigLoader ConfigLoader { get; } = new();

    public static T? Get<T>(string key)
    {
        ConfigLoader.TryGet<T>(key, out var value);

        return value;
    }

    public static void Set(string key, object value)
    {
        ConfigLoader.Container.Add(key, value);
    }

    public static void Load()
    {
        ConfigLoader.TryLoadFile(SettingsFilePath);
    }

    public static void Write()
    {
        File.WriteAllText(SettingsFilePath, ConfigLoader.Container.ToString());
    }
}