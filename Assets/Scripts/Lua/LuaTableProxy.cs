using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using SLua;

public class LuaTableProxy {

    private LuaState luaState = null;
    private LuaTable chunk = null;

    private Dictionary<string, LuaFunction> funcDict = new Dictionary<string, LuaFunction> ();

    public LuaTableProxy (string className) {
        if (string.IsNullOrEmpty (className)) {
            throw new Exception (string.Format ("LuaTableProcy实例化失败, className为空:{0}", className));
        }
        // luaState = LuaSrvManager.GetInstance().LuaState;
        try {
            LuaTable clsTable = (LuaTable)luaState[className];
            if (clsTable == null) {
                throw new Exception (string.Format ("LuaTableProcy实例化失败, {0}类没有定义", className));
            }
            LuaFunction newFunc = (LuaFunction)clsTable["New"];
            if (newFunc == null) {
                throw new Exception (string.Format ("LuaTableProcy实例化失败, {0}类没有定义New方法", className));
            }
            chunk = (LuaTable)newFunc.call ();
        } catch (Exception e) {
            throw new Exception (string.Format ("LuaTableProcy实例化{0}出错了：{1}", className, e.Message));
        }
    }

    public LuaTableProxy (string className, object aParam) {
        if (string.IsNullOrEmpty (className)) {
            throw new Exception (string.Format ("LuaTableProcy实例化失败, className为空:{0}", className));
        }
        // luaState = LuaSrvManager.GetInstance ().LuaState;
        try {
            LuaTable clsTable = (LuaTable)luaState[className];
            if (clsTable == null) {
                throw new Exception (string.Format ("LuaTableProcy实例化失败, {0}类没有定义", className));
            }
            LuaFunction newFunc = (LuaFunction)clsTable["New"];
            if (newFunc == null) {
                throw new Exception (string.Format ("LuaTableProcy实例化失败, {0}类没有定义New方法", className));
            }
            chunk = (LuaTable)newFunc.call (aParam);
        } catch (Exception e) {
            throw new Exception (string.Format ("LuaTableProcy实例化{0}出错了：{1}", className, e.Message));
        }
    }

    public LuaTableProxy (string className, object aParam1, object aParam2) {
        if (string.IsNullOrEmpty (className)) {
            throw new Exception (string.Format ("LuaTableProcy实例化失败, className为空:{0}", className));
        }
        // luaState = LuaSrvManager.GetInstance ().LuaState;
        try {
            LuaTable clsTable = (LuaTable)luaState[className];
            if (clsTable == null) {
                throw new Exception (string.Format ("LuaTableProcy实例化失败, {0}类没有定义", className));
            }
            LuaFunction newFunc = (LuaFunction)clsTable["New"];
            if (newFunc == null) {
                throw new Exception (string.Format ("LuaTableProcy实例化失败, {0}类没有定义New方法", className));
            }
            chunk = (LuaTable)newFunc.call (aParam1, aParam2);
        } catch (Exception e) {
            throw new Exception (string.Format ("LuaTableProcy实例化{0}出错了：{1}", className, e.Message));
        }
    }

    ~LuaTableProxy () {
        if (chunk != null) {
            chunk.Dispose ();
            chunk = null;
        }
        foreach (LuaFunction func in funcDict.Values) {
            func.Dispose ();
        }
    }

    public bool Valid {
        get {
            return (chunk != null);
        }
    }

    public LuaTable GetChunk () {
        return Valid ? chunk : null;
    }

    public void SetData (string strName, object cValue) {
        if (!Valid || string.IsNullOrEmpty (strName)) {
            return;
        }
        chunk[strName] = cValue;
    }

    public void SetData (string strName, object[] cArrayValue) {
        if (!Valid) {
            return;
        }

        if (string.IsNullOrEmpty (strName) || (null == cArrayValue) || (0 == cArrayValue.Length)) {
            return;
        }

        LuaTable cSubTable = (LuaTable)chunk[strName];
        if (null == cSubTable) {
            return;
        }

        int nLength = cArrayValue.Length;
        for (int i = 0; i < nLength; i++) {
            cSubTable[i + 1] = cArrayValue[i];
        }
    }

    public void SetData (int nIndex, object cValue) {
        if (!Valid || (nIndex < 1)) {
            return;
        }

        chunk[nIndex] = cValue;
    }

    public void SetData (int nIndex, object[] cArrayValue) {
        if (!Valid) {
            return;
        }

        if ((nIndex < 1) || (null == cArrayValue) || (0 == cArrayValue.Length)) {
            return;
        }

        LuaTable cSubTable = (LuaTable)chunk[nIndex];
        if (null == cSubTable) {
            return;
        }

        int nLength = cArrayValue.Length;
        for (int i = 0; i < nLength; i++) {
            cSubTable[i + 1] = cArrayValue[i];
        }
    }

