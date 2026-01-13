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

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        //TODO: Change 1001 to hired employee ID
        DataManager.Instance.EmployeeDict.TryGetValue(1001, out EmployeeData employeeData);
        GetText((int)Texts.EmployeeNameText).SetLocalizedText(employeeData.NameTextID);
        GetText((int)Texts.EmployeeSalaryText).text = employeeData.Salary.ToString() + " G";

        GetText((int)Texts.Stat1Text).SetLocalizedText("STAT1");
        GetText((int)Texts.Stat2Text).SetLocalizedText("STAT2");
        GetText((int)Texts.Stat3Text).SetLocalizedText("STAT3");

        GetText((int)Texts.Stat1PointText).text = employeeData.Stat1.ToString();
        GetText((int)Texts.Stat2PointText).text = employeeData.Stat2.ToString();
        GetText((int)Texts.Stat3PointText).text = employeeData.Stat3.ToString();

        Get<GameObject>((int)GameObjects.EmployeePhoto).GetComponent<UnityEngine.UI.Image>().sprite
              = ResourceManager.Instance.Get<Sprite>(employeeData.PhotoImageID);
    }
}
