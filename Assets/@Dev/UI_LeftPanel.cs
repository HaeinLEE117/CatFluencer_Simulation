using UnityEngine;
using static Define;
using UnityEngine.UI;
using System.Collections.Generic;

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

    // Map second-level buttons to popup prefab names
    private readonly Dictionary<Buttons, string> _secondaryPopupMap = new Dictionary<Buttons, string>
    {
        { Buttons.NewVideoButton, "UI_NewVideoPopup" },
        { Buttons.LiveStreamButton, "UI_LiveStreamPopup" },
        { Buttons.EducationButton, "UI_EducationPopup" },
        { Buttons.HireButton, "UI_JobPostingPopup" },
        { Buttons.FireButton, "UI_FirePopup" },
        { Buttons.GoalsButton, "UI_GoalsPopup" },
        { Buttons.PreVideosButton, "UI_UploadedVideosPopup" },
        {  Buttons.SettingButton, "UI_SettingsPopup"   }
    };

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

        // 2차 패널 버튼 팝업 매핑으로 처리
        foreach (var kv in _secondaryPopupMap)
        {
            Buttons btn = kv.Key;
            string popupName = kv.Value;
            GetButton((int)btn).onClick.AddListener(() => {
                // Left panel and option panels hide
                HideAllOptionPanels();
                ToggleShowLeftPanel();

                UIManager.Instance.ShowPopupUI(popupName);
            });
        }
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

        GetObject((int)GameObjects.EditingOptionPanel).SetActive(false);
        GetObject((int)GameObjects.HumanResOptionPanel).SetActive(false);
        GetObject((int)GameObjects.StudioInfoOptionPanel).SetActive(false);
    }

    private void ShowOnlyOptionPanel(GameObjects panel)
    {
        if(GetObject((int)panel).activeSelf)
        {
            // 이미 활성화된 패널이면 숨기기
            GetObject((int)panel).SetActive(false);
        }
        else
        {
            // Hide all first
            HideAllOptionPanels();
            // Show selected
            GetObject((int)panel).SetActive(true);
        }
    }

    private void HideAllOptionPanels()
    {
        GetObject((int)GameObjects.EditingOptionPanel).SetActive(false);
        GetObject((int)GameObjects.HumanResOptionPanel).SetActive(false);
        GetObject((int)GameObjects.StudioInfoOptionPanel).SetActive(false);
    }
}
