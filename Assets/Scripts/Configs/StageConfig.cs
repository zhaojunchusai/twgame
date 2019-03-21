using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
/// <summary>
/// 前置关卡信息
/// </summary>
public class PreStageInfo
{
    public uint StageID;
    public byte StarNum;

    public PreStageInfo()
    {
        Init();
    }

    public void Init()
    {
        StageID = 0;
        StarNum = 0;
    }

    public void ReSet(uint vStageID, byte vStarNum)
    {
        Init();
        StageID = vStageID;
        StarNum = vStarNum;
    }

    public void Copy(PreStageInfo vInfo)
    {
        if (vInfo == null)
            return;
        ReSet(vInfo.StageID, vInfo.StarNum);
    }
}

/// <summary>
/// 敌方阵容单个怪物信息
/// </summary>
public class SingleEnemyInfo
{
    public uint MonsterID;
    /// <summary>
    /// [0-非Boss 1-Boss]
    /// </summary>
    public byte IsBoss;

    public SingleEnemyInfo()
    {
        Init();
    }

    public void Init()
    {
        MonsterID = 0;
        IsBoss = 0;
    }

    public void ReSet(uint vMonsterID, byte vIsBoss)
    {
        Init();
        MonsterID = vMonsterID;
        IsBoss = vIsBoss;
    }

    public void Copy(SingleEnemyInfo vInfo)
    {
        if (vInfo == null)
            return;
        ReSet(vInfo.MonsterID, vInfo.IsBoss);
    }
}


public class StageInfo
{
    /// <summary>
    /// 关卡ID
    /// </summary>
    public uint ID;
    /// <summary>
    /// 关卡名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 上一关卡ID
    /// </summary>
    public uint PrevID;
    /// <summary>
    /// 下一关卡ID
    /// </summary>
    public uint NextID;
    /// <summary>
    /// 玩法大类
    /// </summary>
    public int PlayType;
    /// <summary>
    /// 章节编号
    /// </summary>
    public uint ChapterID;
    /// <summary>
    /// 小关编号
    /// </summary>
    public int SmallGateID;
    /// <summary>
    /// 是否精英关卡
    /// </summary>
    public int IsElite;
    /// <summary>
    /// 大小关卡判断
    /// </summary>
    public bool IsBoss;
    /// <summary>
    /// 图标资源
    /// </summary>
    public string BgTexture;
    /// <summary>
    /// 头像资源
    /// </summary>
    public string BossIcon;
    /// <summary>
    /// 关卡坐标
    /// </summary>
    public Vector2 IconPos;
    /// <summary>
    /// 挑战次数
    /// </summary>
    public int ChallengeCount;
    /// <summary>
    /// 时间限制
    /// </summary>
    public int LimitTime;
    /// <summary>
    /// 消耗体力
    /// </summary>
    public int Physical;
    /// <summary>
    /// 解锁等级
    /// </summary>
    public int UnlockLV;
    /// <summary>
    /// 前置关卡:星级
    /// </summary>
    public List<PreStageInfo> PreStage;
    /// <summary>
    /// 后置关卡
    /// </summary>
    public List<uint> NextStage;
    /// <summary>
    /// 初始魔法
    /// </summary>
    public int InitMagic;
    /// <summary>
    /// 初始能量
    /// </summary>
    public int InitEnergy;
    /// <summary>
    /// 关卡掉落包ID
    /// </summary>
    public uint DropID;
    /// <summary>
    /// 关卡标识
    /// </summary>
    public List<int> MarkInfo;
    /// <summary>
    /// 战斗模式
    /// </summary>
    public int FireType;
    /// <summary>
    /// 己方城堡血量
    /// </summary>
    public int OwnBaseHP;
    /// <summary>
    /// 敌方城堡血量
    /// </summary>
    public int EnemyBaseHP;
    /// <summary>
    /// 标志点坐标
    /// </summary>
    public int MarkPos;
    /// <summary>
    /// 护送目标资源
    /// </summary>
    public string EscortTarget;
    /// <summary>
    /// 护送目标移动速度
    /// </summary>
    public int EscortTargetSpeed;
    /// <summary>
    /// 护送目标血量
    /// </summary>
    public int EscortTargetHP;
    /// <summary>
    /// 最大敌人数量
    /// </summary>
    public int MaxEnemyCount;
    /// <summary>
    /// 标准时长
    /// </summary>
    public int NormTime;
    /// <summary>
    /// 三星评定数值
    /// </summary>
    public int Star3;
    /// <summary>
    /// 二星评定数值
    /// </summary>
    public int Star2;
    /// <summary>
    /// 一星评定数值
    /// </summary>
    public int Star1;
    /// <summary>
    /// 关卡描述
    /// </summary>
    public string Describe;
    /// <summary>
    /// 敌方阵容ID
    /// </summary>
    public List<SingleEnemyInfo> EnemySquad;
    /// <summary>
    /// 背景资源
    /// </summary>
    public string BackGroundID;
    /// <summary>
    /// 城堡类型
    /// </summary>
    public byte CastleType;
    /// <summary>
    /// BOSS出场提示时间
    /// </summary>
    public int BossHintTime;
    /// <summary>
    /// 关卡序列
    /// </summary>
    public string GateSequence;
    /// <summary>
    /// 异域探险BOSS简介
    /// </summary>
    public string BossDesc;
    /// <summary>
    /// 关卡自动战斗初始状态[0-锁定 1-未激活 2-激活全部 3-激活战斗 4-激活召唤]
    /// </summary>
    public int AutoStatus;
    /// <summary>
    /// 敌方城堡图片资源[没有就不填数据]
    /// </summary>
    public string EnemyBarracksPic;

