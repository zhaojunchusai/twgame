using UnityEngine;
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	private static T m_Instance = null;
	public static T Instance
	{
		get
		{
			// Instance requiered for the first time, we look for it
			if( m_Instance == null )
			{
				m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;
				
				// Object not found, we create a temporary one
				if( m_Instance == null )
				{
                    if (GameObject.Find("Main") != null)
                    {
                        m_Instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                        m_Instance.transform.parent = GameObject.Find("Main").transform;
                    }
				}
			}
			return m_Instance;
		}
	}
	// This function is called when the instance is used the first time
	// Put all the initializations you need here, as you would do in Awake
	public virtual void Init(){}
	
	// Make sure the instance isn't referenced anymore when the user quit, just in case.
	private void OnApplicationQuit()
	{
		m_Instance = null;
	}
}