using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using Assets.Script.Common;
public class HttpNetWork 
{
    const uint MAXMSGSEQID = 67108863;//序列号最大值//
    const float MAXTIMEOUT = 10.0F;
    const uint MAXREPEATCOUNT = 2;
    const uint MAXERRORCOUNT = 2;
    private string URL = "";
    private MessageHeader FMsgHeader = new MessageHeader();
    private byte[] FHeadBuf = new byte[MessageHeader.HEADERLENGTH];
    private int FRecBufIndex = 0;
    private uint FMsgSN = 0;
    private bool Sending = true;
    private int mRepeatCount = 0;
    private int mErrorCount = 0;
    public PacketHandle mEventProcessor;
    public SystemError mErrorMsg;
    private Queue<byte[]> SendBufQueue = new Queue<byte[]>();
    private Queue<byte[]> ReceiveBufQueue = new Queue<byte[]>();
	private EncryptType enType = EncryptType.ET_NONE;
	private uint maxEnLen = 0;
	private uint enKey = 0;
    private uint msgid = 0;

    enum EncryptType
    {
        ET_NONE = 1,
        ET_ALL = 2,
        ET_PART = 3,
    }

    public HttpNetWork(string vUrl) 
    {
        URL = vUrl;
        Initialize();
        Scheduler.Instance.AddUpdator(Update);
    }

    public void Initialize()
    {
        Sending = true;
        mRepeatCount = 0;
        mErrorCount = 0;
		enType = EncryptType.ET_NONE;
    }

	private uint getEncryptKey(uint rawNum)
	{
		List<uint> digits = new List<uint>();;
		while (rawNum/10 != 0) {
			digits.Add(rawNum%10);
			rawNum = rawNum/10;
		}
		digits.Add (rawNum);
		uint totalNum = 0;
        for (int i = 0; i < digits.Count; i++)
        {
            totalNum = totalNum + digits[i] * digits[i] * digits[i] + digits[i] * digits[i] + digits[i];
        }
		return  totalNum;
	}

	private uint getMaxEncryptLen(uint rawNum)
	{
		List<uint> digits = new List<uint>();
		while (rawNum/10 != 0) {
			digits.Add(rawNum%10);
			rawNum = rawNum/10;
		}
		digits.Add (rawNum);
		uint totalNum = 0;
        for (int i = 0; i < digits.Count;i++)
        {
            totalNum = totalNum + digits[i];
        }
		return  totalNum;
	}

	public void SetEncryptInfo(uint encrypt_key)
	{
		if (encrypt_key == 0 )
		{
			return;
		}

		if (encrypt_key % 2 == 0)
		{
			enType = EncryptType.ET_ALL;
			maxEnLen = 0;
		}
		else
		{
			enType = EncryptType.ET_PART;
			maxEnLen = getMaxEncryptLen(encrypt_key);
		}
		enKey = getEncryptKey(encrypt_key);
	}

	public byte[] encryptMsg(byte[] bytes)
	{
		if (enType == EncryptType.ET_NONE) 
		{
			return bytes;
		} 
		else
		{

			uint len = Convert.ToUInt32(bytes.Length);
			if(enType == EncryptType.ET_PART)
			{
				len = Math.Min(len, maxEnLen);
			}

			len = (len/4) * 4;		//保证长度为4的倍数
			for (int i=0; i< len; i = i + 4)
			{
				uint rawNum = BitConverter.ToUInt32(bytes, i);
				rawNum = rawNum ^ enKey;
				byte[ ] byteArray = BitConverter.GetBytes(rawNum);
				for(int j = 0 ; j < byteArray.Length; j++)
				{
					bytes[i+j] = byteArray[j];
				}
			}
		}
		return bytes;
	}


