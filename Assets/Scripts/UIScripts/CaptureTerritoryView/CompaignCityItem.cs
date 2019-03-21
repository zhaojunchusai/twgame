using UnityEngine;
using System;
using System.Collections;

public class CompaignCityItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_Icon;
    [HideInInspector]public UISprite Spt_Fighting;
    [HideInInspector]public UILabel Lbl_Name;
    [HideInInspector]public GameObject Gobj_Union;
    [HideInInspector]public UILabel Lbl_UnionName;

    private CaptureCityData _city;
    private bool _initialized = false;
    public void Initialize()
    {
        if (_initialized)
            return;
        _initialized = true;
        Spt_Icon = transform.FindChild("Icon").gameObject.GetComponent<UISprite>();
        Spt_Fighting = transform.FindChild("Fighting").gameObject.GetComponent<UISprite>();
        Lbl_Name = transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Gobj_Union = transform.FindChild("Union").gameObject;
        Lbl_UnionName = transform.FindChild("Union/UnionName").gameObject.GetComponent<UILabel>();
        SetLabelValues();
        UIEventListener.Get(Spt_Icon.gameObject).onClick = ClickCity;
    }

    public void Init(CaptureCityData city)
    {
        Initialize();
        _city = city;
        CommonFunction.SetSpriteName(Spt_Icon,_city.mIcon);
        Lbl_Name.text = _city.mName;
        if (_city.mIsCapital)
        {
            Lbl_Name.color = new Color(90/255f,228/255f,107/255f);
        }
        else
        {
            Lbl_Name.color = Color.white;
        }
        SetFightState();
    }

    public void SetFightState()
    {
        bool fighting = CaptureTerritoryModule.Instance.FightState == ECaptureTerritoryFightState.Fighting;
        Spt_Fighting.gameObject.SetActive(fighting);
        fogs.proto.msg.UnionOccupiedCity info = CaptureTerritoryModule.Instance.GetUnionOccupiedCity(_city.mID);
        if (!fighting && !string.IsNullOrEmpty(info.union_name))
        {
            Gobj_Union.SetActive(true);
            Lbl_UnionName.text = info.union_name;
        }else
            Gobj_Union.SetActive(false);

    }

    private void ClickCity(GameObject go)
    {
        CaptureTerritoryModule.Instance.SelectedCity(_city);
    }

    public void SetLabelValues()
    {
        Lbl_Name.text = "";
    }

    public void Uninitialize()
    {

    }
}
