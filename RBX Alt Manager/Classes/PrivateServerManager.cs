using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RBX_Alt_Manager.Classes
{
    public class PrivateServerManager
    {
        private static PrivateServerManager Instance;
        private List<PrivateServer> servers;
        private readonly string configPath;
        private readonly string configFileName = "private_servers.json";

        private PrivateServerManager()
        {
            configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RobloxAccountManager");
            servers = new List<PrivateServer>();
            LoadServers();
        }

        public static PrivateServerManager GetInstance()
        {
            if (Instance == null)
                Instance = new PrivateServerManager();
            return Instance;
        }

        public List<PrivateServer> GetServers()
        {
            return new List<PrivateServer>(servers);
        }

        public void AddServer(PrivateServer server)
        {
            servers.Add(server);
            SaveServers();
        }

        public void UpdateServer(int index, PrivateServer server)
        {
            if (index >= 0 && index < servers.Count)
            {
                servers[index] = server;
                SaveServers();
            }
        }

        public void DeleteServer(int index)
        {
            if (index >= 0 && index < servers.Count)
            {
                servers.RemoveAt(index);
                SaveServers();
            }
        }

        public bool ValidateJobId(string jobId)
        {
            // Accept both GUID format and numeric Roblox private server link codes
            if (string.IsNullOrWhiteSpace(jobId))
                return false;
            
            // Check if it's a valid GUID
            if (Guid.TryParse(jobId, out _))
                return true;
            
            // Check if it's a numeric private server link code (long numeric string)
            return long.TryParse(jobId, out _);
        }

        private void LoadServers()
        {
            try
            {
                if (!Directory.Exists(configPath))
                    Directory.CreateDirectory(configPath);

                string filePath = Path.Combine(configPath, configFileName);
                if (!File.Exists(filePath))
                {
                    servers = new List<PrivateServer>();
                    return;
                }

                string json = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(json))
                {
                    servers = new List<PrivateServer>();
                    return;
                }

                JObject obj = JObject.Parse(json);
                servers = obj["servers"]?.ToObject<List<PrivateServer>>() ?? new List<PrivateServer>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading private servers: {ex.Message}");
                servers = new List<PrivateServer>();
            }
        }

        private void SaveServers()
        {
            try
            {
                if (!Directory.Exists(configPath))
                    Directory.CreateDirectory(configPath);

                string filePath = Path.Combine(configPath, configFileName);
                JObject obj = new JObject();
                obj["servers"] = JArray.FromObject(servers);
                string json = obj.ToString(Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving private servers: {ex.Message}");
            }
        }
    }
}