    private void SendMsgError(string error)
    {
        DisConnect();
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,error);
    }
	
    public void Update() 
    {
        if (SendBufQueue.Count > 0 && Sending)
        {
            Main.Instance.StartCoroutine(SendMsgToServer(SendBufQueue.Dequeue()));
        }

        if (ReceiveBufQueue.Count > 0)
        {
            ReceiveMsg(ReceiveBufQueue.Dequeue());
        }
    }

    public IEnumerator SendMsgToServer(byte[] bytes) 
    {
        WWW www;
        Sending = false;
        float timeOut = 0.0f;
        try 
        {
            www = new WWW(URL, bytes);
        }catch(System.Exception ex )
        {
            Debug.LogError(ex.Message);
            if (mErrorMsg != null)
                mErrorMsg(bytes, "Error");
            yield break;
        }
        while (!www.isDone)
        {
            timeOut += Time.deltaTime;
            if (timeOut > MAXTIMEOUT)
            {
                www.Dispose();
                www = null;
                mRepeatCount++;
                if (mRepeatCount < MAXREPEATCOUNT)
                {
                    Main.Instance.StartCoroutine(SendMsgToServer(bytes));
                }
                else
                {
                    Debug.LogError(" TimeOut  Repeat Send Count  = " + mRepeatCount);
                    if (mErrorMsg != null)
                    mErrorMsg(bytes,"TimeOut");
                }
                yield break;
            }
            yield return 0;
        }
        yield return www;
        Sending = true;
        mRepeatCount = 0;
        if (!string.IsNullOrEmpty(www.error)) 
        {
            Debug.LogError(www.error);
            if (mErrorCount < MAXERRORCOUNT)
            {
                mErrorCount++;
                Main.Instance.StartCoroutine(SendMsgToServer(bytes));
            }   
            else 
            {
                if (mErrorMsg != null)
                    mErrorMsg(bytes, www.error);
            }
            yield break;
        }
        mErrorCount = 0;
        ReceiveBufQueue.Enqueue(www.bytes);
        www.Dispose();
    }

    public uint SetHeadType()
    {
        uint fy_type = (uint)UnityEngine.Random.Range(1, 15);
        return fy_type | (FMsgSN << 4);
    }

    public void SendMsg<T>(uint uType, T t, uint uMsgid, uint uSessionid ,bool needMask = true) where T : ProtoBuf.IExtensible
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

			msgbodyArray = encryptMsg(msgbodyArray);

            byte[] msgByteArray = new byte[MessageHeader.HEADERLENGTH + (int)msgbody.Length];
            Array.Copy(_head.ToByteArray(), 0, msgByteArray, 0, MessageHeader.HEADERLENGTH);
            Array.Copy(msgbodyArray, 0, msgByteArray, 16, (int)msgbody.Length);
            FMsgSN++;
            //Debug.LogError("HttpURL  " + URL + "  uMsgid = " + uMsgid + " FMsgSN =  " + FMsgSN);
            FMsgSN = FMsgSN % MAXMSGSEQID;

            SendBufQueue.Enqueue(msgByteArray);
            if (needMask)
                ShowLoading(true);
        }
        catch (Exception ex)
        {
            Debug.LogError("Http SendMsg" + ex.Message); 
        }
    }

    public void ReceiveMsg(byte[] bytes)
    {
		ShowLoading(false);
        //try
        //{
            FRecBufIndex = 0;
            int FRecBufSize = bytes.Length;
            while (FRecBufIndex < bytes.Length)
            {
                if (bytes.Length < MessageHeader.HEADERLENGTH)
                {
                    Debug.LogError("ReceiveMsg -> bytes.Length < MessageHeader.HEADERLENGTH");
                    return;
                } 
                Array.Copy(bytes, FRecBufIndex, FHeadBuf, 0, MessageHeader.HEADERLENGTH);
                FMsgHeader.BytesToMsgHeader(FHeadBuf);
                FRecBufIndex += MessageHeader.HEADERLENGTH;
                if (FRecBufSize - FRecBufIndex < FMsgHeader.mPacketLen)
                {
                    Debug.LogError("ReceiveMsg -> FRecBufSize - FRecBufIndex < FMsgHeader.mPacketLen");
                    return;
                }
                Packet packetdata = new Packet();
                byte[] data = new byte[FMsgHeader.mPacketLen];
                packetdata.header.CopyTo(FMsgHeader);
                Array.Copy(bytes, FRecBufIndex, data, 0, FMsgHeader.mPacketLen);

				data = encryptMsg (data);               
				packetdata.ms = new MemoryStream(data);
                packetdata.ms.Seek(0, SeekOrigin.Begin);
                msgid = packetdata.header.mMsgID;
                if (mEventProcessor != null) mEventProcessor(packetdata);
                FRecBufIndex += (int)FMsgHeader.mPacketLen;
            }
         //}
         ////catch (Exception ex)
         ////{
         ////    Debug.LogError("MSGID = " + msgid + " Http ReceiveMsg " + ex.Message); 
         ////}
       
    }

    private void ShowLoading(bool show)
    {
        if(UISystem.Instance.UIIsOpen(HintView.UIName))
        {
            UISystem.Instance.HintView.ShowLoading(show);
        }
    }

    public void DisConnect() 
    {
        SendBufQueue.Clear();
        ReceiveBufQueue.Clear();
        FMsgSN = 0;
        URL = "";
        mRepeatCount = 0;
        Sending = true;
        Scheduler.Instance.RemoveUpdator(Update);
    }

    public void Uninitialize()
    {
        DisConnect(); 
    }

}
