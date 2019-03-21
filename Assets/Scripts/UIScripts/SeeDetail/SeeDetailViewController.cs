using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;

public class SeeDetailViewController : UIBase
{
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

    public SeeDetailView view;
    private GameObject _target;

    private DetailAppearBorder _border = new DetailAppearBorder();
    private Vector2 _halfPageScale = new Vector2(250 / 2, 300 / 2);
    private Vector2 _fingerOffset = new Vector2(30, 20);
    private Vector2 _borderOffset = new Vector2(10, 10);
    private Vector3 _pagePos = Vector3.zero;
    private Vector3 _posEquipChip = new Vector3(-12, 10, 0);
    private Vector3 _posSoldierChip = new Vector3(-12, 14, 0);
    private Vector3 _posLifeSoulMark = new Vector3(11, 11, 0);
    public override void Initialize()
    {
        if (view == null)
        {
            view = new SeeDetailView();
            view.Initialize();
            BtnEventBinding();
        }
        view.Gobj_ItemDesPage.SetActive(false);
        view.Gobj_StrPage.SetActive(false);
        view.Spt_Icon.type = UIBasicSprite.Type.Simple;
        view.Lbl_ItemDesPageOwnNum.enabled = true;
        view.Lbl_ItemDesPageOwnNumTip.enabled = true;
        Vector3 pos = view.Lbl_ItemDesPageName.transform.localPosition;
        view.Lbl_ItemDesPageName.transform.localPosition = new Vector3(pos.x, 37, pos.z);
        _border.up = GlobalConst.DEFAULT_SCREEN_SIZE_Y / 2 - _borderOffset.x;
        _border.down = -GlobalConst.DEFAULT_SCREEN_SIZE_Y / 2 + _borderOffset.x;
        _border.left = -GlobalConst.DEFAULT_SCREEN_SIZE_X / 2 + _borderOffset.x;
        _border.right = GlobalConst.DEFAULT_SCREEN_SIZE_X / 2 - _borderOffset.x;
    }

