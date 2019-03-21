using UnityEngine;
using System.Collections;
using fogs.proto.msg;

public class RemoveBlockItem : MonoBehaviour 
{
    public UISprite Icon;
    public UISprite Frame;
    public UILabel LevelNum;
    public UILabel PlayerName;
    public UILabel PlayerLegion;
    public UILabel PlayerFighting;
    public UILabel PlayerVIPLevel;
    public UIButton RemoveBlockBtn;

    private string _accName;
    private uint _areaID;
    private string _blockName;
   
    public void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        Icon = this.transform.FindChild("PlayerIcon/Icon").gameObject.GetComponent<UISprite>();
        Frame = this.transform.FindChild("PlayerIcon/Frame").gameObject.GetComponent<UISprite>();
        LevelNum = this.transform.FindChild("PlayerIcon/LevelBG/Lv").gameObject.GetComponent<UILabel>();
        PlayerName = this.transform.FindChild("PlayerInfo/Name").gameObject.GetComponent<UILabel>();
        PlayerLegion = this.transform.FindChild("PlayerInfo/Legion").gameObject.GetComponent<UILabel>();
        PlayerFighting = this.transform.FindChild("PlayerInfo/Fighting").gameObject.GetComponent<UILabel>();
        PlayerVIPLevel = this.transform.FindChild("PlayerInfo/VIPBG/VIP").gameObject.GetComponent<UILabel>();
        RemoveBlockBtn = this.transform.FindChild("RemoveBlockBtn/BG").gameObject.GetComponent<UIButton>();
        BtnEventBinding();
    }
    public void InitItem(BlockedInfos data)
    {
        CommonFunction .SetHeadAndFrameSprite(Icon,Frame,data.icon,data.icon_frame,true);
     
        LevelNum.text = data.level.ToString();
        PlayerName.text = data.name;
        PlayerVIPLevel.text =string.Format(ConstString.REMOVEBLOCKVIP,data.vip_level);
        Debug.Log("RemoveBlockUnionName = "+data.union_name);
        if (data.union_name == null||data.union_name == "")
        {
            PlayerLegion.text = string.Format(ConstString.REMOVEBLOCKLEGION, ConstString.RANK_LABEL_NOUNION);
        }
        else
        {
            PlayerLegion.text = string.Format(ConstString.REMOVEBLOCKLEGION, data.union_name);
        }
        PlayerFighting.text = string.Format(ConstString.REMOVEBLOCKFIGHTING, data.combatpwer);
        _accName = data.accname;
        _areaID = data.area_id;
        _blockName = data.name;
    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(RemoveBlockBtn.gameObject).onClick = BtnEvent_RemoveBlockBtn;
    }
    public void BtnEvent_RemoveBlockBtn(GameObject Btn)
    {
       //TODO：发送解除屏蔽消息
       //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, "解除屏蔽，等待服务端完善功能Ing....");
       SystemSettingModule.Instance.SendRemoveBlockRequest(_accName,_areaID,_blockName);
   }
}
