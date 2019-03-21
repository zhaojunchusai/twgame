using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 选择徽章开启方式
/// </summary>
public enum EBadegOpenType
{
    ebotCreate, //创建//
    ebotChange  //修改//
}
/// <summary>
/// 徽章类型
/// </summary>
public enum EBadgeType
{
    /// <summary>
    /// 永久解锁
    /// </summary>
    ebtUnLock,
    /// <summary>
    /// 限时解锁
    /// </summary>
    ebtLockLimit,
    /// <summary>
    /// 未解锁
    /// </summary>
    ebtLock
}
/// <summary>
/// 锁定徽章解锁条件类型
/// </summary>
public enum EBadgeUnLockType
{
    /// <summary>
    /// 无解锁条件
    /// </summary>
    ebultNone = 0,
    /// <summary>
    /// 军团群雄争霸第1
    /// </summary>
    ebult1,
    /// <summary>
    /// 征战榜第1
    /// </summary>
    ebult2,
    /// <summary>
    /// 军团霸主榜第1
    /// </summary>
    ebult3,
    /// <summary>
    /// 军团探险榜第1
    /// </summary>
    ebult4,
    /// <summary>
    /// 军团排位榜第1
    /// </summary>
    ebult5
}

public class ChangeUnionBadgeViewController : UIBase 
{
    private const float INIT_POS_X = -270;
    private const float INIT_POS_Y = 100;
    private const float DIS_POS = 90;
    private const int LINE_LIMIT = 7;


    public ChangeUnionBadgeView view;
    private List<UnionIconData> listUnionBadgeData;
    private List<UnionBadgeItem> listUnionBadgeItem;
    private EBadegOpenType badegOpenType;
    private uint chooseBadgeID;
    private uint changeCostDiamonds;


    public override void Initialize()
    {
        if (view == null)
        {
            view = new ChangeUnionBadgeView();
            if (listUnionBadgeData == null)
            {
                listUnionBadgeData = new List<UnionIconData>();
            }
            else
            {
                listUnionBadgeData.Clear();
            }
            foreach (UnionIconData tmpSingle in ConfigManager.Instance.mUnionConfig.GetUnionIconList())
            {
                listUnionBadgeData.Add(tmpSingle);
            }
        }
        view.Initialize();
        BtnEventBinding();
        RefreshView();
    }

