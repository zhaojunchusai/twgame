
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy: MonoBehaviour
{
    public float lifeTime = 3f;

    void Start()
    {
        Invoke("DestroySelf", lifeTime);
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    void OnDisable() 
    {
        if (gameObject != null)
        {
            Destroy(this.gameObject);
        }
    } 

}
