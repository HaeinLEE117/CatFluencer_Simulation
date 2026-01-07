using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_LocationPopup : UI_UGUI, IUI_Popup
{
    enum Buttons
    {
        PreButton,
        NextButton,

        LocationButton1,
        LocationButton2,
        LocationButton3,
        LocationButton4,

        SelectButton,
    }

    enum Texts
    {
        RecordingLocationText,
        //Colum Titles
        TitleLocationText,
        TitlePopularText,
        TitleCoastText,

        LocationText1,
        PopularText1,
        CoastText1,
        LocationText2,
        PopularText2,
        CoastText2,
        LocationText3,
        PopularText3,
        CoastText3,
        LocationText4,
        PopularText4,
        CoastText4,

        SelectButtonText,
    }

    // 3차 팝업으로 선택된 데이터를 전달하기 위한 내부 변수
    private string _selectedLocation;
    private Button _selectButton;

    protected override void Awake()
    {
        base.Awake();
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        _selectButton = GetButton((int)Buttons.SelectButton);
        _selectButton.onClick.AddListener(OnClickSelect);
        _selectButton.gameObject.SetActive(false);

        GetButton((int)Buttons.LocationButton1).onClick.AddListener(() => SetSelectedLocation(GetText((int)Texts.LocationText1).text));
        GetButton((int)Buttons.LocationButton2).onClick.AddListener(() => SetSelectedLocation(GetText((int)Texts.LocationText2).text));
        GetButton((int)Buttons.LocationButton3).onClick.AddListener(() => SetSelectedLocation(GetText((int)Texts.LocationText3).text));
        GetButton((int)Buttons.LocationButton4).onClick.AddListener(() => SetSelectedLocation(GetText((int)Texts.LocationText4).text));
    
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }

    // Call this when a location item is chosen from list
    public void SetSelectedLocation(string location)
    {
        _selectedLocation = location;
        _selectButton.gameObject.SetActive(true);
    }

    private void OnClickSelect()
    {
        GameManager.Instance.UpdateRecordingLocation(_selectedLocation);
        // Trigger selection event
        EventManager.Instance.TriggerEvent(Define.EEventType.UI_LocationSelected);
        // Close this popup and notify selection
        UIManager.Instance.ShowPopupUI("UI_NewVideoPopup");
    }


    // Provide a getter for selected data that NewVideoPopup can read via a shared service or static cache.
    public string GetSelected() => _selectedLocation;
}
