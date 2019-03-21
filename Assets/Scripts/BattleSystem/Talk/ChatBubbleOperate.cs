using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 战斗对话聊天泡管理
/// </summary>
public class ChatBubbleOperate : Singleton<ChatBubbleOperate>
{

    private Dictionary<EChatRoleType, RoleChatBubble> dicRoleChatBubble;


    /// <summary>
    /// 初始化数据
    /// </summary>
    public void Initialize()
    {
        DeleteOperate();
        if (dicRoleChatBubble == null)
            dicRoleChatBubble = new Dictionary<EChatRoleType, RoleChatBubble>();
        else
            dicRoleChatBubble.Clear();
    }

    /// <summary>
    /// 销毁数据
    /// </summary>
    public void Uninitialize()
    {
        DeleteOperate();
        dicRoleChatBubble = null;
    }


    /// <summary>
    /// 设置对话聊天泡
    /// </summary>
    /// <param name="vTalkInfo"></param>
    public void ReSetStatus(TalkInfo vTalkInfo, Scheduler.OnScheduler vCallFunction = null)
    {
        if (vTalkInfo == null)
        {
            if (vCallFunction != null)
                vCallFunction();
            return;
        }
        if ((string.IsNullOrEmpty(vTalkInfo.TextContent)) || (vTalkInfo.TextContent.Equals("0")))
        {
            if (vCallFunction != null)
                Scheduler.Instance.AddTimer(CommonFunction.GetSecondTimeByMilliSecond(vTalkInfo.TextTime), false, vCallFunction);
            return;
        }

        if (dicRoleChatBubble.ContainsKey(vTalkInfo.ChatRoleType))
        {
            if ((dicRoleChatBubble[vTalkInfo.ChatRoleType] != null) && (dicRoleChatBubble[vTalkInfo.ChatRoleType].Get_IsLive))
            {
                dicRoleChatBubble[vTalkInfo.ChatRoleType].ShowChatText(vTalkInfo.TextContent, vTalkInfo.TextType, vTalkInfo.TextTime, vCallFunction);
                return;
            }
            dicRoleChatBubble.Remove(vTalkInfo.ChatRoleType);
        }

        RoleAttribute tmpRoleAttribute = null;
        if (vTalkInfo.ChatRoleType == EChatRoleType.ecrtNone)
        { }
        else if (vTalkInfo.ChatRoleType == EChatRoleType.ecrtBarracks_Self)
        {
            if (SceneManager.Instance.DicSceneMarkObj.ContainsKey(ESceneMarkType.esmtBarracks_Self))
                tmpRoleAttribute = SceneManager.Instance.DicSceneMarkObj[ESceneMarkType.esmtBarracks_Self];
        }
        else if (vTalkInfo.ChatRoleType == EChatRoleType.ecrtBarracks_Enemy)
        {
            if (SceneManager.Instance.DicSceneMarkObj.ContainsKey(ESceneMarkType.esmtBarracks_Enemy))
                tmpRoleAttribute = SceneManager.Instance.DicSceneMarkObj[ESceneMarkType.esmtBarracks_Enemy];
        }
        else if (vTalkInfo.ChatRoleType == EChatRoleType.ecrtTranfer)
        {
            if (SceneManager.Instance.DicSceneMarkObj.ContainsKey(ESceneMarkType.esmtTransfer))
                tmpRoleAttribute = SceneManager.Instance.DicSceneMarkObj[ESceneMarkType.esmtTransfer];
        }
        else
        {
            foreach (KeyValuePair<int, RoleBase> tmpSingleRole in RoleManager.Instance.Get_RoleDic)
            {
                if (!CheckRoleTypeIsRight(tmpSingleRole.Value, vTalkInfo.ChatRoleType))
                    continue;
                tmpRoleAttribute = tmpSingleRole.Value;
                break;
            }
        }

        if (tmpRoleAttribute != null)
        {
            RoleChatBubble tmpRoleChatBubble = new RoleChatBubble(tmpRoleAttribute, vTalkInfo.TextContent, vTalkInfo.TextType, vTalkInfo.TextTime, vCallFunction);
            dicRoleChatBubble.Add(vTalkInfo.ChatRoleType, tmpRoleChatBubble);
        }
        else
        {
            if (vCallFunction != null)
                Scheduler.Instance.AddTimer(CommonFunction.GetSecondTimeByMilliSecond(vTalkInfo.TextTime), false, vCallFunction);
        }
    }

