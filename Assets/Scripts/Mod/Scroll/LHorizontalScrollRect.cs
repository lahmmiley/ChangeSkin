using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using SLua;

/// <summary>
/// 水平滚动
/// </summary>

[CustomLuaClass]
public class LHorizontalScrollRect : LScrollRect {

    protected override float GetSize (RectTransform item) {
        item.GetLocalCorners (fourCornersArray);
        float size = GetDimension (fourCornersArray[2]) - GetDimension (fourCornersArray[0]);
        return size + contentSpacing;
    }

    protected override float GetSize (Vector3[] fourCornersArray) {
        float size = GetDimension (fourCornersArray[2]) - GetDimension (fourCornersArray[0]);
        return size;
    }

    protected override float GetDimension (Vector2 vector) {
        return vector.x;
    }

    protected override float GetDimension (Vector3 vector) {
        return vector.x;
    }

    protected override Vector2 GetVector (float value) {
        return new Vector2 (-value, 0);
    }

    protected override RectTransform.Axis GetAxis () {
        return RectTransform.Axis.Horizontal;
    }

    protected override void SetCellAnchor (RectTransform rect) {
        rect.anchorMin = new Vector2 (0, 0.5f);
        rect.anchorMax = new Vector2 (0, 0.5f);
    }

    protected override void SetCellPivot (RectTransform rect) {
        rect.pivot = new Vector2 (0, 0.5f);
    }

    protected override void SetAnchorPosition (RectTransform rect, float offset) {
        rect.anchoredPosition = new Vector2 (offset, 0);
    }

    protected override Vector2 GetOffset (float offset) {
        return new Vector2 (offset, 0);
    }

    protected override void Awake () {
        base.Awake ();
        directionSign = 1;
        horizontal = true;
        vertical = false;
    }
}