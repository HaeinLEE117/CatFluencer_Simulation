using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class EmployeeData
{
    public int TemplateID;
    public int Stat1;
    public int Stat2;
    public int Stat3;
    public int Salary;
    public int ContactFee;
    public string NameTextID;
    public string CharMentTextID;
    public string PhotoImageID;
    public string PrefabNameID;
}

[Serializable]
public class EmployeeDataLoader : IDataLoader<int, EmployeeData>
{
    public List<EmployeeData> texts = new List<EmployeeData>();

    public Dictionary<int, EmployeeData> MakeDict()
    {
        Dictionary<int, EmployeeData> dict = new Dictionary<int, EmployeeData>();
        foreach (var text in texts)
            dict.Add(text.TemplateID, text);

        return dict;
    }

    public bool Validate()
    {
        return true;
    }
}