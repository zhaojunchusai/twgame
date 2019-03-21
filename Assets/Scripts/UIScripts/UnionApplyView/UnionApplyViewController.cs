using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;

public class UnionApplyViewController : UIBase
{
    public UnionApplyView view;
    private int _page;
    private const int COUNT_PER_PAGE = 15;
    private List<PendingUnionMember> _pendingUnionMembers;
    private List<UnionApplyItem> _unionApplyItems;
    public override void Initialize()
    {
        if (view == null)
            view = new UnionApplyView();
        view.Initialize();
        BtnEventBinding();
        view.ScrView_Content.onDragFinishedDown = ReqNewPage;
        InitUI();
    }
    public override void Destroy()
    {
        base.Destroy();
        view = null;
        if (_pendingUnionMembers != null) _pendingUnionMembers.Clear();
        if (_unionApplyItems != null) _unionApplyItems.Clear();
    }
    public void InitUI()
    {
        view.Lbl_ApplyCount.text = string.Format(ConstString.FORMAT_UNION_APPLY_MEMBER_COUNT,
                                                 UnionModule.Instance.UnionInfo.pending_members.Count);
        _page = 1;
        ShowApplyMembers();
    }

    public void DelOne(int index)
    {
        int maxIndex = Mathf.Min(_page * COUNT_PER_PAGE , _pendingUnionMembers.Count + 1);
        for (int i = index; i < maxIndex - 1; i++)
        {
            _unionApplyItems[i].InitItem(_unionApplyItems[i + 1].MemberInfo);
        }
        _unionApplyItems[maxIndex - 1].gameObject.SetActive(false);
        view.ScrView_Content.ResetPosition();
    }

    private void ShowApplyMembers()
    {
        if (_unionApplyItems == null)
            _unionApplyItems = new List<UnionApplyItem>();

        _pendingUnionMembers = UnionModule.Instance.UnionInfo.pending_members;
        int minIndex = _page * COUNT_PER_PAGE - COUNT_PER_PAGE;
        int maxIndex = Mathf.Min(_page * COUNT_PER_PAGE , _pendingUnionMembers.Count);
        //Debug.Log(string.Format("page ={0},min ={1},max ={2},{3},{4}", _page, minIndex, maxIndex, 
        //    _pendingUnionMembers.Count, _unionApplyItems.Count));
        for (int i = minIndex; i < maxIndex; i++)
        {
            if (i >= _unionApplyItems.Count)
            {
                _unionApplyItems.Add(InstantiateUnionApplyItem());
            }
            _unionApplyItems[i].gameObject.SetActive(true);
            _unionApplyItems[i].InitItem(_pendingUnionMembers[i]);
        }

        for (int i = maxIndex; i < _unionApplyItems.Count; i++)
        {
            _unionApplyItems[i].gameObject.SetActive(false);
        }
        view.Grd_Content.Reposition();
    }

    private void ReqNewPage()
    {
        if (_pendingUnionMembers.Count <= _page * COUNT_PER_PAGE)
        {
            return;
        }
        ++_page;
        ShowApplyMembers();
    }

    private UnionApplyItem InstantiateUnionApplyItem()
    {
        GameObject go = CommonFunction.InstantiateObject(view.Gobj_UnionApplyItem, view.Grd_Content.transform);
        return go.AddComponent<UnionApplyItem>();
    }

    public void ButtonEvent_OneKeyRefuse(GameObject btn)
    {
        if (UnionModule.Instance.UnionInfo.pending_members.Count <= 0)
            return;
        UnionModule.Instance.OnSendAuditUnionMembers(UnionModule.UnionAuditType.Multi,
                                                  UnionModule.UnionAuditPassType.Refuse, 0);
    }

    public void ButtonEvent_OneKeyAgree(GameObject btn)
    {
        if(UnionModule.Instance.UnionInfo.pending_members.Count <= 0)
            return;
        UnionModule.Instance.OnSendAuditUnionMembers(UnionModule.UnionAuditType.Multi,
                                                  UnionModule.UnionAuditPassType.Pass, 0);
    }

    private void ClickMask(GameObject go)
    {
        UISystem.Instance.CloseGameUI(UnionApplyView.UIName);
    }

    public override void Uninitialize()
    {
        view.ScrView_Content.onDragFinishedDown = null;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_OneKeyRefuse.gameObject).onClick = ButtonEvent_OneKeyRefuse;
        UIEventListener.Get(view.Btn_OneKeyAgree.gameObject).onClick = ButtonEvent_OneKeyAgree;
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = ClickMask;
    }


}
