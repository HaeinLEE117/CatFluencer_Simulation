using UnityEngine;

public class UI_SettingsPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
    }

    enum Buttons
    {
        SFXDownButton,
        SFXUpButton,
        BGMDownButton,
        BGMUpButton,
        AutoSaveButton,
        SaveButton,
    }

    enum Texts
    {
        //Title
        SettingText,

        SFXText,
        BGMText,
        AutoSaveText,
        SaveText,

        SFXVolumeText,
        BGMVolumeText,

        AutoSaveButtonText,
        SaveButtonText,
    }

protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

    }

}