    public override void Uninitialize()
    {

    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        if (listUnionBadgeData != null)
        {
            listUnionBadgeData.Clear();
        }
        listUnionBadgeData = null;
        if (listUnionBadgeItem != null)
        {
            foreach (UnionBadgeItem tmpSingleItem in listUnionBadgeItem)
            {
                if (tmpSingleItem != null)
                {
                    tmpSingleItem.Destroy();
                }
            }
            listUnionBadgeItem.Clear();
        }
        listUnionBadgeItem = null;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Confirm.gameObject).onClick = ButtonEvent_Confirm;
        UIEventListener.Get(view.Spt_MaskBGSprite.gameObject).onClick = ButtonEvent_Close;
    }


    /// <summary>
    /// 确定按钮
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Confirm(GameObject btn)
    {
        UnionIconData tmpItem = ConfigManager.Instance.mUnionConfig.GetUnionIconByID(chooseBadgeID);
        
        if (tmpItem != null)
        {
            if (badegOpenType == EBadegOpenType.ebotCreate)
            {
                UISystem.Instance.CreateUnionView.RefreshBadge(chooseBadgeID);
                UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_CHANGEUNIONBADGEVIEW);
            }
            else
            {
                if (PlayerData.Instance._Diamonds < changeCostDiamonds)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_DIAMOND);
                }
                else
                {
                    if (tmpItem.mIcon != UnionModule.Instance.UnionInfo.base_info.icon)
                    {
                        UnionModule.Instance.OnSendUpdateUnionIcon(tmpItem.mID.ToString());
                    }
                    else
                    {
                        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_CHANGEUNIONBADGEVIEW);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 关闭按钮
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Close(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_CHANGEUNIONBADGEVIEW);
    }

    /// <summary>
    /// 选择徽章
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_ChooseBadge(GameObject btn)
    {
        if (btn != null)
        {
            UnionBadgeItem tmpItem = btn.GetComponent<UnionBadgeItem>();
            if (tmpItem != null)
            {
                if (chooseBadgeID != tmpItem.BadgeID)
                {
                    if (tmpItem.lockType != EBadgeType.ebtLock)
                    {
                        chooseBadgeID = tmpItem.BadgeID;
                        if (view.Spt_BadgeCheck != null)
                        {
                            view.Spt_BadgeCheck.transform.localPosition = btn.transform.localPosition;
                        }
                    }
                    else
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, tmpItem.UnLockHint);
                    }
                }
            }
        }
    }
    

    private void InitView()
    {
        if (listUnionBadgeItem != null)
        {
            foreach (UnionBadgeItem tmpSingleItem in listUnionBadgeItem)
            {
                if (tmpSingleItem != null)
                {
                    tmpSingleItem.InitItem();
                }
            }
        }
        else
        {
            listUnionBadgeItem = new List<UnionBadgeItem>();
        }

        if (view.Spt_CostSprite != null)
        {
            view.Spt_CostSprite.gameObject.SetActive(false);
        }
        if (view.Lbl_CostNumLabel != null)
        {
            view.Lbl_CostNumLabel.text = "0";
        }
        chooseBadgeID = 0;
        changeCostDiamonds = 0;
    }

    private void RefreshView()
    {
        InitView();
        if (UnionModule.Instance.HasUnion)
        {
            badegOpenType = EBadegOpenType.ebotChange;
        }
        else
        {
            badegOpenType = EBadegOpenType.ebotCreate;
        }
        if (view.ScrView_ScrollView != null)
        {
            view.ScrView_ScrollView.ResetPosition();
            view.UIPanel_ScrollView.transform.localPosition = Vector3.zero;
            view.UIPanel_ScrollView.clipOffset = Vector2.zero;
        }
        if ((listUnionBadgeData != null) || (view.Spt_BadgeItem != null))
        {
            int tmpIndex = 0;
            EBadgeType tmpLockType = EBadgeType.ebtUnLock;
            string tmpUnLockHint = "";
            uint tmpLimitTime = 0;
            foreach (UnionIconData tmpSingleData in listUnionBadgeData)
            {
                if (tmpSingleData != null)
                {
                    tmpLockType = EBadgeType.ebtUnLock;
                    tmpUnLockHint = "";
                    tmpLimitTime = 0;
                    CheckSingleBadgeIsLock(tmpSingleData, out tmpLockType, out tmpUnLockHint, out tmpLimitTime);

                    if ((badegOpenType == EBadegOpenType.ebotCreate) && (tmpLockType != EBadgeType.ebtUnLock))
                    {
                        continue;
                    }
                    else
                    {
                        string tmpName = string.Format("[{0}]", tmpIndex);
                        Vector3 tmpPos = new Vector3(INIT_POS_X + DIS_POS * (tmpIndex % LINE_LIMIT), INIT_POS_Y - DIS_POS * (tmpIndex / LINE_LIMIT), 0);
                        if ((listUnionBadgeItem.Count > tmpIndex) && (listUnionBadgeItem[tmpIndex] != null))
                        {
                            listUnionBadgeItem[tmpIndex].RefreshItem(tmpName, tmpPos, tmpSingleData.mID, tmpSingleData.mIcon, tmpLockType, tmpLimitTime, tmpUnLockHint);
                        }
                        else
                        {
                            GameObject tmpObj = CommonFunction.InstantiateObject(view.Spt_BadgeItem.gameObject, view.Spt_BadgeItem.transform.parent);
                            UnionBadgeItem tmpItem = tmpObj.AddComponent<UnionBadgeItem>();
                            tmpItem.RefreshItem(tmpName, tmpPos, tmpSingleData.mID, tmpSingleData.mIcon, tmpLockType, tmpLimitTime, tmpUnLockHint, true);
                            UIEventListener.Get(tmpObj).onClick = ButtonEvent_ChooseBadge;
                            listUnionBadgeItem.Add(tmpItem);
                        }
                        if (badegOpenType == EBadegOpenType.ebotCreate)
                        {
                            if (tmpIndex == 0)
                            {
                                if (view.Spt_BadgeCheck != null)
                                {
                                    view.Spt_BadgeCheck.transform.localPosition = tmpPos;
                                    chooseBadgeID = tmpSingleData.mID;
                                }
                            }
                        }
                        else
                        {
                            if (UnionModule.Instance.UnionInfo != null)
                            {
                                if (UnionModule.Instance.UnionInfo.base_info.icon == tmpSingleData.mID.ToString())
                                {
                                    if (view.Spt_BadgeCheck != null)
                                    {
                                        view.Spt_BadgeCheck.transform.localPosition = tmpPos;
                                        chooseBadgeID = tmpSingleData.mID;
                                    }
                                }
                            }
                        }
                        tmpIndex++;
                    }
                }
            }
        }
        changeCostDiamonds = ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mUpdateUnionIconCost;
        if (badegOpenType == EBadegOpenType.ebotChange)
        {
            if (view.Spt_CostSprite != null)
            {
                view.Spt_CostSprite.gameObject.SetActive(true);
            }
            if (view.Lbl_CostNumLabel != null)
            {
                view.Lbl_CostNumLabel.text = changeCostDiamonds.ToString();
            }
        }
    }

    /// <summary>
    /// 判断是否锁定
    /// </summary>
    /// <param name="vData"></param>
    /// <returns></returns>
    private void CheckSingleBadgeIsLock(UnionIconData vData, out EBadgeType vLockType, out string vUnLockHint, out uint vLimitTime)
    {
        vLockType = EBadgeType.ebtUnLock;
        vLimitTime = 0;
        vUnLockHint = "";
        if (vData != null)
        {
            if (EBadgeUnLockType.ebultNone == ((EBadgeUnLockType)vData.mUnLock))
            {
                vLockType = EBadgeType.ebtUnLock;
            }
            else
            {
                vLockType = EBadgeType.ebtLock;
                if ((UnionModule.Instance.DicLimitBadge != null) && (UnionModule.Instance.DicLimitBadge.Count > 0))
                {
                    foreach (KeyValuePair<uint, uint> tmpData in UnionModule.Instance.DicLimitBadge)
                    {
                        if (tmpData.Key == vData.mID)
                        {
                            vLimitTime = tmpData.Value;
                            if (vLimitTime == 0)
                            {
                                vLockType = EBadgeType.ebtUnLock;
                            }
                            else
                            {
                                vLockType = EBadgeType.ebtLockLimit;
                            }
                        }
                    }
                }
            }
            vUnLockHint = GetLockHintInfo((EBadgeUnLockType)vData.mUnLock);
        }
    }

    private string GetLockHintInfo(EBadgeUnLockType vLockType)
    {
        string tmpResult = "";
        if (EBadgeUnLockType.ebult1 == vLockType)
        {
            tmpResult = ConstString.UNION_BADGE_LOCK_1;
        }
        else if (EBadgeUnLockType.ebult2 == vLockType)
        {
            tmpResult = ConstString.UNION_BADGE_LOCK_2;
        }
        else if (EBadgeUnLockType.ebult3 == vLockType)
        {
            tmpResult = ConstString.UNION_BADGE_LOCK_3;
        }
        else if (EBadgeUnLockType.ebult4 == vLockType)
        {
            tmpResult = ConstString.UNION_BADGE_LOCK_4;
        }
        else if (EBadgeUnLockType.ebult5 == vLockType)
        {
            tmpResult = ConstString.UNION_BADGE_LOCK_5;
        }
        return tmpResult;
    }
}