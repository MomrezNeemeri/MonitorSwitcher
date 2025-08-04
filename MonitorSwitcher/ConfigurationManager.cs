using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonitorSwitcher
{


    // Remove the parentheses after the class name to fix CS8862
    public class ConfigurationManager
    {
        private readonly String _configFilePath;
        public ConfigurationManager(string configFileName = "config.json")
        {

            //store the configuration in local app data
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appSpecificFolder = Path.Combine(appDataFolder, "MonitorSwitcher");
            Directory.CreateDirectory(appSpecificFolder);
            _configFilePath = Path.Combine(appSpecificFolder, configFileName);
            Console.WriteLine($"Configuration file path: {_configFilePath}");
        }

        public async Task SaveConfigAsync(List<string> configData)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string jsonString = JsonSerializer.Serialize(configData, options);
                await File.WriteAllTextAsync(_configFilePath, jsonString);
                Console.WriteLine("Configuration saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving configuration.{ex.Message}");
            }
        }
        public async Task<List<string>> LoadConfigAsync()
        {
            if (!File.Exists(_configFilePath))
            {
                Console.WriteLine("Configuration file not found. Returning empty list.");
                return new List<string>();
            }
            try
            {
                string jsonString = await File.ReadAllTextAsync(_configFilePath);
                List<string> configData = JsonSerializer.Deserialize<List<string>>(jsonString);
                Console.WriteLine("Configuration loaded");
                return configData ?? new List<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}");
                return new List<string>();
            }
        }
    }

}
