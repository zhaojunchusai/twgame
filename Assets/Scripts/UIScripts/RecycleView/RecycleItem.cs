using UnityEngine;
using System;
using System.Collections;

public class RecycleItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_LvBG;
    [HideInInspector]public GameObject Gobj_BG;
    [HideInInspector]public UILabel Lbl_Lv;
    [HideInInspector]public UISprite Spt_Star1;
    [HideInInspector]public UISprite Spt_Star2;
    [HideInInspector]public UISprite Spt_Star3;
    [HideInInspector]public UISprite Spt_Star4;
    [HideInInspector]public UISprite Spt_Star5;
    [HideInInspector]public UISprite Spt_Star6;
    [HideInInspector]public UILabel Lbl_Name;
    [HideInInspector]public UILabel Lbl_Num;
    [HideInInspector]public UISprite Spt_PropBG;
    [HideInInspector]public UISprite Spt_Quality;
    [HideInInspector]public UISprite Spt_Icon;
    [HideInInspector]public UISprite Spt_Mark;
    [HideInInspector]public UISprite Spt_Selected;
    [HideInInspector]public UILabel Lbl_Step;
    [HideInInspector]public UISpriteAnimation Ani_Disappear;

    private bool _initialized = false;
    private GameObject[] _stars = null;
    private System.Action<object, object,int,bool> _clickCallback;
    private System.Func<object ,bool> _getState;
    private ERecycleContentType _type;
    private CommonItemData _data;
    private bool _choosed = false;
    public void Initialize()
    {
        if (_initialized)
            return;
        _initialized = true;

        Spt_LvBG = transform.FindChild("LvBG").gameObject.GetComponent<UISprite>();
        Gobj_BG = transform.FindChild("BG").gameObject;
        Lbl_Lv = transform.FindChild("LvBG/Lv").gameObject.GetComponent<UILabel>();
        Spt_Star1 = transform.FindChild("Stars/Star1").gameObject.GetComponent<UISprite>();
        Spt_Star2 = transform.FindChild("Stars/Star2").gameObject.GetComponent<UISprite>();
        Spt_Star3 = transform.FindChild("Stars/Star3").gameObject.GetComponent<UISprite>();
        Spt_Star4 = transform.FindChild("Stars/Star4").gameObject.GetComponent<UISprite>();
        Spt_Star5 = transform.FindChild("Stars/Star5").gameObject.GetComponent<UISprite>();
        Spt_Star6 = transform.FindChild("Stars/Star6").gameObject.GetComponent<UISprite>();
        Lbl_Name = transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Lbl_Num = transform.FindChild("Num").gameObject.GetComponent<UILabel>();
        Spt_PropBG = transform.FindChild("Prop/PropBG").gameObject.GetComponent<UISprite>();
        Lbl_Step = transform.FindChild("Prop/Step").gameObject.GetComponent<UILabel>();
        Spt_Quality = transform.FindChild("Prop/Quality").gameObject.GetComponent<UISprite>();
        Spt_Icon = transform.FindChild("Prop/Icon").gameObject.GetComponent<UISprite>();
        Spt_Mark = transform.FindChild("Prop/Mark").gameObject.GetComponent<UISprite>();
        Spt_Selected = transform.FindChild("Selected").gameObject.GetComponent<UISprite>();
        Ani_Disappear = transform.FindChild("Disappear").gameObject.GetComponent<UISpriteAnimation>();
        _stars = new GameObject[6] { Spt_Star1.gameObject, Spt_Star2.gameObject, Spt_Star3.gameObject, 
            Spt_Star4.gameObject, Spt_Star5.gameObject, Spt_Star6.gameObject };
        UIEventListener.Get(Gobj_BG).onClick = Click;
    }

    public void InitItem(ERecycleContentType type, CommonItemData data, System.Action<object,object,int,bool> clickcallback, System.Func<object, bool> getState)
    {
        Initialize();
        _type = type;
        _data = data;
        _clickCallback = clickcallback;
        _getState = getState;
        Lbl_Num.gameObject.SetActive(true);
        Lbl_Name.text = data.Name;
        CommonFunction.SetSpriteName(Spt_Icon, data.Icon);
        CommonFunction.SetQualitySprite(Spt_Quality, data.Quality);
        switch (type)
        {
            case ERecycleContentType.Prop:
            case ERecycleContentType.Material:
            case ERecycleContentType.EquipChip:
            case ERecycleContentType.SoldierChip:
                {
                    Lbl_Num.text = string.Format(ConstString.FORMAT_NUM_X,data.Num);
                    Spt_LvBG.gameObject.SetActive(false);
                    Spt_Mark.gameObject.SetActive(true);
                    SetStar(0);
                    Lbl_Step.text = string.Empty;
                    _choosed = GetState(data.ID);
                    CommonFunction.SetChipMark(Spt_Mark, data.SubType, new Vector3(-28, 28, 0), new Vector3(-25, 28, 0));
                }
                break;
            case ERecycleContentType.Equip:
            case ERecycleContentType.Soldier:
                {
                    Spt_LvBG.gameObject.SetActive(true);
                    Lbl_Num.text = "";
                    Spt_Mark.gameObject.SetActive(false);
                    Lbl_Lv.text = data.Level.ToString();
                    SetStar(data.StarLv);
                    Lbl_Step.text = CommonFunction.GetStepShow(Lbl_Step,data.Step);
                    _choosed = GetState(data.UID);
                }
                break;
            case ERecycleContentType.LifeSoul:
                {
                    Spt_LvBG.gameObject.SetActive(true);
                    Spt_Mark.gameObject.SetActive(true);
                    Lbl_Num.text = "";
                    Lbl_Lv.text = data.Level.ToString();
                    SetStar(0);
                    Lbl_Step.text = string.Empty;
                    _choosed = GetState(data.UID);
                    CommonFunction.SetLifeSoulMark(Spt_Mark, data.LifeSoulGodEquip, new Vector3(35, 28, 0), 46, 46);
                }
                break;
            default:
                break;
        }
        Spt_Selected.gameObject.SetActive(_choosed);
    }
    public void ChooseItem()
    {
        if (!_choosed && _data.Quality < ItemQualityEnum.Purple)
        {
            Click(null);
        }
    }
    public void ShowEff()
    {
        if (!_choosed)
            return;
        Ani_Disappear.ResetToBeginning();
        Ani_Disappear.framesPerSecond = 25;
        Ani_Disappear.gameObject.SetActive(true);
        Ani_Disappear.Play();
        Assets.Script.Common.Scheduler.Instance.AddTimer(0.68f, false,
            () =>
            {
                if (Ani_Disappear != null)
                {
                    Ani_Disappear.gameObject.SetActive(false);

                }
            });
    }
    private void SetStar(int count)
    {
        for (int s = 0; s < _stars.Length; s++)
        {
            if (s < count)
            {
                _stars[s].gameObject.SetActive(true);
            }
            else
            {
                _stars[s].gameObject.SetActive(false);
            }
        }
    }
    private bool GetState(UInt64 id)
    {
        if (_getState == null)
            return false;
        return _getState(id);
    }
    private bool GetState(uint id)
    {
        if (_getState == null)
            return false;
        return _getState(id);
    }
    private void Click(GameObject go)
    {
        _choosed = !_choosed;
        Spt_Selected.gameObject.SetActive(_choosed);

        if (_clickCallback != null)
        {
            if ((int)_type < (int)ERecycleContentType.Equip)
            {
                _clickCallback(_data.ID,_data.RecyclePrice ,Mathf.Max(_data.Num,1),_data.Quality > ItemQualityEnum.Blue);
            }
            else
            {
                _clickCallback(_data.UID, _data.RecyclePrice, Mathf.Max(_data.Num, 1), _data.Quality > ItemQualityEnum.Blue);
            }
        }
    }
    public void Uninitialize()
    {

    }
}
