/*
 16/03/11 朱华茂添加lateupdate
 
 
 */

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Common
{
    public sealed class Scheduler{

        public static readonly Scheduler Instance = new Scheduler();

        public delegate void OnScheduler();

        public delegate void ArgCallback(object args);


        private class FrameScheduler
        {
            public uint ID = 0;
            public uint Frame = 0;
            public uint RealFrame = 0;
            public bool IsLoop = false;
            public OnScheduler Callback = null;
            public ArgCallback ArgFunction= null;
            public object Arguments =  null;
            public void Execute() 
            {
                if (Callback != null)
                {
                    Callback();
                }
                if (ArgFunction != null)
                {
                    ArgFunction(Arguments);
                }
            }
        }
        private List<FrameScheduler> _frameDelegates;

        private class TimeScheduler
        {
            public uint ID =0;
            public float RealTime = 0.0F;
            public float Time=0.0f;
            public bool IsLoop =false;
            public OnScheduler Callback = null;
            public ArgCallback ArgFunction = null;
            public object Arguments = null;
            public void Execute()
            {
                if (Callback != null)
                {
                    Callback();
                }
                if (ArgFunction != null)
                {
                    ArgFunction(Arguments);
                }
            }
        }
		private List<TimeScheduler> _timeSchedulers;

        private List<OnScheduler> _updateScheduler;

        private List<OnScheduler> _lateUpdateScheduler;

        private List<TimeScheduler> tmpList;
        private uint _curFrame;

        private uint _curAllotID;

        /// <summary>
        /// 构造
        /// </summary>
        private Scheduler()
        {
            _curFrame = 0;
            _curAllotID = 0;
            _frameDelegates = new List<FrameScheduler>();
            _timeSchedulers = new List<TimeScheduler>();
			_updateScheduler = new List<OnScheduler> ();
            _lateUpdateScheduler = new List<OnScheduler>();
        }

        ~Scheduler()
        {
            _frameDelegates.Clear();
            _frameDelegates = null;
            _timeSchedulers.Clear();
            _timeSchedulers = null;
			_updateScheduler.Clear();
			_updateScheduler = null;
            _lateUpdateScheduler.Clear();
            _lateUpdateScheduler = null;
        }
        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            ++_curFrame;
            UpdateFrameScheduler();
            UpdateTimeScheduler();
			UpdateUpdator();
        }

        public void LateUpdate()
        {
            LateUpdater();
        }

        private void UpdateUpdator()
		{
			if(_updateScheduler.Count <= 0)
			{
				return;
			}

			for (var i=0; i < _updateScheduler.Count; ++i) 
			{
				_updateScheduler[i]();
			}
		}

        private void UpdateFrameScheduler()
        {
			for (var i=0; i<_frameDelegates.Count; ) 
			{
                FrameScheduler obj = _frameDelegates[i];
				if (obj.RealFrame <= _curFrame)
				{
					obj.Execute();
					if (obj.IsLoop)
					{
						obj.RealFrame += obj.Frame;
					}
					else
					{
						_frameDelegates.RemoveAt(i);
						continue;
					}
				}
				++i;
			}
        }

        private void UpdateTimeScheduler()
		{
            if (tmpList == null)
                tmpList = new List<TimeScheduler>();
            tmpList.Clear();
            for (int i = 0; i < _timeSchedulers.Count; i++)
            {
                TimeScheduler tmpObj = _timeSchedulers[i];
                if (tmpObj.RealTime > Time.time)
                    continue;
                tmpObj.Execute();
                if (tmpObj.IsLoop)
                    tmpObj.RealTime += tmpObj.Time;
                else
                    tmpList.Add(tmpObj);
            }
            for (int i = 0; i < tmpList.Count; i++)
            {
                _timeSchedulers.Remove(tmpList[i]);
            }
            return;
            for (var i = 0; i < _timeSchedulers.Count; )
            {
                TimeScheduler obj = _timeSchedulers[i];
                if (obj.RealTime <= Time.time)
                {
                    obj.Callback();
                    if (obj.IsLoop)
                    {
                        obj.RealTime += obj.Time;
                    }
                    else
                    {
                        if (i < _timeSchedulers.Count)
                            _timeSchedulers.RemoveAt(i);
                        continue;
                    }
                }
                ++i;
            }

        }

        public void AddFrame(uint frame, bool loop, OnScheduler callback = null,ArgCallback argFunction = null,object arguments = null)
        {
            ++_curAllotID;
            var frameScheduler = new FrameScheduler
            {
                ID = _curAllotID,
                Frame = frame,
                RealFrame = frame + _curFrame,
                IsLoop = loop,
                Callback = callback,
                ArgFunction = argFunction,
                Arguments = arguments
            };
            _frameDelegates.Add(frameScheduler);
        }

		public void RemoveFrame(OnScheduler callback)
        {
			for (var i=0; i<_frameDelegates.Count; ++i) 
			{
				var deleData = _frameDelegates[i];
				if(deleData.Callback == callback)
				{
					_frameDelegates.RemoveAt(i);
					break;
				}
			}
        }

        public void RemoveFrame(ArgCallback argFunction)
        {
            for (var i = 0; i < _frameDelegates.Count; ++i)
            {
                var deleData = _frameDelegates[i];
                if (deleData.ArgFunction == argFunction)
                {
                    _frameDelegates.RemoveAt(i);
                    break;
                }
            }
        }

        public void AddTimer(float time, bool loop, OnScheduler callback = null,ArgCallback argFunction = null,object arguments = null)
        {
            if (IsAddTimer(time, loop, callback, argFunction, arguments))
            {
                return;
            }
            ++_curAllotID;
            var timeScheduler = new TimeScheduler
            {
                ID = _curAllotID,
                Time = time,
                RealTime = time + Time.time,
                IsLoop = loop,
                Callback = callback,
                ArgFunction = argFunction,
                Arguments = arguments
                 
            };
            _timeSchedulers.Add(timeScheduler);
        }


        public bool IsAddTimer(float time, bool loop, OnScheduler callback, ArgCallback argFunction, object arguments) 
        {
            for (var i = 0; i < _timeSchedulers.Count; ++i)
            {
                var deleData = _timeSchedulers[i];
                if (deleData.Callback == callback && time == deleData.Time && loop == deleData.IsLoop && deleData.ArgFunction == argFunction && deleData.Arguments == arguments)
                {
                    return true;
                }
            }
            return false;
        }

		public void RemoveTimer(OnScheduler callback)
        {
            if (callback == null) return;
			for (var i=0; i<_timeSchedulers.Count; ++i) 
			{
				var deleData = _timeSchedulers[i];
				if(deleData.Callback == callback)
				{
					_timeSchedulers.RemoveAt(i);
					break;
				}
			}
        }

        public void RemoveTimer(ArgCallback argFunction) 
        {
            if (argFunction == null) return;
            for (var i = 0; i < _timeSchedulers.Count; ++i)
            {
                var deleData = _timeSchedulers[i];
                if (deleData.Arguments == argFunction)
                {
                    _timeSchedulers.RemoveAt(i);
                    break;
                }
            }
        }

		public void AddUpdator(OnScheduler callback)
		{
            if (callback == null)
                return;
            if (_updateScheduler.Contains(callback))
                return;
			_updateScheduler.Add(callback);
		}

		public void RemoveUpdator(OnScheduler callback)
		{
            if (callback == null)
                return;
            if (!_updateScheduler.Contains(callback))
                return;
			_updateScheduler.Remove(callback);
		}

        void LateUpdater()
        {
            if (_lateUpdateScheduler.Count <= 0)
            {
                return;
            }

            for (var i = 0; i < _lateUpdateScheduler.Count; ++i)
            {
                _lateUpdateScheduler[i]();
            }
        }

        public void AddLateUpdator(OnScheduler callback)
        {
            if (callback == null)
                return;
            if (_lateUpdateScheduler.Contains(callback))
                return;
            _lateUpdateScheduler.Add(callback);
        }

        public void RemoveLateUpdator(OnScheduler callback)
        {
            if (callback == null)
                return;
            if (!_lateUpdateScheduler.Contains(callback))
                return;
            _lateUpdateScheduler.Remove(callback);
        }
    }
    
}
