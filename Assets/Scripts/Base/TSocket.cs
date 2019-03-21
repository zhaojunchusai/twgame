using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections;
using Assets.Script.Common;

public delegate void PacketHandle(Packet packetdata);
public delegate void SystemError(byte[] data, string errormsg);

public class MessageHeader
{
    public const int HEADERLENGTH = 16;
    public uint mType;
    public uint mMsgID;
    public uint mPacketLen;
    public uint mAccountID;
    private bool _init = false;

    public void Clear()
    {
        mType = 0;
        mMsgID = 0;
        mPacketLen = 0;
        mAccountID = 0;
        _init = false;

    }

    public void CopyTo(MessageHeader data)
    {
        this.mType = data.mType;
        this.mMsgID = data.mMsgID;
        this.mPacketLen = data.mPacketLen;
        this.mAccountID = data.mAccountID;
        this._init = data._init;
    }

    public byte[] ToByteArray()
    {
        byte[] _head = new byte[HEADERLENGTH];
        byte[] _type = BitConverter.GetBytes(mType);
        byte[] _msgID = BitConverter.GetBytes(mMsgID);
        byte[] _length = BitConverter.GetBytes(mPacketLen);
        byte[] _accountID = BitConverter.GetBytes(mAccountID);
        Array.Copy(_type, 0, _head, 0, 4);
        Array.Copy(_msgID, 0, _head, 4, 4);
        Array.Copy(_length, 0, _head, 8, 4);
        Array.Copy(_accountID, 0, _head, 12, 4);
        return _head;
    }

    public MessageHeader BytesToMsgHeader(byte[] _head)
    {
        mType = GetTypeFormBytes(_head);
        mMsgID = GetMsgIDFromBytes(_head);
        mPacketLen = GetLengthFromBytes(_head);
        mAccountID = GetAccountFromBytes(_head);
        _init = true;
        return this;
    }

    public uint GetTypeFormBytes(byte[] _head)
    {
        mType = BitConverter.ToUInt32(_head, 0);
        return mType;
    }

    public uint GetMsgIDFromBytes(byte[] _head)
    {
        mMsgID = BitConverter.ToUInt32(_head, 4);
        return mMsgID;
    }

    public uint GetLengthFromBytes(byte[] _head)
    {
        mPacketLen = BitConverter.ToUInt32(_head, 8);
        return mPacketLen;
    }

    public uint GetAccountFromBytes(byte[] _head)
    {
        mAccountID = BitConverter.ToUInt32(_head, 12);
        return mAccountID;
    }

    public bool IsInitHeader
    {
        get
        {
            return _init;
        }
    }

    public override string ToString()
    {
        return "MsgHeader...Type = " + mType + ";MsgID = " + mMsgID + ";Length = " + mPacketLen + ";AccountID = " + mAccountID;
    }
}

public class Packet
{
    public MessageHeader header;
    public MemoryStream ms;
    public Packet()
    {
        header = new MessageHeader();
    }

}

public class TSocket
{
    public delegate void ConnectResult(object value);
    public delegate void SocketExceptionHandler(bool isconnected);

    const uint MAXMSGSEQID = 67108863;//序列号最大值//
    const int FMaxRecBuf = 1048576;//单个包最大缓存//
    public PacketHandle mEventProcessor;
    public ConnectResult mConnectResult;
    public SocketExceptionHandler SocketExceptionEvent;

    private byte[] FRecBuf = new byte[FMaxRecBuf];
    private byte[] FRecTemp = new byte[FMaxRecBuf];
    private byte[] FHeadBuf = new byte[MessageHeader.HEADERLENGTH];
    private MessageHeader FMsgHeader = new MessageHeader();
    private int FRecBufSize = 0;
    private int FRecBufIndex = 0;
    private int FPackageLength = 0;
    private int FSocketBufCount = 0;
    private int FSocketTimeOut = 4;
    private Socket FSocket;
    private IPEndPoint FIPEndPoint;
    private string FConnectIP;
    private int FServerPort;
    private string FSocketName = "";
    //private bool isConnected = false;
    private uint FMsgSN = 0;

