using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using SLua;

/// <summary>
/// 按钮效果
/// </summary>
[CustomLuaClass]
public class TransitionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private Vector3 originScale = new Vector3(1, 1, 1);
    private bool isstart = false;

    // 是否缩放
    public bool scaleSetting = true;
    // 缩放比率
    public float scaleRate = 1.1f;

    // 是否有音效
    public bool soundSetting = true;
    // 音效ID
    public int soundId = 214;

    void OnEnable () {
        if (isstart) {
            transform.localScale = originScale;
        }
    }

    void Start () {
        originScale = transform.localScale;
        isstart = true;
    }

    public void OnPointerDown (PointerEventData eventData) {
        // 这里与client的为准
    }

    public void OnPointerUp (PointerEventData eventData) {
        // 这里与client的为准
    }
}
