using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public interface IValidate
{
    bool Validate();
}

public interface IDataLoader<Key, Value> : IValidate
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager : Singleton<DataManager>
{
    private HashSet<IValidate> _loaders = new HashSet<IValidate>();

    public GameConfig GameConfig { get; private set; }
    public LocalizationConfig LocalizationConfig { get; private set; }
    public AdsConfig AdsConfig { get; private set; }
    public IAPConfig IAPConfig { get; private set; }
    public EducationConfig EducationConfig { get; private set; }
    public VideoLocationsConfig VideoLocationsConfig { get; private set; }

    public Dictionary<string, TextData> TextDict { get; private set; } = new Dictionary<string, TextData>();
    public Dictionary<int, EmployeeData> EmployeeDict { get; private set; } = new Dictionary<int, EmployeeData>();
    public Dictionary<int, ContentsData> ContentsDict { get; private set; } = new Dictionary<int, ContentsData>();
    public Dictionary<int, LocationData> LocationDict { get; private set; } = new Dictionary<int, LocationData>();
    public Dictionary<int, CastData> CastDict { get; private set; } = new Dictionary<int, CastData>();  

    public void LoadData()
    {
        GameConfig = LoadScriptableObject<GameConfig>("GameConfig");
        LocalizationConfig = LoadScriptableObject<LocalizationConfig>("LocalizationConfig");
        AdsConfig = LoadScriptableObject<AdsConfig>("AdsConfig");
        IAPConfig = LoadScriptableObject<IAPConfig>("IAPConfig");
        EducationConfig = LoadScriptableObject<EducationConfig>("EducationConfig");
        VideoLocationsConfig = LoadScriptableObject<VideoLocationsConfig>("VideoLocationConfig");

        TextDict = LoadJson<TextDataLoader, string, TextData>("TextData").MakeDict();
        EmployeeDict = LoadJson<EmployeeDataLoader, int, EmployeeData>("EmployeeData").MakeDict();
        ContentsDict = LoadJson<ContentsDataLoader, int, ContentsData>("ContentsData").MakeDict();
        LocationDict = LoadJson<LocationDataLoader, int, LocationData>("LocationData").MakeDict();
        CastDict = LoadJson<CastDataLoader, int, CastData>("CastData").MakeDict();

        Validate();
    }

    private T LoadScriptableObject<T>(string path) where T : ScriptableObject
    {
        T asset = ResourceManager.Instance.Get<T>(path);
        if (asset == null)
            Debug.LogError($"Failed to load ScriptableObject at path: {path}");

        return asset;
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : IDataLoader<Key, Value>
    {
        TextAsset textAsset = ResourceManager.Instance.Get<TextAsset>(path);

        Loader loader = JsonConvert.DeserializeObject<Loader>(textAsset.text);
        _loaders.Add(loader);
        Debug.Log(path);

        return loader;
    }

    private bool Validate()
    {
        bool success = true;

        foreach (IValidate loader in _loaders)
        {
            if (loader.Validate() == false)
                success = false;
        }

        _loaders.Clear();

        return success;
    }

    public string GetTextIDwithIntKey(int key)
    {
        string textID = "NO_DATA";
        if (ContentsDict.TryGetValue(key, out ContentsData data))
        {
            textID = data.ContentsTextID;
        }
        else if(LocationDict.TryGetValue(key, out LocationData locData))
        {
            textID = locData.LocationTextID;
        }
        else if(CastDict.TryGetValue(key, out CastData castData))
        {
            textID = castData.NameTextID;
        }
        else if(EmployeeDict.TryGetValue(key, out EmployeeData empData))
        {
            textID = empData.NameTextID;
        }

            return textID;
    }
}