    public void Initialize()
    {
        FSocket = null;
        FRecBufSize = 0;
        FRecBufIndex = 0;
        FPackageLength = 0;
        FSocketBufCount = 0;
        FMsgSN = 0;
        //isConnected = false;
        mEventProcessor = null;
    }

    public TSocket(string connectIP, int serverPor, string socketName)
    {
        SetIPandPort(connectIP, serverPor, socketName);
        Initialize();
    }

    public void SetIPandPort(string connectIP, int serverPor, string socketName)
    {
        FConnectIP = connectIP;
        FServerPort = serverPor;
        FSocketName = socketName;
    }

    public void Connect()
    {
        try
        {
            if (IsConnect())
                DisConnect();

            IPAddress aAddress = IPAddress.Parse(FConnectIP);
            FIPEndPoint = new IPEndPoint(aAddress, FServerPort);
#if UNITY_IPHONE && !UNITY_EDITOR
			String newServerIp = "";
			AddressFamily newAddressFamily = AddressFamily.InterNetwork;
			Ipv6Manager.GetIPType(FConnectIP, FServerPort.ToString(), out newServerIp, out newAddressFamily);
			if (!string.IsNullOrEmpty(newServerIp))
			{
				FConnectIP = newServerIp;
			}
			FSocket = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);
			FSocket.ReceiveBufferSize = FMaxRecBuf;
			FSocket.ReceiveTimeout = FSocketTimeOut;
			FSocket.Connect(FConnectIP,FServerPort);
#else
            FSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            FSocket.ReceiveBufferSize = FMaxRecBuf;
            FSocket.ReceiveTimeout = FSocketTimeOut;
            //FSocket.Connect(FConnectIP,FServerPort);
            FSocket.Connect(FIPEndPoint);
#endif
            // FSocket.NoDelay = true;
            Scheduler.Instance.AddUpdator(ReceiveMsg);
        }
        catch (Exception Ex)
        {
            Debug.LogError("SE-" + Ex.ToString());
            if (mConnectResult != null)
                mConnectResult(Ex.Message);
        }

    }

    public bool IsConnect()
    {
        if (FSocket != null)
        {
            if (FSocket.Connected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void DisConnect()
    {
        Scheduler.Instance.RemoveUpdator(ReceiveMsg);
        try
        {
            if (FSocket != null && FSocket.Connected)
            {
                FSocket.Shutdown(SocketShutdown.Both);
                FSocket.Close();
                FSocket = null;
                SocketExceptionEvent = null;
                mConnectResult = null;
                //Debug.LogError("We're still connnected");
            }
            else
            {
                FSocket = null;
            }
        }
        catch (Exception ex)
        {
            Initialize();
            Debug.LogError(ex.Message);
        }
        Initialize();
    }

    public void ReceiveMsg()
    {
        try
        {
            if (FSocket == null || !FSocket.Connected)
            {
                return;
            }
            FSocketBufCount = FSocket.Available;
            if (FSocketBufCount == 0) return;
            if (FSocketBufCount + FRecBufSize < FMaxRecBuf)
            {
                FPackageLength = FSocket.Receive(FRecTemp);
                if (FPackageLength == 0) return;
                Array.Copy(FRecTemp, 0, FRecBuf, FRecBufSize, FPackageLength);
                FRecBufSize += FPackageLength;
            }
            else
            {
                DisConnect();
                Debug.LogError("FRecBufSize > FMaxRecBuf  =  " + FRecBufSize);
                return;

            }
            ushort tmpCount = 0;
            while (FRecBufSize > 0)
            {
                tmpCount++;
                if (tmpCount > 1000)
                {
                    Debug.LogWarning("ReceiveMsg Count > 10000");
                    break;
                }
                if (!rcv_complete_msg())
                    break;
                if (FMsgHeader.mPacketLen == 0)
                {
                    Debug.LogError("FMsgHeader.mPacketLen == 0  " + FMsgHeader.ToString());
                    if (FRecBufSize - FRecBufIndex > 0)
                    {
                        Array.Copy(FRecBuf, FRecBufIndex, FRecBuf, 0, FRecBufSize - FRecBufIndex);
                        FRecBufSize -= FRecBufIndex;
                        FRecBufIndex = 0;
                        Debug.LogError(FRecBufIndex);
                        continue;
                    }
                    else
                    {
                        return;
                    }
                }
                Packet packetdata = new Packet();
                byte[] data = new byte[FMsgHeader.mPacketLen];
                packetdata.header.CopyTo(FMsgHeader);
                Array.Copy(FRecBuf, FRecBufIndex, data, 0, FMsgHeader.mPacketLen);
                packetdata.ms = new MemoryStream(data);
                packetdata.ms.Seek(0, SeekOrigin.Begin);
                if (mEventProcessor != null) mEventProcessor(packetdata);
                FRecBufIndex += (int)FMsgHeader.mPacketLen;
                FMsgHeader.Clear();
                if (FRecBufSize > FRecBufIndex)
                {
                    Array.Copy(FRecBuf, FRecBufIndex, FRecBuf, 0, FRecBufSize - FRecBufIndex);
                    FRecBufSize -= FRecBufIndex;
                    FRecBufIndex = 0;
                }
                if (FRecBufIndex == FRecBufSize)
                {
                    FRecBufIndex = 0;
                    FRecBufSize = 0;
                }
            }

            //if (FRecBufIndex > 0) 
            //{
            //Array.Copy(FRecBuf, FRecBufIndex, FRecBuf, 0, FRecBufSize - FRecBufIndex);
            //FRecBufSize -= FRecBufIndex;
            //FRecBufIndex = 0;
            //Debug.LogError(FRecBufIndex);
            //Debug.LogError(FRecBufSize);
            //}

        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            if (SocketExceptionEvent != null)
                SocketExceptionEvent(IsConnect());
        }

    }

    bool rcv_complete_msg()
    {
        if (FMsgHeader.IsInitHeader)
        {
            if (FRecBufSize - FRecBufIndex < FMsgHeader.mPacketLen)
                return false;
            else
                return true;
        }
        if (FRecBufSize < MessageHeader.HEADERLENGTH) return false;
        Array.Copy(FRecBuf, FRecBufIndex, FHeadBuf, 0, MessageHeader.HEADERLENGTH);
        FMsgHeader.BytesToMsgHeader(FHeadBuf);
        FRecBufIndex += MessageHeader.HEADERLENGTH;
        if (FRecBufSize - FRecBufIndex < FMsgHeader.mPacketLen)
            return false;
        return true;
    }

    public uint SetHeadType()
    {
        FMsgSN++;
        FMsgSN = FMsgSN % MAXMSGSEQID;
        uint fy_type = (uint)UnityEngine.Random.Range(1, 15);
        return fy_type | (FMsgSN << 4);

    }

    public void SendMsg<T>(uint uType, T t, uint uMsgid, uint uSessionid) where T : ProtoBuf.IExtensible
    {
        try
        {
            MemoryStream msgbody = new MemoryStream();
            ProtoBuf.Serializer.Serialize<T>(msgbody, t);
            msgbody.Seek(0, SeekOrigin.Begin);
            MessageHeader _head = new MessageHeader();
            _head.mType = SetHeadType();
            _head.mMsgID = uMsgid;
            _head.mPacketLen = (uint)msgbody.Length;
            _head.mAccountID = uSessionid;
            byte[] msgbodyArray = new byte[msgbody.Length];
            msgbody.Read(msgbodyArray, 0, msgbodyArray.Length);
            byte[] msgByteArray = new byte[MessageHeader.HEADERLENGTH + (int)msgbody.Length];
            Array.Copy(_head.ToByteArray(), 0, msgByteArray, 0, MessageHeader.HEADERLENGTH);
            Array.Copy(msgbodyArray, 0, msgByteArray, 16, (int)msgbody.Length);
            if (FSocket == null)
            {
                Debug.Log("FSocket == null");
                return;
            }
            if (!FSocket.Connected)
            {
                DisConnect();
                return;
            }
            FSocket.Send(msgByteArray);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            if (SocketExceptionEvent != null)
                SocketExceptionEvent(IsConnect());
        }
    }

    public void Uninitialize()
    {
        DisConnect();
    }

}
