using System.IO;
using UnityEngine;

[System.Serializable]
public class ConfigurationData
{
    public string decimalDelimiter = ",";
    public string logFolder = "Logs";
    public string logExtension = "csv";
    public string bundlesPath = "Bundles\\";
    public string databaseFile = "database.json";


    public override string ToString()
    {
        return JsonUtility.ToJson(this, true);
    }
}

public class Configuration : MonoBehaviour
{
    public static ConfigurationData Data { get { return instance.configData; } }
    private static Configuration instance;
#if UNITY_EDITOR
    [SerializeField]
#endif
    private ConfigurationData configData;

    [SerializeField] private string configFile = "configuration.json";

    private void Awake()
    {
        instance = this;
        configData= LoadConfig();        
    }

    private void OnApplicationQuit()
    {
        SaveConfig(configData);
    }

    private ConfigurationData LoadConfig()
    {
        try
        {
            return JsonReader.ReadAndDeserialize<ConfigurationData>(configFile);            
        }
        catch
        {
            Debug.Log("Could not find configuration file. Using default configuration.");
            return new ConfigurationData();
        }
    }

    private void SaveConfig(ConfigurationData write)
    {
        JsonReader.SerializeAndWrite(configData, configFile);        
    }
}
