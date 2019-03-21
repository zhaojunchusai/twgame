using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Collections;

public class Ipv6Manager 
{
#if UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern string getIPv6(string mHost, string mPort);  
#endif

	public static string GetIPv6(string mHost, string mPort)
	{
		#if UNITY_IPHONE && !UNITY_EDITOR
		string mIPv6 = getIPv6(mHost, mPort);
		return mIPv6;
		#else
		return mHost + "&&ipv4";
		#endif
	}
	
	public static void GetIPType(String serverIp, String serverPorts, out String newServerIp, out AddressFamily  mIPType)
	{
		mIPType = AddressFamily.InterNetwork;
		newServerIp = serverIp;
		try
		{
			string mIPv6 = GetIPv6(serverIp, serverPorts);
			if (!string.IsNullOrEmpty(mIPv6))
			{
				string[] m_StrTemp = System.Text.RegularExpressions.Regex.Split(mIPv6, "&&");
				if (m_StrTemp != null && m_StrTemp.Length >= 2)
				{
					string IPType = m_StrTemp[1];
					if (IPType == "ipv6")
					{
						newServerIp = m_StrTemp[0];
						mIPType = AddressFamily.InterNetworkV6;
					}
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogError("GetIPv6 error:" + e);
		}
		
	}
	
//	public void SocketClient(String serverIp, String serverPorts)
//	{
//		String newServerIp = "";
//
//		AddressFamily newAddressFamily = AddressFamily.InterNetwork;
//
//		getIPType(serverIp, serverPorts, out newServerIp, out newAddressFamily);
//
//		if (!string.IsNullOrEmpty(newServerIp)){ serverIp = newServerIp;}      
//
//		socketClient = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);
//
//		ClientLog.Instance.Log("Socket AddressFamily :" + newAddressFamily.ToString() + "ServerIp:" + serverIp);
//	}
}
