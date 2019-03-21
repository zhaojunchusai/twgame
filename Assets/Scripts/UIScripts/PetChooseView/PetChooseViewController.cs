using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PetChooseViewController : UIBase
{
    public PetChooseView view;

    private List<PetChooseComponent> pet_dic;

    private EFightType fightType;
    private uint currentPetID;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new PetChooseView();
            view.Initialize();
            BtnEventBinding();
        }
        if (pet_dic == null)
            pet_dic = new List<PetChooseComponent>();
        view.Gobj_PetChooseComp.SetActive(false);
        view.UIWrapContent_Grid.onInitializeItem = UpdateWrapPet;
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenChoosePetView);

    }

    public void UpdateViewInfo()
    {
        fightType = EFightType.None;
        currentPetID = 0;
        StartUpdatePetComp();
    }

    public void UpdateViewInfo(EFightType type, uint petID)
    {
        fightType = type;
        currentPetID = petID;
        StartUpdatePetComp();
    }

    private void StartUpdatePetComp() 
    {
        List<PetData> petdata_list = PlayerData.Instance._PetDepot.GetOwnedPets();
        if (petdata_list == null)
            petdata_list = new List<PetData>();
        switch (fightType)
        {
            case EFightType.eftHegemony:
            case EFightType.eftQualifying:
            case EFightType.eftPVP:
            case EFightType.eftSlave:
            case EFightType.eftUnion:
            case EFightType.eftCaptureTerritory:
            case EFightType.eftServerHegemony:
            case EFightType.eftCrossServerWar:
                {
                    petdata_list.Sort((left, right) =>
                    {
                        if (left == null || right == null)
                            return 0;
                        if (left.IsOwned != right.IsOwned)
                        {
                            if (left.IsOwned)
                                return -1;
                            else
                            {
                                return 1;
                            }
                        }
                        if (left.PetInfo.PetID != right.PetInfo.PetID)
                        {
                            if (left.PetInfo.id.Equals(currentPetID) && !right.PetInfo.id.Equals(currentPetID))
                            {
                                return -1;
                            }
                            else
                            {
                                return 1;
                            }
                        }
                        if (left.Level != right.Level)
                        {
                            if (left.Level >= right.Level)
                            {
                                return -1;
                            }
                            else
                            {
                                return 1;
                            }
                        }
                        if (left.PetInfo.quality != right.PetInfo.quality)
                        {
                            if (left.PetInfo.quality > right.PetInfo.quality)
                            {
                                return -1;
                            }
                            else
                            {
                                return 1;
                            }
                        }
                        if (left.PetInfo.id != right.PetInfo.id)
                        {
                            if (left.PetInfo.id < right.PetInfo.id)
                            {
                                return -1;
                            }
                            else
                            {
                                return 1;
                            }
                        }
                        return 0;
                    });
                }
                break;
            case EFightType.eftActivity:
            case EFightType.eftEndless:
            case EFightType.eftMain:
            case EFightType.eftExpedition:
            case EFightType.None:
            default:
                {
                } break;
        }

        Main.Instance.StartCoroutine(UpdatePetComponent(petdata_list));
    }

    private void UpdateWrapPet(GameObject go, int wrapIndex, int realIndex)
    {
        if (!view.UIWrapContent_Grid.enabled) return;
        List<PetData> petdatas = PlayerData.Instance._PetDepot.GetOwnedPets();
        if (petdatas == null) return;
        if (realIndex >= petdatas.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        PetChooseComponent comp = pet_dic[wrapIndex];
        if (comp == null) return;
        PetData petData = petdatas[realIndex];
        if (petData == null)
            return;
        comp.UpdateInfo(petData);
    }

    private IEnumerator UpdatePetComponent(List<PetData> petdata_list)
    {
        int MAXCOUNT = 25;
       
        int count = petdata_list.Count;
        int itemCount = pet_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_Grid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_Grid.minIndex = -index;
        view.UIWrapContent_Grid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_Grid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_Grid.enabled = false;
        }
        yield return null;
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                pet_dic[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            PetData petData = petdata_list[i];
            PetChooseComponent comp = null;
            if (i < itemCount)
            {
                comp = pet_dic[i];

            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_PetChooseComp, view.Grd_Grid.transform);
                comp = new PetChooseComponent();
                vGo.name = i.ToString();
                comp.MyStart(vGo);
                comp.AddEventListener(ButtonEvent_PetChoose);
                pet_dic.Add(comp);
            }
            if (comp == null)
                continue;
            comp.UpdateInfo(petData);
            switch (fightType)
            {
                case EFightType.eftActivity:
                case EFightType.eftEndless:
                case EFightType.eftMain:
                case EFightType.eftExpedition:
                    {
                        comp.IsEquip = petData.IsEquiped;
                    }
                    break;
                case EFightType.eftHegemony:
                case EFightType.eftQualifying:
                case EFightType.eftPVP:
                case EFightType.eftSlave:
                case EFightType.eftUnion:
                case EFightType.eftCaptureTerritory:
                case EFightType.eftServerHegemony:
                case EFightType.eftCrossServerWar:
                    {
                        comp.IsEquip = petData.PetPOD.id.Equals(currentPetID);
                    }
                    break;
                case EFightType.None:
                default:
                    {
                        comp.IsEquip = petData.IsEquiped;
                    } break;
            }

            comp.mRootObject.SetActive(true);
        }
        yield return null;
        view.UIWrapContent_Grid.ReGetChild();
        yield return null;
        view.Grd_Grid.Reposition();
        yield return null;
        view.ScrView_PetChooseScrollView.ResetPosition();
    }


    public void ButtonEvent_UninstallButton(GameObject btn)
    {
        switch (fightType)
        {
            case EFightType.eftActivity:
            case EFightType.eftEndless:
            case EFightType.eftMain:
            case EFightType.eftExpedition:
                {
                    PetSystemModule.Instance.SendDressPet(0);
                }
                break;
            case EFightType.eftHegemony:
            case EFightType.eftQualifying:
            case EFightType.eftPVP:
            case EFightType.eftSlave:
            case EFightType.eftUnion:
            case EFightType.eftCaptureTerritory:
            case EFightType.eftServerHegemony:
            case EFightType.eftCrossServerWar:
                {
                    PetSystemModule.Instance.ChoosePet(0);
                }
                break;
            case EFightType.None:
            default:
                {
                    PetSystemModule.Instance.SendDressPet(0);
                } break;
        }
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_PETCHOOSE);
    }

    public void ButtonEvent_Close(GameObject go)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_PETCHOOSE);
    }

    public void ButtonEvent_PetChoose(BaseComponent basecomp)
    {
        PetChooseComponent comp = basecomp as PetChooseComponent;
        if (comp == null || comp.PetData == null)
            return;
        switch (fightType)
        {
            case EFightType.eftActivity:
            case EFightType.eftEndless:
            case EFightType.eftMain:
            case EFightType.eftExpedition:
                {
                    if (comp.IsEquip)
                    {
                        return;
                    }
                    PetSystemModule.Instance.SendDressPet(comp.PetData.PetInfo.id);
                }
                break;
            case EFightType.eftHegemony:
            case EFightType.eftQualifying:
            case EFightType.eftPVP:
            case EFightType.eftSlave:
            case EFightType.eftUnion:
            case EFightType.eftCaptureTerritory:
            case EFightType.eftServerHegemony:
            case EFightType.eftCrossServerWar:
                {
                    PetSystemModule.Instance.ChoosePet(comp.PetData.PetPOD.id);
                }
                break;
            default:
                {
                    if (comp.IsEquip)
                    {
                        return;
                    }
                    PetSystemModule.Instance.SendDressPet(comp.PetData.PetInfo.id);
                } break;
        }
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_PETCHOOSE);

    }

    public void ReceiveDressPet()
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_PETCHOOSE);
    }


    public override void Uninitialize()
    {

    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        if (pet_dic != null)
            pet_dic.Clear();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_UninstallButton.gameObject).onClick = ButtonEvent_UninstallButton;
        UIEventListener.Get(view.Spt_MarkSprite.gameObject).onClick = ButtonEvent_Close;
    }


}
