//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: Rank.proto
// Note: requires additional types generated from: Base.proto
namespace fogs.proto.msg
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RankInfoReq")]
  public partial class RankInfoReq : global::ProtoBuf.IExtensible
  {
    public RankInfoReq() {}
    
    private uint _rank_type = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"rank_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint rank_type
    {
      get { return _rank_type; }
      set { _rank_type = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RankInfoResp")]
  public partial class RankInfoResp : global::ProtoBuf.IExtensible
  {
    public RankInfoResp() {}
    
    private uint _rank_type = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"rank_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint rank_type
    {
      get { return _rank_type; }
      set { _rank_type = value; }
    }
    private uint _result = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint result
    {
      get { return _result; }
      set { _result = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.RankInfo> _rank_info = new global::System.Collections.Generic.List<fogs.proto.msg.RankInfo>();
    [global::ProtoBuf.ProtoMember(3, Name=@"rank_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.RankInfo> rank_info
    {
      get { return _rank_info; }
    }
  
    private readonly global::System.Collections.Generic.List<string> _update_time = new global::System.Collections.Generic.List<string>();
    [global::ProtoBuf.ProtoMember(4, Name=@"update_time", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<string> update_time
    {
      get { return _update_time; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RankPalyerInfo")]
  public partial class RankPalyerInfo : global::ProtoBuf.IExtensible
  {
    public RankPalyerInfo() {}
    
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.ArenaSoldier> _soldier_info = new global::System.Collections.Generic.List<fogs.proto.msg.ArenaSoldier>();
    [global::ProtoBuf.ProtoMember(1, Name=@"soldier_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.ArenaSoldier> soldier_info
    {
      get { return _soldier_info; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RankPlayerInfoReq")]
  public partial class RankPlayerInfoReq : global::ProtoBuf.IExtensible
  {
    public RankPlayerInfoReq() {}
    
    private uint _rank_type = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"rank_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint rank_type
    {
      get { return _rank_type; }
      set { _rank_type = value; }
    }
    private uint _charid = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"charid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint charid
    {
      get { return _charid; }
      set { _charid = value; }
    }
    private uint _area_id = default(uint);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"area_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint area_id
    {
      get { return _area_id; }
      set { _area_id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RankPlayerInfoResp")]
  public partial class RankPlayerInfoResp : global::ProtoBuf.IExtensible
  {
    public RankPlayerInfoResp() {}
    
    private uint _rank_type = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"rank_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint rank_type
    {
      get { return _rank_type; }
      set { _rank_type = value; }
    }
    private uint _result = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint result
    {
      get { return _result; }
      set { _result = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.ArenaSoldier> _soldiers = new global::System.Collections.Generic.List<fogs.proto.msg.ArenaSoldier>();
    [global::ProtoBuf.ProtoMember(3, Name=@"soldiers", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.ArenaSoldier> soldiers
    {
      get { return _soldiers; }
    }
  
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.UnionMember> _members = new global::System.Collections.Generic.List<fogs.proto.msg.UnionMember>();
    [global::ProtoBuf.ProtoMember(4, Name=@"members", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.UnionMember> members
    {
      get { return _members; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"RankType")]
    public enum RankType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"LEVEL_RANK", Value=1)]
      LEVEL_RANK = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"COMBAT_RANK", Value=2)]
      COMBAT_RANK = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ARENA_RANK", Value=3)]
      ARENA_RANK = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENDLESS_A", Value=4)]
      ENDLESS_A = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENDLESS_B", Value=5)]
      ENDLESS_B = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENDLESS_C", Value=6)]
      ENDLESS_C = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"RECHARGE_RANK", Value=7)]
      RECHARGE_RANK = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CAMPAIGN_UNION", Value=8)]
      CAMPAIGN_UNION = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CAMPAIGN_PLAYER", Value=9)]
      CAMPAIGN_PLAYER = 9,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CAMPAIGN_ALL_PLAYER", Value=10)]
      CAMPAIGN_ALL_PLAYER = 10,
            
      [global::ProtoBuf.ProtoEnum(Name=@"POLE_RANK", Value=11)]
      POLE_RANK = 11,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UNIONOVERLORD", Value=12)]
      UNIONOVERLORD = 12,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EXPLORE_RANK", Value=13)]
      EXPLORE_RANK = 13,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GRID_RANK", Value=14)]
      GRID_RANK = 14,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UnionPveDgnRank", Value=15)]
      UnionPveDgnRank = 15,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CROSSSERVERWAR_UNION", Value=16)]
      CROSSSERVERWAR_UNION = 16,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CROSSSERVERWAR_PERSONAL", Value=17)]
      CROSSSERVERWAR_PERSONAL = 17
    }
  
}