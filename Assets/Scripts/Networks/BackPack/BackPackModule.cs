using System.Collections.Generic;
using UnityEngine;
using fogs.proto.msg;

/* File：BackPackNetWork.cs
 * Desc: 背包界面数据处理
 * Date: 2015-05-12 10:45
 * Author: taiwei
 */

public class BackPackModule : Singleton<BackPackModule>
{
    public BackPackNetWork mBackPackNetWork;
    public void Initialize()
    {
        if (mBackPackNetWork == null)
        {
            mBackPackNetWork = new BackPackNetWork();
            mBackPackNetWork.RegisterMsg();
        }
    }
    public void SendUseItem(uint id, int count, List<ID2Num> list)
    {
        UseItemReq data = new UseItemReq();
        data.id = id;
        data.num = count;
        if (list == null)
        {
            list = new List<ID2Num>();
        }
        data.id_num.Clear();
        data.id_num.AddRange(list);
        mBackPackNetWork.SendUseItem(data);
    }
    public void ReceiveUseItem(UseItemResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateExpInfo(data.new_exp_info);
            //PlayerData.Instance.UpdatePlayerAttribute(data.total_attr);
            PlayerData.Instance.MultipleAddWeapon(data.update_equip_list);
            PlayerData.Instance.MultipleAddSoldier(data.update_soldier_list);
            PlayerData.Instance.UpdateItem(data.update_item);
            List<KeyValuePair<uint, int>> list = new List<KeyValuePair<uint, int>>();
            if (data.update_equip_list != null && data.update_equip_list.Count > 0)
            {
                for (int i = 0; i < data.update_equip_list.Count;++i )
                {
                    Equip tmp = data.update_equip_list[i];
                    if(tmp != null)
                        list.Add(new KeyValuePair<uint, int>(tmp.id,0));
                }
            }
            if (data.update_soldier_list != null && data.update_soldier_list.Count > 0)
            {
                for (int i = 0; i < data.update_soldier_list.Count; ++i)
                {
                    fogs.proto.msg.Soldier tmp = data.update_soldier_list[i];
                    if (tmp != null)
                        list.Add(new KeyValuePair<uint, int>(tmp.id, 0));
                }
            }
            if (data.update_item != null && data.update_item.Count > 0)
            {
                for (int i = 0; i < data.update_item.Count; ++i)
                {
                    fogs.proto.msg.ItemInfo tmp = data.update_item[i];
                    if (tmp != null)
                        list.Add(new KeyValuePair<uint, int>(tmp.id, tmp.change_num));
                }
            }
            UISystem.Instance.BackPackView.UseItemSuccess();
            if (data.use_type == 1)
            {
                if (list != null && list.Count > 0)
                {
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEW_GATEQUIPEFFECT);
                    PropsPackageCommonData tmpData = PropsPackageCommonData.DataToThisById(list[0].Key);
                    if (tmpData != null)
                    {
                        tmpData.num = list[0].Value;
                        UISystem.Instance.GetEquipEffectView.ShowOnesLottery(tmpData);
                    }
                    //UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETSPECIALITEM);
                    //UISystem.Instance.GetSpecialItemView.SetInfo(list[0]);
                }
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }
    public void SendSellItem(uint id, int count)
    {
        SellItemReq data = new SellItemReq();
        data.id = id;
        data.num = count;
        mBackPackNetWork.SendSellItem(data);
    }
    public void ReceiveSellItem(SellItemResp data)
    {
        if (data.result == 0)
        {
            PlayerData.Instance.UpdateItem(data.update_item);
            if (data.money_type == 1)
            {
                PlayerData.Instance.UpdateGold(data.money_num);
            }
            else if (data.money_type == 2)
            {
                PlayerData.Instance.UpdateDiamond(data.money_num);
            }
            UISystem.Instance.ItemSellView.ReceiveItemCell();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }       
    }
    public void SendChipCompositeReq(uint id)
    {
        ChipCompositeReq data = new ChipCompositeReq();
        data.id = id;
        mBackPackNetWork.SendChipCompositeReq(data);
    }
    public void ReceiveChipCompositeResp(ChipCompositeResp data)
    {
        if (data.result == 0)
        {
            switch ((ECurrencyType)data.money_type)
            {
                case ECurrencyType.Gold:
                    PlayerData.Instance.UpdateGold((int)data.money_num);
                    break;
                case ECurrencyType.Diamond:
                    PlayerData.Instance.UpdateDiamond((int)data.money_num);
                    break;
            }
            if (data.comptype == 1)
            {
                if (UISystem.Instance.SoldierAttView != null)
                {
                    UISystem.Instance.SoldierAttView.PlayOndebrisCompound();
                }
                PlayerData.Instance._SoldierDebrisDepot.ReceiveChipCompositeResp(data);
            }
            if (data.comptype == 2)
            {
                PlayerData.Instance.UpdateEquipChipCompositeData(data.update_item, data.equip);
                UISystem.Instance.BackPackView.ReciveChipCompose((int)data.result);
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendBuyEquipBag()
    {
        BuyEquipBagReq data = new BuyEquipBagReq();
        mBackPackNetWork.SendBuyEquipBag(data);
    }

    public void ReceiveBuyEquipBag(BuyEquipBagResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateItem(data.money_item);
            PlayerData.Instance._BuySoldierEquipGridNum = data.buy_equip_bag_num;
            PlayerData.Instance._SoldierEquipGrid = data.equip_bag_num;
            UISystem.Instance.BackPackView.ReceiveEquipGrid();
        }
        else if (data.result == (int)ErrorCodeEnum.CanNotBuyEquipBagNum)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_EQUIPGIRDLIMIT);
        }
        else if (data.result == (int)ErrorCodeEnum.NotEnoughDiamond)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_TOP_DIAMONDLACK);
        }
    }

    public void Uninitialize()
    {
        if (mBackPackNetWork != null)
            mBackPackNetWork.RemoveMsg();
        mBackPackNetWork = null;
    }
}
