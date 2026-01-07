using UnityEngine;
using static Define;

public class UI_MainGame : UI_UGUI, IUI_Scene
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

    private UI_TopPanel _topPanel;
    private UI_LeftPanel _leftPanel;
    private UI_BottomPanel _bottomPanel;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        _topPanel = Utils.FindChildComponent<UI_TopPanel>(gameObject, recursive: true);
        _leftPanel = Utils.FindChildComponent<UI_LeftPanel>(gameObject, recursive: true);
        _bottomPanel = Utils.FindChildComponent<UI_BottomPanel>(gameObject, recursive: true);


        GetButton((int)Buttons.MenuButton).onClick.AddListener(OnClickMenuButton);

    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        _leftPanel.RefreshUI();
        _bottomPanel.RefreshUI();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.Instance.AddEvent(EEventType.UI_PopupStackChanged, OnPopupStackChanged);
        // Initial label state
        OnPopupStackChanged();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (EventManager.Instance != null)
            EventManager.Instance.RemoveEvent(EEventType.UI_PopupStackChanged, OnPopupStackChanged);
    }

    private void OnPopupStackChanged()
    {
        bool hasPopup = UIManager.Instance.GetLastPopupUI<UI_Base>() != null;
        if (hasPopup)
        {
            UpdateMenuButtonLabel(true); // Close
        }
        else
        {
            UpdateMenuButtonLabel(false);
        }
    }

    private void OnClickMenuButton()
    {
        // If any popup is open, close it first
        var lastPopup = UIManager.Instance.GetLastPopupUI<UI_Base>();
        if (lastPopup != null)
        {
            UIManager.Instance.ClosePopupUI();
            return;
        }

        if (_leftPanel.gameObject.activeSelf)
        {
            bool willShow = !(_leftPanel.gameObject.activeSelf);
            _leftPanel.ToggleShowLeftPanel();
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
