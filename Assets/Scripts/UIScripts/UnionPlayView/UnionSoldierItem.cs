using UnityEngine;
using System.Collections.Generic;

public class UnionSoldierItem : MonoBehaviour
{
    const int MAXCOUNT = 3;
    const int MAXSTAR = 6;
    const int MINSTAR = 1;

    [HideInInspector]public UIGrid Grd_StarsGrid;
    [HideInInspector]public UISprite Spt_StarSprite_1;
    [HideInInspector]public UISprite Spt_StarSprite_2;
    [HideInInspector]public UISprite Spt_StarSprite_3;
    [HideInInspector]public UISprite Spt_StarSprite_4;
    [HideInInspector]public UISprite Spt_StarSprite_5;
    [HideInInspector]public UISprite Spt_StarSprite_6;
    [HideInInspector]public UISprite Spt_BGSprite;
    [HideInInspector]public UISprite Spt_IconSprite;
    [HideInInspector]public UILabel Lbl_LeadershipLabel;
    [HideInInspector]public UISprite Spt_QualitySprite;
    [HideInInspector]public UISprite Spt_IconTexture;
    [HideInInspector]public UISprite Spt_BG;
    [HideInInspector]public UISprite Spt_LevelBG;
    [HideInInspector]public UILabel Lbl_LevelLabel;
    [HideInInspector]public UILabel Lbl_EnergyLabel;
    [HideInInspector]public UISprite Spt_EnergyBG;
    [HideInInspector]public UILabel Lbl_NumLabel;
    [HideInInspector]public UISprite Spt_SoldierType;
    [HideInInspector]public GameObject Obj_Leadership;
    [HideInInspector]
    public UILabel lbl_Label_Step;
    private List<UISprite> _starsList;
    private bool _isInit = false;

    public void Initialize()
    {
        if (_isInit)
            return;
        _isInit = true;
        Grd_StarsGrid = transform.FindChild("StarsGrid").gameObject.GetComponent<UIGrid>();
        Spt_StarSprite_1 = transform.FindChild("StarsGrid/StarSprite_1").gameObject.GetComponent<UISprite>();
        Spt_StarSprite_2 = transform.FindChild("StarsGrid/StarSprite_2").gameObject.GetComponent<UISprite>();
        Spt_StarSprite_3 = transform.FindChild("StarsGrid/StarSprite_3").gameObject.GetComponent<UISprite>();
        Spt_StarSprite_4 = transform.FindChild("StarsGrid/StarSprite_4").gameObject.GetComponent<UISprite>();
        Spt_StarSprite_5 = transform.FindChild("StarsGrid/StarSprite_5").gameObject.GetComponent<UISprite>();
        Spt_StarSprite_6 = transform.FindChild("StarsGrid/StarSprite_6").gameObject.GetComponent<UISprite>();
        Spt_BGSprite = transform.FindChild("BGSprite").gameObject.GetComponent<UISprite>();
        Spt_IconSprite = transform.FindChild("LeadershipGroup/IconSprite").gameObject.GetComponent<UISprite>();
        Lbl_LeadershipLabel = transform.FindChild("LeadershipGroup/LeadershipLabel").gameObject.GetComponent<UILabel>();
        Spt_QualitySprite = transform.FindChild("ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = transform.FindChild("ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_BG = transform.FindChild("ItemBaseComp/BG").gameObject.GetComponent<UISprite>();
        Spt_LevelBG = transform.FindChild("LevelBG").gameObject.GetComponent<UISprite>();
        Lbl_LevelLabel = transform.FindChild("LevelBG/LevelLabel").gameObject.GetComponent<UILabel>();
        Lbl_EnergyLabel = transform.FindChild("EnergyGroup/EnergyLabel").gameObject.GetComponent<UILabel>();
        Spt_EnergyBG = transform.FindChild("EnergyGroup/EnergyBG").gameObject.GetComponent<UISprite>();
        Lbl_NumLabel = transform.FindChild("NumLabel").gameObject.GetComponent<UILabel>();
        Spt_SoldierType = transform.FindChild("SoldierType").gameObject.GetComponent<UISprite>();
        Obj_Leadership = transform.FindChild("LeadershipGroup").gameObject;
        lbl_Label_Step = transform.FindChild("Step").gameObject.GetComponent<UILabel>();
        lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
        SetLabelValues();

        _starsList = new List<UISprite>() 
        { 
            Spt_StarSprite_1,
            Spt_StarSprite_2, 
            Spt_StarSprite_3,
            Spt_StarSprite_4, 
            Spt_StarSprite_5, 
            Spt_StarSprite_6 
        };

        Obj_Leadership.SetActive(false);
        Spt_SoldierType.gameObject.SetActive(false);
    }

    void Awake()
    {
        Initialize();
    }

    public void SetLabelValues()
    {
        Lbl_LeadershipLabel.text = "";
        Lbl_LevelLabel.text = "";
        Lbl_EnergyLabel.text = "";
        Lbl_NumLabel.text = "";
    }

    public void UpdateItem(Soldier soldier, int num)
    {
        if (soldier == null)
        {
            Debug.LogError("solider data is null!!!");
            Clear();
            return;
        }

        Spt_IconTexture.gameObject.SetActive(true);
        CommonFunction.SetSpriteName(Spt_IconTexture, soldier.Att.Icon);
        CommonFunction.SetQualitySprite(Spt_QualitySprite, soldier.Att.quality,Spt_BG);
        CommonFunction.SetSpriteName(Spt_SoldierType, CommonFunction.GetSoldierTypeIcon(soldier.Att.Career));
        Lbl_LeadershipLabel.text = soldier.Att.leaderShip.ToString();
        Lbl_EnergyLabel.text = soldier.Att.call_energy.ToString();
        Lbl_LevelLabel.text = soldier.Lv.ToString();
        this.lbl_Label_Step.text = CommonFunction.GetStepShow(this.lbl_Label_Step,soldier.StepNum);
        UpdateStars(soldier.Att.Star);

        Debug.Log("star == "+ soldier.Att.Star);
        Lbl_NumLabel.text = ConstString.UNIONREADINESS_MUTI + num;
    }

    private void UpdateStars(int stars)
    {
        Mathf.Clamp(stars, MINSTAR, MAXSTAR);
        for (int i = 0; i < stars; i++)
        {
            _starsList[i].gameObject.SetActive(true);
        }
        for (int i = stars; i < _starsList.Count; i++)
        {
            _starsList[i].gameObject.SetActive(false);
        }
    }

    public void Uninitialize()
    {

    }

    public void Clear()
    {
        Lbl_LeadershipLabel.text = "0";
        Lbl_EnergyLabel.text = "0";
        Lbl_LevelLabel.text = "0";
        Spt_IconTexture.gameObject.SetActive(false);
        CommonFunction.SetQualitySprite(Spt_QualitySprite, ItemQualityEnum.White);
        Spt_SoldierType.spriteName = "";
        Spt_IconTexture.spriteName = "";
        UpdateStars(0);
    }
}
