using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;
using System.Linq;
public class UnionDonationRecordViewController : UIBase
{
    public UnionDonationRecordView view;
    private List<DonationRecItem> _recItems = new List<DonationRecItem>();
    //private int _page = 1;
    private const int MAXCOUNT = 9;
    private List<UnionDonateLog> _donateLog = new List<UnionDonateLog>();
    public override void Initialize()
    {
        if (view == null)
            view = new UnionDonationRecordView();
        view.Initialize();
        //view.ScrView_Content.onDragFinishedDown = ReqNewPage;
        view.UIWrapContent_Content.onInitializeItem = SetRecordInfo;
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = CloseUI;

        InitUI();
    }
    public override void Destroy()
    {
        base.Destroy();
        view = null;
        _recItems.Clear();
        if(_donateLog!=null) _donateLog.Clear();
    }
    public void InitUI()
    {
       // _page = 1;
        ShowRecords();
    }

    private void ShowRecords()
    {
        _donateLog.Clear();
        _donateLog.AddRange(UnionModule.Instance.UnionInfo.donate_logs.OrderByDescending(s => s.tick).ToList());
       // int minIndex = _page*COUNT_PER_PAGE - COUNT_PER_PAGE;
       // int maxIndex = Mathf.Min(_page*COUNT_PER_PAGE , _donateLog.Count);
        int count = _donateLog.Count;
        int itemCount = _recItems.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_Content.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_Content.minIndex = -index;
        view.UIWrapContent_Content.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_Content.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_Content.enabled = false;
        }

        if (itemCount > count)
        {
            for (int i = itemCount - count; i < itemCount; i++)
            {
                _recItems[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                _recItems.Add(InstantiateRecordItem());
            }
            _recItems[i].gameObject.SetActive(true);
            _recItems[i].Init(_donateLog[i]);
        }
        view.UIWrapContent_Content.ReGetChild();
        view.Grd_Content.Reposition();
        view.ScrView_Content.ResetPosition();
    }

    //private void ReqNewPage()
    //{
    //    if (_donateLog.Count <= _page*COUNT_PER_PAGE)
    //    {
    //        return;
    //    }
    //    ++_page;
    //    ShowRecords();
    //}

    private DonationRecItem InstantiateRecordItem()
    {
        GameObject go = CommonFunction.InstantiateObject(view.Gobj_RecItem, view.Grd_Content.transform);
        return go.AddComponent<DonationRecItem>();
    }

    public void SetRecordInfo(GameObject go, int wrapIndex, int realIndex) 
    {
        if (realIndex >= _donateLog.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        DonationRecItem item = _recItems[wrapIndex];
        item.Init(_donateLog[realIndex]);
    }

    private void CloseUI(GameObject go)
    {
        UISystem.Instance.CloseGameUI(UnionDonationRecordView.UIName);
    }
    
    public override void Uninitialize()
    {
        //view.ScrView_Content.onDragFinishedDown = null;
    }
    
}
