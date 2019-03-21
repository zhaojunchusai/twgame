using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public enum SacrificialDataEnum
{
    SoldierRed = 1,
    Equip = 2,
    SoldierOrange= 3,
    EquipOrange = 4
}

public class SacrificialData
{
    public string icon;
    public int level;
    public int star;
    public int quality;
    public int step = 0;
    public UInt64 uid;
    public bool MyBool;
    public SacrificialDataEnum type;

    public SacrificialData(Soldier sd)
    {
        if(sd == null)
            return;
        this.icon = sd.Att.Icon;
        this.level = sd.Level;
        this.star = sd.Att.Star;
        this.quality = sd.Att.quality;
        this.uid = sd.uId;
        this.MyBool = CommonFunction.IsAlreadyBattle(sd.uId);
        this.step = sd.StepNum;
        this.type = SacrificialDataEnum.SoldierOrange;
    }
    public SacrificialData(Weapon wp)
    {
        if (wp == null)
            return;
        this.icon = wp.Att.icon;
        this.level = wp.Level;
        this.star = wp.Att.star;
        this.quality = wp.Att.quality;
        this.uid = wp.uId;
        this.MyBool = wp.isEquiped;
        this.type = SacrificialDataEnum.Equip;
    }
}
public class SacrificialSystemController : UIBase
{
    public SacrificialSystem view;

    private List<SacrificialComponent> _SacrificialComponentList = new List<SacrificialComponent>();
    List<UInt64> ChooseSolider = new List<ulong>();
    List<UInt64> ChooseEquip = new List<ulong>();
    List<UInt64> ChooseOrangeSoldier = new List<ulong>();
    List<UInt64> ChooseOrangeEquip = new List<ulong>();

    List<UInt64> chooseUid = new List<ulong>();
    List<SacrificialData> MaterialList = new List<SacrificialData>();
    int SummonNum = 0;
    int SummonMoney = 0;
    SacrificialDataEnum type = SacrificialDataEnum.SoldierOrange;
    public Vector3 startPos;
    public int MaxCount = 10;
    public bool isMax;
    static Color color1 = new Color(0.87f, 0.49f, 0.15f);
    static Color color2 = new Color(0.87f, 0.15f, 0.16f);
    static Color color3 = new Color(0.15f, 0.67f, 0.87f);
    public int[] LockData = new int[] { 0, 0, 0 };

