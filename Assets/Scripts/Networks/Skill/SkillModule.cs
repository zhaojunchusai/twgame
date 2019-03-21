using UnityEngine;
using System;
using System.Collections;
using Assets.Script.Common;
using fogs.proto.msg;
using ProtoBuf;
using Assets.Script.Common.StateMachine;

public class SkillModule : Singleton<SkillModule>
{
    public SkillNetWork mSkillNetWork;
    public void Initialize()
    {
        if (mSkillNetWork == null) 
        {
            mSkillNetWork = new SkillNetWork();
            mSkillNetWork.RegisterMsg();
        }
    }
    public void Uninitialize() 
    {
        if (mSkillNetWork != null)
            mSkillNetWork.RemoveMsg();
        mSkillNetWork = null;
    }
    public void SendUpgradeSkillReq(UInt64 uId,uint id, int Level)
    {
        UpgradeSkillReq temp = new UpgradeSkillReq();
        
        temp.owner_id = uId;
        temp.skill_id = (UInt32)id;
        temp.skill_level = (UInt32)Level;

        mSkillNetWork.SendUpgradeSkillReq(temp);
    }
    public void SendFinishGuideStepReq(uint id)
    {
        FinishGuideStepReq temp = new FinishGuideStepReq();
        temp.step = id;
        //mSkillNetWork.SendFinishGuideStepReq(temp);
    }
    public void SendAutoUpgradeSkillReq(UInt64 owner,uint id)
    {
        AutoUpgradeSkillReq temp = new AutoUpgradeSkillReq();
        temp.owner_id = owner;
        temp.skill_id = id;
        mSkillNetWork.SendAutoUpgradeSkillReq(temp);
    }
    public void ReceiveFinishGuideStepResp(Packet data)
    {
        FinishGuideStepResp tData = Serializer.Deserialize<FinishGuideStepResp>(data.ms);

    }
    public void ReceiveAutoUpgradeSkillResp(Packet data)
    {
        this.ReceiveUpgradeSkillResp(data);
    }
    public void ReceiveUpgradeSkillResp(Packet data)
    {
        UpgradeSkillResp tData = Serializer.Deserialize<UpgradeSkillResp>(data.ms);
        if (tData == null) return;
        if(tData.result == 0)
        {
            PlayerData.Instance.UpdateItem(tData.ramain_material);
            //PlayerData.Instance.MoneyRefresh((int)tData.money_type,(int)tData.money_value);
            if (UISystem.Instance.TopFuncView != null)
                UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
            if (tData.owner_id == 0)
            {
                PlayerData.Instance._SkillsDepot.ReceiveUpgradeSkillResp(tData);
                PlayerData.Instance.UpdatePlayerAttribute(tData.attribute);
                if (UISystem.Instance.HeroAttView != null && UISystem.Instance.HeroAttView.rightPanel != null && UISystem.Instance.HeroAttView.rightPanel.skillIntensifyPanel != null)
                    UISystem.Instance.HeroAttView.rightPanel.skillIntensifyPanel.PlaySkillIntensifyEffect();
            }
            else
            {
                Soldier sd = PlayerData.Instance._SoldierDepot.FindByUid(tData.owner_id);
                if (sd == null) return;
                if (sd._skillsDepot == null) return;
                sd._skillsDepot.ReceiveUpgradeSkillResp(tData);
                sd.SerializeShowInfo(tData.attribute);
                if (UISystem.Instance.SoldierAttView != null && UISystem.Instance.SoldierAttView.rightPanel != null && UISystem.Instance.SoldierAttView.rightPanel.skillIntensifyPanel != null)
                    UISystem.Instance.SoldierAttView.rightPanel.skillIntensifyPanel.PlaySkillIntensifyEffect();
            }
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.STRENGTH_SUCESS);
            if (UISystem.Instance.TopFuncView != null)
                UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
        }
        else
        {
            if (tData.owner_id == 0 && tData.result == (int)ErrorCodeEnum.HeroLevelLower)
            {
                Skill temp = PlayerData.Instance._SkillsDepot.FindById(tData.old_skill_id);
                if (temp == null)
                    ErrorCode.ShowErrorTip(tData.result);
                else
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.HERO_LV_ONTENOUGH, Mathf.CeilToInt((temp.Level + 1) * temp.StrongCoefficient())));
                }
            }
            else
            {
                ErrorCode.ShowErrorTip(tData.result);
            }
        }
        if (tData.owner_id == 0)
        {
            PlayerData.Instance._SkillsDepot.OnSkillsError(SkillControl.UpgradeSkillResp,tData.result);
        }
        else
        {
            Soldier sd = PlayerData.Instance._SoldierDepot.FindByUid(tData.owner_id);
            if (sd == null) return;
            if (sd._skillsDepot == null) return;
            sd._skillsDepot.OnSkillsError(SkillControl.UpgradeSkillResp, tData.result);
        }
    }
}
