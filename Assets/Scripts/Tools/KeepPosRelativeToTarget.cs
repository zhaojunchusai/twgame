using UnityEngine;
using System.Collections;

public class KeepPosRelativeToTarget : MonoBehaviour
{

    public Transform target;
    public float factor = 1;

	void Update () {
	    if(target)
	    {
	        transform.position = target.position*factor;
	    }
	}
}