    public void SeeDetail(GameObject go, bool press, uint id)
    {
        if (press)
        {
            view.Spt_ChipMark.gameObject.SetActive(false);
            CommonItemData item = new CommonItemData(id, 0, true, true);
            if (item.Type == IDType.Prop)
            {
                CommonFunction.SetChipMark(view.Spt_ChipMark, item.SubType, _posEquipChip, _posSoldierChip);
            }
            else if (item.Type == IDType.LifeSoul)
            {
                CommonFunction.SetLifeSoulMark(view.Spt_ChipMark, item.LifeSoulGodEquip, _posLifeSoulMark);
            }
            #region old
            //IDType type = CommonFunction.GetTypeOfID(id.ToString());
            //switch (type)
            //{
            //    case IDType.Gold:
            //        {
            //            item = new CommonItemData(id,
            //                ConstString.NAME_GOLD,
            //                ItemQualityEnum.White,
            //                PlayerData.Instance._Gold,
            //                GlobalConst.SpriteName.Gold,
            //                type,
            //                ConstString.DESC_GOLD);
            //            break;
            //        }
            //    case IDType.Diamond:
            //        {
            //            item = new CommonItemData(id,
            //                ConstString.NAME_DIAMOND,
            //                ItemQualityEnum.White,
            //                PlayerData.Instance._Diamonds,
            //                GlobalConst.SpriteName.Diamond,
            //                type,
            //                ConstString.DESC_DIAMOND);
            //            //Debug.Log(PlayerData.Instance._Diamonds);
            //            break;
            //        }
            //    case IDType.SP:
            //        {
            //            item = new CommonItemData(id,
            //                ConstString.NAME_SP,
            //                ItemQualityEnum.White,
            //                PlayerData.Instance._Physical,
            //                GlobalConst.SpriteName.SP,
            //                type,
            //                ConstString.DESC_SP);
            //            break;
            //        }
            //    case IDType.Honor:
            //        {
            //            item = new CommonItemData(id,
            //                ConstString.NAME_HONOR,
            //                ItemQualityEnum.White,
            //                PlayerData.Instance._Honor,
            //                GlobalConst.SpriteName.Honor,
            //                type,
            //                ConstString.DESC_HONOR);
            //            break;
            //        }
            //    case IDType.Medal:
            //        {
            //            item = new CommonItemData(id,
            //                ConstString.NAME_MEDAL,
            //                ItemQualityEnum.White,
            //                PlayerData.Instance._Medal,
            //                GlobalConst.SpriteName.Medal,
            //                type,
            //                ConstString.DESC_MEDAL);
            //            break;
            //        }
            //    case IDType.Prop:
            //        {
            //            item = new CommonItemData(ConfigManager.Instance.mItemData.GetItemInfoByID(id));
            //            item.Num = PlayerData.Instance.GetItemCountByID(item.ID);
            //            CommonFunction.SetChipMark(view.Spt_ChipMark, item.SubType, _posEquipChip, _posSoldierChip);
            //            break;
            //        }
            //    case IDType.Soldier:
            //        {
            //            item = new CommonItemData(ConfigManager.Instance.mSoldierData.FindById(id));
            //            item.Num = PlayerData.Instance._SoldierDepot.FindById(item.ID).Count;
            //            break;
            //        }
            //    case IDType.EQ:
            //        {
            //            item = new CommonItemData(ConfigManager.Instance.mEquipData.FindById(id));
            //            item.Num = PlayerData.Instance.GetEquipCount(item.ID);
            //            break;
            //        }
            //    case IDType.UnionToken:
            //        {
            //            item = new CommonItemData(id, ConstString.NAME_UNIONTOKEN,
            //                ItemQualityEnum.White, PlayerData.Instance.UnionToken,
            //                GlobalConst.SpriteName.UnionToken, IDType.UnionToken, ConstString.DESC_UNIONTOKEN);
            //            break;
            //        }
            //    case IDType.Exp:
            //        {
            //            item = new CommonItemData(id, ConstString.NAME_EXP,
            //                ItemQualityEnum.White, (int)PlayerData.Instance._CurrentExp,
            //                GlobalConst.SpriteName.HeroExp, IDType.Exp, ConstString.DESC_EXP);
            //            break;
            //        }
            //    default:
            //        {
            //            item = new CommonItemData();
            //            break;
            //        }
            //}
            #endregion
            SetInfo(go, item.ID, item.Num, item.Name, item.Icon, (int)item.Quality, item.Desc, item.SuitName);
        }
        else
        {
            view.Gobj_ItemDesPage.gameObject.SetActive(false);
            UISystem.Instance.CloseGameUI(SeeDetailView.UIName);
        }
    }

    public void SeeDetail(GameObject go, bool press, CommonItemData item)
    {
        if (press)
        {
            view.Spt_ChipMark.gameObject.SetActive(false);
            int num = 0;
            switch (item.Type)
            {
                case IDType.Prop:
                    {
                        num = PlayerData.Instance.GetItemCountByID(item.ID);
                        CommonFunction.SetChipMark(view.Spt_ChipMark, item.SubType, _posEquipChip, _posSoldierChip);
                        break;
                    }
                case IDType.Soldier:
                    {
                        num = PlayerData.Instance._SoldierDepot.FindById(item.ID).Count;
                        break;
                    }
                case IDType.EQ:
                    {
                        num = PlayerData.Instance.GetEquipCount(item.ID);
                        break;
                    }
                case IDType.LifeSoul:
                    {
                        CommonFunction.SetLifeSoulMark(view.Spt_ChipMark, item.LifeSoulGodEquip, _posLifeSoulMark);
                        break;
                    }
            }
            SetInfo(go, item.ID, num, item.Name, item.Icon, (int)item.Quality, item.Desc, item.SuitName);
        }
        else
        {
            view.Gobj_ItemDesPage.gameObject.SetActive(false);
            Close(null, null);
        }

    }

