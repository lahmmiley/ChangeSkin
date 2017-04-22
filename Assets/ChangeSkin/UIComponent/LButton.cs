using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_5_3
using DG.Tweening;
#endif
using UnityEngine;

namespace UIComponent
{
    public class LButton : Button, IPointerClickHandler, IPointerDownHandler
    {
        private Vector2 _prePivot;
        private Vector2 _prePosition;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            RectTransform rect = GetRectTransform();
            _prePivot = rect.pivot;
            _prePosition = rect.anchoredPosition;
            rect.pivot = Vector2.one * 0.5f;
            rect.anchoredPosition = new Vector2(_prePosition.x + rect.sizeDelta.x * 0.5f, _prePosition.y - rect.sizeDelta.y * 0.5f);
            #if UNITY_5_3
            rect.DOScale(1.05f, 0.1f);
            #endif
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            RectTransform rect = GetRectTransform();
            rect.pivot = _prePivot;
            rect.anchoredPosition = _prePosition;
            #if UNITY_5_3
            rect.DOScale(1f, 0.1f);
            #endif
        }

        private RectTransform _rect;
        private RectTransform GetRectTransform()
        {
            if(_rect == null)
            {
                _rect = gameObject.GetComponent<RectTransform>();
            }
            return _rect;
        }
    }
}
