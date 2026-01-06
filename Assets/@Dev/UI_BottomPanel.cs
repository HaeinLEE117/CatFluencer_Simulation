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
        // If any popup is open, close it first
        var lastPopup = UIManager.Instance.GetLastPopupUI<UI_Base>();
        if (lastPopup != null)
        {
            UIManager.Instance.ClosePopupUI();
            UpdateMenuButtonLabel(false);
            return;
        }

        // Toggle LeftPanel
        var leftPanel = FindFirstObjectByType<UI_LeftPanel>();
        if (leftPanel != null)
        {
            bool willShow = !leftPanel.gameObject.activeSelf;
            leftPanel.ToggleShowLeftPanel();
            UpdateMenuButtonLabel(willShow);
        }
        else
        {
            // Fallback: fire event for any listener
            EventManager.Instance.TriggerEvent(EEventType.UI_MenuButtonClicked);
            UpdateMenuButtonLabel(true);
        }
    }

    private void UpdateMenuButtonLabel(bool leftPanelVisible)
    {
        var label = GetText((int)Texts.MenuButtonText);
        if (label == null) return;
        // TODO: Localize 텍스트 처리
        label.text = leftPanelVisible ? "Close" : "Menu";
    }
}
