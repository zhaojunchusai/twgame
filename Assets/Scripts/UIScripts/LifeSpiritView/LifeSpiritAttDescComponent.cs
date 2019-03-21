using System.Collections.Generic;
using UnityEngine;
public class LifeSpiritAttDescComponent : BaseComponent
{
    private UILabel Lbl_Title;
    private UILabel Lbl_AttDesc;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Lbl_Title = mRootObject.transform.FindChild("Title").GetComponent<UILabel>();
        Lbl_AttDesc = mRootObject.transform.FindChild("Att").GetComponent<UILabel>();
    }


    public void UpdateCompInfo(string title, string att)
    {
        Lbl_Title.text = title.ToString();
        Lbl_AttDesc.text = att.ToString();
    }

}
