using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using SLua;

public class LScrollPool {

    private GameObject prefab = null;
    private Transform poolRoot = null;
    private string cellClass = null;
    private LuaTable args = null;
    private int size = 10;

    private Stack<LPoolLuaObject> stack = new Stack<LPoolLuaObject> ();

    public LScrollPool (GameObject prefab, Transform poolRoot, string cellClass, LuaTable args) {
        this.prefab = prefab;
        this.poolRoot = poolRoot;
        this.cellClass = cellClass;
        this.args = args;
    }

    public void InitStack () {
        if (size > 0) {
            for (int i = 0; i < size; i++) {
                LPoolLuaObject pObj = NewObjectInstance ();
                pObj.SetParent (poolRoot);
                stack.Push (pObj);
            }
        }
    }

    private LPoolLuaObject NewObjectInstance () {
        GameObject go = GameObject.Instantiate (prefab) as GameObject;
        LPoolLuaObject pObj = go.AddComponent<LPoolLuaObject> ();
        pObj.SetParent (poolRoot);
        go.transform.localScale = new Vector3 (1, 1, 1);
        pObj.SetClass (this.cellClass, this.args);
        return pObj;
    }

    public LPoolLuaObject BorrowObject () {
        LPoolLuaObject po = null;
        if (stack.Count > 0) {
            po = stack.Pop ();
        } else {
            po = NewObjectInstance ();
        }
        return po;
    }

    public void ReturnObject (LPoolLuaObject obj) {
        obj.SetParent (poolRoot, false);
        obj.Release ();
        stack.Push (obj);
    }

}

// [CustomLuaClass]
// public class LPoolObject : MonoBehaviour{

//     private Text nameTxt = null;

//     private bool isPooled = true;

//     public void InitPanel () {
//         this.nameTxt = this.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text> ();
//     }

//     public void InitData (LPoolData data) {
//         nameTxt.text = data.name + " " + data.lev;
//     }

//     public void Release () {
//         gameObject.SetActive (false);
//         gameObject.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0);
//         isPooled = true;
//         nameTxt.text = "";
//     }

//     public void SetParent (Transform parent) {
//         this.gameObject.transform.SetParent (parent);
//     }

//     public GameObject GetGameObject () {
//         return gameObject;
//     }
// }

[CustomLuaClass]
public class LPoolLuaObject : MonoBehaviour {

    private LuaTableProxy table = null;
    private bool isPooled = true;
    private string luaClass = null;

    public void SetClass (string className, LuaTable args) {
        this.luaClass = className;
        table = new LuaTableProxy (className, this.gameObject, args);
    }

    public void InitPanel (LuaTable data) {
        table.CallMethod ("InitPanel", data);
        isPooled = false;
    }

    public void Release () {
        gameObject.SetActive (false);
        isPooled = true;
        table.CallMethod ("Release");
    }

    public void Refresh (LuaTable args) {
        table.CallMethod ("Refresh", args);
    }

    public void SetParent (Transform parent) {
        this.transform.SetParent (parent);
    }

    public void SetParent (Transform parent, bool worldPositionStays) {
        this.gameObject.transform.SetParent (parent, worldPositionStays);
    }

    public GameObject GetGameObject () {
        return gameObject;
    }
}
