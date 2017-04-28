using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using SLua;

[CustomLuaClass]
public class ButtonEnterEvent : UnityEvent {
}
[CustomLuaClass]
public class ButtonExsitDragEvent : UnityEvent {
}

[CustomLuaClass]
public class CustomEnterExsitButton : CustomButton, IPointerEnterHandler, IPointerExitHandler{


    private ButtonEnterEvent m_OnEnter = new ButtonEnterEvent();
    private ButtonExsitDragEvent m_OnExsit = new ButtonExsitDragEvent();

    /// <summary>
    /// 开始拖动事件
    /// </summary>
    public ButtonEnterEvent onEnter {
        get {
            return m_OnEnter;
        }
        set {
            m_OnEnter = value;
        }
    }

    /// <summary>
    /// 拖动ing事件
    /// </summary>
    public ButtonExsitDragEvent onExsit {
        get {
            return m_OnExsit;
        }
        set {
            m_OnExsit = value;
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData) {
        m_OnEnter.Invoke();
    }

    public virtual void OnPointerExit(PointerEventData eventData) {
        m_OnExsit.Invoke();
    }
}
