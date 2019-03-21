using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;

public class RecruitModule : Singleton<RecruitModule>
{
    private RecruitNetWork mNetWork;

    public void Initialize()
    {
        if (mNetWork == null)
        {
            mNetWork = new RecruitNetWork();
            mNetWork.RegisterMsg();
        }
    }

    public void SendOneRecruit(int type ) 
    {
        OneRecruitReq data = new OneRecruitReq();
        data.recruit_type = (RecruitType)type;
        mNetWork.SendOneRecruit(data);
    }

    public void SendTenRecruit(int type) 
    {
        MultipleRecruitReq data = new MultipleRecruitReq();
        data.recruit_type = (RecruitType)type;
        mNetWork.SendTenRecruit(data);
    }

    public void ReceiveOneRecruit(OneRecruitResp data) 
    {
        if (data.result != 0) 
        {
            ErrorCode.ShowErrorTip(data.result);
            return;
        }
        switch (data.cost_type)
        {
            case CostType.CT_UseNothing:
                switch (data.recruit_type)
                {
                    case RecruitType.RT_Brave:
                        PlayerData.Instance.LastBraveRecruitTime = data.recruit_surplus_tm;
                        PlayerData.Instance.RecruitFreeCount = (byte)data.recruit_surplus_count;
                        break;
                    case RecruitType.RT_PutDownRiot:
                        PlayerData.Instance.LastRiotRecruitTime = data.recruit_surplus_tm;
                        break;
                }
                break;
            case CostType.CT_UseMoney:
                switch ((ECurrencyType)data.money_type)
                {
                    case ECurrencyType.Gold:
                        PlayerData.Instance.UpdateGold(data.money_num);
                        break;
                    case ECurrencyType.Diamond:
                        PlayerData.Instance.UpdateDiamond(data.money_num);
                        break;
                }
                break;
            case CostType.CT_UseProp:
                PlayerData.Instance.UpdateItemInfoData(data.update_item);
                break;
        }
        UISystem.Instance.RecruitView.ReceiveOneRecruit(data);
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenRecruitResult);

    }

    public void ReceiveTenRecruit(MultipleRecruitResp data)
    {
        if (data.result != 0)
        {
            ErrorCode.ShowErrorTip(data.result);
            return;
        }
        switch (data.cost_type)
        {
            case CostType.CT_UseMoney:
                switch ((ECurrencyType)data.money_type)
                {
                    case ECurrencyType.Gold:
                        PlayerData.Instance.UpdateGold(data.money_num);
                        break;
                    case ECurrencyType.Diamond:
                        PlayerData.Instance.UpdateDiamond(data.money_num);
                        break;
                }
                break;

            case CostType.CT_UseProp:
                PlayerData.Instance.UpdateItemInfoData(data.update_item);
                break;

        }
        UISystem.Instance.RecruitView.ReceiveMultipleRecruit(data);
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenRecruitResult);
    }

    public void Uninitialize()
    {
        mNetWork.RemoveMsg();
        mNetWork = null;
    }
}
