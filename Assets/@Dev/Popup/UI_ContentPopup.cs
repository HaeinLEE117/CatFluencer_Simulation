using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_ContentPopup : UI_UGUI, IUI_Popup
{
    enum Buttons
    {
        PreButton,
        NextButton,

        ContentButton1,
        ContentButton2,
        ContentButton3,
        ContentButton4,

        SelectButton,
    }

    enum Texts
    {
        RecordingContentText,
        //Colum Titles
        TitleContentText,
        TitlePopularText,

        ContentText1,
        PopularText1,
        ContentText2,
        PopularText2,
        ContentText3,
        PopularText3,
        ContentText4,
        PopularText4,

        SelectButtonText,
    }

    // 3차 팝업으로 선택된 데이터를 전달하기 위한 내부 변수
    private string _selectedContent;
    private Button _selectButton;

    protected override void Awake()
    {
        base.Awake();
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        _selectButton = GetButton((int)Buttons.SelectButton);
        _selectButton.onClick.AddListener(OnClickSelect);
        _selectButton.gameObject.SetActive(false);

        GetButton((int)Buttons.ContentButton1).onClick.AddListener(() => SetSelectedContent(GetText((int)Texts.ContentText1).text));
        GetButton((int)Buttons.ContentButton2).onClick.AddListener(() => SetSelectedContent(GetText((int)Texts.ContentText2).text));
        GetButton((int)Buttons.ContentButton3).onClick.AddListener(() => SetSelectedContent(GetText((int)Texts.ContentText3).text));
        GetButton((int)Buttons.ContentButton4).onClick.AddListener(() => SetSelectedContent(GetText((int)Texts.ContentText4).text));
    
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }

    // Call this when a Content item is chosen from list
    public void SetSelectedContent(string Content)
    {
        _selectedContent = Content;
        _selectButton.gameObject.SetActive(true);
    }

    private void OnClickSelect()
    {
        GameManager.Instance.UpdateRecordingContent(_selectedContent);
        UIManager.Instance.ShowPopupUI(nameof(UI_NewVideoPopup));
    }


    // Provide a getter for selected data that NewVideoPopup can read via a shared service or static cache.
    public string GetSelected() => _selectedContent;
}
