using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using SLua;

[CustomLuaClass]
public class ButtonClickedEvent : UnityEvent {}
[CustomLuaClass]
public class ButtonBeginDragEvent : UnityEvent {}
[CustomLuaClass]
public class ButtonDragEvent : UnityEvent {}
[CustomLuaClass]
public class ButtonEndDragEvent : UnityEvent {}

[CustomLuaClass]
public class CustomDragButton : Button,IBeginDragHandler,IDragHandler,IEndDragHandler {


    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

    private ButtonBeginDragEvent m_OnBeginDrag = new ButtonBeginDragEvent();
    private ButtonDragEvent m_OnDrag = new ButtonDragEvent();
    private ButtonEndDragEvent m_OnEndDrag = new ButtonEndDragEvent();

    private bool isdown = false;
    private bool m_holdtrigger = false;
    private float m_stamp = 0;
    private WaitForSeconds ws = new WaitForSeconds(0.6f);

    /// <summary>
    /// 点击事件
    /// </summary>
    public ButtonClickedEvent onClickCustom {
        get {
            return m_OnClick;
        }
        set {
            m_OnClick = value;
        }
    }

    /// <summary>
    /// 开始拖动事件
    /// </summary>
    public ButtonBeginDragEvent onBeginDrag {
        get {
            return m_OnBeginDrag;
        }
        set {
            m_OnBeginDrag = value;
        }
    }

    /// <summary>
    /// 拖动ing事件
    /// </summary>
    public ButtonDragEvent onDrag {
        get {
            return m_OnDrag;
        }
        set {
            m_OnDrag = value;
        }
    }

    /// <summary>
    /// 拖动结束事件
    /// </summary>
    public ButtonEndDragEvent onEndDrag {
        get {
            return m_OnEndDrag;
        }
        set {
            m_OnEndDrag = value;
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData) {
        m_OnBeginDrag.Invoke();
    }

    public virtual void OnDrag(PointerEventData eventData) {
        m_OnDrag.Invoke();
    }

    public virtual void OnEndDrag(PointerEventData eventData) {
        m_OnEndDrag.Invoke();
    }

    public virtual void OnPointerClick(PointerEventData eventData) {
        if (eventData.button != PointerEventData.InputButton.Left || m_holdtrigger)
            return;

        m_OnClick.Invoke();
    }
}
