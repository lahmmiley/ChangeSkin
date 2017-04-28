using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using SLua;

/// <summary>
/// 垂直滚动
/// </summary>

[CustomLuaClass]
public class LVerticalScrollRect : LScrollRect {

    protected override float GetSize (RectTransform item) {
        item.GetLocalCorners (fourCornersArray);
        float size = GetDimension (fourCornersArray[1]) - GetDimension (fourCornersArray[3]);
        return size + contentSpacing;
    }

    protected override float GetSize (Vector3[] fourCornersArray) {
        float size = GetDimension (fourCornersArray[1]) - GetDimension (fourCornersArray[3]);
        return size;
    }

    protected override float GetDimension (Vector2 vector) {
        return vector.y;
    }

    protected override float GetDimension (Vector3 vector) {
        return vector.y;
    }

    protected override Vector2 GetVector (float value) {
        return new Vector2 (0, value);
    }

    protected override RectTransform.Axis GetAxis () {
        return RectTransform.Axis.Vertical;
    }

    protected override void SetCellAnchor (RectTransform rect) {
        rect.anchorMin = new Vector2 (0.5f, 1f);
        rect.anchorMax = new Vector2 (0.5f, 1f);
    }

    protected override void SetCellPivot (RectTransform rect) {
        rect.pivot = new Vector2 (0.5f, 1f);
    }

    protected override void SetAnchorPosition (RectTransform rect, float offset) {
        rect.anchoredPosition = new Vector2 (0, 0 - offset);
    }

    protected override Vector2 GetOffset (float offset) {
        return new Vector2 (0, 0 - offset);
    }

    protected override void Awake () {
        base.Awake ();
        directionSign = -1;
        horizontal = false;
        vertical = true;
    }
}
