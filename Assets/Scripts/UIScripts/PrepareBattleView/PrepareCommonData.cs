using System;
using System.Collections.Generic;
using fogs.proto.msg;

public class PrepareCommonData
{
    /// <summary>
    /// 战斗类型
    /// </summary>
    public EFightType FightType;
    /// <summary>
    /// 玩家信息
    /// </summary>
    public ArenaPlayer ArenaPlayer;
    /// <summary>
    /// 是否是阵容调整
    /// </summary>
    public bool IsAdjust;

    public StageInfo StageInfo;

    public UnionPveDgnInfo UnionDgnInfo;

    public int Other;

    public uint AccountID;

    public string UID;

    public fogs.proto.msg.Attribute PlayerAtt;

    public List<SoldierList> soliders;

    public List<EquipList> equips;
}
