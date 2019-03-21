using UnityEngine;
using System.Collections;

public class ShowUIAniScale : ShowUIAnimation
{
    public Vector3 m_from = Vector3.one;
    private Vector3 m_to;
    
    public override void StartOpenAnimation()
    {
        base.StartOpenAnimation();
        m_to = transform.localScale; 

        iTween.ScaleFrom(gameObject, iTween.Hash(
            "scale", m_from,
            "islocal", _isLocal,
            "movetopath", _moveToPath,
            "time", m_animationTime,
            "easeType", m_openEaseType.ToString(),
            "loopType", m_loopType.ToString(),
            "delay", m_delay,
            "oncomplete", "OnOpenAnimationCompleted"
            ));
    }

    public override void StartCloseAnimation()
    {
        base.StartCloseAnimation();
        iTween.ScaleTo(gameObject, iTween.Hash(
            "scale", m_from,
            "islocal", _isLocal,
            "movetopath", _moveToPath,
            "time", m_animationTime,
            "easeType", m_closeEaseType.ToString(),
            "loopType", m_loopType.ToString(),
            "delay", m_delay,
            "oncomplete", "CloseAnimationCompleted"
            ));
    }

    public override void CloseAnimationCompleted()
    {
        base.CloseAnimationCompleted();
        transform.localScale = m_to;
    }
}
