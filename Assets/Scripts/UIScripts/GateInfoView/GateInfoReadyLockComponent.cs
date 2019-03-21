using UnityEngine;
using System.Collections.Generic;
public class GateInfoReadyLockComponent : BaseComponent
{
    public UILabel Lbl_LockLevel;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Lbl_LockLevel = mRootObject.transform.FindChild("LockLevel").GetComponent<UILabel>();
    }

    public void UpdateInfo(string text) 
    {
        Lbl_LockLevel.text = string.Format(ConstString.BACKPACK_GATELOCKTIP, text);
    }
}
