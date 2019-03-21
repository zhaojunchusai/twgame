using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;

public class HeadChangeItem : MonoBehaviour 
{

    public UIButton Btn_ItemBtn;
    public UISprite BGIcon;
    public UISprite HeadTex;
    public UISprite Spt_Status;
    public UILabel Lbl_TimeLabel;
    public UISprite Spt_Selected;

    public uint itemID;
    private int _resettime;
    private int _status;
    private uint _type;

    public void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        Btn_ItemBtn = transform.gameObject.GetComponent<UIButton>();
        BGIcon = transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        HeadTex = transform.FindChild("Head").gameObject.GetComponent<UISprite>();
        Spt_Status = transform.FindChild("Status").gameObject.GetComponent<UISprite>();
        Lbl_TimeLabel = transform.FindChild("TimeLabel").gameObject.GetComponent<UILabel>();
        Spt_Selected = transform.FindChild("Selected").gameObject.GetComponent<UISprite>();

        BtnEvenBinding();
    }
    /// <summary>
    /// 设置头像内容
    /// </summary>
    /// <param name="vdata"></param>
    public void InitItem(IconInformation vdata)
    {
        if (vdata != null)
        {
            
            _resettime = vdata.resettime;
            _status = vdata.status;
            PlayerPortraitData data = ConfigManager.Instance.mPlayerPortraitConfig.GetPlayerPortraitByID((uint)vdata.id);
            if (data != null)
            {
                itemID = data.id;
                _type = data.type;
                CommonFunction.SetSpriteName(HeadTex, data.icon);
                string Frame_A = string.Format(GlobalConst.SpriteName.Frame_Name_A, PlayerData.Instance.FrameID);
                CommonFunction.SetSpriteName(BGIcon, Frame_A);

                if (vdata.status == 0)
                {
                    Spt_Status.gameObject.SetActive(true);
                    Lbl_TimeLabel.gameObject.SetActive(false);
                }
                else if (vdata.status == 1)
                {
                    Spt_Status.gameObject.SetActive(false);
                    if (data.type == 3 && vdata.resettime != 0)
                    {
                        Lbl_TimeLabel.gameObject.SetActive(true);
                        TimeUpdate();
                        Scheduler.Instance.AddTimer(1, true, TimeUpdate);
                    }
                    else
                    {
                        Lbl_TimeLabel.gameObject.SetActive(false);
                    }
                }
                else
                {
                    Debug.LogError("iconinformation status error");
                }
            }
            else
            {
                Debug.LogError("playerportrait config error");
            }
        }
        else
        {
            Debug.LogError("iconinformation is null");
        }

    }
    
    public void BtnEvenBinding()
    {
        UIEventListener.Get(Btn_ItemBtn.gameObject).onClick = BtnEvent_ClickButton;
    }
    public void BtnEvent_ClickButton(GameObject Btn)
    {
        if (ConfigManager.Instance.mPlayerPortraitConfig.IsIdCorrect(itemID))
        {

            UISystem.Instance.SystemSettingView.UnSelectPreIcon();
            if (UISystem.Instance.SystemSettingView.selIconId == itemID)
            {
                Spt_Selected.gameObject.SetActive(false);
                UISystem.Instance.SystemSettingView.selIconId = 0;
            }
            else
            {
                UISystem.Instance.SystemSettingView.selIconId = itemID;
                Spt_Selected.gameObject.SetActive(true);
                if(_status==1)
                    UISystem.Instance.SystemSettingView.changeId = itemID;
            }

            SetDescGobj();
        }
        else
        {
            Debug.LogError("itemId error");
        }
    }

    /// <summary>
    /// 重置时间跳动
    /// </summary>
    private void TimeUpdate()
    {
        //Debug.LogError(reset_time - Main.mTime);
        if (_resettime > Main.mTime)
        {
            Lbl_TimeLabel.text = CommonFunction.GetTimeString(_resettime - Main.mTime);
        }
        else
        {
            if (PlayerData.Instance.HeadID == itemID)
            {
                uint defaultNum;
                if (PlayerData.Instance._Gender == 1)
                    defaultNum = 10002;
                else defaultNum = 10001;
                SystemSettingModule.Instance.SendHeadChangeRequset(defaultNum);
                //PlayerData.Instance.HeadID = defaultNum;
                //UISystem.Instance.MainCityView.UpdateHeadIcon();
                //UISystem.Instance.SystemSettingView.InitPlayerInfo();
            }
            //Debug.LogError("end and remove");
            StopTimeUpdate();
            ResetToLocked();
            //Spt_Selected.gameObject.SetActive(false);
        }
        if (UISystem.Instance.SystemSettingView.view.Gobj_Desc.activeSelf && UISystem.Instance.SystemSettingView.selDescId == itemID)
            UISystem.Instance.SystemSettingView.SetDescLabels(_status, Lbl_TimeLabel.text);
    }
    /// <summary>
    /// 结束时间跳动
    /// </summary>
    public void StopTimeUpdate()
    {
        Scheduler.Instance.RemoveTimer(TimeUpdate);
        if (UISystem.Instance.SystemSettingView.changeId == itemID)
            UISystem.Instance.SystemSettingView.changeId = 0;
    }
    /// <summary>
    /// 时间结束，重置回锁定
    /// </summary>
    private void ResetToLocked()
    {
        _status = 0;
        Spt_Status.gameObject.SetActive(true);
        Lbl_TimeLabel.gameObject.SetActive(false);
    }
    /// <summary>
    /// 开启关闭头像描述框
    /// </summary>
    private void SetDescGobj()
    {
        if(UISystem.Instance.SystemSettingView.selDescId == itemID)
        {
            UISystem.Instance.SystemSettingView.view.Gobj_Desc.SetActive(false);
            UISystem.Instance.SystemSettingView.selDescId = 0;
        }
        else
        {
            //UISystem.Instance.SystemSettingView.view.Gobj_Desc.SetActive(true);
            UISystem.Instance.SystemSettingView.selDescId = itemID;
            UISystem.Instance.SystemSettingView.SetDescLabels(_status, Lbl_TimeLabel.text);
            UISystem.Instance.SystemSettingView.OpenDescSelected(PositionType(),transform.position, BGIcon.width/2f, BGIcon.height/2f);
        }
    }
    /// <summary>
    /// 计算头像相对位置
    /// </summary>
    /// <returns></returns>
    private int PositionType()
    {
        if (this.transform.position.x <= 0 && this.transform.position.y >= 0)
        {
            return 2;
        }
        else if (this.transform.position.x <= 0 && this.transform.position.y < 0)
        {
            return 3;
        }
        else if (this.transform.position.x > 0 && this.transform.position.y < 0)
        {
            return 4;
        }
        else return 1;
    }
 

    public void OnDestroy()
    {
        Scheduler.Instance.RemoveTimer(TimeUpdate);

    }
}
