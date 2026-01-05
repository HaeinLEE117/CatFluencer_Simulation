using UnityEngine;
using static Define;

public class UI_LeftPanel : UI_UGUI
{
    enum GameObjects
    {
        //2차 패널 오브젝트
        EditingOptionPanel,
        HumanResOptionPanel,
        StudioInfoOptionPanel
    }

    enum Buttons
    {
        //1차 패널 버튼
        EditingButton,
        HumanResourceButton,
        StudioInfoButton,
        SettingButton,

        //2차 패널 버튼
        NewVideoButton,
        LiveStreamButton,

        EducationButton,
        HireButton,
        FireButton,

        GoalsButton,
        PreVideosButton
    }

    enum Texts
    {
        //1차 패널 텍스트
        EditingButtonText,
        HumanResourceButtonText,
        StudioInfoButtonText,
        SettingButtonText,

        //2차 패널 텍스트
        NewVideoButtonText,
        LiveStreamButtonText,

        EducationButtonText,
        HireButtonText,
        FireButtonText,

        GoalsButtonText,
        PreVideosButtonText
    }

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        EventManager.Instance.AddEvent(EEventType.UI_MenuButtonClicked, ToggleShowLeftPanel);

        gameObject.SetActive(false);

    }
    private void OnDestroy()
    {
        EventManager.Instance.RemoveEvent(EEventType.UI_MenuButtonClicked, ToggleShowLeftPanel);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }

    public void ToggleShowLeftPanel()
    {
        bool wasActive = this.gameObject.activeSelf;
        gameObject.SetActive(!wasActive);

    }
}
