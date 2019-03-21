using UnityEngine;
using System.Collections;

public class RotateGameobject : MonoBehaviour
{
    public GameObject vgo;
    public float rotateSpeed;
    public Vector3 rotateAngle;
	void Update () 
    {
        vgo.transform.Rotate(rotateAngle * Time.deltaTime * rotateSpeed);
	}

}
