using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using System.Linq;
using Assets.Script.Common;

public class AchievementViewController : UIBase 
{
    private List<Achievementinfo> _finishedList;
    private List<Achievementinfo> _awardList;
    private List<Achievementinfo> _timerList;
    private List<Achievementinfo> _underwayList;
    private List<Achievementinfo> _totalList;
    private List<AchievementItem> _itemList;

    public AchievementView view;
    private GameObject _achievementItem;
    private bool isAnimFinished;

    const int MAXCOUNT = 6;

    public override void Initialize()
    {
        if (view == null)
            view = new AchievementView();
        view.Initialize();
        BtnEventBinding();
        InitAndClearLists();

        LoadRes();
        SendAchievementListReq();
    }
    public override void Uninitialize()
    {
        foreach(AchievementItem item in _itemList)
        {
            item.StopTimeUpdate();
        }

        if (_finishedList != null)
        {
            _finishedList.Clear();
            _finishedList = null;
        }
        if (_awardList != null)
        {
            _awardList.Clear();
            _awardList = null;
        }
        if (_timerList != null)
        {
            _timerList.Clear();
            _timerList = null;
        }
        if (_underwayList != null)
        {
            _underwayList.Clear();
            _underwayList = null;
        }
        if (_totalList != null)
        {
            _totalList.Clear();
            _totalList = null;
        }

    }
    public override void Destroy()
    {
        _itemList.Clear();
        _itemList = null;
        view = null;
    }
    /// <summary>
    /// 退出成就界面
    /// </summary>
    public void ButtonEvent_Quit(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(AchievementView.UIName);
    }
    /// <summary>
    /// 退出领取成就奖励界面
    /// </summary>
    public void GetAchievement_Quit(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        CloseGetAchievement();
    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_Quit.gameObject).onClick = ButtonEvent_Quit;
        view.UIWrapContent_UIGrid.onInitializeItem = SetAchievementInfo;
        UIEventListener.Get(view.Spt_BGMask.gameObject).onClick = GetAchievement_Quit;
    }
    /// <summary>
    /// 获取单条成就模板资源
    /// </summary>
    private void LoadRes()
    {
        if (_achievementItem == null)
            _achievementItem = view.ScrView_AchievementGroup.transform.FindChild("AchievementItem").gameObject;
        if (_achievementItem == null) Debug.LogError("Can't find AchievementItem");
        _achievementItem.SetActive(false);
    }
    /// <summary>
    /// 向服务器请求成就列表信息
    /// </summary>
    public void SendAchievementListReq()
    {
        AchievementModule.Instance.SendAchievementListReq();
        //FillTest();
        //InitAchievementItem(_testList);
    }
    /// <summary>
    /// 向服务器请求领取成就奖励
    /// </summary>
    public void SendGetAchievementAward(int id)
    {
        AchievementModule.Instance.SendGetAchievementAwardReq(id);
        //UpadataTest();
        //ReceiveAchievementAward(_testList, 1040001);
    }
    /// <summary>
    /// 初始化成就条数据
    /// </summary>
    public void InitAchievementItem(List<Achievementinfo> data)
    {
        InitAndClearLists();
        OrderLists(data);
        Main.Instance.StartCoroutine(ShowAchievementList());
    }
    /// <summary>
    /// 分支列表初始化
    /// </summary>
    private void InitAndClearLists()
    {
        if (_finishedList == null)
        {
            _finishedList = new List<Achievementinfo>();
        }
        else
        {
            _finishedList.Clear();
        }
        if (_awardList == null)
        {
            _awardList = new List<Achievementinfo>();
        }
        else
        {
            _awardList.Clear();
        }
        if (_timerList == null)
        {
            _timerList = new List<Achievementinfo>();
        }
        else
        {
            _timerList.Clear();
        }
        if (_underwayList == null)
        {
            _underwayList = new List<Achievementinfo>();
        }
        else
        {
            _underwayList.Clear();
        }
        if (_totalList == null)
        {
            _totalList = new List<Achievementinfo>();
        }
        else
        {
            _totalList.Clear();
        }
        if (_itemList == null)
        {
            _itemList = new List<AchievementItem>();
        }
        //else
        //{
        //    _itemList.Clear();
        //}
    }
    /// <summary>
    /// 成就分类排序
    /// </summary>
    /// <param name="data"></param>
    private void OrderLists(List<Achievementinfo> data)
    {
        foreach (Achievementinfo info in data)
        {
            switch (info.status)
            {
                case 0:
                    _finishedList.Add(info);
                    break;
                case 1:
                    _awardList.Add(info);
                    break;
                case 2:
                    _underwayList.Add(info);
                    break;
                case 3:
                    _timerList.Add(info);
                    break;
            }
        }
        
        _totalList.AddRange(_awardList.OrderBy(s => s.id).ToList());
        _totalList.AddRange(_underwayList.OrderBy(s => s.id).ToList());
        _totalList.AddRange(_timerList.OrderBy(s => s.resettime).ToList());
        _totalList.AddRange(_finishedList.OrderBy(s => s.id).ToList());
    }
    /// <summary>
    /// 获取并展示各条成就信息
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowAchievementList()
    {
        int count = _totalList.Count;
        int itemCount = _itemList.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_UIGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_UIGrid.minIndex = -index;
        view.UIWrapContent_UIGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_UIGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_UIGrid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = itemCount - count; i < itemCount; i++)
            {
                _itemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(_achievementItem, view.Grd_UIGrid.transform);
                AchievementItem item = vGo.GetComponent<AchievementItem>();
                vGo.SetActive(true);
                _itemList.Insert(i, item);
                vGo.name = i.ToString();
                _itemList[i].EventGetAchievementAward = SendGetAchievementAward;
            }
            else
            {
                _itemList[i].gameObject.SetActive(true);
            }
            _itemList[i].SetInfo(_totalList[i]);
        }
        view.UIWrapContent_UIGrid.ReGetChild();
        yield return 0;
        view.Grd_UIGrid.repositionNow = true;
        yield return 0;
        view.ScrView_AchievementGroup.ResetPosition();
        view.Grd_UIGrid.repositionNow = true;
    }
    /// <summary>
    /// 更新成就条目信息
    /// </summary>
    public void SetAchievementInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= _totalList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        AchievementItem item = _itemList[wrapIndex];
        item.SetInfo(_totalList[realIndex]);
    }
    /// <summary>
    /// 领取成就奖励
    /// </summary>
    public void ReceiveAchievementAward(List<Achievementinfo> _getAchievementList,int getid)
    {
        InitAchievementItem(_getAchievementList);
        OpenGetAchievement(getid);
    }
    /// <summary>
    /// 打开领奖界面
    /// </summary>
    /// <param name="id"></param>
    private void OpenGetAchievement(int id)
    {
        
        AchievementItemData tmp = ConfigManager.Instance.mAchievementConfig.FindDataByID(id);
        if (tmp != null)
        {
            if (tmp.award_type == 1)
            {
                view.Spt_AwardIcon.gameObject.SetActive(true);
                CommonFunction.SetSpriteName(view.Spt_AwardIcon, ConfigManager.Instance.mPlayerPortraitConfig.GetPlayerPortraitByID((uint)tmp.award_id).icon);
                string Frame_A = string.Format(GlobalConst.SpriteName.Frame_Name_A, GlobalConst.SYSTEMFRAME);
                CommonFunction.SetSpriteName(view.Spt_AwardIconFrame, Frame_A);
            }
            else if (tmp.award_type == 2)
            {
                view.Spt_AwardIcon.gameObject.SetActive(false);
                string Frame_A = string.Format(GlobalConst.SpriteName.Frame_Name_A, tmp.award_id);
                CommonFunction.SetSpriteName(view.Spt_AwardIconFrame, Frame_A);
            }
            else
            {
                Debug.LogError("award type is wrong");
            }
            view.Lbl_UnlockLabel.text = string.Format(ConstString.ACHIEVEMENT_UNLOCK, tmp.name);
            if (tmp.type == 1)
            {
                view.Lbl_TimeLabel.gameObject.SetActive(false);
            }
            else if (tmp.type == 2)
            {
                view.Lbl_TimeLabel.gameObject.SetActive(true);
                if (tmp.award_type == 1)
                {
                    view.Lbl_TimeLabel.text = string.Format(ConstString.ACHIEVEMENT_CONSTTIME,
                        (ConfigManager.Instance.mPlayerPortraitConfig.GetConstTimeByID((uint)tmp.award_id)) / 3600);
                }
                else if (tmp.award_type == 2)
                {
                    view.Lbl_TimeLabel.text = string.Format(ConstString.ACHIEVEMENT_CONSTTIME,
                        (ConfigManager.Instance.mFrameConfig.GetConstTimeByID((uint)tmp.award_id)) / 3600);
                }
                else
                {
                    Debug.LogError("award type error");
                }
            }
            else
            {
                Debug.LogError("achievement type error");
            }

            view.Gobj_GetAchievemenet.SetActive(true);
            PlayEffectAnim();
            //PlayAwardAnim();
        }
        else
        {
            Debug.LogError("achievement id error");
        }
    }
    /// <summary>
    /// 播放解锁内容动画
    /// </summary>
    private void PlayAwardAnim()
    {

        if (!isAnimFinished)
        {
            view.TS_AWard_Scale.gameObject.SetActive(true);
            view.TS_AWard_Scale.from = Vector3.one * 0.001f;
            view.TS_AWard_Scale.to = Vector3.one;
            view.TS_AWard_Scale.delay = 0;
            view.TS_AWard_Scale.duration = 0.75f;
            view.TS_AWard_Scale.PlayForward();
            view.TS_AWard_Scale.SetOnFinished(SetOnFinished);
            
        }

    }
    private void SetOnFinished()
    {
        //Debug.LogError("set");
        isAnimFinished = true;
    }
    /// <summary>
    /// 播放解锁特效
    /// </summary>
    private void PlayEffectAnim()
    {
        isAnimFinished = false;

        view.TS_Effect_Scale.from = Vector3.one * 3;
        view.TS_Effect_Scale.to = Vector3.zero;
        view.TS_Effect_Scale.delay = 0.1f;
        view.TS_Effect_Scale.duration = 1.75f;
        view.TS_Effect_Scale.PlayForward();

        view.TR_Effect_Rotation.from = Vector3.zero ;
        view.TR_Effect_Rotation.to = new Vector3(0, 0, 360);
        view.TR_Effect_Rotation.delay = 0.1f;
        view.TR_Effect_Rotation.duration = 1.75f;
        view.TR_Effect_Rotation.PlayForward();
        Scheduler.Instance.AddTimer(1.7f, false, PlayAwardAnim);
    }
    /// <summary>
    /// 关闭领奖界面
    /// </summary>
    private void CloseGetAchievement()
    {
        
        if (isAnimFinished)
        {
            view.Gobj_GetAchievemenet.SetActive(false);
            view.TS_AWard_Scale.gameObject.SetActive(false);

            view.TS_AWard_Scale.Restart();
            view.TS_Effect_Scale.Restart();
            view.TR_Effect_Rotation.Restart();
        }
        else
        {          
            Scheduler.Instance.RemoveTimer(PlayAwardAnim);

            view.TS_Effect_Scale.duration = 0.01f;
            view.TR_Effect_Rotation.duration = 0.01f;
            view.TS_AWard_Scale.duration = 0.01f;
            view.TS_AWard_Scale.gameObject.SetActive(true);
            isAnimFinished = true;
        }

    }
    
    /*
    //--------------------------test data------------------------------------------//
    private List<Achievementinfo> _testList;
    private void FillTest()
    {
        if (_testList == null)
        {
            _testList = new List<Achievementinfo>();
        }
        else _testList.Clear();
        Achievementinfo test1 = new Achievementinfo();
        test1.id = 1010001;
        test1.status = 0;
        test1.resettime = 0;
        test1.cur_course = 1;
        _testList.Add(test1);

        Achievementinfo test11 = new Achievementinfo();
        test11.id = 1170001;
        test11.status = 0;
        test11.resettime = 0;
        test11.cur_course = 1;
        _testList.Add(test11);

        Achievementinfo test2 = new Achievementinfo();
        test2.id = 1050001;
        test2.status = 1;
        test2.resettime = 0;
        test2.cur_course = 100;
        _testList.Add(test2);

        Achievementinfo test21 = new Achievementinfo();
        test21.id = 1011001;
        test21.status = 1;
        test21.resettime = 0;
        test21.cur_course = 1;
        _testList.Add(test21);

        Achievementinfo test22 = new Achievementinfo();
        test22.id = 1040001;
        test22.status = 1;
        test22.resettime = 0;
        test22.cur_course = 1;
        _testList.Add(test22);

        Achievementinfo test3 = new Achievementinfo();
        test3.id = 1020001;
        test3.status = 2;
        test3.resettime = 0;
        test3.cur_course = 10;
        _testList.Add(test3);

        Achievementinfo test31 = new Achievementinfo();
        test31.id = 1200001;
        test31.status = 2;
        test31.resettime = 0;
        test31.cur_course = 486457869;
        _testList.Add(test31);

        Achievementinfo test32 = new Achievementinfo();
        test32.id = 1180001;
        test32.status = 2;
        test32.resettime = 0;
        test32.cur_course = 0;
        _testList.Add(test32);

        Achievementinfo test4 = new Achievementinfo();
        test4.id = 1120001;
        test4.status = 3;
        test4.resettime =(int) Main.mTime+20;
        test4.cur_course = 1;
        _testList.Add(test4);

        Achievementinfo test41 = new Achievementinfo();
        test41.id = 1130001;
        test41.status = 3;
        test41.resettime = (int)Main.mTime + 10;
        test41.cur_course = 1;
        _testList.Add(test41);
    }
    private void UpadataTest()
    {
        _testList[3].status = 0;
        _testList[4].status = 0;
        _testList[5].status = 0;

    }
    */
}
