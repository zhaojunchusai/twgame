//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: Dungeon.proto
// Note: requires additional types generated from: Base.proto
namespace fogs.proto.msg
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"DungeonInfoReq")]
  public partial class DungeonInfoReq : global::ProtoBuf.IExtensible
  {
    public DungeonInfoReq() {}
    
    private fogs.proto.msg.DungeonType _type;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public fogs.proto.msg.DungeonType type
    {
      get { return _type; }
      set { _type = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"DungeonInfoResp")]
  public partial class DungeonInfoResp : global::ProtoBuf.IExtensible
  {
    public DungeonInfoResp() {}
    
    private fogs.proto.msg.DungeonType _type;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public fogs.proto.msg.DungeonType type
    {
      get { return _type; }
      set { _type = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.PassDungeon> _major_info = new global::System.Collections.Generic.List<fogs.proto.msg.PassDungeon>();
    [global::ProtoBuf.ProtoMember(2, Name=@"major_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.PassDungeon> major_info
    {
      get { return _major_info; }
    }
  
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.ActivityDungeonInfo> _activity_info = new global::System.Collections.Generic.List<fogs.proto.msg.ActivityDungeonInfo>();
    [global::ProtoBuf.ProtoMember(3, Name=@"activity_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.ActivityDungeonInfo> activity_info
    {
      get { return _activity_info; }
    }
  
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.EndlessDungeonInfo> _endless_info = new global::System.Collections.Generic.List<fogs.proto.msg.EndlessDungeonInfo>();
    [global::ProtoBuf.ProtoMember(4, Name=@"endless_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.EndlessDungeonInfo> endless_info
    {
      get { return _endless_info; }
    }
  
    private fogs.proto.msg.TodayPlayDungeons _today_elite_dgn = null;
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"today_elite_dgn", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.TodayPlayDungeons today_elite_dgn
    {
      get { return _today_elite_dgn; }
      set { _today_elite_dgn = value; }
    }
    private readonly global::System.Collections.Generic.List<ulong> _load_soldiers = new global::System.Collections.Generic.List<ulong>();
    [global::ProtoBuf.ProtoMember(6, Name=@"load_soldiers", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<ulong> load_soldiers
    {
      get { return _load_soldiers; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"DungeonStartReq")]
  public partial class DungeonStartReq : global::ProtoBuf.IExtensible
  {
    public DungeonStartReq() {}
    
    private uint _dgn_id = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"dgn_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint dgn_id
    {
      get { return _dgn_id; }
      set { _dgn_id = value; }
    }
    private readonly global::System.Collections.Generic.List<ulong> _soldiers = new global::System.Collections.Generic.List<ulong>();
    [global::ProtoBuf.ProtoMember(3, Name=@"soldiers", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<ulong> soldiers
    {
      get { return _soldiers; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"DungeonStartResp")]
  public partial class DungeonStartResp : global::ProtoBuf.IExtensible
  {
    public DungeonStartResp() {}
    
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private uint _dgn_id = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"dgn_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint dgn_id
    {
      get { return _dgn_id; }
      set { _dgn_id = value; }
    }
    private int _ph_power = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"ph_power", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int ph_power
    {
      get { return _ph_power; }
      set { _ph_power = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MopupDungeonReq")]
  public partial class MopupDungeonReq : global::ProtoBuf.IExtensible
  {
    public MopupDungeonReq() {}
    
    private uint _dgn_id = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"dgn_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint dgn_id
    {
      get { return _dgn_id; }
      set { _dgn_id = value; }
    }
    private int _times = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"times", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int times
    {
      get { return _times; }
      set { _times = value; }
    }
    private fogs.proto.msg.DungeonType _type;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public fogs.proto.msg.DungeonType type
    {
      get { return _type; }
      set { _type = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MopupDungeonResp")]
  public partial class MopupDungeonResp : global::ProtoBuf.IExtensible
  {
    public MopupDungeonResp() {}
    
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.DropList> _drop_items = new global::System.Collections.Generic.List<fogs.proto.msg.DropList>();
    [global::ProtoBuf.ProtoMember(2, Name=@"drop_items", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.DropList> drop_items
    {
      get { return _drop_items; }
    }
  
    private fogs.proto.msg.AddExp _exp = null;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"exp", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.AddExp exp
    {
      get { return _exp; }
      set { _exp = value; }
    }
    private int _ph_power = default(int);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"ph_power", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int ph_power
    {
      get { return _ph_power; }
      set { _ph_power = value; }
    }
    private fogs.proto.msg.ItemInfo _mopup_item = null;
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"mopup_item", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.ItemInfo mopup_item
    {
      get { return _mopup_item; }
      set { _mopup_item = value; }
    }
    private uint _dgn_id = default(uint);
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"dgn_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint dgn_id
    {
      get { return _dgn_id; }
      set { _dgn_id = value; }
    }
    private fogs.proto.msg.TodayPlayDungeons _today_elite_dgn = null;
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"today_elite_dgn", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.TodayPlayDungeons today_elite_dgn
    {
      get { return _today_elite_dgn; }
      set { _today_elite_dgn = value; }
    }
    private fogs.proto.msg.DungeonType _type;
    [global::ProtoBuf.ProtoMember(8, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public fogs.proto.msg.DungeonType type
    {
      get { return _type; }
      set { _type = value; }
    }
    private fogs.proto.msg.ActivityDungeonInfo _activity_info = null;
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"activity_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.ActivityDungeonInfo activity_info
    {
      get { return _activity_info; }
      set { _activity_info = value; }
    }
    private fogs.proto.msg.EndlessDungeonInfo _endless_info = null;
    [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name=@"endless_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.EndlessDungeonInfo endless_info
    {
      get { return _endless_info; }
      set { _endless_info = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"DungeonRewardReq")]
  public partial class DungeonRewardReq : global::ProtoBuf.IExtensible
  {
    public DungeonRewardReq() {}
    
    private int _fight_result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"fight_result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int fight_result
    {
      get { return _fight_result; }
      set { _fight_result = value; }
    }
    private uint _dgn_id = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"dgn_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint dgn_id
    {
      get { return _dgn_id; }
      set { _dgn_id = value; }
    }
    private int _reward_arg = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"reward_arg", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int reward_arg
    {
      get { return _reward_arg; }
      set { _reward_arg = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"DungeonRewardResp")]
  public partial class DungeonRewardResp : global::ProtoBuf.IExtensible
  {
    public DungeonRewardResp() {}
    
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private int _fight_result = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"fight_result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int fight_result
    {
      get { return _fight_result; }
      set { _fight_result = value; }
    }
    private uint _dgn_id = default(uint);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"dgn_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint dgn_id
    {
      get { return _dgn_id; }
      set { _dgn_id = value; }
    }
    private uint _star_level = default(uint);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"star_level", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint star_level
    {
      get { return _star_level; }
      set { _star_level = value; }
    }
    private fogs.proto.msg.DropList _drop_items = null;
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"drop_items", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.DropList drop_items
    {
      get { return _drop_items; }
      set { _drop_items = value; }
    }
    private fogs.proto.msg.AddExp _exp = null;
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"exp", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.AddExp exp
    {
      get { return _exp; }
      set { _exp = value; }
    }
    private int _ph_power = default(int);
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"ph_power", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int ph_power
    {
      get { return _ph_power; }
      set { _ph_power = value; }
    }
    private fogs.proto.msg.TodayPlayDungeons _today_elite_dgn = null;
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"today_elite_dgn", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.TodayPlayDungeons today_elite_dgn
    {
      get { return _today_elite_dgn; }
      set { _today_elite_dgn = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EndlessDungeonRewardReq")]
  public partial class EndlessDungeonRewardReq : global::ProtoBuf.IExtensible
  {
    public EndlessDungeonRewardReq() {}
    
    private int _fight_result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"fight_result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int fight_result
    {
      get { return _fight_result; }
      set { _fight_result = value; }
    }
    private uint _dgn_id = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"dgn_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint dgn_id
    {
      get { return _dgn_id; }
      set { _dgn_id = value; }
    }
    private int _enemy_soldier_num = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"enemy_soldier_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int enemy_soldier_num
    {
      get { return _enemy_soldier_num; }
      set { _enemy_soldier_num = value; }
    }
    private int _enemy_boss_num = default(int);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"enemy_boss_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int enemy_boss_num
    {
      get { return _enemy_boss_num; }
      set { _enemy_boss_num = value; }
    }
    private int _enemy_castle_num = default(int);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"enemy_castle_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int enemy_castle_num
    {
      get { return _enemy_castle_num; }
      set { _enemy_castle_num = value; }
    }
    private int _my_soldier_num = default(int);
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"my_soldier_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int my_soldier_num
    {
      get { return _my_soldier_num; }
      set { _my_soldier_num = value; }
    }
    private int _my_hero_num = default(int);
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"my_hero_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int my_hero_num
    {
      get { return _my_hero_num; }
      set { _my_hero_num = value; }
    }
    private int _my_castle_num = default(int);
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"my_castle_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int my_castle_num
    {
      get { return _my_castle_num; }
      set { _my_castle_num = value; }
    }
    private int _use_time = default(int);
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"use_time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int use_time
    {
      get { return _use_time; }
      set { _use_time = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EndlessDungeonRewardResp")]
  public partial class EndlessDungeonRewardResp : global::ProtoBuf.IExtensible
  {
    public EndlessDungeonRewardResp() {}
    
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private int _fight_result = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"fight_result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int fight_result
    {
      get { return _fight_result; }
      set { _fight_result = value; }
    }
    private int _game_over = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"game_over", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int game_over
    {
      get { return _game_over; }
      set { _game_over = value; }
    }
    private int _cur_grade = default(int);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"cur_grade", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int cur_grade
    {
      get { return _cur_grade; }
      set { _cur_grade = value; }
    }
    private int _cur_victory = default(int);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"cur_victory", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int cur_victory
    {
      get { return _cur_victory; }
      set { _cur_victory = value; }
    }
    private fogs.proto.msg.EndlessDungeonInfo _info = null;
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.EndlessDungeonInfo info
    {
      get { return _info; }
      set { _info = value; }
    }
    private fogs.proto.msg.DropList _drop_items = null;
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"drop_items", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.DropList drop_items
    {
      get { return _drop_items; }
      set { _drop_items = value; }
    }
    private fogs.proto.msg.AddExp _exp = null;
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"exp", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.AddExp exp
    {
      get { return _exp; }
      set { _exp = value; }
    }
    private int _ph_power = default(int);
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"ph_power", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int ph_power
    {
      get { return _ph_power; }
      set { _ph_power = value; }
    }
    private int _cur_use_time = default(int);
    [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name=@"cur_use_time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int cur_use_time
    {
      get { return _cur_use_time; }
      set { _cur_use_time = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"DungeonStarRewardReq")]
  public partial class DungeonStarRewardReq : global::ProtoBuf.IExtensible
  {
    public DungeonStarRewardReq() {}
    
    private uint _chapter_id = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"chapter_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint chapter_id
    {
      get { return _chapter_id; }
      set { _chapter_id = value; }
    }
    private uint _fight_mode = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"fight_mode", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint fight_mode
    {
      get { return _fight_mode; }
      set { _fight_mode = value; }
    }
    private int _star = default(int);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"star", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int star
    {
      get { return _star; }
      set { _star = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"DungeonStarRewardResp")]
  public partial class DungeonStarRewardResp : global::ProtoBuf.IExtensible
  {
    public DungeonStarRewardResp() {}
    
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private uint _chapter_id = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"chapter_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint chapter_id
    {
      get { return _chapter_id; }
      set { _chapter_id = value; }
    }
    private uint _fight_mode = default(uint);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"fight_mode", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint fight_mode
    {
      get { return _fight_mode; }
      set { _fight_mode = value; }
    }
    private fogs.proto.msg.DropList _drop_items = null;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"drop_items", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.DropList drop_items
    {
      get { return _drop_items; }
      set { _drop_items = value; }
    }
    private int _star = default(int);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"star", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int star
    {
      get { return _star; }
      set { _star = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BuyDungeonTimesReq")]
  public partial class BuyDungeonTimesReq : global::ProtoBuf.IExtensible
  {
    public BuyDungeonTimesReq() {}
    
    private uint _dgn_id = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"dgn_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint dgn_id
    {
      get { return _dgn_id; }
      set { _dgn_id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BuyDungeonTimesResp")]
  public partial class BuyDungeonTimesResp : global::ProtoBuf.IExtensible
  {
    public BuyDungeonTimesResp() {}
    
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private int _diamond = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"diamond", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int diamond
    {
      get { return _diamond; }
      set { _diamond = value; }
    }
    private fogs.proto.msg.TodayPlayDungeons _today_elite_dgn = null;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"today_elite_dgn", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.TodayPlayDungeons today_elite_dgn
    {
      get { return _today_elite_dgn; }
      set { _today_elite_dgn = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BuyOtherDungeonTimesReq")]
  public partial class BuyOtherDungeonTimesReq : global::ProtoBuf.IExtensible
  {
    public BuyOtherDungeonTimesReq() {}
    
    private fogs.proto.msg.OtherDungeonType _dgn_type;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"dgn_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public fogs.proto.msg.OtherDungeonType dgn_type
    {
      get { return _dgn_type; }
      set { _dgn_type = value; }
    }
    private uint _chapter_id = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"chapter_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint chapter_id
    {
      get { return _chapter_id; }
      set { _chapter_id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BuyOtherDungeonTimesResp")]
  public partial class BuyOtherDungeonTimesResp : global::ProtoBuf.IExtensible
  {
    public BuyOtherDungeonTimesResp() {}
    
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private int _diamond = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"diamond", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int diamond
    {
      get { return _diamond; }
      set { _diamond = value; }
    }
    private int _buy_times = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"buy_times", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int buy_times
    {
      get { return _buy_times; }
      set { _buy_times = value; }
    }
    private int _today_buy_times = default(int);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"today_buy_times", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int today_buy_times
    {
      get { return _today_buy_times; }
      set { _today_buy_times = value; }
    }
    private uint _chapter_id = default(uint);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"chapter_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint chapter_id
    {
      get { return _chapter_id; }
      set { _chapter_id = value; }
    }
    private uint _today_times = default(uint);
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"today_times", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint today_times
    {
      get { return _today_times; }
      set { _today_times = value; }
    }
    private fogs.proto.msg.OtherDungeonType _dgn_type = fogs.proto.msg.OtherDungeonType.ACTIVITY_DGN;
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"dgn_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(fogs.proto.msg.OtherDungeonType.ACTIVITY_DGN)]
    public fogs.proto.msg.OtherDungeonType dgn_type
    {
      get { return _dgn_type; }
      set { _dgn_type = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"OtherDungeonType")]
    public enum OtherDungeonType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ACTIVITY_DGN", Value=1)]
      ACTIVITY_DGN = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENDLESS_DGN", Value=2)]
      ENDLESS_DGN = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EXOTICADVANTURE", Value=3)]
      EXOTICADVANTURE = 3
    }
  
}