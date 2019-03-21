using UnityEngine;
using System.Collections;

public class EndlessView
{
    public static string UIName = "EndlessView";
    public GameObject _uiRoot;
    public GameObject GO_PTLevelLabel;
    public GameObject GO_JYLevelLabel;
    public GameObject GO_YXLevelLabel;
    public UILabel Lab_PTMaxDPLabel;
    public UILabel Lab_PTMaxScoreLabel;
    public UILabel Lab_PTEndlessRankLabel;
    public UIButton Btn_PTChallengeButton;
    public UILabel Lab_JYMaxDPLabel;
    public UILabel Lab_JYMaxScoreLabel;
    public UILabel Lab_JYEndlessRankLabel;
    public UIButton Btn_JYChallengeButton;
    public UILabel Lab_YXMaxDPLabel;
    public UILabel Lab_YXMaxScoreLabel;
    public UILabel Lab_YXEndlessRankLabel;
    public UIButton Btn_YXChallengeButton;
    public GameObject Lab_PTIsLockLabel;
    public GameObject Lab_JYIsLockLabel;
    public GameObject Lab_YXIsLockLabel;
    public UIButton Btn_CloseButton;

    public UILabel Lbl_ClearanceDesc;

    public GameObject DoubleCount;
    public GameObject DoubleReward;
    public TweenScale Anim_TScale;


    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/EndlessView");
        DoubleCount = _uiRoot.transform.FindChild("Anim/Double/Count").gameObject;
        DoubleReward = _uiRoot.transform.FindChild("Anim/Double/Reward").gameObject;
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        GO_PTLevelLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_PT/PT_LevelLabel").gameObject;
        GO_JYLevelLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_JY/JY_LevelLabel").gameObject;
        GO_YXLevelLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_YX/YX_LevelLabel").gameObject;
        Lab_PTMaxDPLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_PT/PT_LevelLabel/MaxDPLabel").gameObject.GetComponent<UILabel>();

        Lab_PTMaxScoreLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_PT/PT_LevelLabel/MaxScoreLabel").gameObject.GetComponent<UILabel>();
        Lab_PTEndlessRankLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_PT/PT_LevelLabel/EndlessRankLabel").gameObject.GetComponent<UILabel>();
        Btn_PTChallengeButton = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_PT/PT_ChallengeButton").gameObject.GetComponent<UIButton>();
        Lab_JYMaxDPLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_JY/JY_LevelLabel/MaxDPLabel").gameObject.GetComponent<UILabel>();
        Lab_JYMaxScoreLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_JY/JY_LevelLabel/MaxScoreLabel").gameObject.GetComponent<UILabel>();
        Lab_JYEndlessRankLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_JY/JY_LevelLabel/EndlessRankLabel").gameObject.GetComponent<UILabel>();
        Btn_JYChallengeButton = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_JY/JY_ChallengeButton").gameObject.GetComponent<UIButton>();
        Lab_YXMaxDPLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_YX/YX_LevelLabel/MaxDPLabel").gameObject.GetComponent<UILabel>();
        Lab_YXMaxScoreLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_YX/YX_LevelLabel/MaxScoreLabel").gameObject.GetComponent<UILabel>();
        Lab_YXEndlessRankLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_YX/YX_LevelLabel/EndlessRankLabel").gameObject.GetComponent<UILabel>();
        Btn_YXChallengeButton = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_YX/YX_ChallengeButton").gameObject.GetComponent<UIButton>();
        Lab_PTIsLockLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_PT/PT_IsLockLabel").gameObject;
        Lab_JYIsLockLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_JY/JY_IsLockLabel").gameObject;
        Lab_YXIsLockLabel = _uiRoot.transform.FindChild("Anim/BattleGroundItem/BattlegroundItem_YX/YX_IsLockLabel").gameObject;
        Lbl_ClearanceDesc = _uiRoot.transform.FindChild("Anim/DescGroup/ClearanceDesc").gameObject.GetComponent<UILabel>();

        Btn_CloseButton = _uiRoot.transform.FindChild("Anim/Top/CloseButton").gameObject.GetComponent<UIButton>();

        SetLabelVaules();
    }
    public void Unitialize()
    {

    }
    public void SetLabelVaules()
    {

    }
    public void SetVisible(bool IsVisible)
    {
        _uiRoot.SetActive(IsVisible);
    }
}
