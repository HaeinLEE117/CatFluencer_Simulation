using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EducationConfig", menuName = "Config/EducationConfig")]
public class EducationConfig : ScriptableObject
{
    [SerializeField]
    private List<int> stat1EducationCost = new List<int>() { 300, 700, 1500, 3100, 6000 };
    [SerializeField]  
    private List<int> stat1EducationSection = new List<int>() { 30, 60, 100, 120, 200};
    [SerializeField]
    private List<int> stat2EducationCost = new List<int>() { 400, 800, 1600, 3200, 7000 };
    [SerializeField]
    private List<int> stat2EducationSection = new List<int>() { 20, 50, 90, 150, 250};

    public List<int> Stat1EducationCost => stat1EducationCost;
    public List<int> Stat1EducationSection => stat1EducationSection;
    public List<int> Stat2EducationCost => stat2EducationCost;
    public List<int> Stat2EducationSection => stat2EducationSection;

}
