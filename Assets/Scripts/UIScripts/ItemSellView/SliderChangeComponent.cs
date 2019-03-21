using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(UISlider))]
public class SliderChangeComponent : MonoBehaviour
{
    public delegate void SliderChangeDelegate();

    public SliderChangeDelegate OnSliderChange;

    private void Awake() 
    {
        UISlider slider = this.GetComponent<UISlider>();
        if (slider == null) 
        {
            slider = this.gameObject.AddComponent<UISlider>();
        }
        EventDelegate eventDelegate = new EventDelegate(this, "SliderChange");  //检测slider的值变化
        slider.onChange.Add(eventDelegate);
    }

    public void SliderChange() 
    {
        if (OnSliderChange != null) 
        {
            OnSliderChange();
        }
    }

}

