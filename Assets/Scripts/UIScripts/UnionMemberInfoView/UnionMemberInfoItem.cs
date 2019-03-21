using UnityEngine;
using System.Collections;
using fogs.proto.msg;

public class UnionMemberInfoItem {

    private Transform root;
    private UISprite Spt_HeadIcon;
    private UISprite Spt_HeadFrame;
    private UILabel Lbl_LevelValue;
    private UILabel Lbl_ItemName;
    private UILabel Lbl_ItemActive;
    private UILabel Lbl_ItemJob;
    private UILabel Lbl_ItemOffLine;


    public UnionMemberInfoItem(Transform vRoot, UnionMember vData, string vName, Vector3 vPos)
    {
        root = vRoot;
        if (root != null)
        {
            Spt_HeadIcon = root.FindChild("ItemHeadInfo/HeadIcon").GetComponent<UISprite>();
            Spt_HeadFrame = root.FindChild("ItemHeadInfo/HeadFrame").GetComponent<UISprite>();
            Lbl_LevelValue = root.FindChild("ItemHeadInfo/LevelBack/LevelValue").GetComponent<UILabel>();
            Lbl_ItemName = root.FindChild("ItemName").GetComponent<UILabel>();
            Lbl_ItemActive = root.FindChild("ItemActive").GetComponent<UILabel>();
            Lbl_ItemJob = root.FindChild("ItemJob").GetComponent<UILabel>();
            Lbl_ItemOffLine = root.FindChild("ItemOffLine").GetComponent<UILabel>();
            RefreshItem(vData, vName, vPos);
        }
    }
    ~UnionMemberInfoItem()
    {
        if (root != null)
        {
            GameObject.Destroy(root.gameObject);
        }
    }


    public void InitItem()
    {
        if (Spt_HeadIcon != null)
        {
            Spt_HeadIcon.gameObject.SetActive(false);
        }
        if (Spt_HeadFrame != null)
        {
            Spt_HeadFrame.gameObject.SetActive(false);
        }
        if (Lbl_LevelValue != null)
        {
            Lbl_LevelValue.text = "";
        }
        if (Lbl_ItemName != null)
        {
            Lbl_ItemName.text = "";
        }
        if (Lbl_ItemActive != null)
        {
            Lbl_ItemActive.text = "";
        }
        if (Lbl_ItemJob != null)
        {
            Lbl_ItemJob.text = "";
        }
        if (Lbl_ItemOffLine != null)
        {
            Lbl_ItemOffLine.text = "";
        }
        if (root != null)
        {
            root.gameObject.SetActive(false);
        }
    }

    public void RefreshItem(UnionMember vData, string vName, Vector3 vPos)
    {
        InitItem();
        if (vData != null)
        {
            if (Spt_HeadIcon != null)
            {
                CommonFunction.SetHeadAndFrameSprite(Spt_HeadIcon, Spt_HeadFrame, vData.icon, vData.icon_frame, true);
                Spt_HeadIcon.gameObject.SetActive(true);
            }
            if (Lbl_LevelValue != null)
            {
                Lbl_LevelValue.text = vData.level.ToString();
            }
            if (Lbl_ItemName != null)
            {
                Lbl_ItemName.text = vData.charname;
            }
            if (Lbl_ItemActive != null)
            {
                Lbl_ItemActive.text = string.Format(ConstString.RAND_UNION_MEMBER_ACTIVE, vData.vitality);
            }
            if (Lbl_ItemJob != null)
            {
                Lbl_ItemJob.text = string.Format(ConstString.FORMAT_UNION_JOB, CommonFunction.GetUnionMemberJobString(vData.position));
            }
            if (Lbl_ItemOffLine != null)
            {
                CommonFunction.SetOfflineTime(Lbl_ItemOffLine, vData.offline_tick, false);
            }
            if (root != null)
            {
                root.name = vName;
                root.localPosition = vPos;
                root.gameObject.SetActive(true);
            }
        }
    }

}