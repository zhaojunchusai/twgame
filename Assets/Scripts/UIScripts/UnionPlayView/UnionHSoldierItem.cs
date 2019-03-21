using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class UnionHSoldierItem : MonoBehaviour
{
    const int MAXNUM = 3;
    [HideInInspector]public UISprite Spt_IconSprite;
    [HideInInspector]public UISprite Spt_IconBGSprite;
    [HideInInspector]public UISprite Spt_QualitySprite;
    [HideInInspector]public UISprite Spt_MaskSprite;
    [HideInInspector]public UISprite Spt_DeadSprite;
    [HideInInspector]public UILabel Lbl_NumLabel;
    [HideInInspector]
    public UILabel lbl_Label_Step;
    [SerializeField]
    private uint _id;
    public uint ID
    {
        get { return _id; }
    }

    [SerializeField]
    private ulong _uid;
    public ulong UID
    {
        get { return _uid; }
    }

    [SerializeField]
    private int _num;
    public int Num
    {
        get { return _num; }
    }

    [SerializeField]
    private int _restNum;
    public int RestNum
    {
        get { return _restNum; }
        set { _restNum = value; }
    }

    private bool _isInit = false;

    private GameObject _attackEffect;

    public void Initialize()
    {
        if (_isInit)
            return;
        _isInit = true;
        Spt_IconSprite = transform.FindChild("IconSprite").gameObject.GetComponent<UISprite>();
        Spt_IconBGSprite = transform.FindChild("IconBGSprite").gameObject.GetComponent<UISprite>();
        Spt_QualitySprite = transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_MaskSprite = transform.FindChild("MaskSprite").gameObject.GetComponent<UISprite>();
        Spt_DeadSprite = transform.FindChild("DeadSprite").gameObject.GetComponent<UISprite>();
        Lbl_NumLabel = transform.FindChild("NumLabel").gameObject.GetComponent<UILabel>();
        lbl_Label_Step = transform.transform.FindChild("Step").gameObject.GetComponent<UILabel>();
        lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
        lbl_Label_Step.text = "";
        SetLabelValues();
    }

    void Awake()
    {
        Initialize();
        GameObject source = null;
        ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_UNIONBATTLE, (GameObject go) => { source = go; });
        _attackEffect = CommonFunction.InstantiateObject(source, this.transform);
        _attackEffect.gameObject.SetActive(false);
        Clear();
    }

    public void SetLabelValues()
    {
        Lbl_NumLabel.text = "x0";
    }

    public void Uninitialize()
    {

    }

    public void UpdateItem(Soldier soldier,int num)
    {
        lbl_Label_Step.text = "";
        _id = soldier.ID;
        _num = num;
        _uid = soldier.uId;
        Lbl_NumLabel.text = "x" + _num;
        Spt_IconSprite.gameObject.SetActive(true);
        CommonFunction.SetSpriteName(Spt_IconSprite, soldier.Att.Icon);
        CommonFunction.SetQualitySprite(Spt_QualitySprite, soldier.Att.quality, Spt_IconBGSprite);
        this.lbl_Label_Step.text = CommonFunction.GetStepShow(this.lbl_Label_Step,soldier.StepNum);
        Spt_MaskSprite.gameObject.SetActive(false);
        Spt_DeadSprite.gameObject.SetActive(false);
    }
  
    public void DoSettle()
    {
        if (_id == 0)
            return;
        if (_num > _restNum)
        {
            _num = _restNum;
            Lbl_NumLabel.text = "x" + _num;
        }
        if (_restNum <= 0)
        {
            SetDead();
        }
    }

    public void Clear()
    {
        Spt_IconSprite.gameObject.SetActive(false);
        CommonFunction.SetQualitySprite(Spt_QualitySprite, ItemQualityEnum.White, Spt_IconBGSprite);
        Spt_MaskSprite.gameObject.SetActive(false);
        Spt_DeadSprite.gameObject.SetActive(false);
        _id = 0;
        _num = 0;
        _restNum = 0;
        Lbl_NumLabel.text = "";
    }
    /// <summary>
    /// 死亡
    /// </summary>
    private void SetDead()
    {
        Spt_MaskSprite.gameObject.SetActive(true);
        Spt_DeadSprite.gameObject.SetActive(true);
    }

    public void Attacked()
    {
        GameObject effect = ShowEffectManager.Instance.ShowEffect(_attackEffect, this.transform);
        AutoDestroy comp = effect.AddComponent<AutoDestroy>();
        comp.lifeTime = 10f;
    }
}
