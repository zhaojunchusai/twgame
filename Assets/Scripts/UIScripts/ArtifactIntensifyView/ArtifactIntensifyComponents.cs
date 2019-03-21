using System.Collections.Generic;
using UnityEngine;
using fogs.proto.msg;
public class ArtifactIntensifyAttComponent : BaseComponent
{
    public UISprite Spt_AttIcon;
    public UILabel Spt_AttDesc;

    public ArtifactIntensifyAttComponent(GameObject root)
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


public class ArtifactIntensifyMaterialComponent : ItemBaseComponent
{
    public UILabel Lbl_CountLabel;
    private UISprite Spt_BgSprite;
    private ItemInfo itemInfo;
    public ArtifactIntensifyMaterialComponent(GameObject root)
    {
        base.MyStart(root);
        Spt_QualitySprite = mRootObject.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("IconTexture").gameObject.GetComponent<UISprite>();
        Lbl_CountLabel = mRootObject.transform.FindChild("CountLabel").gameObject.GetComponent<UILabel>();
        Spt_BgSprite = mRootObject.transform.FindChild("BgSprite").gameObject.GetComponent<UISprite>();

    }

    public void UpdateInfo(fogs.proto.msg.Item item, int needCount)
    {
        if (item == null)
        {
            Clear();
            return;
        }
        itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(item.id);
        if (itemInfo == null)
            return;
        base.UpdateInfo(itemInfo.icon, itemInfo.quality);
        UpdateCount(needCount, item.num);

        UIEventListener.Get(this.mRootObject).onClick = (GameObject go) =>
        {
            if (CommonFunction.CheckIsOpen(OpenFunctionType.GetPath, true))
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETPATH);
                UISystem.Instance.GetPathView.UpdateViewInfo(itemInfo.id, 1);
            }
        };
    }
    private void UpdateCount(int needCount, int num)
    {
        Lbl_CountLabel.text = string.Format("{0}/{1}", num.ToString(), needCount.ToString());

        if (needCount <= num)
        {
            Lbl_CountLabel.color = Color.green;
        }
        else
        {
            Lbl_CountLabel.color = Color.red;
        }
    }

    public override void Clear()
    {
        base.Clear();
    }
}
