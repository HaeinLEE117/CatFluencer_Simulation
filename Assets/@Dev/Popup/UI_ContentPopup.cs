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
    private int _startIndex = 0;

    Button preBtn;
    Button nextBtn; 

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

        // Wire navigation
        preBtn  = GetButton((int)Buttons.PreButton);
        nextBtn = GetButton((int)Buttons.NextButton);

        preBtn.onClick.AddListener(OnPrev);
        nextBtn.onClick.AddListener(OnNext);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        _selectButton.gameObject.SetActive(false);

        var items = GetOrderedContentItems();
        int total = items.Count;
        _startIndex = NormalizeStartIndex(total, _startIndex);
        // Optional: set header/localized titles
        // GetText((int)Texts.RecordingContentText)?.SetLocalizedText("RECORDING_CONTENT_TITLE");
        // GetText((int)Texts.TitleContentText)?.SetLocalizedText("TITLE_CONTENT");
        // GetText((int)Texts.TitlePopularText)?.SetLocalizedText("TITLE_POPULAR");
        // GetText((int)Texts.SelectButtonText)?.SetLocalizedText("SELECT");

        var buttons = new Button[]
        {
            GetButton((int)Buttons.ContentButton1),
            GetButton((int)Buttons.ContentButton2),
            GetButton((int)Buttons.ContentButton3),
            GetButton((int)Buttons.ContentButton4),
        };
        var contentTexts = new TMPro.TMP_Text[]
        {
            GetText((int)Texts.ContentText1),
            GetText((int)Texts.ContentText2),
            GetText((int)Texts.ContentText3),
            GetText((int)Texts.ContentText4),
        };
        var popularTexts = new TMPro.TMP_Text[]
        {
            GetText((int)Texts.PopularText1),
            GetText((int)Texts.PopularText2),
            GetText((int)Texts.PopularText3),
            GetText((int)Texts.PopularText4),
        };

        int pageCount = Mathf.Clamp(total - _startIndex, 0, 4);
        for (int i = 0; i < 4; i++)
        {
            int idx = _startIndex + i;
            bool active = i < pageCount;
            if (buttons[i] != null) buttons[i].gameObject.SetActive(active);
            if (contentTexts[i] != null) contentTexts[i].gameObject.SetActive(active);
            if (popularTexts[i] != null) popularTexts[i].gameObject.SetActive(active);

            if (active)
            {
                var data = items[idx];
                contentTexts[i]?.SetLocalizedText(data.ContentsTextID);
                popularTexts[i]?.SetTextwithFont(data.Popularity.ToString());
            }
            else
            {
                // Optional: clear texts to avoid stale values if objects are reused
                if (contentTexts[i] != null) contentTexts[i].text = string.Empty;
                if (popularTexts[i] != null) popularTexts[i].text = string.Empty;
            }
        }

        // Toggle navigation buttons
        bool canPrev = _startIndex > 0;
        bool canNext = _startIndex + 4 < total;
        if (preBtn != null) preBtn.gameObject.SetActive(total > 4);
        if (nextBtn != null) nextBtn.gameObject.SetActive(total > 4);
        if (preBtn != null) preBtn.interactable = canPrev;
        if (nextBtn != null) nextBtn.interactable = canNext;
    }

    private void OnPrev()
    {
        int total = GetOrderedContentItems().Count;
        if (total <= 4) return;
        _startIndex = NormalizeStartIndex(total, _startIndex - 4);
        RefreshUI();
    }

    private void OnNext()
    {
        int total = GetOrderedContentItems().Count;
        if (total <= 4) return;
        _startIndex = NormalizeStartIndex(total, _startIndex + 4);
        RefreshUI();
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

    private System.Collections.Generic.List<ContentsData> GetOrderedContentItems()
    {
        var contentsDict = GameManager.Instance.AvailableContents;
        var items = new System.Collections.Generic.List<ContentsData>();
        if (contentsDict != null)
        {
            var keys = new System.Collections.Generic.List<int>(contentsDict.Keys);
            keys.Sort();
            foreach (var k in keys)
            {
                var v = contentsDict[k];
                if (v != null) items.Add(v);
            }
        }
        return items;
    }

    private int NormalizeStartIndex(int total, int desired)
    {
        if (total <= 0) return 0;
        if (desired < 0) desired = 0;
        // Last page start index should be total - remainder (or total - 4 if divisible by 4)
        int remainder = total % 4;
        int lastStart = remainder == 0 ? Mathf.Max(0, total - 4) : Mathf.Max(0, total - remainder);
        if (desired > lastStart) return lastStart;
        // Snap to page boundary (multiple of 4)
        return (desired / 4) * 4;
    }
}
