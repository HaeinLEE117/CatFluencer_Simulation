using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class LocationData
{
    public int LocationID;
    public int Coast;
    public string LocationTextID;
    public int Popularity;
}

[Serializable]
public class LocationDataLoader : IDataLoader<int, LocationData>
{
    public List<LocationData> texts = new List<LocationData>();

    public Dictionary<int, LocationData> MakeDict()
    {
        Dictionary<int, LocationData> dict = new Dictionary<int, LocationData>();
        foreach (var text in texts)
            dict.Add(text.LocationID, text);

        return dict;
    }

    public bool Validate()
    {
        return true;
    }
}