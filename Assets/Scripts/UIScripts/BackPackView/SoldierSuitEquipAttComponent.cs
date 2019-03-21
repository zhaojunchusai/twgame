using UnityEngine;
using System.Collections.Generic;
public class SoldierSuitEquipAttComponent : BaseComponent
{
    private UILabel Lbl_Desc;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Lbl_Desc = mRootObject.transform.FindChild("Label").GetComponent<UILabel>();
    }


    public void UpdateDesc(string desc) 
    {
        Lbl_Desc.text = desc;
    }
}
