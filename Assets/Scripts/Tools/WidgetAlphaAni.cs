using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIWidget))]
public class WidgetAlphaAni : MonoBehaviour
{
    [Range(0, 1)]public float From;
    [Range(0, 1)]public float To;
    public float Duration = 1;
    public EventDelegate.Callback OnFinishEvent;
    private bool _start = false;
    private float _original = 0;
    private float _destination = 0;
    private float _factor = 0;
    private UIWidget _widget;

    void Update()
    {
        if(!_start)
            return;

        _factor += Time.deltaTime;

        if(_factor < Duration)
        {
            _widget.alpha = _original + _factor/Duration*(_destination - _original);
        }
        else
        {
            _widget.alpha = _destination;
            _start = false;
            if (OnFinishEvent != null)
                OnFinishEvent();
        }
    }

    public void PlayAni(bool forward)
    {
        if(forward)
        {
            _original = From;
            _destination = To;
        }
        else
        {
            _original = To;
            _destination = From;
        }

        if (_widget == null)
            _widget = GetComponent<UIWidget>();
        _factor = 0;
        _start = true;
        _widget.alpha = _original;
    }
}
