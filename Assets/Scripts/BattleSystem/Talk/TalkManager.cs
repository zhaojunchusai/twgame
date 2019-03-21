using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 对话角色类型
/// </summary>
public enum EChatRoleType
{
    ecrtNone = 0,
    ecrtBOSS = 1,
    ecrtHero_Self = 2,
    ecrtHero_Enemy = 3,
    ecrtSoldier_Self = 4,
    ecrtSoldier_Enemy = 5,
    ecrtBarracks_Self = 6,
    ecrtBarracks_Enemy = 7,
    ecrtEscort = 8,
    ecrtShooter = 9,
    ecrtTranfer = 10
}

/// <summary>
/// 聊天泡文本显示类型
/// </summary>
public enum EChatTextType
{
    ecttCommon = 0,
    ecttAlways = 1,
    ecttInterval = 2
}

/// <summary>
/// 场景镜头移动类型
/// </summary>
public enum ESceneCameraMoveType
{
    /// <summary>
    /// 移动到指定位置
    /// </summary>
    escmtMove = 0,
    /// <summary>
    /// 直接设定到指定位置
    /// </summary>
    escmtReSet = 1
}

/// <summary>
/// 对话类型
/// </summary>
public enum EChatType
{
    ectNone = 0,
    ectNewGuide = 1,
    ectBossTalk = 2
}

/// <summary>
/// 战斗聊天泡信息
/// </summary>
public class TalkInfo
{
    /// <summary>
    /// 对话角色类型
    /// </summary>
    public EChatRoleType ChatRoleType;
    /// <summary>
    /// 移动目标位置
    /// </summary>
    public int TargetPosX;
    /// <summary>
    /// 场景镜头移动类型
    /// </summary>
    public ESceneCameraMoveType SceneMoveType;
    /// <summary>
    /// 聊天文本信息
    /// </summary>
    public string TextContent;
    /// <summary>
    /// 聊天泡文本显示类型
    /// </summary>
    public EChatTextType TextType;
    /// <summary>
    /// 文本显示时间
    /// </summary>
    public int TextTime;
    /// <summary>
    /// 镜头移动速度
    /// </summary>
    public int MoveSpeed;

    public TalkInfo(EChatRoleType vChatRoleType, int vTargetPosX, ESceneCameraMoveType vSceneMoveType, string vTextContent, EChatTextType vTextType, int vTextTime, int vMoveSpeed = 0)
    {
        ChatRoleType = vChatRoleType;
        TargetPosX = vTargetPosX;
        SceneMoveType = vSceneMoveType;
        TextContent = vTextContent;
        TextType = vTextType;
        TextTime = vTextTime;
        MoveSpeed = vMoveSpeed;
    }
}

/// <summary>
/// BOSS出场对话
/// </summary>
public class TalkManager : Singleton<TalkManager>
{

    /// <summary>
    /// 关卡ID
    /// </summary>
    private uint stageID;
    /// <summary>
    /// 对话类型
    /// </summary>
    private EChatType chatType;
    /// <summary>
    /// 聊天信息
    /// </summary>
    private List<TalkInfo> listTalkInfo;
    /// <summary>
    /// 当前场景
    /// </summary>
    private FightSceneBase pScene;
    /// <summary>
    /// 场景移动物件
    /// </summary>
    private TweenPosition pSceneMove;
    /// <summary>
    /// 聊天泡操作
    /// </summary>
    private ChatBubbleOperate pChatBubbleOperate;
    /// <summary>
    /// 镜头移动操作
    /// </summary>
    private SceneMoveOperate pSceneMoveOperate;
    /// <summary>
    /// 当前对话索引
    /// </summary>
    private int curTalkIndex;
    /// <summary>
    /// 当前对话角色层级
    /// </summary>
    private int curChatSortOrder;

    /// <summary>
    /// 当前对话角色层级
    /// </summary>
    public int Get_CurChatSortOrder
    {
        get {
            return curChatSortOrder;
        }
        set {
            curChatSortOrder = value;
        }
    }



    /// <summary>
    /// 初始化数据
    /// </summary>
    public void Initialize()
    {
        pChatBubbleOperate = ChatBubbleOperate.Instance;
        pSceneMoveOperate = SceneMoveOperate.Instance;
    }

    /// <summary>
    /// 销毁数据
    /// </summary>
    public void Uninitialize()
    {
        pChatBubbleOperate = null;
        pSceneMoveOperate = null;
    }


