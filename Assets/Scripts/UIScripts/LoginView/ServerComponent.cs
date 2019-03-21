using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
/* File: ServerComponent.cs
 * Desc: 服务器列表显示
 * Date：2015-5-5 19:27
 * Add by taiwei
 * modify by taiwei 2015-05-18 20:25
 */
public class ServerComponent : BaseComponent
{
    public UILabel Lbl_Name;
    public UILabel Lbl_Status;

    private ServerDetail _serverinfo; //服务器列表信息
    public ServerDetail serverinfo 
    {
        get 
        {
            return _serverinfo;
        }
    }
    public override void MyStart(GameObject root) 
    {
        base.MyStart(root);
        Lbl_Name = mRootObject.transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Lbl_Status = mRootObject.transform.FindChild("Status").gameObject.GetComponent<UILabel>();
        //base.AutoSetGoProperty(this, root);
        Clear();
    }

    public void UpdateInfo(ServerDetail info) 
    {
        _serverinfo = info;
        if (_serverinfo == null) 
        {
            return;
        }
        Lbl_Name.text = _serverinfo.desc;
        switch (_serverinfo.status) 
        {
            case 0:
                Lbl_Status.text = "爆满";
                break;
            case 1:
                Lbl_Status.text = "空闲";
                break;
        }

    }

    public override void Clear()
    {
        base.Clear();
        _serverinfo = null;
        Lbl_Name.text = string.Empty;
        Lbl_Status.text = string.Empty;
    }

    
}

