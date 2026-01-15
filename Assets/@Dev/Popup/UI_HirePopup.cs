using UnityEngine;

public class UI_HirePopup : UI_UGUI, IUI_Popup
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

        ApplicantMentText,

        ApplicantPaymentText,
        HireButtonText,
    }

    private int _index;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.PreButton).onClick.AddListener(OnPrev);
        GetButton((int)Buttons.NextButton).onClick.AddListener(OnNext);
        GetButton((int)Buttons.HireButton).onClick.AddListener(OnHire);

        _index = 0;

        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        var list = GameManager.Instance.HireableEmployees;
        if (list == null || list.Count == 0)
        {
            // TODO: Show empty state
            return;
        }

        if (_index < 0) _index = 0;
        if (_index >= list.Count) _index = 0;

        var e = list[_index];

        GetText((int)Texts.ApplicantNameText).SetLocalizedText(e.NameTextID);
        GetText((int)Texts.Stat1Text).SetLocalizedText("STAT1");
        GetText((int)Texts.Stat2Text).SetLocalizedText("STAT2");
        GetText((int)Texts.Stat3Text).SetLocalizedText("STAT3");

        GetText((int)Texts.Stat1PointText).text = e.Stat1.ToString();
        GetText((int)Texts.Stat2PointText).text = e.Stat2.ToString();
        GetText((int)Texts.Stat3PointText).text = e.Stat3.ToString();

        GetText((int)Texts.ApplicantMentText).SetLocalizedText(e.CharMentTextID);
        GetText((int)Texts.ApplicantPaymentText).text = e.ContactFee.ToString() + " G";

        var imgGO = GetObject((int)GameObjects.ApplicantPhoto);
        var img = imgGO != null ? imgGO.GetComponent<UnityEngine.UI.Image>() : null;
        if (img != null)
        {
            img.sprite = ResourceManager.Instance.Get<Sprite>(e.PhotoImageID);
        }

        bool canNavigate = list.Count > 1;
        GetButton((int)Buttons.PreButton).interactable = canNavigate;
        GetButton((int)Buttons.NextButton).interactable = canNavigate;
    }

    private void OnPrev()
    {
        var list = GameManager.Instance.HireableEmployees;
        if (list == null || list.Count <= 1) return;
        _index = (_index - 1 + list.Count) % list.Count;
        RefreshUI();
    }

    private void OnNext()
    {
        var list = GameManager.Instance.HireableEmployees;
        if (list == null || list.Count <= 1) return;
        _index = (_index + 1) % list.Count;
        RefreshUI();
    }

    private void OnHire()
    {
        var list = GameManager.Instance.HireableEmployees;
        if (list == null || list.Count == 0) return;
        var e = list[_index];
        GameManager.Instance.HireEmployee(e.TemplateID);
        // Optionally close popup or update candidates
        UIManager.Instance.ClosePopupUI();
    }
}
