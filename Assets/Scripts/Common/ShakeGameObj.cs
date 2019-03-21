using UnityEngine;
using System.Collections;

public class ShakeGameObj : MonoBehaviour
{
    public Vector3 shakeVector = new Vector3(0.05f, 0.05f, 0f);
    public float shakeTime = 0.5f;
    public bool startShake = false;
    private bool _isShaking = false;
    private Vector3 _lastPos = Vector3.zero;
    public void Awake()
    {
       
    }

    public void Update()
    {
        if (startShake)
        {
            startShake = false;
           
            Shake(shakeTime);
        }
    }

    public void Shake(float time)
    {
        if (_isShaking)
            return;
        _isShaking = true;
        shakeTime = time;
        _lastPos = transform.localPosition;
        iTween.ShakePosition(this.gameObject, shakeVector, shakeTime);
        Invoke("Reset", shakeTime + 0.2f);
    }

    public void Stop(Vector3 wantedPos)
    {
        _lastPos = wantedPos;
        if (_isShaking)
        {
            _isShaking = false;
            CancelInvoke("Reset");
        }
        iTween.Stop(this.gameObject);
        this.transform.localPosition = _lastPos;
    }

    public void Stop()
    {
        if (_isShaking)
        {
            _isShaking = false;
            CancelInvoke("Reset");
        }
        iTween.Stop(this.gameObject);
        this.transform.localPosition = _lastPos;
    }

    private void Reset()
    {
        _isShaking = false;
        iTween.Stop(this.gameObject);
        this.transform.localPosition = _lastPos;
    }
}
