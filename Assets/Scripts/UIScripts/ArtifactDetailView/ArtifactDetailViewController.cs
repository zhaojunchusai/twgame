using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ArtifactDetailViewController : UIBase
{
    public ArtifactDetailView view;
    private List<SoldierEquipDetailAttComponent> soldierEquipAttributeList;
    private Weapon weaponPOD;

    public enum EDetailAnchor
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public struct DetailAppearBorder
    {
        public float up;
        public float down;
        public float left;
        public float right;
    }
    private GameObject _target;

    private DetailAppearBorder _border = new DetailAppearBorder();
    private Vector2 _halfPageScale = new Vector2();
    private Vector2 _fingerOffset = new Vector2(20, 20);
    private Vector2 _borderOffset = new Vector2(70, 70);
    private Vector3 _pagePos = Vector3.zero;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new ArtifactDetailView();
            view.Initialize();
        }
        if (soldierEquipAttributeList == null)
        {
            soldierEquipAttributeList = new List<SoldierEquipDetailAttComponent>();
        }
        view.Gobj_EquipAttComp.gameObject.SetActive(false);
        _border.up = GlobalConst.DEFAULT_SCREEN_SIZE_Y / 2 - _borderOffset.x;
        _border.down = -GlobalConst.DEFAULT_SCREEN_SIZE_Y / 2 + _borderOffset.x;
        _border.left = -GlobalConst.DEFAULT_SCREEN_SIZE_X / 2 + _borderOffset.x;
        _border.right = GlobalConst.DEFAULT_SCREEN_SIZE_X / 2 - _borderOffset.x;
    }

    public void UpdateViewInfo(GameObject go, Weapon pod)
    {
        _target = go;
        weaponPOD = pod;
        if (weaponPOD == null)
        {
            view._uiRoot.SetActive(false);
            return;
        }
        UpdateItemInfo();
        UpdateItemAtt();
        //UpdateItemDesc();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        CalculateScale();
        if (_target == null)
        {
            return;
        }
        //_halfPageScale = new Vector2((float)view.Spt_BgSprite.width / 2, (view.Spt_BgSprite.height /2 + 60 ));      //.height);//+ 30 / 2));
        Vector3 position = CommonFunction.GetObjPosRelateToRootPanel(_target);
        GetDetailPagePos(position);
        FixBorder();
        //PlayOpenSyrAnim();
        view.Gobj_Detail.gameObject.SetActive(true);
        view.Gobj_Detail.transform.localPosition = _pagePos;
    }

    private void CalculateScale()
    {
        view.Lbl_ItemDesc.text = weaponPOD.Att.descript;
        view.Spt_BgSprite.height = 120 + (int)view.Lbl_ItemDesc.printedSize.y;
        //Vector2 size = view.Lbl_ItemDesc.printedSize;
        //if (size.y > 80)
        //    view.Spt_StrPageBG.height = (int)size.y + 30;
        //else
        //{
        //    view.Spt_StrPageBG.height = 110;
        //}
        _halfPageScale = new Vector2((float)view.Spt_BgSprite.width / 2, view.Spt_BgSprite.height / 2);
    }

    private void GetDetailPagePos(Vector3 position)
    {
        EDetailAnchor anchor = GetDetailAnchor(position);
        switch (anchor)
        {
            case EDetailAnchor.BottomRight:
                {
                    _pagePos = new Vector3(position.x + _halfPageScale.x + _fingerOffset.x + 30, position.y - _halfPageScale.y - _fingerOffset.y, 0);
                    break;
                }
            case EDetailAnchor.TopLeft:
                {
                    _pagePos = new Vector3(position.x - _halfPageScale.x - _fingerOffset.x, position.y + _halfPageScale.y + _fingerOffset.y, 0);
                    break;
                }
            case EDetailAnchor.BottomLeft:
                {
                    _pagePos = new Vector3(position.x - _halfPageScale.x - _fingerOffset.x, position.y - _halfPageScale.y - _fingerOffset.y, 0);
                    break;
                }
            case EDetailAnchor.TopRight:
                {
                    _pagePos = new Vector3(position.x + _halfPageScale.x + _fingerOffset.x + 30, position.y + _halfPageScale.y + _fingerOffset.y, 0);
                    break;
                }
        }
    }

    private EDetailAnchor GetDetailAnchor(Vector3 pos)
    {
        if (pos.x >= 0 && pos.y >= 0)
        {
            return EDetailAnchor.BottomLeft;
        }
        else if (pos.x < 0 && pos.y >= 0)
        {
            return EDetailAnchor.BottomRight;
        }
        else if (pos.x >= 0 && pos.y < 0)
        {
            return EDetailAnchor.TopLeft;
        }
        else
        {
            return EDetailAnchor.TopRight;
        }
    }

    private void FixBorder()
    {
        if (_pagePos.x + _halfPageScale.x > _border.right)
        {

            _pagePos += new Vector3(_border.right - _pagePos.x - _halfPageScale.x, 0, 0);
        }

        else if (_pagePos.x - _halfPageScale.x < _border.left)
        {

            _pagePos += new Vector3(_border.left - (_pagePos.x - _halfPageScale.x), 0, 0);
        }

        if (_pagePos.y + _halfPageScale.y > _border.up)
        {
            _pagePos += new Vector3(0, _border.up - _pagePos.y - _halfPageScale.y, 0);
        }

        else if (_pagePos.y - _halfPageScale.y < _border.down)
        {
            _pagePos += new Vector3(0, _border.down - (_pagePos.y - _halfPageScale.y), 0);
        }
    }
    private void UpdateItemInfo()
    {
        CommonFunction.SetSpriteName(view.Spt_ItemIcon, weaponPOD.Att.icon);
        CommonFunction.SetQualitySprite(view.Spt_ItemQuality, weaponPOD.Att.quality, view.Spt_ItemBg);
    }

    private void UpdateItemAtt()
    {
        if (weaponPOD == null || weaponPOD.InfoAttribute == null)
        {
            return;
        }
        List<KeyValuePair<string, string>> attribute_dic = CommonFunction.GetWeaponAttributeDescNoWord(weaponPOD.InfoAttribute);
        if (attribute_dic == null)
        {
            attribute_dic = new List<KeyValuePair<string, string>>();
        }
        if (attribute_dic.Count <= soldierEquipAttributeList.Count)
        {
            for (int i = 0; i < soldierEquipAttributeList.Count; i++)
            {
                SoldierEquipDetailAttComponent comp = soldierEquipAttributeList[i];
                if (i < attribute_dic.Count)
                {
                    comp.mRootObject.SetActive(true);
                    comp.UpdateInfo(attribute_dic[i].Key, attribute_dic[i].Value.ToString());
                }
                else
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int count = soldierEquipAttributeList.Count;
            for (int i = 0; i < attribute_dic.Count; i++)
            {
                SoldierEquipDetailAttComponent comp = null;
                if (i < count)
                {
                    comp = soldierEquipAttributeList[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_EquipAttComp, view.Grd_AttGroup.transform);
                    go.name = "att_" + i;
                    comp = new SoldierEquipDetailAttComponent();
                    comp.MyStart(go);
                    soldierEquipAttributeList.Add(comp);
                }
                if (comp == null) continue;
                comp.mRootObject.SetActive(true);
                comp.UpdateInfo(attribute_dic[i].Key, attribute_dic[i].Value.ToString());
            }
        }
        view.Grd_AttGroup.repositionNow = true;
    }

    //private void UpdateItemDesc()
    //{

    //}

    public override void Uninitialize()
    {
        weaponPOD = null;
        view.Lbl_ItemDesc.text = string.Empty;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        if (soldierEquipAttributeList!=null)
            soldierEquipAttributeList.Clear();
    }
}
