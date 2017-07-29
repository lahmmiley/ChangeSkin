using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NewOldComparer {
    private static string mAddMsg; //Prefab新增信息
    private static string mLackMsg; //Prefab缺少信息
    private static string mLackComponentMsg; //组件缺失信息
    private static List<string> mNeededComponents;
    private static Transform mOriginOld;
    private static Transform mOriginNew;

	public static void Compare(GameObject oldPrefab, GameObject newPrefab)
    {
        Transform oldTrans = oldPrefab.GetComponent<Transform>();
        Transform newTrans = newPrefab.GetComponent<Transform>();
        Init(oldTrans, newTrans);

        CompareChild(oldTrans, newTrans);

        PrintMsg();
    }

    private static void Init(Transform oldTrans, Transform newTrans)
    {
        mAddMsg = mLackMsg = mLackComponentMsg = "\n";

        mOriginOld = oldTrans;
        mOriginNew = newTrans;

        mNeededComponents = new List<string>();
        mNeededComponents.Add("Text");
        mNeededComponents.Add("Image");
        mNeededComponents.Add("Mask");
        mNeededComponents.Add("Button");
        mNeededComponents.Add("TransitionButton");
        mNeededComponents.Add("CustomButton");
        mNeededComponents.Add("CustomEnterExsitButton");
        mNeededComponents.Add("ScrollRect");
        mNeededComponents.Add("Toggle");
        mNeededComponents.Add("ToggleGroup");
        mNeededComponents.Add("Input");
        mNeededComponents.Add("Canvas");
        mNeededComponents.Add("VerticalGroupLayout");
        mNeededComponents.Add("HorizontalGroupLayout");
        mNeededComponents.Add("ContentSizeFitter");
        mNeededComponents.Add("LayoutElement");
    }

    private static void PrintMsg()
    {
        Debug.Log("新Prefab缺少：" + mLackMsg);
        Debug.Log("新Prefab新增：" + mAddMsg);
        Debug.Log("新Prefab组件缺失情况：" + mLackComponentMsg);
        Debug.Log("比较新旧Prefab结束");
    }

    private static void CompareChild(Transform oldTrans, Transform newTrans)
    {
        List<Transform> lackList = new List<Transform>();
        List<Transform> addList = new List<Transform>();
        List<Transform> commonOldList = new List<Transform>();
        List<Transform> commonNewList = new List<Transform>();

        for (int i = 0; i < oldTrans.childCount; i++)
            lackList.Add(oldTrans.GetChild(i));
        for (int i = 0; i < newTrans.childCount; i++)
            addList.Add(newTrans.GetChild(i));

        for (int i = 0; i < oldTrans.childCount; i++)
        {
            Transform child = oldTrans.GetChild(i);
            Transform childInNewTrans = newTrans.FindChild(child.name);
            if (childInNewTrans != null)
            {
                lackList.Remove(child);
                addList.Remove(childInNewTrans);
                commonOldList.Add(child);
                commonNewList.Add(childInNewTrans);
            }
        }

        for (int i = 0; i < lackList.Count; i++)
        {
            string oldPath = GetHierarchyPath(mOriginOld, oldTrans);
            mLackMsg += oldPath + "/" + lackList[i].name + "\n";
        }

        for (int i = 0; i < addList.Count; i++)
        {
            string newPath = GetHierarchyPath(mOriginNew, newTrans);
            mAddMsg += newPath + "/" + addList[i].name + "\n";
        }

        for (int i = 0; i < commonNewList.Count; i++)
        {
            CompareComponent(commonOldList[i], commonNewList[i]);
            CompareChild(commonOldList[i], commonNewList[i]);
        }
    }

    private static void CompareComponent(Transform oldTrans, Transform newTrans)
    {
        string newPath = GetHierarchyPath(mOriginNew, newTrans);
        foreach(string component in mNeededComponents)
        {
            if (oldTrans.GetComponent(component) != null && newTrans.GetComponent(component) == null)
            {
                mLackComponentMsg += newPath + "缺少" + component + "组件\n";
            }
        }
    } 

    private static string GetHierarchyPath(Transform top, Transform trans)
    {
        if (top == null || trans == null)
            return string.Empty;

        string path = string.Empty;
        Transform cur = trans;
        while(cur != top && cur != cur.root)
        {
            path = (path == string.Empty) ? (cur.name) : (cur.name + "/" + path);
            cur = cur.parent;
        }
        if (cur == cur.root && cur != top)
        {
            Debug.Log(trans.name + "与" + top.name + "不在一个Hierarchy中");
            return string.Empty;
        }

        path = top.name + "/" + path;
        return path;
    }
}
