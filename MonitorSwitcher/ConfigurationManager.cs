using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace MonitorSwitcher
{


    public class ConfigurationManager
    {
        private readonly String _configFilePath;
        public ConfigurationManager(string configFileName = "config.json")
        {

            //store the configuration in local app data
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appSpecificFolder = Path.Combine(appDataFolder, "MonitorSwitcher");
            Directory.CreateDirectory(appSpecificFolder);
            this._configFilePath = Path.Combine(appSpecificFolder, configFileName);
            Console.WriteLine($"Configuration file path: {_configFilePath}");
        }

        public async Task SaveConfigAsync(List<ProgramConfig> configData)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string jsonString = JsonSerializer.Serialize(configData, options);
                await File.WriteAllTextAsync(this._configFilePath, jsonString);
                Console.WriteLine("Configuration saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving configuration.{ex.Message}");
            }
        }
        public async Task<List<ProgramConfig>> LoadConfigAsync()
        {
            if (!File.Exists(_configFilePath))
            {
                Console.WriteLine("Configuration file not found. Returning empty list.");
                return new List<ProgramConfig>();
            }
            try
            {
                string jsonString = await File.ReadAllTextAsync(_configFilePath);
                List<ProgramConfig> configData = JsonSerializer.Deserialize<List<ProgramConfig>>(jsonString);
                Console.WriteLine("Configuration loaded");
                return configData ?? new List<ProgramConfig>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}");
                return new List<ProgramConfig>();
            }
        }
    }

}
