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
        GetButton((int)Buttons.DoneButton).onClick.AddListener(OnClickDone);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }

    private void OnClickDone()
    {
        var titleText = GetText((int)Texts.TitleInputText)?.text ?? string.Empty;
        Debug.Log("Entered Title: " + titleText);
        if (!IsValidTitle(titleText))
        {
            // Show error and keep popup open
            UIManager.Instance.ShowConfirmPopup(
                LocalizationManager.Instance.GetLocalizedText("FAILED"),
                LocalizationManager.Instance.GetLocalizedText("INVALID_TITLE_SPECIAL"),
                null);
            return;
        }

        GameManager.Instance.UpdateRecordingTitle(titleText);
        UIManager.Instance.ShowPopupUI(nameof(UI_NewVideoPopup));
    }

    private bool IsValidTitle(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;
        // Remove embedded null and zero-width/invisible characters that can appear from TMP input
        text = text.Replace("\0", string.Empty)
                   .Replace("\u200B", string.Empty) // zero width space
                   .Replace("\u200C", string.Empty) // zero width non-joiner
                   .Replace("\u200D", string.Empty) // zero width joiner
                   .Replace("\uFEFF", string.Empty); // zero width no-break space
        foreach (var ch in text)
        {
            if (char.IsWhiteSpace(ch))
                continue;
            if (char.IsControl(ch))
                return false;
            if (IsAsciiLetter(ch) || char.IsDigit(ch) || IsKoreanChar(ch))
                continue;
            Debug.Log($"Invalid character in title: {ch}");
            return false;
        }
        return true;
    }

    private bool IsAsciiLetter(char ch)
    {
        return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z');
    }

    private bool IsKoreanChar(char ch)
    {
        // Hangul Syllables, Jamo, Compatibility Jamo, Extended-A/B
        return (ch >= '\uAC00' && ch <= '\uD7A3')
               || (ch >= '\u1100' && ch <= '\u11FF')
               || (ch >= '\u3130' && ch <= '\u318F')
               || (ch >= '\uA960' && ch <= '\uA97F')
               || (ch >= '\uD7B0' && ch <= '\uD7FF');
    }
}
