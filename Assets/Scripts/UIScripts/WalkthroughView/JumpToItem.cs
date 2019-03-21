using UnityEngine;
using System.Collections;

public class JumpToItem {

    public UISprite Spt_RightItem_IconBack;
    public UISprite Spt_RightItem_Icon;
    public UISprite Spt_RightItem_IconMask;
    public UILabel Lbl_RightItem_ConditionHint;
    public Transform trans;


    private WalkthroughViewController viewController;
    private OpenFunctionType openType;
    private ETaskOpenView taskOpenView;
    private uint showID;
    private EWalkthroughType walkthroughType;
    private bool isOpen;
    private string lockHint;


    public JumpToItem(WalkthroughViewController vViewController, Transform vTrans, OpenFunctionType vOpenFunctionType, ETaskOpenView vJumpToInfo)
    {
        viewController = vViewController;
        trans = vTrans;
        if (trans != null)
        {
            Spt_RightItem_IconBack = trans.FindChild("RightItem_IconBack").GetComponent<UISprite>();
            Spt_RightItem_Icon = trans.FindChild("RightItem_IconBack/RightItem_Icon").GetComponent<UISprite>();
            Spt_RightItem_IconMask = trans.FindChild("RightItem_IconBack/RightItem_IconMask").GetComponent<UISprite>();
            Lbl_RightItem_ConditionHint = trans.FindChild("RightItem_IconBack/RightItem_ConditionHint").GetComponent<UILabel>();
            UIEventListener.Get(Spt_RightItem_IconBack.gameObject).onClick = ButtonEvent_SkillComp;
        }
        openType = vOpenFunctionType;
        taskOpenView = vJumpToInfo;
        showID = 0;
        walkthroughType = EWalkthroughType.ewtJumpTo;

        if (Spt_RightItem_Icon != null)
        {
            Spt_RightItem_Icon.gameObject.SetActive(false);
            OpenLevelData tmpData = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(openType);        
            if (tmpData != null)
            {
                CommonFunction.SetSpriteName(Spt_RightItem_Icon, tmpData.jump_icon);
                Spt_RightItem_Icon.gameObject.SetActive(true);
            }
        }
        RefreshInfos();
    }
    public JumpToItem(WalkthroughViewController vViewController, Transform vTrans, uint vShowID)
    {
        viewController = vViewController;
        trans = vTrans;
        if (trans != null)
        {
            Spt_RightItem_IconBack = trans.FindChild("RightItem_IconBack").GetComponent<UISprite>();
            Spt_RightItem_Icon = trans.FindChild("RightItem_IconBack/RightItem_Icon").GetComponent<UISprite>();
            Spt_RightItem_IconMask = trans.FindChild("RightItem_IconBack/RightItem_IconMask").GetComponent<UISprite>();
            Lbl_RightItem_ConditionHint = trans.FindChild("RightItem_IconBack/RightItem_ConditionHint").GetComponent<UILabel>();
            UIEventListener.Get(Spt_RightItem_IconBack.gameObject).onPress = ButtonEvent_PressItem;
        }
        openType = OpenFunctionType.None;
        taskOpenView = ETaskOpenView.None;
        showID = vShowID;
        walkthroughType = EWalkthroughType.ewtLineup;

        string tmpIconName = CommonFunction.GetIconNameByID(vShowID);
        int tmpQuality = CommonFunction.GetQualityByID(vShowID);
        if (Spt_RightItem_Icon != null)
        {
            Spt_RightItem_Icon.gameObject.SetActive(false);
            if (!string.IsNullOrEmpty(tmpIconName))
            {
                CommonFunction.SetSpriteName(Spt_RightItem_Icon, tmpIconName);
                Spt_RightItem_Icon.gameObject.SetActive(true);
            }
        }
        if ((Spt_RightItem_IconBack != null) && (Spt_RightItem_IconMask != null))
        {
            CommonFunction.SetQualitySprite(Spt_RightItem_IconMask, tmpQuality, Spt_RightItem_IconBack);
        }
    }

    public void Destroy()
    {
        trans = null;
    }


    public void ButtonEvent_SkillComp(GameObject vBtn)
    {
        if (walkthroughType == EWalkthroughType.ewtJumpTo)
        {
            string tmpJumpView = CommonFunction.OpenTargetView(taskOpenView);
            if (!string.IsNullOrEmpty(tmpJumpView))
            {
                UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_WALKTHROUGHVIEW);
            }
        }
        else if (walkthroughType == EWalkthroughType.ewtLineup)
        {
        }
    }

    private void ButtonEvent_PressItem(GameObject go, bool press)
    {
        HintManager.Instance.SeeDetail(go, press, showID);
    }


    public void RefreshInfos()
    {
        string tmpHint = "";
        OpenLevelData tmpOpenLevelData = null;
        isOpen = CommonFunction.CheckFuncIsOpen(taskOpenView, out lockHint, out tmpHint, out tmpOpenLevelData);
        //Debug.LogError(string.Format(string.Format("[{0}, {1}]", taskOpenView, lockHint)));
        if (isOpen)
        {
            ShowInfo(false, lockHint);
        }
        else
        {
            ShowInfo(true, lockHint);
        }
    }

    private void ShowInfo(bool vIsMask, string vCondition)
    {
        if (Spt_RightItem_IconMask != null)
        {
            Spt_RightItem_IconMask.gameObject.SetActive(vIsMask);
        }
        if (Lbl_RightItem_ConditionHint != null)
        {
            Lbl_RightItem_ConditionHint.text = vCondition;
        }
    }

}