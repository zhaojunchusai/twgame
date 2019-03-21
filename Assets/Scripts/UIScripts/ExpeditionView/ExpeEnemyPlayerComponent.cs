using System.Collections;
using UnityEngine;
using fogs.proto.msg;

public class ExpeEnemyPlayerComponent : BaseComponent
{
    public UILabel Lbl_LevelLabel;
    public UISprite Spt_IconTexture;
    public UISprite Spt_QualitySprite;
    public UISprite Spt_PlayerBg;

    public ExpeEnemyPlayerComponent(GameObject root)
    {
        base.MyStart(root);
        Spt_PlayerBg = mRootObject.transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        Spt_QualitySprite = mRootObject.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("IconTexture").gameObject.GetComponent<UISprite>();
        Lbl_LevelLabel = mRootObject.transform.FindChild("LevelBG/LevelLabel").gameObject.GetComponent<UILabel>();
       
    }

    public void UpdateInfo(uint icon, uint frame, int level)
    {
        Lbl_LevelLabel.text = level.ToString();
        // CommonFunction.SetSpriteName(Spt_IconTexture, icon);
        CommonFunction.SetHeadAndFrameSprite(Spt_IconTexture, Spt_QualitySprite, icon, frame, true);
    }

    public override void Clear()
    {
        base.Clear();
        Lbl_LevelLabel.text = string.Empty;
        //Spt_IconTexture.mainTexture = null;
    }
}


