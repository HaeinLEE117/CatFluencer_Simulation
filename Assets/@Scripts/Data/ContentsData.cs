using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ContentsData
{
    public int ContentsID;
    public string ContentsTextID;
    public int Popularity;
}

[Serializable]
public class ContentsDataLoader : IDataLoader<int, ContentsData>
{
    public List<ContentsData> texts = new List<ContentsData>();

    public Dictionary<int, ContentsData> MakeDict()
    {
        Dictionary<int, ContentsData> dict = new Dictionary<int, ContentsData>();
        foreach (var text in texts)
            dict.Add(text.ContentsID, text);

        return dict;
    }

    public bool Validate()
    {
        return true;
    }
}