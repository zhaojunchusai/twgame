using UnityEngine;
using System.Collections;

public class AotoPosition : MonoBehaviour
{
    public enum Direction
    {
        Horizontal,
        Vertical
    }

    public Direction dir = Direction.Vertical;
    public float time = 0.5f;
    public float distance = 20f;

	void Start ()
	{
	    Vector3 pos;
        if (dir == Direction.Vertical)
        {
            pos = new Vector3(0, distance, 0);
        }
        else
        {
            pos = new Vector3(distance,0 , 0);
        }
	    iTween.MoveFrom(gameObject,iTween.Hash(
            "position", pos,
            "loopType", "pingpong",
            "islocal",true,
            "time", time,
            "easeType", "linear"
            ));
	}
	
}
