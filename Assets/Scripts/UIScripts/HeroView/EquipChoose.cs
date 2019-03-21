using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class EquipChoose
{
    public List<Weapon> tempList;
    int index = -1;
    private int MaxCount = 8;
    public static string UIName ="EquipChoose";
    public GameObject _uiRoot;
    public GameObject Item;
    public GameObject parent;
    private UIWrapContent WrapContentEquip;
    public UIScrollView ScrollViewEquip;
    public GameObject Btn_Button_close;
    //public UIPanel UIPanel_EquipScrollView;
    public UIScrollView ScrView_EquipScrollView;
    public UIGrid Grd_Grid;
    //public UISprite Tex_texture;
    //public UISprite Spt_quality;
    //public UISprite Quality_Back;
    public UILabel Lbl_Label_name;
    public UILabel Lbl_Label_descript;
    public UIBoundary Boundary = new UIBoundary();
    public TweenScale Anim_TScale;
    public UILabel TitleLabel;

    private List<ItemComponent> _weaponItemList = new List<ItemComponent>();

    public void Initialize()
    {
        //_uiRoot = GameObject.Instantiate(Resources.Load("Prefabs/UI/EquipChoose") as GameObject) as GameObject;
        //TODO:修改资源加载方式
       GameObject tmp = ResourceLoadManager.Instance.LoadView("EquipChoose") as GameObject;  //临时使用 modify by taiwei
       _uiRoot = CommonFunction.InstantiateObject(tmp, null);
        Btn_Button_close = _uiRoot.transform.FindChild("MarkSprite").gameObject;
        Grd_Grid = _uiRoot.transform.FindChild("Anim/EquipScrollView/Grid").gameObject.GetComponent<UIGrid>();
        WrapContentEquip = _uiRoot.transform.FindChild("Anim/EquipScrollView/Grid").gameObject.GetComponent<UIWrapContent>();
        ScrollViewEquip = _uiRoot.transform.FindChild("Anim/EquipScrollView").gameObject.GetComponent<UIScrollView>();
        Item = Grd_Grid.transform.FindChild("item").gameObject;
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        TitleLabel = _uiRoot.transform.FindChild("Anim/TitleGroup/TTSprite").gameObject.GetComponent<UILabel>();
        Item.SetActive(false);
        tempList = new List<Weapon>();
        SetLabelValues();
        SetBoundary();
    }
    public void InitWeaponItem(int index)
    {
        this.index = index;
        if (index == -1)
            tempList = PlayerData.Instance._WeaponDepot.GetLockAndUnlockList();
        else
        {
            tempList = PlayerData.Instance._WeaponDepot.GetLockAndUnlockList(Filter);
        }
        Main.Instance.StartCoroutine(CreatWeaponItem(tempList));
    }
    private IEnumerator CreatWeaponItem(List<Weapon> _data)
    {
        if (_data.Count > MaxCount)
            yield return 0;
        int count = _data.Count;
        int itemCount = _weaponItemList.Count;

        int index = Mathf.CeilToInt((float)count / WrapContentEquip.wideCount) - 1;
        if (index == 0)
            index = 1;
        WrapContentEquip.minIndex = -index;
        WrapContentEquip.maxIndex = 0;
        if (count % 2 != 0)
        {
            this.WrapContentEquip.cullContent = false;
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
            List<ItemComponent> destoryList = new List<ItemComponent>(itemCount - count + 1);

            for (int i = count; i < itemCount; i++)
            {
                destoryList.Add(_weaponItemList[i]);
            }

            foreach (ItemComponent temp in destoryList)
            {
                _weaponItemList.Remove(temp);
                GameObject.Destroy(temp.gameObject);
            }
            destoryList.Clear();
        }
        for (int i = 0; i < count; i++)
        {
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
                _weaponItemList[i].gameObject.SetActive(true);
            }
            _weaponItemList[i].SetInfo(_data[i]);
        }

        WrapContentEquip.ReGetChild();
        Grd_Grid.repositionNow = true;
        yield return 0;
        ScrollViewEquip.ResetPosition();
        yield return 0;
        Grd_Grid.repositionNow = true;
        Grd_Grid.gameObject.SetActive(false);
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
        Weapon temp = PlayerData.Instance._WeaponDepot.FindByUid(comp.uid);
        if(temp == null)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.THIS_EQUIP_UNLOCK);
            return;
        }
        string message = "";
        PlayerData.Instance._ArtifactedDepot.installEquipt(temp.Slot, this.index, message);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.parent.transform));
        GameObject.Destroy(this._uiRoot);
        return;
    }
    public bool Filter(Weapon wp)
    {
        if (index == -1)
            return true;

        if (PlayerData.Instance._ArtifactedDepot._EquiptList.Count >= this.index)
        {
            if (PlayerData.Instance._ArtifactedDepot._EquiptList[this.index] != null)
            {
                if (wp.uId == PlayerData.Instance._ArtifactedDepot._EquiptList[this.index].uId)
                    return false;
            }
        }
        return EquptedName.DIR_EQUPTED_NAME[index] == wp.Att.type;
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
        this.TitleLabel.text = ConstString.CHANGE_ARTIFACT;
    }

    public void Uninitialize()
    {

    }



    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }
    
}
