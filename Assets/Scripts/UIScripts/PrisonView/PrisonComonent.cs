using UnityEngine;
using System;
using System.Collections;
using Assets.Script.Common;
public class PrisonComonent : MonoBehaviour
{
    public GameObject _uiRoot;

    public UISprite Spt_Back;
    public UISprite Spt_Back1;
    public UISprite Spt_Back2;

    public UISprite Spt_Sprite_Icon;
    public UISprite Spt_Sprite_Quality;
    public UISprite Spt_Sprite_Quality_Back;
    public UISprite Sprite_Null;

    public UILabel Lbl_Label_Level;
    public UILabel Lbl_Label_Name;
    public UILabel Lbl_Label_Title_h;
    public UILabel Lbl_Label_Title_had;
    public UISprite Spt_Title_h_Sprite;
    public UISprite Spt_Title_had_Sprite;

    public UILabel Lbl_Label_get_had;
    public UILabel Lbl_Label_get_h;
    public UILabel Lbl_Label_Timer;

    public UIButton Btn_Button_Prison;
    public UILabel Lbl_BtnButton_PrisonLabel;
    public UISprite Spt_BtnButton_PrisonBackground;

    public UIButton Btn_Button_Close;

    public GameObject LockGroup;
    public GameObject LockBack;
    public UILabel Lbl_Label_Lock;
    public GameObject NullGroup;


    private long time;
    private long last_collect_time;
    private int currentMoney;
    private long MaxTime = 86400;
    public fogs.proto.msg.PrisonInfo tmpInfo;
    public int tmpIndex;

    public delegate void _PrisonDeleget(PrisonComonent comp);
    public _PrisonDeleget PrisonEvent;

    public delegate void CloseDeleget(PrisonComonent comp);
    public CloseDeleget CloseEvent;

