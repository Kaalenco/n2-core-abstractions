using System.IO.Abstractions;

namespace N2.Core;

public interface ISettingsService
{
    IDirectoryInfo? DirectoryRoot { get; }
    string SettingsFileName { get; set; }
    TConfig GetConfigSettings<TConfig>(string sectionName) where TConfig : class, new();
    TConfig GetConfigSettings<TConfig>() where TConfig : class, new();
    string GetConnectionString(string name);
    TValue GetSetting<TValue>(string name, TValue defaultValue) where TValue : struct;
    void Reload<T>() where T : class;
}
