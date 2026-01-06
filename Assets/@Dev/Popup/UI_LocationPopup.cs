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

    // Example selected data. In real implementation, bind to selection list.
    private string _selectedLocation;

    protected override void Awake()
    {
        base.Awake();
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.SelectButton).onClick.AddListener(OnClickSelect);
        GetButton((int)Buttons.LocationButton1).onClick.AddListener(() => SetSelectedLocation(GetText((int)Texts.LocationText1).text));
        GetButton((int)Buttons.LocationButton2).onClick.AddListener(() => SetSelectedLocation(GetText((int)Texts.LocationText2).text));
        GetButton((int)Buttons.LocationButton3).onClick.AddListener(() => SetSelectedLocation(GetText((int)Texts.LocationText3).text));
        GetButton((int)Buttons.LocationButton4).onClick.AddListener(() => SetSelectedLocation(GetText((int)Texts.LocationText4).text));
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        // Update selection text if needed
        var txt = GetText((int)Texts.SelectButtonText);
        if (txt != null)
            txt.text = _selectedLocation;
    }

    // Call this when a location item is chosen from list
    public void SetSelectedLocation(string location)
    {
        _selectedLocation = location;
        RefreshUI();
    }

    private void OnClickSelect()
    {
        //TODO: 정상적으로 작동하지 않음, 오히려 팝업 클로즈 버튼 버그 생성
        //TODO:다른 팝업들도 UI_NewVideoPopup으로 돌아가야 하는데 여러 팝업에서 정보를 공유하는 방법 고민 필요

        // Close this popup and notify selection
        UIManager.Instance.ClosePopupUI();
        // Trigger selection event
        EventManager.Instance.TriggerEvent(Define.EEventType.UI_LocationSelected);
        // Reopen NewVideo popup
        UIManager.Instance.ShowPopupUI("UI_NewVideoPopup");
    }


    // Provide a getter for selected data that NewVideoPopup can read via a shared service or static cache.
    public string GetSelected() => _selectedLocation;
}