    private void Init()
    {
        ID = 0;
        Name = string.Empty;
        PlayType = 0;
        ChapterID = 0;
        SmallGateID = 0;
        IsElite = 0;
        ChallengeCount = 0;
        LimitTime = 0;
        Physical = 0;
        UnlockLV = 0;

        if (PreStage == null)
            PreStage = new List<PreStageInfo>();
        PreStage.Clear();
        if (NextStage == null)
            NextStage = new List<uint>();
        NextStage.Clear();
        InitMagic = 0;
        InitEnergy = 0;
        DropID = 0;

        if (MarkInfo == null)
            MarkInfo = new List<int>();
        MarkInfo.Clear();

        FireType = 0;
        OwnBaseHP = 0;
        EnemyBaseHP = 0;
        MarkPos = 0;
        EscortTarget = string.Empty;
        EscortTargetSpeed = 0;
        EscortTargetHP = 0;
        MaxEnemyCount = 0;
        NormTime = 0;
        Star3 = 0;
        Star2 = 0;
        Star1 = 0;
        Describe = string.Empty;
        if (EnemySquad == null)
            EnemySquad = new List<SingleEnemyInfo>();
        EnemySquad.Clear();

        PrevID = 0;
        NextID = 0;
        IsBoss = false;
        BgTexture = string.Empty;
        BossIcon = string.Empty;
        IconPos = Vector2.zero;
        BackGroundID = string.Empty;
        CastleType = 0;
        BossHintTime = 0;
        GateSequence = string.Empty;
        AutoStatus = 0;
        EnemyBarracksPic = "";
    }