    /// <summary>
    /// 销毁操作-战斗结束时调用
    /// </summary>
    public void DeleteOperate()
    {
        if (dicRoleChatBubble == null)
            return;
        CloseShowChat();
        dicRoleChatBubble.Clear();
    }

    /// <summary>
    /// 关闭聊天
    /// </summary>
    public void CloseShowChat()
    {
        if (dicRoleChatBubble == null)
            return;
        foreach (KeyValuePair<EChatRoleType, RoleChatBubble> tmpSingleInfo in dicRoleChatBubble)
        {
            if (tmpSingleInfo.Value == null)
                continue;
            tmpSingleInfo.Value.ClearOperate();
        }
    }

    /// <summary>
    /// 检测角色类型是否匹配
    /// </summary>
    /// <param name="vRoleBase"></param>
    /// <param name="vChatRoleType"></param>
    /// <returns>true-类型匹配 false-类型不匹配</returns>
    private bool CheckRoleTypeIsRight(RoleBase vRoleBase, EChatRoleType vChatRoleType)
    {
        bool tmpResult = false;
        if (vRoleBase == null)
            return tmpResult;
        if (!vRoleBase.IsLive())
            return tmpResult;

        switch (vChatRoleType)
        {
            case EChatRoleType.ecrtBOSS:
                {
                    if (vRoleBase.Get_RoleType == ERoleType.ertMonster)
                    {
                        MonsterAttributeInfo tmpMonster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(vRoleBase.Get_FightAttribute.KeyData);
                        if (tmpMonster != null)
                        {
                            if (tmpMonster.IsBoss == GlobalConst.MONSTER_TYPE_BOSS)
                                tmpResult = true;
                        }
                    }
                    break;
                }
            case EChatRoleType.ecrtHero_Self:
                {
                    if (vRoleBase.Get_RoleType == ERoleType.ertHero)
                    {
                        if (vRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
                            tmpResult = true;
                    }
                    break;
                }
            case EChatRoleType.ecrtHero_Enemy:
                {
                    if (vRoleBase.Get_RoleType == ERoleType.ertHero)
                    {
                        if (vRoleBase.Get_RoleCamp == EFightCamp.efcEnemy)
                            tmpResult = true;
                    }
                    break;
                }
            case EChatRoleType.ecrtSoldier_Self:
                {
                    if (vRoleBase.Get_RoleType == ERoleType.ertSoldier)
                    {
                        if (vRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
                            tmpResult = true;
                    }
                    break;
                }
            case EChatRoleType.ecrtSoldier_Enemy:
                {
                    if ((vRoleBase.Get_RoleType == ERoleType.ertSoldier) || (vRoleBase.Get_RoleType == ERoleType.ertMonster))
                    {
                        if (vRoleBase.Get_RoleCamp == EFightCamp.efcEnemy)
                            tmpResult = true;
                    }
                    break;
                }
            case EChatRoleType.ecrtBarracks_Self:
                {
                    if (vRoleBase.Get_RoleType == ERoleType.ertBarracks)
                    {
                        if (vRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
                            tmpResult = true;
                    }
                    break;
                }
            case EChatRoleType.ecrtBarracks_Enemy:
                {
                    if (vRoleBase.Get_RoleType == ERoleType.ertBarracks)
                    {
                        if (vRoleBase.Get_RoleCamp == EFightCamp.efcEnemy)
                            tmpResult = true;
                    }
                    break;
                }
            case EChatRoleType.ecrtEscort:
                {
                    if (vRoleBase.Get_RoleType == ERoleType.ertEscort)
                    {
                        tmpResult = true;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }

        return tmpResult;
    }
}