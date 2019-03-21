using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class CenterPanel
{
    public UILabel Lbl_name;
    public UILabel Lbl_level;
    public GameObject UIPanel_Panel_player;
    public GameObject player;
    public GameObject spine;
    public TdSpine.MainSpine mainSpine;
    public UIPanel centerPanel;

    public UISlider Slider_ProgressBar;
    public UILabel Lbl_SliderProgressBarLabel;
    public UILabel Lbl_Label_attribute1;
    public UILabel Lbl_Label_attribute2;
    public UILabel Lbl_Label_attribute3;
    public UILabel Lbl_Label_attribute4;
    public UILabel Lbl_Label_attribute5;
    public UILabel Lbl_Label_attribute6;
    public UILabel Lbl_Label_attribute7;
    public UILabel Lbl_Label_attribute8;
    public UILabel Lbl_Label_attribute9;
    public UILabel Lbl_Label_attribute10;
    public UILabel PowerLabel;

    private GameObject Btn_PetButton;
    private GameObject Gobj_PetLevel;
    private UILabel Lbl_PetLevel;
    private UISprite Spt_PetIcon;
    private UISprite Spt_PetQuality;
    private UISprite Spt_PetShade;
    private UISprite Spt_EquipPet;
    private GameObject Btn_ShowPet;
    private UILabel Lbl_ShowPetButton;

    public void init(GameObject _uiRoot)
    {
        centerPanel = _uiRoot.transform.FindChild("Anim/center").gameObject.GetComponent<UIPanel>();
        Lbl_name = _uiRoot.transform.FindChild("Anim/center/name").gameObject.GetComponent<UILabel>();
        Lbl_level = _uiRoot.transform.FindChild("Anim/center/level").gameObject.GetComponent<UILabel>();
        UIPanel_Panel_player = _uiRoot.transform.FindChild("Anim/center/Panel_player").gameObject;
        player = UIPanel_Panel_player.transform.Find("player").gameObject;
        Slider_ProgressBar = _uiRoot.transform.FindChild("Anim/center/Progress Bar").gameObject.GetComponent<UISlider>();
        Lbl_SliderProgressBarLabel = _uiRoot.transform.FindChild("Anim/center/Progress Bar/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute1 = _uiRoot.transform.FindChild("Anim/center/AttBack/BaseAtt/Grid/Label_1/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute2 = _uiRoot.transform.FindChild("Anim/center/AttBack/BaseAtt/Grid/Label_2/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute3 = _uiRoot.transform.FindChild("Anim/center/AttBack/BaseAtt/Grid/Label_3/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute4 = _uiRoot.transform.FindChild("Anim/center/AttBack/BaseAtt/Grid/Label_4/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute5 = _uiRoot.transform.FindChild("Anim/center/AttBack/BaseAtt/Grid/Label_5/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute6 = _uiRoot.transform.FindChild("Anim/center/AttBack/OtherAtt/Grid/Label_1/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute7 = _uiRoot.transform.FindChild("Anim/center/AttBack/OtherAtt/Grid/Label_2/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute8 = _uiRoot.transform.FindChild("Anim/center/AttBack/OtherAtt/Grid/Label_3/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute9 = _uiRoot.transform.FindChild("Anim/center/AttBack/OtherAtt/Grid/Label_4/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute10 = _uiRoot.transform.FindChild("Anim/center/AttBack/OtherAtt/Grid/Label_5/Label_attribute").gameObject.GetComponent<UILabel>();
        PowerLabel = _uiRoot.transform.FindChild("Anim/center/fighting/Label_Fighting").gameObject.GetComponent<UILabel>();
        Btn_PetButton = _uiRoot.transform.FindChild("Anim/center/PetButton").gameObject;
        Spt_PetIcon = _uiRoot.transform.FindChild("Anim/center/PetButton/PetIcon").gameObject.GetComponent<UISprite>();
        Spt_PetQuality = _uiRoot.transform.FindChild("Anim/center/PetButton/PetQuality").gameObject.GetComponent<UISprite>();
        Spt_PetShade = _uiRoot.transform.FindChild("Anim/center/PetButton/PetShade").gameObject.GetComponent<UISprite>();
        Spt_EquipPet = _uiRoot.transform.FindChild("Anim/center/PetButton/EquipMark").gameObject.GetComponent<UISprite>();
        Gobj_PetLevel = _uiRoot.transform.FindChild("Anim/center/PetButton/LevelGroup").gameObject;
        Lbl_PetLevel = _uiRoot.transform.FindChild("Anim/center/PetButton/LevelGroup/Label").gameObject.GetComponent<UILabel>();
        Btn_ShowPet = _uiRoot.transform.FindChild("Anim/center/ShowPetButton").gameObject;
        Lbl_ShowPetButton = _uiRoot.transform.FindChild("Anim/center/ShowPetButton/ShowPet").gameObject.GetComponent<UILabel>();
        UIEventListener.Get(Btn_PetButton).onClick = ButtonEvent_ChangePet;
        UIEventListener.Get(Btn_ShowPet).onClick = ButtonEvent_ShowPet;

        PetSystemModule.Instance.PetUpdateEvent += PetUpdateEvent;
        _SkeleAnimation();
        _setAtt();
    }
    public void _setAtt()
    {
        PlayerData player = PlayerData.Instance;
        if (player == null) return;
        ShowInfoHero playerAtt = player._Attribute;
        if (playerAtt == null) return;
        Lbl_name.text = player._NickName;
        int NextLvExp = ConfigManager.Instance.mHeroData.FindByLevel((int)PlayerData.Instance._Level).EXP;

        Lbl_level.text = player._Level.ToString();
        if (NextLvExp != 0)
            Slider_ProgressBar.value = (float)player._CurrentExp / NextLvExp;
        else
            Slider_ProgressBar.value = 1;
        Lbl_SliderProgressBarLabel.text = string.Format("{0}/{1}", player._CurrentExp.ToString(), NextLvExp.ToString());
        Lbl_Label_attribute1.text = playerAtt.Leadership.ToString();
        Lbl_Label_attribute2.text = ((int)(playerAtt.Attack)).ToString();
        Lbl_Label_attribute3.text = playerAtt.HP.ToString();
        Lbl_Label_attribute4.text = playerAtt.MP.ToString();
        Lbl_Label_attribute5.text = playerAtt.Energy.ToString();
        Lbl_Label_attribute6.text = playerAtt.Crit.ToString();
        Lbl_Label_attribute7.text = playerAtt.Tenacity.ToString();
        Lbl_Label_attribute8.text = playerAtt.HPRecovery.ToString();
        Lbl_Label_attribute9.text = playerAtt.MPRecovery.ToString();
        Lbl_Label_attribute10.text = playerAtt.EnergyRecovery.ToString();
        PowerLabel.text = playerAtt.CombatPower.ToString();

        if (PlayerData.Instance._WeaponDepot != null)
        {
            PlayerData.Instance._WeaponDepot.ErrotDeleteEvent -= this.OnWeaponChange;
            PlayerData.Instance._WeaponDepot.ErrotDeleteEvent += this.OnWeaponChange;
        }
        UpdatePetInfo();
    }
    public void OnClose()
    {
        if (this.spine != null)
        {
            GameObject.Destroy(this.spine);
            PlayerData.Instance._WeaponDepot.ErrotDeleteEvent -= this.OnWeaponChange;
        }
        PetSystemModule.Instance.PetUpdateEvent -= PetUpdateEvent;
    }
    private void OnWeaponChange(EquipControl control, int errorCode)
    {
        if (control == EquipControl.GetoffEquipAndArtifactResp || control == EquipControl.PutonEquipAndArtifactResp)
        {
            //Weapon temp = PlayerData.Instance._WeaponDepot.FindByUid(uID);

            //if (temp == null)
            //    return;
            if (this.mainSpine != null && errorCode == 0)
                this.mainSpine.RepleaceEquipment(PlayerData.Instance._ArtifactedDepot);
        }
    }
    private void ButtonEvent_ChangePet(GameObject go)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PETCHOOSE);
        UISystem.Instance.PetChooseView.UpdateViewInfo();
    }

    private void ButtonEvent_ShowPet(GameObject go)
    {
        PetSystemModule.Instance.SendShowMount(!PlayerData.Instance.IsShowPet);
    }

    private void PetUpdateEvent()
    {
        UpdatePetInfo();
        PowerLabel.text = PlayerData.Instance._Attribute.CombatPower.ToString();
    }

    private void UpdatePetInfo()
    {
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Pet, false))
        {
            Btn_ShowPet.SetActive(false);
            Btn_PetButton.SetActive(false);
            this.mainSpine.RepleaceEquipment(PlayerData.Instance._ArtifactedDepot);
            return;
        }
        List<PetData> petList = PlayerData.Instance._PetDepot.GetOwnedPets();
        if (petList == null || petList.Count == 0)
        {
            Btn_ShowPet.SetActive(false);
            Btn_PetButton.SetActive(false);
            this.mainSpine.RepleaceEquipment(PlayerData.Instance._ArtifactedDepot);
        }
        else
        {
            Btn_ShowPet.SetActive(true);
            Btn_PetButton.SetActive(true);
            PetData petData = PlayerData.Instance._PetDepot.GetEquipedPet();
            if (petData == null)
            {
                Btn_ShowPet.SetActive(false);
                Spt_EquipPet.enabled = true;
                Gobj_PetLevel.SetActive(false);
                Spt_PetIcon.spriteName = string.Empty;
                CommonFunction.SetQualitySprite(Spt_PetQuality, (int)ItemQualityEnum.White, Spt_PetShade);
                CommonFunction.SetSpriteName(Spt_PetShade, GlobalConst.SpriteName.PETSYSTEM_PETBG);
                this.mainSpine.RepleaceEquipment(PlayerData.Instance._ArtifactedDepot);
            }
            else
            {
                Gobj_PetLevel.SetActive(true);
                Spt_EquipPet.enabled = false;
                Lbl_PetLevel.text = petData.Level.ToString();
                CommonFunction.SetQualitySprite(Spt_PetQuality, petData.PetInfo.quality, Spt_PetShade);
                CommonFunction.SetSpriteName(Spt_PetIcon, petData.PetInfo.icon);
                Btn_ShowPet.SetActive(true);
                if (PlayerData.Instance.IsShowPet)
                {
                    Lbl_ShowPetButton.text = ConstString.PET_HEROVIEW_MOUNTS;
                }
                else
                {
                    Lbl_ShowPetButton.text = ConstString.PET_HEROVIEW_SHOWPET;
                }
                this.mainSpine.RepleaceEquipment(PlayerData.Instance._ArtifactedDepot, petData.PetInfo.res_name, PlayerData.Instance.IsShowPet);
            }
        }
    }



    private void _SkeleAnimation()
    {
        if (UIPanel_Panel_player == null) return;
        if (this.spine != null)
        {
            GameObject.Destroy(this.spine);
        }
        if (this.mainSpine != null)
        {

        }
        ResourceLoadManager.Instance.LoadCharacter(CommonFunction.GetHeroResourceNameByGender((EHeroGender)PlayerData.Instance._Gender), ResourceLoadType.AssetBundle, (obj) =>
        {
            if (obj != null)
            {
                GameObject go = CommonFunction.InstantiateObject(obj, player.transform);
                go.SetActive(true);
                this.spine = go;
                TdSpine.MainSpine mainSpine = go.GetComponent<TdSpine.MainSpine>();
                if (mainSpine != null)
                    UnityEngine.Object.Destroy(mainSpine);
                mainSpine = go.AddComponent<TdSpine.MainSpine>();
                this.mainSpine = mainSpine;
                this.mainSpine.InitSkeletonAnimation();
                go.transform.localScale *= RoleManager.Instance.Get_UIHero_Scale;
                this.mainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);

                Main.Instance.StartCoroutine(SpineEquipInit());

            }
        });


        UIEventListener.Get(UIPanel_Panel_player.transform.FindChild("player").gameObject).onClick = (go) =>
        {
            if (this.mainSpine == null) return;

            List<string> tempList = new List<string>();
            tempList.Add(GlobalConst.ANIMATION_NAME_MOVE);
            tempList.Add(GlobalConst.ANIMATION_NAME_VICTORY);
            //tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_BASIC);
            tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE);
            tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE6);
            tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE8);
            tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE10);
            int index = UnityEngine.Random.Range(0, tempList.Count - 1);

            this.mainSpine.pushAnimation(tempList[index], true, 1);
            this.mainSpine.EndEvent += (string animationName) =>
            {
                this.mainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
            };
        };
    }
    private IEnumerator SpineEquipInit()
    {
        yield return 0;
        //if (this.mainSpine != null)
        //    this.mainSpine.RepleaceEquipment(PlayerData.Instance._ArtifactedDepot);
        this.mainSpine.ResetAlph(1.0f);
        this.mainSpine.setSortingOrder(1);
        this.mainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
        UISystem.Instance.ResortViewOrder();
    }
}
