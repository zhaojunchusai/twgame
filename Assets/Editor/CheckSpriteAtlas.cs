using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

public class CheckSpriteAtlas {

    [MenuItem("Assets/CheckSpriteAtlas/Check Sprite Atlas")]
    static void CheckSprite()
    {
        CheckWholeSprite(Selection.activeTransform);
    }

    static void CheckWholeSprite(Transform vTrans)
    {
        if (vTrans == null)
            return;
        for (int i = 0; i < vTrans.childCount; i++)
        {
            if ((vTrans.GetChild(i).GetComponent<UISprite>() != null) && (vTrans.GetChild(i).GetComponent<UISprite>().atlas != null))
            {
                if (!vTrans.GetChild(i).GetComponent<UISprite>().atlas.name.Contains("_New_"))
                    Debug.LogWarning(string.Format("[{0}]: [{1}], [{2}], [{3}]", 
                        vTrans.name, vTrans.GetChild(i).name, 
                        vTrans.GetChild(i).GetComponent<UISprite>().atlas.name,
                        vTrans.GetChild(i).GetComponent<UISprite>().spriteName));
            }
            CheckWholeSprite(vTrans.GetChild(i));
        }
    }
}
