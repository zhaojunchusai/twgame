//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: Item.proto
// Note: requires additional types generated from: Base.proto
namespace fogs.proto.msg
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BuyItemReq")]
  public partial class BuyItemReq : global::ProtoBuf.IExtensible
  {
    public BuyItemReq() {}
    
    private uint _id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint id
    {
      get { return _id; }
      set { _id = value; }
    }
    private int _num = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
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
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BuyItemResp")]
  public partial class BuyItemResp : global::ProtoBuf.IExtensible
  {
    public BuyItemResp() {}
    
    private uint _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint result
    {
      get { return _result; }
      set { _result = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.ItemInfo> _update_item = new global::System.Collections.Generic.List<fogs.proto.msg.ItemInfo>();
    [global::ProtoBuf.ProtoMember(2, Name=@"update_item", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.ItemInfo> update_item
    {
      get { return _update_item; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SellItemReq")]
  public partial class SellItemReq : global::ProtoBuf.IExtensible
  {
    public SellItemReq() {}
    
    private uint _id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint id
    {
      get { return _id; }
      set { _id = value; }
    }
    private int _num;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int num
    {
      get { return _num; }
      set { _num = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SellItemResp")]
  public partial class SellItemResp : global::ProtoBuf.IExtensible
  {
    public SellItemResp() {}
    
    private uint _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint result
    {
      get { return _result; }
      set { _result = value; }
    }
    private uint _money_type = default(uint);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"money_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    public uint money_type
    {
      get { return _money_type; }
      set { _money_type = value; }
    }
    private int _money_num = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"money_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int money_num
    {
      get { return _money_num; }
      set { _money_num = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.ItemInfo> _update_item = new global::System.Collections.Generic.List<fogs.proto.msg.ItemInfo>();
    [global::ProtoBuf.ProtoMember(4, Name=@"update_item", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.ItemInfo> update_item
    {
      get { return _update_item; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ID2Num")]
  public partial class ID2Num : global::ProtoBuf.IExtensible
  {
    public ID2Num() {}
    
    private int _id = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int id
    {
      get { return _id; }
      set { _id = value; }
    }
    private int _num = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
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
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UseItemReq")]
  public partial class UseItemReq : global::ProtoBuf.IExtensible
  {
    public UseItemReq() {}
    
    private uint _id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint id
    {
      get { return _id; }
      set { _id = value; }
    }
    private int _num;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int num
    {
      get { return _num; }
      set { _num = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.ID2Num> _id_num = new global::System.Collections.Generic.List<fogs.proto.msg.ID2Num>();
    [global::ProtoBuf.ProtoMember(3, Name=@"id_num", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.ID2Num> id_num
    {
      get { return _id_num; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UseItemResp")]
  public partial class UseItemResp : global::ProtoBuf.IExtensible
  {
    public UseItemResp() {}
    
    private uint _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint result
    {
      get { return _result; }
      set { _result = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.ItemInfo> _update_item = new global::System.Collections.Generic.List<fogs.proto.msg.ItemInfo>();
    [global::ProtoBuf.ProtoMember(3, Name=@"update_item", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.ItemInfo> update_item
    {
      get { return _update_item; }
    }
  
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.UsedItemInfo> _update_useditem = new global::System.Collections.Generic.List<fogs.proto.msg.UsedItemInfo>();
    [global::ProtoBuf.ProtoMember(4, Name=@"update_useditem", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.UsedItemInfo> update_useditem
    {
      get { return _update_useditem; }
    }
  
    private fogs.proto.msg.Attribute _total_attr = null;
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"total_attr", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.Attribute total_attr
    {
      get { return _total_attr; }
      set { _total_attr = value; }
    }
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.Equip> _update_equip_list = new global::System.Collections.Generic.List<fogs.proto.msg.Equip>();
    [global::ProtoBuf.ProtoMember(6, Name=@"update_equip_list", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.Equip> update_equip_list
    {
      get { return _update_equip_list; }
    }
  
    private readonly global::System.Collections.Generic.List<fogs.proto.msg.Soldier> _update_soldier_list = new global::System.Collections.Generic.List<fogs.proto.msg.Soldier>();
    [global::ProtoBuf.ProtoMember(7, Name=@"update_soldier_list", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<fogs.proto.msg.Soldier> update_soldier_list
    {
      get { return _update_soldier_list; }
    }
  
    private fogs.proto.msg.AddExp _new_exp_info = null;
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"new_exp_info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public fogs.proto.msg.AddExp new_exp_info
    {
      get { return _new_exp_info; }
      set { _new_exp_info = value; }
    }
    private int _use_type = default(int);
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"use_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int use_type
    {
      get { return _use_type; }
      set { _use_type = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}