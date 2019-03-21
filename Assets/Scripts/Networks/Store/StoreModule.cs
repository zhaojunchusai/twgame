using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;

public class StoreModule : Singleton<StoreModule>
{
    public StoreNetwork mStoreNetwork;
    private uint _vipRechargeID;
    public void Initialize()
    {
        if (mStoreNetwork == null)
        {
            mStoreNetwork = new StoreNetwork();
            mStoreNetwork.RegisterMsg();
        }
    }

    public void Uninitialize()
    {
        if (mStoreNetwork != null)
            mStoreNetwork.RemoveMsg();
        mStoreNetwork = null;
    }
    public void SendRecycleSell(ERecycleContentType type, List<uint> ids)
    {
        if (ids == null || ids.Count < 1)
            return;
        List<ulong> list = new List<ulong>();
        for (int i = 0; i < ids.Count; i++)
        {
            ulong k = (ulong)ids[i];
            list.Add(k);
        }
        SendRecycleSell(type, list);
    }
    public void SendRecycleSell(ERecycleContentType type, List<ulong> ids)
    {
        if (ids == null || ids.Count < 1)
            return;
        RecycleReq req = new RecycleReq();
        req.type = (RecycleType)type;
        req.id.AddRange(ids);
        mStoreNetwork.SendRecycleSell(req);
    }
    public void ReceiveRecycleSell(RecycleResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateItem(resp.update_money);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.TIP_GET_RECYCLE, resp.update_money.change_num));
            PlayerData.Instance.MultipleAddWeapon(resp.equip_unload_list);
            PlayerData.Instance._LifeSoulDepot.AddPackLifeSouls(resp.life_unload_list);
            PlayerData.Instance.UpdateItem(resp.item_list);
            switch (resp.type)
            {
                case RecycleType.RT_PROP:
                case RecycleType.RT_MATERIAL:
                case RecycleType.RT_CHIP:
                case RecycleType.RT_SOLDIERSOUL:
                    {
                        //PlayerData.Instance.UpdateItem(resp.item_list);
                        //炼化武将也会有技能书等掉落，所以放外面了
                    }
                    break;
                case RecycleType.RT_EQUIP:
                    {
                        PlayerData.Instance._SoldierEquip.MultipleDelete(resp.equip_list);
                    }
                    break;
                case RecycleType.RT_SOLDIER:
                    {
                        PlayerData.Instance._SoldierDepot.multipleDelete(resp.equip_list);
                    }
                    break;
                case RecycleType.RT_LIFESOUL:
                    {
                        PlayerData.Instance._LifeSoulDepot.DeleteLifeSouls(resp.equip_list);
                    }
                    break;
                default:
                    break;
            }
            if (UISystem.Instance.UIIsOpen(RecycleView.UIName))
            {
                UISystem.Instance.RecycleView.ClearChoosedList((ERecycleContentType)resp.type);
                UISystem.Instance.RecycleView.ShowEffs();
                Assets.Script.Common.Scheduler.Instance.AddTimer(0.46f, false, UISystem.Instance.RecycleView.RefreshUI);
                //UISystem.Instance.RecycleView.RefreshUI();
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }
    public void SendOneKeyBuy()
    {
        BuyAllCommodityReq req = new BuyAllCommodityReq();
        req.shop_type = ShopType.ST_NomalShop;
        mStoreNetwork.SendOneKeyBuy(req);
    }
    public void ReceiveOneKeyBuy(BuyAllCommodityResp resp)
    {

        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateItem(resp.update_item);
            for (int i = 0; i < resp.update_equip.Count; i++)
            {
                PlayerData.Instance._SoldierEquip.OneAdd(resp.update_equip[i]);
            }
            for (int i = 0; i < resp.commodity_id.Count; i++)
            {
                PlayerData.Instance.UpdateBuyCommodity(resp.shop_type, resp.commodity_id[i]);
            }
            if (UISystem.Instance.UIIsOpen(StoreView.UIName))
            {
                UISystem.Instance.StoreView.ShowStore(resp.shop_type);
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void SendUpdateShopReq(UpdateType type, ShopType shop)
    {
        UpdateShopReq req = new UpdateShopReq();
        req.shop_type = shop;
        req.update_type = type;
        mStoreNetwork.SendUpdateShopReq(req);
    }

    public void ReceiveUpdateShopResp(UpdateShopResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateShopInfo(data.shop_info.shop_type, data.shop_info);
            PlayerData.Instance.UpdateItem(data.money_iteminfo);
            if (UISystem.Instance.UIIsOpen(StoreView.UIName))
            {
                UISystem.Instance.StoreView.ShowStore(data.shop_info);
            }
            UISystem.Instance.StoreView.PlayStoreRefreashEffect();
            UISystem.Instance.StoreView.SetRefreshCostNum();
        }
        else if (data.result == (int)ErrorCodeEnum.TimesLimited)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.NO_REFRESH_TIMES);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendBuyCommodityReq(ShopType shop, int commodityID)
    {
        BuyCommodityReq req = new BuyCommodityReq();
        req.shop_type = shop;
        req.commodity_id = commodityID;
        mStoreNetwork.SendBuyCommodityReq(req);
    }

    public void ReceiveBuyCommodityResp(BuyCommodityResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateItem(data.update_item);
            for (int i = 0; i < data.update_equip.Count; i++)
            {
                PlayerData.Instance._SoldierEquip.OneAdd(data.update_equip[i]);
            }
            PlayerData.Instance.UpdateBuyCommodity(data.shop_type, data.commodity_id);
            if (UISystem.Instance.UIIsOpen(StoreView.UIName))
            {
                UISystem.Instance.StoreView.CloseStoreItemDetail();
                UISystem.Instance.StoreView.ShowStore(data.shop_type);
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendGetCommodityOrderNum(uint id)
    {
        Debug.Log("SendGetCommodityOrderNum");
        GetCommodityOrderNumReq req = new GetCommodityOrderNumReq();
        req.commodity_id = id;
        req.plat_type = 1;
        _vipRechargeID = id;
        mStoreNetwork.SendGetCommodityOrderNum(req);
    }

    public void ReceiveGetCommodityOrderNum(GetCommodityOrderNumResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            //充值接口//
            if (GlobalConst.SERVER_TYPE != EServerType.Wai_Sifu
                && GlobalConst.SERVER_TYPE != EServerType.Wai_SifuB)
                return;

            RechargeShowData rechargeShowData = ConfigManager.Instance.mRechargeConfig.
                GetRechargeDataByID(uint.Parse(resp.commodity_id));
            if (rechargeShowData == null)
            {
                Debug.LogError("Recharge ERROR");
                return;
            }
            UISystem.Instance.ShowGameUI(RechargeWebMask.UIName);
            SFRecharge.Instance.SetMargins(0, 100, 0, 0);
            SFRecharge.Instance.OnSendRecharge(rechargeShowData.Price.ToString(), resp.order_num, LoginModule.Instance.CurServerId.ToString());
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void ReceiveNotifyRecharge(RechargeResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateDiamond(resp.diamond_num);
            PlayerData.Instance.UpdateVip(resp.vip_level, resp.vip_exp);
            PlayerData.Instance._SoldierEquipGrid = resp.equip_grid_num;
            PlayerData.Instance.HasMonthCard = resp.has_month_card;
            PlayerData.Instance.MonthCardDeadline = resp.month_card_deadline;
            PlayerData.Instance.NoFirstPayList = resp.first_recharge_list;
            PlayerData.Instance.MultipleAddWeapon(resp.equip_list);
            PlayerData.Instance.UpdatePlayerAttribute(resp.attr);
            PlayerData.Instance.WorldChatCount = (uint)resp.chat_surplus_num;
            PlayerData.Instance.UpdateVipRewardInfo(resp.vip_reward_info);
            PlayerData.Instance._PreyLifeSoulInfo.grid_num = resp.soul_grid_num;
            PlayerData.Instance._PetDepot.AddPetList(resp.pet_list);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BUY_SUCCESS);

            if ((GlobalConst.ISOPENSDKOPERATE) && ((GlobalConst.PLATFORM == TargetPlatforms.Android_7725) || (GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)))
            {
                SDKManager.Instance.NotifySDKRechargeInfo(resp.currency, resp.recharge_money.ToString());
            }

            if (UISystem.Instance.UIIsOpen(VipRechargeView.UIName)
                && UISystem.Instance.VipRechargeView.CurState == VipRechargeViewController.VipRechargeState.Recharge
                && _vipRechargeID != 0)
            {
                UISystem.Instance.VipRechargeView.RechargeFlayItweenAnim(_vipRechargeID);
                _vipRechargeID = 0;
            }
            if (UISystem.Instance.UIIsOpen(VipRechargeView.UIName))
            {
                UISystem.Instance.VipRechargeView.RefrashUI();
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void SendMallBuy(uint id, int num)
    {
        BuySpecialCommodityReq req = new BuySpecialCommodityReq();
        req.id = (int)id;
        req.num = num;
        mStoreNetwork.SendBuySpecialCommodity(req);
    }
    public void ReceiveMallBuy(BuySpecialCommodityResp resp)
    {
        if (resp.result == 0)
        {
            PlayerData.Instance.UpdateItem(resp.update_item);
            UISystem.Instance.CloseGameUI(ItemSellView.UIName);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BUY_SUCCESS);
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void SendGetVipUpReward(int level)
    {
        GetVipUpRewardReq req = new GetVipUpRewardReq();
        req.level = level;
        mStoreNetwork.SendGetVipUpReward(req);
    }

    public void ReceiveGetVipUpReward(GetVipUpRewardResp resp)
    {
        if (resp.result == 0)
        {
            GetVipReward(resp.vip_reward_info, resp.add_exp, resp.awards_common, resp.awards_equip, resp.awards_soldier);
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void SendVipDailyReward()
    {
        GetVipDailyRewardReq req = new GetVipDailyRewardReq();
        mStoreNetwork.SendVipDailyReward(req);
    }

    public void ReceiveVipDailyReward(GetVipDailyRewardResp resp)
    {
        if (resp.result == 0)
        {
            GetVipReward(resp.vip_reward_info, resp.add_exp, resp.awards_common, resp.awards_equip, resp.awards_soldier);
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    private void GetVipReward(VipRewardInfo info, AddExp exp, List<fogs.proto.msg.ItemInfo> itemInfos, List<Equip> equips, List<fogs.proto.msg.Soldier> soldiers)
    {
        PlayerData.Instance.UpdateVipRewardInfo(info);
        PlayerData.Instance.UpdateItem(itemInfos);
        PlayerData.Instance.MultipleAddWeapon(equips);
        PlayerData.Instance.MultipleAddSoldier(soldiers);
        PlayerData.Instance.UpdateExpInfo(exp);
        UISystem.Instance.VipRechargeView.RefreshGetBtn();

        List<CommonItemData> list = CommonFunction.GetCommonItemDataList(itemInfos, equips, soldiers);
        UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
        UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list);
    }

}
