using UnityEngine;
using System.Collections;
using fogs.proto.msg;

public class ActivityTextDescItem : MonoBehaviour
{
    [HideInInspector]public UILabel Lbl_TitleLabel;
    [HideInInspector]public UILabel Lbl_DescLabel;
    private bool _isInit = false;

    public void Initialize()
    {
        if (_isInit)
            return;
        _isInit = true;

        Lbl_TitleLabel = transform.FindChild("TitleLabel").gameObject.GetComponent<UILabel>();
        Lbl_DescLabel = transform.FindChild("DescLabel").gameObject.GetComponent<UILabel>();
        //SetLabelValues();
    }

    void Awake()
    {
        Initialize(); 
    }

    public void SetLabelValues()
    {
        Lbl_TitleLabel.text = "活动内容:";
        Lbl_DescLabel.text = "活动内容";
    }

    public void Uninitialize()
    {

    }

    public void UpdateItem(ActivityTimeInfo data)
    {
        Initialize();
        Lbl_DescLabel.text = ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityItemDescByType((GameActivityType)data.activity_type);
    }

}
