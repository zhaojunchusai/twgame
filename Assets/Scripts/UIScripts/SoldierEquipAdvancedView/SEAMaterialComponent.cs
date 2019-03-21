using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SEAMaterialComponent : MonoBehaviour
{
    public GameObject mRootObject;
    protected UISprite Spt_SelectSprite;
    protected UISprite Spt_QualitySprite;
    protected UISprite Spt_IconTexture;
    protected UISprite Spt_ItemBG;

    protected UIGrid Grd_StarLevelGroup;
    protected UISprite Spt_StarSprite;
    protected UILabel Label_Level;
    private List<GameObject> starList;

    private Weapon weaponPOD;
    public Weapon WeaponPOD
    {
        get
        {
            return weaponPOD;
        }
    }

    private bool isSelect = false;
    public bool IsSelect
    {
        get
        {
            return isSelect;
        }
        set
        {
            isSelect = value;
            Spt_SelectSprite.enabled = isSelect;
        }
    }

    public void MyStart(GameObject root)
    {
        mRootObject = root;
        Initialize();
        //Clear();
        starList = new List<GameObject>();
    }
    private void Initialize()
    {
        Spt_SelectSprite = mRootObject.transform.FindChild("SelectSprite").gameObject.GetComponent<UISprite>();
        Spt_QualitySprite = mRootObject.transform.FindChild("ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_ItemBG = mRootObject.transform.FindChild("ItemBaseComp/BGSprite").gameObject.GetComponent<UISprite>();
        Grd_StarLevelGroup = mRootObject.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();
        Spt_StarSprite = mRootObject.transform.FindChild("StarLevelGroup/StarSprite").gameObject.GetComponent<UISprite>();
        Label_Level = mRootObject.transform.FindChild("ItemBaseComp/Lv/Label").gameObject.GetComponent<UILabel>();
    }

    public void UpdateInfo(Weapon pod, GameObject ob)
    {
        weaponPOD = pod;
        if (pod == null)
        {
            Clear();
            return;
        }
        if (Label_Level != null)
        {
            Label_Level.text = pod.Level.ToString();
        }
        CommonFunction.SetQualitySprite(Spt_QualitySprite, weaponPOD.Att.quality, Spt_ItemBG);
        CommonFunction.SetSpriteName(Spt_IconTexture, weaponPOD.Att.icon);
        UpdateEquipStars();
    }

    private void UpdateEquipStars()
    {
        if (weaponPOD.Att.star <= starList.Count)
        {
            for (int i = 0; i < starList.Count; i++)
            {
                if (i < weaponPOD.Att.star)
                {
                    starList[i].gameObject.SetActive(true);
                }
                else
                {
                    starList[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            int index = starList.Count;
            for (int i = 0; i < starList.Count; i++)
            {
                starList[i].gameObject.SetActive(true);
            }
            for (int i = index; i < weaponPOD.Att.star; i++)
            {
                GameObject go = CommonFunction.InstantiateObject(Spt_StarSprite.gameObject, Grd_StarLevelGroup.transform);
                go.name = "star_" + i;
                go.SetActive(true);
                starList.Add(go);
            }
        }
        if (Spt_StarSprite.gameObject.activeSelf)
        {
            Spt_StarSprite.gameObject.SetActive(false);
        }
        Grd_StarLevelGroup.Reposition();
    }

    public void Clear()
    {      
        weaponPOD = null;
        isSelect = false;
    }

}
