using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_LocationPopup : UI_UGUI, IUI_Popup
{
    enum Buttons
    {
        PreButton,
        NextButton,

        LocationButton1,
        LocationButton2,
        LocationButton3,
        LocationButton4,

        SelectButton,
    }

    enum Texts
    {
        RecordingLocationText,
        //Colum Titles
        TitleLocationText,
        TitlePopularText,
        TitleCoastText,

        LocationText1,
        PopularText1,
        CoastText1,
        LocationText2,
        PopularText2,
        CoastText2,
        LocationText3,
        PopularText3,
        CoastText3,
        LocationText4,
        PopularText4,
        CoastText4,

        ComboExpectationText,
        SelectButtonText,
    }

    // 3차 팝업으로 선택된 데이터를 전달하기 위한 내부 변수
    private int _selectedLocation;
    private Button _selectButton;
    private int _startIndex = 0;

    Button preBtn;
    Button nextBtn;

    private int[] _locationIDs = new int[4];

    protected override void Awake()
    {
        base.Awake();
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        _selectButton = GetButton((int)Buttons.SelectButton);
        _selectButton.onClick.AddListener(OnClickSelect);
        _selectButton.gameObject.SetActive(false);

        GetButton((int)Buttons.LocationButton1).onClick.AddListener(() => SetSelectedLocation(_locationIDs[0]));
        GetButton((int)Buttons.LocationButton2).onClick.AddListener(() => SetSelectedLocation(_locationIDs[1]));
        GetButton((int)Buttons.LocationButton3).onClick.AddListener(() => SetSelectedLocation(_locationIDs[2]));
        GetButton((int)Buttons.LocationButton4).onClick.AddListener(() => SetSelectedLocation(_locationIDs[3]));
        
        preBtn = GetButton((int)Buttons.PreButton);
        nextBtn = GetButton((int)Buttons.NextButton);

        preBtn.onClick.AddListener(OnPrev);
        nextBtn.onClick.AddListener(OnNext);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        _selectButton.gameObject.SetActive(false);

        var items = GetOrderedCLocationItems();
        int total = items.Count;
        _startIndex = NormalizeStartIndex(total, _startIndex);
        // Optional: set header/localized titles
        // GetText((int)Texts.RecordingContentText)?.SetLocalizedText("RECORDING_CONTENT_TITLE");
        // GetText((int)Texts.TitleContentText)?.SetLocalizedText("TITLE_CONTENT");
        // GetText((int)Texts.TitlePopularText)?.SetLocalizedText("TITLE_POPULAR");
        // GetText((int)Texts.SelectButtonText)?.SetLocalizedText("SELECT");

        var buttons = new Button[]
        {
            GetButton((int)Buttons.LocationButton1),
            GetButton((int)Buttons.LocationButton2),
            GetButton((int)Buttons.LocationButton3),
            GetButton((int)Buttons.LocationButton4),
        };
        var contentTexts = new TMPro.TMP_Text[]
        {
            GetText((int)Texts.LocationText1),
            GetText((int)Texts.LocationText2),
            GetText((int)Texts.LocationText3),
            GetText((int)Texts.LocationText4),
        };
        var popularTexts = new TMPro.TMP_Text[]
        {
            GetText((int)Texts.PopularText1),
            GetText((int)Texts.PopularText2),
            GetText((int)Texts.PopularText3),
            GetText((int)Texts.PopularText4),
        };
        var coastTexts = new TMPro.TMP_Text[]
        {
            GetText((int)Texts.CoastText1),
            GetText((int)Texts.CoastText2),
            GetText((int)Texts.CoastText3),
            GetText((int)Texts.CoastText4),
        };

        int pageCount = Mathf.Clamp(total - _startIndex, 0, 4);
        for (int i = 0; i < 4; i++)
        {
            int idx = _startIndex + i;
            bool active = i < pageCount;
            if (buttons[i] != null) buttons[i].gameObject.SetActive(active);
            if (contentTexts[i] != null) contentTexts[i].gameObject.SetActive(active);
            if (popularTexts[i] != null) popularTexts[i].gameObject.SetActive(active);
            if (coastTexts[i] != null) coastTexts[i].gameObject.SetActive(active);

            if (active)
            {
                var data = items[idx];
                contentTexts[i]?.SetLocalizedText(data.LocationTextID);
                popularTexts[i]?.SetTextwithFont(data.Popularity.ToString());
                coastTexts[i]?.SetTextwithFont(data.Coast.ToString());
                _locationIDs[i] = data.LocationID;
            }
            else
            {
                // Optional: clear texts to avoid stale values if objects are reused
                if (contentTexts[i] != null) contentTexts[i].text = string.Empty;
                if (popularTexts[i] != null) popularTexts[i].text = string.Empty;
                if (coastTexts[i] != null) coastTexts[i].text = string.Empty;
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
        int total = GetOrderedCLocationItems().Count;
        if (total <= 4) return;
        _startIndex = NormalizeStartIndex(total, _startIndex - 4);
        RefreshUI();
    }

    private void OnNext()
    {
        int total = GetOrderedCLocationItems().Count;
        if (total <= 4) return;
        _startIndex = NormalizeStartIndex(total, _startIndex + 4);
        RefreshUI();
    }

    // Call this when a location item is chosen from list
    public void SetSelectedLocation(int locationID)
    {
        _selectedLocation = locationID;
        Debug.Log($"Location selected: {_selectedLocation}");
        _selectButton.gameObject.SetActive(true);
    }

    private void OnClickSelect()
    {
        UIManager.Instance.NotifyLocationSelected(_selectedLocation);
        UIManager.Instance.ShowPopupUI(nameof(UI_NewVideoPopup));
    }


    // Provide a getter for selected data that NewVideoPopup can read via a shared service or static cache.
    public int GetSelected() => _selectedLocation;

    private System.Collections.Generic.List<LocationData> GetOrderedCLocationItems()
    {
        var locationsDict = GameManager.Instance.AvailableLocations;
        var items = new System.Collections.Generic.List<LocationData>();
        if (locationsDict != null)
        {
            var keys = new System.Collections.Generic.List<int>(locationsDict.Keys);
            keys.Sort();
            foreach (var k in keys)
            {
                var v = locationsDict[k];
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
