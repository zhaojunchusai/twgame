using UnityEngine;
using System.Collections;

public class ShowUIAniAlpha : ShowUIAnimation
{
    public float m_from = 0f; //alpha值，取值0-1//
    public float m_to = 1f; //alpha值，取值0-1//

    public override void StartOpenAnimation()
    {
        base.StartOpenAnimation();
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", m_from,
            "to", m_to,
            "time", m_animationTime,
            "easeType", m_openEaseType.ToString(),
            "delay", m_delay, 
            "onupdate", "ValueUpdate",
            "oncomplete", "OnOpenAnimationCompleted"
            ));
    }

    public override void StartCloseAnimation()
    {
        base.StartCloseAnimation();
        iTween.ValueTo(gameObject, iTween.Hash(
            "from",m_to ,
            "to", m_from,
            "time", m_animationTime,
            "easeType", m_closeEaseType.ToString(),
            "delay", m_delay,
            "onupdate", "ValueUpdate",
            "oncomplete", "CloseAnimationCompleted"
            ));
    }

    public void ValueUpdate(float value )
    {
        //Color color = new Color(255,255,255,value);
        //CommonFunction.ChangeObjColor(color,gameObject);
        CommonFunction.ChangeObjAlpha(value,gameObject);
    }
}
