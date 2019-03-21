using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;

public class UnionHPlayerInfoItem : MonoBehaviour
{
    public delegate void VoidDelegate();
    public VoidDelegate InBattleComplete;

    const float INVERTALTIME = 1f;

    [HideInInspector]public UISprite Spt_ItemBGSprite;
    [HideInInspector]public UILabel Lbl_PlayerNameLabel;
    [HideInInspector]public UILabel Lbl_PlayerLvLabel;
    [HideInInspector]public UILabel Lbl_CombatPowerLabel;
    [HideInInspector]public UISprite Spt_SplitSprite;
    [HideInInspector]public UIGrid Grd_SoldierGrid;
    [HideInInspector]
    public TweenPosition Grd_Position;

    [HideInInspector]public GameObject EffectPos;
    [HideInInspector]
    public GameObject RefreshPos;

    [HideInInspector]public UnionHSoldierItem UnionHSoldierItem_0;
    [HideInInspector]public UnionHSoldierItem UnionHSoldierItem_1;
    [HideInInspector]public UnionHSoldierItem UnionHSoldierItem_2;
    [HideInInspector]public UnionHSoldierItem UnionHSoldierItem_3;
    [HideInInspector]public UnionHSoldierItem UnionHSoldierItem_4;
    [HideInInspector]public UnionHSoldierItem UnionHSoldierItem_5;

    [HideInInspector]public GameObject Obj_BattleObj;
    [HideInInspector]public GameObject Obj_RefreshObj;
    [SerializeField]
    private uint _id;
    public uint ID
    {
        get { return _id; }
    }

    private bool _isWin;
    public bool IsWin
    {
        get { return _isWin; }
        set { _isWin = value; }
    }

    [SerializeField]
    private List<UnionHSoldierItem> _soldierItemList;


    public void Initialize()
    {
        Spt_ItemBGSprite = transform.FindChild("ItemBGSprite").gameObject.GetComponent<UISprite>();
        Lbl_PlayerNameLabel = transform.FindChild("PlayerNameLabel").gameObject.GetComponent<UILabel>();
        Lbl_PlayerLvLabel = transform.FindChild("PlayerLvLabel").gameObject.GetComponent<UILabel>();
        Lbl_CombatPowerLabel = transform.FindChild("CombatPowerLabel").gameObject.GetComponent<UILabel>();
        Spt_SplitSprite = transform.FindChild("SplitSprite").gameObject.GetComponent<UISprite>();
        Grd_SoldierGrid = transform.FindChild("SoldierGrid").gameObject.GetComponent<UIGrid>();
        Grd_Position = transform.FindChild("SoldierGrid").gameObject.GetComponent<TweenPosition>();
        EffectPos = transform.FindChild("Effect").gameObject;
        RefreshPos = transform.FindChild("Refresh").gameObject;
        UnionHSoldierItem_0 = transform.FindChild("SoldierGrid/UnionHSoldierItem_0").gameObject.AddComponent<UnionHSoldierItem>();
        UnionHSoldierItem_1 = transform.FindChild("SoldierGrid/UnionHSoldierItem_1").gameObject.AddComponent<UnionHSoldierItem>();
        UnionHSoldierItem_2 = transform.FindChild("SoldierGrid/UnionHSoldierItem_2").gameObject.AddComponent<UnionHSoldierItem>();
        UnionHSoldierItem_3 = transform.FindChild("SoldierGrid/UnionHSoldierItem_3").gameObject.AddComponent<UnionHSoldierItem>();
        UnionHSoldierItem_4 = transform.FindChild("SoldierGrid/UnionHSoldierItem_4").gameObject.AddComponent<UnionHSoldierItem>();
        UnionHSoldierItem_5 = transform.FindChild("SoldierGrid/UnionHSoldierItem_5").gameObject.AddComponent<UnionHSoldierItem>();
        this.Grd_Position.AddOnFinished(OnFinish);
        this.Grd_Position.enabled = false;
        SetLabelValues();

        _soldierItemList = new List<UnionHSoldierItem>(){
        UnionHSoldierItem_0,
        UnionHSoldierItem_1,
        UnionHSoldierItem_2,
        UnionHSoldierItem_3,
        UnionHSoldierItem_4,
        UnionHSoldierItem_5
        };
    }