    public object GetData (string strName) {
        if (!Valid || string.IsNullOrEmpty (strName)) {
            return null;
        }

        return chunk[strName];
    }

    public object GetData (int nIndex) {
        if (!Valid || (nIndex < 1)) {
            return null;
        }

        return chunk[nIndex];
    }

    private LuaFunction GetMethod (string strFunc) {
        if (funcDict.ContainsKey (strFunc)) {
            return funcDict[strFunc];
        } else {
            return null;
        }
    }

    public object CallMethod (string strFunc) {
        LuaFunction cResFunc = GetMethod (strFunc);
        if (null == cResFunc) {
            if (string.IsNullOrEmpty (strFunc)) {
                return null;
            }
            if (!Valid) {
                return null;
            }

            object cFuncObj = chunk[strFunc];
            if ((null == cFuncObj) || !(cFuncObj is LuaFunction)) {
                return null;
            }

            cResFunc = (LuaFunction)cFuncObj;
            if (null == cResFunc) {
                return null;
            }
            funcDict[strFunc] = cResFunc;
        }

        try {
            return cResFunc.call (chunk);
        } catch (Exception e) {
            Debug.LogException (e);
            cResFunc = null;
            return null;
        }
    }

    public object CallMethod (string strFunc, object cParam) {
        LuaFunction cResFunc = GetMethod (strFunc);
        if (null == cResFunc) {
            if (string.IsNullOrEmpty (strFunc)) {
                return null;
            }

            if (!Valid) {
                return null;
            }

            object cFuncObj = chunk[strFunc];
            if ((null == cFuncObj) || !(cFuncObj is LuaFunction)) {
                return null;
            }

            cResFunc = (LuaFunction)cFuncObj;
            if (null == cResFunc) {
                return null;
            }
            funcDict[strFunc] = cResFunc;
        }

        try {
            return cResFunc.call (chunk, cParam);
        } catch (Exception e) {
            Debug.LogException (e);
            cResFunc = null;
            return null;
        }
    }

    public object CallMethod (string strFunc, object cParam1, object cParam2) {
        LuaFunction cResFunc = GetMethod (strFunc);
        if (null == cResFunc) {
            if (string.IsNullOrEmpty (strFunc)) {
                return null;
            }

            if (!Valid) {
                return null;
            }

            object cFuncObj = chunk[strFunc];
            if ((null == cFuncObj) || !(cFuncObj is LuaFunction)) {
                return null;
            }

            cResFunc = (LuaFunction)cFuncObj;
            if (null == cResFunc) {
                return null;
            }
            funcDict[strFunc] = cResFunc;
        }

        try {
            return cResFunc.call (chunk, cParam1, cParam2);
        } catch (Exception e) {
            Debug.LogException (e);
            cResFunc = null;
            return null;
        }
    }

    public object CallMethod (string strFunc, object cParam1, object cParam2, object cParam3) {
        LuaFunction cResFunc = GetMethod (strFunc);
        if (null == cResFunc) {
            if (string.IsNullOrEmpty (strFunc)) {
                return null;
            }

            if (!Valid) {
                return null;
            }

            object cFuncObj = chunk[strFunc];
            if ((null == cFuncObj) || !(cFuncObj is LuaFunction)) {
                return null;
            }

            cResFunc = (LuaFunction)cFuncObj;
            if (null == cResFunc) {
                return null;
            }
            funcDict[strFunc] = cResFunc;
        }

        try {
            return cResFunc.call (chunk, cParam1, cParam2, cParam3);
        } catch (Exception e) {
            Debug.LogException (e);
            cResFunc = null;
            return null;
        }
    }

    public object CallMethod (string strFunc, params object[] aParams) {
        LuaFunction cResFunc = GetMethod (strFunc);
        if (null == cResFunc) {
            if (string.IsNullOrEmpty (strFunc)) {
                return null;
            }

            if (!Valid) {
                return null;
            }

            object cFuncObj = chunk[strFunc];
            if ((null == cFuncObj) || !(cFuncObj is LuaFunction)) {
                return null;
            }

            cResFunc = (LuaFunction)cFuncObj;
            if (null == cResFunc) {
                return null;
            }
            funcDict[strFunc] = cResFunc;
        }

        try {
            if (null == aParams) {
                return cResFunc.call (chunk);
            } else {
                return cResFunc.call (chunk, aParams);
            }
        } catch (Exception e) {
            Debug.LogException (e);
            cResFunc = null;
            return null;
        }
    }
}
