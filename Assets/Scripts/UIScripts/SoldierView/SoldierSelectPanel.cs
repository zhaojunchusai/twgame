using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class SDSelectPanel
{
    public GameObject root;
    public Soldier infoSoldier;
    public UIButton Btn_Button_close;
    public UIButton Btn_Button_intensify;
    public UIButton Btn_Button_fastChoose;

    public GameObject Soldier1;
    public GameObject Soldier2;
    public GameObject Soldier3;
    public GameObject Soldier4;
    public GameObject Soldier5;

    public UILabel Lbl_Label_Cost;

    private UIWrapContent WrapContent;
    public UIScrollView ScrollView;
    public UIGrid Grd_Grid;
    public GameObject Item;
    private int MaxCount = 20;

    public List<Soldier> materialList;
    public List<Soldier> selectList;
    public List<Soldier> leftList;
    private List<SDUpStartemComponent> _selectItemList = new List<SDUpStartemComponent>();
    public void init(GameObject _uiRoot)
    {
        root = _uiRoot.transform.FindChild("SoldierSelectPanel").gameObject;

        Btn_Button_close = _uiRoot.transform.FindChild("SoldierSelectPanel/Anim/CloseButton").gameObject.GetComponent<UIButton>();
        Btn_Button_intensify = _uiRoot.transform.FindChild("SoldierSelectPanel/Anim/IntensifyButton").gameObject.GetComponent<UIButton>();
        Btn_Button_fastChoose = _uiRoot.transform.FindChild("SoldierSelectPanel/Anim/FastChooseButton").gameObject.GetComponent<UIButton>();

        Soldier1 = _uiRoot.transform.FindChild("SoldierSelectPanel/Anim/SoldierComp1").gameObject;
        Soldier2 = _uiRoot.transform.FindChild("SoldierSelectPanel/Anim/SoldierComp2").gameObject;
        Soldier3 = _uiRoot.transform.FindChild("SoldierSelectPanel/Anim/SoldierComp3").gameObject;
        Soldier4 = _uiRoot.transform.FindChild("SoldierSelectPanel/Anim/SoldierComp4").gameObject;
        Soldier5 = _uiRoot.transform.FindChild("SoldierSelectPanel/Anim/SoldierComp5").gameObject;

        Lbl_Label_Cost = _uiRoot.transform.FindChild("SoldierSelectPanel/Anim/CostGroup/CostLabel").gameObject.GetComponent<UILabel>();

        WrapContent = _uiRoot.transform.FindChild("SoldierSelectPanel/Anim/MaterialScroll/Grid").gameObject.GetComponent<UIWrapContent>();
        ScrollView = _uiRoot.transform.FindChild("SoldierSelectPanel/Anim/MaterialScroll").gameObject.GetComponent<UIScrollView>();
        Grd_Grid = _uiRoot.transform.FindChild("SoldierSelectPanel/Anim/MaterialScroll/Grid").gameObject.GetComponent<UIGrid>();
        Item = _uiRoot.transform.FindChild("SoldierSelectPanel/Anim/MaterialScroll/item").gameObject;
        WrapContent.onInitializeItem = SetSelectInfo;
    }
    public void setInfo()
    {
        if (!root.activeSelf)
            //UISystem.Instance.SoldierAttView.PlayOpenSelectAnim();

        selectList = new List<Soldier>();
        leftList = new List<Soldier>(5);

        root.SetActive(true);

        for (int i = 0; i < 5;++i )
        {
            leftList.Add(null);
        }
        materialList = null;
        selectList.Clear();
        _setInfo();
    }
    public void OnClose()
    {
        root.SetActive(false);
        infoSoldier = null;
    }
    public void OnIntensify()
    {

    }
    public void FastChoose()
    {
        List<Soldier> tempSelect = new List<Soldier>(5);
        List<Soldier> soldierList = PlayerData.Instance._SoldierDepot.getSoldierList((wp) => { return true; });
        soldierList.Reverse();
        for(int i = 1;i < Soldier.MAXQUALITY;++i)
        {
            List<Soldier> tempList = soldierList.FindAll((Soldier tp) => { return tp.Att.quality == i; });
            if (tempList.Count < 1) continue;

            int count = 0;
            foreach(Soldier sd in tempList)
            {
                if (!sd.isMaxLevel())
                    continue;
                tempSelect.Add(sd);
                count += (int)Math.Pow(2,sd.Att.Star - 1);
                if (count >= 5)
                {
                    selectList.Clear();
                    selectList = tempSelect;
                    InitSelectItem();
                    OnSelect();
                    return;
                }
            }
            tempSelect.Clear();
        }
        ErrorCode.ShowErrorTip((int)ErrorCodeEnum.NotEnoughMaterial);
        Lbl_Label_Cost.text = "";
    }
    private void _setInfo()
    {
        for (int i = 1; i <= 5; ++i)
        {
            GameObject sd = root.transform.FindChild(string.Format("SoldierComp{0}", i)).gameObject;
            UISprite tx = sd.transform.FindChild("IconTexture").gameObject.GetComponent<UISprite>();
            UISprite qt = sd.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
            UISprite bc = sd.transform.FindChild("back").gameObject.GetComponent<UISprite>();
            UIGrid Grid_star = sd.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();
           
            if (tx)
            {
                tx.gameObject.SetActive(false);
            }
            if (qt)
            {
                qt.gameObject.SetActive(false);
            }
            if(bc)
            {
                bc.gameObject.SetActive(false);
            }
            Grid_star.gameObject.SetActive(false);
            int count = i;
            UIEventListener.Get(sd).onClick = (GameObject go) =>
                {
                    
                    if(this.leftList[count - 1] != null)
                    {
                        for (int k = 0; k < _selectItemList.Count; k++)
                        {
                            SDUpStartemComponent widget = _selectItemList[k];
                            GameObject mark = widget.transform.FindChild("mark").gameObject;
                            Soldier temp = this.materialList.Find((sod) => { if (sd == null)return false; return sod.uId == widget.uID; });
                            if (temp == null)
                                continue;
                            if (k < materialList.Count)
                            {
                                if (leftList[count - 1] == temp)
                                {
                                    mark.SetActive(false);
                                    selectList.Remove(leftList[count - 1]);
                                    OnSelect();
                                }
                            }
                        }
                    }
                };
        }
        Lbl_Label_Cost.text = "";
        InitSelectItem();
    }
    public void InitSelectItem()
    {
        materialList = PlayerData.Instance._SoldierDepot.getSoldierList(filter);

        Main.Instance.StartCoroutine(CreatSelectItem(materialList));
    }
    private IEnumerator CreatSelectItem(List<Soldier> _data)
    {
        int count = _data.Count;
        int itemCount = _selectItemList.Count;
        
        int index = Mathf.CeilToInt((float)count / WrapContent.wideCount) - 1;
        if (index == 0)
            index = 1;
        WrapContent.minIndex = -index;
        WrapContent.maxIndex = 0;

        if (count > MaxCount)
        {
            WrapContent.enabled = true;
            count = MaxCount;
        }
        else
        {
            WrapContent.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                _selectItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(Item, Grd_Grid.transform);
                SDUpStartemComponent item = vGo.GetComponent<SDUpStartemComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<SDUpStartemComponent>();
                    item.MyStart(vGo);
                }
                _selectItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                _selectItemList[i].TouchEvent += OnItemTouch;
            }
            else
            {
                _selectItemList[i].gameObject.SetActive(true);
            }
            _selectItemList[i].SetInfo(_data[i],false);

            GameObject LessLv = _selectItemList[i].gameObject.transform.FindChild("LessLv").gameObject;
            GameObject mark = _selectItemList[i].gameObject.transform.FindChild("mark").gameObject;

            if (!_data[i].isMaxLevel())
            {
                LessLv.SetActive(true);
            }
            else
            {
                LessLv.SetActive(false);
            }
            if (selectList.Contains(_data[i]))
            {
                mark.SetActive(true);
            }
            else
            {
                mark.SetActive(false);
            }
        }
        WrapContent.ReGetChild();
        Grd_Grid.repositionNow = true;
        yield return 0.5;
        this.ScrollView.ResetPosition();
        //Grd_Grid.repositionNow = true;
        //Grd_Grid.gameObject.SetActive(false);
        //Grd_Grid.gameObject.SetActive(true);

    }

    public void SetSelectInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= this.materialList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        SDUpStartemComponent item = _selectItemList[wrapIndex];
        item.SetInfo(this.materialList[realIndex],false);

        GameObject LessLv = item.gameObject.transform.FindChild("LessLv").gameObject;
        GameObject mark = item.gameObject.transform.FindChild("mark").gameObject;

        if (!materialList[realIndex].isMaxLevel())
        {
            LessLv.SetActive(true);
        }
        else
        {
            LessLv.SetActive(false);
        }
        if (selectList.Contains(materialList[realIndex]))
        {
            mark.SetActive(true);
        }
        else
        {
            mark.SetActive(false);
        }
    }

    public void OnItemTouch(SDUpStartemComponent go)
    {
        if (go == null) return;
        Soldier tempSoldier = this.materialList.Find((sd) => { if (sd == null)return false; return sd.uId == go.uID; });
        if (tempSoldier == null) return;

        GameObject mark = go.transform.FindChild("mark").gameObject;
        GameObject LessLv = go.transform.FindChild("LessLv").gameObject;
        if (LessLv.activeSelf)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, "µÈ¼¶²»×ã");
            return;
        }
        
        if(mark.activeInHierarchy)
        {
            mark.SetActive(false);
            selectList.Remove(tempSoldier);
        }
        else
        {
            int num = 0;
            foreach (var temp in selectList)
            {
                num += (int)Math.Pow(2, temp.Att.Star - 1);
            }
            if (num >= 5)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.MATERIAL_ENOUGH);
                return;
            }
            mark.SetActive(true);
            selectList.Add(tempSoldier);
        }
        OnSelect();

        return;
    }
    private bool filter(Soldier sd)
    {
        if(selectList.Count > 0)
        {
            if (sd.Att.quality != selectList[0].Att.quality)
                return false;
        }
        if (sd.Att.quality == 6)
            return false;
        return true;
    }
    private void OnSelect()
    {
        //if (selectList.Count < 2) InitSelectItem();
        //if (selectList.Count > 0)
        //{
        //    int cost = ConfigManager.Instance.mSoldierSelectData.FindByQuality
        //            (
        //                (this.selectList[0].Att.quality) < 5 ? (this.selectList[0].Att.quality) : 5
        //            ).money_num;
        //    if(cost > PlayerData.Instance._Gold)
        //    {
        //        Lbl_Label_Cost.color = Color.red;
        //    }
        //    else
        //    {
        //        Lbl_Label_Cost.color = Color.green;
        //    }
        //    Lbl_Label_Cost.text = string.Format
        //        (
        //        "{0}",
        //        cost
        //        );

        //}
        //else
        //    Lbl_Label_Cost.text = "";

        //int count = 0;
        //int num = 0;
        //for (int i = 1; i <= 5; ++i)
        //{
        //    if (selectList.Count > count && num == 0)
        //    {
        //        num = (int)Math.Pow(2, selectList[count].Att.Star - 1);
        //        ++count;
        //    }
        //    GameObject sd = root.transform.FindChild(string.Format("SoldierComp{0}",i)).gameObject;
        //    UISprite tx = sd.transform.FindChild("IconTexture").gameObject.GetComponent<UISprite>();
        //    UISprite qt = sd.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
        //    UISprite bc = sd.transform.FindChild("back").gameObject.GetComponent<UISprite>();
        //    UIGrid Grid_star = sd.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();

        //    SoldierAttributeInfo temp = new SoldierAttributeInfo();
        //    if (count > 0)
        //    {
        //        temp = selectList[count - 1].Att;
        //    }
        //    if (tx)
        //    {

        //        if(num > 0)
        //        {
        //            tx.gameObject.SetActive(true);
        //            CommonFunction.SetSpriteName(tx, temp.Icon);
        //            leftList[i - 1] = selectList[count - 1];
        //        }
        //        else
        //        {
        //            tx.gameObject.SetActive(false);
        //            leftList[i - 1] = null;
        //        }
                
        //    }
        //    if (qt)
        //    {
        //        if (num > 0)
        //        {
        //            CommonFunction.SetQualitySprite(qt, temp.quality, bc);
        //            qt.gameObject.SetActive(true);
        //        }
        //        else
        //        {
        //            qt.gameObject.SetActive(false);
        //        }
        //    }
        //    if (Grid_star != null)
        //    {
        //        if (num > 0)
        //        {
        //            Grid_star.gameObject.SetActive(true);
        //            var tempList = Grid_star.GetChildList();
        //            for (int k = 0; k < tempList.Count; ++k)
        //            {
        //                GameObject star = tempList[k].FindChild("SelectSprite").gameObject;
        //                if (k < temp.Star)
        //                {
        //                    star.SetActive(true);
        //                }
        //                else
        //                {
        //                    star.SetActive(false);
        //                }
        //            }
        //        }
        //        else
        //            Grid_star.gameObject.SetActive(false);
                
        //    }
        //    --num;
        //    Soldier last = null;
        //    for (int m = 0; m < leftList.Count; ++m)
        //    {
        //        if (leftList[m] != last && leftList[m] != null)
        //        {
        //            last = leftList[m];
        //        }
        //        else
        //        {
        //            leftList[m] = null;
        //        }
        //    }
        //    last = null;
        //}
    }
    public void RefreshCost()
    {
        if (selectList.Count > 0)
        {
            //int cost = ConfigManager.Instance.mSoldierSelectData.FindByQuality
            //        (
            //            (this.selectList[0].Att.quality) < 5 ? (this.selectList[0].Att.quality) : 5
            //        ).money_num;
            //if (cost > PlayerData.Instance._Gold)
            //{
            //    Lbl_Label_Cost.color = Color.red;
            //}
            //else
            //{
            //    Lbl_Label_Cost.color = Color.green;
            //}
            //Lbl_Label_Cost.text = string.Format
            //    (
            //    "{0}",
            //    cost
            //    );

        }
        else
            Lbl_Label_Cost.text = "";

    }
}
