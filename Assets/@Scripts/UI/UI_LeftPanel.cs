using UnityEngine;

public class UI_LeftPanel : UI_UGUI
{
    enum GameObjects
    {

    }

    enum Buttons
    {
        //Left Panel
        EditingButton,
        HumanResourceButton,
        StudioInfoButton,
        SettingButton,
    }

    enum Texts
    {
        //Left Panel
        EditingButtonText,
        HumanResourceButtonText,
        StudioInfoButtonText,
        SettingButtonText,
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