    public void CopyTo(StageInfo vInfo)
    {
        if (vInfo == null)
            return;
        Init();

        this.ID = vInfo.ID;
        this.Name = vInfo.Name;
        this.PrevID = vInfo.PrevID;
        this.NextID = vInfo.NextID;
        this.PlayType = vInfo.PlayType;
        this.ChapterID = vInfo.ChapterID;
        this.SmallGateID = vInfo.SmallGateID;
        this.IsElite = vInfo.IsElite;
        this.ChallengeCount = vInfo.ChallengeCount;
        this.LimitTime = vInfo.LimitTime;
        this.Physical = vInfo.Physical;
        this.UnlockLV = vInfo.UnlockLV;
        this.PreStage.AddRange(vInfo.PreStage);
        this.NextStage.AddRange(vInfo.NextStage);
        this.InitMagic = vInfo.InitMagic;
        this.InitEnergy = vInfo.InitEnergy;
        this.DropID = vInfo.DropID;
        this.MarkInfo.AddRange(vInfo.MarkInfo);
        this.FireType = vInfo.FireType;
        this.OwnBaseHP = vInfo.OwnBaseHP;
        this.EnemyBaseHP = vInfo.EnemyBaseHP;
        this.MarkPos = vInfo.MarkPos;
        this.EscortTarget = vInfo.EscortTarget;
        this.EscortTargetSpeed = vInfo.EscortTargetSpeed;
        this.EscortTargetHP = vInfo.EscortTargetHP;
        this.MaxEnemyCount = vInfo.MaxEnemyCount;
        this.NormTime = vInfo.NormTime;
        this.Star3 = vInfo.Star3;
        this.Star2 = vInfo.Star2;
        this.Star1 = vInfo.Star1;
        this.Describe = vInfo.Describe;
        this.EnemySquad.AddRange(vInfo.EnemySquad);
        this.BgTexture = vInfo.BgTexture;
        this.BossIcon = vInfo.BossIcon;
        this.IconPos = vInfo.IconPos;
        this.IsBoss = vInfo.IsBoss;
        this.BackGroundID = vInfo.BackGroundID;
        this.CastleType = vInfo.CastleType;
        this.BossHintTime = vInfo.BossHintTime;
        this.GateSequence = vInfo.GateSequence;
        this.BossDesc = vInfo.BossDesc;
        this.AutoStatus = vInfo.AutoStatus;
        this.EnemyBarracksPic = vInfo.EnemyBarracksPic;
    }

}

public class StageConfig : BaseConfig
{
    private List<StageInfo> _stageList;

