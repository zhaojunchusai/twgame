//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: PolePos.proto
// Note: requires additional types generated from: Base.proto
namespace fogs.proto.msg
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EnterPoleLobbyReq")]
  public partial class EnterPoleLobbyReq : global::ProtoBuf.IExtensible
  {
    public EnterPoleLobbyReq() {}
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EnterPoleLobbyResp")]
  public partial class EnterPoleLobbyResp : global::ProtoBuf.IExtensible
  {
    public EnterPoleLobbyResp() {}
    
    private int _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private fogs.proto.msg.PoleRecInf _pole_recinf;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"pole_recinf", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public fogs.proto.msg.PoleRecInf pole_recinf
    {
      get { return _pole_recinf; }
      set { _pole_recinf = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.SoldierList> _defence_soldiers = new global::System.Collections.Generic.List<fogs.proto.msg.SoldierList>();
    [global::ProtoBuf.ProtoMember(3, Name=@"defence_soldiers", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.SoldierList> defence_soldiers
    {
      get { return _defence_soldiers; }
    }
  
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.EquipList> _defence_equips = new global::System.Collections.Generic.List<fogs.proto.msg.EquipList>();
    [global::ProtoBuf.ProtoMember(4, Name=@"defence_equips", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.EquipList> defence_equips
    {
      get { return _defence_equips; }
    }
  
    private int _buy_times = default(int);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"buy_times", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int buy_times
    {
      get { return _buy_times; }
      set { _buy_times = value; }
    }
    private int _combat_power = default(int);
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"combat_power", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int combat_power
    {
      get { return _combat_power; }
      set { _combat_power = value; }
    }
    private uint _start_time = default(uint);
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"start_time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint start_time
    {
      get { return _start_time; }
      set { _start_time = value; }
    }
    private uint _end_time = default(uint);
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"end_time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint end_time
    {
      get { return _end_time; }
      set { _end_time = value; }
    }
    private int _status = default(int);
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"status", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int status
    {
      get { return _status; }
      set { _status = value; }
    }
    private int _rank_up = default(int);
    [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name=@"rank_up", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int rank_up
    {
      get { return _rank_up; }
      set { _rank_up = value; }
    }
    private int _pettypeid = default(int);
    [global::ProtoBuf.ProtoMember(11, IsRequired = false, Name=@"pettypeid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int pettypeid
    {
      get { return _pettypeid; }
      set { _pettypeid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SavePoleArrayReq")]
  public partial class SavePoleArrayReq : global::ProtoBuf.IExtensible
  {
    public SavePoleArrayReq() {}
    
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.SoldierList> _defence_soldiers = new global::System.Collections.Generic.List<fogs.proto.msg.SoldierList>();
    [global::ProtoBuf.ProtoMember(1, Name=@"defence_soldiers", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.SoldierList> defence_soldiers
    {
      get { return _defence_soldiers; }
    }
  
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.EquipList> _defence_equips = new global::System.Collections.Generic.List<fogs.proto.msg.EquipList>();
    [global::ProtoBuf.ProtoMember(2, Name=@"defence_equips", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.EquipList> defence_equips
    {
      get { return _defence_equips; }
    }
  
    private int _combat_power = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"combat_power", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int combat_power
    {
      get { return _combat_power; }
      set { _combat_power = value; }
    }
    private int _pettypeid = default(int);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"pettypeid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int pettypeid
    {
      get { return _pettypeid; }
      set { _pettypeid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SavePoleArrayResp")]
  public partial class SavePoleArrayResp : global::ProtoBuf.IExtensible
  {
    public SavePoleArrayResp() {}
    
    private int _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private int _combat_power = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"combat_power", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int combat_power
    {
      get { return _combat_power; }
      set { _combat_power = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MatchPoleReq")]
  public partial class MatchPoleReq : global::ProtoBuf.IExtensible
  {
    public MatchPoleReq() {}
    
    private int _type = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int type
    {
      get { return _type; }
      set { _type = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AccidScore")]
  public partial class AccidScore : global::ProtoBuf.IExtensible
  {
    public AccidScore() {}
    
    private uint _accid = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"accid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint accid
    {
      get { return _accid; }
      set { _accid = value; }
    }
    private int _score = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"score", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int score
    {
      get { return _score; }
      set { _score = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MatchPoleResp")]
  public partial class MatchPoleResp : global::ProtoBuf.IExtensible
  {
    public MatchPoleResp() {}
    
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private int _type = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int type
    {
      get { return _type; }
      set { _type = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.ArenaPlayer> _opponent = new global::System.Collections.Generic.List<fogs.proto.msg.ArenaPlayer>();
    [global::ProtoBuf.ProtoMember(3, Name=@"opponent", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.ArenaPlayer> opponent
    {
      get { return _opponent; }
    }
  
    private uint _next_tick = default(uint);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"next_tick", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint next_tick
    {
      get { return _next_tick; }
      set { _next_tick = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.AccidScore> _can_get = new global::System.Collections.Generic.List<fogs.proto.msg.AccidScore>();
    [global::ProtoBuf.ProtoMember(5, Name=@"can_get", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.AccidScore> can_get
    {
      get { return _can_get; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"StartPoleReq")]
  public partial class StartPoleReq : global::ProtoBuf.IExtensible
  {
    public StartPoleReq() {}
    
    private uint _accid = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"accid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint accid
    {
      get { return _accid; }
      set { _accid = value; }
    }
    private int _fight_type = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"fight_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int fight_type
    {
      get { return _fight_type; }
      set { _fight_type = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.SoldierList> _defence_soldiers = new global::System.Collections.Generic.List<fogs.proto.msg.SoldierList>();
    [global::ProtoBuf.ProtoMember(3, Name=@"defence_soldiers", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.SoldierList> defence_soldiers
    {
      get { return _defence_soldiers; }
    }
  
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.EquipList> _defence_equips = new global::System.Collections.Generic.List<fogs.proto.msg.EquipList>();
    [global::ProtoBuf.ProtoMember(4, Name=@"defence_equips", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.EquipList> defence_equips
    {
      get { return _defence_equips; }
    }
  
    private string _uid = "";
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"uid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string uid
    {
      get { return _uid; }
      set { _uid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"StartPoleResp")]
  public partial class StartPoleResp : global::ProtoBuf.IExtensible
  {
    public StartPoleResp() {}
    
    private int _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private int _fight_type = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"fight_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int fight_type
    {
      get { return _fight_type; }
      set { _fight_type = value; }
    }
    private fogs.proto.msg.Attribute _hero_attr = null;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"hero_attr", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.Attribute hero_attr
    {
      get { return _hero_attr; }
      set { _hero_attr = value; }
    }
    private fogs.proto.msg.Item _consume = null;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"consume", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.Item consume
    {
      get { return _consume; }
      set { _consume = value; }
    }
    private int _total_revenge_times = default(int);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"total_revenge_times", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int total_revenge_times
    {
      get { return _total_revenge_times; }
      set { _total_revenge_times = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ClearPoleMatchCDReq")]
  public partial class ClearPoleMatchCDReq : global::ProtoBuf.IExtensible
  {
    public ClearPoleMatchCDReq() {}
    
    private int _clear_cd_tip = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"clear_cd_tip", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int clear_cd_tip
    {
      get { return _clear_cd_tip; }
      set { _clear_cd_tip = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ClearPoleMatchCDResp")]
  public partial class ClearPoleMatchCDResp : global::ProtoBuf.IExtensible
  {
    public ClearPoleMatchCDResp() {}
    
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private fogs.proto.msg.Item _consume = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"consume", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.Item consume
    {
      get { return _consume; }
      set { _consume = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EndPoleReq")]
  public partial class EndPoleReq : global::ProtoBuf.IExtensible
  {
    public EndPoleReq() {}
    
    private fogs.proto.msg.PoleResult _pole_result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"pole_result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public fogs.proto.msg.PoleResult pole_result
    {
      get { return _pole_result; }
      set { _pole_result = value; }
    }
    private string _uid = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"uid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string uid
    {
      get { return _uid; }
      set { _uid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EndPoleResp")]
  public partial class EndPoleResp : global::ProtoBuf.IExtensible
  {
    public EndPoleResp() {}
    
    private fogs.proto.msg.PoleResult _pole_result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"pole_result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public fogs.proto.msg.PoleResult pole_result
    {
      get { return _pole_result; }
      set { _pole_result = value; }
    }
    private int _result;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private int _score = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"score", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int score
    {
      get { return _score; }
      set { _score = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PoleRewardReq")]
  public partial class PoleRewardReq : global::ProtoBuf.IExtensible
  {
    public PoleRewardReq() {}
    
    private int _key;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"key", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int key
    {
      get { return _key; }
      set { _key = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PoleRewardResp")]
  public partial class PoleRewardResp : global::ProtoBuf.IExtensible
  {
    public PoleRewardResp() {}
    
    private int _key;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"key", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int key
    {
      get { return _key; }
      set { _key = value; }
    }
    private int _result;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private fogs.proto.msg.DropList _drop_list = null;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"drop_list", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.DropList drop_list
    {
      get { return _drop_list; }
      set { _drop_list = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PoleRecordReq")]
  public partial class PoleRecordReq : global::ProtoBuf.IExtensible
  {
    public PoleRecordReq() {}
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PoleRecordResp")]
  public partial class PoleRecordResp : global::ProtoBuf.IExtensible
  {
    public PoleRecordResp() {}
    
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.ArenaRecord> _records = new global::System.Collections.Generic.List<fogs.proto.msg.ArenaRecord>();
    [global::ProtoBuf.ProtoMember(2, Name=@"records", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.ArenaRecord> records
    {
      get { return _records; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PoleRevengeReq")]
  public partial class PoleRevengeReq : global::ProtoBuf.IExtensible
  {
    public PoleRevengeReq() {}
    
    private uint _accid = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"accid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint accid
    {
      get { return _accid; }
      set { _accid = value; }
    }
    private string _uid = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"uid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string uid
    {
      get { return _uid; }
      set { _uid = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PoleRevengeResp")]
  public partial class PoleRevengeResp : global::ProtoBuf.IExtensible
  {
    public PoleRevengeResp() {}
    
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PoleBuyTimesReq")]
  public partial class PoleBuyTimesReq : global::ProtoBuf.IExtensible
  {
    public PoleBuyTimesReq() {}
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PoleBuyTimesResp")]
  public partial class PoleBuyTimesResp : global::ProtoBuf.IExtensible
  {
    public PoleBuyTimesResp() {}
    
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private int _times = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"times", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int times
    {
      get { return _times; }
      set { _times = value; }
    }
    private int _diamond = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"diamond", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int diamond
    {
      get { return _diamond; }
      set { _diamond = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"PoleResult")]
    public enum PoleResult
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ATTACK_WIN", Value=1)]
      ATTACK_WIN = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ATTACK_LOSE", Value=2)]
      ATTACK_LOSE = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"REVENGE_WIN", Value=3)]
      REVENGE_WIN = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"REVENGE_LOSE", Value=4)]
      REVENGE_LOSE = 4
    }
  
}