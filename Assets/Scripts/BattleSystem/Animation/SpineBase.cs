using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Assets.Script.Common;
namespace TdSpine
{
    using animationInf = KeyValuePair<string, bool>;
    using _animationInf = KeyValuePair<KeyValuePair<string, bool>, uint>;
    public class SpineBase : MonoBehaviour
    {
        public SkeletonAnimation skeletonAnimation;
        public SkeletonAnimation hourseAnimation = null;
        /// <summary>
        /// 当前倍数
        /// </summary>
        private float  multiplicator = 1.0f;
        /// <summary>
        /// 当前时间比例
        /// </summary>
        private float _CurTimeScale = 1.0f;
        /// <summary>
        /// 是否暂停
        /// </summary>
        private bool _IsPause;
        protected float arf = 0;
        List<_animationInf> holdAnimationS = new List<_animationInf>();
        /// <summary>
        /// 当前动画开启时间
        /// </summary>
        private float _CurAnimationStartTime = 0.0f;
        /// <summary>
        /// 当前动画持续时间
        /// </summary>
        float _durTime = 0.0f;
        /// <summary>
        /// 角色大小
        /// </summary>
        private Vector3 size = Vector3.zero;
        private Color currentColor = Color.white;
        bool isLock = false;
        bool isHadHourse = false;

        public delegate void AnimationDelegate(string animationName, float time);
        /// <summary>
        /// 动作播放代理事件，会得到当前播放动作名字和已经播放的时间
        /// </summary>
        public event AnimationDelegate AnimationTimeEvent;

        public delegate void EventsDelegate(string animationName, string eventName);
        /// <summary>
        /// 帧事件代理
        /// </summary>
        public event EventsDelegate EventsEvent;

        public delegate void EndDelegate(string animationName);
        /// <summary>
        /// 动作播放完成代理
        /// </summary>
        public event EndDelegate EndEvent;

        public delegate void StartDelegate(string animationName);
        /// <summary>
        /// 动作播放开始代理
        /// </summary>
        public event StartDelegate StartEvent;

