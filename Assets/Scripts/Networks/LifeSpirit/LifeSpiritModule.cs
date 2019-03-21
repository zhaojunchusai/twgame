using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
public class LifeSpiritModule : Singleton<LifeSpiritModule>
{
    private LifeSpiritNetwork mLifeSpiritNetwork;

    public void Initialize()
    {
        if (mLifeSpiritNetwork == null)
        {
            mLifeSpiritNetwork = new LifeSpiritNetwork();
            mLifeSpiritNetwork.RegisterMsg();
        }
    }

    public void SendPutOnSoul(LifeSoulOpType type, List<EquipedSoul> list, ulong c_uid = 0)
    {
        PutOnSoulReq data = new PutOnSoulReq();
        data.c_uid = c_uid;
        data.soul_list.Clear();
        data.soul_list.AddRange(list);
        data.type = type;
        mLifeSpiritNetwork.SendPutOnSoul(data);
    }

    public void ReceivePutOnSoul(PutOnSoulResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance._LifeSoulDepot.PutOnSoulResp(data);
            UISystem.Instance.LifeSpiritView.OnPutonLifeSoulSuccess();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendTakeOffSoul(LifeSoulOpType type, List<ulong> i_uid, ulong c_uid = 0)
    {
        TakeOffSoulReq data = new TakeOffSoulReq();
        data.type = type;
        data.c_uid = c_uid;
        data.i_uid.Clear();
        data.i_uid.AddRange(i_uid);
        mLifeSpiritNetwork.SendTakeOffSoul(data);
    }

    public void ReceiveTakeOffSoul(TakeOffSoulResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            if (data.c_uid == 0)
            {
                PlayerData.Instance.UpdatePlayerAttribute(data.attr);
                PlayerData.Instance._LifeSoulDepot.TakeOffPlayerLifeSoul(data.i_uid);
            }
            else
            {
                Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(data.c_uid);
                soldier._lifeSoulDepot.Delete(data.i_uid);
                soldier.SerializeShowInfo(data.attr);
                PlayerData.Instance._LifeSoulDepot.TakeOffSoldierLifeSoul(data.i_uid);
            }
            UISystem.Instance.LifeSpiritView.OnTakeoffSoulSuccess();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendUpgradeSoul(ulong i_uid, ulong c_uid, List<ulong> consume_uid)
    {
        UpgradeSoulReq data = new UpgradeSoulReq();
        data.c_uid = c_uid;
        data.i_uid = i_uid;
        data.consume_uid.Clear();
        data.consume_uid.AddRange(consume_uid);
        mLifeSpiritNetwork.SendUpgradeSoul(data);
    }

    public void ReceiveUpgradeSoul(UpgradeSoulResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.LifeSpiritIntensifyView.OnIntensifySuccess(data);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendSellSoul(ulong uid)
    {
        SellSoulReq data = new SellSoulReq();
        data.uid = uid;
        mLifeSpiritNetwork.SendSellSoul(data);
    }

    public void ReceiveSellSoul(SellSoulResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateItem(data.update_items);
            PlayerData.Instance._LifeSoulDepot.DeleteLifeSoul(data.uid);
            UISystem.Instance.ItemSellView.ReceiveItemCell();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendBuySoulGrid()
    {
        BuySoulGridReq data = new BuySoulGridReq();
        mLifeSpiritNetwork.SendBuySoulGrid(data);
    }

    public void ReceiveBuySoulGrid(BuySoulGridResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance._PreyLifeSoulInfo.grid_num = data.grid_num;
            PlayerData.Instance._PreyLifeSoulInfo.buy_times = data.buy_times;
            PlayerData.Instance.UpdateItem(data.update_item);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITPACK_ADDPACKGRIDSUCCESS);
            if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_LIFESPIRITPACKVIEW))
                UISystem.Instance.LifeSpiritPackView.OnBuyGirdSuccess();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendExploreSoul(LifeSoulOpType type, int explore_type)
    {
        ExploreSoulReq data = new ExploreSoulReq();
        data.type = type;
        data.explore_type = explore_type;
        mLifeSpiritNetwork.SendExploreSoul(data);
    }

    public void ReceiveExploreSoul(ExploreSoulResp data)
    {
        UISystem.Instance.PreyLifeSpiritView.OnPreyLifeSpiritSuccess(data);
    }

    public void SendCollectSoul(LifeSoulOpType type, ulong uid)
    {
        CollectSoulReq data = new CollectSoulReq();
        data.type = type;
        data.uid = uid;
        mLifeSpiritNetwork.SendCollectSoul(data);
    }

    public void ReceiveCollectSoul(CollectSoulResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS || data.result == (int)ErrorCodeEnum.SoulOneKeyCollectMax)
        {
            PlayerData.Instance._LifeSoulDepot.AddPackLifeSouls(data.souls);
            UISystem.Instance.PreyLifeSpiritView.OnPickupLifeSpiritSuccess(data);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void Uninitialize()
    {
        mLifeSpiritNetwork = null;
    }
}
