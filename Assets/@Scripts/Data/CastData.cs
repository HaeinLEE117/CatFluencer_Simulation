using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CastData
{
    public int TemplateID;
    public int Stat1;
    public int Stat2;
    public int Stat3;
    public int CastPay;
    public string NameTextID;
    public string PhotoImageID;
}

[Serializable]
public class CastDataLoader : IDataLoader<int, CastData>
{
    public List<CastData> texts = new List<CastData>();

    public Dictionary<int, CastData> MakeDict()
    {
        Dictionary<int, CastData> dict = new Dictionary<int, CastData>();
        foreach (var text in texts)
            dict.Add(text.TemplateID, text);

        return dict;
    }

    public bool Validate()
    {
        return true;
    }
}