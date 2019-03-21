using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WalkthroughViewController : UIBase
{
    public WalkthroughView view;
    private List<WalkthroughItem> listWalkthrough;//攻略标题项//
    private List<UILabel> listHint;//跳转标题//
    private List<JumpToItem> listJumpTo;//跳转 物品显示项//
    private Dictionary<ETaskOpenView, OpenFunctionType> dicTaskJumpTo;//跳转对应数据//

    public override void Initialize()
    {
        UISystem.Instance.ClearWalkthroughJumpView();
        if (view == null)
        {
            view = new WalkthroughView();
            ObtainJumpToInfo();
        }
        view.Initialize();
        BtnEventBinding();
        InitView();
        RefreshView();
    }

    public override void Uninitialize()
    { }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        listWalkthrough = null;
        listJumpTo = null;
        listHint = null;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_BackClose.gameObject).onClick = ButtonEvent_BackClose;
    }

    public void ButtonEvent_BackClose(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_WALKTHROUGHVIEW);
    }

    /// <summary>
    /// 获取跳转对应数据
    /// </summary>
    private void ObtainJumpToInfo()
    {
        dicTaskJumpTo = TaskModule.Instance.DicTaskJumpTo;
        //dicTaskJumpTo = new Dictionary<ETaskOpenView, OpenFunctionType>();
        //string tmpInfo = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.OPEN_VIEW_FUNC_RELATIONSHIP);
        //string[] tmpArr = tmpInfo.Split(';');
        //if ((tmpArr != null) && (tmpArr.Length > 0))
        //{
        //    for (int i = 0; i < tmpArr.Length; i++)
        //    {
        //        if (string.IsNullOrEmpty(tmpArr[i]))
        //            continue;
        //        string[] tmpInfoArr = tmpArr[i].Split(',');
        //        int tmpTaskID = 0;
        //        int tmpOpenID = 0;
        //        if ((tmpInfoArr != null) && (tmpInfoArr.Length >= 2))
        //        {
        //            if (int.TryParse(tmpInfoArr[0], out tmpTaskID) && int.TryParse(tmpInfoArr[1], out tmpOpenID))
        //            {
        //                dicTaskJumpTo.Add((ETaskOpenView)tmpTaskID, (OpenFunctionType)tmpOpenID);
        //            }
        //        }
        //    }
        //}
    }

    /// <summary>
    /// 获取锁定功能ID[锁定的功能不显示]
    /// </summary>
    /// <param name="vSingleInfo"></param>
    /// <returns></returns>
    private List<ETaskOpenView> ObtainLockFunc(WalkthroughInfo vSingleInfo)
    {
        List<ETaskOpenView> tmpResult = new List<ETaskOpenView>();
        if (vSingleInfo != null)
        {
            if (vSingleInfo.Type == EWalkthroughType.ewtJumpTo)
            {
                if (vSingleInfo.ListShowInfo != null)
                {
                    for (int i = 0; i < vSingleInfo.ListShowInfo.Count; i++)
                    {
                        for (int j = 0; j < vSingleInfo.ListShowInfo[i].ListProjects.Count; j++)
                        {
                            ETaskOpenView tmpValue = (ETaskOpenView)vSingleInfo.ListShowInfo[i].ListProjects[j];
                            if (dicTaskJumpTo.ContainsKey(tmpValue))
                            {
                                if (MainCityModule.Instance.LockFuncs.Contains((uint)dicTaskJumpTo[tmpValue]))
                                {
                                    tmpResult.Add(tmpValue);
                                }
                            }
                        }
                    }
                }
            }
        }
        return tmpResult;
    }

    private void RefreshView()
    {
        if (view == null)
            return;
        if (view.Trans_LeftItem == null)
            return;
        List<WalkthroughInfo> tmpList = ConfigManager.Instance.mWalkthroughConfig.GetWalkthroughList();
        if (tmpList == null)
            return;

        int tmpIndex = 0;
        foreach (WalkthroughInfo tmpSingleInfo in tmpList)
        {
            if (tmpSingleInfo == null)
                continue;
            if (tmpSingleInfo.Type == EWalkthroughType.ewtJumpTo)
            {
                List<ETaskOpenView> tmpInfo = ObtainLockFunc(tmpSingleInfo);
                //如果锁定的功能与跳转的功能数量相同则不需要显示此攻略//
                int tmpCount = 0;
                for (int i = 0; i < tmpSingleInfo.ListShowInfo.Count; i++)
                {
                    tmpCount += tmpSingleInfo.ListShowInfo[i].ListProjects.Count;
                }
                if (tmpInfo.Count == tmpCount)
                    continue;
            }
            GameObject tmpObj = CommonFunction.InstantiateObject(view.Trans_LeftItem.gameObject, view.Trans_LeftItem.parent);
            if (tmpObj == null)
                continue;
            tmpObj.name = string.Format("WalkthroughItem_{0}", tmpIndex);
            tmpObj.gameObject.SetActive(true);
            tmpObj.transform.localPosition = new Vector3(0, 173 - tmpIndex * 65, 0);
            WalkthroughItem tmpItem = new WalkthroughItem(this, tmpObj.transform, tmpSingleInfo);
            listWalkthrough.Add(tmpItem);
            if (tmpIndex == 0)
            {
                RefreshSingleWalkthroughInfo(tmpSingleInfo, tmpObj.transform);
            }
            tmpIndex++;
        }
        view.ScrView_LeftScrollView.ResetPosition();
        view.Trans_LeftItem.gameObject.SetActive(false);
    }

    /// <summary>
    /// 刷新单个攻略详细信息
    /// </summary>
    /// <param name="vSingleInfo">攻略信息</param>
    /// <param name="vClickTrans">被点击攻略选项物件[设定选中框]</param>
    public void RefreshSingleWalkthroughInfo(WalkthroughInfo vSingleInfo, Transform vClickTrans = null)
    {
        InitContent();
        if (vSingleInfo == null)
            return;
        if (view == null)
            return;

        if (vClickTrans != null)
        {
            if (view.Spt_LeftItem_Click != null)
            {
                view.Spt_LeftItem_Click.transform.localPosition = new Vector3(0, 2 + vClickTrans.transform.localPosition.y, 0);
                view.Spt_LeftItem_Click.gameObject.SetActive(true);
            }
        }
        if (view.Lbl_RightTitle != null)
        {
            view.Lbl_RightTitle.text = vSingleInfo.Name;
        }
        if (view.Lbl_RightContent != null)
        {
            view.Lbl_RightContent.text = vSingleInfo.Content;
        }
        if (view.Spt_RightSplit != null)
        {
            view.Spt_RightSplit.gameObject.SetActive(true);
            view.Spt_RightSplit.transform.localPosition = new Vector3(0, -view.Lbl_RightContent.localSize.y, 0);
        }
        if (view.Trans_RightItemJumpTo != null)
        {
            view.Trans_RightItemJumpTo.transform.localPosition = new Vector3(0, 76 - view.Lbl_RightContent.localSize.y, 0);
        }

        if (vSingleInfo.ListShowInfo != null)
        {
            List<ETaskOpenView> tmpInfo = ObtainLockFunc(vSingleInfo);
            int tmpPosY = 74;//第一个标题位置//
            int tmpIndex_Data = 0;
            int tmpIndex_Item = 0;
            foreach (JumpToInfo tmpShowInfo in vSingleInfo.ListShowInfo)
            {
                if (tmpShowInfo != null)
                {
                    if (view.Lbl_RightHint != null)
                    {
                        GameObject tmpObj = CommonFunction.InstantiateObject(view.Lbl_RightHint.gameObject, view.Lbl_RightHint.transform.parent);
                        if ((tmpObj != null) && (tmpObj.GetComponent<UILabel>() != null))
                        {
                            tmpObj.GetComponent<UILabel>().text = tmpShowInfo.Title;
                            tmpObj.name = string.Format("JumpHint_{0}", tmpIndex_Data);
                            tmpObj.transform.localPosition = new Vector3(-270, tmpPosY, 0);
                            tmpObj.gameObject.SetActive(true);
                            tmpPosY -= 74;//第一行跳转项距离标题Y坐标距离//
                            listHint.Add(tmpObj.GetComponent<UILabel>());
                        }
                    }
                    if (view.Trans_SingleJumpToItem != null)
                    {
                        if (tmpShowInfo.ListProjects != null)
                        {
                            int tmpItemPosY = 0;
                            tmpIndex_Item = 0;
                            for (int i = 0; i < tmpShowInfo.ListProjects.Count; i++)
                            {
                                if (tmpInfo.Contains((ETaskOpenView)tmpShowInfo.ListProjects[i]))
                                    continue;
                                GameObject tmpObj = CommonFunction.InstantiateObject(view.Trans_SingleJumpToItem.gameObject, view.Trans_SingleJumpToItem.parent);
                                if (tmpObj == null)
                                    continue;
                                tmpObj.name = string.Format("JumpToITem_{0}_{1}", tmpIndex_Data, tmpIndex_Item);
                                tmpObj.gameObject.SetActive(true);
                                //跳转项X与Y轴距离都是93//
                                tmpItemPosY = tmpPosY - 93 * (tmpIndex_Item / 6);
                                tmpObj.transform.localPosition = new Vector3(-233 + 93 * (tmpIndex_Item % 6), tmpItemPosY, 0);
                                JumpToItem tmpItem;
                                if (vSingleInfo.Type == EWalkthroughType.ewtJumpTo)
                                {
                                    tmpItem = new JumpToItem(this, tmpObj.transform, dicTaskJumpTo[(ETaskOpenView)tmpShowInfo.ListProjects[i]], (ETaskOpenView)tmpShowInfo.ListProjects[i]);
                                }
                                else
                                {
                                    tmpItem = new JumpToItem(this, tmpObj.transform, (uint)tmpShowInfo.ListProjects[i]);
                                }
                                if (tmpItem != null)
                                {
                                    listJumpTo.Add(tmpItem);
                                }
                                tmpIndex_Item++;
                            }
                            tmpPosY = tmpItemPosY - 55;//标题距离上一行跳转项距离//
                        }
                    }
                    tmpIndex_Data++;
                }
            }
        }
        view.ScrView_RightScrollView.ResetPosition();
    }

    private void InitView()
    {
        InitOption();
        InitContent();
    }

    private void InitOption()
    {
        if (listWalkthrough == null)
        {
            listWalkthrough = new List<WalkthroughItem>();
        }
        else
        {
            foreach (WalkthroughItem tmp in listWalkthrough)
            {
                if (tmp != null)
                {
                    GameObject.Destroy(tmp.trans.gameObject);
                }
            }
            listWalkthrough.Clear();
        }
    }

    private void InitContent()
    {
        if (view != null)
        {
            if (view.Spt_LeftItem_Click != null)
            {
                view.Spt_LeftItem_Click.gameObject.SetActive(false);
            }
            if (view.Lbl_RightTitle != null)
            {
                view.Lbl_RightTitle.text = string.Empty;
            }
            if (view.Lbl_RightContent != null)
            {
                view.Lbl_RightContent.text = string.Empty;
            }
            if (view.Spt_RightSplit != null)
            {
                view.Spt_RightSplit.gameObject.SetActive(false);
            }
        }
        if (listHint == null)
        {
            listHint = new List<UILabel>();
        }
        else
        {
            foreach (UILabel tmp in listHint)
            {
                if (tmp != null)
                {
                    GameObject.Destroy(tmp.gameObject);
                }
            }
            listHint.Clear();
        }
        if (listJumpTo == null)
        {
            listJumpTo = new List<JumpToItem>();
        }
        else
        {
            foreach (JumpToItem tmp in listJumpTo)
            {
                if (tmp != null)
                {
                    GameObject.Destroy(tmp.trans.gameObject);
                }
            }
            listJumpTo.Clear();
        }
    }
}