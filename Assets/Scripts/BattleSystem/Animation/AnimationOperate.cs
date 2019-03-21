using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine;

public delegate void AnimationCompleteEvent();
public class AnimationOperate
{
    public AnimationCompleteEvent _CompleteFunction;
    public AnimationOperate(SkeletonAnimation vSkeletonAnimation)
    {
        _SkeletonAnimation = vSkeletonAnimation;
    }

    /// <summary>
    /// Spine动画控件
    /// </summary>
    private SkeletonAnimation _SkeletonAnimation;
    public SkeletonAnimation GetSkeletonAnimation
    {
        get {
            return _SkeletonAnimation;
        }
        set
        {
            _SkeletonAnimation = value;
        }
    }

    public void TestPause()
    {
        _SkeletonAnimation.timeScale = 0;
    }
    public void TestResume()
    {
        _SkeletonAnimation.timeScale = 1;
    }

    /// <summary>
    /// 当前动画层级
    /// </summary>
    private int _CurTrackIndex;
    public int GetCurTrackIndex
    {
        get
        {
            return _CurTrackIndex;
        }
    }

    /// <summary>
    /// 当前动画名字
    /// </summary>
    private string _CurName;
    public string GetCurName
    {
        get {
            return _CurName;
        }
    }

    /// <summary>
    /// 下一个排队播放动画[同一时间只记录一个]
    /// </summary>
    private AnimationItemInfo _NextInfo = new AnimationItemInfo();
    public AnimationItemInfo GetNextInfo
    {
        get {
            return _NextInfo;
        }
        set {
            if (_NextInfo.GetIsFull)
                return;
            _NextInfo.ReSetInfo(value);
        }
    }
    
    /// <summary>
    /// 是否唯一[true: 不能被强制中断以外操作打断]
    /// </summary>
    private bool _IsOnly;
    public bool GetIsOnly
    {
        get {
            return _IsOnly;
        }
    }
    
    /// <summary>
    /// 是否正在播放
    /// </summary>
    private bool _IsPlay;
    public bool GetIsPlay
    {
        get
        {
            return _IsPlay;
        }
    }

    /// <summary>
    /// 是否循环播放[true: 循环播放]
    /// </summary>
    private bool _IsLoop;
    public bool GetIsLoop
    {
        get
        {
            return _IsLoop;
        }
    }



    //-----------------------------------------检测-------------------------------------------//
    /// <summary>
    /// 当前动画是否已经播放完毕[true: 播放完毕]
    /// </summary>
    /// <param name="vTrackIndex">动画层级</param>
    /// <param name="vAnimationName">动画名字</param>
    /// <returns></returns>
    public bool CheckAnimationIsFinished(int vTrackIndex, string vAnimationName)
    {
        //检测角色动画控件是否存在//
        if (_SkeletonAnimation == null)
            return false;

        //检测当前是否正在播放动画
        if (_IsPlay)
        {//正在播放动画//
            if (vAnimationName == _CurName)
                return false;
            //检测需要播放动画层级是否比现有动画优先//
            if (_CurTrackIndex >= 0)
            {
                if (vTrackIndex < _CurTrackIndex)
                    return false;
                else if (vTrackIndex > _CurTrackIndex)
                    return true;
            }
            if (_IsOnly)
                return false;
        }
        else
        {//停止播放动画//
            if (_CurTrackIndex >= 0)
            {
                TrackEntry tmpTrackEntry = _SkeletonAnimation.state.GetCurrent(_CurTrackIndex);
                if (tmpTrackEntry != null)
                {
                    //检测是否需要循环播放//
                    if (tmpTrackEntry.Loop)
                    {
                        if (vAnimationName == _CurName)
                            return false;
                        //检测需要播放动画层级是否比现有动画优先//
                        if (vTrackIndex < _CurTrackIndex)
                            return false;
                    }
                }
            }
        }

        return true;
    }
    public bool CheckAnimationIsFinished()
    {
        return CheckAnimationIsFinished(_CurTrackIndex, _CurName);
    }

