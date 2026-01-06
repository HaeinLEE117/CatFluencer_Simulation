using UnityEngine;

public class UI_FirePopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        EmployeePhoto,
    }

    enum Buttons
    {
        //»ó´Ü
        PreButton,
        NextButton,

        FireButton,
    }

    enum Texts
    {
        EmployeetNameText,

        Stat1Text,
        Stat2Text, 
        Stat3Text,

        Stat1PointText,
        Stat2PointText,
        Stat3PointText,


        EmployeeSalaryText,
        FireButtonText,
    }

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

    }
}
