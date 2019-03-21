using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;

public class UnionMemberInfoViewController : UIBase 
{
    private const float ITEMPOSY_INIT = 125;
    private const float ITEMPOSY_DIS = 110;


    public UnionMemberInfoView view;
    private List<UnionMemberInfoItem> listUnionMembers;


    public override void Initialize()
    {
        if (view == null)
        {
            view = new UnionMemberInfoView();
            view.Initialize();
        }
        BtnEventBinding();
        InitView();
    }

    public override void Uninitialize()
    { }

    public override void Destroy()
    {
        view = null;
        listUnionMembers.Clear();
        base.Destroy();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = ButtonEvent_CloseView;
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_CloseView(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_UNIONMEMBERINFO);
    }


    public void ShowView(RankPlayerInfoResp vData)
    {
        if ((vData != null) || (vData.members != null) || (view.Spt_MemberItem != null))
        {
            int tmpIndex = 0;
            foreach (UnionMember tmpSingleData in vData.members)
            {
                if (tmpSingleData != null)
                {
                    Vector3 tmpPos = new Vector3(0, ITEMPOSY_INIT - ITEMPOSY_DIS * tmpIndex, 0);
                    if ((tmpIndex < listUnionMembers.Count) && (listUnionMembers[tmpIndex] != null))
                    {
                        listUnionMembers[tmpIndex].RefreshItem(tmpSingleData, string.Format("[{0}]", tmpIndex), tmpPos);
                    }
                    else
                    {
                        GameObject tmpObj = CommonFunction.InstantiateObject(view.Spt_MemberItem.gameObject, view.Spt_MemberItem.transform.parent);
                        if (tmpObj == null)
                            continue;
                        UnionMemberInfoItem tmpItem = new UnionMemberInfoItem(tmpObj.transform, tmpSingleData, string.Format("[{0}]", tmpIndex), tmpPos);
                        listUnionMembers.Add(tmpItem);
                    }
                    tmpIndex++;
                }
            }
        }
    }


    private void InitView()
    {
        if ((view != null) && (view.ScrView_ScrollView != null))
        {
            view.ScrView_ScrollView.panel.clipOffset = Vector2.zero;
            view.ScrView_ScrollView.transform.localPosition = Vector3.zero;
        }
        if (listUnionMembers == null)
        {
            listUnionMembers = new List<UnionMemberInfoItem>();
        }
        else
        {
            foreach (UnionMemberInfoItem tmpSingle in listUnionMembers)
            {
                if (tmpSingle != null)
                {
                    tmpSingle.InitItem();
                }
            }
        }
    }
}
