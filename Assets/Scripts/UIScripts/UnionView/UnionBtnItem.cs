using UnityEngine;
using System.Collections;
using Assets.Script.Common;

public class UnionBtnItem {

    public BoxCollider Box_Root;
    public UISprite Spt_Icon;
    public UISprite Spt_HintBG;
    public UILabel Lbl_HintInfo;
    public UISprite Spt_Lock;
    public UISprite Spt_Name;
    public UISprite Spt_Notice;

    private Transform rootTrans;
    private UIPanel rootPanel;
    private SkeletonAnimation skeletonAnimation;
    private Transform effect;

    public UnionBtnItem(Transform vRootTrans, UIPanel vPanel)
    {
        rootTrans = vRootTrans;
        rootPanel = vPanel;
        InitItemComponent();
        if (Spt_Icon != null)
        {
            skeletonAnimation = Spt_Icon.GetComponentInChildren<SkeletonAnimation>();
            effect = Spt_Icon.transform.FindChild("Effect");
        }
        else
        {
            skeletonAnimation = null;
            effect = null;
        }
        RefreshInfos("", true, false);
    }

    private void InitItemComponent()
    {
        if (rootTrans != null)
        {
            Box_Root = rootTrans.gameObject.GetComponent<BoxCollider>();
            Spt_Icon = rootTrans.FindChild("Icon").gameObject.GetComponent<UISprite>();
            Spt_HintBG = rootTrans.FindChild("HintBG").gameObject.GetComponent<UISprite>();
            Lbl_HintInfo = rootTrans.FindChild("HintBG/HintInfo").gameObject.GetComponent<UILabel>();
            Spt_Lock = rootTrans.FindChild("Lock").gameObject.GetComponent<UISprite>();
            Spt_Name = rootTrans.FindChild("Name").gameObject.GetComponent<UISprite>();
            Spt_Notice = rootTrans.FindChild("Notice").gameObject.GetComponent<UISprite>();
        }
    }

    public void RefreshInfos(string vHint, bool vIsLock, bool vIsNotice)
    {
        RefreshHintInfo(vHint);
        RefreshLockStatus(vIsLock);
        RefreshNoticeStatus(vIsNotice);
        RefreshSkeletonOrder();
    }
    public void RefreshHintInfo(string vHint)
    {
        if (Lbl_HintInfo != null)
        {
            if (!string.IsNullOrEmpty(vHint))
            {
                Lbl_HintInfo.text = vHint;
                Spt_HintBG.gameObject.SetActive(true);
                RefreshHintStatus(true);
            }
            else
            {
                RefreshHintStatus(false);
            }
        }
    }
    public void RefreshHintStatus(bool vIsShowHint)
    {
        if (Spt_HintBG != null)
        {
            Spt_HintBG.gameObject.SetActive(vIsShowHint);
        }
    }
    public void RefreshLockStatus(bool vIsLock)
    {
        if (Spt_Lock != null)
        {
            //Spt_Lock.gameObject.SetActive(vIsLock);
            Spt_Lock.gameObject.SetActive(false);
        }
        if (effect != null)
        {
            effect.gameObject.SetActive(!vIsLock);
        }
        if (Spt_Icon != null)
        {
            CommonFunction.SetGameObjectGray(Spt_Icon.gameObject, vIsLock);
        }
        if (Spt_Name != null)
        {
            CommonFunction.SetGameObjectGray(Spt_Name.gameObject, vIsLock);
        }
        if (skeletonAnimation != null)
        {
            if (vIsLock)
            {
                CommonFunction.ReSetSpineShader(skeletonAnimation, "Unlit/Transparent Colored Gray");
            }
            else
            {
                CommonFunction.ReSetSpineShader(skeletonAnimation, "Unlit/Transparent Colored");
            }
        }
        if (Box_Root != null)
        {
            Box_Root.enabled = !vIsLock;
        }
    }
    public void RefreshNoticeStatus(bool vIsNotice)
    {
        if (Spt_Notice != null)
        {
            Spt_Notice.gameObject.SetActive(vIsNotice);
        }
    }



    private void ShowHint_CaptureTerritory()
    {
        string tmpHint = "";
        if (CaptureTerritoryModule.Instance.FightState == ECaptureTerritoryFightState.Fighting)
        {
            tmpHint = string.Format(ConstString.TIP_CAMPAIGN_TIMER_END, CommonFunction.GetTimeString(CaptureTerritoryModule.Instance.CampaignTimer));
        }
        else
        {
            tmpHint = string.Format(ConstString.TIP_CAMPAIGN_TIMER_START, CommonFunction.GetTimeString(CaptureTerritoryModule.Instance.CampaignTimer));
        }

        if (Lbl_HintInfo != null)
        {
            Lbl_HintInfo.text = tmpHint;
        }
    }
    private void RefreshSkeletonOrder()
    {
        if ((rootPanel != null) && (skeletonAnimation != null))
        {
            skeletonAnimation.renderer.sortingOrder = rootPanel.sortingOrder + 2;
        }
    }
}