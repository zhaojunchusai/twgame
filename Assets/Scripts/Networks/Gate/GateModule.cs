using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using System;
using System.Collections;
using Assets.Script.Common;
using fogs.proto.msg;

public class GateModule : Singleton<GateModule> 
{
    public GateNetWork mGateNetWork;
    public void Initialize()
    {
        if (mGateNetWork == null) 
        {
            mGateNetWork = new GateNetWork();
            mGateNetWork.RegisterMsg();
        }
    }

    public void SendGuideGateGiftReq(uint id)
    {
        Debug.Log("SendGuideGateGiftReq " + id);
        SpecialItemReq req = new SpecialItemReq();
        req.id = id;
        mGateNetWork.SendGuideGateGiftReq(req);
    }

    public void SendGuideGateGiftResp(SpecialItemResp tmpData)
    {
        Debug.Log("SendGuideGateGiftResp " + tmpData.result);
        if (tmpData.result == 0)
        {
            if (tmpData.equip != null)
            {
                PlayerData.Instance.MultipleAddWeapon(new List<Equip>(tmpData.equip));
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETSPECIALITEM);
                //List<CommonItemData> list = new List<CommonItemData>();
                //for (int i = 0; i < tmpData.equip.Count; i++)
                //{
                //    list.Add(new CommonItemData(tmpData.equip[i].id, 0,true));
                //}
                if (tmpData.equip.Count > 0)
                UISystem.Instance.GetSpecialItemView.SetInfo(new CommonItemData(tmpData.equip[0].id, 0));
            }
            if (tmpData.soldier != null && tmpData.soldier.Count > 0)
            {
                List<Soldier> list = PlayerData.Instance.MultipleAddSoldier(new List<fogs.proto.msg.Soldier>(tmpData.soldier));
                if (list.Count > 0)
                {
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETSPECIALITEM);
                    UISystem.Instance.GetSpecialItemView.SetInfo(list[0]);
                }
            }
            if (tmpData.item != null)
            {
                PlayerData.Instance.UpdateItem(tmpData.item);
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETSPECIALITEM);
                List<CommonItemData> list = new List<CommonItemData>();
                for (int i = 0; i < tmpData.item.Count; i++)
                {
                    list.Add(new CommonItemData(tmpData.item[i].id, tmpData.item[i].change_num, true));
                }
                if (list.Count > 0)
                {
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETSPECIALITEM);
                    UISystem.Instance.GetSpecialItemView.SetInfo(list[0]);
                }
            }
        }
    }

    public void Uninitialize() 
    {
        if (mGateNetWork != null)
            mGateNetWork.RemoveMsg();
        mGateNetWork = null;
    }
}
