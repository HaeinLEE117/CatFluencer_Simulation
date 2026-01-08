using UnityEngine;

public class UI_ChatPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        NPCImage,
    }

    enum Buttons
    {
        NextButton,
    }

    enum Texts
    {
        ComentText,
        NPCNameText

    }

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

    }


}
