using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using System.Collections.Generic;

public class AchievementModule : Singleton<AchievementModule>
{
    private AchievementNetWork mNetWork;
    public List<Achievementinfo> _achievementList;
    public List<Achievementinfo> _getAchievementList;

    //-------------------------------初始化与逆初始化-----------------------------------//
    public void Initialize()
    {
        if (mNetWork == null)
        {
            mNetWork = new AchievementNetWork();
            mNetWork.RegisterMsg();
        }
        if (_achievementList == null)
        {
            _achievementList = new List<Achievementinfo>();
        }
        else
        {
            _achievementList.Clear();
        }
        if (_getAchievementList == null)
        {
            _getAchievementList = new List<Achievementinfo>();
        }
        else
        {
            _getAchievementList.Clear();
        }
    }
    public void Uninitialize()
    {
        if (_achievementList != null)
        {
            _achievementList.Clear();
            _achievementList = null;
        }

        mNetWork.RemoveMsg();
        mNetWork = null;
    }
    //--------------------------------网络通讯函数---------------------------------------//
    /// <summary>
    /// 请求成就清单
    /// </summary>
    public void SendAchievementListReq()
    {
        Debug.Log("send achievementlist requst");
        EnterAchievementReq data = new EnterAchievementReq();
        mNetWork.EnterAchievementReq(data);
    }
    /// <summary>
    /// 接收处理成就清单
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveAchievementListResp(EnterAchievementResp data)
    {
        Debug.Log("receive achievementlist requst");
        if (data == null)
        {
            Debug.LogError("none data");
        }
        else {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                try
                {
                    _achievementList.Clear();
                    foreach (Achievementinfo achieve in data.aminfo)
                    {
                        Achievementinfo tmp = new Achievementinfo();
                        tmp.id = achieve.id;
                        tmp.status = achieve.status;
                        tmp.resettime = achieve.resettime;
                        tmp.cur_course = achieve.cur_course;
                        _achievementList.Add(tmp);
                    }
                    UISystem.Instance.AchievementView.InitAchievementItem(_achievementList);
                }
                catch
                {
                    Debug.LogError("there's something wrong in achievementinfo");
                }
            }
            else
            {
                ErrorCode.ShowErrorTip(data.result);
            }
        }
    }
    /// <summary>
    /// 发送领取奖励请求
    /// </summary>
    /// <param name="id"></param>
    public void SendGetAchievementAwardReq(int id)
    {
        Debug.Log("SendGetAchievementAwardReq");
        GetAchievementReq data = new GetAchievementReq();
        data.id = id;
        mNetWork.GetAchievementReq(data);
    }
    /// <summary>
    /// 接收领取奖励回复
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveGetAchievementAwardReq(GetAchievementResp data)
    {
        Debug.Log("ReceiveGetAchievementAwardReq");
        if (data == null)
        {
            Debug.LogError("none data");
        }
        else
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                try
                {
                    _getAchievementList.Clear();
                    foreach (Achievementinfo achieve in data.aminfo)
                    {
                        Achievementinfo tmp = new Achievementinfo();
                        tmp.id = achieve.id;
                        tmp.status = achieve.status;
                        tmp.resettime = achieve.resettime;
                        tmp.cur_course = achieve.cur_course;
                        _getAchievementList.Add(tmp);
                    }
                    UISystem.Instance.AchievementView.ReceiveAchievementAward(_getAchievementList,data.getid);
                }
                catch
                {
                    Debug.LogError("there's something wrong in achievementinfo");
                }
            }
            else
            {
                ErrorCode.ShowErrorTip(data.result);
            }
        }

    }
    //---------------------------------------功能函数--------------------------------//

}
