using System;
using System.Configuration;
using LockPoc.Data.Core;

namespace LockPoc.Core
{
    public class AppSettingsConfigurationProvider: IConfigurationProvider
    {
        T IConfigurationProvider.GetValue<T>(string name, bool required, T defaultValue)
            => GetValue(name, required, defaultValue);

        string IConfigurationProvider.GetString(string name, bool required, string defaultValue)
            => GetString(name, required, defaultValue);

        string IConfigurationProvider.GetConnectionString(string name)
            => GetConnectionString(name);
        
        protected virtual T GetValue<T>(string name, bool required = true, T defaultValue = default) where T : struct, IConvertible
        {
            if (!ConfigurationManager.AppSettings.HasKeys() || ConfigurationManager.AppSettings.Get(name) == null)
                throw new ArgumentOutOfRangeException(nameof(name), $"{name} is not found in {nameof(ConfigurationManager.AppSettings)}.");
            
            var stringValue = ConfigurationManager.AppSettings[name];
        
            if (stringValue.Length == 0)
            {
                if(required)
                    throw new ArgumentException(nameof(name), $"{name} has no value and is required.");
                
                return defaultValue;
            }
        
            // Convert.ChangeType for booleans works better with their string representation
            if (typeof(T) != typeof(bool))
                return (T)Convert.ChangeType(stringValue, typeof(T));
            
            if (bool.TryParse(stringValue, out var parsedBooleanValue))
                stringValue = parsedBooleanValue.ToString();
            else
                stringValue = stringValue.Trim() == "1" ? "true" : defaultValue.ToString();
        
            return (T)Convert.ChangeType(stringValue, typeof(T));
        }
        
        protected virtual string GetString(string name, bool required = true, string defaultValue = default)
        {
            if (!ConfigurationManager.AppSettings.HasKeys() || ConfigurationManager.AppSettings.Get(name) == null)
                throw new ArgumentOutOfRangeException(nameof(name), $"{name} is not found in {nameof(ConfigurationManager.AppSettings)}.");
            
            var stringValue = ConfigurationManager.AppSettings[name];
        
            if (stringValue.Length == 0)
            {
                if(required)
                    throw new ArgumentException(nameof(name), $"{name} has no value and is required.");
                
                return defaultValue;
            }
        
            return stringValue;
        }
        
        protected virtual string GetConnectionString(string name)
        {
            var settings = ConfigurationManager.ConnectionStrings[name];
        
            if (settings != null)
                return settings.ConnectionString;
            
            throw new ArgumentOutOfRangeException(nameof(name), $"ConnectionString with name: {name} is not found in {nameof(ConfigurationManager.AppSettings)}.");
        }
    }
}