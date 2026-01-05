using UnityEngine;
using static Define;
using UnityEngine.UI;

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

        //1차, 2차 패널 초기 비활성화
        gameObject.SetActive(false);
        
        GetObject((int)GameObjects.EditingOptionPanel).SetActive(false);
        GetObject((int)GameObjects.HumanResOptionPanel).SetActive(false);
        GetObject((int)GameObjects.StudioInfoOptionPanel).SetActive(false);


        //1차 패널 토글 이벤트 등록
        EventManager.Instance.AddEvent(EEventType.UI_MenuButtonClicked, ToggleShowLeftPanel);

        // 1차 패널 버튼 클릭 시 2차 패널 토글
        GetButton((int)Buttons.EditingButton).onClick.AddListener(() => ShowOnlyOptionPanel(GameObjects.EditingOptionPanel));
        GetButton((int)Buttons.HumanResourceButton).onClick.AddListener(() => ShowOnlyOptionPanel(GameObjects.HumanResOptionPanel));
        GetButton((int)Buttons.StudioInfoButton).onClick.AddListener(() => ShowOnlyOptionPanel(GameObjects.StudioInfoOptionPanel));
        GetButton((int)Buttons.SettingButton).onClick.AddListener(HideAllOptionPanels);

        // TODO: 2차 패널 토글 이벤트 등록
        // 2차 패널 버튼 기본 동작 등록 (필요 시 이벤트 매니저로 확장 가능)
        GetButton((int)Buttons.NewVideoButton).onClick.AddListener(() => OnClickSecondary("NewVideo"));
        GetButton((int)Buttons.LiveStreamButton).onClick.AddListener(() => OnClickSecondary("LiveStream"));

        GetButton((int)Buttons.EducationButton).onClick.AddListener(() => OnClickSecondary("Education"));
        GetButton((int)Buttons.HireButton).onClick.AddListener(() => OnClickSecondary("Hire"));
        GetButton((int)Buttons.FireButton).onClick.AddListener(() => OnClickSecondary("Fire"));

        GetButton((int)Buttons.GoalsButton).onClick.AddListener(() => OnClickSecondary("Goals"));
        GetButton((int)Buttons.PreVideosButton).onClick.AddListener(() => OnClickSecondary("PreVideos"));
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

    private void ShowOnlyOptionPanel(GameObjects panel)
    {
        // Hide all first
        HideAllOptionPanels();
        // Show selected
        GetObject((int)panel).SetActive(true);
    }

    private void HideAllOptionPanels()
    {
        GetObject((int)GameObjects.EditingOptionPanel).SetActive(false);
        GetObject((int)GameObjects.HumanResOptionPanel).SetActive(false);
        GetObject((int)GameObjects.StudioInfoOptionPanel).SetActive(false);
    }

    private void OnClickSecondary(string action)
    {
        Debug.Log($"[UI_LeftPanel] Secondary action clicked: {action}");
        // 필요 시 EventManager로 별도 EEventType 정의 후 TriggerEvent 호출
    }
}
