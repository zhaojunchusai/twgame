using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
public class HESKIntensifyPanel
{
    public GameObject root;
    public Skill infoSkill;
    public UIButton Btn_Button_intensify;
    public UIButton Btn_Button_onekeyintensify;

    public GameObject Marsk;
    public UISprite Icon;
    public UISprite Quality;
    public UISprite back;
    public UILabel Name;
    public UILabel Lbl_Label_attributeAft;
    public UILabel Lbl_Label_Lv_Befor;
    public UILabel Lbl_Label_Lv_After;
    public GameObject Material;
    public GameObject Cost;
    public Transform CostTrans;
    public GameObject AfterGroup;
    public GameObject MaxLevel;
    public KeyValuePair<KeyValuePair<MaterialBag.MaterialResult, MaterialsBagAttributeInfo>, List<KeyValuePair<Item, int>>> result;
    private GameObject Go_SkillIntensifyEffectTrail;
    private GameObject Go_SkillIntensifyEffect;
    public QuickMatItem QuickMatItem;

    public void init(GameObject _uiRoot)
    {
        root = _uiRoot.transform.FindChild("SkillIntensifyPanel").gameObject;
        root.SetActive(false);

        Marsk = _uiRoot.transform.FindChild("SkillIntensifyPanel/MaskSprite").gameObject;
        Btn_Button_intensify = _uiRoot.transform.FindChild("SkillIntensifyPanel/IntensifyButton").gameObject.GetComponent<UIButton>();
        Btn_Button_onekeyintensify = _uiRoot.transform.FindChild("SkillIntensifyPanel/OneKeyIntensifyButton").gameObject.GetComponent<UIButton>();

        Icon = _uiRoot.transform.FindChild("SkillIntensifyPanel/ArtifactComp/IconTexture").gameObject.GetComponent<UISprite>();
        Quality = _uiRoot.transform.FindChild("SkillIntensifyPanel/ArtifactComp/QualitySprite").gameObject.GetComponent<UISprite>();
        back = _uiRoot.transform.FindChild("SkillIntensifyPanel/ArtifactComp/back").gameObject.GetComponent<UISprite>();

        Name = _uiRoot.transform.FindChild("SkillIntensifyPanel/NameGroup/NameLabel").gameObject.GetComponent<UILabel>();
        Lbl_Label_attributeAft = _uiRoot.transform.FindChild("SkillIntensifyPanel/AttributeComparisonGroup/IntensifiedGroup/IntensifiedLabel").gameObject.GetComponent<UILabel>();


        Lbl_Label_Lv_Befor = _uiRoot.transform.FindChild("SkillIntensifyPanel/LevelGroup/LevelLabelLeft").gameObject.GetComponent<UILabel>();
        Lbl_Label_Lv_After = _uiRoot.transform.FindChild("SkillIntensifyPanel/LevelGroup/AfterGroup/LevelLabelRight").gameObject.GetComponent<UILabel>();

        Material = _uiRoot.transform.FindChild("SkillIntensifyPanel/CostItems/MaterialComp").gameObject;
        Cost = _uiRoot.transform.FindChild("SkillIntensifyPanel/CostItems/CostComp").gameObject;
        CostTrans = _uiRoot.transform.FindChild("SkillIntensifyPanel/CostItems");
        AfterGroup = _uiRoot.transform.FindChild("SkillIntensifyPanel/LevelGroup/AfterGroup").gameObject;
        MaxLevel = _uiRoot.transform.FindChild("SkillIntensifyPanel/LevelGroup/MaxLevel").gameObject;
        QuickMatItem = _uiRoot.transform.FindChild("SkillIntensifyPanel/QuickMatItem").gameObject.GetComponent<QuickMatItem>();
        if (QuickMatItem == null)
            QuickMatItem = _uiRoot.transform.FindChild("SkillIntensifyPanel/QuickMatItem").gameObject.AddComponent<QuickMatItem>();
    }
    public void setInfo(Skill wp)
    {
        //if(!this.root.activeSelf)
            //UISystem.Instance.HeroAttView.PlayOpenSkillAnim();
        //GuideManager.Instance.CheckTrigger(GuideTrigger.OpenHeroSkillStrengthen);
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
        if (Quality != null)
        {
            CommonFunction.SetQualitySprite(this.Quality,infoSkill.Att.Quality,this.back);
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
            Lbl_Label_Lv_Befor.text = string.Format("Lv.{0}",infoSkill.Level);
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

        if(infoSkill.enableStrong() == SkillCheck.MaxLevel)
        {
            this.Material.SetActive(false);
            if(this.Cost)
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

        if (infoSkill.Att.materialBag.Count <= (infoSkill.Level - infoSkill.Att.initLevel))
            return;
        if (Material != null)
        {
            this.Material.SetActive(true);

            result = MaterialBag.getResult(infoSkill.Att.materialBag[infoSkill.Level - infoSkill.Att.initLevel + 1]);

            var resultList = result.Value;

            do
            {
                if (0 >= resultList.Count) break;

                UISprite tx = Material.transform.FindChild("IconTexture").gameObject.GetComponent<UISprite>();
                UISprite sp = Material.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
                UISprite bc = Material.transform.FindChild("back").gameObject.GetComponent<UISprite>();
                UILabel lb = Material.transform.FindChild("CountLabel").gameObject.GetComponent<UILabel>();

                Item tempIt = resultList[0].Key;

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
                        //this.OnClose();
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
                    {
                        lb.color = Color.green;
                    }
                    else
                    {
                        lb.color = Color.red;
                    }
                    lb.text = string.Format("{0}/{1}", tempIt.num, resultList[0].Value);
                }
                SetQuickMatItem(tempItInfo, tempIt.num, resultList[0].Value);
            } while (false);
            //装备材料
        }
        if(Cost)
        {
            UILabel lb = Cost.transform.FindChild("CountLabel").gameObject.GetComponent<UILabel>();
            if (lb && result.Key.Key != MaterialBag.MaterialResult.noId)
            {
                if (PlayerData.Instance.GoldIsEnough(result.Key.Value.costType, result.Key.Value.Cost))
                {
                    Color effect = new Color(0.149f, 0, 0);

                    CommonFunction.SetLabelColor(lb, Color.green, effect);
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

    private void SetQuickMatItem(ItemInfo item, int hasNum, int needNum)
    {
        if (hasNum < needNum)
        {
            CostTrans.localPosition = new Vector3(0,-115,0);
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
            this.Material.SetActive(true);

            result = MaterialBag.getResult(infoSkill.Att.materialBag[infoSkill.Level - infoSkill.Att.initLevel + 1]);

            var resultList = result.Value;

            do
            {
                if (0 >= resultList.Count) break;

                UISprite tx = Material.transform.FindChild("IconTexture").gameObject.GetComponent<UISprite>();
                UISprite sp = Material.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
                UISprite bc = Material.transform.FindChild("back").gameObject.GetComponent<UISprite>();
                UILabel lb = Material.transform.FindChild("CountLabel").gameObject.GetComponent<UILabel>();

                Item tempIt = resultList[0].Key;

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
            //装备材料
        }
    }
    //====================================================================//
    public void PlaySkillIntensifyEffect()
    {
         if (Go_SkillIntensifyEffect==null)
         {
             ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_HEROSKILL, (GameObject gb) => { Go_SkillIntensifyEffect = gb; });

         }
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_SkillIntensifyEffect, Quality.transform);
        //if(Go_SkillIntensifyEffectTrail==null)
        //{
        //    ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_SKILLINTENSIFYITEMTRAIL, (GameObject gb) => { Go_SkillIntensifyEffectTrail = gb; });

        //}
        //GameObject Go = ShowEffectManager.Instance.ShowEffect(Go_SkillIntensifyEffectTrail, Material.transform);
        //iTween.MoveTo(Go, Quality.transform.position, 0.5F);
        //Main.Instance.StartCoroutine(ShowSkillIntensifyEffect(0.25F));
    }
    //private IEnumerator ShowSkillIntensifyEffect(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    if (Go_SkillIntensifyEffect==null)
    //    {
    //        ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_HEROSKILL, (GameObject gb) => { Go_SkillIntensifyEffect = gb; });

    //    }
    //    GameObject go = ShowEffectManager.Instance.ShowEffect(Go_SkillIntensifyEffect, Quality.transform);
    //}

}