    Color[] color = { color1, color2, color3, color1, color2, color3, color1, color2, color3, color1, color2, color3 };
    public override void Initialize()
    {
        if (view == null)
            view = new SacrificialSystem();
        view.Initialize();
        startPos = this.view.UIPanel_MaterialScroll.transform.localPosition;
        BtnEventBinding();
        this.SetInfo();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SACRIFICIAL);
    }

    public void SetInfo()
    {
        this.SetGodSoldier(SacrificialDataEnum.SoldierOrange);
        this.view.Tog_TabSoldier.value = true;
    }
    public void SetGodSoldier(SacrificialDataEnum vType)
    {
        this.type = vType;
        if (vType == SacrificialDataEnum.SoldierRed)
        {
            this.chooseUid = this.ChooseSolider;
            this.view.Tog_TabRed.Set(true);
        }
        else
        {
            this.chooseUid = this.ChooseOrangeSoldier;
            this.view.Tog_TabOrange.Set(true);
        }
        CheckIsOpen();
        this.InitSoldierItem();
        this.view.Lbl_Label_Title.gameObject.SetActive(true);
        this.view.Lbl_Label_Title.text = ConstString.GODSOLDIER_TITLE_RETURN;
        this.view.Lbl_Label_Summon.text = ConstString.GOLDSOLDIER_SUMMON;
        this.OnChoose();
    }
    public void SetGodWeapon(SacrificialDataEnum vType)
    {
        this.type = vType;
        if (vType == SacrificialDataEnum.Equip)
        {
            this.chooseUid = this.ChooseEquip;
            this.view.Tog_TabRed_Equip.Set(true);
        }
        else if(vType == SacrificialDataEnum.EquipOrange)
        {
            this.chooseUid = this.ChooseOrangeEquip;
            this.view.Tog_TabOrange_Equip.Set(true);
        }
        CheckIsOpen();
        this.InitWeaponItem();
        this.view.Lbl_Label_Title.gameObject.SetActive(false);
        this.view.Lbl_Label_Title.text = "";
        this.view.Lbl_Label_Summon.text = ConstString.GOLDEQUIP_SUMMON;
        this.OnChoose();
    }

    public void OnChoose()
    {
        int GoldNum = 0;
        int ExpNum = 0;
        SacrificialSystemInfo tmpInfo = ConfigManager.Instance.mSacrificialData.FindByType((int)this.type);
        if (tmpInfo == null)
            return;
        GoldNum = tmpInfo.gold_num;
        ExpNum = tmpInfo.exp_num;
        int exp = 0;
        switch (this.type)
        {
            case SacrificialDataEnum.Equip: exp = PlayerData.Instance.Sacrificial_Equip_EXP; break;
            case SacrificialDataEnum.SoldierRed: exp = PlayerData.Instance.Sacrificial_Soldier_EXP; break;
            case SacrificialDataEnum.SoldierOrange: exp = PlayerData.Instance.Sacrificial_Ora_Soldier_EXP; break;
            case SacrificialDataEnum.EquipOrange: exp = PlayerData.Instance.Sacrificial_Ora_Equip_EXP; break;
        }
        for (int i = 0; i < this.chooseUid.Count; ++i)
        {
            SacrificialData tmpData = this.MaterialList.Find((data) => { if (data == null)return false; return data.uid == chooseUid[i]; });
            if (tmpData == null)
                continue;
            Dictionary<int, int> valuePick;
            if (tmpData.quality == tmpInfo.Quality_First)
                valuePick = tmpInfo.exp_value_list;
            else
            {
                if (tmpData.quality == tmpInfo.Quality_Sec)
                    valuePick = tmpInfo.exp_value_list2;
                else
                    continue;
            }
            if (tmpData.type == SacrificialDataEnum.SoldierOrange || tmpData.type == SacrificialDataEnum.SoldierRed)
            {
                if (!valuePick.ContainsKey(1))
                    continue;
                SoldierStepInfo tmpStarWorth = ConfigManager.Instance.mSoldierStepData.FindByStarOrStep(tmpData.star);
                SoldierStepInfo tmpStepWorth = ConfigManager.Instance.mSoldierStepData.FindByStarOrStep(tmpData.step);
                int vNum = 0;
                if (tmpStarWorth != null)
                    vNum += tmpStarWorth.starValue;
                if (tmpStepWorth != null)
                    vNum += tmpStepWorth.stepValue;
                exp += valuePick[1] * vNum;
            }
            else
            {
                if (!valuePick.ContainsKey(tmpData.star))
                    continue;
                exp += valuePick[tmpData.star];
            }
        }
        int expShow = exp;

        if (exp >= ExpNum * MaxCount)
        {
            exp = ExpNum * MaxCount;
            isMax = true;
        }
        else
        {
            isMax = false;
        }
        if (isMax)
            this.SetAllGray(true);
        else
            this.SetAllGray(false);
        int num = exp / ExpNum;
        if (this.view.Lbl_Label_CountGroup)
            this.view.Lbl_Label_CountGroup.text = string.Format("X{0}", num);
        this.SummonMoney = num * GoldNum;

        if (this.view.Lbl_Label_CostGroup)
        {
            this.view.Lbl_Label_CostGroup.text = (this.SummonMoney).ToString();
            if (PlayerData.Instance.GoldIsEnough(tmpInfo.money_type, this.SummonMoney))
            {
                this.view.Lbl_Label_CostGroup.color = new Color(0.92f,0.78f,0.37f);
            }
            else
            {
                this.view.Lbl_Label_CostGroup.color = Color.red;
            }

        }
        if(tmpInfo.money_type == 1)
        {
            CommonFunction.SetSpriteName(this.view.Spt_CostIcon, "ZCJ_icon_jinbi_l");
        }
        else
        {
            CommonFunction.SetSpriteName(this.view.Spt_CostIcon, "ZCJ_icon_daibi_l");
        }
        this.SummonNum = num;
        int LeftNum = exp % ExpNum;
        if (LeftNum == 0 && exp != 0)
            --num;
        if (this.view.Spt_SliderProgressBarForeground && this.view.Spt_SliderProgressBarBackColor)
        {
            if(num == 0)
            {
                this.view.Spt_SliderProgressBarBackColor.gameObject.SetActive(false);
            }
            else
            {
                this.view.Spt_SliderProgressBarBackColor.gameObject.SetActive(true);
                this.view.Spt_SliderProgressBarBackColor.color = this.color[num - 1];
            }
            this.view.Spt_SliderProgressBarForeground.color = this.color[num];
        }

        if ((LeftNum == 0 && exp != 0) || isMax)
            LeftNum = ExpNum;
        if (this.view.Slider_ProgressBar)
            this.view.Slider_ProgressBar.value = (float)(LeftNum) / ExpNum;
        if (expShow > ExpNum * MaxCount)
        {
            LeftNum = expShow - ExpNum * (MaxCount - 1);
            if (this.view.Lbl_SliderProgressBarLabel)
                this.view.Lbl_SliderProgressBarLabel.text = string.Format("[ff0000]{0}[-]/[38d452]{1}[-]", LeftNum, ExpNum);
        }
        else
        {
            if (this.view.Lbl_SliderProgressBarLabel)
                this.view.Lbl_SliderProgressBarLabel.text = string.Format("[ffffff]{0}[-]/[38d452]{1}[-]", LeftNum, ExpNum);
        }

    }
    private bool IsExpMax(bool isShow = true)
    {
        SacrificialSystemInfo tmpInfo = ConfigManager.Instance.mSacrificialData.FindByType((int)this.type);        
        string message = ConstString.GOLDSOLDIER_MAX;
        if (type == SacrificialDataEnum.Equip)
            message = ConstString.GOLDEQUIP_MAX;
        if (type == SacrificialDataEnum.SoldierOrange)
            message = ConstString.GOLDSOLDIER_MAX_ORANGE;
        if (type == SacrificialDataEnum.EquipOrange)
            message = ConstString.GOLDEQUIP_MAX_ORANGE;
        if (tmpInfo == null)
        {
            if (isShow)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, message);
            this.isMax = true;
            return true;
        }
        int exp = 0;
        switch(this.type)
        {
            case SacrificialDataEnum.Equip: exp = PlayerData.Instance.Sacrificial_Equip_EXP; break;
            case SacrificialDataEnum.SoldierRed: exp = PlayerData.Instance.Sacrificial_Soldier_EXP; break;
            case SacrificialDataEnum.SoldierOrange: exp = PlayerData.Instance.Sacrificial_Ora_Soldier_EXP; break;
            case SacrificialDataEnum.EquipOrange: exp = PlayerData.Instance.Sacrificial_Ora_Equip_EXP; break;
        }
        for (int i = 0; i < this.chooseUid.Count; ++i)
        {
            SacrificialData tmpData = this.MaterialList.Find((data) => { if (data == null)return false; return data.uid == chooseUid[i]; });
            if (tmpData == null)
                continue;
            Dictionary<int, int> valuePick;
            if (tmpData.quality == tmpInfo.Quality_First)
                valuePick = tmpInfo.exp_value_list;
            else
            {
                if (tmpData.quality == tmpInfo.Quality_Sec)
                    valuePick = tmpInfo.exp_value_list2;
                else
                    continue;
            } 
            if (tmpData.type == SacrificialDataEnum.SoldierOrange || tmpData.type == SacrificialDataEnum.SoldierRed)
            {
                if (!valuePick.ContainsKey(1))
                    continue;
                SoldierStepInfo tmpStarWorth = ConfigManager.Instance.mSoldierStepData.FindByStarOrStep(tmpData.star);
                SoldierStepInfo tmpStepWorth = ConfigManager.Instance.mSoldierStepData.FindByStarOrStep(tmpData.step);
                int vNum = 0;
                if (tmpStarWorth != null)
                    vNum += tmpStarWorth.starValue;
                if (tmpStepWorth != null)
                    vNum += tmpStepWorth.stepValue;
                exp += valuePick[1] * vNum;
            }
            else
            {
                if (!valuePick.ContainsKey(tmpData.star))
                    continue;
                exp += valuePick[tmpData.star];
            }
        }

        if (exp >= tmpInfo.exp_num * MaxCount)
        {
            if (isShow)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, message);
            this.isMax = true;
            return true;
        }
        this.isMax = false;
        return false;
    }
    private void SetAllGray(bool isGary)
    {
        if (this._SacrificialComponentList == null || this._SacrificialComponentList.Count <= 0)
            return;

        for (int i = 0; i < this._SacrificialComponentList.Count;++i )
        {
            SacrificialComponent tmp = this._SacrificialComponentList[i];
            if (tmp == null)
                continue;
            tmp.SetGray(!tmp.Spt_SelectSprite.gameObject.activeSelf && isGary);
        }
    }
    #region 共用滑动层
    public void InitSoldierItem()
    {
        List<Soldier> tmpList = PlayerData.Instance._SoldierDepot.getSoldierList(filter);
        //武将们先按照是否上阵排序，再按等级排序，最后按武将ID排列。
        //是否上阵：从非到是排序
        //等级排序：从小到大
        //武将ID排序：从小到大
        tmpList.RemoveAll((sd) => { return sd == null; });
        tmpList.Reverse();
        //tmpList.Sort((left, right) =>
        //{
        //    if (CommonFunction.IsAlreadyBattle(left.uId) != CommonFunction.IsAlreadyBattle(right.uId))
        //    {
        //        if (!CommonFunction.IsAlreadyBattle(left.uId))
        //            return -1;
        //        else
        //            return 1;
        //    }
        //    if (left.Level != right.Level)
        //    {
        //        if (left.Level < right.Level)
        //            return -1;
        //        else
        //            return 1;
        //    }
        //    if (left.Att.id != right.Att.id)
        //    {
        //        if (left.Att.id < right.Att.id)
        //            return -1;
        //        else
        //            return 1;
        //    }
        //    return 0;
        //});
        if (this.MaterialList == null)
            this.MaterialList = new List<SacrificialData>();
        this.MaterialList.Clear();
        this.MaterialList.Capacity = tmpList.Count + 1;
        for (int i = 0; i < tmpList.Count;++i )
        {
            if (tmpList[i] == null)
                continue;
            this.MaterialList.Add(new SacrificialData(tmpList[i]));
        }
        List<SacrificialData> IsBattle = new List<SacrificialData>();
        List<SacrificialData> myTmpList = new List<SacrificialData>(tmpList.Count);
        for (int i = 0; i < MaterialList.Count; ++i)
        {
            SacrificialData tmpSoldier = MaterialList[i];
            if (tmpSoldier.MyBool)
            {
                IsBattle.Add(tmpSoldier);
                continue;
            }
            myTmpList.Add(tmpSoldier);
        }
        for (int i = 0; i < IsBattle.Count; ++i)
        {
            myTmpList.Add(IsBattle[i]);
        }
        MaterialList = myTmpList;
        Main.Instance.StartCoroutine(CreatSacrificialDataItem(this.MaterialList));
    }
    public void InitWeaponItem()
    {
        List<Weapon> tmpList = PlayerData.Instance._SoldierEquip.getWeaponList(filter);
        //装备们先按照是否上阵排序，再按等级排序，最后按装备ID排列。
        //是否已装备：从非到是排序
        //等级排序：从小到大
        //装备ID排序：从小到大

        tmpList.RemoveAll((sd) => { return sd == null; });
        tmpList.Reverse();
        //tmpList.Sort((left, right) =>
        //{
        //    if (left.isEquiped != right.isEquiped)
        //    {
        //        if (!left.isEquiped)
        //            return -1;
        //        else
        //            return 1;
        //    }
        //    if (left.Level != right.Level)
        //    {
        //        if (left.Level < right.Level)
        //            return -1;
        //        else
        //            return 1;
        //    }
        //    if (left.Att.id != right.Att.id)
        //    {
        //        if (left.Att.id < right.Att.id)
        //            return -1;
        //        else
        //            return 1;
        //    }
        //    return 0;
        //});
        if (this.MaterialList == null)
            this.MaterialList = new List<SacrificialData>();
        this.MaterialList.Clear();
        this.MaterialList.Capacity = tmpList.Count + 1;
        for (int i = 0; i < tmpList.Count; ++i)
        {
            if (tmpList[i] == null)
                continue;
            this.MaterialList.Add(new SacrificialData(tmpList[i]));
        }
        List<SacrificialData> IsBattle = new List<SacrificialData>();
        List<SacrificialData> myTmpList = new List<SacrificialData>(tmpList.Count);
        for (int i = 0; i < MaterialList.Count; ++i)
        {
            SacrificialData tmpSoldier = MaterialList[i];
            if (tmpSoldier.MyBool)
            {
                IsBattle.Add(tmpSoldier);
                continue;
            }
            myTmpList.Add(tmpSoldier);
        }
        for (int i = 0; i < IsBattle.Count; ++i)
        {
            myTmpList.Add(IsBattle[i]);
        }
        MaterialList = myTmpList;
        Main.Instance.StartCoroutine(CreatSacrificialDataItem(this.MaterialList));
    }
    private IEnumerator CreatSacrificialDataItem(List<SacrificialData> _data)
    {
        if(_data == null || _data.Count == 0)
        {
            this.view.Lbl_Label_Tips.gameObject.SetActive(true);
            SacrificialSystemInfo tmpInfo = ConfigManager.Instance.mSacrificialData.FindByType((int)this.type);
            if(tmpInfo != null)
            {
                this.view.Lbl_Label_Tips.text = tmpInfo.noMaterial;
            }
        }
        else
        {
            this.view.Lbl_Label_Tips.gameObject.SetActive(false);
        }
        yield return null;

        this.view.ScrView_MaterialScroll.ResetPosition();

        yield return null;

        this.view.UIWrapContent_MaterialGrid.CleanChild();
        int count = _data.Count;
        int itemCount = _SacrificialComponentList.Count;
        int MaxCount = 16;
        int index = Mathf.CeilToInt((float)count / this.view.UIWrapContent_MaterialGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        this.view.UIWrapContent_MaterialGrid.minIndex = -index;
        this.view.UIWrapContent_MaterialGrid.maxIndex = 0;
        if (count > MaxCount)
        {
            this.view.UIWrapContent_MaterialGrid.enabled = true;
            count = MaxCount;
        }
        else
        {
            this.view.UIWrapContent_MaterialGrid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                _SacrificialComponentList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(this.view.ItemBase, this.view.Grd_MaterialGrid.transform);
                SacrificialComponent item = vGo.GetComponent<SacrificialComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<SacrificialComponent>();
                    item.MyStart(vGo);
                }
                _SacrificialComponentList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                _SacrificialComponentList[i].TouchEvent += OnItemTouch;
            }
            else
            {
                _SacrificialComponentList[i].gameObject.SetActive(true);
            }
        }
        yield return null;
        if (count > MaxCount - 4)
            this.view.UIWrapContent_MaterialGrid.ReGetChild();
        yield return null;
        this.view.Grd_MaterialGrid.repositionNow = true;
        yield return null;
        this.view.ScrView_MaterialScroll.ResetPosition();
        yield return null;

        for (int i = 0; i < this._SacrificialComponentList.Count && i < _data.Count; ++i)
        {
            if (this.chooseUid.Contains(_data[i].uid))
                _SacrificialComponentList[i].Spt_SelectSprite.gameObject.SetActive(true);
            else
                _SacrificialComponentList[i].Spt_SelectSprite.gameObject.SetActive(false);
            _SacrificialComponentList[i].SetInfo(_data[i], this.isMax);
        }
        startPos = this.view.UIPanel_MaterialScroll.transform.localPosition;
    }
    public void SetSoldierInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= this.MaterialList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        SacrificialComponent item = _SacrificialComponentList[wrapIndex];
        if (this.chooseUid.Contains(this.MaterialList[realIndex].uid))
            item.Spt_SelectSprite.gameObject.SetActive(true);
        else
            item.Spt_SelectSprite.gameObject.SetActive(false);
        item.SetInfo(this.MaterialList[realIndex], this.isMax);
    }
    public void OnFastChoose()
    {
        if (this.IsExpMax())
        {
            return;
        }

        Vector3 afterPos = this.view.UIPanel_MaterialScroll.transform.localPosition;
        float increment = Math.Abs(afterPos.y - startPos.y);
        int num = (int)increment / 110;
        int k = 12;
        int j = 0;
        if ((increment % 110) <= 100 && (increment % 110) >= 10)
            k += 4;
        if (increment % 110 >= 100 || increment % 110 == 0 || (num + 3) * 4 >= this.MaterialList.Count)
        {
            if (increment != 0)
            {
                j += 4;
                k += 4;
            }
        }

        var temp = this.view.Grd_MaterialGrid.GetChildList();
        List<Transform> list = new List<Transform>(temp.Count + 1);
        list.AddRange(temp);
        list.Sort((left, right) =>
        {
            if (left.localPosition.y != right.localPosition.y)
            {
                if (left.localPosition.y > right.localPosition.y)
                    return -1;
                else
                    return 1;
            }
            if (left.localPosition.x != right.localPosition.x)
            {
                if (left.localPosition.x < right.localPosition.x)
                    return -1;
                else
                    return 1;
            }

            return 0;
        });

        k = k < list.Count ? k : list.Count;

        for (int i = j; i < k; ++i)
        {
            SacrificialComponent comp = list[i].GetComponent<SacrificialComponent>();
            if (comp == null)
                continue;
            //comp.Spt_SelectSprite.gameObject.SetActive(true);
            if (!chooseUid.Contains(comp.data.uid) && ChooseFilter(comp.data))
            {
                if (this.IsExpMax(false))
                {
                    break;
                }
                comp.Spt_SelectSprite.gameObject.SetActive(true);
                chooseUid.Add(comp.data.uid);
            }
        }
        this.OnChoose();
    }
    public void OnItemTouch(SacrificialComponent com)
    {
        if (!this.CheckIsOpen(true))
            return;

        GameObject mark = com.Spt_SelectSprite.gameObject;
        SacrificialComponent comp = com;
        if (mark.activeSelf)
        {
            mark.SetActive(false);
            chooseUid.Remove(comp.data.uid);
        }
        else
        {
            bool ismax = this.isMax;
            if (this.IsExpMax())
            {
                return;
            }
            mark.SetActive(true);
            chooseUid.Add(comp.data.uid);
        }
        this.OnChoose();
    }

    private bool filter(Soldier sd)
    {
        if (sd == null) return false;
        SacrificialSystemInfo tmpInfo = ConfigManager.Instance.mSacrificialData.FindByType((int)this.type);
        if (sd.Att.quality == tmpInfo.Quality_First || sd.Att.quality == tmpInfo.Quality_Sec) return true;
        return false;
    }
    private bool filter(Weapon sd)
    {
        if (sd == null) return false;
        SacrificialSystemInfo tmpInfo = ConfigManager.Instance.mSacrificialData.FindByType((int)this.type);
        if (sd.Att.quality == tmpInfo.Quality_First || sd.Att.quality == tmpInfo.Quality_Sec) return true;

        return false;
    }
    private bool ChooseFilter(SacrificialData data)
    {
        SacrificialSystemInfo tmpInfo = ConfigManager.Instance.mSacrificialData.FindByType((int)this.type);
        if (data.quality != tmpInfo.Quality_First) return false;
        if (data.star != 1) return false;
        if (data.MyBool) return false;
        return true;
    }
    private void OnSoldierSummon()
    {
        if (this.SummonNum == 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GOLDSOLDIER_MATERIAL_ONTENOUGH);
            return;
        }
        SacrificialSystemInfo tmpSaInfo = ConfigManager.Instance.mSacrificialData.FindByType((int)this.type);

        if (!PlayerData.Instance.GoldIsEnough(tmpSaInfo.money_type, this.SummonMoney))
        {
            if(tmpSaInfo.money_type == 1)
            {
                ErrorCode.ShowErrorTip((int)ErrorCodeEnum.NotEnoughGold);
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
            }
            else
                ErrorCode.ShowErrorTip((int)ErrorCodeEnum.NotEnoughDiamond);
            return;
        }

        List<ulong> tmpChooseSoldier;
        if (this.type == SacrificialDataEnum.SoldierRed)
            tmpChooseSoldier = this.ChooseSolider;
        else
            tmpChooseSoldier = this.ChooseOrangeSoldier;
        if (tmpChooseSoldier == null)
            return;
        if (CommonFunction.GetFullBagsItem(tmpChooseSoldier).Count > 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SKIIBOOK_FULL);
            return;
        }
        if (CommonFunction.CheckSholierLifeSoul(tmpChooseSoldier))
        {
            return;
        }

        for (int i = 0; i < tmpChooseSoldier.Count; ++i)
        {
            UInt64 tmp = tmpChooseSoldier[i];
            if(CommonFunction.IsAlreadyBattle(tmp))
            {
                UISystem.Instance.HintView.ShowMessageBox(
                    MessageBoxType.mb_YesNo,
                    ConstString.GOLDSOLODER_MATERIAL_HADREADYBATTLE,
                    () =>
                    {
                        if (!CheekIsFullBags())
                            SoldierModule.Instance.SendSoldierUpQualityReq(tmpChooseSoldier,this.type);
                    },
                    () =>
                    {
                        
                    },
                    ConstString.HINT_LEFTBUTTON_GOON,
                    ConstString.HINT_RIGHTBUTTON_CANCEL
                    );
                return;
            }
        }
        if (!CheekIsFullBags())
            SoldierModule.Instance.SendSoldierUpQualityReq(tmpChooseSoldier,this.type);
    }
    private bool CheekIsFullBags()
    {
        int equipCount = 0;
        List<ulong> tmpChooseSoldier;
        if (this.type == SacrificialDataEnum.SoldierRed)
            tmpChooseSoldier = this.ChooseSolider;
        else
            tmpChooseSoldier = this.ChooseOrangeSoldier;
        if (tmpChooseSoldier == null)
            return false;

        for (int i = 0; i < tmpChooseSoldier.Count; ++i)
        {
            Soldier tmpSoldier = PlayerData.Instance._SoldierDepot.FindByUid(tmpChooseSoldier[i]);
            if (tmpSoldier == null)
                continue;
            equipCount += tmpSoldier._equippedDepot._EquiptList.FindAll((wp) => { return wp != null; }).Count;
        }
        if (PlayerData.Instance.IsEquipGridOverflow(equipCount))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GOLDSOLDIER_RETURNMATERIAL_MAX);
            return true;
        }
        List<KeyValuePair<uint, int>> returnSkill = CommonFunction.GetSkillReturnBagsItem(tmpChooseSoldier);
        if (returnSkill.Count > 0)
        {
            ItemInfo tmpItem = null;
            int count = 0;
            while(tmpItem == null && count < returnSkill.Count)
            {
                tmpItem = ConfigManager.Instance.mItemData.GetItemInfoByID(returnSkill[count].Key);
                ++count;
            }
            if(tmpItem == null)
                return false;

            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GOLDSOLDIER_RETURNMATERIALSKILL_MAX, tmpItem.name, returnSkill[count - 1].Value));
            return true;
        }
        return false;
    }
    private void OnEquipSummmon()
    {
        if (this.SummonNum == 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GOLDEQUIP_MATERIAL_ONTENOUGH);
            return;
        }
        SacrificialSystemInfo tmpSaInfo = ConfigManager.Instance.mSacrificialData.FindByType((int)this.type);

        if (!PlayerData.Instance.GoldIsEnough(tmpSaInfo.money_type, this.SummonMoney))
        {
            if (tmpSaInfo.money_type == 1)
            {
                ErrorCode.ShowErrorTip((int)ErrorCodeEnum.NotEnoughGold);
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
            }
            else
                ErrorCode.ShowErrorTip((int)ErrorCodeEnum.NotEnoughDiamond);
            return;
        }
        if (this.ChooseEquip == null || this.ChooseOrangeEquip == null)
            return;
        bool hadEquip = false;
        List<KeyValuePair<UInt64, UInt64>> material = new List<KeyValuePair<ulong, ulong>>(this.chooseUid.Count + 1);
        for (int i = 0; i < this.chooseUid.Count; ++i)
        {
            UInt64 tmp = this.chooseUid[i];
            Weapon tmpEquip = PlayerData.Instance._SoldierEquip.FindByUid(tmp);
            if (tmpEquip == null)
                continue;
            UInt64 soldierUid = 0;
            if (tmpEquip.isEquiped)
            {
                hadEquip = true;
                Soldier mySoldier = PlayerData.Instance._SoldierDepot._soldierList.Find
                        (
                        (Soldier sd) =>
                        {
                            if (sd == null) return false;
                            return sd._equippedDepot._EquiptList.Contains(tmpEquip);
                        }
                        );
                if (mySoldier != null)
                    soldierUid = mySoldier.uId;
                else
                    Debug.LogError("can not find soldier " + tmpEquip.Att.name + " UID:" + tmpEquip.uId);
            }
            material.Add(new KeyValuePair<UInt64, UInt64>(tmp, soldierUid));
        }
        if (hadEquip)
        {
            UISystem.Instance.HintView.ShowMessageBox(
                MessageBoxType.mb_YesNo,
                ConstString.GOLDEQUIP_MATERIAL_HADREADYBATTLE,
                () => { EquipModule.Instance.SendEquipUpQualityReq(material,this.type); },
                () => { },
                ConstString.HINT_LEFTBUTTON_GOON,
                ConstString.HINT_RIGHTBUTTON_CANCEL
                );
            return;
        }
        EquipModule.Instance.SendEquipUpQualityReq(material, this.type);
    }
    #endregion
    public void ButtonEvent_FastEquipButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        if (!this.CheckIsOpen(true))
            return;

        this.OnFastChoose();
    }

    public void ButtonEvent_Button_close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        Close(null,null);
    }

    public void ButtonEvent_RuleButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        string title = (this.type == SacrificialDataEnum.Equip || this.type == SacrificialDataEnum.EquipOrange) ? ConstString.GODEQUIP_TITLE : ConstString.GODSOLDIER_TITLE;
        SacrificialSystemInfo tmpInfo = ConfigManager.Instance.mSacrificialData.FindByType((int)this.type);

        string message = "";
        if (tmpInfo != null)
            message = tmpInfo.desc;
        UISystem.Instance.HintView.ShowRuleHintView(title,message);
    }

    public void ButtonEvent_SummonButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        if (!this.CheckIsOpen(true))
            return;

        switch(this.type)
        {
            case SacrificialDataEnum.Equip: this.OnEquipSummmon(); break;
            case SacrificialDataEnum.SoldierOrange: this.OnSoldierSummon(); break;
            case SacrificialDataEnum.SoldierRed: this.OnSoldierSummon(); break;
            case SacrificialDataEnum.EquipOrange: this.OnEquipSummmon(); break;
        }
    }
    public void ButtonEvent_Tog_TabSoldier(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        this.SetGodSoldier(SacrificialDataEnum.SoldierOrange);
        this.IsExpMax(false);
    }
    public void ButtonEvent_Tog_TabEquips(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        this.SetGodWeapon(SacrificialDataEnum.EquipOrange);
        this.IsExpMax(false);
    }
    public void ButtonEvent_Tog_TabRed(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        this.SetGodSoldier(SacrificialDataEnum.SoldierRed);
        this.IsExpMax(false);
    }
    public void ButtonEvent_Tog_TabOrange(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        this.SetGodSoldier(SacrificialDataEnum.SoldierOrange);
        this.IsExpMax(false);
    }
    public void ButtonEvent_Tog_TabRed_Equip(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        this.SetGodWeapon(SacrificialDataEnum.Equip);
        this.IsExpMax(false);
    }
    public void ButtonEvent_Tog_TabOrange_Equip(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        this.SetGodWeapon(SacrificialDataEnum.EquipOrange);
        this.IsExpMax(false);
    }

	public void SetToggle(bool left)
    {
        if(left)
        {
            this.SetGodSoldier(SacrificialDataEnum.SoldierOrange);
            this.IsExpMax(false);
            this.view.Tog_TabSoldier.Set(left);
        }
        else
        {
            this.SetGodWeapon(SacrificialDataEnum.EquipOrange);
            this.IsExpMax(false);
            this.view.Tog_TabDebirs.Set(!left);
        }

    }
    public bool CheckIsOpen(bool isShow = false)
    {
        bool result = true;
        string message = "";
        switch(this.type)
        {
            case SacrificialDataEnum.Equip:
                result = ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.SacrificialSystem_EquipRed, isShow);
                message = ConfigManager.Instance.mOpenLevelConfig.GetOpenStr(OpenFunctionType.SacrificialSystem_EquipRed);
                break;
            case SacrificialDataEnum.SoldierOrange:
                result = ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.SacrificialSystem_SoldierOrange, isShow);
                message = ConfigManager.Instance.mOpenLevelConfig.GetOpenStr(OpenFunctionType.SacrificialSystem_SoldierOrange);
                break;
            case SacrificialDataEnum.SoldierRed:
                result = ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.SacrificialSystem_SoldierRed, isShow);
                message = ConfigManager.Instance.mOpenLevelConfig.GetOpenStr(OpenFunctionType.SacrificialSystem_SoldierRed);
                break;
            case SacrificialDataEnum.EquipOrange:
                result = ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.SacrificialSystem_EquipOrange, isShow);
                message = ConfigManager.Instance.mOpenLevelConfig.GetOpenStr(OpenFunctionType.SacrificialSystem_EquipOrange);
                break;
        }
        if (!result)
        {
            this.view.Lb_LockDep.text = message;
        }
        this.view.Go_MaskGroup.SetActive(!result);
        CommonFunction.SetGameObjectGray(view.Btn_FastEquipButton.gameObject, !result);
        CommonFunction.SetGameObjectGray(view.Btn_SummonButton.gameObject, !result);
        return result;
    }

    public override void Uninitialize()
    {
        this.MaterialList.Clear();
        this.ChooseSolider.Clear();
        this.ChooseEquip.Clear();
        this.ChooseOrangeEquip.Clear();
        this.ChooseOrangeSoldier.Clear();
        PlayerData.Instance._SoldierDepot.SoldierErrorEvent -= _SoldierDepot_SoldierErrorEvent;
        PlayerData.Instance._SoldierEquip.ErrotDeleteEvent -= _SoldierEquip_ErrotDeleteEvent;
        PlayerData.Instance.UpdatePlayerGoldEvent -= Instance_UpdatePlayerGoldEvent;
    }
    public override void Destroy()
    {
        base.Destroy();
        this.MaterialList.Clear();
        this.ChooseSolider.Clear();
        this.ChooseEquip.Clear();
        this.ChooseOrangeEquip.Clear();
        this.ChooseOrangeSoldier.Clear();
        this._SacrificialComponentList.Clear();
        PlayerData.Instance._SoldierDepot.SoldierErrorEvent -= _SoldierDepot_SoldierErrorEvent;
        PlayerData.Instance._SoldierEquip.ErrotDeleteEvent -= _SoldierEquip_ErrotDeleteEvent;
        PlayerData.Instance.UpdatePlayerGoldEvent -= Instance_UpdatePlayerGoldEvent;
    }
    public void BtnEventBinding()
    {
        this.view.UIWrapContent_MaterialGrid.onInitializeItem = this.SetSoldierInfo;
        PlayerData.Instance._SoldierDepot.SoldierErrorEvent += _SoldierDepot_SoldierErrorEvent;
        PlayerData.Instance._SoldierEquip.ErrotDeleteEvent += _SoldierEquip_ErrotDeleteEvent;
        PlayerData.Instance.UpdatePlayerGoldEvent += Instance_UpdatePlayerGoldEvent;
        UIEventListener.Get(view.Btn_FastEquipButton.gameObject).onClick = ButtonEvent_FastEquipButton;
        UIEventListener.Get(view.Btn_Button_close.gameObject).onClick = ButtonEvent_Button_close;
        UIEventListener.Get(view.Btn_RuleButton.gameObject).onClick = ButtonEvent_RuleButton;
        UIEventListener.Get(view.Btn_SummonButton.gameObject).onClick = ButtonEvent_SummonButton;
        UIEventListener.Get(view.Tog_TabSoldier.gameObject).onClick = ButtonEvent_Tog_TabSoldier;
        UIEventListener.Get(view.Tog_TabDebirs.gameObject).onClick = ButtonEvent_Tog_TabEquips;
        UIEventListener.Get(view.Tog_TabRed.gameObject).onClick = ButtonEvent_Tog_TabRed;
        UIEventListener.Get(view.Tog_TabOrange.gameObject).onClick = ButtonEvent_Tog_TabOrange;
        UIEventListener.Get(view.Tog_TabRed_Equip.gameObject).onClick = ButtonEvent_Tog_TabRed_Equip;
        UIEventListener.Get(view.Tog_TabOrange_Equip.gameObject).onClick = ButtonEvent_Tog_TabOrange_Equip;

    }

    void Instance_UpdatePlayerGoldEvent()
    {
        if (this.view.Lbl_Label_CostGroup)
        {
            if (PlayerData.Instance.GoldIsEnough(1, this.SummonMoney))
            {
                this.view.Lbl_Label_CostGroup.color = new Color(0.92f, 0.78f, 0.37f);
            }
            else
            {
                this.view.Lbl_Label_CostGroup.color = Color.red;
            }

        }
    }

    void _SoldierEquip_ErrotDeleteEvent(EquipControl control, int errorCode)
    {
        if (control == EquipControl.EquipUpQualityResp && errorCode == 0)
        {
            this.ChooseEquip.Clear();
            this.ChooseOrangeEquip.Clear();
            this.SetGodWeapon(this.type);
        }
    }

    void _SoldierDepot_SoldierErrorEvent(SoldierControl control, int errorCode, ulong uid = 0)
    {
        if (control == SoldierControl.SoldierUpQualityResp && errorCode == 0)
        {
            this.ChooseSolider.Clear();
            this.ChooseOrangeSoldier.Clear();
            this.SetGodSoldier((SacrificialDataEnum)uid);
        }
    }
    public void SetEffectActive(bool IsActive)
    {

        view.GO_GrayBackSpt.SetActive(IsActive);
       // view.GO_SoldierBackSpt.SetActive(IsActive);
    }


}