    public void SeeDetail(GameObject go, bool press, ItemInfo item)
    {
        if (press)
        {
            CommonFunction.SetChipMark(view.Spt_ChipMark, (ItemTypeEnum)item.type, _posEquipChip, _posSoldierChip);
            int num = PlayerData.Instance.GetItemCountByID(item.id);
            SetInfo(go, item.id, num, item.name, item.icon, item.quality, item.desc, string.Empty);
        }
        else
        {
            view.Gobj_ItemDesPage.gameObject.SetActive(false);
            Close(null, null);
        }
    }

    public void SeeDetail(GameObject go, bool press, EquipAttributeInfo item)
    {
        if (press)
        {
            view.Spt_ChipMark.gameObject.SetActive(false);
            int num = PlayerData.Instance.GetEquipCount(item.id);
            string suitname = string.Empty;
            if (item.CoordID != 0)
            {
                EquipCoordinatesInfo info = ConfigManager.Instance.mEquipCoordinatesConfig.GetEquipCoordinatesInfoByID(item.CoordID);
                if (info != null)
                {
                    suitname = info.name;
                }

            }
            SetInfo(go, item.id, num, item.name, item.icon, item.quality, item.descript, suitname);
        }
        else
        {
            view.Gobj_ItemDesPage.gameObject.SetActive(false);
            Close(null, null);
        }
    }

    public void SeeDetail(GameObject go, bool press, MonsterAttributeInfo item)
    {
        if (press)
        {
            view.Spt_ChipMark.gameObject.SetActive(false);
            SetInfo(go, item.ID, -1, item.Name, item.HeadID, item.Star, item.Desc, string.Empty);
        }
        else
        {
            view.Gobj_ItemDesPage.gameObject.SetActive(false);
            Close(null, null);
        }
    }

    public void SeeDetail(GameObject go, bool press, CombatPetInfo item)
    {
        if (press)
        {
            view.Spt_ChipMark.gameObject.SetActive(false);
            PetData petdata = PlayerData.Instance._PetDepot.GetPetDataByID(item.id);
            int num = 0;
            if (petdata != null)
                num = 1;
            SetInfo(go, item.id, num, item.name, item.icon, item.quality, item.pet_desc, string.Empty);
        }
        else
        {
            view.Gobj_ItemDesPage.gameObject.SetActive(false);
            Close(null, null);
        }
    }

    public void SeeDetail(GameObject go, bool press, string content)
    {
        if (press)
        {
            _target = go;
            SeeDetail(content);
        }
        else
        {
            view.Gobj_StrPage.gameObject.SetActive(false);
            Close(null, null);
        }
    }

    private void SeeDetail(string content)
    {
        Vector3 position = CommonFunction.GetObjPosRelateToRootPanel(_target);

        CalculateScale(content);

        GetDetailPagePos(position);

        FixBorder();
        //PlayOpenSyrAnim();
        view.Gobj_StrPage.gameObject.SetActive(true);
        view.Gobj_StrPage.transform.localPosition = _pagePos;
    }

    private void CalculateScale(string content)
    {
        view.Lbl_StrPageContent.text = content;
        Vector2 size = view.Lbl_StrPageContent.printedSize;
        if (size.y > 80)
            view.Spt_StrPageBG.height = (int)size.y + 30;
        else
        {
            view.Spt_StrPageBG.height = 110;
        }
        _halfPageScale = new Vector2((float)view.Spt_StrPageBG.width / 2, (size.y + 30 / 2));
    }

