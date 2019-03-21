using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
public class PetSystemModule : Singleton<PetSystemModule>
{
    private PetSystemNetwork mPetNetwork;
    public bool IsIgnoreUpgradeTip = false;
    public bool IsIgnoreStrenghTip = false;

    private bool isDressPet = false;

    public delegate void PetUpdateDelegate();
    public event PetUpdateDelegate PetUpdateEvent;

    public delegate void PetChooseDelegate(uint PetID);
    public event PetChooseDelegate PetChooseEvent;


    public void Initialize()
    {
        if (mPetNetwork == null)
        {
            mPetNetwork = new PetSystemNetwork();
        }
        mPetNetwork.RegisterMsg();
        IsIgnoreUpgradeTip = false;
        IsIgnoreStrenghTip = false;
    }
    #region Msg
    /// <summary>
    /// 装备宠物  0表示卸载宠物
    /// </summary>
    public void SendDressPet(uint id)
    {
        isDressPet = (id == 0) ? false : true;
        DressPetReq data = new DressPetReq();
        data.id = id;
        mPetNetwork.SendDressPet(data);
    }

    public void ReceiveDressPet(DressPetResp data)
    {
        if (data.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            if (isDressPet)
            {
                PetData pet = PlayerData.Instance._PetDepot.GetEquipedPet();
                if (pet == null)//如果是进行装备操作  首先判断之前是否已装备宠物 如果是从无到有则默认显示宠物
                {
                    PlayerData.Instance.IsShowPet = true;
                }
            }
            else
            {
                PlayerData.Instance.IsShowPet = false;
            }
            PlayerData.Instance.EquipPetID = data.id;
            List<PetData> pet_datas = PlayerData.Instance._PetDepot.GetOwnedPets();
            if (pet_datas != null)
            {
                for (int i = 0; i < pet_datas.Count; i++)
                {
                    PetData pet = pet_datas[i];
                    if (data == null)
                        continue;
                    pet.IsEquiped = pet.PetInfo.id.Equals(data.id);
                }
            }
            PlayerData.Instance._Attribute.CombatPower = Calculation_Attribute.Instance.CalculationPlayerCombatPower();
            PlayerData.Instance._PetDepot.SortPet();
            if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_PETSYSTEM))
            {
                UISystem.Instance.PetSystemView.ReceiveDressPet();
            }
            else if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_PETCHOOSE))
            {
                UISystem.Instance.PetChooseView.ReceiveDressPet();
            }
            if (PetUpdateEvent != null)
            {
                PetUpdateEvent();
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    /// <summary>
    /// 更换宠物展示  true展示宠物
    /// </summary>
    public void SendShowMount(bool showMount)
    {
        ShowMountReq data = new ShowMountReq();
        int show = showMount ? 1 : 0;
        data.showMount = (uint)show;
        mPetNetwork.SendShowMount(data);
    }

    public void ReceiveShowMount(ShowMountResp data)
    {
        if (data.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.IsShowPet = (data.showMount == 0) ? false : true;
            if (PetUpdateEvent != null)
            {
                PetUpdateEvent();
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    /// <summary>
    /// 升级宠物
    /// </summary>
    /// <param name="id"></param>
    public void SendPromotePet(uint id)
    {
        PromotePetLvReq data = new PromotePetLvReq();
        data.id = id;
        mPetNetwork.SendPromotePet(data);
    }

    public void ReceivePromotePet(PromotePetLvResp data)
    {
        if (data.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdatePlayerAttribute(data.attr);
            PlayerData.Instance.UpdateItem(data.item_list);
            PlayerData.Instance._PetDepot.UpdatePetData(data.new_pet);
            UISystem.Instance.PetSystemView.ReceivePromotePet(data.new_pet.id);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendUpgradePetSKill(uint petID, uint skillID)
    {
        UpgradePetSkillLvReq data = new UpgradePetSkillLvReq();
        data.id = petID;
        data.skillid = skillID;
        mPetNetwork.SendUpgradePetSkill(data);

    }
    public void ReceiveUpgradePetSKill(UpgradePetSkillLvResp data)
    {
        if (data.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateItem(data.item_list);
            PlayerData.Instance._PetDepot.UpdatePetSkill(data.id, data.skill);
            UISystem.Instance.PetSystemView.ReceiveUpgradePetSKill(data.id);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void ChoosePet(uint petID)
    {
        if (PetChooseEvent != null)
            PetChooseEvent(petID);
    }


    #endregion


    public void Uninitialize()
    {
        if (mPetNetwork != null)
            mPetNetwork.RemoveMsg();
        mPetNetwork = null;
        IsIgnoreStrenghTip = false;
        IsIgnoreUpgradeTip = false;
    }
}
