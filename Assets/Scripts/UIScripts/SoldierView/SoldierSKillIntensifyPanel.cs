using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SDSKIntensifyPanel
{
    public GameObject root;
    public Skill infoSkill;
    public UIButton Btn_Button_intensify;
    public UIButton Btn_Button_onekeyintensify;

    public GameObject Marsk;
    public UISprite Icon;
    public UISprite quality;
    public UISprite qualityBack;
    public UILabel Name;
    public UILabel Lbl_Label_attributeAft;
    public UILabel Lbl_Label_Lv_Befor;
    public UILabel Lbl_Label_Lv_After;
    public GameObject Material;
    public GameObject Cost;
    public GameObject AfterGroup;
    public Transform CostTrans;
    public GameObject MaxLevel;
    public QuickMatItem QuickMatItem;
    public KeyValuePair<KeyValuePair<MaterialBag.MaterialResult, MaterialsBagAttributeInfo>, List<KeyValuePair<fogs.proto.msg.Item, int>>> result;
    private GameObject Go_SkillIntensifyEffectTrail;
    private GameObject Go_SkillIntensifyEffect;
    private UISpriteAnimation SkillLvUpEff;
    public void init(GameObject _uiRoot)
    {
        root = _uiRoot.transform.FindChild("SkillIntensifyPanel").gameObject;
        root.SetActive(false); ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_SOLDIERSKILL, (GameObject gb) => { Go_SkillIntensifyEffect = gb; });
        ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_SKILLINTENSIFYITEMTRAIL, (GameObject gb) => { Go_SkillIntensifyEffectTrail = gb; });

        Marsk = _uiRoot.transform.FindChild("SkillIntensifyPanel/MaskSprite").gameObject;
        Btn_Button_intensify = _uiRoot.transform.FindChild("SkillIntensifyPanel/IntensifyButton").gameObject.GetComponent<UIButton>();
        Btn_Button_onekeyintensify = _uiRoot.transform.FindChild("SkillIntensifyPanel/OneKeyIntensifyButton").gameObject.GetComponent<UIButton>();

        Icon = _uiRoot.transform.FindChild("SkillIntensifyPanel/ArtifactComp/IconTexture").gameObject.GetComponent<UISprite>();
        quality = _uiRoot.transform.FindChild("SkillIntensifyPanel/ArtifactComp/QualitySprite").gameObject.GetComponent<UISprite>();
        SkillLvUpEff = _uiRoot.transform.FindChild("SkillIntensifyPanel/ArtifactComp/Effect").gameObject.GetComponent<UISpriteAnimation>();
        qualityBack = _uiRoot.transform.FindChild("SkillIntensifyPanel/ArtifactComp/back").gameObject.GetComponent<UISprite>();
        Name = _uiRoot.transform.FindChild("SkillIntensifyPanel/NameGroup/NameLabel").gameObject.GetComponent<UILabel>();
        Lbl_Label_attributeAft = _uiRoot.transform.FindChild("SkillIntensifyPanel/AttributeComparisonGroup/IntensifiedGroup/IntensifiedLabel").gameObject.GetComponent<UILabel>();

        Lbl_Label_Lv_Befor = _uiRoot.transform.FindChild("SkillIntensifyPanel/LevelGroup/LevelLabelLeft").gameObject.GetComponent<UILabel>();
        Lbl_Label_Lv_After = _uiRoot.transform.FindChild("SkillIntensifyPanel/LevelGroup/AfterGroup/LevelLabelRight").gameObject.GetComponent<UILabel>();

        Material = _uiRoot.transform.FindChild("SkillIntensifyPanel/CostItems/MaterialComp").gameObject;
        Cost = _uiRoot.transform.FindChild("SkillIntensifyPanel/CostItems/CostComp").gameObject;
        AfterGroup = _uiRoot.transform.FindChild("SkillIntensifyPanel/LevelGroup/AfterGroup").gameObject;
        MaxLevel = _uiRoot.transform.FindChild("SkillIntensifyPanel/LevelGroup/MaxLevel").gameObject;
        CostTrans = _uiRoot.transform.FindChild("SkillIntensifyPanel/CostItems");

        QuickMatItem = _uiRoot.transform.FindChild("SkillIntensifyPanel/QuickMatItem").gameObject.GetComponent<QuickMatItem>();
        if (QuickMatItem == null)
            QuickMatItem = _uiRoot.transform.FindChild("SkillIntensifyPanel/QuickMatItem").gameObject.AddComponent<QuickMatItem>();
    }

    public void setInfo(Skill wp)
    {
        //if (!root.activeSelf)
            //UISystem.Instance.SoldierAttView.PlayOpenSkilAnim();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenSoldierSkillStrengthen);
        root.SetActive(true);
        infoSkill = wp;
        _setInfo();
    }

    public void OnClose()
    {
        root.SetActive(false);
        infoSkill = null;
    }

    public void OnIntensify()
    {

    }

    private void _setInfo()
    {
        if (infoSkill == null) return;
        if (Icon != null)
        {
            CommonFunction.SetSpriteName(Icon,infoSkill.Att.Icon);
        }
        if (quality)
        {
            CommonFunction.SetQualitySprite(quality, infoSkill.Att.Quality, qualityBack);
        }
        if (Name != null)
        {
            Name.text = infoSkill.Att.Name;
        }
        if (Lbl_Label_attributeAft != null)
        {
            if (infoSkill.enableStrong() != SkillCheck.MaxLevel)
                Lbl_Label_attributeAft.text = string.Format(ConstString.SKILL_STRENGTH, infoSkill.GetDescript(infoSkill.Level));
            else
                Lbl_Label_attributeAft.text = infoSkill.GetDescript(infoSkill.Level);
        }
        if (Lbl_Label_Lv_Befor != null && Lbl_Label_Lv_After != null)
        {
            Lbl_Label_Lv_Befor.text = string.Format("Lv.{0}", infoSkill.Level);
            if (infoSkill.enableStrong() != SkillCheck.MaxLevel)
            {
                this.MaxLevel.SetActive(false);
                this.AfterGroup.SetActive(true);
                Lbl_Label_Lv_After.text = string.Format("Lv.{0}", infoSkill.Level + 1);
            }
            else
            {
                this.MaxLevel.SetActive(true);
                this.AfterGroup.SetActive(false);
            }
        }

        if (infoSkill.enableStrong() == SkillCheck.MaxLevel)
        {
            this.Material.SetActive(false);
            if (this.Cost)
            {
                UILabel lb = Cost.transform.FindChild("CountLabel").gameObject.GetComponent<UILabel>();
                lb.text = "";
            }
            return;
        }
        else
        {
            this.Material.SetActive(true);
        }

        if (Material != null)
        {
            if (!infoSkill.Att.materialBag.ContainsKey(infoSkill.Level - infoSkill.Att.initLevel + 1)) return;
            result = MaterialBag.getResult(infoSkill.Att.materialBag[infoSkill.Level - infoSkill.Att.initLevel + 1]);

            var resultList = result.Value;

            do
            {
                if (0 >= resultList.Count) break;

                UISprite tx = Material.transform.FindChild("IconTexture").gameObject.GetComponent<UISprite>();
                UISprite sp = Material.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
                UISprite bc = Material.transform.FindChild("back").gameObject.GetComponent<UISprite>();
                UILabel lb = Material.transform.FindChild("CountLabel").gameObject.GetComponent<UILabel>();

                fogs.proto.msg.Item tempIt = resultList[0].Key;

                ItemInfo tempItInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(tempIt.id);

                if (tempItInfo == null)
                {
                    UIEventListener.Get(Material.gameObject).onClick = (GameObject go) => { return; };
                    break;
                }
                else
                {
                    UIEventListener.Get(Material.gameObject).onClick = (GameObject go) =>
                    {
                         UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETPATH);
                         UISystem.Instance.GetPathView.UpdateViewInfo(tempItInfo.id, 1);
                         this.OnClose();
                    };
                }

                if (tx)
                {
                    CommonFunction.SetSpriteName(tx, tempItInfo.icon);
                }
                if (sp)
                {
                    CommonFunction.SetQualitySprite(sp, tempItInfo.quality, bc);
                }
                if (lb)
                {
                    if (tempIt.num >= resultList[0].Value)
                        lb.color = Color.green;
                    else
                        lb.color = Color.red;
                    lb.text = string.Format("{0}/{1}", tempIt.num, resultList[0].Value);
                }
                SetQuickMatItem(tempItInfo, tempIt.num, resultList[0].Value);

            } while (false);
            //×°±¸²ÄÁÏ
            if (Cost)
            {
                UILabel lb = Cost.transform.FindChild("CountLabel").gameObject.GetComponent<UILabel>();
                if (lb && result.Key.Key != MaterialBag.MaterialResult.noId)
                {
                    if (PlayerData.Instance.GoldIsEnough(result.Key.Value.costType, result.Key.Value.Cost))
                    {
                        Color effect = new Color(0.149f, 0, 0);

                        CommonFunction.SetLabelColor(lb, Color.white, effect);
                        lb.effectStyle = UILabel.Effect.Outline;
                    }
                    else
                    {
                        Color effect = new Color(0.149f, 0, 0);
                        CommonFunction.SetLabelColor(lb, Color.red, effect);
                        lb.effectStyle = UILabel.Effect.Outline;

                    }
                    lb.text = string.Format("{0}/{1}", CommonFunction.GetTenThousandUnit(PlayerData.Instance._Gold), CommonFunction.GetTenThousandUnit(result.Key.Value.Cost));
                }
            }
        }
    }

    private void SetQuickMatItem(ItemInfo item, int hasNum, int needNum)
    {
        if (hasNum < needNum)
        {
            CostTrans.localPosition = new Vector3(0, -115, 0);
            QuickMatItem.gameObject.SetActive(true);
            QuickMatItem.Init(item, hasNum, needNum);
            CommonFunction.SetGameObjectGray(Btn_Button_onekeyintensify.gameObject, true);
        }
        else
        {
            CostTrans.localPosition = new Vector3(0, -154, 0);
            QuickMatItem.gameObject.SetActive(false);
            QuickMatItem.Clear();
            CommonFunction.SetGameObjectGray(Btn_Button_onekeyintensify.gameObject, false);
        }
    }

    public void RefreshCost()
    {
        if (Cost)
        {
            UILabel lb = Cost.transform.FindChild("CountLabel").gameObject.GetComponent<UILabel>();
            if (lb && result.Key.Key != MaterialBag.MaterialResult.noId)
            {
                if (result.Key.Value.Cost < PlayerData.Instance._Gold)
                {
                    lb.color = Color.green;
                }
                else
                    lb.color = Color.red;
                lb.text = string.Format("{0}/{1}", CommonFunction.GetTenThousandUnit(PlayerData.Instance._Gold), CommonFunction.GetTenThousandUnit(result.Key.Value.Cost));
            }
        }
    }

    public void RefreshMaterial()
    {
        if (Material != null)
        {
            if (!infoSkill.Att.materialBag.ContainsKey(infoSkill.Level - infoSkill.Att.initLevel + 1)) return;
            result = MaterialBag.getResult(infoSkill.Att.materialBag[infoSkill.Level - infoSkill.Att.initLevel + 1]);

            var resultList = result.Value;

            do
            {
                if (0 >= resultList.Count) break;

                UISprite tx = Material.transform.FindChild("IconTexture").gameObject.GetComponent<UISprite>();
                UISprite sp = Material.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
                UISprite bc = Material.transform.FindChild("back").gameObject.GetComponent<UISprite>();
                UILabel lb = Material.transform.FindChild("CountLabel").gameObject.GetComponent<UILabel>();

                fogs.proto.msg.Item tempIt = resultList[0].Key;

                ItemInfo tempItInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(tempIt.id);

                if (tempItInfo == null)
                {
                    UIEventListener.Get(Material.gameObject).onClick = (GameObject go) => { return; };
                    break;
                }
                else
                {
                    UIEventListener.Get(Material.gameObject).onClick = (GameObject go) =>
                    {
                        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETPATH);
                        UISystem.Instance.GetPathView.UpdateViewInfo(tempItInfo.id, 1);
                    };
                }

                if (tx)
                {
                    CommonFunction.SetSpriteName(tx, tempItInfo.icon);
                }
                if (sp)
                {
                    CommonFunction.SetQualitySprite(sp, tempItInfo.quality, bc);
                }
                if (lb)
                {
                    if (tempIt.num >= resultList[0].Value)
                        lb.color = Color.green;
                    else
                        lb.color = Color.red;
                    lb.text = string.Format("{0}/{1}", tempIt.num, resultList[0].Value);
                }
                SetQuickMatItem(tempItInfo, tempIt.num, resultList[0].Value);
            } while (false);
        }
    }
    //========================================================================//
    public void PlaySkillIntensifyEffect()
    {
        SkillLvUpEff.ResetToBeginning();
        SkillLvUpEff.loop = false;
        SkillLvUpEff.framesPerSecond = 25;
        SkillLvUpEff.gameObject.SetActive(true);
        SkillLvUpEff.Play();
        Assets.Script.Common.Scheduler.Instance.RemoveTimer(CloseEff);
        Assets.Script.Common.Scheduler.Instance.AddTimer(0.69f, false, CloseEff);
        //GameObject go = ShowEffectManager.Instance.ShowEffect(Go_SkillIntensifyEffect, quality.transform);

        //GameObject Go = ShowEffectManager.Instance.ShowEffect(Go_SkillIntensifyEffectTrail, Material.transform);
        //iTween.MoveTo(Go, quality.transform.position, 0.5F);
        //Main.Instance.StartCoroutine(ShowSkillIntensifyEffect(0.25F));
    }

    private void CloseEff()
    {
        SkillLvUpEff.gameObject.SetActive(false);
    }
    //private IEnumerator ShowSkillIntensifyEffect(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    GameObject go = ShowEffectManager.Instance.ShowEffect(Go_SkillIntensifyEffect, quality.transform);
    //}
}