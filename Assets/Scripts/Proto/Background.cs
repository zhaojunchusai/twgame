//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: Background.proto
// Note: requires additional types generated from: Base.proto
namespace fogs.proto.msg
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GMOperateReq")]
  public partial class GMOperateReq : global::ProtoBuf.IExtensible
  {
    public GMOperateReq() {}
    
    private string _content = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"content", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string content
    {
      get { return _content; }
      set { _content = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GMOperateResp")]
  public partial class GMOperateResp : global::ProtoBuf.IExtensible
  {
    public GMOperateResp() {}
    
    private uint _result = default(uint);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint result
    {
      get { return _result; }
      set { _result = value; }
    }
    private fogs.proto.msg.ItemInfo _item = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"item", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.ItemInfo item
    {
      get { return _item; }
      set { _item = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.Equip> _equip = new global::System.Collections.Generic.List<fogs.proto.msg.Equip>();
    [global::ProtoBuf.ProtoMember(3, Name=@"equip", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.Equip> equip
    {
      get { return _equip; }
    }
  
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.Soldier> _soldier = new global::System.Collections.Generic.List<fogs.proto.msg.Soldier>();
    [global::ProtoBuf.ProtoMember(4, Name=@"soldier", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.Soldier> soldier
    {
      get { return _soldier; }
    }
  
    private fogs.proto.msg.AddExp _new_level_info = null;
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"new_level_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.AddExp new_level_info
    {
      get { return _new_level_info; }
      set { _new_level_info = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}