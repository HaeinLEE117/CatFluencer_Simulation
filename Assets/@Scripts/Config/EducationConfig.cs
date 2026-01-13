using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EducationConfig", menuName = "Config/EducationConfig")]
public class EducationConfig : ScriptableObject
{
    [SerializeField]
    private List<int> stat1EducationCosts = new List<int>() { 300, 700, 1500, 3100, 6000 };
    [SerializeField]  
    private List<int> stat1EducationSections = new List<int>() { 30, 60, 100, 120, 200};
    [SerializeField]
    private List<int> stat1EducationDeltaPoints = new List<int>() { 10, 15, 20, 30, 40 };
    [SerializeField]
    private List<int> stat2EducationCosts = new List<int>() { 400, 800, 1600, 3200, 7000 };
    [SerializeField]
    private List<int> stat2EducationSections = new List<int>() { 20, 50, 90, 150, 250};
    [SerializeField]
    private List<int> stat2EducationDeltaPoints = new List<int>() { 5, 10, 15, 25, 35 };

    public List<int> Stat1EducationCosts => stat1EducationCosts;
    public List<int> Stat1EducationSections => stat1EducationSections;
    public List<int> Stat2EducationCosts => stat2EducationCosts;
    public List<int> Stat2EducationSections => stat2EducationSections;
    public List<int> Stat1EducationDeltaPoints => stat1EducationDeltaPoints;
    public List<int> Stat2EducationDeltaPoints => stat2EducationDeltaPoints;

}
