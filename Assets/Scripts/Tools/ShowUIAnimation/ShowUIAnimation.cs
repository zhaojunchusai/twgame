using UnityEngine;
using System.Collections;

public abstract class ShowUIAnimation : MonoBehaviour
{
    public bool m_enablePlay = false;
    public bool m_nativeLink = false;
    public bool m_openPlay = true;
    public bool m_closePlay = true;
    public float m_animationTime = 1f;
    public float m_delay = 0f;
    public iTween.LoopType m_loopType = iTween.LoopType.none;
    public iTween.EaseType m_openEaseType = iTween.EaseType.easeOutBack;
    public iTween.EaseType m_closeEaseType = iTween.EaseType.easeInBack;

    protected const bool _moveToPath = false;
    protected const bool _isLocal = true;

    public delegate void ShowAnimationCompletedHandler();
    public delegate void CloseAnimationCompletedHandler();

    public ShowAnimationCompletedHandler OnShowAnimationCompleted;
    public CloseAnimationCompletedHandler OnCloseAnimationCompleted;

    public virtual void OnEnable()
    {
        if (m_enablePlay)
        {
            StartOpenAnimation();
        }
    }

    //public virtual void OnDisable()
    //{
    //    if (m_nativeLink && m_closePlay)
    //    {
    //        StartCloseAnimation();
    //    }
    //}

    public virtual void StartOpenAnimation()
    {
        //if(!m_openPlay)
        //    return;
    }

    public virtual void StartCloseAnimation()
    {
        //if (!m_closePlay)
        //{
        //    gameObject.SetActive(false);
        //    return;
        //}
        
    }

    public virtual void OnOpenAnimationCompleted()
    {

        if (OnShowAnimationCompleted != null)
        {
            OnShowAnimationCompleted();
        }
    }

    public virtual void CloseAnimationCompleted()
    {

        if (OnCloseAnimationCompleted != null)
        {
            OnCloseAnimationCompleted();
        }
    }

    public virtual void CloseUIWhenAnimationDone()
    {

        iTween[] itween = GetComponents<iTween>();
        //gameObject.SetActive(false);
        if (itween != null)
        {
            if (OnCloseAnimationCompleted != null)
            {
                OnCloseAnimationCompleted();
            }
            for (int i = 0; i < itween.Length; i++)
            {
                Destroy(itween[i]);
            }
        }
    }

    public virtual void StopAnimation()
    {
        iTween.Stop(gameObject);
    }
}
