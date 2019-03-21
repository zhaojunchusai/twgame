using System;
using System.Collections.Generic;
using UnityEngine;

public class SDKBaseUserData
{
    /// <summary>
    /// 给游戏服务器验签使用，验签方式
    /// </summary>
    public string sign;
    /// <summary>
    /// 账号ID  用户惟一标识
    /// </summary>
    public string userid;
    /// <summary>
    /// 玩家昵称
    /// </summary>
    public string nickname;
    /// <summary>
    /// 用户性别
    /// </summary>
    public string sex;
    /// <summary>
    /// 用户本次成功登录时间，UNIXTIME时间戳
    /// </summary>
    public string logintime;
    /// <summary>
    /// 用户来源渠道名，枚举值：Facebook, Google
    /// </summary>
    public string logintype;
}

public class IdplayUserData : SDKBaseUserData
{
    /// <summary>
    /// 用户来源游戏别名，如：fengyuntx、7725
    /// </summary>
    public string gamename;
    /// <summary>
    /// OPENID用户标识，有约定的游戏才可以使用
    /// </summary>
    public string openuid;
	/// <summary>
	/// ios
	/// 是否绑定  0-不需要绑定(idplay login) 1-已经绑定(facebook or quick login but has binded) 2-可以绑定
	/// </summary>
	public int isbind = -1;
	public override string ToString ()
	{
		return string.Format ("[IdplayUserData] userid={0} nickname={1} sex={2} logintime={3} logintype={4} gamename={5}",
		                      userid,nickname,sex,logintime,logintype,gamename);
	}
}
