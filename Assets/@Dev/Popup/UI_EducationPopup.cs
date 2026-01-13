using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EducationPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        EmployeePhoto,

    }

    enum Buttons
    {
        UpGradeStat1Button,
        UpGradeStat2Button,

        PreButton,
        NextButton,
    }

    enum Texts
    {
        EmployeeNameText,
        EmployeeSalaryText,

        //Column 1
        Stat1Text,
        Stat2Text, 
        Stat3Text,

        //Column 2
        Stat1PointText,
        Stat2PointText,
        Stat3PointText,

        //Column 3
        UpPoint1,
        UpPoint2,

        //Buttons
        UpGradeStat1ButtonText,
        UpGradeStat2ButtonText,
    }

    private List<int> _hiredIds;
    private int _currentIndex;

    Button PreButton;
    Button NextButton;
    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        _currentIndex = 0;

        // Wire navigation buttons

        PreButton = GetButton((int)Buttons.PreButton);
        PreButton.onClick.AddListener(OnPrev);
        NextButton = GetButton((int)Buttons.NextButton);
        NextButton.onClick.AddListener(OnNext);

        RefreshUI();

        EventManager.Instance.AddEvent(Define.EEventType.EmployEducationDone, RefreshUI);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        var hiredDict = GameManager.Instance.HiredEmployees;
        int count = hiredDict != null ? hiredDict.Count : 0;
        if (count == 0)
        {
            Debug.Log("No hired employees to display.");
            // 버튼 비활성화
            PreButton.gameObject.SetActive(false);
            NextButton.gameObject.SetActive(false);
            return;
        }

        // _hiredIds를 최신 상태로 동기화(필요 시)
        _hiredIds = new List<int>(hiredDict.Keys);

        // 1명일 때는 순회 버튼 비활성화, 2명 이상이면 활성화
        bool multi = _hiredIds.Count > 1;
        PreButton.gameObject.SetActive(multi);
        NextButton.gameObject.SetActive(multi);

        // 현재 인덱스 정규화
        if (_currentIndex < 0) _currentIndex = 0;
        if (_currentIndex >= _hiredIds.Count) _currentIndex = 0;

        int currentId = _hiredIds[_currentIndex];
        if (!hiredDict.TryGetValue(currentId, out EmployeeData employeeData) || employeeData == null)
        {
            Debug.LogWarning($"Employee id {currentId} not found in HiredEmployees");
            return;
        }

        GetText((int)Texts.EmployeeNameText).SetLocalizedText(employeeData.NameTextID);
        GetText((int)Texts.EmployeeSalaryText).text = employeeData.Salary.ToString() + " G";

        GetText((int)Texts.Stat1Text).SetLocalizedText("STAT1");
        GetText((int)Texts.Stat2Text).SetLocalizedText("STAT2");
        GetText((int)Texts.Stat3Text).SetLocalizedText("STAT3");

        GetText((int)Texts.Stat1PointText).text = (employeeData.Stat1).ToString();
        GetText((int)Texts.Stat2PointText).text = (employeeData.Stat2).ToString();
        GetText((int)Texts.Stat3PointText).text = (employeeData.Stat3).ToString();

        var photoGO = GetObject((int)GameObjects.EmployeePhoto);
        var img = photoGO != null ? photoGO.GetComponent<UnityEngine.UI.Image>() : null;
        if (img != null)
            img.sprite = ResourceManager.Instance.Get<Sprite>(employeeData.PhotoImageID);

        int traindeltaPointStat1 = GameManager.Instance.GetTraindeltaPointStat1(currentId);
        int traindeltaPointStat2 = GameManager.Instance.GetTraindeltaPointStat2(currentId);
        int trainStat1Coast = GameManager.Instance.GetEmployeeTrainStat1Cost(currentId);
        int trainStat2Coast = GameManager.Instance.GetEmployeeTrainStat2Cost(currentId);

        GetButton((int)Buttons.UpGradeStat1Button).onClick.RemoveAllListeners();
        GetButton((int)Buttons.UpGradeStat2Button).onClick.RemoveAllListeners();

        GetButton((int)Buttons.UpGradeStat1Button).onClick.AddListener(() =>
        {
            EmployeeManager.Instance.ApplyTrainingStat1(currentId, trainStat1Coast,traindeltaPointStat1);
        });
        GetButton((int)Buttons.UpGradeStat2Button).onClick.AddListener(() =>
        {
            EmployeeManager.Instance.ApplyTrainingStat2(currentId, trainStat2Coast, traindeltaPointStat2);
        });
        GetText((int)Texts.UpPoint1).text = traindeltaPointStat1.ToString();
        GetText((int)Texts.UpPoint2).text = traindeltaPointStat2.ToString();

        GetText((int)Texts.UpGradeStat1ButtonText).text = trainStat1Coast.ToString() + " G";
        GetText((int)Texts.UpGradeStat2ButtonText).text = trainStat2Coast.ToString() + " G";
    
    }

    private void OnPrev()
    {
        if (_hiredIds == null || _hiredIds.Count <= 1) return;
        _currentIndex = (_currentIndex - 1 + _hiredIds.Count) % _hiredIds.Count;
        RefreshUI();
    }

    private void OnNext()
    {
        if (_hiredIds == null || _hiredIds.Count <= 1) return;
        _currentIndex = (_currentIndex + 1) % _hiredIds.Count;
        RefreshUI();
    }
}
