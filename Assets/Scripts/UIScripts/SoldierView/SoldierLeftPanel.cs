using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class SoldierLeftPanel
{
    public GameObject _uiRoot;
    public Soldier soldier;
    public Soldier DebrisSoldier;
    public Debris debris;

    private UIWrapContent WrapContentSoldier;
    public UIScrollView ScrollViewSoldier;
    public UIGrid Grd_GridSoldier;
    public GameObject ItemSoldier;
    private int MaxSoldierCount = 6;

    private UIWrapContent WrapContentDebris;
    public UIScrollView ScrollViewDebris; 
    public UIGrid Grd_GridDebris;
    public GameObject ItemDebris;

    private List<Soldier> tempList;
    private SoldierAttViewController father;
    private GameObject LastMarkOb;

    private List<Debris> DebrisList;
    private GameObject LastMarkOb_Debris;

    public GameObject Debris_Prompt;
    public UILabel Debris_Lable;
    public UIButton Debris_Compound;
    public UILabel SoldierCount;
    public UIButton Filter_Button;
    public UIButton FilterCancel_Button;

    private List<SoldiertemComponent> _soldierItemList = new List<SoldiertemComponent>();
    private List<DebristemComponent> _debrisItemList = new List<DebristemComponent>();

    public bool isSoldier = true;
    public bool isFirst = false;

    public bool needRefreshSoldier = false;
    public bool needRefreshDebris = false;
    public bool IsFilter = false;

    public void Initialize(GameObject _uiRoot)
    {
        this._uiRoot = _uiRoot;

        WrapContentSoldier = _uiRoot.transform.FindChild("Anim/left/SoldierOb/Soldier/Grid").gameObject.GetComponent<UIWrapContent>();
        ScrollViewSoldier = _uiRoot.transform.FindChild("Anim/left/SoldierOb/Soldier").gameObject.GetComponent<UIScrollView>();
        Grd_GridSoldier = this._uiRoot.transform.FindChild("Anim/left/SoldierOb/Soldier/Grid").gameObject.GetComponent<UIGrid>();
        ItemSoldier = this._uiRoot.transform.FindChild("Anim/left/SoldierOb/Soldier/item").gameObject;
        WrapContentDebris = _uiRoot.transform.FindChild("Anim/left/Debris/Grid").gameObject.GetComponent<UIWrapContent>();
        ScrollViewDebris = _uiRoot.transform.FindChild("Anim/left/Debris").gameObject.GetComponent<UIScrollView>();
        Grd_GridDebris = this._uiRoot.transform.FindChild("Anim/left/Debris/Grid").gameObject.GetComponent<UIGrid>();
        ItemDebris = this._uiRoot.transform.FindChild("Anim/left/Debris/item").gameObject;

        Debris_Prompt = this._uiRoot.transform.FindChild("Anim/left/TabDebirs/prompt").gameObject;

        Debris_Lable = this._uiRoot.transform.FindChild("Anim/left/Count").gameObject.GetComponent<UILabel>();
        Debris_Compound = this._uiRoot.transform.FindChild("Anim/left/CompoundButton").gameObject.GetComponent<UIButton>();
        SoldierCount = this._uiRoot.transform.FindChild("Anim/left/Num/count").gameObject.GetComponent<UILabel>();
        Filter_Button = this._uiRoot.transform.FindChild("Anim/left/Num/FilterButton").gameObject.GetComponent<UIButton>();
        FilterCancel_Button = this._uiRoot.transform.FindChild("Anim/left/Num/CancelButton").gameObject.GetComponent<UIButton>();

        this.Filter_Button.gameObject.SetActive(true);
        this.FilterCancel_Button.gameObject.SetActive(false);

        if(Debris_Prompt)
        {
            Debris_Prompt.gameObject.SetActive(false);
        }
    }
    public void init(GameObject _uiRoot,SoldierAttViewController father)
    {
        this.father = father;
        UIToggle toggle = this._uiRoot.transform.FindChild("Anim/left/TabSoldier").gameObject.GetComponent<UIToggle>();
        toggle.Set(true);
		EventDelegate.Add(toggle.onChange, Toggle);
        this.RefreshSoldier();
        this.RefreshDebris();
        BtnEventBinding();
        IsFilter = false;
        this.Filter_Button.gameObject.SetActive(true);
        this.FilterCancel_Button.gameObject.SetActive(false);
    }
	public void Toggle ()
	{
		bool val = UIToggle.current.value;

        if(val != isSoldier)
        {
            if(val)
            {
                father.SetOneStepButton(true, true);
                if (needRefreshSoldier)
                    this.InitSoldierItem(false);

                if (soldier != null)
                {
                    father.Refresh(soldier);
                }
                else
                    father.SetNull();
            }
            else
            {
                father.SetOneStepButton(true, false);

                if (needRefreshDebris)
                    this.InitDebrisItem();

                if (DebrisSoldier != null)
                    father.Refresh(DebrisSoldier);
                else
                    father.SetNull();
            }
        }
        isSoldier = val;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this._uiRoot.transform));
	}
    public void OnClose()
    {
        this.debris = null;
        this.soldier = null;
        this.isFirst = false;
    }
    public void SetFilterValue(bool vBool)
    {
        if (this.soldier == null)
            return;
        this.IsFilter = vBool;
        this.Filter_Button.gameObject.SetActive(!this.IsFilter);
        this.FilterCancel_Button.gameObject.SetActive(this.IsFilter);
        this.InitSoldierItem(true);
    }
    public void RefreshSoldier()
    {
        if (this.isSoldier)
            InitSoldierItem(false);
        else
            this.needRefreshSoldier = true;
    }
    public void RefreshDebris()
    {
        if (!this.isSoldier)
            InitDebrisItem();
        else
            this.needRefreshDebris = true;
        this.Debris_Prompt.SetActive(false);
        foreach (Debris de in PlayerData.Instance._SoldierDebrisDepot._SoldierDebrisList)
        {
            if (de.enabelCompound() == DebrisCheck.Ok)
            {
                this.Debris_Prompt.SetActive(true);
                break;
            }
        }
    }
    public  bool SoldierFilter(Soldier sd)
    {
        if(this.IsFilter && this.soldier != null && sd != null)
        {
            if (!sd.Att.Name.Equals(this.soldier.Att.Name))
                return false;
        }
        return true;
    }
    public void InitSoldierItem(bool IsFilter)
    {
        tempList = PlayerData.Instance._SoldierDepot.getSoldierList(SoldierFilter);
        UISystem.Instance.SoldierAttView.ShowSoldierFilterEffect(tempList, IsFilter);
        this.needRefreshSoldier = false;
        if (this.soldier != null)
            this.soldier = PlayerData.Instance._SoldierDepot.FindByUid(this.soldier.uId);
        if(tempList.Count == 0)
        {
            this.soldier = null;
            father.SetNull();
        }

        if (this.SoldierCount != null)
        {
            int num = PlayerData.Instance.GetSoldierListCount();
            if (num >= SoldierDepot.MAXCOUNT)
            {
                this.SoldierCount.color = new Color(1,0.21f,0.21f);
            }
            else
            {
                this.SoldierCount.color = new Color(0.46f, 0.98f, 0.61f);
            }
            this.SoldierCount.text = string.Format("{0}/{1}", num, SoldierDepot.MAXCOUNT);
        }

        Main.Instance.StartCoroutine(CreatSoldierItem(tempList));
    }
   

    private IEnumerator CreatSoldierItem(List<Soldier> _data)
    {
        //ScrollViewSoldier.ResetPosition();

        yield return 0.5;
        WrapContentSoldier.CleanChild();

        int count = _data.Count;
        int itemCount = _soldierItemList.Count;
        int MaxCount = 10;
        int index = Mathf.CeilToInt((float)count / WrapContentSoldier.wideCount) - 1;
        if (index == 0)
            index = 1;
        WrapContentSoldier.minIndex = -index;
        WrapContentSoldier.maxIndex = 0;
        this.WrapContentSoldier.cullContent = true;
        if (count > MaxCount)
        {
            WrapContentSoldier.enabled = true;
            if (count % 2 != 0)
            {
                this.WrapContentSoldier.cullContent = false;
            }
            else
            {
                this.WrapContentSoldier.cullContent = true;
            }
            count = MaxCount;
        }
        else
        {
            WrapContentSoldier.enabled = true;
            WrapContentSoldier.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                _soldierItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(ItemSoldier, Grd_GridSoldier.transform);
                SoldiertemComponent item = vGo.GetComponent<SoldiertemComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<SoldiertemComponent>();
                    item.MyStart(vGo);
                }
                _soldierItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                _soldierItemList[i].TouchEvent += OnSoldierItemTouch;
            }
            else
            {
                if (_soldierItemList[i] != null)
                    _soldierItemList[i].gameObject.SetActive(true);
            }
            _soldierItemList[i].SetInfo(_data[i]);
            GameObject mark = _soldierItemList[i].transform.FindChild("mark").gameObject;
            if (this.soldier == null)
                mark.SetActive(false);
            else
            {
                if (this.soldier != _data[i])
                    mark.SetActive(false);
                else
                {
                    if(LastMarkOb != null)
                        LastMarkOb.SetActive(false);
                    mark.SetActive(true);
                    LastMarkOb = mark;
                }
            }
        }
        if (count > 0 && this.soldier == null)
        {
            GameObject mark = _soldierItemList[0].transform.FindChild("mark").gameObject;
            if(LastMarkOb != null)
            {
                LastMarkOb.SetActive(false);
            }
            else
            {
                LastMarkOb = mark;
            }
            if (mark)
            {
                if (LastMarkOb != null)
                    LastMarkOb.SetActive(false);
                mark.SetActive(true);
                LastMarkOb = mark;
            }
            this.soldier = _data[0];

            this.isFirst = true;
        }
        if (isSoldier)
            father.Refresh(this.soldier);
        if (count > 6)
            WrapContentSoldier.ReGetChild();
        yield return 0;
        Grd_GridSoldier.repositionNow = true;
        yield return 0;
        ScrollViewSoldier.ResetPosition();
        //Grd_GridSoldier.gameObject.SetActive(false);
        //Grd_GridSoldier.gameObject.SetActive(true);
    }
    public void SetSoldierInfo(GameObject go, int wrapIndex, int realIndex)
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
        SoldiertemComponent item = _soldierItemList[wrapIndex];
        item.SetInfo(tempList[realIndex]);
        GameObject mark = item.transform.FindChild("mark").gameObject;
        if(tempList[realIndex] == this.soldier)
        {
            if (LastMarkOb != null)
            {
                LastMarkOb.SetActive(false);
            }
            LastMarkOb = mark;
            if (mark)
                mark.SetActive(true);
        }
        else
        {
            if (mark)
                mark.SetActive(false);
        }
    }
    public void OnSoldierItemTouch(SoldiertemComponent comp)
    {
        if (comp == null) return;
        if (this.soldier != null && this.soldier.uId == comp.uid)
            return;
        Soldier temp = tempList.Find((sd) => { if (sd == null) return false; return sd.uId == comp.uid; });
        if (temp == null) return;

        GameObject mark = comp.transform.FindChild("mark").gameObject;

        if (LastMarkOb != null)
            LastMarkOb.SetActive(false);

        mark.SetActive(true);

        father.Refresh(temp);
        soldier = temp;
        LastMarkOb = mark;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this._uiRoot.transform));

        return;
    }

    public void InitDebrisItem()
    {
        DebrisList = PlayerData.Instance._SoldierDebrisDepot._SoldierDebrisList;
        this.needRefreshDebris = false;
        if(DebrisList.Count == 0)
        {
            father.SetNull();
            this.debris = null;
            this.DebrisSoldier = null;
        }
        this.Debris_Prompt.SetActive(false);
        foreach (Debris de in DebrisList)
        {
            if(de.enabelCompound() == DebrisCheck.Ok)
            {
                this.Debris_Prompt.SetActive(true);
                break;
            }
        }
        Main.Instance.StartCoroutine(CreatDebrisItem(DebrisList));
    }
    private IEnumerator CreatDebrisItem(List<Debris> _data)
    {
        ScrollViewDebris.ResetPosition();
        yield return 0;
        int count = _data.Count;
        int itemCount = _debrisItemList.Count;

        int index = Mathf.CeilToInt((float)count / WrapContentDebris.wideCount) - 1;
        if (index == 0)
            index = 1;
        WrapContentDebris.minIndex = -index;
        WrapContentDebris.maxIndex = 0;

        if (count > MaxSoldierCount)
        {
            WrapContentDebris.enabled = true;
            count = MaxSoldierCount;
        }
        else
        {
            WrapContentDebris.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                _debrisItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(ItemDebris, Grd_GridDebris.transform);
                DebristemComponent item = vGo.GetComponent<DebristemComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<DebristemComponent>();
                    item.MyStart(vGo);
                }
                _debrisItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                _debrisItemList[i].TouchEvent += OnDebrisItemTouch;
            }
            else
            {
                _debrisItemList[i].gameObject.SetActive(true);
            }
            _debrisItemList[i].SetInfo(_data[i]);
            GameObject mark = _debrisItemList[i].transform.FindChild("mark").gameObject;
            if(this.debris == null)
                mark.SetActive(false);
            else
            {
                if (this.debris.Att.id != _debrisItemList[i].id)
                    mark.SetActive(false);
            }
        }

        if (count > 0 && (this.debris == null || this.debris.count == 0))
        {
            GameObject mark = _debrisItemList[0].transform.FindChild("mark").gameObject;
            
            if (LastMarkOb_Debris != null)
            {
                LastMarkOb_Debris.SetActive(false);
            }
            else
            {
                LastMarkOb_Debris = mark;
            }

            if (mark)
                mark.SetActive(true);
            this.debris = _data[0];
            LastMarkOb_Debris = mark;

        }
        if(this.debris != null)
        {
            DebrisSoldier = Soldier.createByID((uint)this.debris.Att.compound_Id);
            if (DebrisSoldier != null)
            {
                foreach (uint id in DebrisSoldier.Att.passivitySkill)
                {
                    DebrisSoldier._skillsDepot.oneAdd(id);
                }
                DebrisSoldier._skillsDepot.oneAdd(DebrisSoldier.Att.initiativeSkill);
            }
        }

        setLable();
        if (!isSoldier)
            father.Refresh(DebrisSoldier);

        WrapContentDebris.ReGetChild();
        Grd_GridDebris.repositionNow = true;
        yield return 0;
        ScrollViewDebris.ResetPosition();
        yield return 0;
        //Grd_GridDebris.gameObject.SetActive(false);
        //Grd_GridDebris.gameObject.SetActive(true);
        Grd_GridDebris.repositionNow = true;
      
    }
    public void SetDebrisInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= DebrisList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        DebristemComponent item = _debrisItemList[wrapIndex];
        item.SetInfo(DebrisList[realIndex]);
        if (DebrisList[realIndex] == this.debris)
        {
            GameObject mark = item.transform.FindChild("mark").gameObject;
            if (mark)
                mark.SetActive(true);
        }
        else
        {
            GameObject mark = item.transform.FindChild("mark").gameObject;
            if (mark)
                mark.SetActive(false);
        }
    }
    public void OnDebrisItemTouch(DebristemComponent temp)
    {
        if (temp == null) return;
        Debris debris = this.DebrisList.Find((db) => { if (db == null)return false; return db.Att.id == temp.id; });
        if (debris == null) return;

        GameObject mark = temp.transform.FindChild("mark").gameObject;


        if (LastMarkOb_Debris != null)
            LastMarkOb_Debris.SetActive(false);

        mark.SetActive(true);

        this.debris = debris;

        DebrisSoldier = Soldier.createByID((uint)this.debris.Att.compound_Id);
        if (DebrisSoldier != null)
        {
            foreach (uint id in DebrisSoldier.Att.passivitySkill)
            {
                DebrisSoldier._skillsDepot.oneAdd(id);
            }
            DebrisSoldier._skillsDepot.oneAdd(DebrisSoldier.Att.initiativeSkill);
        }

        father.Refresh(DebrisSoldier);

        LastMarkOb_Debris = mark;
        setLable();

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this._uiRoot.transform));
        return;
    }
    private void setLable()
    {
        if(this.debris != null)
        {
            if (this.debris.count >= this.debris.Att.compound_count)
                Debris_Lable.color = Color.green;
            else
                Debris_Lable.color = Color.red;

            Debris_Lable.text = string.Format("{0}/{1}",this.debris.count,this.debris.Att.compound_count);
        }
    }
    public void BtnEventBinding()
    {
        WrapContentSoldier.onInitializeItem = SetSoldierInfo;
        WrapContentDebris.onInitializeItem = SetDebrisInfo;
    }
}