    void Awake()
    {
        Initialize();
        ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_HEGEMONYRefresh, (GameObject go) => 
        {
            if(go != null)
            {
                Obj_RefreshObj = CommonFunction.InstantiateObject(go, this.RefreshPos.transform);
                Obj_RefreshObj.gameObject.SetActive(false);
            }
        });
    }
    public void LoadEffect(string effect)
    {
        GameObject source = null;
        ResourceLoadManager.Instance.LoadEffect(effect, (GameObject go) => { source = go; });
        Obj_BattleObj = CommonFunction.InstantiateObject(source, this.EffectPos.transform);
        Obj_BattleObj.gameObject.SetActive(false);
    }
    public void SetLabelValues()
    {
        Lbl_PlayerNameLabel.text = "";
        Lbl_PlayerLvLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_LVNUM, 0);
        Lbl_CombatPowerLabel.text = string.Format(ConstString.UNIONRANK_COMBAT, 0);
    }

    public void Uninitialize()
    {

    }

    public void UpdateItem(ArenaPlayer playerInfo)
    {
        //ShowRefreshEffect();
        //Debug.LogWarning("This >>>>>>>>>>> "+ this.transform.name);
        _id = playerInfo.hero.charid;
        Lbl_PlayerNameLabel.text = playerInfo.hero.charname;
        Lbl_PlayerLvLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_LVNUM, playerInfo.hero.level);
        Lbl_CombatPowerLabel.text = string.Format(ConstString.UNIONRANK_COMBAT,CommonFunction.GetTenThousandUnit(playerInfo.combat_power));
        int infoCount = playerInfo.soldiers.Count;
        int itemCount = _soldierItemList.Count;
        for (int i = 0; i < itemCount; i++)
        {
            UnionHSoldierItem soldierItem = _soldierItemList[i];
            soldierItem.gameObject.SetActive(true);
            if (i <= infoCount - 1)
            {
                ArenaSoldier arenaSoldier = playerInfo.soldiers[i];
                Soldier soldier = Soldier.createByID(arenaSoldier.soldier.id);
                soldier.Serialize(arenaSoldier.soldier);
                soldierItem.UpdateItem(soldier, arenaSoldier.num);
            }
            else
            {
                soldierItem.Clear();
            }
        }
    }

    public void Clear()
    {
        foreach (UnionHSoldierItem item in _soldierItemList)
        {
            item.Clear();
        }
        Lbl_PlayerNameLabel.text = "";
        Lbl_PlayerLvLabel.text = string.Format(ConstString.UNIONBATTLE_LABEL_LVNUM, 0);
        Lbl_CombatPowerLabel.text = string.Format(ConstString.UNIONRANK_COMBAT, 0);
        _id = 0;
    }

    public void SetRestSoldier(List<ArenaSoldier> list)
    {
        KillAllSoldiers();
        //foreach (ArenaSoldier _as in list)
        //{
        //    Debug.LogWarning("In item " + transform.name + " id = " + _as.soldier.id + " num = " + _as.num);
        //}
        if (list == null || list.Count < 1)
        {
            return;
        }
        int infoCount = list.Count;
        for (int i = 0; i < infoCount; i++)
        {
            ArenaSoldier info = list[i];
            UnionHSoldierItem item = GetSoldierByID(info.soldier.id);
            if (item != null)
            {
                item.RestNum = info.num;
            }
        }
    }

    public void KillAllSoldiers()
    {
        foreach (UnionHSoldierItem item in _soldierItemList)
        {
            item.RestNum = 0;
        }
    }
    /// <summary>
    /// 结算剩余的士兵
    /// </summary>
    public void DoSettle()
    {
        foreach (UnionHSoldierItem item in _soldierItemList)
        {
            item.DoSettle();
        }
    }

    public void StartBattle(float delay)
    {
        StartCoroutine(BattleInspector(delay));
    }

    private UnionHSoldierItem GetSoldierByID(uint id)
    {
        if (_soldierItemList.Count == null || _soldierItemList.Count < 1)
            return null;
        int count = _soldierItemList.Count;
        for (int i = 0; i < count; i++)
        {
            UnionHSoldierItem item = _soldierItemList[i];
            if (item.ID == id)
                return item;
        }
        return null;
    }

    private IEnumerator BattleInspector(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowAttackedEffect();
        yield return new WaitForSeconds(INVERTALTIME);
        if (InBattleComplete != null)
        {
            InBattleComplete();
        }
    }

    public void ShowAttackedEffect()
    {
        GameObject effect = ShowEffectManager.Instance.ShowEffect(this.Obj_BattleObj, this.EffectPos.transform);
        if (effect == null)
            return;
        UISystem.Instance.ResortViewOrder();
        AutoDestroy comp = effect.AddComponent<AutoDestroy>();
        comp.lifeTime = 2f;
        //Debug.LogWarning("ShowAttackedEffect " + transform.name + " at "+ Time.time);
        //for (int i = 0; i < 3; i++)
        //{
        //    UnionHSoldierItem item = _soldierItemList[i * 2];
        //    if (item.ID > 0)
        //        item.Attacked();
        //}
    }
    public void TweenPosition()
    {
        this.Grd_Position.enabled = true;
        this.Grd_Position.Restart();
    }
    void OnFinish()
    {
        this.Grd_Position.enabled = false;
    }
    private void ShowRefreshEffect()
    {
        GameObject effect = ShowEffectManager.Instance.ShowEffect(this.Obj_RefreshObj, this.RefreshPos.transform);
        if (effect == null)
            return;
        UISystem.Instance.ResortViewOrder();
        AutoDestroy comp = effect.AddComponent<AutoDestroy>();
        comp.lifeTime = 2f;
    }
}
