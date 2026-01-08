using UnityEngine;

public class UI_TitleEnterPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        TitleInputPlaceholder,
    }

    enum Buttons
    {
        DoneButton,
    }

    enum Texts
    {
        TitleEnterText,

        TitleInputText,

        DoneButtonText,
    }

    // 3차 팝업으로 선택된 데이터를 전달하기 위한 내부 변수
    private string _enteredTitle;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        //TODO: TitleInputPlaceholder 로컬라이징 시스템 적용 시 수정 필요
        //TODO: 입력텍스트 예외처리

        GetButton((int)Buttons.DoneButton).onClick.AddListener(OnClickDone);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }

    private void OnClickDone()
    {
        GameManager.Instance.UpdateRecordingTitle(GetText((int)Texts.TitleInputText).text);

        EventManager.Instance.TriggerEvent(Define.EEventType.UI_CastSelected);

        UIManager.Instance.ShowPopupUI(nameof(UI_NewVideoPopup));
    }
}
