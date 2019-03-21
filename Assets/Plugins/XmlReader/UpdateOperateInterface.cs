using UnityEngine;
using System.Collections;
using System;

//更新//
namespace UpdateOperate
{
	//下载操作接口//
	public interface IOperateDownLoad
	{
		bool LoadFile (string vURL, string vPath);
	}
	
	//XML操作接口//
	public interface IOperateXML
	{
		void LoadXML (string vPath, ref object vData, Type vObjType);
		void SaveXML (string vPath, ref object vData);
		object DeserializeObject (string pXmlizedString, Type classType);
	}
	
	//MD5操作接口//
	public interface IOperateMD5
	{
		bool CheckMD5IsSame (string vMD5Base, string vPath);
		string GetFileMD5 (string vPath);
		string GetFileMD5ForValue (string vValue);
	}
	
}