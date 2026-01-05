using UnityEngine;

public class UI_MainGame : UI_UGUI, IUI_Scene
{
    enum GameObjects
    {
    }

    enum Buttons
    {
    }

    enum Texts
    {
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

    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        _leftPanel.RefreshUI();
        _bottomPanel.RefreshUI();
    }
}