    /*
    public void SeeDetail(GameObject go, bool press, uint id, uint num,string name, string icon,string desc)
    {
        if (CheckId(id))
            return;
        _press = press;
        if (press)
        {
            _target = go;
            view.Lbl_ItemDesPageName.text = name;
            view.Lbl_ItemDesPageOwnNum.text = num.ToString();
            SeeItemDetail(desc);
            CommonFunction.SetSpriteName(view.Spt_Icon, icon);
        }
        else
        {
            view.Gobj_ItemDesPage.gameObject.SetActive(false);
        }
    }
    */
    private void SetInfo(GameObject go, uint id, int num, string name, string icon, int quality, string desc, string suitname)
    {
        view.Gobj_SuitNameGroup.SetActive(false);
        if (CheckId(id))
            return;
        _target = go;
        view.Lbl_ItemDesPageName.text = name;
        if (num < 0)
        {
            Vector3 pos = view.Lbl_ItemDesPageName.transform.localPosition;
            view.Lbl_ItemDesPageName.transform.localPosition = new Vector3(pos.x, 25, pos.z);
            view.Lbl_ItemDesPageOwnNum.enabled = false;
            view.Lbl_ItemDesPageOwnNumTip.enabled = false;
        }
        else
        {
            Vector3 pos = view.Lbl_ItemDesPageName.transform.localPosition;
            view.Lbl_ItemDesPageName.transform.localPosition = new Vector3(pos.x, 37, pos.z);
            view.Lbl_ItemDesPageOwnNum.text = num.ToString();
            view.Lbl_ItemDesPageOwnNum.enabled = true;
            view.Lbl_ItemDesPageOwnNumTip.enabled = true;
        }
        SeeItemDetail(desc);
        CommonFunction.SetSpriteName(view.Spt_Icon, icon);
        CommonFunction.SetQualitySprite(view.Spt_Quality, quality, view.Spt_BG);
        if (string.IsNullOrEmpty(suitname))
        {
            view.Gobj_SuitNameGroup.SetActive(false);
        }
        else
        {
            view.Gobj_SuitNameGroup.SetActive(true);
            view.Lbl_SuitName.text = suitname;
        }
    }

    private void SeeItemDetail(string desc)
    {
        Vector3 position = CommonFunction.GetObjPosRelateToRootPanel(_target);

        CalculateItemPageScale(desc);

        GetDetailPagePos(position);

        FixBorder();

        view.Gobj_ItemDesPage.gameObject.SetActive(true);
        // PlayOpenItemAnim();
        view.Gobj_ItemDesPage.transform.localPosition = _pagePos;

    }

    private void CalculateItemPageScale(string desc)
    {
        view.Lbl_ItemDesPageDesc.text = desc;
        Vector2 size = view.Lbl_ItemDesPageDesc.printedSize;

        if (size.y <= 36)   // 36/18=2 行字的时候使用最小的面板//
        {
            //view.Spt_ItemDesPageFG.height = 56;
            view.Spt_ItemDesPageBG.height = 150;

        }
        else
        {
            //view.Spt_ItemDesPageFG.height = (int)size.y + 20;
            view.Spt_ItemDesPageBG.height = (int)size.y + 112;
        }

        _halfPageScale = new Vector2(view.Spt_ItemDesPageBG.width / 2, view.Spt_ItemDesPageBG.height / 2);
    }

    private void GetDetailPagePos(Vector3 position)
    {
        EDetailAnchor anchor = GetDetailAnchor(position);
        switch (anchor)
        {
            case EDetailAnchor.BottomRight:
                {
                    _pagePos = new Vector3(position.x + _halfPageScale.x + _fingerOffset.x, position.y - _halfPageScale.y - _fingerOffset.y, 0);
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
                    _pagePos = new Vector3(position.x + _halfPageScale.x + _fingerOffset.x, position.y + _halfPageScale.y + _fingerOffset.y, 0);
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

    private bool CheckId(uint id)
    {
        return id == 0;
    }

    public override void Uninitialize()
    {

    }

    public override void Destroy()
    {
        view = null;
    }

    public void BtnEventBinding()
    {

    }

    //界面动画
    //public void PlayOpenItemAnim()
    //{
    //    view.Item_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Item_TScale.Restart(); view.Item_TScale.PlayForward();
    //}
    //public void PlayOpenSyrAnim()
    //{
    //    view.Str_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Str_TScale.Restart(); view.Str_TScale.PlayForward();
    //}
}
