using UnityEngine;
using UnityEngine.UI;

public class UI_DirectingBalancePopup : UI_UGUI, IUI_Popup
{
    #region UI Elements
    enum GameObjects
    {
        LengthBar1,
        LengthBar2,
        LengthBar3,
        LengthBar4,
        LengthBar5,

        TrendBar1,
        TrendBar2,
        TrendBar3,
        TrendBar4,
        TrendBar5,

        LaughBar1,
        LaughBar2,
        LaughBar3,
        LaughBar4,
        LaughBar5,

        InfoBar1,
        InfoBar2,
        InfoBar3,
        InfoBar4,
        InfoBar5,

        MemoryBar1,
        MemoryBar2,
        MemoryBar3,
        MemoryBar4,
        MemoryBar5,

        EmotionBar1,
        EmotionBar2,
        EmotionBar3,
        EmotionBar4,
        EmotionBar5,

    }

    enum Buttons
    {
        LengthDownButton,
        LengthUpButton,
        TrendDownButton,
        TrendUpButton,
        LaughDownButton,
        LaughUpButton,
        InfoDownButton,
        InfoUpButton,
        MemoryDownButton,
        MemoryUpButton,
        EmotionDownButton,
        EmotionUpButton,

        StartButton,
    }

    enum Texts
    {
        DirectingText,
        RestPointsText,

        LengthText,
        TrendText,
        LaughText,
        InfoText,
        MemoryText,
        EmotionText,

        LocationText,
        SelectedLocationText,
        ContentText,
        SelectedContentText,
        StartButtonText,
    }
    #endregion

    // Current allocated points for each stat (0..5) 
    private int _length, _trend, _laugh, _info, _memory, _emotion;
    private int _maxBarsPerStat = 5;

    // Colors
    private Color _activeColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    private Color _inactiveColor = new Color(0.4f, 0.4f, 0.4f, 1f);

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetText((int)Texts.SelectedContentText).text = GameManager.Instance.RecordingVideoData.Content;
        GetText((int)Texts.SelectedLocationText).text = GameManager.Instance.RecordingVideoData.Location;

        GetButton((int)Buttons.StartButton).onClick.AddListener(OnStartButtonClicked);

        // Wire up buttons
        GetButton((int)Buttons.LengthUpButton).onClick.AddListener(() => AdjustStat(ref _length, +1));
        GetButton((int)Buttons.LengthDownButton).onClick.AddListener(() => AdjustStat(ref _length, -1));

        GetButton((int)Buttons.TrendUpButton).onClick.AddListener(() => AdjustStat(ref _trend, +1));
        GetButton((int)Buttons.TrendDownButton).onClick.AddListener(() => AdjustStat(ref _trend, -1));

        GetButton((int)Buttons.LaughUpButton).onClick.AddListener(() => AdjustStat(ref _laugh, +1));
        GetButton((int)Buttons.LaughDownButton).onClick.AddListener(() => AdjustStat(ref _laugh, -1));

        GetButton((int)Buttons.InfoUpButton).onClick.AddListener(() => AdjustStat(ref _info, +1));
        GetButton((int)Buttons.InfoDownButton).onClick.AddListener(() => AdjustStat(ref _info, -1));

        GetButton((int)Buttons.MemoryUpButton).onClick.AddListener(() => AdjustStat(ref _memory, +1));
        GetButton((int)Buttons.MemoryDownButton).onClick.AddListener(() => AdjustStat(ref _memory, -1));

        GetButton((int)Buttons.EmotionUpButton).onClick.AddListener(() => AdjustStat(ref _emotion, +1));
        GetButton((int)Buttons.EmotionDownButton).onClick.AddListener(() => AdjustStat(ref _emotion, -1));

        RefreshUI();
    }
    public override void RefreshUI()
    {
        base.RefreshUI();
        UpdateBars(GameObjects.LengthBar1, _length);
        UpdateBars(GameObjects.TrendBar1, _trend);
        UpdateBars(GameObjects.LaughBar1, _laugh);
        UpdateBars(GameObjects.InfoBar1, _info);
        UpdateBars(GameObjects.MemoryBar1, _memory);
        UpdateBars(GameObjects.EmotionBar1, _emotion);

        GetText((int)Texts.RestPointsText).text = "Points: "+GetRemainingPoints().ToString();
    }

    private void AdjustStat(ref int stat, int delta)
    {
        int currentTotal = _length + _trend + _laugh + _info + _memory + _emotion;
        int target = Mathf.Clamp(stat + delta, 0, _maxBarsPerStat);
        int change = target - stat;
        if (change == 0) return;

        // Enforce total available points
        int maxPoints = GameManager.Instance.GameData.TotalVidieoBalancePoints;
        if (change > 0)
        {
            if (currentTotal + change > maxPoints)
            {
                // Cap to remaining points
                change = Mathf.Max(0, maxPoints - currentTotal);
                target = stat + change;
            }
        }
        stat = target;
        RefreshUI();
    }

    private int GetRemainingPoints()
    {
        int currentTotal = _length + _trend + _laugh + _info + _memory + _emotion;
        int maxPoints = GameManager.Instance.GameData.TotalVidieoBalancePoints;
        return Mathf.Max(0, maxPoints - currentTotal);
    }

    // Update 5 bars for a stat, starting enum index provided
    private void UpdateBars(GameObjects startEnum, int value)
    {
        int startIndex = (int)startEnum; // assumes *_Bar1 is the first in a contiguous block of 5
        for (int i = 0; i < _maxBarsPerStat; i++)
        {
            var go = GetObject(startIndex + i);
            var img = go != null ? go.GetComponent<Image>() : null;
            if (img == null) continue;
            img.color = i < value ? _activeColor : _inactiveColor;
        }
    }

    private void OnStartButtonClicked()
    {
        GameManager.Instance.UpdateVideoBalanceData(_length, _trend, _laugh, _info, _memory, _emotion);
        UIManager.Instance.ClosePopupUI();
    }
}