    //-----------------------------------------播放-------------------------------------------//
    /// <summary>
    /// 延迟播放动画
    /// </summary>
    /// <param name="vIsOnly">是否唯一[true: 不能被强制中断以外操作打断]</param>
    /// <param name="vTrackIndex">动画层级</param>
    /// <param name="vAnimationName">动画名字</param>
    /// <param name="vIsLoop">是否循环播放[true: 循环播放]</param>
    /// <param name="vDelay">延迟时间</param>
    /// <returns></returns>
    public IEnumerable DelayReSetAnimation(bool vIsOnly, int vTrackIndex, string vAnimationName, bool vIsLoop, float vDelay = 0, AnimationCompleteEvent vCompleteEvent = null)
    {
        yield return new WaitForSeconds(vDelay);
        ReSetAnimation(vIsOnly, vTrackIndex, vAnimationName, vIsLoop, vCompleteEvent);
    }
    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="vIsOnly">是否唯一[true: 不能被强制中断以外操作打断][true: ]</param>
    /// <param name="vTrackIndex">动画层级</param>
    /// <param name="vAnimationName">动画名字</param>
    /// <param name="vIsLoop">是否循环播放[true: 循环播放]</param>
    /// <returns></returns>
    public bool ReSetAnimation(bool vIsOnly, int vTrackIndex, string vAnimationName, bool vIsLoop, AnimationCompleteEvent vCompleteEvent = null)
    {
        if (!CheckAnimationIsFinished(vTrackIndex, vAnimationName))
            return false;
        if (_SkeletonAnimation == null)
            return false;

        _IsOnly = vIsOnly;
        _CurTrackIndex = vTrackIndex;
        _CurName = vAnimationName;
        _IsLoop = vIsLoop;
        _CompleteFunction = vCompleteEvent;
        _IsPlay = true;
        bool tmpFlipx = _SkeletonAnimation.skeleton.FlipX;
        _SkeletonAnimation.Reset();
        _SkeletonAnimation.skeleton.FlipX = tmpFlipx;
        _SkeletonAnimation.state.SetAnimation(vTrackIndex, vAnimationName, false);
        _SkeletonAnimation.state.Complete += CompleteAnimationOperate;
        return true;
    }
    /// <summary>
    /// 动画播放完毕操作
    /// </summary>
    /// <param name="state">动画状态</param>
    /// <param name="trackIndex">动画层级</param>
    /// <param name="LoopCount">已经播放的循环次数</param>
    private void CompleteAnimationOperate(Spine.AnimationState state, int trackIndex, int LoopCount)
    {
        _IsPlay = false;
        if (_CompleteFunction != null)
            _CompleteFunction();
        //检测是否有排队动画//
        if (!_NextInfo.GetIsFull)
        {//没有排队动画//
            //检测是否需要循环播放[true: 继续播放当前动画; false: 播放待机动画]//
            if (_IsLoop)
                ReSetAnimation(false, _CurTrackIndex, _CurName, true, _CompleteFunction);
            else
                ReSetAnimation(false, GlobalConst.ANIMATION_TRACK_BASE, GlobalConst.ANIMATION_NAME_IDLE, true);
            return;
        }
        else
        {//存在排队动画//
            ReSetAnimation(_NextInfo._IsOnly, _NextInfo._TrackIndex, _NextInfo._AnimationName, _NextInfo._IsLoop, _NextInfo._CompleteFunction);
        }
    }

    //-----------------------------------------停止-------------------------------------------//
    /// <summary>
    /// 强行停止当前动画并播放待机动画
    /// </summary>
    public void StopCurAnimation()
    {
        if (_SkeletonAnimation == null)
            return;

        _CurTrackIndex = -1;
        _CurName = string.Empty;
        _IsOnly = false;
        _IsPlay = false;
        _IsLoop = false;
        _CompleteFunction = null;
        ReSetAnimation(false, GlobalConst.ANIMATION_TRACK_BASE, GlobalConst.ANIMATION_NAME_IDLE, true);
    }

}

/// <summary>
/// 单个动画信息
/// </summary>
public class AnimationItemInfo
{
    private bool _IsFull;//是否拥有数据[true: 有数据]//
    public int _TrackIndex;//动画层级//
    public string _AnimationName;//动画名字//
    public bool _IsLoop;//是否循环//
    public bool _IsOnly;//是否唯一//
    public AnimationCompleteEvent _CompleteFunction;

    public bool GetIsFull
    {
        get
        {
            return _IsFull;
        }
    }
    public AnimationItemInfo()
    {
        ClearInfo();
    }
    public AnimationItemInfo(int vTrackIndex, string vAnimationName, bool vIsLoop, bool vIsOnly, AnimationCompleteEvent vCompleteFunction)
    {
        ReSetInfo(vTrackIndex, vAnimationName, vIsLoop, vIsOnly, vCompleteFunction);
    }

    public void ClearInfo()
    {
        this._IsFull = false;
        this._TrackIndex = 0;
        this._AnimationName = string.Empty;
        this._IsLoop = false;
        this._IsOnly = false;
        this._CompleteFunction = null;
    }
    public void ReSetInfo(int vTrackIndex, string vAnimationName, bool vIsLoop, bool vIsOnly, AnimationCompleteEvent vCompleteFunction)
    {
        ClearInfo();
        this._IsFull = true;
        this._TrackIndex = vTrackIndex;
        this._AnimationName = vAnimationName;
        this._IsLoop = vIsLoop;
        this._IsOnly = vIsOnly;
        this._CompleteFunction = vCompleteFunction;
    }
    public void ReSetInfo(AnimationItemInfo vAnimationItemInfo)
    {
        if (vAnimationItemInfo == null)
            return;
        ReSetInfo(vAnimationItemInfo._TrackIndex, vAnimationItemInfo._AnimationName, vAnimationItemInfo._IsLoop, vAnimationItemInfo._IsOnly, vAnimationItemInfo._CompleteFunction);
    }
}