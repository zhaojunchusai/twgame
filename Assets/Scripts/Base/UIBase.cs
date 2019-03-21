using UnityEngine;
using System.Collections;

public class UIBase :IUIInterface
{	
	private UISystem mUISystem;	
  	public UISystem tUISystem
	{
		get
		{
            if (mUISystem == null)
                mUISystem = UISystem.Instance;
			return mUISystem;
		}
		set{mUISystem = value;}
	}	
	
	public string UIName;
	
	public virtual void Create()
	{					
			
	}	
	
	public virtual void Init()//UI初始化
	{
			
	}
	
	public virtual void Open(object Sender, object e)
	{
        tUISystem.ShowGameUI(UIName);					
	}	
	
	public virtual void Close(object Sender, object e)
	{
        tUISystem.CloseGameUI(UIName);
	}	
	
	public virtual void Destroy()
	{
		
	}

    public virtual UIBoundary GetUIBoundary()
    {
        UIBoundary boundary = new UIBoundary();
        return boundary;
    }

    public virtual void Initialize() 
    {
      
    }

    public virtual void ShowAnimation(GameObject page,EShowUIType type)
    {
        ShowUIAnimationManager.Instance.SetShowParameters(page, type, GetUIBoundary(), ShowAnimationDown);
    }

    public virtual void ShowAnimationDown()
    {

    }

    public virtual void ReturnTop() 
    {

    }

    public virtual void CloseAnimation(GameObject page, EShowUIType type)
    {
        ShowUIAnimationManager.Instance.SetCloseParameters(page, type, CloseAnimationDown);
    }

    public virtual void CloseAnimationDown()
    {
        Close(null,null);
    }

    public virtual void Uninitialize()
    {

    }

}
