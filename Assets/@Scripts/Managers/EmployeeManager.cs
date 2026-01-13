using System.Collections.Generic;
using UnityEngine;

// Manages currently hired employees and their education/progress data
public class EmployeeManager : Singleton<EmployeeManager>
{
    // Source of truth: use GameData.HiredEmployees managed by GameManager
    private Dictionary<int, EmployeeData> HiredDict => GameManager.Instance.GameData.HiredEmployees;
    public IReadOnlyDictionary<int, EmployeeData> HiredEmployees => HiredDict;

    // Hire an employee (adds to GameData.HiredEmployees)
    public bool Hire(int employeeId)
    {
        var dict = HiredDict;
        if (dict == null)
        {
            GameManager.Instance.GameData.HiredEmployees = new Dictionary<int, EmployeeData>();
            dict = HiredDict;
        }

        if (dict.ContainsKey(employeeId))
            return false;

        if (!DataManager.Instance.EmployeeDict.TryGetValue(employeeId, out var data) || data == null)
            return false;

        dict.Add(employeeId, data);
        EventManager.Instance.TriggerEvent(Define.EEventType.InitHiredEmployeesChanged);
        return true;
    }

    // Fire an employee (removes from GameData.HiredEmployees)
    public bool Fire(int employeeId)
    {
        var dict = HiredDict;
        if (dict == null)
            return false;

        bool removed = dict.Remove(employeeId);
        if (removed)
            EventManager.Instance.TriggerEvent(Define.EEventType.InitHiredEmployeesChanged);
        return removed;
    }

    // Apply education/training points directly to the hired employee's stats in GameData
    public bool ApplyTraining(int employeeId, int stat1Delta = 0, int stat2Delta = 0, int stat3Delta = 0)
    {
        var dict = HiredDict;
        if (dict == null)
            return false;
        if (!dict.TryGetValue(employeeId, out EmployeeData employData) || employData == null)
            return false;

        employData.Stat1Trained += stat1Delta;
        employData.Stat2Trained += stat2Delta;
        employData.Stat3Trained += stat3Delta;

        EventManager.Instance.TriggerEvent(Define.EEventType.EmployEducationDone);
        return true;
    }

    // Get current display stats (base + trained) for UI convenience
    public bool TryGetDisplayStats(int employeeId, out int stat1, out int stat2, out int stat3)
    {
        stat1 = stat2 = stat3 = 0;
        var dict = HiredDict;
        if (dict == null)
            return false;
        if (!dict.TryGetValue(employeeId, out EmployeeData employData) || employData == null)
            return false;

        stat1 = employData.Stat1 + employData.Stat1Trained;
        stat2 = employData.Stat2 + employData.Stat2Trained;
        stat3 = employData.Stat3 + employData.Stat3Trained;
        return true;
    }

    // 직원 교육 비용 계산 (예: Stat1 기준). 현재 누적 훈련량을 기준으로 섹션을 찾아 비용 반환.
    public int GetEmployeeTrainStat1Coast(int employeeID)
    {
        var dict = HiredDict;
        if (dict == null || !dict.TryGetValue(employeeID, out EmployeeData employData) || employData == null)
            return 0;

        var config = DataManager.Instance.EducationConfig;
        if (config == null)
        {
            Debug.LogError("EducationConfig not loaded.");
            return 0;
        }

        int trained = employData.Stat1Trained;
        int sectionIndex = 0;
        var sections = config.Stat1EducationSections;
        for (int i = 0; i < sections.Count; i++)
        {
            if (trained < sections[i]) { sectionIndex = i; break; }
            sectionIndex = i;
        }

        var costs = config.Stat1EducationCosts;
        if (sectionIndex < 0 || sectionIndex >= costs.Count)
            return 0;
        return costs[sectionIndex];
    }

    // 직원 교육 비용 계산 (예: Stat2 기준). 현재 누적 훈련량을 기준으로 섹션을 찾아 비용 반환.
    public int GetEmployeeTrainStat2Coast(int employeeID)
    {
        var dict = HiredDict;
        if (dict == null || !dict.TryGetValue(employeeID, out EmployeeData employData) || employData == null)
            return 0;

        var config = DataManager.Instance.EducationConfig;
        if (config == null)
        {
            Debug.LogError("EducationConfig not loaded.");
            return 0;
        }

        int trained = employData.Stat2Trained;
        int sectionIndex = 0;
        var sections = config.Stat2EducationSections;
        for (int i = 0; i < sections.Count; i++)
        {
            if (trained < sections[i]) { sectionIndex = i; break; }
            sectionIndex = i;
        }

        var costs = config.Stat2EducationCosts;
        if (sectionIndex < 0 || sectionIndex >= costs.Count)
            return 0;
        return costs[sectionIndex];
    }

    // 교육 포인트 증가량 계산 (Stat1/Stat2). 현재 누적 훈련량 기준 섹션의 포인트값 반환
    public int GetTrainDeltaPointsStat1(int employeeID)
    {
        var dict = HiredDict;
        if (dict == null || !dict.TryGetValue(employeeID, out EmployeeData employData) || employData == null)
            return 0;

        var config = DataManager.Instance.EducationConfig;
        if (config == null)
        {
            Debug.LogError("EducationConfig not loaded.");
            return 0;
        }

        int trained = employData.Stat1Trained;
        int sectionIndex = 0;
        var sections = config.Stat1EducationSections;
        for (int i = 0; i < sections.Count; i++)
        {
            if (trained < sections[i]) { sectionIndex = i; break; }
            sectionIndex = i;
        }

        var points = config.Stat1EducationDeltaPoints;
        if (sectionIndex < 0 || sectionIndex >= points.Count)
            return 0;
        return points[sectionIndex];
    }

    public int GetTrainDeltaPointsStat2(int employeeID)
    {
        var dict = HiredDict;
        if (dict == null || !dict.TryGetValue(employeeID, out EmployeeData employData) || employData == null)
            return 0;

        var config = DataManager.Instance.EducationConfig;
        if (config == null)
        {
            Debug.LogError("EducationConfig not loaded.");
            return 0;
        }

        int trained = employData.Stat2Trained;
        int sectionIndex = 0;
        var sections = config.Stat2EducationSections;
        for (int i = 0; i < sections.Count; i++)
        {
            if (trained < sections[i]) { sectionIndex = i; break; }
            sectionIndex = i;
        }

        var points = config.Stat2EducationDeltaPoints;
        if (sectionIndex < 0 || sectionIndex >= points.Count)
            return 0;
        return points[sectionIndex];
    }
}
