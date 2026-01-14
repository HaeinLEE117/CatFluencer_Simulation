using UnityEngine;

public class UI_HirePopup : UI_UGUI, IUI_Popup
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

        ApplicantMentText,

        ApplicantPaymentText,
        FireButtonText,
    }

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        //TODO: EmployeeManager와 연동 필요
        GetButton((int)Buttons.HireButton).onClick.AddListener(ClosePopup);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }

    private void ClosePopup()
    {
        UIManager.Instance.ClosePopupUI();
    }

}
