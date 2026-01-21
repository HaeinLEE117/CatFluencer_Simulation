using System;
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

    private UI_Base _activePopup;
    private bool _wasPopupOpen;
    private readonly Dictionary<string, UI_Base> _popups = new Dictionary<string, UI_Base>();
    private readonly Stack<UI_Base> _popupStack = new Stack<UI_Base>();

    // 동적 정렬 순서 관리용
    private int _currentTopOrder = 1000; // 시작값은 충분히 큰 값으로 설정

    public T ShowPopupUI<T>(string name = null) where T : UI_Base, IUI_Popup
    {
        return ShowPopupUI<T>(name, closeCurrent: false);
    }

    public T ShowPopupUI<T>(string name, bool closeCurrent) where T : UI_Base, IUI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        if (closeCurrent)
        {
            CloseAllPopupUI();
        }
        else if (_activePopup != null)
        {
            _popupStack.Push(_activePopup);
            // 이전 활성 팝업은 그대로 스택에 보존하고 가시성만 끔
            SetPopupActive(_activePopup, false, _currentTopOrder);
        }

        if (_popups.TryGetValue(name, out UI_Base popup) == false)
        {
            GameObject go = ResourceManager.Instance.Instantiate(name);
            popup = Utils.GetOrAddComponent<T>(go);
            _popups[name] = popup;
        }

        _activePopup = popup;
        _wasPopupOpen = true;

        popup.transform.SetParent(PopupRoot);

        // 새 팝업을 최상단으로 올림
        _currentTopOrder++;
        SetPopupActive(popup, true, _currentTopOrder);

        if (SceneManager.Instance.CurrentSceneType == Define.EScene.DevScene)
        {
            GameManager.Instance.StopTime();
        }

        EventManager.Instance.TriggerEvent(Define.EEventType.UI_PopupOpened);
        return popup as T;
    }

    public UI_Base ShowPopupUI(string name)
    {
        return ShowPopupUI(name, closeCurrent: true);
    }

    public UI_Base ShowPopupUI(string name, bool closeCurrent)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        if (closeCurrent)
        {
            CloseAllPopupUI();
        }
        else if (_activePopup != null)
        {
            _popupStack.Push(_activePopup);
            SetPopupActive(_activePopup, false, _currentTopOrder);
        }

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

        _currentTopOrder++;
        SetPopupActive(popup, true, _currentTopOrder);

        if (SceneManager.Instance.CurrentSceneType == Define.EScene.DevScene)
        {
            GameManager.Instance.StopTime();
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
        if (_activePopup == null)
            return;

        // 현재 팝업 비활성화
        var closing = _activePopup;
        _activePopup = null;

        SetPopupActive(closing, false, _currentTopOrder);

        // 최상단 오더 감소
        if (_currentTopOrder > 0)
            _currentTopOrder--;

        // 스택에 있으면 복원
        if (_popupStack.Count > 0)
        {
            _activePopup = _popupStack.Pop();
            // 복원되는 팝업은 현재 최상단 오더로 보이도록
            SetPopupActive(_activePopup, true, _currentTopOrder);
        }
        else
        {
            if (SceneManager.Instance.CurrentSceneType == Define.EScene.DevScene)
            {
                GameManager.Instance.StartTime();
            }
            _wasPopupOpen = false;
        }

        EventManager.Instance.TriggerEvent(Define.EEventType.UI_PopupClosed);
    }

    public void CloseAllPopupUI()
    {
        // 활성 팝업/스택 모두 닫기
        if (_activePopup != null)
        {
            SetPopupActive(_activePopup, false, _currentTopOrder);
            _activePopup = null;
        }
        while (_popupStack.Count > 0)
        {
            var p = _popupStack.Pop();
            SetPopupActive(p, false, _currentTopOrder);
        }

        // 오더 초기화
        _currentTopOrder = 1000;

        if (SceneManager.Instance.CurrentSceneType == Define.EScene.DevScene)
        {
            GameManager.Instance.StartTime();
        }

        _wasPopupOpen = false;
        EventManager.Instance.TriggerEvent(Define.EEventType.UI_PopupClosed);
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

    public void NotifyLocationSelected(int locationID)
    {
        GameManager.Instance.UpdateRecordingLocation(locationID);
    }

    #region 대화창 팝업
    internal void ShowChatPopup(string titleText, string commentText, string npcNameText, Action onClosed = null)
    {
        var chat = ShowPopupUI<UI_ChatPopup>(nameof(UI_ChatPopup), closeCurrent: false);
        if (chat != null)
        {
            chat.Configure(titleText, commentText, npcNameText);
            chat.SetOnClosed(onClosed);
        }
    }
    #endregion

    public void ShowConfirmPopup(string titleText,string message, Action onConfirm, Action onCancel = null)
    {
        var popup = ShowPopupUI<UI_ConfirmPopup>(null, closeCurrent: false);
        if (popup == null)
            return;
        popup.SetTitle(titleText);
        popup.SetMessage(message);
        popup.SetActions(onConfirm, onCancel, confirmLabel: null, cancelLabel: null);
    }

    // 정렬순서 인자로 받아 활성/비활성 처리
    private void SetPopupActive(UI_Base popup, bool active, int sortingOrder)
    {
        if (popup == null) return;

        if (popup is UI_Toolkit toolkitUI)
        {
            var doc = toolkitUI.GetComponent<UIDocument>();
            if (doc != null)
            {
                doc.sortingOrder = sortingOrder;
                doc.rootVisualElement.visible = active;
            }
        }
        else
        {
            var canvas = popup.GetComponent<Canvas>();
            if (canvas != null)
                canvas.sortingOrder = sortingOrder;

            popup.gameObject.SetActive(active);
        }

        if (active)
        {
            popup.transform.SetParent(PopupRoot);
        }
    }
}