    /// <summary>
    /// 开启战斗对话
    /// </summary>
    /// <param name="vChatType"></param>
    public void OpenFightTalk(EChatType vChatType = EChatType.ectNone)
    {
        if (vChatType == EChatType.ectNone)
            return;
        Get_CurChatSortOrder = GlobalConst.TEMPORARY_SORT_ORDER;
        uint tmpSceneID = SceneManager.Instance.Get_FightID;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_BossInScene, vChatType);
    }

    /// <summary>
    /// 重新开启聊天泡设置
    /// </summary>
    /// <param name="vStageID"></param>
    /// <param name="vChatType"></param>
    public void ReStartTalk(uint vStageID, EChatType vChatType)
    {
        InitTalk();
        stageID = vStageID;
        chatType = vChatType;
        curTalkIndex = 0;
        pChatBubbleOperate.Initialize();
        pSceneMoveOperate.Initialize();
        GetTalkInfo();
        ShowTalkOperate();
    }



    /// <summary>
    /// 对话数据排序
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int SortListData(BossTalkInfo x, BossTalkInfo y)
    {
        if (x.ID > y.ID)
            return 1;
        else
            return -1;
    }

    /// <summary>
    /// 检测是否需要移动
    /// </summary>
    /// <param name="vTargetPosX"></param>
    /// <returns></returns>
    private bool CheckIsNeedMove(int vTargetPosX)
    {
        if (pSceneMove == null)
            return false;
        if (Mathf.Abs(pSceneMove.transform.localPosition.x - vTargetPosX) <= 20)
            return false;
        else
            return true;
    }

    /// <summary>
    /// 获取修正之后的目标X位置
    /// </summary>
    /// <param name="vInitPosX"></param>
    /// <returns></returns>
    private int GetOffsetTargetPosX(int vInitPosX)
    {
        if ((pScene == null) || (pSceneMove == null))
            return vInitPosX;
        int tmpLimitMin = (int)(pScene.limitRight - pScene.transOther.localPosition.x);
        //int tmpLimitMax = 0;
        int tmpLimitMax = (int)(pScene.limitLeft - pScene.transOther.localPosition.x);
        if (vInitPosX < tmpLimitMin)
            return tmpLimitMin;
        if (vInitPosX > tmpLimitMax)
            return tmpLimitMax;
        return vInitPosX;
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void InitTalk()
    {
        stageID = 0;
        chatType = EChatType.ectNone;
        if (listTalkInfo == null)
            listTalkInfo = new List<TalkInfo>();
        else
            listTalkInfo.Clear();
    }

    /// <summary>
    /// 获取聊天信息
    /// </summary>
    private void GetTalkInfo()
    {
        pScene = SceneManager.Instance.Get_CurScene;
        if ((pScene != null) && (pScene.transContent != null))
        {
            pSceneMove = pScene.transContent.gameObject.GetComponent<TweenPosition>();
        }

        if (chatType == EChatType.ectNone)
            return;

        List<BossTalkInfo> tmpListBossTalkData = ConfigManager.Instance.mBossTalkConfig.FindByStageID(stageID);
        if (tmpListBossTalkData == null)
            return;

        tmpListBossTalkData.Sort(SortListData);
        for (int i = 0; i < tmpListBossTalkData.Count; i++)
        {
            if (tmpListBossTalkData[i].MessageType != (int)chatType)
                continue;
            listTalkInfo.Add(new TalkInfo((EChatRoleType)tmpListBossTalkData[i].RoleType, GetOffsetTargetPosX(tmpListBossTalkData[i].TargetPosX),
                                            (ESceneCameraMoveType)tmpListBossTalkData[i].MoveType, tmpListBossTalkData[i].Text, 
                                            (EChatTextType)tmpListBossTalkData[i].TextType, tmpListBossTalkData[i].Time, tmpListBossTalkData[i].MoveSpeed));
        }
    }

    /// <summary>
    /// 开启操作
    /// </summary>
    private void ShowTalkOperate()
    {
        if ((listTalkInfo == null) || (listTalkInfo.Count <= 0))
        {
            CloseTalkOperate();
            return;
        }
        if ((curTalkIndex < 0) || (curTalkIndex >= listTalkInfo.Count) || (listTalkInfo[curTalkIndex] == null))
        {
            CloseTalkOperate();
            return;
        }

        //优先移动镜头//
        if (CheckIsNeedMove(listTalkInfo[curTalkIndex].TargetPosX))
        {
            ShowSceneMoveOperate();
        }
        else
        {
            ShowChatBubbleOperate();
        }
    }

    /// <summary>
    /// 场景移动操作
    /// </summary>
    private void ShowSceneMoveOperate()
    {
        pSceneMoveOperate.ReSetPosition(pScene, pSceneMove, listTalkInfo[curTalkIndex].SceneMoveType, listTalkInfo[curTalkIndex].TargetPosX, listTalkInfo[curTalkIndex].MoveSpeed, ShowChatBubbleOperate);
    }

    /// <summary>
    /// 显示聊天泡操作
    /// </summary>
    private void ShowChatBubbleOperate()
    {
        curTalkIndex += 1;
        pChatBubbleOperate.ReSetStatus(listTalkInfo[curTalkIndex - 1], ShowTalkOperate);
    }

    /// <summary>
    /// 关闭操作
    /// </summary>
    private void CloseTalkOperate()
    {
        if (pSceneMove != null)
            pSceneMove.transform.localPosition = Vector3.zero;
        pChatBubbleOperate.CloseShowChat();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_CloseShowBossTalk);
        GuideManager.Instance.CheckTrigger(GuideTrigger.FightSceneTalkFinish);
    }
}