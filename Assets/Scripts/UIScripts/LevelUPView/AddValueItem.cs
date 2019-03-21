using UnityEngine;
using System.Collections;
[RequireComponent(typeof(UILabel))]
//数字增加
public class AddValueItem : MonoBehaviour
{
    public float Speed = 1;      //value / framea 
    public float MaxTime = 3f;   // seconds
    public UILabel Lbl_ValueLabel;
    public float curAdditionValue = 0f;
    public float totalAdditionValue = 0f;
    public bool isUpdateing = false;

    private float _realSpeed = 0f;
    private int _oValue = 0;

    void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        Lbl_ValueLabel = this.GetComponent<UILabel>();
        _oValue = 0;
    }
    void Start() {
        isUpdateing = false;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    int valueT = UnityEngine.Random.Range(10, 100);
        //    ShowAddValue(valueT);
        //    Debug.Log(valueT + " Start =================== Time "+ Time.time);
        //}
        if (isUpdateing)
        {
            if (Mathf.Abs(curAdditionValue) < Mathf.Abs(totalAdditionValue))
                curAdditionValue += _realSpeed;
            else
            {
                isUpdateing = false;
                if (curAdditionValue != totalAdditionValue)
                    curAdditionValue = totalAdditionValue;
            }
            UpdateLabel();
        }
        
    }

    public void ShowAddValue(int newValue, int oldValue)
    {
        Debug.Log("Old " + oldValue + " New  " + newValue);
        if (isUpdateing)
        {
            isUpdateing = false;
            _oValue =(int)(_oValue + totalAdditionValue);
            totalAdditionValue = 0f;
            curAdditionValue = 0f;
            UpdateLabel();
        }
        _oValue = oldValue;
        totalAdditionValue = (float)(newValue - oldValue);
        float maxValue = MaxTime / Time.deltaTime * Speed;
        if (maxValue < Mathf.Abs(totalAdditionValue))
        {
            int toatalFrame = (int)(MaxTime / Time.deltaTime);
            _realSpeed = totalAdditionValue / toatalFrame;
        }
        else
        {
            if (totalAdditionValue > 0)
                _realSpeed = Speed;
            else
                _realSpeed = -Speed;
        }
        curAdditionValue = 0f;
        isUpdateing = true;
    }

    private void UpdateLabel()
    {
        Lbl_ValueLabel.text = CommonFunction.GetTenThousandUnit(Mathf.Max(0,_oValue + (int)curAdditionValue));
    }
}
