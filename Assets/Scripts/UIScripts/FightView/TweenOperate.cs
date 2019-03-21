using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

public class TweenerRunInfos
{
    public float Duration;
    public float Delay;

    public TweenerRunInfos(float vDuration, float vDelay)
    {
        Duration = vDuration;
        Delay = vDelay;
    }
}
public class TweenOperate : MonoBehaviour {

    private TweenAlpha ta;
    private TweenScale ts;
    private TweenPosition tp;
    private TweenRotation tr;
    private TweenColor tc;

    //private Dictionary<UITweener, string> dicTween = new Dictionary<UITweener, string>();
    private Dictionary<UITweener, TweenerRunInfos> dicTween = new Dictionary<UITweener, TweenerRunInfos>();

	// Use this for initialization
	void Start () {
        SetComponent();
	}

    void OnEnable()
    {
        ReSet();
    }

    private void SetComponent()
    {
        ta = this.GetComponent<TweenAlpha>();
        ts = this.GetComponent<TweenScale>();
        tp = this.GetComponent<TweenPosition>();
        tr = this.GetComponent<TweenRotation>();
        tc = this.GetComponent<TweenColor>();

        //if (ta != null)
        //{
        //    dicTween.Add(ta, string.Format("{0}|{1}", ta.duration, ta.delay));
        //}
        //if (ts != null)
        //{
        //    dicTween.Add(ts, string.Format("{0}|{1}", ts.duration, ts.delay));
        //}
        //if (tp != null)
        //{
        //    dicTween.Add(tp, string.Format("{0}|{1}", tp.duration, tp.delay));
        //}
        //if (tr != null)
        //{
        //    dicTween.Add(tr, string.Format("{0}|{1}", tr.duration, tr.delay));
        //}
        //if (tc != null)
        //{
        //    dicTween.Add(tc, string.Format("{0}|{1}", tc.duration, tc.delay));
        //}
        if (ta != null)
        {
            dicTween.Add(ta, new TweenerRunInfos(ta.duration, ta.delay));
        }
        if (ts != null)
        {
            dicTween.Add(ts, new TweenerRunInfos(ts.duration, ts.delay));
        }
        if (tp != null)
        {
            dicTween.Add(tp, new TweenerRunInfos(tp.duration, tp.delay));
        }
        if (tr != null)
        {
            dicTween.Add(tr, new TweenerRunInfos(tr.duration, tr.delay));
        }
        if (tc != null)
        {
            dicTween.Add(tc, new TweenerRunInfos(tc.duration, tc.delay));
        }
    }

    private IEnumerator DelayOperate(UITweener vTween, float vDuration, float vDelay)
    {
        yield return new WaitForSeconds(0.5f);
        if (vTween != null)
        {
            vTween.duration = vDuration;
            vTween.delay = vDelay;
        }
    }

    public void Stop()
    {
        if (dicTween != null)
        {
            //foreach (KeyValuePair<UITweener, string> tmpSingle in dicTween)
            //{
            //    if ((tmpSingle.Key != null) && (tmpSingle.Key.gameObject.active == true) && (tmpSingle.Key.enabled == true) && (!string.IsNullOrEmpty(tmpSingle.Value)))
            //    {
            //        float tmpDuration = 0, tmpDelay = 0;

            //        tmpSingle.Key.duration = 0;
            //        tmpSingle.Key.delay = 0;
            //        tmpSingle.Key.ResetToBeginning();
            //        tmpSingle.Key.PlayForward();

            //        string[] tmpArr = tmpSingle.Value.Split('|');
            //        if (tmpArr.Length >= 2)
            //        {
            //            if (float.TryParse(tmpArr[0], out tmpDuration) && float.TryParse(tmpArr[1], out tmpDelay))
            //            {
            //                StartCoroutine(DelayOperate(tmpSingle.Key, tmpDuration, tmpDelay));
            //            }
            //        }
            //    }
            //}

            foreach (KeyValuePair<UITweener, TweenerRunInfos> tmpSingle in dicTween)
            {
                if ((tmpSingle.Key != null) && (tmpSingle.Key.gameObject.active == true) && (tmpSingle.Key.enabled == true) && (tmpSingle.Value != null))
                {
                    tmpSingle.Key.duration = 0;
                    tmpSingle.Key.delay = 0;
                    tmpSingle.Key.ResetToBeginning();
                    tmpSingle.Key.PlayForward();
                    //StartCoroutine(DelayOperate(tmpSingle.Key, tmpSingle.Value.Duration, tmpSingle.Value.Delay));
                }
            }
        }

        Scheduler.Instance.AddTimer(0.2f, false, ReSet);
    }

    public void ReSet()
    {
        if (dicTween != null)
        {
            foreach (KeyValuePair<UITweener, TweenerRunInfos> tmpSingle in dicTween)
            {
                if ((tmpSingle.Key != null) && (tmpSingle.Value != null))
                {
                    tmpSingle.Key.duration = tmpSingle.Value.Duration;
                    tmpSingle.Key.delay = tmpSingle.Value.Delay;
                }
            }
        }
    }

}
