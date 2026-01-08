using UnityEngine;

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

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }
}
