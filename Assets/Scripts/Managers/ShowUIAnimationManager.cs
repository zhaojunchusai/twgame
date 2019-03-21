using UnityEngine;
using System.Collections;

public class ShowUIAnimationManager : MonoSingleton<ShowUIAnimationManager>
{

    public void SetShowParameters(GameObject ui, EShowUIType type, UIBoundary boundary, ShowUIAnimation.ShowAnimationCompletedHandler OnShowAnimationCompleted)
    {
        SetAnimationParas(ui, type, boundary, true, OnShowAnimationCompleted,null);
    }

    public void SetCloseParameters(GameObject ui, EShowUIType type, ShowUIAnimation.CloseAnimationCompletedHandler OnCloseAnimationCompleted)
    {
        SetAnimationParas(ui, type, UIBoundary.zero, false, null, OnCloseAnimationCompleted);
    }

    private void SetAnimationParas(GameObject ui, EShowUIType type, UIBoundary boundary, bool isOpen, 
        ShowUIAnimation.ShowAnimationCompletedHandler OnShowAnimationCompleted, 
        ShowUIAnimation.CloseAnimationCompletedHandler OnCloseAnimationCompleted)
    {
        
        switch (type)
        {
            case EShowUIType.Default:
                {
                    CheckDefaultAnimation(ui, isOpen, OnShowAnimationCompleted, OnCloseAnimationCompleted);
                    break;
                }

        }
    }

    public bool HasCloseAnimation(GameObject ui)
    {
        ShowUIAnimation[] animations = ui.GetComponents<ShowUIAnimation>();
        for (int i = 0; i < animations.Length; i++)
        {
            if (animations[i].m_closePlay)
            {
                return true;
            }
        }

        return false;
    }

    private void CheckDefaultAnimation(GameObject ui, bool isOpen, 
        ShowUIAnimation.ShowAnimationCompletedHandler OnShowAnimationCompleted, 
        ShowUIAnimation.CloseAnimationCompletedHandler OnCloseAnimationCompleted)
    {
 
        ShowUIAnimation[] animations = ui.GetComponents<ShowUIAnimation>();
        
        if (animations.Length <= 0)
            return;

        animations[0].OnShowAnimationCompleted = OnShowAnimationCompleted;
        animations[0].OnCloseAnimationCompleted = OnCloseAnimationCompleted;

        int closeplaycount = 0;

        for (int i = 0; i < animations.Length; i++)
        {
            if (!animations[i].m_nativeLink)
                continue;
            
            if(isOpen)
            {
                if (animations[i].m_openPlay)
                {
                    animations[i].StartOpenAnimation();
                }
            }
            else
            {
                if (animations[i].m_closePlay)
                {
                    closeplaycount++;
                    animations[i].StartCloseAnimation();
                }
            }
           
        }

        if (!isOpen && closeplaycount == 0)
        {
            ui.SetActive(false);
        }

    }
    


}
