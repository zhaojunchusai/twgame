using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class SoldierEquipChoose
{
    public Soldier soldier;
    public List<Weapon> tempList;
    int index = -1;
    int type = -1;
    private int MaxCount = 8;
    public static string UIName ="EquipChoose";
    public GameObject _uiRoot;
    public GameObject Item;
    public GameObject parent;
    private UIWrapContent WrapContentEquip;
    public UIScrollView ScrollViewEquip;
    public GameObject Btn_Button_close;
    public UIPanel UIPanel_EquipScrollView;
    public UIScrollView ScrView_EquipScrollView;
    public UIGrid Grd_Grid;
    public UILabel Lbl_Label_name;
    public UILabel Lbl_Label_descript;
    public UIBoundary Boundary = new UIBoundary();
    public TweenScale Anim_TScale;
    private bool isOpen = true;
    public UILabel TitleLabel;
    private List<ItemComponent> _weaponItemList = new List<ItemComponent>();
    public void Initialize()
    {
        //TODO:修改资源加载方式  
//        _uiRoot = GameObject.Instantiate(Resources.Load("Prefabs/UI/EquipChoose") as GameObject) as GameObject;
        GameObject tmp = ResourceLoadManager.Instance.LoadView("EquipChoose") as GameObject;  //临时使用 modify by taiwei
        _uiRoot = CommonFunction.InstantiateObject(tmp, null);
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        Btn_Button_close = _uiRoot.transform.FindChild("MarkSprite").gameObject;
        Grd_Grid = _uiRoot.transform.FindChild("Anim/EquipScrollView/Grid").gameObject.GetComponent<UIGrid>();
        WrapContentEquip = _uiRoot.transform.FindChild("Anim/EquipScrollView/Grid").gameObject.GetComponent<UIWrapContent>();
        ScrollViewEquip = _uiRoot.transform.FindChild("Anim/EquipScrollView").gameObject.GetComponent<UIScrollView>();
        Item = Grd_Grid.transform.FindChild("item").gameObject;
        TitleLabel = _uiRoot.transform.FindChild("Anim/TitleGroup/TTSprite").gameObject.GetComponent<UILabel>();
        tempList = new List<Weapon>();
        Item.SetActive(false);
        isOpen = true;
        SetLabelValues();
        SetBoundary();
    }
    public void InitWeaponItem(int index, int type)
    {
        this.index = index;
        this.type = type;
        if (index == -1)
            tempList = PlayerData.Instance._SoldierEquip._weaponList;
        else
        {
            tempList = PlayerData.Instance._SoldierEquip.getWeaponList(Filter);
        }

        Main.Instance.StartCoroutine(CreatWeaponItem(tempList));
    }

    private IEnumerator CreatWeaponItem(List<Weapon> _data)
    {
        if (!isOpen) yield break;
        yield return 0;
        int count = _data.Count;
       // WrapContentEquip.CleanChild();

        int itemCount = _weaponItemList.Count;

        int index = Mathf.CeilToInt((float)count / WrapContentEquip.wideCount) - 1;
        if (index == 0)
            index = 1;
        WrapContentEquip.minIndex = -index;
        WrapContentEquip.maxIndex = 0;
        if (!isOpen) yield break;
        if (count % 2 != 0)
        {
            this.WrapContentEquip.cullContent = false;
            if (count == 1)
                this.WrapContentEquip.cullContent = true;
        }
        else
        {
            this.WrapContentEquip.cullContent = true;
        }
        if (count > MaxCount)
        {
            WrapContentEquip.enabled = true;
            count = MaxCount;
        }
        else
        {
            WrapContentEquip.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                _weaponItemList[i].gameObject.SetActive(false);
            }
        }
        if (!isOpen) yield break;
        for (int i = 0; i < count; i++)
        {
            if (!isOpen) yield break;
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(Item, Grd_Grid.transform);
                ItemComponent item = vGo.GetComponent<ItemComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<ItemComponent>();
                    item.MyStart(vGo);
                }
                _weaponItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                _weaponItemList[i].TouchEvent += OnItemTouch;
            }
            else
            {
                if(_weaponItemList[i] == null)
                {
                    GameObject vGo = CommonFunction.InstantiateObject(Item, Grd_Grid.transform);
                    ItemComponent item = vGo.GetComponent<ItemComponent>();
                    if (item == null)
                    {
                        item = vGo.AddComponent<ItemComponent>();
                        item.MyStart(vGo);
                    }
                    _weaponItemList.Insert(i, item);
                    vGo.name = i.ToString();
                    vGo.SetActive(true);
                    _weaponItemList[i].TouchEvent += OnItemTouch;
                }
                _weaponItemList[i].gameObject.SetActive(true);
            }
            _weaponItemList[i].SetInfo(_data[i]);
        }
        if (!isOpen) yield break;
        if (WrapContentEquip != null)
            WrapContentEquip.ReGetChild();
        if (!isOpen) yield break;
        if (this.Grd_Grid != null)
            Grd_Grid.repositionNow = true;
        if (!isOpen) yield break;
        yield return 0;
        if (this.ScrollViewEquip != null)
            ScrollViewEquip.ResetPosition();
        if (!isOpen) yield break;
        yield return 0;
        if (this.Grd_Grid != null)
            Grd_Grid.repositionNow = true;
        if (!isOpen) yield break;
        if (this.Grd_Grid != null && this.Grd_Grid.gameObject != null)
            Grd_Grid.gameObject.SetActive(false);
        if (!isOpen) yield break;
        if (this.Grd_Grid != null && this.Grd_Grid.gameObject != null)
            Grd_Grid.gameObject.SetActive(true);
    }

    public void SetWeaponInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= tempList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        ItemComponent item = _weaponItemList[wrapIndex];
        item.SetInfo(tempList[realIndex]);
    }
    public void OnItemTouch(ItemComponent comp)
    {
        Weapon temp = PlayerData.Instance._SoldierEquip.FindByUid(comp.uid);
        string message = "";
        soldier._equippedDepot.installEquipt(temp.Slot, this.index, message);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.parent.transform));
        GameObject.Destroy(this._uiRoot);
        return;
    }
    public bool Filter(Weapon wp)
    {
        if (index == -1)
            return true;
        if(this.soldier._equippedDepot._EquiptList.Count >= this.index)
        {
            if(this.soldier._equippedDepot._EquiptList[this.index] != null)
            {
                if (wp == this.soldier._equippedDepot._EquiptList[this.index])
                    return false;
            }
        }
        return type == (int)wp.Att.type;
    }

    public void SetBoundary()
    {
        Boundary.left = -313f;
        Boundary.right = 367f;
        Boundary.up = 204f;
        Boundary.down = -216f;
        WrapContentEquip.onInitializeItem = SetWeaponInfo;
    }

    public void SetLabelValues()
    {
        this.TitleLabel.text = ConstString.CHANGE_EQUIP;
    }

    public void Uninitialize()
    {
        isOpen = false;
    }



    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
