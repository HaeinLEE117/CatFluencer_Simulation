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

    #region 교육/훈련

    // Apply education/training points directly to the hired employee's stats in GameData
    public bool ApplyTraining(int employeeId, int stat1Delta = 0, int stat2Delta = 0, int stat3Delta = 0)
    {
        var dict = HiredDict;
        if (dict == null)
            return false;
        if (!dict.TryGetValue(employeeId, out EmployeeData employData) || employData == null)
            return false;

        // Apply training deltas
        employData.Stat1 += stat1Delta;
        employData.Stat2 += stat2Delta;
        employData.Stat3 += stat3Delta;

        EventManager.Instance.TriggerEvent(Define.EEventType.EmployEducationDone);
        return true;
    }

    public bool ApplyTrainingStat1(int employeeId,int coast, int stat1Delta)
    {
        if (GameManager.Instance.GoldDeduct(coast))
            return ApplyTraining(employeeId, stat1Delta, 0, 0);

        return false;
    }

    public bool ApplyTrainingStat2(int employeeId, int coast, int stat2Delta)
    {
        if (GameManager.Instance.GoldDeduct(coast))
            return ApplyTraining(employeeId, 0, stat2Delta, 0);
        return false;
    }

    // 직원 교육 비용 계산 (예: Stat1 기준). 현재 스텟을 기준으로 섹션을 찾아 비용 반환.
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

        int sectionIndex = 0;
        var sections = config.Stat1EducationSections;
        for (int i = 0; i < sections.Count; i++)
        {
            if (employData.Stat1 < sections[i]) { sectionIndex = i; break; }
            sectionIndex = i;
        }

        var costs = config.Stat1EducationCosts;
        if (sectionIndex < 0 || sectionIndex >= costs.Count)
            return 0;
        return costs[sectionIndex];
    }

    // 직원 교육 비용 계산 (예: Stat2 기준). 현재 스텟을 기준으로 섹션을 찾아 비용 반환.
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

        int sectionIndex = 0;
        var sections = config.Stat2EducationSections;
        for (int i = 0; i < sections.Count; i++)
        {
            if (employData.Stat2 < sections[i]) { sectionIndex = i; break; }
            sectionIndex = i;
        }

        var costs = config.Stat2EducationCosts;
        if (sectionIndex < 0 || sectionIndex >= costs.Count)
            return 0;
        return costs[sectionIndex];
    }

    // 교육 포인트 증가량 계산 (Stat1/Stat2). 현재 텟을 기준 섹션의 포인트값 반환
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

        int sectionIndex = 0;
        var sections = config.Stat1EducationSections;
        for (int i = 0; i < sections.Count; i++)
        {
            if (employData.Stat1 < sections[i]) { sectionIndex = i; break; }
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

        int sectionIndex = 0;
        var sections = config.Stat2EducationSections;
        for (int i = 0; i < sections.Count; i++)
        {
            if ( employData.Stat2 < sections[i]) { sectionIndex = i; break; }
            sectionIndex = i;
        }

        var points = config.Stat2EducationDeltaPoints;
        if (sectionIndex < 0 || sectionIndex >= points.Count)
            return 0;
        return points[sectionIndex];
    }
    #endregion


    //TODO: 구인 포스트를 올린 후, 일정 주가 지나면 지원자가 있다고 확인 팝업을 띄우는 기능 구현 필요
    public void StartUIJobPosting()
    {
        // Subscribe to week advancement and after a delay, show the hire popup.
        if (_jobPostingActive)
        {
            //TODO: Show ConfirmPopup
        }
        _jobPostingActive = true;
        _weeksUntilApplicant = constants.WEEKSFORJOBPOSTINGDONE;
        EventManager.Instance.AddEvent(Define.EEventType.WeekAdvanced, OnWeekAdvancedForJobPosting);
    }

    private void OnWeekAdvancedForJobPosting()
    {
        if (!_jobPostingActive) return;
        _weeksUntilApplicant--;
        if (_weeksUntilApplicant <= 0)
        {
            _jobPostingActive = false;
            EventManager.Instance.RemoveEvent(Define.EEventType.WeekAdvanced, OnWeekAdvancedForJobPosting);
            // Show applicant confirmation popup
            UIManager.Instance.ShowPopupUI("UI_HirePopup");
        }
    }

    // Configurable defaults; can be moved to GameConfig if needed
    private bool _jobPostingActive;
    private int _weeksUntilApplicant = 0;
}
