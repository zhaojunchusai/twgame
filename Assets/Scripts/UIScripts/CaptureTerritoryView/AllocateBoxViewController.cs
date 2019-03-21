using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;
public class AllocateBoxViewController : UIBase 
{
    public AllocateBoxView view;

    private List<AllocateBoxItem> _rankItemList = new List<AllocateBoxItem>();
    private const int COUNT_PER_PAGE = 15;
    public int _page = 0;

    public override void Initialize()
    {
        if (view == null)
            view = new AllocateBoxView();
        view.Initialize();
        BtnEventBinding();
        view.ScrView_Items.onDragFinishedDown = ReqNewPage;
        InitBoxCount();
        InitRankContent(CaptureTerritoryModule.Instance.UnionMemberScoreList, 1);
    }

    private void InitBoxCount()
    {      
        view.Lbl_BoxCount.text = string.Format(ConstString.CAPTURE_BOX_COUNT,CaptureTerritoryModule.Instance.TotalBoxCount);
    }

    private void InitRankContent(List<CampaignRankInfo> list,int page)
    {
        _page = page;

        if (page == 1)
            view.ScrView_Items.ResetPosition();

        int startIndex = COUNT_PER_PAGE * (page - 1);
        int endIndex = Mathf.Min(list.Count, COUNT_PER_PAGE * page);
        for (int i = startIndex; i < endIndex; i++)
        {
            if (i >= _rankItemList.Count)
            {
                AllocateBoxItem unionItem = CommonFunction.InstantiateItem<AllocateBoxItem>(view.Gobj_AllocateBoxItem, view.Grd_Grid.transform);
                _rankItemList.Add(unionItem);
            }
            _rankItemList[i].gameObject.SetActive(true);
            _rankItemList[i].gameObject.name = string.Format("item{0:D3}", i);
            _rankItemList[i].Init(list[i],i);
        }

        for (int i = endIndex; i < _rankItemList.Count; i++)
        {
            _rankItemList[i].gameObject.SetActive(false);
        }

        view.Grd_Grid.Reposition();
    }
    
    private void ReqNewPage()
    {
        InitRankContent(CaptureTerritoryModule.Instance.UnionMemberScoreList, _page + 1);
    }

    public void CleanRecords()
    {
        CaptureTerritoryModule.Instance.ClearBoxAllocation();
        InitBoxCount();
        for (int i = 0; i < _rankItemList.Count; i++)
        {
            _rankItemList[i].CleanNum();
        }
    }

    public void ButtonEvent_SeeBox(GameObject btn)
    {
        List<ChestInfo> list = CaptureTerritoryModule.Instance.ChestInfo;
        if (list.Count <= 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.TIP_NO_BOX);
            return;
        }
        List<CommonItemData> show = new List<CommonItemData>();
        for (int i = 0; i < list.Count; i++)
        {
            show.Add(new CommonItemData(list[i].id,list[i].num,true));
        }
        show.Sort();
        UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
        UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(show);
        UISystem.Instance.RecieveResultVertView.SetTitle(ConstString.TITLE_SEE_BOX);
    }

    public void ButtonEvent_Confirm(GameObject btn)
    {
        CaptureTerritoryModule.Instance.SendAssignChest();
    }

    public void ButtonEvent_Cancel(GameObject btn)
    {
        CleanRecords();
    }

    public void ButtonEvent_Close(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(AllocateBoxView.UIName);
    }

    public override void Uninitialize()
    {

    }

    public override void Destroy()
    {
        view = null;
        _rankItemList.Clear();
        _page = 0;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_SeeBox.gameObject).onClick = ButtonEvent_SeeBox;
        UIEventListener.Get(view.Btn_Confirm.gameObject).onClick = ButtonEvent_Confirm;
        UIEventListener.Get(view.Btn_Cancel.gameObject).onClick = ButtonEvent_Cancel;
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = ButtonEvent_Close;
    }


}