    public StageConfig()
    {
        _stageList = new List<StageInfo>();
        Initialize(GlobalConst.Config.DIR_XML_STAGE, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void Uninitialize()
    {
        if (_stageList != null)
        {
            _stageList.Clear();
            _stageList = null;
        }
    }

    public List<StageInfo> GetStageList()
    {
        return _stageList;
    }


    public List<StageInfo> GetStageListByIDs(List<uint> array)
    {
        List<StageInfo> list = new List<StageInfo>();
        for (int i = 0; i < array.Count; i++)
        {
            uint _gateID = array[i];
            for (int j = 0; j < _stageList.Count; j++)
            {
                StageInfo _stageinfo = _stageList[j];
                if (_stageinfo.ID == _gateID)
                {
                    StageInfo info = new StageInfo();
                    info.CopyTo(_stageinfo);
                    list.Add(info);
                    break;
                }
            }
        }
        return list;
    }
    /// <summary>
    /// 通过关卡ID数组 获取信息
    /// </summary>
    /// <param name="array"></param>
    public List<StageInfo> GetStagesByID(List<uint> array, byte isElite)
    {
        List<StageInfo> list = new List<StageInfo>();
        for (int i = 0; i < array.Count; i++)
        {
            uint _gateID = array[i];
            for (int j = 0; j < _stageList.Count; j++)
            {
                StageInfo _stageinfo = _stageList[j];
                if (_stageinfo.PlayType == (int)EFightType.eftMain && _stageinfo.ID == _gateID && _stageinfo.IsElite == isElite)
                {
                    StageInfo info = new StageInfo();
                    info.CopyTo(_stageinfo);
                    list.Add(info);
                    break;
                }
            }
        }
        return list;
    }

    public StageInfo CopyData(StageInfo vInfo)
    {
        StageInfo _info = new StageInfo();
        _info.ID = vInfo.ID;
        _info.NextID = vInfo.NextID;
        _info.PrevID = vInfo.PrevID;
        _info.Name = vInfo.Name;
        _info.PlayType = vInfo.PlayType;
        _info.ChapterID = vInfo.ChapterID;
        _info.SmallGateID = vInfo.SmallGateID;
        _info.IsElite = vInfo.IsElite;
        _info.ChallengeCount = vInfo.ChallengeCount;
        _info.LimitTime = vInfo.LimitTime;
        _info.Physical = vInfo.Physical;
        _info.UnlockLV = vInfo.UnlockLV;
        _info.NextStage = vInfo.NextStage;
        _info.InitMagic = vInfo.InitMagic;
        _info.InitEnergy = vInfo.InitEnergy;
        _info.DropID = vInfo.DropID;
        _info.MarkInfo = new List<int>();
        for (int i = 0; i < vInfo.MarkInfo.Count; i++)
        {
            _info.MarkInfo.Add(vInfo.MarkInfo[i]);
        }
        _info.FireType = vInfo.FireType;
        _info.OwnBaseHP = vInfo.OwnBaseHP;
        _info.EnemyBaseHP = vInfo.EnemyBaseHP;
        _info.MarkPos = vInfo.MarkPos;
        _info.EscortTarget = vInfo.EscortTarget;
        _info.EscortTargetSpeed = vInfo.EscortTargetSpeed;
        _info.EscortTargetHP = vInfo.EscortTargetHP;
        _info.MaxEnemyCount = vInfo.MaxEnemyCount;
        _info.NormTime = vInfo.NormTime;
        _info.Star3 = vInfo.Star3;
        _info.Star2 = vInfo.Star2;
        _info.Star1 = vInfo.Star1;
        _info.Describe = vInfo.Describe;
        _info.EnemySquad = new List<SingleEnemyInfo>();
        for (int i = 0; i < vInfo.EnemySquad.Count; i++)
        {
            SingleEnemyInfo _enemy = new SingleEnemyInfo();
            _enemy.IsBoss = vInfo.EnemySquad[i].IsBoss;
            _enemy.MonsterID = vInfo.EnemySquad[i].MonsterID;
            _info.EnemySquad.Add(_enemy);
        }
        _info.BgTexture = vInfo.BgTexture;
        _info.BossIcon = vInfo.BossIcon;
        _info.IconPos = vInfo.IconPos;
        _info.IsBoss = vInfo.IsBoss;
        _info.BackGroundID = vInfo.BackGroundID;
        _info.CastleType = vInfo.CastleType;
        _info.BossHintTime = vInfo.BossHintTime;
        for (int i = 0; i < vInfo.PreStage.Count; i++)
        {
            PreStageInfo info = vInfo.PreStage[i];
            _info.PreStage.Add(info);
        }
        _info.GateSequence = vInfo.GateSequence;
        _info.BossDesc = vInfo.BossDesc;
        _info.AutoStatus = vInfo.AutoStatus;
        _info.EnemyBarracksPic = vInfo.EnemyBarracksPic;
        return _info;
    }


    public List<StageInfo> GetActivityStagesByID(List<uint> array, EFightType type)
    {
        List<StageInfo> list = new List<StageInfo>();
        for (int i = 0; i < array.Count; i++)
        {
            uint _gateID = array[i];
            for (int j = 0; j < _stageList.Count; j++)
            {
                StageInfo _stageinfo = _stageList[j];
                if (_stageinfo.PlayType == (int)EFightType.eftActivity && _stageinfo.ID == _gateID)
                {
                    list.Add(_stageinfo);
                }
            }
        }
        return list;
    }

    /// <summary>
    /// 通过ID获取关卡信息
    /// </summary>
    /// <param name="vID"></param>
    /// <returns></returns>
    public StageInfo GetInfoByID(uint vID)
    {
        StageInfo info = _stageList.Find((stage) => { return vID == stage.ID; });
        if (info == null)
        {
            Debug.LogError("can not get stage info by ID:" + vID);
            return null;
        }
        StageInfo _info = new StageInfo();
        _info.CopyTo(info);
        return _info;
    }

    /// <summary>
    /// 取得活动副本数据
    /// </summary>
    /// <returns></returns>
    public List<StageInfo> GetActivityStagesByType(uint ChapterID)
    {
        List<StageInfo> list = new List<StageInfo>();
        for (int i = 0; i < _stageList.Count; i++)
        {
            StageInfo _info = _stageList[i];
            if (_info.PlayType == (int)EFightType.eftMain && _info.ChapterID == ChapterID)
            {
                StageInfo info = new StageInfo();
                info.CopyTo(_info);
                list.Add(info);
            }
        }
        if (list.Count == 0)
        {
            Debug.LogError("can not find NormalActivity data ");
        }
        return list;
    }

    private void ParseConfig(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                StageInfo data = new StageInfo();
                //Debug.LogWarning("--------------------------------------------------------");
                foreach (XmlElement xe in Element)
                {
                    //Debug.LogWarning(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
                    if (xe.Name == "ID")
                    {
                        data.ID = uint.Parse(xe.InnerText);
                        //Debug.LogWarning(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
                    }
                    else if (xe.Name == "Name")
                    {
                        data.Name = xe.InnerText;
                    }
                    else if (xe.Name == "MapType")
                    {
                        data.PlayType = byte.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "ChapterID")
                    {
                        data.ChapterID = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "SmallGateID")
                    {
                        data.SmallGateID = ushort.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "PrevID")
                    {
                        data.PrevID = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "NextID")
                    {
                        data.NextID = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "EliteFlag")
                    {
                        data.IsElite = byte.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "IsBoss")
                    {
                        int boss = int.Parse(xe.InnerText);
                        if (boss == 2)
                        {
                            data.IsBoss = false;
                        }
                        else if (boss == 1)
                        {
                            data.IsBoss = true;
                        }
                    }
                    else if (xe.Name == "BgTexture")
                    {
                        data.BgTexture = xe.InnerText;
                    }
                    else if (xe.Name == "BossIcon")
                    {
                        data.BossIcon = xe.InnerText;
                    }
                    else if (xe.Name == "IconPos")
                    {
                        string[] arr = xe.InnerText.Split(':');
                        int posx = 0;
                        int posy = 0;
                        if (arr.Length >= 2)
                        {
                            if (!int.TryParse(arr[0], out posx))
                            {
                                Debug.LogError("error pos x");
                            }
                            if (!int.TryParse(arr[1], out posy))
                            {
                                Debug.LogError("error pos y");
                            }
                        }
                        data.IconPos = new Vector2(posx, posy);
                    }
                    else if (xe.Name == "ChallengeTimes")
                    {
                        data.ChallengeCount = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "TimeLimit")
                    {
                        data.LimitTime = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "ConsumePower")
                    {
                        data.Physical = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "UnlockLevel")
                    {
                        data.UnlockLV = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "PreGateID")
                    {
                        data.PreStage = new List<PreStageInfo>();
                        string[] tmpArray = xe.InnerText.Split(';');
                        for (int i = 0; i < tmpArray.Length; i++)
                        {
                            if (string.IsNullOrEmpty(tmpArray[i]))
                                continue;
                            string[] _arr = tmpArray[i].Split(':');
                            if (_arr.Length < 2)
                                continue;
                            uint tmpStageID = 0;
                            byte tmpStarNum = 0;
                            PreStageInfo info = new PreStageInfo();
                            if (!uint.TryParse(_arr[0], out tmpStageID) || !byte.TryParse(_arr[1], out tmpStarNum))
                                Debug.LogError(string.Format("PreGateID: {0}", xe.InnerText));
                            info.StageID = tmpStageID;
                            info.StarNum = tmpStarNum;
                            data.PreStage.Add(info);
                        }
                    }
                    else if (xe.Name == "NextGateID")
                    {
                        data.NextStage = new List<uint>();
                        string[] tmpArray = xe.InnerText.Split(';');
                        for (int i = 0; i < tmpArray.Length; i++)
                        {
                            if (string.IsNullOrEmpty(tmpArray[i]))
                                continue;
                            uint id = 0;
                            if (uint.TryParse(tmpArray[i], out id))
                            {
                                data.NextStage.Add(id);
                            }
                            else
                            {
                                Debug.LogError("Error NextStage Data:" + xe.InnerText);
                            }
                        }
                        //data.NextStage = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "InitMagic")
                    {
                        data.InitMagic = ushort.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "InitEnergy")
                    {
                        data.InitEnergy = ushort.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "DropBagID")
                    {
                        data.DropID = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "GateFlag")
                    {
                        if (data.MarkInfo == null)
                            data.MarkInfo = new List<int>();
                        string temp = xe.InnerText;
                        string[] tmpArray = Regex.Split(temp, ";");
                        if ((tmpArray != null) && (tmpArray.Length > 0))
                        {
                            for (int i = 0; i < tmpArray.Length; i++)
                            {
                                if (string.IsNullOrEmpty(tmpArray[i]))
                                    continue;
                                int tmpMarkInfo = 0;
                                if (int.TryParse(tmpArray[i], out tmpMarkInfo))
                                    data.MarkInfo.Add(tmpMarkInfo);
                                else
                                    Debug.LogError(string.Format("GateFlag: {0}", xe.InnerText));
                            }
                        }
                    }
                    else if (xe.Name == "FightMode")
                    {
                        data.FireType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "OwnCastleBlood")
                    {
                        data.OwnBaseHP = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "EnemyCastleBlood")
                    {
                        data.EnemyBaseHP = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "FlagCoordinate")
                    {
                        data.MarkPos = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "ProtectTargetID")
                    {
                        data.EscortTarget = xe.InnerText;
                    }
                    else if (xe.Name == "TargetSpeed")
                    {
                        data.EscortTargetSpeed = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "ProtectTargetBlood")
                    {
                        data.EscortTargetHP = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "EnemyLimit")
                    {
                        data.MaxEnemyCount = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "NormTime")
                    {
                        data.NormTime = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "ThreeStar")
                    {
                        data.Star3 = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "TwoStar")
                    {
                        data.Star2 = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "OneStar")
                    {
                        data.Star1 = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "GateIntrduce")
                    {
                        data.Describe = xe.InnerText;
                    }
                    else if (xe.Name == "EnemyQueue")
                    {
                        if (data.EnemySquad == null)
                            data.EnemySquad = new List<SingleEnemyInfo>();
                        if (xe.InnerText == "0")
                            continue;
                        string[] tmpArray = xe.InnerText.Split(';');
                        if (tmpArray.Length > 0)
                        {
                            for (int i = 0; i < tmpArray.Length; i++)
                            {
                                if (string.IsNullOrEmpty(tmpArray[i]))
                                    continue;
                                string[] tmpSingleInfo = tmpArray[i].Split(':');
                                if (tmpSingleInfo.Length < 2)
                                    continue;
                                uint tmpMonsterID = 0;
                                byte tmpIsBoss = 0;
                                SingleEnemyInfo tmpInfo = new SingleEnemyInfo();
                                if (!uint.TryParse(tmpSingleInfo[0], out tmpMonsterID) || !byte.TryParse(tmpSingleInfo[1], out tmpIsBoss))
                                {
                                    Debug.LogError(string.Format("EnemyQueue: {0}", xe.InnerText));
                                }
                                tmpInfo.ReSet(tmpMonsterID, tmpIsBoss);
                                data.EnemySquad.Add(tmpInfo);
                            }
                        }
                    }
                    else if (xe.Name == "BackGround")
                    {
                        data.BackGroundID = xe.InnerText;
                    }
                    else if (xe.Name == "CastleType")
                    {
                        byte.TryParse(xe.InnerText, out data.CastleType);
                    }
                    else if (xe.Name == "Warning")
                    {
                        if (!int.TryParse(xe.InnerText, out data.BossHintTime))
                            data.BossHintTime = 0;
                    }
                    else if (xe.Name == "GateSequence")
                    {
                        if (xe.InnerText.Equals("0"))
                        {
                            data.GateSequence = string.Empty;
                        }
                        else
                        {
                            data.GateSequence = xe.InnerText;
                        }
                    }
                    else if (xe.Name == "BossDesc")
                    {
                        data.BossDesc = xe.InnerText;
                    }
                    else if (xe.Name == "AutoStatus")
                    {
                        if (!int.TryParse(xe.InnerText, out data.AutoStatus))
                            data.AutoStatus = 0;
                        data.EnemyBarracksPic = "image_castle_3";
                    }
                    else if (xe.Name == "EnemyBarracksPic")
                    {
                        data.EnemyBarracksPic = xe.InnerText;
                    }
                }
                _stageList.Add(data);
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load Stage XML Error:" + ex.Message + ex.StackTrace);
        }
    }
}