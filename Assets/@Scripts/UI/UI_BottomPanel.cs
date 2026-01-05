using System;
using UnityEngine;
using static Define;

public class UI_BottomPanel : UI_UGUI
{
    enum GameObjects
    {
    }

    enum Buttons
    {
        MenuButton,
    }

    enum Texts
    {
        MenuButtonText,

    }


    protected override void Awake()
    {
        base.Awake();


        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.MenuButton).onClick.AddListener(OnClickMenuButton);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

    }

    private void OnClickMenuButton()
    {
        EventManager.Instance.TriggerEvent(EEventType.UI_MenuButtonClicked);
    }
}