        void OnEvents(Spine.AnimationState state, int trackIndex, Spine.Event e)
        {
            if (EventsEvent != null)
                EventsEvent(state.ToString(),e.Data.name);
        }
        protected virtual void Awake()
        {
            _IsPause = false;
            Scheduler.Instance.AddUpdator(AnimationUpdate);
        }
        protected virtual void Start()
        {
            if (skeletonAnimation == null)
                skeletonAnimation = GetComponent<SkeletonAnimation>();
            this.WeaponSlotChange(true, false);
            Main.Instance.StartCoroutine(GetPos());
        }
        protected IEnumerator GetPos()
        {
            yield return new WaitForSeconds(2.0f);
            this.size = this.GetStartSize();
            //if(this.size == Vector3.zero)
            //{
            //    Main.Instance.StartCoroutine(GetPos());
            //}
        }
        void OnDestroy()
        {
            if (skeletonAnimation != null && skeletonAnimation.state != null)
            {
                skeletonAnimation.state.Event -= OnEvents;
            }
            Scheduler.Instance.RemoveUpdator(AnimationUpdate);
        }
        public void Clear()
        {
            this.holdAnimationS.Clear();
        }
        public virtual void UnInit()
        {
            if (this.skeletonAnimation != null && skeletonAnimation.state != null)
                skeletonAnimation.state.Event -= OnEvents;
            this.hourseAnimation = null;
            this.skeletonAnimation = null;
            if (this.holdAnimationS != null)
                this.holdAnimationS.Clear();
        }
        public void DeleteEvent()
        {
            if (this.skeletonAnimation != null)
                skeletonAnimation.state.Event -= OnEvents;
        }
        public virtual void InitSkeletonAnimation()
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            if (this.skeletonAnimation == null)
                return;
            this.skeletonAnimation.Reset();
            if (this.skeletonAnimation != null && this.skeletonAnimation.state != null)
                skeletonAnimation.state.Event += OnEvents;
            this.WeaponSlotChange(true,false);
        }
        /// <summary>
        /// 每帧检测动画
        /// </summary>
        protected virtual void AnimationUpdate()
        {
            if (_IsPause)
                return;
            if (skeletonAnimation == null || holdAnimationS == null || skeletonAnimation.state == null || skeletonAnimation.state.Data == null)
                return;
            float tmpTime = Time.time;

            if (_CurAnimationStartTime != 0.0f && ((tmpTime - _CurAnimationStartTime) * _CurTimeScale * multiplicator) >= _durTime)// && !isStaic)
            {
                if (holdAnimationS.Count != 0)
                {
                    _animationInf tempInf = holdAnimationS[0];
                    holdAnimationS.RemoveAt(0);

                    if (EndEvent != null)
                        EndEvent(tempInf.Key.Key);
                }
                if (holdAnimationS.Count != 0)
                {
                    //skeletonAnimation.Reset();
                    skeletonAnimation.state.SetAnimation(0, holdAnimationS[0].Key.Key, holdAnimationS[0].Key.Value);
                    if (hourseAnimation != null)
                    {
                        //hourseAnimation.Reset();
                        hourseAnimation.state.SetAnimation(0, holdAnimationS[0].Key.Key, holdAnimationS[0].Key.Value);
                    }
                    if(holdAnimationS[0].Key.Key == "death")
                    {
                        this.WeaponSlotChange(false, true);
                    }
                    else
                    {
                        this.WeaponSlotChange(true, false);
                    }

                    if (StartEvent != null)
                        StartEvent(holdAnimationS[0].Key.Key);
                    var a = skeletonAnimation.state.Data;
                    if (a != null || a.SkeletonData != null || a.SkeletonData.FindAnimation(holdAnimationS[0].Key.Key) != null)
                    {
                        _durTime = a.SkeletonData.FindAnimation(holdAnimationS[0].Key.Key).duration;
                    }
                    _CurAnimationStartTime = Time.time;
                }
            }
            if (holdAnimationS.Count != 0)
            {
                if (AnimationTimeEvent != null)
                    AnimationTimeEvent(holdAnimationS[0].Key.Key, tmpTime - _CurAnimationStartTime);
            }
        }
        /// <summary>
        /// 将动作加入动作序列，优先级越高越先播放
        /// </summary>
        /// <param name="animationName"></param>
        /// <param name="isLoop"></param>
        /// <param name="proprity"></param>
        /// <returns></returns>
        public bool pushAnimation(String animationName, bool isLoop, uint proprity)
        {
            //if (this.GetComponent<RoleBase>() != null)
            //{
            //    if ((this.GetComponent<RoleBase>().Get_RoleType == ERoleType.ertHero) && (this.GetComponent<RoleBase>().Get_RoleCamp == EFightCamp.efcSelf))
            //    {
            //        Debug.LogWarning(string.Format("pushAnimation: {0}", animationName));
            //    }
            //}
            if (isLock) return false;
            if (skeletonAnimation == null)
                return false;
            if (!this.CheckIsHaveAnimation(animationName) || !this.CheckIsHourseHaveAnimation(animationName)) return false;
            _animationInf tempAnimation = new _animationInf(new animationInf(animationName, isLoop), proprity);
            if (holdAnimationS.Contains(tempAnimation))
                return true;
            if (holdAnimationS.Count == 0)
            {
                //skeletonAnimation.Reset();
                skeletonAnimation.state.SetAnimation(0, animationName, isLoop);
                if (hourseAnimation != null)
                {
                    //hourseAnimation.Reset();
                    hourseAnimation.state.SetAnimation(0, animationName, isLoop);
                }
                if (StartEvent != null)
                    StartEvent(animationName);

                if (animationName == "death")
                {
                    this.WeaponSlotChange(false, true);
                }
                else
                {
                    this.WeaponSlotChange(true, false);
                }

                var a = skeletonAnimation.state.Data;
                _durTime = a.SkeletonData.FindAnimation(animationName).duration;
                _CurAnimationStartTime = Time.time;
            }
            int index = holdAnimationS.FindIndex
                (
                    (_animationInf a) =>
                    {
                        return a.Value < proprity;
                    }
                 );
            if (index == -1)
            {
                holdAnimationS.Add(tempAnimation);
            }
            else
            {
                if (index == 0)
                    holdAnimationS.Insert(1, tempAnimation);
                else
                    holdAnimationS.Insert(index, tempAnimation);
            }
            return true;
        }
        /// <summary>
        /// 终止当前动作，强制插入动作，并且会清空当前动画缓存
        /// </summary>
        /// <param name="animationName"></param>
        /// <param name="isLoop"></param>
        /// <returns></returns>
        public bool repleaceAnimation(String animationName, bool isLoop)
        {
            //if (this.GetComponent<RoleBase>() != null)
            //{
            //    if ((this.GetComponent<RoleBase>().Get_RoleType == ERoleType.ertHero) && (this.GetComponent<RoleBase>().Get_RoleCamp == EFightCamp.efcSelf))
            //    {
            //        Debug.LogWarning(string.Format("repleaceAnimation: {0}", animationName));

            //    }
            //}
            if (isLock || animationName == "") return false;
            if (skeletonAnimation == null || skeletonAnimation.state == null || skeletonAnimation.state.Data == null)
                return false;
            if (!this.CheckIsHaveAnimation(animationName)) return false;

            _animationInf tempAnimation = new _animationInf(new animationInf(animationName, isLoop), 0);
            if (holdAnimationS.Contains(tempAnimation))
            {
                if(holdAnimationS[0].Key.Key == animationName)
                {
                    this.holdAnimationS.RemoveRange(1,this.holdAnimationS.Count - 1);
                }
                return false;
            }
            skeletonAnimation.state.ClearTracks();
            //skeletonAnimation.Reset();
            skeletonAnimation.state.SetAnimation(0, animationName, isLoop);
            if (StartEvent != null)
                StartEvent(animationName);
            if (hourseAnimation != null && hourseAnimation.state != null)
            {
                if (this.CheckIsHourseHaveAnimation(animationName))
                {
                    hourseAnimation.state.ClearTracks();
                    //hourseAnimation.Reset();
                    hourseAnimation.state.SetAnimation(0, animationName, isLoop);
                }
            }
            if (animationName == "death")
            {
                this.WeaponSlotChange(false, true);
            }
            else
            {
                this.WeaponSlotChange(true, false);
            }
            var a = skeletonAnimation.state.Data;
            _durTime = a.SkeletonData.FindAnimation(animationName).duration;
            _CurAnimationStartTime = Time.time;
            //isStaic = isLoop;
            if (holdAnimationS.Count != 0)
            {
                if (EndEvent != null)
                    EndEvent(holdAnimationS[0].Key.Key);
                holdAnimationS.Clear();
            }
            holdAnimationS.Add(tempAnimation);
            //if (this.GetComponent<RoleBase>() != null)
            //{
            //    if ((this.GetComponent<RoleBase>().Get_RoleType == ERoleType.ertHero) && (this.GetComponent<RoleBase>().Get_RoleCamp == EFightCamp.efcSelf))
            //    {
            //        Debug.LogWarning(string.Format("holdAnimationS Count: {0}", holdAnimationS.Count));

            //    }
            //}

            return true;
        }
        /// <summary>
        /// 加入延时播放动画（建议别用我还没想好怎么去设计更符合实际情况）
        /// </summary>
        /// <param name="animationName"></param>
        /// <param name="isLoop"></param>
        /// <param name="delayTime"></param>
        /// <returns></returns>
        public bool addDelayAnimation(String animationName, bool isLoop, float delayTime)
        {
            return false;

            //if (isLock) return false;
            //_animationInf tempAnimation = new _animationInf(new animationInf(animationName, isLoop), 0);
            //if (holdAnimationS.Contains(tempAnimation))
            //    return false;
            //skeletonAnimation.state.AddAnimation(0, animationName, isLoop, delayTime);

            //return true;
        }
        /// <summary>
        /// 设置播放快慢
        /// </summary>
        /// <param name="timeScare"></param>
        /// <returns></returns>
        public bool setTimeScare(float timeScare)
        {
            _CurTimeScale = timeScare;
            if (skeletonAnimation != null)
                skeletonAnimation.timeScale = timeScare * this.multiplicator;
            if (hourseAnimation != null)
            {
                hourseAnimation.timeScale = timeScare * this.multiplicator;
            }
            //_CurAnimationStartTime = Time.time;
            return true;
        }
        /// <summary>
        /// 设置倍数播放
        /// </summary>
        /// <param name="f_Multiplicator"></param>
        /// <returns></returns>
        public bool SetMultiple(float f_Multiplicator)
        {
            this.multiplicator = f_Multiplicator;
            if (skeletonAnimation != null)
                skeletonAnimation.timeScale = this._CurTimeScale * this.multiplicator;
            if (hourseAnimation != null)
            {
                hourseAnimation.timeScale = this._CurTimeScale * this.multiplicator;
            }
            return true;
        }
        /// <summary>
        /// 暂停
        /// </summary>
        /// <returns></returns>
        public bool Pause()
        {
            if (skeletonAnimation == null)
                return false;
            //_CurTimeScale = skeletonAnimation.timeScale;
            skeletonAnimation.timeScale = 0;
            if (hourseAnimation != null)
            {
                hourseAnimation.timeScale = 0;
            }
            _IsPause = true;
            _durTime = _durTime - (Time.time - _CurAnimationStartTime);
            return true;
        }
        /// <summary>
        /// 继续
        /// </summary>
        /// <returns></returns>
        public bool Resume()
        {
            if (skeletonAnimation == null)
                return false;
            if (_CurTimeScale != 0)
            {
                //skeletonAnimation.timeScale = _CurTimeScale;
                skeletonAnimation.timeScale = this._CurTimeScale * this.multiplicator;
                if (hourseAnimation != null)
                {
                    //hourseAnimation.timeScale = _CurTimeScale;
                    hourseAnimation.timeScale = this._CurTimeScale * this.multiplicator;
                }
                _IsPause = false;
                _CurAnimationStartTime = Time.time;
            }
            return true;
        }
        /// <summary>
        /// 锁住动画，不会继续播放动画无法添加和强制插入
        /// </summary>
        /// <returns></returns>
        public bool Lock()
        {
            Pause();
            isLock = true;
            return true;
        }
        /// <summary>
        /// 解锁
        /// </summary>
        /// <returns></returns>
        public bool unLock()
        {
            Resume();
            isLock = false;
            return true;
        }
        /// <summary>
        /// 设置层级
        /// </summary>
        /// <param name="_order"></param>
        /// <returns></returns>
        public virtual bool  setSortingOrder(int _order)
        {
            if (skeletonAnimation == null)
                return false;
            if (hourseAnimation != null)
            {
                skeletonAnimation.renderer.sortingOrder = _order + 1;
                hourseAnimation.renderer.sortingOrder = _order;
            }
            else
            {
                skeletonAnimation.renderer.sortingOrder = _order;
            }
            return true;
        }
        public void TurnLeft()
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);

            //if(skeletonAnimation != null)
            //{
            //    skeletonAnimation.skeleton.flipX = true;
            //}
            //if(hourseAnimation != null)
            //{
            //    hourseAnimation.skeleton.flipX = true;
            //}
        }
        public void TurnRight()
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0,0);
            //if (skeletonAnimation != null)
            //{
            //    skeletonAnimation.skeleton.flipX = false;
            //}
            //if (hourseAnimation != null)
            //{
            //    hourseAnimation.skeleton.flipX = false;
            //}
        }
        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="color"></param>
        /// <param name="time"></param>
        public void SetColor(Color color)
        {
            Color ttt = new Color(color.r, color.g, color.b);
            if (this.skeletonAnimation != null)
            {
                foreach (Slot slot in this.skeletonAnimation.skeleton.Slots)
                {
                    if (slot.data.name == "weapon1" || slot.data.name == "weapon1_death")
                    {
                        if (slot.a == 0)
                            continue;
                    }
                    ttt.a = slot.a;
                    slot.SetColor(ttt);
                }
                this.currentColor = color;
            }
            if (this.hourseAnimation != null && this.hourseAnimation.skeleton != null)
            {
                foreach (Slot slot in this.hourseAnimation.skeleton.Slots)
                {
                    ttt.a = slot.a;
                    slot.SetColor(color);
                }
            }
        }
        public void SetColorNoSave(Color color)
        {
            Color ttt = new Color(color.r, color.g, color.b);
            if (this.skeletonAnimation != null && this.skeletonAnimation.skeleton != null)
            {
                foreach (Slot slot in this.skeletonAnimation.skeleton.Slots)
                {
                    if (slot.data.name == "weapon1" || slot.data.name == "weapon1_death")
                    {
                        if (slot.a == 0)
                            continue;
                    }
                    ttt.a = slot.a;
                    slot.SetColor(ttt);
                }
            }

            if (this.hourseAnimation != null && this.hourseAnimation.skeleton != null)
            {
                foreach (Slot slot in this.hourseAnimation.skeleton.Slots)
                {
                    ttt.a = slot.a;
                    slot.SetColor(ttt);
                }
            }
        }
        /// <summary>
        /// 颜色渐变
        /// </summary>
        /// <param name="color">渐变达到的颜色</param>
        /// <param name="time">多少秒后变回原本的颜色</param>
        public void FadeColor(Color color,float time = 0.1f)
        {
            Color ttt = new Color(color.r, color.g, color.b);
            if (this.skeletonAnimation != null && this.skeletonAnimation.skeleton != null)
            {
                foreach (Slot slot in this.skeletonAnimation.skeleton.Slots)
                {
                    if (slot.data.name == "weapon1" || slot.data.name == "weapon1_death")
                    {
                        if(slot.a == 0)
                        continue;
                    }
                    ttt.a = slot.a;
                    slot.SetColor(ttt);
                }
            }

            if (this.hourseAnimation != null && this.hourseAnimation.skeleton != null)
            {
                foreach (Slot slot in this.hourseAnimation.skeleton.Slots)
                {
                    ttt.a = slot.a;
                    slot.SetColor(ttt);
                }
            }
            if(UpdateTimeTool.Instance.IsAddTimer(this.ReSetColor))
            {
                UpdateTimeTool.Instance.RemoveTimer(this.ReSetColor);
            }
            UpdateTimeTool.Instance.AddTimer(time, false, this.ReSetColor);
        }
        public void ReSetColor()
        {
            Color ttt = new Color(currentColor.r, currentColor.g, currentColor.b);
            if (this.skeletonAnimation != null)
            {
                foreach (Slot slot in this.skeletonAnimation.skeleton.Slots)
                {
                    if (slot.a == 0)
                        continue;
                    ttt.a = slot.a;
                    slot.SetColor(ttt);
                }
            }
            if (this.hourseAnimation != null)
            {
                foreach (Slot slot in this.hourseAnimation.skeleton.Slots)
                {
                    ttt.a = slot.a;
                    slot.SetColor(ttt);
                }
            }

        }
        /// <summary>
        /// 渐渐消失
        /// </summary>
        /// <param name="time">几秒内消失</param>
        public void FadeOut(float time)
        {
            if (UpdateTimeTool.Instance.IsAddTimer(this.FadeOutUpDate))
            {
                return;
            }
            ResetAlph(1.0f);
            UpdateTimeTool.Instance.AddTimer(time / 10.0f, true, this.FadeOutUpDate);
            this.arf = 1.0f;
        }
        /// <summary>
        /// 渐渐进入
        /// </summary>
        /// <param name="time">进入时间</param>
        public void FadeIn(float time)
        {
            if (UpdateTimeTool.Instance.IsAddTimer(this.FadeInUpDate))
                return;
            ResetAlph(0.0f);
            UpdateTimeTool.Instance.AddTimer(time / 10.0f, true, this.FadeInUpDate);

        }
        private void FadeInUpDate()
        {
            float a = 0;
            if (this.skeletonAnimation != null)
            {
                foreach (Slot slot in this.skeletonAnimation.skeleton.Slots)
                {
                    if (slot.data.name == "weapon1" || slot.data.name == "weapon1_death")
                    {
                        if(slot.a > 0)
                        continue;
                    }
                    slot.a += (1.0f / 10);
                    a = slot.a;
                }
            }
            if (this.hourseAnimation != null)
            {
                foreach (Slot slot in this.hourseAnimation.skeleton.Slots)
                {
                    slot.a += (1.0f / 10);
                }
            }
            if (a >= 1.0f)
                UpdateTimeTool.Instance.RemoveTimer(this.FadeInUpDate);
        }
        protected virtual void FadeOutUpDate()
        {
            float a = 0;
            if (this.skeletonAnimation != null)
            {
                foreach (Slot slot in this.skeletonAnimation.skeleton.Slots)
                {
                    if (slot.data.name == "weapon1" || slot.data.name == "weapon1_death")
                    {
                        if (slot.a <= 0)
                            continue;
                    }
                    slot.a -= ((float)this.arf / 10);
                    a = slot.a;
                }
            }
            if (this.hourseAnimation != null)
            {
                foreach (Slot slot in this.hourseAnimation.skeleton.Slots)
                {
                    slot.a -= ((float)this.arf / 10);
                }
            }
            if (a <= 0)
                UpdateTimeTool.Instance.RemoveTimer(this.FadeOutUpDate);
        }
        /// <summary>
        /// 设置Alph值
        /// </summary>
        /// <param name="num">需要显示的Alph数值</param>
        public void ResetAlph(float num)
        {
            if(this.skeletonAnimation != null)
            {
                foreach (Slot slot in this.skeletonAnimation.skeleton.Slots)
                {
                    if (slot.data.name == "weapon1" || slot.data.name == "weapon1_death")
                    {
                        if (slot.a <= 0)
                            continue;
                    }
                    slot.a = num;
                    
                }
            }
            if (this.hourseAnimation != null)
            {
                foreach (Slot slot in this.hourseAnimation.skeleton.Slots)
                {
                    slot.a = num;
                }
            }
        }
        /// <summary>
        /// 获取最大层级
        /// </summary>
        /// <returns></returns>
        public virtual int GetMaxSort()
        {
            if (this.skeletonAnimation == null)
                return 0;
            return this.skeletonAnimation.renderer.sortingOrder;
        }
        /// <summary>
        /// 获取最小层级
        /// </summary>
        /// <returns></returns>
        public int GetMinSort()
        {
            if (this.hourseAnimation == null)
                return GetMaxSort();
            else
            {
                return this.hourseAnimation.renderer.sortingOrder;
            }
        }
        public Vector3 GetStartSize()
        {
            if (this.size.Equals(Vector3.zero))
            {
                if (this.skeletonAnimation != null && this.skeletonAnimation.renderer != null && this.skeletonAnimation.renderer.bounds != null)
                {
                    this.size = this.skeletonAnimation.renderer.bounds.size;
                }
                if (!isHadHourse)
                {
                    if (this.hourseAnimation != null && this.hourseAnimation.renderer != null && this.hourseAnimation.renderer.bounds != null)
                    {
                        isHadHourse = true;
                        this.size.x = this.size.x > this.hourseAnimation.renderer.bounds.size.x ? this.size.x : this.hourseAnimation.renderer.bounds.size.x;
                        this.size.y += this.hourseAnimation.renderer.bounds.size.y;
                        this.size.y *= 0.8f;
                    }
                }
            }
            return this.size;
        }
        private Vector3 GetSize()
        {
            Vector3 size = new Vector3();
            size = Vector3.zero;
            if(this.skeletonAnimation != null)
            {
                size = this.skeletonAnimation.renderer.bounds.size;
            }
            if(this.hourseAnimation != null)
            {
                isHadHourse = true;
                size += this.hourseAnimation.renderer.bounds.size;
                size.y *= 0.8f;
            }
            return size;
        }
        private void SetSlotColor()
        {
            
        }
        private void WeaponSlotChange(bool weapon1,bool weapon_death)
        {
            if (this.skeletonAnimation == null)
                return;
            if (this.skeletonAnimation.skeleton == null)
                return;
            Slot slot1 = this.skeletonAnimation.skeleton.FindSlot("weapon1");
            Slot slot2 = this.skeletonAnimation.skeleton.FindSlot("weapon1_death");
            if (slot1 != null && slot2 != null)
            {
                if (weapon1)
                    slot1.a = 1.0f;
                else
                    slot1.a = 0.0f;
                if (weapon_death)
                    slot2.a = 1.0f;
                else
                    slot2.a = 0.0f;
            }

            Transform delet = this.transform.FindChild("EquipEffectName");

            if (delet != null)
            {
                BoneFollower follower = delet.gameObject.GetComponent<BoneFollower>();
                if(follower != null)
                {
                    string BoneName = "";
                    if (weapon1)
                        BoneName = "bone_weapon1";
                    else
                        BoneName = "bone_weapon1_death";
                    Bone followBone = follower.skeletonRenderer.skeleton.FindBone(BoneName);

                    if (followBone == null)
                        return;
                    follower.boneName = BoneName;
                    follower.Reset();
                }
            }

        }

        /// <summary>
        /// 检测动画是否存在
        /// </summary>
        /// <param name="vName"></param>
        /// <returns></returns>
        public bool CheckIsHaveAnimation(string vName)
        {
            if (skeletonAnimation == null)
                return false;
            if (skeletonAnimation.state == null)
                return false;
            if (skeletonAnimation.state.Data == null)
                return false;
            if (skeletonAnimation.state.Data.skeletonData == null)
                return false;
            Spine.Animation tmp = skeletonAnimation.state.Data.skeletonData.FindAnimation(vName);
            if (tmp == null)
                return false;
            else
                return true;
        }
        public bool CheckIsHourseHaveAnimation(string vName)
        {
            if (hourseAnimation == null)
                return true;
            if (hourseAnimation.state == null)
                return true;
            if (hourseAnimation.state.Data == null)
                return true;
            if (hourseAnimation.state.Data.skeletonData == null)
                return true;
            Spine.Animation tmp = hourseAnimation.state.Data.skeletonData.FindAnimation(vName);
            if (tmp == null)
                return false;
            else
                return true;
        }
    }
};