    private Color NameLabel = new Color();
    private Color TimeLabel = new Color();
    private Color ButtonLabel = new Color();
    private Color Gray = new Color(0.16f,0.16f,0.16f);
    public void MyStart(GameObject root)
    {
        this._uiRoot = root;
        if (_uiRoot == null)
            return;

        Spt_Sprite_Icon = _uiRoot.transform.FindChild("IconGroup/Sprite_Icon").gameObject.GetComponent<UISprite>();
        Spt_Sprite_Quality = _uiRoot.transform.FindChild("IconGroup/Sprite_Quality").gameObject.GetComponent<UISprite>();
        Spt_Sprite_Quality_Back = _uiRoot.transform.FindChild("IconGroup/Sprite_Quality_Back").gameObject.GetComponent<UISprite>();
        Sprite_Null = _uiRoot.transform.FindChild("IconGroup/Sprite_Null").gameObject.GetComponent<UISprite>();

        Lbl_Label_Level = _uiRoot.transform.FindChild("NameGroup/Label_Level").gameObject.GetComponent<UILabel>();
        Lbl_Label_Name = _uiRoot.transform.FindChild("NameGroup/Label_Name").gameObject.GetComponent<UILabel>();

        Lbl_Label_get_had = _uiRoot.transform.FindChild("InfoGroup/Label_get_had/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_get_h = _uiRoot.transform.FindChild("InfoGroup/Label_get_h/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_Timer = _uiRoot.transform.FindChild("InfoGroup/Label_Timer").gameObject.GetComponent<UILabel>();

        NullGroup = _uiRoot.transform.FindChild("InfoGroup/NullGroup").gameObject;
        Btn_Button_Prison = _uiRoot.transform.FindChild("ButtonGroup/Button_Prison").gameObject.GetComponent<UIButton>();
        Lbl_BtnButton_PrisonLabel = _uiRoot.transform.FindChild("ButtonGroup/Button_Prison/Label").gameObject.GetComponent<UILabel>();
        Spt_BtnButton_PrisonBackground = _uiRoot.transform.FindChild("ButtonGroup/Button_Prison/Background").gameObject.GetComponent<UISprite>();

        Btn_Button_Close = _uiRoot.transform.FindChild("ButtonGroup/Button_Close").gameObject.GetComponent<UIButton>();

        LockGroup = _uiRoot.transform.FindChild("LockGroup").gameObject;
        LockBack = _uiRoot.transform.FindChild("LockGroup/Sprite").gameObject;
        LockBack.gameObject.SetActive(false);
        Lbl_Label_Lock = _uiRoot.transform.FindChild("LockGroup/Label_Lock").gameObject.GetComponent<UILabel>();

        Spt_Back = _uiRoot.transform.FindChild("BackGroup/Back").gameObject.GetComponent<UISprite>();
        Spt_Back1 = _uiRoot.transform.FindChild("BackGroup/Back1").gameObject.GetComponent<UISprite>();
        Spt_Back2 = _uiRoot.transform.FindChild("BackGroup/Back2").gameObject.GetComponent<UISprite>();
        Lbl_Label_Title_h = _uiRoot.transform.FindChild("InfoGroup/Label_get_h").gameObject.GetComponent<UILabel>();
        Lbl_Label_Title_had = _uiRoot.transform.FindChild("InfoGroup/Label_get_had").gameObject.GetComponent<UILabel>();
        Spt_Title_h_Sprite = _uiRoot.transform.FindChild("InfoGroup/Label_get_had/Sprite").gameObject.GetComponent<UISprite>();
        Spt_Title_had_Sprite = _uiRoot.transform.FindChild("InfoGroup/Label_get_h/Sprite").gameObject.GetComponent<UISprite>();

        string time = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_PRSION_FREETIME);
        if (!string.IsNullOrEmpty(time))
            this.MaxTime = long.Parse(time);
        UIEventListener.Get(this.Btn_Button_Prison.gameObject).onClick = OnPrison;
        UIEventListener.Get(this.Btn_Button_Close.gameObject).onClick = OnClose;
        UIEventListener.Get(this.LockBack).onClick = OnLock;

        this.NameLabel = Lbl_Label_Name.color;
        this.TimeLabel = Lbl_Label_Timer.color;
        this.ButtonLabel = Lbl_BtnButton_PrisonLabel.color;

    }
    public void OnPrison(GameObject btn)
    {
        if (PrisonEvent != null)
            PrisonEvent(this);
    }
    public void OnClose(GameObject btn)
    {
        if (CloseEvent != null)
            CloseEvent(this);
    }
    public void OnLock(GameObject btn)
    {
        OpenFunctionType tmpType = OpenFunctionType.None;
        if (this.tmpInfo == null)
            return;

        switch (this.tmpInfo.prison_id)
        {
            case 2: tmpType = OpenFunctionType.SlaveSlot2; break;
            case 3: tmpType = OpenFunctionType.SlaveSlot3; break;
            case 4: tmpType = OpenFunctionType.SlaveSlot4; break;
        }
        if (tmpType == OpenFunctionType.None)
            return;
        CommonFunction.CheckIsOpen(tmpType,true);
    }
    public void SetInfo(fogs.proto.msg.PrisonInfo info, int index)
    {
        this.tmpInfo = info;
        this.tmpIndex = index;
        if (this.tmpInfo == null || this.tmpInfo.status == 1)
        {
            if (info.locked == 1)
                this.SetNull();
            else
                this.SetLock();
        }
        else
        {
            this.SetHadInfo(this.tmpInfo);
        }
    }
    private void SetHadInfo(fogs.proto.msg.PrisonInfo info)
    {
        CommonFunction.SetGameObjectGray(this._uiRoot, false);
        SetLabelGray(false);
        this.LockGroup.SetActive(false);
        this.NullGroup.SetActive(true);
        this.Sprite_Null.gameObject.SetActive(false);
        this.Btn_Button_Close.gameObject.SetActive(true);
        this.Spt_Sprite_Icon.gameObject.SetActive(true);
        if (Spt_Sprite_Quality != null && Spt_Sprite_Icon != null)
            CommonFunction.SetHeadAndFrameSprite(Spt_Sprite_Icon, Spt_Sprite_Quality, this.tmpInfo.icon, this.tmpInfo.frame, true);

        this.Lbl_Label_Name.text = info.name;
        this.Lbl_Label_Level.text = string.Format("Lv.{0}",info.level);
        float num = PlayerData.Instance._Prison.GetPrisonInfo().status == 1 ? 1 : 0.6f;

        this.Lbl_Label_get_h.text = CommonFunction.GetTenThousandUnit((int)(ConfigManager.Instance.mSlaveIncomeConfig.FindAllByLv(this.tmpInfo.level).money * 6 * num));
        this.Lbl_Label_get_had.text = CommonFunction.GetTenThousandUnit((int)info.current_money);
        this.currentMoney = (int)info.current_money;
        if ((Main.mTime - (long)info.start_time) < 0)
        {
            this.time = Main.mTime;
        }
        else
        {
            this.time = (long)info.start_time;
        }
        this.last_collect_time = Main.mTime;
        this.Lbl_Label_Timer.text = SecondToTime();
        this.StartTime();
        this.Lbl_BtnButton_PrisonLabel.text = ConstString.PRISON_DO_GET;
    }
    private void SetNull()
    {
        CommonFunction.SetGameObjectGray(this._uiRoot, false);
        SetLabelGray(false);
        this.LockGroup.SetActive(false);
        this.NullGroup.SetActive(false);
        this.Btn_Button_Close.gameObject.SetActive(false);
        this.Spt_Sprite_Icon.gameObject.SetActive(false);
        this.Sprite_Null.gameObject.SetActive(true);
        if (Spt_Sprite_Quality != null && Spt_Sprite_Icon != null)
            CommonFunction.SetSpriteName(Spt_Sprite_Quality, "Cmn_BG_Big");

        this.Lbl_Label_Name.text = ConstString.PRISON_DO_NULL;
        this.Lbl_Label_Level.text = "";

        this.Lbl_Label_get_h.text = ConstString.PRISON_DO_EMPTY;
        this.Lbl_Label_get_had.text = ConstString.PRISON_DO_EMPTY;
        this.time = 0;
        this.Lbl_Label_Timer.text = this.SecondToTime();
        this.Lbl_BtnButton_PrisonLabel.text = ConstString.PRISON_DO_PRISON;
    }
    private void SetLock()
    {
        this.SetNull();
        CommonFunction.SetGameObjectGray(this._uiRoot,true);
        SetLabelGray(true);
        this.LockGroup.SetActive(true);

        OpenFunctionType tmpType = OpenFunctionType.None;
        if (this.tmpInfo == null)
            return;

        switch(this.tmpInfo.prison_id)
        {
            case 2: tmpType = OpenFunctionType.SlaveSlot2; break;
            case 3: tmpType = OpenFunctionType.SlaveSlot3; break;
            case 4: tmpType = OpenFunctionType.SlaveSlot4; break;
        }
        if (tmpType == OpenFunctionType.None)
            return;

        OpenLevelData data = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(tmpType);
        string Level = "";
        string vip = "";
        string gate = "";
        if (data.openLevel != -1)
        {
            if (PlayerData.Instance._Level < data.openLevel)
            {
                Level = string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_LV, data.openLevel);
            }
        }
        if (data.vipLevel != -1)
        {
            if (PlayerData.Instance._VipLv < data.vipLevel)
            {
                vip = string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_VIP, data.vipLevel);
            }
        }
        if (data.gateId != -1)
        {
            if (!PlayerData.Instance.IsPassedGate((uint)data.gateId))
            {
                StageInfo info = ConfigManager.Instance.mStageData.GetInfoByID((uint)data.gateId);
                if (info != null)
                {
                    gate = string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_GATE, info.GateSequence);
                }
            }
        }
        if (!string.IsNullOrEmpty(Level) || !string.IsNullOrEmpty(vip) || !string.IsNullOrEmpty(gate))
        {
            vip = string.IsNullOrEmpty(Level) || string.IsNullOrEmpty(vip) ? vip : ConstString.GATE_SWEEPTIP_EITHER + vip;
            gate = string.IsNullOrEmpty(vip) || string.IsNullOrEmpty(gate) ? gate : ConstString.GATE_SWEEPTIP_EITHER + gate;
            this.Lbl_Label_Lock.text = Level + vip + gate + ConstString.GATE_ESCORT_LOCKTIP_UNLOCK;
        }
    }
    private void StartTime()
    {
        Scheduler.Instance.AddTimer(1.0f,true,this.DeletOne);
    }
    public void EndTime()
    {
        Scheduler.Instance.RemoveTimer(this.DeletOne);
    }
    private string SecondToTime()
    {
        if (this.time <= 0)
            return "--:--:--";
        //System.DateTime time = Main.mTime;
        //System.TimeSpan nowTime = time - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long result = Main.mTime - this.time;
        result = this.MaxTime - result;
        if (result <= 0)
            return "--:--:--";
        long h = result / 3600;
        long m = (result % 3600) / 60;
        long s = (result % 3600) % 60;
        if (h >= 24)
            return string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
        this.UpdateGold(Main.mTime);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
    }
    private void UpdateGold(long now)
    {
        DateTime startTime = CommonFunction.GetDateTime(this.time);
        DateTime lastTime = CommonFunction.GetDateTime(this.last_collect_time);
        DateTime nowTime = CommonFunction.GetDateTime(now);

        TimeSpan tt = lastTime - startTime;
        TimeSpan gg = nowTime - startTime;

        long num = (((gg.Minutes) / 10) - ((tt.Minutes) / 10)) + 6 * (gg.Hours - tt.Hours);
        if(num >= 1)
        {
            this.last_collect_time = now;

            int GoldAdd = ConfigManager.Instance.mSlaveIncomeConfig.FindAllByLv(this.tmpInfo.level).money * (int)num;
            if (PlayerData.Instance._Prison.GetPrisonInfo().status == 2)
                GoldAdd = (int)((float)GoldAdd * 0.6);
            this.Lbl_Label_get_had.text = CommonFunction.GetTenThousandUnit(this.currentMoney + GoldAdd);
            UISystem.Instance.PrisonView.TotalMoney = GoldAdd;
        }
    }
    private void DeletOne()
    {
        if (this.time <= 0)
            this.EndTime();
        this.Lbl_Label_Timer.text = SecondToTime();
        return;
    }
    private void SetLabelGray(bool isGray)
    {
        Lbl_Label_Name.color = isGray ? Gray : this.NameLabel;
        Lbl_Label_Timer.color = isGray ? Gray : this.TimeLabel;
        Lbl_BtnButton_PrisonLabel.color = isGray ? Gray : this.ButtonLabel;
        Lbl_Label_Name.effectStyle = isGray ? UILabel.Effect.None : UILabel.Effect.Outline;
        Lbl_Label_Timer.effectStyle = isGray ? UILabel.Effect.None : UILabel.Effect.Outline;
        Lbl_BtnButton_PrisonLabel.effectStyle = isGray ? UILabel.Effect.None : UILabel.Effect.Outline;

    }
}
