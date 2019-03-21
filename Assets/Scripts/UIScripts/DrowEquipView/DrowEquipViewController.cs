using UnityEngine;
using System;
using System.Collections;
using Assets.Script.Common;
 
public class DrowEquipViewController : UIBase
{
    public DrowEquipView view;
    public override void Initialize()
    {
        if (view == null)
            view = new DrowEquipView();
        view.Initialize();
        BtnEventBinding();
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_TOPFUNC);
        this.Init();
    }
    public override void Init()
    {
        this.SetOneChoose();
        this.SetTenChoose();
        this.CountDown();
    }
    public void SetOneChoose()
    {
        DrowEquipInfo info = ConfigManager.Instance.mDrowEquipData.FindByType(0);
        if (info == null)
            return;
        if (PlayerData.Instance.DrowEquipFreeCount <= 0)
        {
            this.view.Lbl_OnceFreeLb.gameObject.SetActive(true);
            this.view.Lbl_OnceFreeLb.text = ConstString.FREECOUNTZERO;
        }
        if(PlayerData.Instance.DrowEquipFreeCount > 0 && Main.mTime >= PlayerData.Instance.DrowEquipFreeTime)
        {
            this.view.Spt_OnceFreeSP.gameObject.SetActive(true);
            CommonFunction.SetSpriteName(this.view.Spt_CurrencyIcon, GetMoneyIcon(info.Gold_type));
            this.view.Lbl_CurrencyNum.text = ConstString.FREE;
            return;
        }
        this.view.Spt_OnceFreeSP.gameObject.SetActive(false);
        int num = PlayerData.Instance.GetItemCountByID((uint)info.Item_id);
        if(num >= info.Item_num)
        {
            ItemInfo tmpInfo = ConfigManager.Instance.mItemData.GetItemInfoByID((uint)info.Item_id);
            if (tmpInfo == null)
                return;

            CommonFunction.SetSpriteName(view.Spt_CurrencyIcon, tmpInfo.icon);

            view.Lbl_CurrencyNum.text = string.Format("{0}/{1}", num, info.Item_num);

            return;
        }
        else
        {
            CommonFunction.SetSpriteName(view.Spt_CurrencyIcon, GetMoneyIcon(info.Gold_type));

            view.Lbl_CurrencyNum.text = info.Gold_num.ToString();
        }

    }
    public void SetTenChoose()
    {
        DrowEquipInfo info = ConfigManager.Instance.mDrowEquipData.FindByType(1);
        if (info == null)
            return;

        int num = PlayerData.Instance.GetItemCountByID((uint)info.Item_id);
        if(num >= info.Item_num)
        {
            ItemInfo tmpInfo = ConfigManager.Instance.mItemData.GetItemInfoByID((uint)info.Item_id);
            if(tmpInfo == null)
                return;

            CommonFunction.SetSpriteName(view.Spt_TenCurrencyIcon,tmpInfo.icon);

            view.Lbl_TenCurrencyNum.text = string.Format("{0}/{1}", num, info.Item_num);
            view.Dis.SetActive(false);
        }
        else
        {
            CommonFunction.SetSpriteName(view.Spt_TenCurrencyIcon, GetMoneyIcon(info.Gold_type));

            view.Lbl_TenCurrencyNum.text = info.Gold_num.ToString();
            view.Dis.SetActive(true);
        }
    }
    private string GetMoneyIcon(int type)
    {
        if (type == 2)
            return "ZCJ_icon_daibi_l";
        else
            return "ZCJ_icon_jinbi_l";
    }
    private void CountDown()
    {
        long time = Main.mTime;
        if (PlayerData.Instance.DrowEquipFreeCount <= 0)
        {
            this.view.Lbl_OnceFreeLb.gameObject.SetActive(true);
            this.view.Lbl_OnceFreeLb.text = ConstString.FREECOUNTZERO;
            this.view.Spt_OnceFreeSP.gameObject.SetActive(false);
            return;
        }
        else
            this.view.Lbl_OnceFreeLb.gameObject.SetActive(true);

        if (time >= PlayerData.Instance.DrowEquipFreeTime)
        {
            this.view.Lbl_OnceFreeLb.text = string.Format(ConstString.RECRUITFREECOUNT, PlayerData.Instance.DrowEquipFreeCount);
            this.view.Spt_OnceFreeSP.gameObject.SetActive(true);
            DrowEquipInfo info = ConfigManager.Instance.mDrowEquipData.FindByType(0);
            if (info == null)
                return;
            CommonFunction.SetSpriteName(this.view.Spt_CurrencyIcon, GetMoneyIcon(info.Gold_type));
            this.view.Lbl_CurrencyNum.text = ConstString.FREE;
            return;
        }
        else
        {
            this.view.Spt_OnceFreeSP.gameObject.SetActive(false);
        }
        long tmpTime = PlayerData.Instance.DrowEquipFreeTime - time;
        if (tmpTime <= 0)
            return;

        long hour = tmpTime / 3600;
        long min = tmpTime/ 60 - hour * 60;
        long sec = tmpTime % 60;
        string timeStr = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, min, sec);
        this.view.Lbl_OnceFreeLb.text = string.Format(ConstString.RECRUITTIMELATERFREE, timeStr);
    }
    public void ButtonEvent_Button_close(GameObject btn)
    {
        this.Close(null,null);
    }
    public void ButtonEvent_Button_ChooseOnce(GameObject btn)
    {
        if (PlayerData.Instance.DrowEquipFreeCount > 0 && Main.mTime >= PlayerData.Instance.DrowEquipFreeTime)
        {
            DrowEquipModule.Instance.SendOneExtractEquipReq();
            return;
        }
        else
        {
            DrowEquipInfo info = ConfigManager.Instance.mDrowEquipData.FindByType(0);

            if (info != null && info.Gold_type == 1 && PlayerData.Instance._Gold < info.Gold_num)
            {
                int num = PlayerData.Instance.GetItemCountByID((uint)info.Item_id);
                if (num < info.Item_num)
                {
                    ErrorCode.ShowErrorTip((int)ErrorCodeEnum.NotEnoughGold);
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
                    return;
                }
            }
        }
         DrowEquipModule.Instance.SendOneExtractEquipReq();
    }
    public void ButtonEvent_Button_ChooseTen(GameObject btn)
    {
        DrowEquipInfo info = ConfigManager.Instance.mDrowEquipData.FindByType(1);

        if (info != null && info.Gold_type == 1 && PlayerData.Instance._Gold < info.Gold_num)
        {
            int num = PlayerData.Instance.GetItemCountByID((uint)info.Item_id);
            if (num < info.Item_num)
            {
                ErrorCode.ShowErrorTip((int)ErrorCodeEnum.NotEnoughGold);
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
                return;
            }
        }
        DrowEquipModule.Instance.SendMultipleExtractReq();
    }

    public override void Uninitialize()
    {
        Scheduler.Instance.RemoveTimer(CountDown);
    }
    public override void Destroy()
    {
        base.Destroy();
        Scheduler.Instance.RemoveTimer(CountDown);
    }
    public void BtnEventBinding()
    {
        Scheduler.Instance.AddTimer(1.0f, true, CountDown);
        UIEventListener.Get(view.Btn_Button_close.gameObject).onClick = ButtonEvent_Button_close;
        UIEventListener.Get(view.Spt_ChooseOnce.gameObject).onClick = ButtonEvent_Button_ChooseOnce;
        UIEventListener.Get(view.Spt_ChooseTen.gameObject).onClick = ButtonEvent_Button_ChooseTen;
    }


}
