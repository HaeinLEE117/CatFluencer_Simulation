using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : Singleton<UIManager>
{
    Transform _root;
    Transform Root
    {
        get
        {
            return Utils.GetRootTransform(ref _root, "@UI_Root");
        }
    }

    #region Scene UI
    private UI_Base _sceneUI;
    public UI_Base SceneUI
    {
        get
        {
			foreach (UI_Base ui in FindObjectsByType<UI_Base>(FindObjectsSortMode.None))
			{
				if (ui is IUI_Scene)
				{
					_sceneUI = ui;
					break;
				}
			}
			
			return _sceneUI;
        }
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Base, IUI_Scene
	{
        if (_sceneUI != null)
            return _sceneUI as T;

        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        T sceneUI = FindFirstObjectByType<T>();
        if (sceneUI == null)
        {
            GameObject go = ResourceManager.Instance.Instantiate(name);
            sceneUI = Utils.GetOrAddComponent<T>(go);
        }

        sceneUI.transform.SetParent(Root);
        _sceneUI = sceneUI;

        return sceneUI;
    }
    #endregion

    #region Popup UI
    Transform _popupRoot;
    Transform PopupRoot
    {
        get
        {
            return Utils.GetRootTransform(ref _popupRoot, "@PopupRoot", Root);
        }
    }

    // Single active popup model
    private UI_Base _activePopup;
    private bool _wasPopupOpen;
    private readonly Dictionary<string, UI_Base> _popups = new Dictionary<string, UI_Base>();
    private const int FixedPopupSortingOrder = 300;

    public T ShowPopupUI<T>(string name = null) where T : UI_Base, IUI_Popup
	{
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        if (_popups.TryGetValue(name, out UI_Base popup) == false)
        {
            GameObject go = ResourceManager.Instance.Instantiate(name);
            popup = Utils.GetOrAddComponent<T>(go);
            _popups[name] = popup;
        }

        // Close previous popup if different
        if (_activePopup != null && _activePopup != popup)
        {
            if (_activePopup is UI_Toolkit prevToolkit)
                prevToolkit.GetComponent<UIDocument>().rootVisualElement.visible = false;
            else
                _activePopup.gameObject.SetActive(false);
        }

        _activePopup = popup;
        _wasPopupOpen = true;

        popup.transform.SetParent(PopupRoot);
        popup.gameObject.SetActive(true);

        if (SceneManager.Instance.CurrentSceneType == Define.EScene.DevScene)
        {
            GameManager.Instance.StopTime();
        }

        if (popup is UI_Toolkit toolkitUI)
        {
            var doc = toolkitUI.GetComponent<UIDocument>();
            doc.sortingOrder = FixedPopupSortingOrder;
            doc.rootVisualElement.visible = true;
        }
        else
        {
            var canvas = popup.GetComponent<Canvas>();
            if (canvas != null)
                canvas.sortingOrder = FixedPopupSortingOrder;
        }

        EventManager.Instance.TriggerEvent(Define.EEventType.UI_PopupOpened);
        return popup as T;
    }

    public UI_Base ShowPopupUI(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        // Close current then open requested
        ClosePopupUI();

        if (_popups.TryGetValue(name, out UI_Base popup) == false)
        {
            GameObject go = ResourceManager.Instance.Instantiate(name);
            popup = go.GetComponent<UI_Base>();
            if (popup == null)
                popup = Utils.GetOrAddComponent<UI_GenericPopup>(go);
            _popups[name] = popup;
        }

        _activePopup = popup;
        _wasPopupOpen = true;

        popup.transform.SetParent(PopupRoot);
        popup.gameObject.SetActive(true);

        if (popup is UI_Toolkit toolkitUI)
        {
            var doc = toolkitUI.GetComponent<UIDocument>();
            doc.sortingOrder = FixedPopupSortingOrder;
            doc.rootVisualElement.visible = true;
        }
        else
        {
            var canvas = popup.GetComponent<Canvas>();
            if (canvas != null)
                canvas.sortingOrder = FixedPopupSortingOrder;
        }

        EventManager.Instance.TriggerEvent(Define.EEventType.UI_PopupOpened);
        return popup;
    }

    public T GetLastPopupUI<T>() where T : UI_Base
	{
        return _activePopup as T;
    }

    public void ClosePopupUI()
    {
        // If no active popup, do nothing
        if (_activePopup == null)
            return;

        // Deactivate current active popup
        UI_Base popup = _activePopup;
        _activePopup = null;

        if (popup is UI_Toolkit toolkitUI)
            toolkitUI.GetComponent<UIDocument>().rootVisualElement.visible = false;
        else
            popup.gameObject.SetActive(false);

        // Resume time flow only in DevScene
        if (SceneManager.Instance.CurrentSceneType == Define.EScene.DevScene)
        {
            GameManager.Instance.StartTime();
        }

        // Single-popup policy: after closing, none is open
        _wasPopupOpen = false;
        EventManager.Instance.TriggerEvent(Define.EEventType.UI_PopupClosed);
    }

    public void CloseAllPopupUI()
    {
        ClosePopupUI();
    }
    #endregion

    public T ShowUI<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourceManager.Instance.Instantiate(name);

        return go.GetOrAddComponent<T>();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _popups.Clear();

        Root.DestroyChildren();

        _sceneUI = null;
    }

    public void NotifyLocationSelected(string location)
    {
        // 공통 검증/전처리
        if (string.IsNullOrEmpty(location))
            return;

        GameManager.Instance.UpdateRecordingLocation(location);
    }
}
