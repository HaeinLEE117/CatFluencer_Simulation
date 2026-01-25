using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_CastPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        ApplicantPhoto,
    }

    enum Buttons
    {
        //상단
        PreButton,
        NextButton,

        HireButton,
    }

    enum Texts
    {
        ApplicantNameText,

        Stat1Text,
        Stat2Text, 
        Stat3Text,

        Stat1PointText,
        Stat2PointText,
        Stat3PointText,

        ApplicantCastFeeText,
        ComboExpectationText,
    }

    // 3차 팝업으로 선택된 데이터를 전달하기 위한 내부 변수
    private int _selectedCast;
    private int _index = 0;
    private Button _preBtn;
    private Button _nextBtn;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.HireButton).onClick.AddListener(OnClickSelect);

        _preBtn = GetButton((int)Buttons.PreButton);
        _nextBtn = GetButton((int)Buttons.NextButton);
        if (_preBtn != null) _preBtn.onClick.AddListener(OnPrev);
        if (_nextBtn != null) _nextBtn.onClick.AddListener(OnNext);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        var dict = GameManager.Instance.AvailableCasts;
        int total = dict != null ? dict.Count : 0;
        if (total <= 0)
        {
            // disable navigation and clear fields
            if (_preBtn != null) _preBtn.gameObject.SetActive(false);
            if (_nextBtn != null) _nextBtn.gameObject.SetActive(false);
            var name = GetText((int)Texts.ApplicantNameText);
            var s1 = GetText((int)Texts.Stat1Text);
            var s2 = GetText((int)Texts.Stat2Text);
            var s3 = GetText((int)Texts.Stat3Text);
            var p1 = GetText((int)Texts.Stat1PointText);
            var p2 = GetText((int)Texts.Stat2PointText);
            var p3 = GetText((int)Texts.Stat3PointText);
            var fee = GetText((int)Texts.ApplicantCastFeeText);
            if (name != null) name.text = string.Empty;
            if (s1 != null) s1.text = string.Empty;
            if (s2 != null) s2.text = string.Empty;
            if (s3 != null) s3.text = string.Empty;
            if (p1 != null) p1.text = string.Empty;
            if (p2 != null) p2.text = string.Empty;
            if (p3 != null) p3.text = string.Empty;
            if (fee != null) fee.text = string.Empty;
            return;
        }

        // order by key for deterministic navigation
        var keys = new System.Collections.Generic.List<int>(dict.Keys);
        keys.Sort();

        if (_index < 0) _index = 0;
        if (_index >= keys.Count) _index = keys.Count - 1;
        int currentId = keys[_index];
        var data = dict[currentId];

        _selectedCast = data.TemplateID;

        GetText((int)Texts.ApplicantNameText)?.SetLocalizedText(data.NameTextID);
        GetText((int)Texts.Stat1Text)?.SetLocalizedText("STAT1");
        GetText((int)Texts.Stat2Text)?.SetLocalizedText("STAT2");
        GetText((int)Texts.Stat3Text)?.SetLocalizedText("STAT3");

        GetText((int)Texts.Stat1PointText)?.SetTextwithFont(data.Stat1.ToString());
        GetText((int)Texts.Stat2PointText)?.SetTextwithFont(data.Stat2.ToString());
        GetText((int)Texts.Stat3PointText)?.SetTextwithFont(data.Stat3.ToString());

        int castPay = data.CastPay;
        GetText((int)Texts.ApplicantCastFeeText)?.SetTextwithFont(castPay.ToString() + " G");

        var imgGO = GetObject((int)GameObjects.ApplicantPhoto);
        var img = imgGO != null ? imgGO.GetComponent<UnityEngine.UI.Image>() : null;
        if (img != null)
        {
            img.sprite = ResourceManager.Instance.Get<Sprite>(data.PhotoImageID);
        }

        bool canNavigate = keys.Count > 1;
        if (_preBtn != null)
        {
            _preBtn.gameObject.SetActive(canNavigate);
            _preBtn.interactable = _index > 0;
        }
        if (_nextBtn != null)
        {
            _nextBtn.gameObject.SetActive(canNavigate);
            _nextBtn.interactable = _index < keys.Count - 1;
        }
    }

    private void OnClickSelect()
    {
        GameManager.Instance.UpdateRecordingCast(_selectedCast);
        UIManager.Instance.ShowPopupUI(nameof(UI_NewVideoPopup));
    }

    private void OnPrev()
    {
        var dict = GameManager.Instance.AvailableCasts;
        int total = dict != null ? dict.Count : 0;
        if (total <= 1) return;
        _index = Mathf.Max(0, _index - 1);
        RefreshUI();
    }

    private void OnNext()
    {
        var dict = GameManager.Instance.AvailableCasts;
        int total = dict != null ? dict.Count : 0;
        if (total <= 1) return;
        _index = Mathf.Min(total - 1, _index + 1);
        RefreshUI();
    }
}
