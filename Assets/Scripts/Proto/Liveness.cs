//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: Liveness.proto
// Note: requires additional types generated from: Base.proto
namespace fogs.proto.msg
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LivenessNotify")]
  public partial class LivenessNotify : global::ProtoBuf.IExtensible
  {
    public LivenessNotify() {}
    
    private int _num = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int num
    {
      get { return _num; }
      set { _num = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UpdateLivenessDataReq")]
  public partial class UpdateLivenessDataReq : global::ProtoBuf.IExtensible
  {
    public UpdateLivenessDataReq() {}
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UpdateLivenessDataResp")]
  public partial class UpdateLivenessDataResp : global::ProtoBuf.IExtensible
  {
    public UpdateLivenessDataResp() {}
    
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private fogs.proto.msg.LivenessInfo _liveness_info = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"liveness_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.LivenessInfo liveness_info
    {
      get { return _liveness_info; }
      set { _liveness_info = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LivenessRewardReq")]
  public partial class LivenessRewardReq : global::ProtoBuf.IExtensible
  {
    public LivenessRewardReq() {}
    
    private int _id = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int id
    {
      get { return _id; }
      set { _id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LivenessRewardResp")]
  public partial class LivenessRewardResp : global::ProtoBuf.IExtensible
  {
    public LivenessRewardResp() {}
    
    private int _result = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int result
    {
      get { return _result; }
      set { _result = value; }
    }
    private int _id = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int id
    {
      get { return _id; }
      set { _id = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.ItemInfo> _update_item = new global::System.Collections.Generic.List<fogs.proto.msg.ItemInfo>();
    [global::ProtoBuf.ProtoMember(3, Name=@"update_item", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.ItemInfo> update_item
    {
      get { return _update_item; }
    }
  
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.Equip> _update_equip = new global::System.Collections.Generic.List<fogs.proto.msg.Equip>();
    [global::ProtoBuf.ProtoMember(4, Name=@"update_equip", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.Equip> update_equip
    {
      get { return _update_equip; }
    }
  
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.Equip> _update_soldierequip = new global::System.Collections.Generic.List<fogs.proto.msg.Equip>();
    [global::ProtoBuf.ProtoMember(5, Name=@"update_soldierequip", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.Equip> update_soldierequip
    {
      get { return _update_soldierequip; }
    }
  
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.Soldier> _update_sodier = new global::System.Collections.Generic.List<fogs.proto.msg.Soldier>();
    [global::ProtoBuf.ProtoMember(6, Name=@"update_sodier", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.Soldier> update_sodier
    {
      get { return _update_sodier; }
    }
  
    private readonly global::System.Collections.Generic.List<int> _award_list = new global::System.Collections.Generic.List<int>();
    [global::ProtoBuf.ProtoMember(7, Name=@"award_list", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<int> award_list
    {
      get { return _award_list; }
    }
  
    private fogs.proto.msg.AddExp _add_exp = null;
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"add_exp", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.AddExp add_exp
    {
      get { return _add_exp; }
      set { _add_exp = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}