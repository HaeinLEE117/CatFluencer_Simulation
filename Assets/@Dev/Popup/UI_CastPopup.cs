using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_CastPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        ApplicantPhoto,
    }

    enum Buttons
    {
        //상단
        PreButton,
        NextButton,

        HireButton,
    }

    enum Texts
    {
        ApplicantNameText,

        Stat1Text,
        Stat2Text, 
        Stat3Text,

        Stat1PointText,
        Stat2PointText,
        Stat3PointText,

        ApplicantPaymentText,
        ComboExpectationText,
    }

    // 3차 팝업으로 선택된 데이터를 전달하기 위한 내부 변수
    private string _selectedCast;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.HireButton).onClick.AddListener(OnClickSelect);
    }


    private void OnClickSelect()
    {
        GameManager.Instance.UpdateRecordingCast(GetText((int)Texts.ApplicantNameText).text);

        EventManager.Instance.TriggerEvent(Define.EEventType.UI_CastSelected);

        UIManager.Instance.ShowPopupUI("UI_NewVideoPopup");
    }

}
