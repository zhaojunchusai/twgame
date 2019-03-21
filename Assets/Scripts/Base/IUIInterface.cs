using UnityEngine;
using System.Collections;

public interface IUIInterface
{
    
	UISystem tUISystem
	{
		set;
		get;
	}
	void Create();
	
	void Init();

    void ReturnTop();
	
	void Open(object Sender, object e);	
	
	void Close(object Sender, object e);
		
	void Destroy();			

    void Initialize();

    void Uninitialize();

    
}
