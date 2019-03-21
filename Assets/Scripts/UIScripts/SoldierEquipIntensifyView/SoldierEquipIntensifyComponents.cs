using UnityEngine;
using System.Collections.Generic;
public class SEIStarComponent : BaseComponent
{
    public UISprite ui_SelectSprite;

    public bool IsSelect 
    {
        set 
        {
            ui_SelectSprite.enabled = value;
        }
    }

    public SEIStarComponent(GameObject root) 
    {
        mRootObject = root;
        //base.AutoSetGoProperty<SEIStarComponent>(this, root);
    }
}

public class SEIAttComponent : BaseComponent
{
    public UISprite Spt_AttIcon;
    public UILabel Spt_AttDesc;

    public SEIAttComponent(GameObject root)
    {
        base.MyStart(root);
        Spt_AttIcon = mRootObject.transform.FindChild("AttIcon").GetComponent<UISprite>();
        Spt_AttDesc = mRootObject.transform.FindChild("AttDesc").GetComponent<UILabel>();
    }

    public void UpdateInfo(string name, string desc)
    {
        CommonFunction.SetSpriteName(Spt_AttIcon, name);
        Spt_AttIcon.MakePixelPerfect();
        Spt_AttDesc.text = desc;
    }

    public override void Clear()
    {
        base.Clear();
    }
}