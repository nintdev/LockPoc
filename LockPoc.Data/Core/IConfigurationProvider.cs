using System;

namespace LockPoc.Data.Core
{
    public interface IConfigurationProvider
    {
        T GetValue<T>(string name,
            bool required = true,
            T defaultValue = default) where T : struct, IConvertible;
        
        string GetString(string name,
            bool required = true,
            string defaultValue = default);

        string GetConnectionString(string name);
    }
}