using System.Collections.Generic;
using UnityEngine;
using System;
public class SuitEquipAttViewController : UIBase
{
    private class SuitEquipAtt
    {
        public EquipCoordEnum type;
        public int att;
        public int current;
        public int need;
    }
    private SuitEquipAttView view;
    private string[] equipColors = new string[] { "[37b045]", "[31a1cb]", "[b652db]", "[e76b28]" };
    private List<SuitEquipAttComponent> suitEquipAtt_dic;
    private int Bgcount = 0;
    private int BaseHeight = 260;
    private int BaseHeight_Suit = 194;
    private int BaseWidth = 444;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new SuitEquipAttView();
            view.Initialize();
            suitEquipAtt_dic = new List<SuitEquipAttComponent>();
        }
    }
    public override void Uninitialize()
    {
        base.Uninitialize();
    }
    public override void Destroy()
    {
        base.Destroy();
        suitEquipAtt_dic.Clear();
        this.view = null;
    }
    public void UpdateInfo(Soldier soldier)
    {
        if (soldier == null) return;
        Bgcount = 0;
        CommonFunction.SetSpriteName(this.view.Spt_Attribute, soldier.Att.AttributeBias);
        Dictionary<uint, SuitEquipedData> dic = new Dictionary<uint, SuitEquipedData>();
        for (int i = 0; i < soldier._equippedDepot._EquiptList.Count; i++)   //首先得到神装装备的套装散件
        {
            Weapon weapon = soldier._equippedDepot._EquiptList[i];
            if (weapon == null)
                continue;
            if (weapon.Att.CoordID == 0)
                continue;
            if (dic.ContainsKey(weapon.Att.CoordID))
            {
                dic[weapon.Att.CoordID].num++;
            }
            else
            {
                SuitEquipedData data = new SuitEquipedData();
                data.soldierUID = soldier.uId;
                data.suitID = weapon.Att.CoordID;
                data.num = 1;
                dic.Add(weapon.Att.CoordID, data);
            }
        }
        Dictionary<string, List<string>> attdesc_dic = new Dictionary<string, List<string>>();
        foreach (KeyValuePair<uint, SuitEquipedData> tmp in dic)
        {
            if (tmp.Equals(null) || tmp.Value.Equals(null))
                continue;
            EquipCoordinatesInfo info = ConfigManager.Instance.mEquipCoordinatesConfig.GetEquipCoordinatesInfoByID(tmp.Key);
            if (info.Equals(null) || info.attributes.Equals(null))
                continue;
            List<string> list = new List<string>();
            bool status = false;  //记录当前是否满足基本条件  满足基本条件的才进入计算
            for (int i = 0; i < info.attributes.Count; i++)
            {
                EquipCoordAttribute att = info.attributes[i];
                if (att == null)
                    continue;
                if (att.list == null || att.list.Count <= 0)
                    continue;
                if (att.condition <= tmp.Value.num) //判断身上的套装散件是否满足条件 add by taiwei
                {
                    status = true;   //由于已经经过排序 因而第一个必然是最小条件
                    string colorStr = string.Empty;
                    if (i < equipColors.Length)
                    {
                        colorStr = equipColors[i];
                    }
                    else
                    {
                        colorStr = equipColors[0];
                    }
                    for (int j = 0; j < att.list.Count; j++)
                    {
                        EquipCoordDetailAttribute tmp_att = att.list[j];
                        if (tmp_att == null)
                            continue;
                        System.Text.StringBuilder sub = new System.Text.StringBuilder();
                        sub.Append(colorStr);
                        switch (tmp_att.type)
                        {
                            case EquipCoordEnum.HP:
                                {
                                    sub.Append(ConstString.hp_max);
                                }
                                break;
                            case EquipCoordEnum.Attack:
                                {
                                    sub.Append(ConstString.phy_atk);
                                }
                                break;
                            case EquipCoordEnum.Crit:
                                {
                                    sub.Append(ConstString.crt_rate);
                                }
                                break;
                            case EquipCoordEnum.Dodge:
                                {
                                    sub.Append(ConstString.ddg_rate);
                                }
                                break;
                            case EquipCoordEnum.Hit:
                                {
                                    sub.Append(ConstString.acc_rate);
                                }
                                break;
                            case EquipCoordEnum.Uprising:
                                {
                                    sub.Append(ConstString.tnc_rate);
                                }
                                break;
                        }
                        sub.Append("+" + tmp_att.att.ToString());
                        int count = tmp.Value.num;
                        if (count > att.condition)
                        {
                            count = att.condition;
                        }
                        sub.Append("(" + count.ToString() + "/" + att.condition.ToString() + ")[-]");
                        list.Add(sub.ToString());
                    }

                }
                else
                {
                    if (status)
                    {
                        for (int j = 0; j < att.list.Count; j++)
                        {
                            EquipCoordDetailAttribute tmp_att = att.list[j];
                            if (tmp_att == null)
                                continue;
                            System.Text.StringBuilder sub = new System.Text.StringBuilder();
                            sub.Append("[968269]");
                            switch (tmp_att.type)
                            {
                                case EquipCoordEnum.HP:
                                    {
                                        sub.Append(ConstString.hp_max);
                                    }
                                    break;
                                case EquipCoordEnum.Attack:
                                    {
                                        sub.Append(ConstString.phy_atk);
                                    }
                                    break;
                                case EquipCoordEnum.Crit:
                                    {
                                        sub.Append(ConstString.crt_rate);
                                    }
                                    break;
                                case EquipCoordEnum.Dodge:
                                    {
                                        sub.Append(ConstString.ddg_rate);
                                    }
                                    break;
                                case EquipCoordEnum.Hit:
                                    {
                                        sub.Append(ConstString.acc_rate);
                                    }
                                    break;
                                case EquipCoordEnum.Uprising:
                                    {
                                        sub.Append(ConstString.tnc_rate);
                                    }
                                    break;
                            }
                            sub.Append("+" + tmp_att.att.ToString());
                            int count = tmp.Value.num;
                            if (count > att.condition)
                            {
                                count = att.condition;
                            }
                            sub.Append("(" + count.ToString() + "/" + att.condition.ToString() + ")[-]");
                            list.Add(sub.ToString());
                        }
                    }
                }
                if (status)
                {
                    if (attdesc_dic.ContainsKey(info.name))
                    {
                        attdesc_dic.Remove(info.name);
                    }
                    attdesc_dic.Add(info.name, list);
                }
            }
        }

        Bgcount = attdesc_dic.Count;
        UpdateAtt(attdesc_dic);
        UpdateBGHeight();
    }

    private void UpdateAtt(Dictionary<string, List<string>> att_dic)
    {
        if (suitEquipAtt_dic == null)
            suitEquipAtt_dic = new List<SuitEquipAttComponent>();
        int obj_count = suitEquipAtt_dic.Count;
        int data_count = att_dic.Count;
        if (data_count < obj_count)
        {
            for (int i = obj_count - data_count; i < obj_count; i++)
            {
                SuitEquipAttComponent comp = suitEquipAtt_dic[i];
                if (comp.Equals(null))
                    continue;
                comp.mRootObject.SetActive(false);
            }
        }
        int index = 0;
        foreach (KeyValuePair<string, List<string>> tmp in att_dic)
        {
            if (tmp.Equals(null))
                continue;
            SuitEquipAttComponent comp = null;
            if (index < obj_count)
            {
                comp = suitEquipAtt_dic[index];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(this.view.Gobj_SuitAttComp, this.view.Grid_Att.transform);
                if (index > 0)
                    go.name = "suitEquipAtt_" + (index + 1).ToString();
                else
                {
                    go.name = "suitEquipAtt_" + index.ToString();
                }
                comp = new SuitEquipAttComponent();
                comp.MyStart(go);
                suitEquipAtt_dic.Add(comp);
            }
            if (comp == null)
                continue;
            comp.UpdateInfo(tmp.Key, tmp.Value);
            comp.mRootObject.SetActive(true);
            index++;
        }
        this.view.Grid_Att.repositionNow = true;
    }

    private void UpdateBGHeight()
    {
        if (Bgcount > 1)
        {
            this.view.Spt_PanelBg.width = BaseWidth + (180) * (Bgcount - 1);
        }
        else
        {
            if (Bgcount <= 0)
            {
                this.view.Spt_PanelBg.width = BaseWidth / 2 + 23;
                this.view.Gobj_SuitGroup.SetActive(false);
                this.view.Spt_Light.gameObject.SetActive(false);
            }
            else
            {
                this.view.Spt_PanelBg.width = BaseWidth;
                this.view.Gobj_SuitGroup.SetActive(true);
                this.view.Spt_Light.gameObject.SetActive(true);
            }
        }
    }
}

public class SuitEquipAttComponent : BaseComponent
{
    private UILabel Lbl_SuitTitle;
    private UILabel Lbl_Desc;
    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Lbl_SuitTitle = root.transform.FindChild("TitleGroup/Title").GetComponent<UILabel>();
        Lbl_Desc = root.transform.FindChild("Desc").GetComponent<UILabel>();
    }

    public void UpdateInfo(string title, List<string> attList)
    {
        Lbl_SuitTitle.text = title;
        System.Text.StringBuilder sub = new System.Text.StringBuilder();
        for (int i = 0; i < attList.Count; i++)
        {
            string att = attList[i];
            if (string.IsNullOrEmpty(att))
                continue;
            sub.Append(att);
            if (i < attList.Count - 1)
            {
                sub.Append("\r\n");
            }
        }
        Lbl_Desc.text = sub.ToString();
    }
}


public class SuitEquipedData
{
    public UInt64 soldierUID;
    public uint suitID;
    public int num;
}