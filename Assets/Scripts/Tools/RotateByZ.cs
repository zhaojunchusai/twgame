using UnityEngine;
using System.Collections;

public class RotateByZ : MonoBehaviour
{
    public Transform mTrans;

    public float speed = 10;
    public bool forward = true;

    void Awake() {

        mTrans = this.transform;
    }

    void Update()
    {
        if (forward)
           transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * speed);
        else
        {
            transform.RotateAround(transform.position, Vector3.back, Time.deltaTime * speed);
        }
    }
}
