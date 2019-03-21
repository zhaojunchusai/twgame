using System;
using System.IO;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using fogs.proto.msg;
using System.Xml;

public static class CommonFunction
{
    /// <summary>
    /// 游戏物体渲染开关//
    /// </summary>
    /// <param name="vGo"></param>
    /// <param name="vRender"></param>
    public static void RenaderObject(GameObject vGo, bool vRender)
    {
        if (vGo == null)
            return;
        foreach (Renderer rend in vGo.GetComponentsInChildren<Renderer>())
        {
            rend.enabled = vRender;
        }
    }

    /// <summary>
    /// 改变2D Character颜色//
    /// </summary>
    /// <param name="vColor"></param>
    /// <param name="vGO"></param>
    public static void ChangeCharacter2DColor(Color vColor, GameObject vGO)
    {
        if (null == vGO) return;
        SpriteRenderer[] spriteRenderers = vGO.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = vColor;
        }
    }

    public static Color ToColor(int r, int g, int b, int a = 255)
    {
        Color color = new Color();
        color.r = (float)r / 255f;
        color.g = (float)g / 255f;
        color.b = (float)b / 255f;
        color.a = (float)a / 255f;
        return color;
    }

    /// <summary>
    /// 改变游戏物体颜色//
    /// </summary>
    /// <param name="vColor"></param>
    /// <param name="vGO"></param>
    public static void ChangeObjColor(Color vColor, GameObject vGO)
    {
        if (null == vGO) return;
        UIWidget[] tWidget = vGO.GetComponentsInChildren<UIWidget>();
        for (int i = 0; i < tWidget.Length; i++)
        {
            tWidget[i].color = vColor;
        }
        Material mMat;
        Renderer ren = vGO.renderer;
        if (ren != null)
        {
            mMat = ren.material;
            if (mMat)
                mMat.color = vColor;
        }
    }

    /// <summary>
    /// 改变游戏物体透明度//
    /// </summary>
    /// <param name="alpha">0-255 </param>
    /// <param name="vGO"></param>
    public static void ChangeObjAlpha(float alpha, GameObject vGO)
    {
        if (vGO.GetComponent<UIPanel>() != null)
        {
            vGO.GetComponent<UIPanel>().alpha = alpha;
        }
        else
        {
            UIWidget[] tWidget = vGO.GetComponentsInChildren<UIWidget>();
            for (int i = 0; i < tWidget.Length; i++)
            {
                tWidget[i].alpha = alpha;
            }
        }
    }

    public static void SetBtnState(GameObject btn, bool state)
    {
        if (btn.GetComponent<UIButton>() != null)
        {
            btn.GetComponent<UIButton>().enabled = state;
        }

        if (btn.GetComponent<BoxCollider>() != null)
        {
            btn.GetComponent<BoxCollider>().enabled = state;
        }

        if (btn.GetComponentInChildren<UISprite>() != null)
        {
            btn.GetComponentInChildren<UISprite>().color = state ? Color.white : Color.black;
        }
    }

    /// <summary>
    /// 将毫秒转换为秒并返回
    /// </summary>
    /// <param name="vMilliSecond">毫秒</param>
    /// <returns></returns>
    public static float GetSecondTimeByMilliSecond(float vMilliSecond)
    {
        return vMilliSecond / 1000;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public static string GetDateTimeString(long second)
    {
        System.DateTime time = System.DateTime.MinValue;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        time = startTime.AddSeconds(second);
        string str = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hour, time.Minute, time.Second);
        return str;
    }

    public static System.DateTime GetDateTime(long second)
    {
        System.DateTime time = System.DateTime.MinValue;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        time = startTime.AddSeconds(second);
        return time;
    }

    /// <summary>
    /// 将秒转换为字符串显示时间[时:分:秒]
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static string GetTimeString(long second)
    {
        if (second < 0)
            second = 0;
        if (second / 3600 > 99)
            return string.Format(ConstString.FORMAT_TIME_LONG, (second) / 3600, (second % 3600) / 60, (second % 3600) % 60);
        else
            return string.Format(ConstString.FORMAT_TIME, (second) / 3600, (second % 3600) / 60, (second % 3600) % 60);
    }
    /// <summary>
    /// 将时间字符串[时:分:秒]转换为秒
    /// </summary>
    /// <param name="times"></param>
    /// <returns></returns>
    public static long GetTimeInt(string times)
    {
        if (times != null)
        {
            string[] s = times.Split(':');
            if (s.Length == 3)
            {
                return int.Parse(s[0]) * 3600 + int.Parse(s[1]) * 60 + int.Parse(s[2]);
            }
            else
            {
                Debug.LogError("time format error");
                return -1;
            }

        }
        else
        {
            Debug.LogError("time empty");
            return -1;
        }

    }
    /// <summary>
    /// 将秒转换为字符串显示取整时间 如12:20  返回12小时 
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static string GetIntegerTimeString(long second)
    {
        int day = (int)(second / 86400);
        if (day > 0)
        {
            return string.Format(ConstString.FORMAT_INTEGERTIME_DAY, day);
        }
        int hour = (int)(second / 3600);
        if (hour > 0)
        {
            return string.Format(ConstString.FORMAT_INTEGERTIME_HOUR, hour);
        }
        int min = (int)(second / 60);
        return string.Format(ConstString.FORMAT_INTEGERTIME_MIN, min);
    }

    /// <summary>
    /// [1天23：59：48]
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static string GetTimeStringByDay(long second)
    {
        string str = "";
        long day = second / 86400;
        if (day > 0)
        {
            str = string.Format(ConstString.FORMAT_INTEGERTIME_DAY, day);
        }
        long time = second % 86400;
        str += GetTimeString(time);
        return str;
    }

    public static string GetDayString(long second)
    {
        return string.Format(ConstString.TASK_DAY, second / 86400);
    }

    /// <summary>
    /// 将秒转换为字符串显示时间[分:秒]
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static string GetTimeStringNoHours(long second)
    {
        return string.Format(ConstString.FORMAT_TIME_NOHOURS, (second % 3600) / 60, (second % 3600) % 60);
    }

    /// <summary>
    /// 服务端返回数据时，判断掉落物品 武将，装备格子，物品叠加是否超限
    /// showTip 默认为true 会弹出提示文字
    /// </summary>
    public static string ShowItemOverflowTip(List<DropList> list, bool showTip = true)
    {
        string tip = string.Empty;
        if (list == null || list.Count == 0)
            return tip;
        List<EDropTipType> dropList = new List<EDropTipType>();
        for (int i = 0; i < list.Count; i++)
        {
            List<Attachment> mailList = list[i].mail_list;
            if (mailList == null) continue;
            for (int j = 0; j < mailList.Count; j++)
            {
                Attachment info = mailList[j];
                if (info == null)
                    continue;
                IDType idType = GetTypeOfID(info.id.ToString());
                List<EDropTipType> add = new List<EDropTipType>();
                if ((idType == IDType.Soldier) || (idType == IDType.EQ) || (idType == IDType.Prop))
                {
                    dropList = CheckTipType(dropList, add);
                }
            }
        }
        tip = GetOverflowTip(dropList);
        if (showTip)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, tip);
        }
        return tip;
    }

    /// <summary>
    /// 服务端返回数据时，判断掉落物品 武将，装备格子，物品叠加是否超限
    /// showTip 默认为true 会弹出提示文字
    /// </summary>
    public static string ShowItemOverflowTip(List<Attachment> mailList, bool showTip = true)
    {
        string tip = string.Empty;
        if (mailList != null && mailList.Count > 0)
        {
            List<EDropTipType> list = new List<EDropTipType>();
            for (int j = 0; j < mailList.Count; j++)
            {
                Attachment info = mailList[j];
                if (info == null)
                    continue;
                IDType idType = GetTypeOfID(info.id.ToString());
                List<EDropTipType> add = new List<EDropTipType>();
                if ((idType == IDType.Soldier) || (idType == IDType.EQ) || (idType == IDType.Prop))
                {
                    list = CheckTipType(list, add);
                }
            }
            tip = GetOverflowTip(list);
            if (showTip)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, tip);
            }
        }
        return tip;
    }

    /// <summary>
    /// 取得越界提示
    /// </summary>
    private static string GetOverflowTip(List<EDropTipType> list)
    {
        string tip = string.Empty;
        if (list.Contains(EDropTipType.Equip) && (list.Contains(EDropTipType.Item) == false) && (list.Contains(EDropTipType.Soldier)))
        {
            tip = ConstString.BACKPACK_EQUIPTIP_OVERFLOW;
            return tip;
        }
        else if (list.Contains(EDropTipType.Item) && (list.Contains(EDropTipType.Equip) == false) && (list.Contains(EDropTipType.Soldier)))
        {
            tip = ConstString.BACKPACK_ITEMTIP_OVERFLOW;
            return tip;
        }
        else if (list.Contains(EDropTipType.Soldier) && (list.Contains(EDropTipType.Equip) == false) && (list.Contains(EDropTipType.Item)))
        {
            tip = ConstString.BACKPACK_SOLDIERTIP_OVERFLOW;
            return tip;
        }
        else
        {
            StringBuilder sub = new StringBuilder();
            if (list.Contains(EDropTipType.Item))
            {
                sub.Append(ConstString.BACKPACK_ITEMTIP_OVERFLOW_P);
            }
            if (list.Contains(EDropTipType.Equip))
            {
                sub.Append(ConstString.BACKPACK_EQUIPTIP_OVERFLOW_P);
            }
            if (list.Contains(EDropTipType.Soldier))
            {
                sub.Append(ConstString.BACKPACK_SOLDIERTIP_OVERFLOW_P);
            }
            sub.Append(ConstString.BACKPACK_TIP_OVERFLOW_S);
            tip = sub.ToString();
        }
        return tip;
    }

    private static List<EDropTipType> CheckTipType(List<EDropTipType> currentList, List<EDropTipType> nextList)
    {
        if (currentList == null)
        {
            currentList = new List<EDropTipType>();
        }
        if (nextList == null)
        {
            nextList = new List<EDropTipType>();
        }
        List<EDropTipType> list = new List<EDropTipType>(currentList);
        for (int j = 0; j < nextList.Count; j++)
        {
            EDropTipType next = nextList[j];
            if (next != EDropTipType.None)
            {
                if (currentList.Contains(next) == false)
                {
                    currentList.Add(next);
                }
            }
        }
        return list;
    }

    /// <summary>
    /// 领取奖励判断金币，钻石，武将，装备格子，物品叠加等是否超限
    /// 返回true 则说明超限
    /// showTip 默认为true 会弹出提示文字
    /// </summary>
    public static bool GetItemOverflowTip(List<CommonItemData> list, bool showTip = true)
    {
        bool status = false;
        if (list == null || list.Count <= 0)
            return status;
        string tip = string.Empty;
        for (int i = 0; i < list.Count; i++)
        {
            CommonItemData data = list[i];
            if (data == null) continue;
            if (data.Type == IDType.Gold)
            {
                if (PlayerData.Instance.IsGoldOverflow(data.Num))
                {
                    status = true;
                    tip = ConstString.BACKPACK_TIP_GOLDOVERFLOW;
                    break;
                }
            }
            if (data.Type == IDType.Diamond)
            {
                if (PlayerData.Instance.IsDiamondOverflow(data.Num))
                {
                    status = true;
                    tip = ConstString.BACKPACK_TIP_DIAMONDOVERFLOW;
                    break;
                }
            }
            if (data.Type == IDType.Soldier)
            {
                if (PlayerData.Instance.IsSoldierFull(data.Num))
                {
                    status = true;
                    tip = ConstString.BACKPACK_TIP_SOLDIEROVERFLOW;
                    break;
                }
            }
            if (data.Type == IDType.Prop)
            {
                Item item = PlayerData.Instance.GetOwnItemByID(data.ID);
                if (item == null)
                {
                    Debris tmpDe = PlayerData.Instance._SoldierDebrisDepot.FindByid(data.ID);
                    if (tmpDe != null)
                    {
                        if ((tmpDe.count + data.Num) > GlobalConst.MAX_Item_Spill)
                        {
                            status = true;
                            tip = string.Format(ConstString.BACKPACK_TIP_PROPOVERFLOW, tmpDe.Att.name);
                            break;
                        }
                    }
                }
                else
                {
                    if ((item.num + data.Num) > GlobalConst.MAX_Item_Spill)
                    {
                        status = true;
                        ItemInfo info = ConfigManager.Instance.mItemData.GetItemInfoByID(item.id);
                        if (info != null)
                        {
                            tip = string.Format(ConstString.BACKPACK_TIP_PROPOVERFLOW, info.name);
                        }
                        break;
                    }
                }
            }
            if (data.Type == IDType.EQ)
            {
                if (PlayerData.Instance.IsEquipGridOverflow(data.Num))
                {
                    status = true;
                    EquipAttributeInfo info = ConfigManager.Instance.mEquipData.FindById(data.ID);
                    if (info != null)
                    {
                        tip = string.Format(ConstString.BACKPACK_TIP_EQUIPOVERFLOW, info.name);
                    }
                    break;
                }
            }
        }
        if (showTip && status)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, tip);
        }
        return status;
    }

    public static DateTime GetTimeByLong(long time)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(time + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    /// <summary>
    /// 获得文件MD5//
    /// </summary>
    /// <param name="vFilePath"></param>
    /// <param name="vFileMD5"></param>
    /// <returns></returns>
    public static bool GetFileMD5(string vFilePath, out string vFileMD5)
    {
        vFileMD5 = "";
        if (!File.Exists(vFilePath))
        {
            return false;
        }
        try
        {
            MD5CryptoServiceProvider md5Generator = new MD5CryptoServiceProvider();
            FileStream tFile = new FileStream(vFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] hash = md5Generator.ComputeHash(tFile);
            vFileMD5 = System.BitConverter.ToString(hash);
            tFile.Close();
            return true;
        }
        catch (Exception ex)
        {
            vFileMD5 = "";
            Debug.LogError(ex.Message);
            return false;
        }
    }

    /// <summary>
    /// 写入文件//
    /// </summary>
    /// <param name="vBytes"></param>
    /// <param name="vFilePath"></param>
    /// <returns></returns>
    public static bool WriteFile(byte[] vBytes, string vFilePath)
    {
        try
        {
            if (!Directory.Exists(Path.GetDirectoryName(vFilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(vFilePath));
            if (File.Exists(vFilePath))
                File.Delete(vFilePath);
            FileStream fs = new FileStream(@vFilePath, FileMode.Create);
            fs.Seek(0, SeekOrigin.Begin);
            fs.Write(vBytes, 0, vBytes.Length);
            fs.Flush();
            fs.Close();
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }
    }


    public static byte[] ReadFile(string vFilePath)
    {
        using (FileStream fsRead = new FileStream(vFilePath, FileMode.Open, FileAccess.Read))
        {
            fsRead.Seek(0, SeekOrigin.Begin);
            byte[] result = new byte[(int)fsRead.Length];
            fsRead.Read(result, 0, result.Length);
            fsRead.Flush();
            fsRead.Close();
            return result;
        }
    }

    /// <summary>
    /// 替换换行符 配置表读取出来的\n 需要二次转换 add by taiwei
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string ReplaceEscapeChar(string text)
    {
        string str = string.Empty;
        str = text.Replace("/n", "\n");
        str = str.Replace("\n", "\n");
        str = str.Replace("\\n", "\n");
        str = str.Replace("\t", "\t");
        str = str.Replace("/t", "\t");

        return str;
    }


    /// <summary>
    /// 给一个Label添加新行  
    /// </summary>
    public static void AddNewLine(UILabel label, string text)
    {
        string str = label.text;

        if (str.Length > 0 && !str.EndsWith("\n"))
        {
            str += "\n";
        }
        str += text;
        label.text = str;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetMD5Hash(string str)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] res = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str), 0, str.Length);
        return System.BitConverter.ToString(res);
    }


    public static string GetMD5Hash<T>(T t)
    {
        MemoryStream msg = new MemoryStream();
        ProtoBuf.Serializer.Serialize<T>(msg, t);
        msg.Seek(0, SeekOrigin.Begin);
        byte[] msgdata = new byte[msg.Length];
        msg.Read(msgdata, 0, msgdata.Length);
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] res = md5.ComputeHash(msgdata);
        return System.BitConverter.ToString(res);
    }

    public static int GetEncKey(long key)
    {
        int result = 0;
        string str = key.ToString();
        int value = 0;
        for (int i = 0; i < str.Length; i++)
        {
            value = int.Parse(str[i].ToString());
            result += (int)(Math.Pow(value, 2) + value);
        }
        return result;
    }

    static public GameObject InstantiateObject(GameObject prefab, Transform parent)
    {
        if (prefab == null)
            return null;
        GameObject clone = GameObject.Instantiate(prefab) as GameObject;
        if (clone != null && parent != null)
        {
            clone.layer = parent.gameObject.layer;
            clone.transform.parent = parent;
            clone.transform.localPosition = Vector3.zero;
            clone.transform.localRotation = Quaternion.identity;
            clone.transform.localScale = prefab.transform.localScale;
        }
        return clone;
    }
    static public GameObject SetParent(GameObject prefab, Transform parent)
    {
        if (prefab == null)
            return null;
        GameObject clone = prefab;
        if (clone != null && parent != null)
        {
            clone.layer = parent.gameObject.layer;
            clone.transform.parent = parent;
            clone.transform.localPosition = Vector3.zero;
            clone.transform.localRotation = Quaternion.identity;
            clone.transform.localScale = prefab.transform.localScale;
            clone.SetActive(true);
        }
        return clone;
    }
    /// <summary>
    /// 获取物体相对于UISystem中根Panel的位置
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static Vector3 GetObjPosRelateToRootPanel(GameObject go)
    {
        Vector3 result;
        Vector3 panelpos = UISystem.Instance.parentTran.position;
        Vector3 gopos = go.transform.position;
        result = (gopos - panelpos) * 576 / 2;
        return result;
    }

    /// <summary>
    /// 改变NGUI物件色彩度（修改shader后实现置灰效果）  
    /// PS:目前是通过修改ransparent Colored实现置灰效果 
    /// </summary>
    /// <param name="mTarget">目标物体</param>
    /// <param name="mIsGray">是否置灰</param>
    public static void UpdateWidgetGray(UIWidget mTarget, bool mIsGray)
    {
        if (mTarget == null) return;
        mTarget.color = mIsGray ? Color.black : Color.white;
    }
    public static void SetGameObjectGray(GameObject obj, bool mIsGray)
    {
        if (obj == null)
            return;

        //UILabel[] tmp = obj.GetComponentsInChildren<UILabel>();
        //foreach (UILabel tt in tmp)
        //{
        //    SetUILabelColor(tt, !mIsGray);
        //}

        UIButton[] tmpBtn = obj.GetComponentsInChildren<UIButton>();
        foreach (var item in tmpBtn)
        {
            item.enabled = !mIsGray;
        }

        UISprite[] tmpSp = obj.GetComponentsInChildren<UISprite>();
        foreach (UISprite tt in tmpSp)
        {
            UpdateWidgetGray(tt, mIsGray);
        }

    }
    /// <summary>
    /// 消耗品、材料排序规则为：按品质由高到低排序，同品质道具按道具编号从小到大排序
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<fogs.proto.msg.Item> SortItemByQuality(List<fogs.proto.msg.Item> list)
    {
        List<fogs.proto.msg.Item> items = new List<fogs.proto.msg.Item>(list.ToArray());
        items.Sort((fogs.proto.msg.Item left, fogs.proto.msg.Item right) =>
        {
            if (left == null || right == null)
            {
                return 0;
            }
            ItemInfo item_l = ConfigManager.Instance.mItemData.GetItemInfoByID(left.id);
            ItemInfo item_r = ConfigManager.Instance.mItemData.GetItemInfoByID(right.id);
            if (item_l != null && item_r != null)
            {
                if (item_l.quality < item_r.quality)
                {
                    return 1;
                }
                else if (item_l.quality > item_r.quality)
                {
                    return -1;
                }
                else
                {
                    if (item_l.id < item_r.id)
                    {
                        return 1;
                    }
                    else if (item_l.id > item_r.id)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else
            {
                return 0;
            }
        });
        return items;
    }

    /// <summary>
    /// 将指定Texture置灰
    /// </summary>
    /// <param name="mTexture">目标Texture</param>
    /// <param name="mIsGray">是否置灰</param>
    public static void UpdateTextureGray(UITexture mTexture, bool mIsGray)
    {
        Shader shader = mIsGray ? Shader.Find("Unlit/Transparent Colored Gray") : Shader.Find("Unlit/Transparent Colored");
        mTexture.material = new Material(shader);
    }

    /// <summary>
    /// 通过ID获取物体图标
    /// </summary>
    /// <param name="vID"></param>
    /// <returns></returns>
    public static string GetIconNameByID(uint vID)
    {
        string tmpResult = string.Empty;
        IDType tmpType = GetTypeOfID(vID.ToString());
        switch (tmpType)
        {
            case IDType.Soldier:
                {
                    SoldierAttributeInfo tmpSoldier = ConfigManager.Instance.mSoldierData.FindById(vID);
                    if (tmpSoldier != null)
                        tmpResult = tmpSoldier.Icon;
                }
                break;
            case IDType.EQ:
                {
                    EquipAttributeInfo tmpEquip = ConfigManager.Instance.mEquipData.FindById(vID);
                    if (tmpEquip != null)
                        tmpResult = tmpEquip.icon;
                }
                break;
            case IDType.Prop:
                {
                    ItemInfo tmpItem = ConfigManager.Instance.mItemData.GetItemInfoByID(vID);
                    if (tmpItem != null)
                        tmpResult = tmpItem.icon;
                }
                break;
            case IDType.SP:
                {
                    tmpResult = GlobalConst.SpriteName.SP;
                }
                break;
            case IDType.Gold:
                {
                    tmpResult = GlobalConst.SpriteName.Gold;
                }
                break;
            case IDType.Diamond:
                {
                    tmpResult = GlobalConst.SpriteName.Diamond;
                }
                break;
            case IDType.Medal:
                {
                    tmpResult = GlobalConst.SpriteName.Medal;
                }
                break;
            case IDType.Honor:
                {
                    tmpResult = GlobalConst.SpriteName.Honor;
                }
                break;
            case IDType.None:
            default:
                { }
                break;
        }
        return tmpResult;
    }

    public static int GetQualityByID(uint vID)
    {
        int tmpResult = 1;
        IDType tmpType = GetTypeOfID(vID.ToString());
        switch (tmpType)
        {
            case IDType.Soldier:
                {
                    SoldierAttributeInfo tmpSoldier = ConfigManager.Instance.mSoldierData.FindById(vID);
                    if (tmpSoldier != null)
                        tmpResult = tmpSoldier.quality;
                }
                break;
            case IDType.EQ:
                {
                    EquipAttributeInfo tmpEquip = ConfigManager.Instance.mEquipData.FindById(vID);
                    if (tmpEquip != null)
                        tmpResult = tmpEquip.quality;
                }
                break;
            case IDType.Prop:
                {
                    ItemInfo tmpItem = ConfigManager.Instance.mItemData.GetItemInfoByID(vID);
                    if (tmpItem != null)
                        tmpResult = tmpItem.quality;
                }
                break;
            default:
                { }
                break;
        }
        return tmpResult;
    }

    /// <summary>
    /// 通过战斗类型获取战斗名
    /// </summary>
    /// <param name="vFightType"></param>
    /// <returns></returns>
    public static string GetFightNameByType(EFightType vFightType)
    {
        string tmpResult = string.Empty;

        switch (vFightType)
        {
            case EFightType.eftMain:
                {
                    tmpResult = ConstString.FIGHTNAME_MAIN;
                }
                break;
            case EFightType.eftActivity:
                {
                    tmpResult = ConstString.FIGHTNAME_ACTIVITY;
                }
                break;
            case EFightType.eftEndless:
                {
                    tmpResult = ConstString.FIGHTNAME_ENDLESS;
                }
                break;
            case EFightType.eftExpedition:
                {
                    tmpResult = ConstString.FIGHTNAME_EXPEDITION;
                }
                break;
            case EFightType.eftPVP:
                {
                    tmpResult = ConstString.FIGHTNAME_PVP;
                }
                break;
            case EFightType.eftSlave:
                { }
                break;
            case EFightType.eftUnion:
                { }
                break;
            case EFightType.eftNewGuide:
                {
                    tmpResult = ConstString.FIGHTNAME_NEWGUIDE;
                }
                break;
            case EFightType.eftCaptureTerritory:
                { }
                break;
            case EFightType.eftServerHegemony:
                { }
                break;
            case EFightType.eftQualifying:
                { }
                break;
            case EFightType.eftCrossServerWar:
                { }
                break;
            default:
                { }
                break;
        }

        return tmpResult;
    }

    /// <summary>
    /// 获取战斗满星条件
    /// </summary>
    /// <param name="vFightType"></param>
    /// <param name="vStageInfo"></param>
    /// <returns></returns>
    public static string GetFightMaxCondition(EFightType vFightType, StageInfo vStageInfo)
    {
        string tmpResult = string.Empty;

        if (vStageInfo != null)
        {
            int tmpMaxStar = vStageInfo.Star3;
            int tmpNorTime = vStageInfo.NormTime;
            switch (vFightType)
            {
                case EFightType.eftMain:
                    {
                        switch ((EPVESceneType)vStageInfo.FireType)
                        {
                            case EPVESceneType.epvestAttack:
                                {
                                    tmpResult = string.Format(ConstString.FIGHTVIEW_CONDITION_ATTACK, tmpMaxStar + tmpNorTime);
                                }
                                break;
                            case EPVESceneType.epvestDefen:
                                {
                                    tmpResult = string.Format(ConstString.FIGHTVIEW_CONDITION_DEFENSE, tmpMaxStar);
                                }
                                break;
                            case EPVESceneType.epvestEscort:
                                {
                                    tmpResult = string.Format(ConstString.FIGHTVIEW_CONDITION_ESCORT, tmpMaxStar);
                                }
                                break;
                            case EPVESceneType.epvestTransfer:
                                {
                                    tmpResult = string.Format(ConstString.FIGHTVIEW_CONDITION_TRANSFER, tmpMaxStar);
                                }
                                break;
                            default:
                                { }
                                break;
                        }

                    }
                    break;
                case EFightType.eftActivity:
                    {
                        tmpResult = string.Format("", 0);
                    }
                    break;
                case EFightType.eftEndless:
                    {
                        tmpResult = string.Format("", 0);
                    }
                    break;
                case EFightType.eftExpedition:
                    {
                        tmpResult = string.Format("", 0);
                    }
                    break;
                case EFightType.eftPVP:
                    {
                        tmpResult = string.Format("", 0);
                    }
                    break;
                case EFightType.eftSlave:
                    {
                        tmpResult = string.Format("", 0);
                    }
                    break;
                case EFightType.eftUnion:
                    {
                        tmpResult = string.Format("", 0);
                    }
                    break;
                case EFightType.eftCaptureTerritory:
                    {
                        tmpResult = string.Format("", 0);
                    }
                    break;
                case EFightType.eftServerHegemony:
                    {
                        tmpResult = string.Format("", 0);
                    }
                    break;
                case EFightType.eftQualifying:
                    {
                        tmpResult = string.Format("", 0);
                    }
                    break;
                case EFightType.eftCrossServerWar:
                    {
                        tmpResult = string.Format("", 0);
                    }
                    break;
                default:
                    { }
                    break;
            }
        }

        return tmpResult;
    }

    /// <summary>
    /// 取得士兵类型标识ICON
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetSoldierTypeIcon(int type)
    {
        string icon = string.Empty;
        switch ((ESoldierType)type)
        {
            case ESoldierType.Defense:
                icon = GlobalConst.SpriteName.SoldierType_Defense;
                break;
            case ESoldierType.Suport:
                icon = GlobalConst.SpriteName.SoldierType_Suport;
                break;
            case ESoldierType.Attack:
                icon = GlobalConst.SpriteName.SoldierType_Attack;
                break;
            case ESoldierType.Control:
                icon = GlobalConst.SpriteName.SoldierType_Control;
                break;
            default:
                Debug.LogError("error soldier type!!");
                break;
        }
        return icon;
    }
    /// <summary>
    /// 取得士兵类型标识ICON
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetEquipTypeIcon(int type)
    {
        string icon = string.Empty;
        switch ((EquiptType)type)
        {
            case EquiptType._Weapon:
                icon = GlobalConst.SpriteName.EquipType_Weapon;
                break;
            case EquiptType._ring:
                icon = GlobalConst.SpriteName.EquipType_Ring;
                break;
            case EquiptType._necklace:
                icon = GlobalConst.SpriteName.EquipType_Necklace;
                break;
            default:
                Debug.LogError("error equip type!!");
                break;
        }
        return icon;
    }

    public static List<StageInfo> SortStages(List<uint> stages)
    {
        List<StageInfo> _list = new List<StageInfo>();
        for (int i = 0; i < stages.Count; i++)
        {
            StageInfo _info = ConfigManager.Instance.mStageData.GetInfoByID(stages[i]);
            if (_info != null)
            {
                StageInfo _stage = new StageInfo();
                _stage.CopyTo(_info);
                _list.Add(_stage);
            }
        }
        _list = SortStages(_list);
        return _list;
    }

    /// <summary>
    /// 关卡排序
    /// </summary>
    /// <param name="stages"></param>
    /// <returns></returns>
    public static List<StageInfo> SortStages(List<StageInfo> stages)
    {
        return SortStagesByID(stages);
        //List<StageInfo> _list = new List<StageInfo>();
        //if (stages == null || stages.Count == 0) return _list;
        //List<uint> _array = new List<uint>();
        //for (int i = 0; i < stages.Count; i++)
        //{
        //    _array.Add(stages[i].ID);
        //}
        //StageInfo _lastinfo = null;
        //for (int i = 0; i < stages.Count; i++)
        //{
        //    if (!_array.Contains(stages[i].PrevID))   //取得当前列表第一项
        //    {
        //        _lastinfo = stages[i];
        //        break;
        //    }
        //}
        //_list.Add(_lastinfo);
        //StageInfo tmp = new StageInfo();
        //tmp.CopyTo(_lastinfo);
        //for (int i = 1; i < stages.Count; i++)
        //{
        //    if (tmp.NextID != 0)
        //    {
        //        StageInfo _info = ConfigManager.Instance.mStageData.GetInfoByID(tmp.NextID);
        //        _list.Add(_info);
        //        tmp = _info;
        //    }
        //}
        //return _list;
    }

    private static List<StageInfo> SortStagesByID(List<StageInfo> stages)
    {
        List<StageInfo> _list = new List<StageInfo>();
        if (stages == null || stages.Count == 0) return _list;
        stages.Sort((StageInfo leftStage, StageInfo rightStage) =>
        {
            if (leftStage == null || rightStage == null)
            {
                return 0;
            }
            else if (leftStage.ID < rightStage.ID)
            {
                return -1;
            }
            else if (leftStage.ID == rightStage.ID)
            {
                return 0;
            }
            else if (leftStage.ID > rightStage.ID)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        });
        _list = new List<StageInfo>(stages);
        return _list;

    }

    /// <summary>
    /// 是否为金币ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsCoinID(uint id)
    {
        return GlobalCoefficient.CoinID.Equals(id);
    }

    /// <summary>
    /// 是否为军团勋章ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsUnionTokenID(uint id)
    {
        return GlobalCoefficient.UnionTokenID.Equals(id);
    }

    /// <summary>
    /// 是否为宝石ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsGemID(uint id)
    {
        return GlobalCoefficient.GemID.Equals(id);
    }

    /// <summary>
    /// 是否为体力ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsSPID(uint id)
    {
        return GlobalCoefficient.SpID.Equals(id);
    }

    /// <summary>
    /// 是否为玩家经验ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsPlayerExpID(uint id)
    {
        return GlobalCoefficient.PlayerExpID.Equals(id);
    }

    /// <summary>
    /// 是否为士兵经验ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsSoldierExpID(uint id)
    {
        return GlobalCoefficient.SoldierExpID.Equals(id);
    }

    /// <summary>
    /// 竞技场积分ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsArenaIntegralID(uint id)
    {
        return GlobalCoefficient.ArenaIntegralID.Equals(id);
    }

    /// <summary>
    /// 远征积分ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsExpeditionIntegralID(uint id)
    {
        return GlobalCoefficient.ExpeditionIntegralID.Equals(id);
    }

    /// <summary>
    /// 检测战斗类型是否合法
    /// </summary>
    /// <param name="vFightType"></param>
    /// <returns></returns>
    public static bool CheckFightType(EFightType vFightType)
    {
        if ((EFightType.eftMain == vFightType) || (EFightType.eftActivity == vFightType) ||
            (EFightType.eftEndless == vFightType) || (EFightType.eftExpedition == vFightType) ||
            (EFightType.eftPVP == vFightType) || (EFightType.eftNewGuide == vFightType) ||
            (EFightType.eftSlave == vFightType) || (EFightType.eftUnion == vFightType) ||
            (EFightType.eftCaptureTerritory == vFightType) || (EFightType.eftServerHegemony == vFightType) ||
            (EFightType.eftQualifying == vFightType) || (EFightType.eftCrossServerWar == vFightType))
            return true;
        return false;
    }

    public static string ConvertToWeek(WeekEnum week)
    {
        switch (week)
        {
            case WeekEnum.Monday:
                return ConstString.HINT_WEEK_Monday;
            case WeekEnum.Tuesday:
                return ConstString.HINT_WEEK_Tuesday;
            case WeekEnum.Wednesday:
                return ConstString.HINT_WEEK_Wednesday;
            case WeekEnum.Thursday:
                return ConstString.HINT_WEEK_Thursday;
            case WeekEnum.Friday:
                return ConstString.HINT_WEEK_Friday;
            case WeekEnum.Saturday:
                return ConstString.HINT_WEEK_Saturday;
            case WeekEnum.Sunday:
                return ConstString.HINT_WEEK_Sunday;
            default:
                return string.Empty;
        }

    }
    /// <summary>
    /// 检测敌方状态是否正确
    /// </summary>
    /// <param name="vEnemyAttribute">敌方信息</param>
    /// <returns></returns>
    private static bool CheckEnemyIsRight(RoleAttribute vEnemyAttribute)
    {
        //检测目标是否存在//
        if (vEnemyAttribute == null)
            return false;
        //检测目标是否存活//
        if (!vEnemyAttribute.IsLive())
            return false;
        //检测目标角色状态是否正确//
        RoleBase tmpRoleBase = vEnemyAttribute.GetComponent<RoleBase>();
        if (tmpRoleBase != null)
        {
            string tmpRoleName = tmpRoleBase.Get_CurrentActionName;
            if (string.IsNullOrEmpty(tmpRoleName))
                return false;
            if (tmpRoleName.Equals(typeof(ActionDeath).Name))
                return false;
        }
        return true;
    }

    /// <summary>
    /// 检测一个物件是否在攻击范围内
    /// </summary>
    /// <param name="vFireAttribute">攻击方信息</param>
    /// <param name="vTarget">目标物件</param>
    /// <returns>true-在攻击范围内 false-不在攻击范围内</returns>
    public static bool CheckObjInAttackRange(RoleAttribute vFireAttribute, RoleAttribute vTarget)
    {
        if ((vFireAttribute == null) || (vTarget == null))
            return false;
        List<RoleAttribute> tmpList = FindHitFightObjects(vFireAttribute, vTarget.Get_RoleCamp);
        if (tmpList == null)
            return false;
        for (int i = 0; i < tmpList.Count; i++)
        {
            if (tmpList[i].gameObject.name.Equals(vTarget.name))
                return true;
        }
        return false;
    }

    /// <summary>
    /// 查找场景中单个碰撞物件
    /// </summary>
    /// <param name="vSelfAttribute">自身物件信息</param>
    /// <param name="vFightCamp">查找匹配阵营-没有阵营筛选就不填</param>
    /// <param name="vIsDistance">是否使用配置表距离[false-不适用配置表数据]</param>
    /// <returns></returns>
    public static RoleAttribute FindHitSingleFightObject(RoleAttribute vSelfAttribute, EFightCamp vFightCamp = EFightCamp.efcNone, bool vIsDistance = true)
    {
        if (vSelfAttribute == null)
            return null;

        //获取碰撞物件列表//
        List<RoleAttribute> tmpList = FindHitFightObjects(vSelfAttribute, vFightCamp, vIsDistance);
        if (tmpList == null)
            return null;
        if (tmpList.Count <= 0)
            return null;

        //筛选最近物件//
        int tmpIndex = 0;
        float tmpDis = Mathf.Abs(tmpList[0].transform.position.x - vSelfAttribute.transform.position.x);
        for (int i = 1; i < tmpList.Count; i++)
        {
            if (tmpList[i].Get_RoleType == ERoleType.ertBarracks)
                continue;
            if (tmpList[tmpIndex].Get_RoleType == ERoleType.ertBarracks)
            {
                tmpIndex = i;
                continue;
            }
            if (tmpDis > Mathf.Abs(tmpList[i].transform.position.x - vSelfAttribute.transform.position.x))
                tmpIndex = i;
        }
        return tmpList[tmpIndex];
    }

    /// <summary>
    /// 查找场景中碰撞物件
    /// </summary>
    /// <param name="vInitPos">初始坐标</param>
    /// <param name="vDirection">射线方向</param>
    /// <param name="vDistance">射线距离</param>
    /// <param name="vFightCamp">查找匹配阵营-没有阵营筛选就不填</param>
    /// <returns></returns>
    public static List<RoleAttribute> FindHitFightObjects(Vector3 vInitPos, Vector3 vDirection, float vDistance, EFightCamp vFightCamp = EFightCamp.efcNone, bool vIsDistance = true)
    {
        Ray tmpRay = new Ray(vInitPos, vDirection);

        LayerMask tmpMask = LayerMaskTool.GetLayerMask(LayerMaskEnum.UI);
        //Debug.DrawRay(vInitPos, vDirection, Color.red);
        RaycastHit[] tmpHitArray;
        if (vIsDistance)
        {
            tmpHitArray = Physics.RaycastAll(tmpRay, vDistance, tmpMask.value);
        }
        else
            tmpHitArray = Physics.RaycastAll(tmpRay, 1000000, tmpMask.value);
        if (tmpHitArray == null)
        {
            return null;
        }
        List<RoleAttribute> tmpResult = new List<RoleAttribute>();
        for (int i = 0; i < tmpHitArray.Length; i++)
        {
            RoleAttribute tmpAttribute = tmpHitArray[i].collider.GetComponent<RoleAttribute>();
            if (tmpAttribute == null)
                continue;
            if (vFightCamp != EFightCamp.efcNone)
            {
                if (tmpAttribute.Get_RoleCamp != vFightCamp)
                    continue;
            }
            tmpResult.Add(tmpAttribute);
        }

        return tmpResult;
    }

    /// <summary>
    /// 查找场景中碰撞物件
    /// </summary>
    /// <param name="vInitPos">初始坐标</param>
    /// <param name="vEndPos">结束坐标</param>
    /// <param name="vFightCamp">查找匹配阵营-没有阵营筛选就不填</param>
    /// <returns></returns>
    public static List<RoleAttribute> FindHitFightObjects(Vector3 vInitPos, Vector3 vEndPos, ERoleDirection director, EFightCamp vFightCamp = EFightCamp.efcNone)
    {
        Vector3 dir = new Vector3();
        if (director == ERoleDirection.erdLeft)
            dir.x = -1;
        else
            dir.x = 1;
        return FindHitFightObjects(vInitPos, dir, Mathf.Abs(vEndPos.x - vInitPos.x), vFightCamp);
    }

    /// <summary>
    /// 查找场景中碰撞物件
    /// </summary>
    /// <param name="vCenterPos">中心点坐标</param>
    /// <param name="vWidth">检测半径</param>
    /// <param name="vFightCamp">查找匹配阵营-没有阵营筛选就不填</param>
    /// <returns></returns>
    public static List<RoleAttribute> FindHitFightObjects(Vector3 vCenterPos, float vWidth, EFightCamp vFightCamp = EFightCamp.efcNone)
    {
        return FindHitFightObjects(new Vector3(vCenterPos.x - vWidth, vCenterPos.y, vCenterPos.z), new Vector3(1, 0, 0), vWidth * 2, vFightCamp);
    }

    /// <summary>
    /// 查找场景中碰撞物件
    /// </summary>
    /// <param name="vSelfAttribute">自身物件信息</param>
    /// <param name="vFightCamp">查找匹配阵营-没有阵营筛选就不填</param>
    /// <param name="vIsDistance">是否需要设置查找范围</param>
    /// <param name="vDistance">查找范围-像素</param>
    /// <returns></returns>
    public static List<RoleAttribute> FindHitFightObjects(RoleAttribute vSelfAttribute, EFightCamp vFightCamp = EFightCamp.efcNone, bool vIsDistance = true, int vDistance = 0)
    {
        if (vSelfAttribute == null)
            return null;
        if (SceneManager.Instance.Get_CurScene == null)
            return null;

        Vector3 tmpInitPos = vSelfAttribute.gameObject.transform.position;

        Vector3 tmpDirection = Vector3.zero;
        if (vSelfAttribute.Get_RoleCamp == EFightCamp.efcEnemy)
        {
            if (vSelfAttribute.Get_IsSubverted)
                tmpDirection = new Vector3(1, 0, 0);
            else
                tmpDirection = new Vector3(-1, 0, 0);
        }
        else
        {
            if (vSelfAttribute.Get_IsSubverted)
                tmpDirection = new Vector3(-1, 0, 0);
            else
                tmpDirection = new Vector3(1, 0, 0);
        }

        float tmpDistance = 0;
        if (vDistance == 0)
            tmpDistance = (float)vSelfAttribute.Get_FightAttribute.AttDistance / SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
        else
            tmpDistance = (float)vDistance / SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;

        return FindHitFightObjects(tmpInitPos, tmpDirection, tmpDistance, vFightCamp, vIsDistance);
    }

    /// <summary>
    /// 清除子物件
    /// </summary>
    /// <param name="t"></param>
    public static void ClearChild(Transform t)
    {
        for (int i = 0, imax = t.childCount; i < imax; ++i)
        {
            Transform child = t.GetChild(i);
            if (child)
                GameObject.Destroy(child.gameObject);
        }
    }

    public static long GetLfetTime(long vTotalTime)
    {
        // TimeSpan ts = DateTime.UtcNow.Subtract(DateTime.Parse("1970-1-1")).Duration();
        //long _nowTime = (long)ts.TotalSeconds;
        //Debug.LogError("ssssssssss   =   " + (vTotalTime - Main.mTime));
        return vTotalTime - Main.mTime;

    }

    public static bool IsEndTime(long vTotalTime)
    {
        //TimeSpan ts = DateTime.UtcNow.Subtract(DateTime.Parse("1970-1-1")).Duration();
        //long _nowTime = (long)ts.TotalSeconds;
        if (Main.mTime >= vTotalTime)
            return true;
        else
            return false;
    }

    public static string OneKeyReadMailRespToString(fogs.proto.msg.OneKeyReadMailResp response)
    {
        StringBuilder sbVar = new StringBuilder();
        string str = "result = " + response.result;
        str += " success mail id: ";
        foreach (ulong i in response.success_mail_idlist)
        {
            str += i + ",";
        }
        str += " failed mail id: ";
        foreach (ulong j in response.fail_mail_idlist)
        {
            str += j + ",";
        }

        return str;
    }

    /// <summary>
    /// 通过性别查找资源名字
    /// </summary>
    /// <param name="vGender">角色性别</param>
    /// <param name="vGender">是否特殊处理</param>
    /// <returns></returns>
    public static string GetHeroResourceNameByGender(EHeroGender vGender, bool vIsSpecial = false)
    {
        //if (vGender == EHeroGender.ehgFamale)
        //    return GlobalConst.HERO_RES_FAMALE;
        //else
        //    return GlobalConst.HERO_RES_MALE;

        uint tmpID = 0;
        if (!vIsSpecial)
        {
            if (vGender == EHeroGender.ehgFamale)
                tmpID = GlobalConst.HERO_COMID_FAMALE;
            else
                tmpID = GlobalConst.HERO_COMID_MALE;
        }
        else
        {
            if (vGender == EHeroGender.ehgFamale)
                tmpID = GlobalConst.HERO_SPEID_FAMALE;
            else
                tmpID = GlobalConst.HERO_SPEID_MALE;
        }
        return string.Format(GlobalConst.HERO_RES_STR, tmpID);
    }


    /// <summary>
    /// 屏蔽玩家
    /// </summary>
    public static void SetShieldingPlayers(string _accname, uint _areaid, string _blockName)
    {
        if (_accname == null || _areaid == null)
        {
            return;
        }
        SystemSettingModule.Instance.SendShieldingPlayersRequest(_accname, _areaid, _blockName);
    }
    /// <summary>
    /// 通过性别查找图标
    /// </summary>
    /// <param name="vGender">角色性别</param>
    /// <param name="vGender">是否特殊处理</param>
    /// <returns></returns>
    public static string GetHeroIconNameByGender(EHeroGender vGender, bool vIsSpecial = false)
    {
        //if (vGender == EHeroGender.ehgFamale)
        //    return GlobalConst.HERO_ICON_FAMALE;
        //else
        //    return GlobalConst.HERO_ICON_MALE;

        uint tmpID = 0;
        if (!vIsSpecial)
        {
            if (vGender == EHeroGender.ehgFamale)
                tmpID = GlobalConst.HERO_COMID_FAMALE;
            else
                tmpID = GlobalConst.HERO_COMID_MALE;
        }
        else
        {
            if (vGender == EHeroGender.ehgFamale)
                tmpID = GlobalConst.HERO_SPEID_FAMALE;
            else
                tmpID = GlobalConst.HERO_SPEID_MALE;
        }
        return string.Format(GlobalConst.HERO_ICON_STR, tmpID);
    }

    public static void ResetParticlePanelOrder(GameObject offestObj, GameObject root, SetParticleSortingLayer comp)
    {
        UIPanel offestPanel = offestObj.GetComponent<UIPanel>();
        OffestSortingOrder offestComp = offestObj.GetComponent<OffestSortingOrder>();
        UIPanel rootPanel = root.GetComponent<UIPanel>();
        if (offestPanel == null || offestComp == null || rootPanel == null)
        {
            return;
        }
        int offestOrder = rootPanel.sortingOrder + offestComp.offestOrder;
        comp.UpdateParticleSortingLayer(offestOrder);
        int childMinOrder = 0;
        int childMaxOrder = 0;
        ParticleSystem[] childParticles = comp.transform.GetComponentsInChildren<ParticleSystem>(true);

        if (childParticles != null && childParticles.Length > 0)
        {
            int leng = childParticles.Length;
            for (int j = 0; j < leng; j++)    //取得最小偏移值
            {
                ParticleSystem child = childParticles[j];
                if (j == 0)
                {
                    childMinOrder = child.renderer.sortingOrder;
                    childMaxOrder = child.renderer.sortingOrder;
                }
                if (child.renderer.sortingOrder <= childMinOrder)
                {
                    childMinOrder = child.renderer.sortingOrder;
                }
                if (child.renderer.sortingOrder >= childMaxOrder)
                {
                    childMaxOrder = child.renderer.sortingOrder;
                }
            }
        }
        int offest = childMaxOrder - childMinOrder;
        offestPanel.sortingOrder = offestOrder + 50;
        offestPanel.Refresh();
    }

    public static string GetHeroIconNameByID(uint id, bool isSquare)
    {
        if (id == GlobalConst.SYSTEMICONID)//系统头像
        {
            return GlobalConst.SYSTEMICON;
        }
        if (id != 0)
        {
            PlayerPortraitData data = ConfigManager.Instance.mPlayerPortraitConfig.GetPlayerPortraitByID(id);
            if (isSquare)
                return data.icon;
            else
                return data.icon2;
        }

        else
        {
            if (isSquare)
                return GetHeroIconNameByGender((EHeroGender)PlayerData.Instance._Gender);
            else
                return GetHeroIconNameByGender((EHeroGender)PlayerData.Instance._Gender, true);

        }
    }

    public static string GetUnionIconNameByID(uint id)
    {
        if (id != 0)
        {
            UnionIconData data = ConfigManager.Instance.mUnionConfig.GetUnionIconByID(id);
            return data.mIcon;
        }
        else
        {

            return GlobalConst.SYSTEMICON;
        }
    }

    /// <summary>
    /// 设置玩家头像
    /// </summary>
    /// <param name="head">头像</param>
    /// <param name="frame">头像框</param>
    /// <param name="id">头像id(GlobalConst.SYSTEMICONID为系统头像)</param>
    /// <param name="isSquare">头像是否为方框,普通头像默认设置为True</param>
    public static void SetHeadAndFrameSprite(UISprite head, UISprite frame, uint id, uint frameID, bool isSquare)
    {
        if (isSquare)
        {
            SetSpriteName(head, GetHeroIconNameByID(id, true));
        }
        else
        {
            SetSpriteName(head, GetHeroIconNameByID(id, false));
        }
        if (id == GlobalConst.SYSTEMICONID)//系统头像
        {
            string Frames_A = string.Format(GlobalConst.SpriteName.Frame_Name_A, GlobalConst.SYSTEMFRAME);
            SetSpriteName(frame, Frames_A);
        }
        if (frameID == 0)
        { frameID = GlobalConst.SYSTEMFRAME; }
        string Frame_A = string.Format(GlobalConst.SpriteName.Frame_Name_A, frameID);
        //Debug.LogError("ssssssss=     " + Frame_A);
        SetSpriteName(frame, Frame_A);
    }

    public static void SetUnionFrameSprite(UISprite head, UISprite frame, uint id, uint frameID)
    {
        SetSpriteName(head, GetUnionIconNameByID(id));
        if (frameID == 0)
        { frameID = GlobalConst.SYSTEMFRAME; }
        string Frame_A = string.Format(GlobalConst.SpriteName.Frame_Name_A, frameID);
        //Debug.LogError("ssssssss=     " + Frame_A);
        SetSpriteName(frame, Frame_A);
    }

    public static void SetMoneyIcon(UISprite sprite, ECurrencyType type)
    {
        switch (type)
        {
            case ECurrencyType.Honor:
                {
                    SetSpriteName(sprite, GlobalConst.SpriteName.Honor);
                    break;
                }
            case ECurrencyType.Gold:
                {
                    SetSpriteName(sprite, GlobalConst.SpriteName.Gold);
                    break;
                }
            case ECurrencyType.Diamond:
                {
                    SetSpriteName(sprite, GlobalConst.SpriteName.Diamond);
                    break;
                }
            case ECurrencyType.Medal:
                {
                    SetSpriteName(sprite, GlobalConst.SpriteName.Medal);
                    break;
                }
            case ECurrencyType.UnionToken:
                {
                    SetSpriteName(sprite, GlobalConst.SpriteName.UnionToken);
                    break;
                }
            case ECurrencyType.RecycleCoin:
                {
                    SetSpriteName(sprite, GlobalConst.SpriteName.RecycleCoin);
                    break;
                }
        }
    }

    public static string GetMoneyNameByType(ECurrencyType type)
    {
        switch (type)
        {
            case ECurrencyType.Honor:
                {
                    return ConstString.NAME_HONOR;
                }
            case ECurrencyType.Gold:
                {
                    return ConstString.NAME_GOLD;
                }
            case ECurrencyType.Diamond:
                {
                    return ConstString.NAME_DIAMOND;
                }
            case ECurrencyType.Medal:
                {
                    return ConstString.NAME_MEDAL;
                }
            case ECurrencyType.UnionToken:
                {
                    return ConstString.NAME_UNIONTOKEN;
                }
            case ECurrencyType.RecycleCoin:
                {
                    return ConstString.NAME_RECYCLECOIN;
                }
            default:
                return "";
        }
    }

    /// <summary>
    /// 足够则为true
    /// </summary>
    /// <param name="type"></param>
    /// <param name="needNum"></param>
    /// <param name="needTip"></param>
    /// <returns></returns>
    public static bool CheckMoneyEnough(ECurrencyType type, int needNum, bool needTip = false)
    {
        bool result = false;
        switch (type)
        {
            case ECurrencyType.Honor:
                {
                    result = PlayerData.Instance._Honor >= needNum;
                    if (!result && needTip)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_HONOR);
                    break;
                }
            case ECurrencyType.Gold:
                {
                    result = PlayerData.Instance._Gold >= needNum;
                    if (!result && needTip)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_GOLD);
                    break;
                }
            case ECurrencyType.Diamond:
                {
                    result = PlayerData.Instance._Diamonds >= needNum;
                    if (!result && needTip)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_DIAMOND);
                    break;
                }
            case ECurrencyType.RecycleCoin:
                {
                    result = PlayerData.Instance.RecycleCoin >= needNum;
                    if (!result && needTip)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_RECYCLECOIN);
                    break;
                }
            case ECurrencyType.Medal:
                {
                    result = PlayerData.Instance._Medal >= needNum;
                    if (!result && needTip)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_MEDAL);
                    break;
                }
            case ECurrencyType.UnionToken:
                {
                    result = PlayerData.Instance.UnionToken >= needNum;
                    if (!result && needTip)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_TOKEN);
                    break;
                }
        }
        return result;
    }

    public static void SetSpriteAtlasByName(UISprite sprite, string atlasname)
    {
        if (sprite == null)
        {
            Debug.LogError(" sprite is null please check ");
            return;
        }
        if (string.IsNullOrEmpty(atlasname))
        {
            Debug.LogError(" sprite NAME is null please check at " + sprite.gameObject.name);
            return;
        }
        List<UIAtlas> lAtlasList = ResourceLoadManager.Instance.GetLoadUIAtlas();
        for (int i = 0; i < lAtlasList.Count; i++)
        {
            UIAtlas atlas = lAtlasList[i];
            if (atlas == null)
                continue;
            if (atlas.name == atlasname)
            {
                sprite.atlas = atlas;
                return;
            }
        }
    }

    public static bool SetSpriteName(UISprite sprite, string name)
    {
        bool result = false;
        if (sprite == null)
        {
            Debug.LogError(" sprite is null please check ");
            return result;
        }
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError(" sprite NAME is null please check at " + sprite.gameObject.name);
            return result;
        }
        if (sprite.atlas != null)
        {
            UISpriteData spriteData = sprite.atlas.GetSprite(name); //如果该Sprite已经正确设置或者就在当前图集中,则不在需要遍历图集 
            if (spriteData != null)
            {
                sprite.spriteName = name;
                return true;
            }
        }
        UIAtlas atlas = ResourceLoadManager.Instance.GetAtlasBySpriteName(name);
        if (atlas != null)
        {
            sprite.atlas = atlas;
            sprite.spriteName = name;
            result = true;
        }
        else
        {
            sprite.atlas = null;
            sprite.spriteName = string.Empty;
            result = false;
        }
        return result;
    }

    public static string QualitySpriteName(int quality)
    {
        switch (quality)
        {
            case 1: return GlobalConst.SpriteName.Quality_1;
            case 2: return GlobalConst.SpriteName.Quality_2;
            case 3: return GlobalConst.SpriteName.Quality_3;
            case 4: return GlobalConst.SpriteName.Quality_4;
            case 5: return GlobalConst.SpriteName.Quality_5;
            case 6: return GlobalConst.SpriteName.Quality_6;
        }
        return string.Empty;
    }
    public static string QualityBGSpriteName(int quality)
    {
        switch (quality)
        {
            case 1: return GlobalConst.SpriteName.QualityBG_1;
            case 2: return GlobalConst.SpriteName.QualityBG_2;
            case 3: return GlobalConst.SpriteName.QualityBG_3;
            case 4: return GlobalConst.SpriteName.QualityBG_4;
            case 5: return GlobalConst.SpriteName.QualityBG_5;
            case 6: return GlobalConst.SpriteName.QualityBG_6;
        }
        return string.Empty;
    }

    public static void SetQualitySprite(UISprite qualitySprite, int qualityNum, UISprite qualityBg = null)
    {
        if (qualityBg != null)
            SetSpriteName(qualityBg, QualityBGSpriteName(qualityNum));

        if (qualitySprite != null)
            SetSpriteName(qualitySprite, QualitySpriteName(qualityNum));
    }

    public static void SetQualitySprite(UISprite qualitySprite, ItemQualityEnum qualityEnum, UISprite qualityBg = null)
    {
        SetQualitySprite(qualitySprite, (int)qualityEnum, qualityBg);
    }

    public static IDType GetTypeOfID(string id)
    {
        IDType result = IDType.None;
        switch (id)
        {
            case "50000000":
                return IDType.SP;
            case "51000000":
                return IDType.Gold;
            case "52000000":
                return IDType.Diamond;
            case "53000000":
                return IDType.Medal;
            case "54000000":
                return IDType.Exp;
            case "55000000":
                return IDType.SoldierExp;
            case "56000000":
                return IDType.Honor;
            case "57000000":
                return IDType.UnionToken;
            case "58000000":
                return IDType.Free;
            case "59000000":
                return IDType.RecycleCoin;
        }
        string substr = id.Substring(0, 1);
        switch (substr)
        {
            case "1":
                result = IDType.Soldier;
                break;
            case "2":
                result = IDType.EQ;
                break;
            case "3":
                result = IDType.Prop;
                break;
            case "6":
                result = IDType.LifeSoul;
                break;
            case "4":
                result = IDType.Pet;
                break;
        }
        return result;
    }
    public static void SetChipMark(UISprite mark, ItemTypeEnum type, Vector3 posEquipChip, Vector3 posSoldierChip, int width = 32, int heigh = 28)
    {
        mark.width = width;
        mark.height = heigh;
        switch (type)
        {
            case ItemTypeEnum.SoldierChip:
                {
                    mark.gameObject.SetActive(true);
                    SetSpriteName(mark, GlobalConst.SpriteName.MarkSoldierChip);
                    mark.MakePixelPerfect();
                    mark.transform.localPosition = posSoldierChip;
                    break;
                }
            case ItemTypeEnum.EquipChip:
                {
                    mark.gameObject.SetActive(true);
                    SetSpriteName(mark, GlobalConst.SpriteName.MarkEquipChip);
                    mark.MakePixelPerfect();
                    mark.transform.localPosition = posEquipChip;
                    break;
                }
            default:
                mark.gameObject.SetActive(false);
                break;
        }
    }

    public static void SetLifeSoulMark(UISprite mark, int godEquip, Vector3 pos, int width = 35, int heigh = 35)
    {
        mark.gameObject.SetActive(true);
        mark.width = width;
        mark.height = heigh;
        mark.transform.localPosition = pos;
        SetLifeSpiritTypeMark(mark, godEquip);
    }

    /// <summary>
    /// 将一个物品信息绘制在一般大小物品栏UI上，各部分独立指定
    /// </summary>
    /// <param name="item">物品信息</param>
    /// <param name="bg">背景图片</param>
    /// <param name="icon">物品图片</param>
    /// <param name="frame">品质边框</param>
    /// <param name="num">数量角标</param>
    /// <param name="tag">特殊类型角标</param>
    public static void ShowItemByInfo(CommonItemData item, UISprite bg, UISprite icon, UISprite frame, UILabel num = null, UISprite mark = null)
    {
        CommonFunction.SetQualitySprite(frame, item.Quality, bg);
        CommonFunction.SetSpriteName(icon, item.Icon);
        if (num != null)
        {
            if (item.Num <= 1)
            {
                num.text = string.Empty;
            }
            else
            {
                if (item.Num / 10000 > 0)
                {
                    num.text = "x" + string.Format(ConstString.TASK_TENTHOUSAND, (item.Num / 10000).ToString());
                }
                else
                {
                    num.text = "x" + item.Num.ToString();
                }
            }
        }
        if (mark != null)
        {
            if (item.Type == IDType.Prop)
            {
                CommonFunction.SetChipMark(mark, item.SubType, new Vector3(-25, 25, 0), new Vector3(-25, 27, 0));
            }
            else if (item.Type == IDType.LifeSoul)
            {
                CommonFunction.SetLifeSoulMark(mark, item.LifeSoulGodEquip, new Vector3(26, 26, 0));
            }
            else
            {
                mark.gameObject.SetActive(false);
            }
        }

    }

    public static bool IsInTimeInterval(DateTime time, DateTime startTime, DateTime endTime)
    {
        if (startTime > endTime)
        {
            DateTime tempTime = startTime;
            startTime = endTime;
            endTime = tempTime;
        }

        DateTime newTime = new DateTime();
        newTime = newTime.AddHours(time.Hour);
        newTime = newTime.AddMinutes(time.Minute);

        DateTime newStartTime = new DateTime();
        newStartTime = newStartTime.AddHours(startTime.Hour);
        newStartTime = newStartTime.AddMinutes(startTime.Minute);
        DateTime newEndTime = new DateTime();
        if (startTime.Hour > endTime.Hour)
        {
            newEndTime = newEndTime.AddDays(1);
        }
        newEndTime = newEndTime.AddHours(endTime.Hour);
        newEndTime = newEndTime.AddMinutes(endTime.Minute);
        if (newTime > newStartTime && newTime < newEndTime)
        {
            return true;
        }
        return false;
    }

    public static bool IsInTimeInterval(string timeInterval)
    {
        if (string.IsNullOrEmpty(timeInterval) || timeInterval == GlobalConst.COMMON_UNCONDITIONAL)
            return false;
        string[] strs = timeInterval.Split(';');
        if (strs.Length < 2) return false;
        string[] startTimeStr = strs[0].Split(':');
        if (startTimeStr.Length < 2) return false;
        DateTime startTime = new DateTime();
        startTime.AddHours(int.Parse(startTimeStr[0]));
        startTime.AddMinutes(int.Parse(startTimeStr[1]));
        string[] endTimeStr = strs[1].Split(':');
        DateTime endTime = new DateTime();
        endTime.AddHours(int.Parse(endTimeStr[0]));
        endTime.AddMinutes(int.Parse(endTimeStr[1]));
        return IsInTimeInterval(DateTime.Now, startTime, endTime);
    }

    public static List<uint> GetFullBagsItem(List<Soldier> material, bool isStar)
    {
        List<uint> fullList = new List<uint>();
        Dictionary<uint, int> returnSkillNum = new Dictionary<uint, int>();
        ReturnMaterialConfig att = ConfigManager.Instance.mReturnMaterialData;

        foreach (Soldier sd in material)
        {
            foreach (Skill sk in sd._skillsDepot._skillsList)
            {
                ReturnMaterialInfo info = att.FindByLevel(sk.Level);
                if (info == null) continue;
                List<KeyValuePair<uint, int>> ReturnList = isStar ? info.ReturnMaterialStar : info.ReturnMaterial;
                foreach (KeyValuePair<uint, int> returnSkill in ReturnList)
                {
                    if (returnSkillNum.ContainsKey(returnSkill.Key))
                        returnSkillNum[returnSkill.Key] += returnSkill.Value;
                    else
                        returnSkillNum.Add(returnSkill.Key, PlayerData.Instance.GetItemCountByID(returnSkill.Key) + returnSkill.Value);
                    //if (PlayerData.Instance.GetItemCountByID(returnSkill.Key) + returnSkill.Value > GlobalConst.MAX_Item_Spill)
                    //    fullList.Add(returnSkill.Key);
                }
            }
        }
        foreach (var tmp in returnSkillNum)
        {
            if (tmp.Value > GlobalConst.MAX_Item_Spill)
                fullList.Add(tmp.Key);
        }
        return fullList;
    }
    public static List<KeyValuePair<uint, int>> GetSkillReturnBagsItem(List<UInt64> _materials)
    {
        List<Soldier> _materialList = new List<Soldier>(_materials.Count + 1);
        foreach (UInt64 a in _materials)
        {
            Soldier sd = PlayerData.Instance._SoldierDepot.FindByUid(a);
            if (sd == null) continue;
            _materialList.Add(sd);
        }

        return GetSkillReturnBagsItem(_materialList);

    }
    public static List<KeyValuePair<uint, int>> GetSkillReturnBagsItem(List<Soldier> material)
    {
        List<KeyValuePair<uint, int>> fullList = new List<KeyValuePair<uint, int>>();
        ReturnMaterialConfig att = ConfigManager.Instance.mReturnMaterialData;
        Dictionary<uint, int> returnSkillNum = new Dictionary<uint, int>();
        foreach (Soldier sd in material)
        {
            foreach (Skill sk in sd._skillsDepot._skillsList)
            {
                ReturnMaterialInfo info = att.FindByLevel(sk.Level);
                if (info == null) continue;
                foreach (KeyValuePair<uint, int> returnSkill in info.ReturnMaterialStar)
                {
                    if (returnSkillNum.ContainsKey(returnSkill.Key))
                        returnSkillNum[returnSkill.Key] += returnSkill.Value;
                    else
                        returnSkillNum.Add(returnSkill.Key, PlayerData.Instance.GetItemCountByID(returnSkill.Key) + returnSkill.Value);
                }
            }
        }
        foreach (var tmp in returnSkillNum)
        {
            if (tmp.Value > GlobalConst.MAX_Item_Spill)
                fullList.Add(new KeyValuePair<uint, int>(tmp.Key, tmp.Value));
        }
        return fullList;
    }
    public static List<uint> GetFullBagsItem(List<UInt64> _materials, bool isStar = false)
    {
        List<Soldier> _materialList = new List<Soldier>(_materials.Count + 1);
        foreach (UInt64 a in _materials)
        {
            Soldier sd = PlayerData.Instance._SoldierDepot.FindByUid(a);
            if (sd == null) continue;
            _materialList.Add(sd);
        }

        return GetFullBagsItem(_materialList, isStar);
    }

    public static string EquipBackSpriteName(int pos)
    {
        switch (pos)
        {
            case 0: return "Sketch_Icon_wq";
            case 1: return "Sketch_Icon_jz";
            case 2: return "Sketch_Icon_xl";
            case 3: return "Sketch_Icon_zb";
            case 4: return "Sketch_Icon_zq";
        }
        return "";
    }
    /// <summary>
    /// 属性面板设置函数
    /// </summary>
    /// <param name="gp">属性面板父类</param>
    /// <param name="info">属性值</param>
    /// <param name="after">如果有对比则穿入对比属性</param>
    public static void SetAttributeGroup(GameObject gp, ShowInfoBase info, ShowInfoBase after = null, bool isAddLabel = true)
    {
        UISprite AttBeforSptrite;
        UISprite BldBeforSptrite;
        UILabel Lbl_Label_attBef;
        UILabel Lbl_Label_bldBef;

        AttBeforSptrite = gp.transform.FindChild("BGSpriteAtt").gameObject.GetComponent<UISprite>();
        BldBeforSptrite = gp.transform.FindChild("BGSpriteBlood").gameObject.GetComponent<UISprite>();

        Lbl_Label_attBef = gp.transform.FindChild("BGSpriteAtt/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_bldBef = gp.transform.FindChild("BGSpriteBlood/Label").gameObject.GetComponent<UILabel>();

        List<KeyValuePair<string, float>> resultBef = GetAttributeSprite(info);
        List<KeyValuePair<string, float>> resultAft = null;

        if (after != null)
        {
            resultAft = GetAttributeSprite(after);
        }

        if (Lbl_Label_attBef != null && AttBeforSptrite != null)
        {
            if (resultBef.Count > 0)
            {
                Lbl_Label_attBef.gameObject.SetActive(true);
                AttBeforSptrite.gameObject.SetActive(true);

                KeyValuePair<string, string> resultBefStr;

                string[] tt = Regex.Split(resultBef[0].Key, ",");
                if (tt.Length < 2)
                {
                    resultBefStr = new KeyValuePair<string, string>(tt[0], "");
                }
                else
                {
                    if (isAddLabel)
                        resultBefStr = new KeyValuePair<string, string>(tt[0], tt[1]);
                    else
                        resultBefStr = new KeyValuePair<string, string>(tt[0], "");
                }

                if (resultAft != null && resultAft.Count > 0)
                {
                    if ((int)resultAft[0].Value - (int)resultBef[0].Value > 0)
                        Lbl_Label_attBef.text = string.Format("[c4ad87]{0}{1}[-][3abd22] + {2}[-]", resultBefStr.Value, (int)resultBef[0].Value, (int)resultAft[0].Value - (int)resultBef[0].Value);
                    else
                        Lbl_Label_attBef.text = string.Format("[c4ad87]{0}{1}[-]", resultBefStr.Value, (int)resultBef[0].Value);
                }
                else
                {
                    Lbl_Label_attBef.text = string.Format("[c4ad87]{0}{1}[-]", resultBefStr.Value, (int)resultBef[0].Value);
                }
                SetSpriteName(AttBeforSptrite, resultBefStr.Key);
            }
            else
            {
                Lbl_Label_attBef.gameObject.SetActive(false);
                AttBeforSptrite.gameObject.SetActive(false);
            }
        }
        if (Lbl_Label_bldBef != null && BldBeforSptrite != null)
        {
            if (resultBef.Count > 1)
            {
                Lbl_Label_bldBef.gameObject.SetActive(true);
                BldBeforSptrite.gameObject.SetActive(true);

                KeyValuePair<string, string> resultBefStr;

                string[] tt = Regex.Split(resultBef[1].Key, ",");
                if (tt.Length < 2)
                {
                    resultBefStr = new KeyValuePair<string, string>(tt[0], "");
                }
                else
                {
                    if (isAddLabel)
                        resultBefStr = new KeyValuePair<string, string>(tt[0], tt[1]);
                    else
                        resultBefStr = new KeyValuePair<string, string>(tt[0], "");
                }
                if (resultAft != null && resultAft.Count > 1)
                {
                    if ((int)resultAft[1].Value - (int)resultBef[1].Value > 0)
                        Lbl_Label_bldBef.text = string.Format("[c4ad87]{0}{1}[-][3abd22] + {2}[-]", resultBefStr.Value, (int)resultBef[1].Value, (int)resultAft[1].Value - (int)resultBef[1].Value);
                    else
                        Lbl_Label_bldBef.text = string.Format("[c4ad87]{0}{1}[-]", resultBefStr.Value, (int)resultBef[1].Value);
                }
                else
                {
                    Lbl_Label_bldBef.text = string.Format("[c4ad87]{0}{1}[-]", resultBefStr.Value, (int)resultBef[1].Value);
                }
                SetSpriteName(BldBeforSptrite, resultBefStr.Key);
            }
            else
            {
                Lbl_Label_bldBef.gameObject.SetActive(false);
                BldBeforSptrite.gameObject.SetActive(false);
            }
        }
    }

    public static List<KeyValuePair<string, float>> GetAttributeSprite(ShowInfoBase info)
    {
        List<KeyValuePair<string, float>> result = new List<KeyValuePair<string, float>>();

        AttributeSpriteTool(result, "YX_icon_gjl", ConstString.phy_atk, info.Attack);
        AttributeSpriteTool(result, "YX_icon_smz", ConstString.hp_max, info.HP);

        if (info is ShowInfoHero)
        {
            ShowInfoHero temp = (ShowInfoHero)info;
            if (temp != null)
            {
                AttributeSpriteTool(result, "YX_icon_tyl", ConstString.leader, temp.Leadership);
            }
        }
        if (info is ShowInfoSoldiers)
        {
            ShowInfoSoldiers tempSoldier = (ShowInfoSoldiers)info;
            if (tempSoldier != null)
            {
                AttributeSpriteTool(result, "YX_icon_tyl", ConstString.leader, tempSoldier.Leadership);
            }
        }
        AttributeSpriteTool(result, "YX_icon_smzhf", ConstString.hp_revert, info.HPRecovery);
        AttributeSpriteTool(result, "YX_icon_mf", ConstString.mp_max, info.MP);
        AttributeSpriteTool(result, "YX_icon_mfhf", ConstString.mp_revert, info.MPRecovery);
        AttributeSpriteTool(result, "YX_icon_zhnl", ConstString.energy_max, info.Energy);
        AttributeSpriteTool(result, "YX_icon_zhnlhf", ConstString.energy_revert, info.EnergyRecovery);
        AttributeSpriteTool(result, "YX_icon_bj", ConstString.crt_rate, info.Crit);
        AttributeSpriteTool(result, "YX_icon_rx", ConstString.tnc_rate, info.Tenacity);
        AttributeSpriteTool(result, "YX_icon_gjsd", ConstString.atk_interval, info.AttRate);
        AttributeSpriteTool(result, "YX_icon_jl", ConstString.atk_space, info.AttDistance);
        AttributeSpriteTool(result, "YX_icon_mz", ConstString.acc_rate, info.Accuracy);
        AttributeSpriteTool(result, GlobalConst.SpriteName.Attribute_AccRate, ConstString.acc_rate, info.Accuracy);
        AttributeSpriteTool(result, GlobalConst.SpriteName.Attribute_DdgRate, ConstString.ddg_rate, info.Dodge);
        AttributeSpriteTool(result, GlobalConst.SpriteName.Attribute_Speed, ConstString.speed, info.MoveSpeed);

        return result;
    }

    public static void AttributeSpriteTool(List<KeyValuePair<string, float>> result, string icon, string descript, float num)
    {
        if (num != 0)
        {
            //组合图标名和属性中文名的字符串
            string resultString = string.Format("{0},{1}:", icon, descript);
            result.Add(new KeyValuePair<string, float>(resultString, num));
        }
    }

    public static string GetTargetUINameByID(int id, bool isSprite)
    {
        //TODO:UI缺竞技场 聊天 连续登陆  活动公告 活跃宝箱 排行榜 公会 奴隶 扫荡 批量扫荡 一键强化 战斗两倍加速 奴隶位置二 奴隶位置三 奴隶位置四
        OpenFunctionType type = (OpenFunctionType)id;
        string uiName = "";
        switch (type)
        {
            case OpenFunctionType.Hero:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncHero : ViewType.DIR_VIEWNAME_HEROATT;
                break;
            case OpenFunctionType.Soldier:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncSoldier : ViewType.DIR_VIEWNAME_SOLDIERATT;
                break;
            case OpenFunctionType.PVE:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncPVE : ViewType.DIR_VIEWNAME_PVPVIEW;
                break;
            case OpenFunctionType.Activity:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncActivity : ViewType.DIR_VIEWNAME_ACTIVITIES;
                break;
            case OpenFunctionType.Endless:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncEndless : ViewType.DIR_VIEWNAME_ENDLESS;
                break;
            case OpenFunctionType.Expedition:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncExpedition : ViewType.DIR_VIEWNAME_EXPEDITION;
                break;
            case OpenFunctionType.Arena:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncArena : "";
                break;
            case OpenFunctionType.Package:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncPackage : ViewType.DIR_VIEWNAME_BACKPACK;
                break;
            case OpenFunctionType.Store:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncStore : ViewType.DIR_VIEWNAME_STORE;
                break;
            case OpenFunctionType.SystemSetting:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncSystemSetting : ViewType.DIR_VIEWNAME_SYSTEMSETTINGVIEW;
                break;
            case OpenFunctionType.Chat:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncChat : "";
                break;
            case OpenFunctionType.Mail:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncMail : ViewType.DIR_VIEWNAME_MAILVIEW;
                break;
            case OpenFunctionType.LevelUp:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncLevelUp : ViewType.DIR_VIEWNAME_LEVELUP;
                break;
            case OpenFunctionType.ExchangeGold:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncExchangeGold : ViewType.DIR_VIEWNAME_EXCHANGEGOLD;
                break;
            case OpenFunctionType.VIP:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncVIP : "";
                break;
            case OpenFunctionType.Recruit:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncRecruit : ViewType.DIR_VIEWNAME_RECRUITVIEW;
                break;
            case OpenFunctionType.ContinuousLogin:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncContinuousLogin : "";
                break;
            case OpenFunctionType.ActivityAnnouncement:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncActivityAnnouncement : "";
                break;
            case OpenFunctionType.Task:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncTask : ViewType.DIR_VIEWNAME_TASKVIEW;
                break;
            case OpenFunctionType.Liveness:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncLiveness : "";
                break;
            case OpenFunctionType.Rank:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncRank : "";
                break;
            case OpenFunctionType.Sociaty:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncSociaty : "";
                break;
            case OpenFunctionType.Slave:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncSlave : "";
                break;
            case OpenFunctionType.Clear:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncClear : "";
                break;
            case OpenFunctionType.BatchClear:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncBatchClear : "";
                break;
            case OpenFunctionType.OneKeyEnchance:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncOneKeyEnchance : "";
                break;
            case OpenFunctionType.DoubleFightSpeed:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncDoubleFightSpeed : "";
                break;
            case OpenFunctionType.SlaveSlot2:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncSlaveSlot2 : "";
                break;
            case OpenFunctionType.SlaveSlot3:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncSlaveSlot3 : "";
                break;
            case OpenFunctionType.SlaveSlot4:
                uiName = isSprite ? GlobalConst.SpriteName.OpenFuncSlaveSlot4 : "";
                break;
        }
        return uiName;
    }

    /// <summary>
    /// 随机触发技能[武将 怪物]
    /// </summary>
    /// <param name="role">技能发起者</param>
    /// <param name="vSkill">技能类[主动 被动]</param>
    /// <returns></returns>
    public static bool RandomSkill(RoleAttribute role, SkillsDepot vSkill)
    {
        if ((role == null) || (vSkill == null))
            return false;
        return vSkill.RandomSkill(role);
    }

    /// <summary>
    /// 激活光环技能[武将 怪物]
    /// </summary>
    /// <param name="role">技能发起者</param>
    /// <param name="vSkill">技能类[主动 被动]</param>
    public static void ActivateHalo(RoleAttribute role, SkillsDepot vSkill)
    {
        if ((role == null) || (vSkill == null))
            return;
        vSkill.ActivateHalo(role);
    }

    /// <summary>
    /// 获取城堡文件名
    /// </summary>
    /// <returns></returns>
    public static string GetCastleFileName()
    {
        if (PlayerData.Instance.mCastleInfo == null)
            return string.Empty;
        CastleAttributeInfo tmpCastleInfo = ConfigManager.Instance.mCastleConfig.FindByID(PlayerData.Instance.mCastleInfo.mID);
        if (tmpCastleInfo == null)
            return string.Empty;
        return string.Format("{0}.assetbundle", tmpCastleInfo.fight_source);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="limit">超过多少时，用转换</param>
    /// <returns></returns>
    public static string GetTenThousandUnit(int value, int limit = 1000000)
    {
        if (value >= limit)
            return string.Format(ConstString.TASK_TENTHOUSAND, value / 10000);
        return value.ToString();

    }

    public static List<CommonItemData> GetCommonItemDataList(fogs.proto.msg.DropList vDropListInfo)
    {
        if (vDropListInfo != null)
        {
            List<fogs.proto.msg.ItemInfo> tmpItemList = new List<fogs.proto.msg.ItemInfo>();
            tmpItemList.AddRange(vDropListInfo.item_list);
            tmpItemList.AddRange(vDropListInfo.special_list);
            return GetCommonItemDataList(tmpItemList, vDropListInfo.equip_list, vDropListInfo.soldier_list);
        }
        else
        {
            return null;
        }
    }

    public static List<CommonItemData> GetCommonItemDataList(List<fogs.proto.msg.ItemInfo> itemInfos, List<Equip> equips, List<fogs.proto.msg.Soldier> soldiers)
    {
        List<CommonItemData> list = new List<CommonItemData>();

        if (itemInfos != null)
        {
            for (int i = 0; i < itemInfos.Count; i++)
            {
                CommonItemData item = new CommonItemData(itemInfos[i].id, itemInfos[i].change_num, true);
                list.Add(item);
            }
        }

        if (soldiers != null)
        {
            for (int i = 0; i < soldiers.Count; i++)
            {
                CommonItemData item = new CommonItemData(ConfigManager.Instance.mSoldierData.FindById(soldiers[i].id));
                item.Num = 1;
                list.Add(item);
            }
        }
        if (equips != null)
        {
            for (int i = 0; i < equips.Count; i++)
            {
                CommonItemData item = new CommonItemData(ConfigManager.Instance.mEquipData.FindById(equips[i].id));
                item.Num = 1;
                list.Add(item);
            }
        }

        return list;
    }

    public static List<CommonItemData> GetCommonItemDataList(List<fogs.proto.msg.ItemInfo> itemInfos, List<Equip> equips, List<fogs.proto.msg.Soldier> soldiers, List<fogs.proto.msg.LifeSoul> lifeSouls,List<fogs.proto.msg.Pet> petList)
    {
        List<CommonItemData> list = new List<CommonItemData>();

        if (itemInfos != null)
        {
            for (int i = 0; i < itemInfos.Count; i++)
            {
                CommonItemData item = new CommonItemData(itemInfos[i].id, itemInfos[i].change_num, true);
                list.Add(item);
            }
        }

        if (soldiers != null)
        {
            for (int i = 0; i < soldiers.Count; i++)
            {
                CommonItemData item = new CommonItemData(ConfigManager.Instance.mSoldierData.FindById(soldiers[i].id));
                item.Num = 1;
                list.Add(item);
            }
        }
        if (equips != null)
        {
            for (int i = 0; i < equips.Count; i++)
            {
                CommonItemData item = new CommonItemData(ConfigManager.Instance.mEquipData.FindById(equips[i].id));
                item.Num = 1;
                list.Add(item);
            }
        }
        if (lifeSouls != null)
        {
            for (int i = 0; i < lifeSouls.Count; i++)
            {
                LifeSoulData ls = new LifeSoulData(lifeSouls[i]);
                CommonItemData item = new CommonItemData(ls);
                item.Num = 1;
                list.Add(item);
            }
        }
        if (petList != null) 
        {
            for (int i = 0; i < petList.Count; i++)
            {
                PetData pet = new PetData();
                pet.Init(petList[i]);
                CommonItemData item = new CommonItemData(pet.PetInfo);
                item.Num = 1;
                list.Add(item);
            }
        }

        return list;
    }

    public static List<CommonItemData> GetCommonItemDataList(uint dropid)
    {
        List<CommonItemData> list = new List<CommonItemData>();
        List<DroppackInfo> pack = ConfigManager.Instance.mDroppackData.GetDropPackByID(dropid);
        if (pack != null)
            list.AddRange(GetCommonItemDataList(pack));
        return list;
    }

    private static List<CommonItemData> GetCommonItemDataList(List<DroppackInfo> pack)
    {
        List<CommonItemData> list = new List<CommonItemData>();
        for (int i = 0; i < pack.Count; i++)
        {
            //Debug.LogWarning(string.Format("[{0}, {1}, {2}]", i, pack[i].DropBagID, pack[i].ItemID));
            DroppackInfo info = pack[i];
            CommonItemData item = null;
            switch (info.ItemType)
            {
                case DropItemType.Soldier:
                    {
                        item = new CommonItemData(ConfigManager.Instance.mSoldierData.FindById(info.ItemID));
                        item.Num = info.ItemLowerLimit;
                        break;
                    }
                case DropItemType.Equip:
                    {
                        item = new CommonItemData(ConfigManager.Instance.mEquipData.FindById(info.ItemID));
                        item.Num = info.ItemLowerLimit;
                        break;
                    }
                case DropItemType.Item:
                    {
                        item = new CommonItemData(ConfigManager.Instance.mItemData.GetItemInfoByID(info.ItemID));
                        item.Num = info.ItemLowerLimit;
                        break;
                    }
                case DropItemType.Number:
                    {
                        item = new CommonItemData(info.ItemID, info.ItemLowerLimit, true);
                        break;
                    }
                case DropItemType.DropPack:
                    {
                        List<DroppackInfo> pack1 = ConfigManager.Instance.mDroppackData.GetSonPackByID(info.ItemID);
                        list.AddRange(GetCommonItemDataList(pack1));
                        break;
                    }
                default:
                    {
                        item = null;
                        break;
                    }
            }
            if (item != null)
            {
                list.Add(item);
            }
        }
        return list;
    }

    public static List<CommonItemData> GetCommonItemDataList(List<Prop_value> props)
    {
        List<CommonItemData> list = new List<CommonItemData>();
        for (int i = 0; i < props.Count; i++)
        {
            Prop_value info = props[i];
            CommonItemData item = null;
            switch ((DropItemType)info.type)
            {
                case DropItemType.Soldier:
                    {
                        item = new CommonItemData(ConfigManager.Instance.mSoldierData.FindById(info.id));
                        item.Num = info.count;
                        break;
                    }
                case DropItemType.Equip:
                    {
                        item = new CommonItemData(ConfigManager.Instance.mEquipData.FindById(info.id));
                        item.Num = info.count;
                        break;
                    }
                case DropItemType.Item:
                    {
                        item = new CommonItemData(ConfigManager.Instance.mItemData.GetItemInfoByID(info.id));
                        item.Num = info.count;
                        break;
                    }
                case DropItemType.Number:
                    {
                        item = new CommonItemData(info.id, info.count, true);
                        break;
                    }
                case DropItemType.DropPack:
                    {
                        List<DroppackInfo> pack1 = ConfigManager.Instance.mDroppackData.GetSonPackByID(info.id);
                        list.AddRange(GetCommonItemDataList(pack1));
                        break;
                    }
                default:
                    {
                        item = null;
                        break;
                    }
            }
            if (item != null)
            {
                list.Add(item);
            }
        }
        return list;
    }

    public static void LogDropData(DropList data)
    {
        foreach (fogs.proto.msg.ItemInfo info in data.item_list)
        {
            Debug.LogWarning("DropData Item id = " + info.id + " change_num " + info.change_num);
        }
        foreach (fogs.proto.msg.Equip info in data.equip_list)
        {
            Debug.LogWarning("DropData Equip id = " + info.id);
        }
        foreach (fogs.proto.msg.Soldier info in data.soldier_list)
        {
            Debug.LogWarning("DropData Soldier id = " + info.id);
        }
        foreach (fogs.proto.msg.ItemInfo info in data.special_list)
        {
            Debug.LogWarning("DropData Info id =" + info.id + " change_num = " + info.change_num);
        }
    }

    /// <summary>
    /// 检测士兵是否已上阵
    /// </summary>
    /// <param name="soldierID"></param>
    /// <returns></returns>
    public static bool IsAlreadyBattle(ulong soldierUID)
    {
        List<ulong> soldiers = PlayerData.Instance._MajorDungeonSoldierList;
        if (soldiers != null && soldiers.Count > 0)
        {
            for (int i = 0; i < soldiers.Count; i++)
            {
                if (soldiers[i] == soldierUID)
                {
                    return true;
                }
            }
        }
        if (PlayerData.Instance._ArenaInfo != null && PlayerData.Instance._ArenaInfo.defence_soldiers != null)
        {
            for (int i = 0; i < PlayerData.Instance._ArenaInfo.defence_soldiers.Count; i++)
            {
                SoldierList soldier = PlayerData.Instance._ArenaInfo.defence_soldiers[i];
                if (soldier == null)
                    continue;
                if (soldier.uid == soldierUID)
                {
                    return true;
                }
            }
        }

        if (PlayerData.Instance._QualifyingInfo != null && PlayerData.Instance._QualifyingInfo.defence_soldiers != null)
        {
            for (int i = 0; i < PlayerData.Instance._QualifyingInfo.defence_soldiers.Count; i++)
            {
                SoldierList soldier = PlayerData.Instance._QualifyingInfo.defence_soldiers[i];
                if (soldier == null)
                    continue;
                if (soldier.uid == soldierUID)
                {
                    return true;
                }
            }
        }
        return ReadyBattleSoldierManager.Instance.IsAlreadyBattle(soldierUID);
    }
    /// <summary>
    /// 金币 = 基数 * 兑换率 N 次方；N = 玩家当前兑换的次数
    /// 金币 = 基数+ 次数*系数
    /// </summary>
    /// <param name="factor"></param>
    /// <param name="exchangeTimes"></param>
    /// <param name="baseNum"></param>
    /// <returns></returns>
    public static int GetExchangeCoins(float factor, int exchangeTimes, int baseNum)
    {
        return (int)(baseNum + exchangeTimes * factor); //(int)(baseNum * Math.Pow(factor, exchangeTimes));
    }
    public static bool XmlStringIsNull(string str)
    {
        return string.IsNullOrEmpty(str) || str == "0";
    }
    public static List<string> GetSplitStr(string str, char factor)
    {
        List<string> res = new List<string>();
        string[] strs = str.Split(factor);
        res.AddRange(strs);
        return res;
    }
    public static List<int> GetParseStrToInt(string str)
    {
        return GetParseStrToInt(str, ';');
    }
    public static List<uint> GetParseStrToUint(string str)
    {
        return GetParseStrToUint(str, ';');
    }
    public static List<int> GetParseStrToInt(string str, char splitChar)
    {
        if (XmlStringIsNull(str))
        {
            return null;
        }
        string[] strArray1 = str.Split(splitChar);
        if (strArray1 != null && strArray1.Length > 0)
        {
            int Len = strArray1.Length;
            List<int> result = new List<int>();
            for (int i = 0; i < Len; i++)
            {
                if (string.IsNullOrEmpty(strArray1[i]))
                    continue;
                int data = int.Parse(strArray1[i]);
                result.Add(data);
            }
            return result;
        }
        return null;
    }
    public static List<uint> GetParseStrToUint(string str, char splitChar)
    {
        if (XmlStringIsNull(str))
        {
            return null;
        }
        string[] strArray1 = str.Split(splitChar);
        if (strArray1 != null && strArray1.Length > 0)
        {
            int Len = strArray1.Length;
            List<uint> result = new List<uint>();
            for (int i = 0; i < Len; i++)
            {
                if (string.IsNullOrEmpty(strArray1[i]))
                    continue;
                uint data = uint.Parse(strArray1[i]);
                result.Add(data);
            }
            return result;
        }
        return null;
    }

    public static void PlayOpenAnimation(GameObject go, bool isReverse = false)
    {
        TweenScale tweenScale = go.GetComponent<TweenScale>();
        if (tweenScale == null)
        {
            tweenScale = go.AddComponent<TweenScale>();
        }
        tweenScale.delay = 0.5f;
        go.SetActive(true);
        tweenScale.from = Vector3.one * GlobalConst.ViewScaleAnim;
        tweenScale.to = Vector3.one;
        tweenScale.Restart();
        if (isReverse)
        {
            tweenScale.PlayReverse();
        }
        else
        {
            tweenScale.PlayForward();
        }
    }

    public static List<KeyValuePair<string, string>> GetWeaponAttributeDesc(ShowInfoWeapon weapon)
    {
        List<KeyValuePair<string, string>> attribute_dic = new List<KeyValuePair<string, string>>();
        if (weapon.HP != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_HP, ConstString.hp_max + ":" + weapon.HP.ToString());
            attribute_dic.Add(tmp);
        }
        if (weapon.Attack != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Attack, ConstString.phy_atk + ":" + weapon.Attack.ToString());
            attribute_dic.Add(tmp);
        }
        if (weapon.Accuracy != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_AccRate, ConstString.acc_rate + ":" + weapon.Accuracy.ToString());
            attribute_dic.Add(tmp);
        }
        if (weapon.Dodge != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_DdgRate, ConstString.ddg_rate + ":" + weapon.Dodge.ToString());
            attribute_dic.Add(tmp);
        }
        if (weapon.Crit != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Crit, ConstString.crt_rate + ":" + weapon.Crit.ToString());
            attribute_dic.Add(tmp);
        }
        if (weapon.Tenacity != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Tenacity, ConstString.tnc_rate + ":" + weapon.Tenacity.ToString());
            attribute_dic.Add(tmp);
        }
        if (weapon.MoveSpeed != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Speed, ConstString.speed + ":" + weapon.MoveSpeed.ToString());
            attribute_dic.Add(tmp);
        }
        return attribute_dic;
    }

    public static List<KeyValuePair<string, string>> GetWeaponAttributeDescNoWord(ShowInfoWeapon weapon)
    {
        List<KeyValuePair<string, string>> attribute_dic = new List<KeyValuePair<string, string>>();
        if (weapon.HP != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_HP, weapon.HP.ToString());
            attribute_dic.Add(tmp);
        }
        if (weapon.Attack != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Attack, weapon.Attack.ToString());
            attribute_dic.Add(tmp);
        }
        if (weapon.Accuracy != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_AccRate, weapon.Accuracy.ToString());
            attribute_dic.Add(tmp);
        }
        if (weapon.Dodge != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_DdgRate, weapon.Dodge.ToString());
            attribute_dic.Add(tmp);
        }
        if (weapon.Crit != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Crit, weapon.Crit.ToString());
            attribute_dic.Add(tmp);
        }
        if (weapon.Tenacity != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Tenacity, weapon.Tenacity.ToString());
            attribute_dic.Add(tmp);
        }
        if (weapon.MoveSpeed != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Speed, weapon.MoveSpeed.ToString());
            attribute_dic.Add(tmp);
        }
        return attribute_dic;
    }

    /// <summary>
    /// 设置字体颜色
    /// </summary>
    /// <param name="vLabel"></param>
    /// <param name="vColorR"></param>
    /// <param name="vColorG"></param>
    /// <param name="vColorB"></param>
    /// <param name="vColorA"></param>
    /// <param name="vColorER"></param>
    /// <param name="vColorEG"></param>
    /// <param name="vColorEB"></param>
    /// <param name="vColorEA"></param>
    public static void SetLabelColor_I(UILabel vLabel, byte vColorR = 255, byte vColorG = 255, byte vColorB = 255, byte vColorA = 255, byte vColorER = 255, byte vColorEG = 255, byte vColorEB = 255, byte vColorEA = 255)
    {
        Color32 color32_font = new Color32(vColorR, vColorG, vColorB, vColorA);
        Color32 color32_effect = new Color32(vColorER, vColorEG, vColorEB, vColorEA);
        SetLabelColor(vLabel, color32_font, color32_effect);
    }

    /// <summary>
    /// 设置字体颜色
    /// </summary>
    /// <param name="vLabel"></param>
    /// <param name="vColor"></param>
    /// <param name="vColorE"></param>
    public static void SetLabelColor(UILabel vLabel, Color vColor, Color vColorE)
    {
        if (vLabel == null)
            return;
        if (vColor != null)
            vLabel.color = vColor;
        if (vColorE != null)
            vLabel.effectColor = vColorE;
    }

    /// <summary>
    /// 更改按钮文字颜色  isEnabel为false时  表示按钮灰化不可用
    /// </summary>
    /// <param name="vLabel"></param>
    /// <param name="isEnabel"></param>
    public static void SetUILabelColor(UILabel vLabel, bool isEnabel)
    {
        Color32 color32;
        if (isEnabel)
        {
            color32 = new Color32(111, 52, 14, 255);
            vLabel.effectStyle = UILabel.Effect.Outline;
        }
        else
        {
            color32 = new Color32(54, 54, 54, 255);
            vLabel.effectStyle = UILabel.Effect.None;
        }
        vLabel.color = color32;
    }


    /// <summary>
    /// 设置物件层级
    /// </summary>
    /// <param name="vTrans"></param>
    /// <param name="vLayer"></param>
    public static void SetObjLayer(Transform vTrans, int vLayer)
    {
        if (vTrans == null)
            return;
        vTrans.gameObject.layer = vLayer;
        for (int i = 0; i < vTrans.childCount; i++)
            SetObjLayer(vTrans.GetChild(i), vLayer);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="min">最小字符数（包含）</param>
    /// <param name="max">最大字符数（包含）</param>
    /// <returns></returns>
    public static bool CheckStringRule(string str, int min, int max, string lengthErrTip = "")
    {
        bool result = false;
        string patten = @"^[\u4e00-\u9fa5A-Za-z0-9]+$";
        Match match = Regex.Match(str, patten);
        if (match.Success)
        {
            char[] nameArr = str.ToCharArray();
            int len = 0;
            for (int i = 0; i < nameArr.Length; i++)
            {
                if ((nameArr[i] >= 'a' && nameArr[i] <= 'z') || (nameArr[i] >= 'A' && nameArr[i] <= 'Z') || (nameArr[i] >= '0' && nameArr[i] <= '9'))
                {
                    ++len;
                }
                else
                {
                    len += 2;
                }
                if (len > max)
                {
                    break;
                }
            }
            if (len > max || len < min)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,
                                                          string.IsNullOrEmpty(lengthErrTip)
                                                              ? string.Format(ConstString.ERR_STRING_LENGTH, min, max)
                                                              : lengthErrTip);
            }
            else
            {
                result = true;
            }
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_STRING_SPECIAL_LETTER);
        }

        return result;
    }

    public static string GetUnionMemberJobString(UnionPosition position)
    {
        string result = string.Empty;
        switch (position)
        {
            case UnionPosition.UP_MEMBER:
                {
                    result = ConstString.UNION_JOB_MEMBER;
                    break;
                }
            case UnionPosition.UP_CHAIRMAN:
                {
                    result = ConstString.UNION_JOB_CHAIRMAN;
                    break;
                }
            case UnionPosition.UP_VICE_CHAIRMAN:
                {
                    result = ConstString.UNION_JOB_VICE_CHAIRMAN;
                    break;
                }

        }
        return result;
    }

    public static void CopyRankInfo(RankInfo source, RankInfo target)
    {
        if (source == null)
            target = null;
        target.charid = source.charid;
        target.combat_power = source.combat_power;
        target.frame = source.frame;
        target.high_grade = source.high_grade;
        target.high_victory = source.high_victory;
        target.icon = source.icon;
        target.level = source.level;
        target.name = source.name;
        target.rank = source.rank;
        target.union_name = source.union_name;
        target.use_time = source.use_time;
        target.accid = source.accid;
        target.area_id = source.area_id;
        target.vip_level = source.vip_level;
        //target.altar_status = source.altar_status;

        if (source.union_info != null)
        {
            if (target.union_info == null)
            {
                target.union_info = new RankUnionInfo();
            }
            target.union_info.union_id = source.union_info.union_id;
            target.union_info.union_name = source.union_info.union_name;
            target.union_info.chairman = source.union_info.chairman;
            target.union_info.altar_status = source.union_info.altar_status;
            target.union_info.union_level = source.union_info.union_level;
            target.union_info.icon = source.union_info.icon;
            target.union_info.max_members = source.union_info.max_members;
            target.union_info.members = source.union_info.members;
            target.union_info.value = source.union_info.value;
            target.union_info.value_time = source.union_info.value_time;
            target.union_info.host_union_name = source.union_info.host_union_name;
            target.union_info.rank = source.union_info.rank;
        }
    }

    public static ArenaPlayer CreateArenaPlayerInfo(RankInfo baseInfo, List<ArenaSoldier> soldierList)
    {
        if (baseInfo == null)
            return null;
        ArenaPlayer player = new ArenaPlayer();
        player.hero = new ArenaHero();
        player.hero.level = (int)baseInfo.level;
        player.hero.charid = baseInfo.charid;
        player.hero.charname = baseInfo.name;
        player.hero.unionname = baseInfo.union_name;
        player.combat_power = (int)baseInfo.combat_power;
        player.rank = baseInfo.rank;
        player.hero.icon = baseInfo.icon;
        player.hero.icon_frame = baseInfo.frame;
        player.hero.area_id = (int)baseInfo.area_id;
        player.hero.accid = baseInfo.accid;
        player.hero.vip_lv = (int)baseInfo.high_grade;
        player.soldiers.AddRange(soldierList);
        return player;
    }

    public static PlayerInfoTypeEnum GetInfoTypeByRankType(RankType type)
    {
        switch (type)
        {

            case RankType.ARENA_RANK:
                return PlayerInfoTypeEnum.Arena;
            case RankType.COMBAT_RANK:
            case RankType.LEVEL_RANK:
                return PlayerInfoTypeEnum.MostLineup;
            case RankType.ENDLESS_A:
            case RankType.ENDLESS_B:
            case RankType.ENDLESS_C:
                return PlayerInfoTypeEnum.EndlessPlayerInfo;
            case RankType.CAMPAIGN_PLAYER:
                return PlayerInfoTypeEnum.Integral;
            case RankType.POLE_RANK:
                return PlayerInfoTypeEnum.QualifyingLog;
            default:
                return PlayerInfoTypeEnum.MostLineup;
        }
    }

    public static GameAcitvityStateEnum GetActivityStateByTime(ulong startTime, ulong endTime, long nowTime, out int days)
    {
        days = 0;
        if (startTime == 0 || endTime == 0)
        {
            return GameAcitvityStateEnum.Eternal;
        }
        TimeSpan ts;
        DateTime sDateTime = GetTimeByLong((long)startTime);
        DateTime eDateTime = GetTimeByLong((long)endTime);
        DateTime nDateTime = GetTimeByLong(nowTime);
        GameAcitvityStateEnum state = GameAcitvityStateEnum.NotStart;
        if (sDateTime.CompareTo(nDateTime) > 0)
        {
            ts = sDateTime.Subtract(nDateTime);
            days = Mathf.Max(1, ts.Days);
            state = GameAcitvityStateEnum.NotStart;
        }
        else if (nDateTime.CompareTo(eDateTime) > 0)
        {
            ts = nDateTime.Subtract(eDateTime);
            days = Mathf.Max(1, ts.Days);
            state = GameAcitvityStateEnum.Expired;
        }
        else
        {
            ts = eDateTime.Subtract(nDateTime);
            days = Mathf.Max(1, ts.Days);
            state = GameAcitvityStateEnum.InProgress;
        }
        //Debug.Log("now  " + nDateTime.ToShortDateString() + " start " + sDateTime.ToShortDateString() + " end " + eDateTime.ToShortDateString() + " sub " + ts.Days);
        return state;
    }
    /// <summary>
    /// 输入 2015-12-4 00:00:00
    /// </summary>
    /// <param name="endTime"></param>
    /// <param name="nowTime"></param>
    /// <returns></returns>
    //public static string GetActivityDateString(ulong startTime,ulong endTime)
    //{
    //if (XmlStringIsNull(endTime))
    //{
    //    return ConstString.GAMEACTIVTIY_STATE_ETERNAL;
    //}
    //DateTime eDateTime = Convert.ToDateTime(endTime);
    //return string.Format(ConstString.GAMEACTIVTIY_STATE_DATESTR, eDateTime.Year, eDateTime.Month, eDateTime.Day, eDateTime.Hour);
    //}

    public static void ShakeGameObj(Transform trans, float time)
    {
        ShakeGameObj comp = trans.GetComponent<ShakeGameObj>();
        if (comp == null)
        {
            comp = trans.gameObject.AddComponent<ShakeGameObj>();
        }
        comp.Shake(time);
    }

    public static BaseUnion CopyBaseUnion(BaseUnion source)
    {
        if (source == null)
        {
            return null;
        }
        BaseUnion destination = new BaseUnion();
        destination.id = source.id;				//军团id
        destination.chairman = source.chairman;		//军团长的charname
        destination.level = source.level;			//等级
        destination.vitality = source.vitality;		//活跃度	
        destination.name = source.name;			//名字
        destination.icon = source.icon;			//军团图标
        destination.member_num = source.member_num;		//成员数量
        destination.limit_type = source.limit_type;		//军团限制类型
        destination.limit_level = source.limit_level;				//军团等级限制	
        destination.prev_week_vitality = source.prev_week_vitality;		//上周活跃度
        return destination;
    }

    public static List<RankInfo> CopyRankInfo(List<RankInfo> source)
    {
        List<RankInfo> list = new List<RankInfo>();
        if (source == null || source.Count < 1)
            return list;
        int count = source.Count;
        for (int i = 0; i < count; i++)
        {
            RankInfo sourceInfo = source[i];
            RankInfo newData = new RankInfo();
            newData.accid = sourceInfo.accid;
            newData.level = sourceInfo.level;
            newData.charid = sourceInfo.charid;
            newData.name = sourceInfo.name;
            newData.union_name = sourceInfo.union_name;
            newData.combat_power = sourceInfo.combat_power;
            newData.high_victory = sourceInfo.high_victory;
            newData.high_grade = sourceInfo.high_grade;
            newData.rank = sourceInfo.rank;
            newData.icon = sourceInfo.icon;
            newData.frame = sourceInfo.frame;
            newData.use_time = sourceInfo.use_time;
            newData.area_id = sourceInfo.area_id;
            newData.accid = sourceInfo.accid;
            newData.vip_level = sourceInfo.vip_level;
            if (sourceInfo.union_info != null)
            {
                if (newData.union_info == null)
                {
                    newData.union_info = new RankUnionInfo();
                }
                newData.union_info.union_id = sourceInfo.union_info.union_id;
                newData.union_info.union_name = sourceInfo.union_info.union_name;
                newData.union_info.chairman = sourceInfo.union_info.chairman;
                newData.union_info.altar_status = sourceInfo.union_info.altar_status;
                newData.union_info.union_level = sourceInfo.union_info.union_level;
                newData.union_info.icon = sourceInfo.union_info.icon;
                newData.union_info.max_members = sourceInfo.union_info.max_members;
                newData.union_info.members = sourceInfo.union_info.members;
                newData.union_info.value = sourceInfo.union_info.value;
                newData.union_info.value_time = sourceInfo.union_info.value_time;
                newData.union_info.host_union_name = sourceInfo.union_info.host_union_name;
                newData.union_info.rank = sourceInfo.union_info.rank;
            }


            list.Add(newData);
        }
        return list;
    }
    public static string GetUnionPvpStateStr(UnionPvpState state)
    {
        string result = "";
        result = ConstString.UNION_PVP_STATE[(int)state - 1];
        return result;
    }
    /// <summary>
    /// 检测PVP排名  返回Ture则说明已上榜
    /// </summary>
    /// <returns></returns>
    public static bool CheckPVPRank(uint rank)
    {
        if (rank > GlobalCoefficient.PVPRankLimit)
            return false;
        return true;
    }

    public static void ShowOfflineTip()
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightSetPause, null);
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.OFFLINE_TIP, Main.Instance.LoginOut);
    }

    public static void ShowVipLvNotEnoughTip(string content)
    {
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, content,
            () =>
            {
                UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
                UISystem.Instance.VipRechargeView.ShowRecharge();
            }
                                                  , null, ConstString.FORMAT_RECHARGE);
    }
    public static string GetStepShow(UILabel label, int num, bool isZero = false)
    {
        if(label != null)
        {
            label.color = new Color(1,0.89f,0);
            label.effectDistance = new Vector2(2,2);
            float x = label.transform.localPosition.x;
            if (x == 0 || x == 60)
                label.transform.localPosition = new Vector3(label.transform.localPosition.x - 30, label.transform.localPosition.y, label.transform.localPosition.z);
        }
        if (num <= 0 && !isZero)
            return string.Empty;
        return string.Format("+{0}", num);
    }
    public static bool CheckFuncIsOpen(uint vType)
    {
        return CheckFuncIsOpen((ETaskOpenView)vType);
    }
    public static bool CheckFuncIsOpen(ETaskOpenView vType)
    {
        string tmpCondition = string.Empty;
        string tmpHint = string.Empty;
        OpenLevelData tmpOpenLevelData = null;
        return CheckFuncIsOpen((ETaskOpenView)vType, out tmpCondition, out tmpHint, out tmpOpenLevelData);
    }
    public static bool CheckFuncIsOpen(uint vType, out string vContent, out string vHint, out OpenLevelData vOpenLevelData)
    {
        return CheckFuncIsOpen((ETaskOpenView)vType, out vContent, out vHint, out vOpenLevelData);
    }
    public static bool CheckFuncIsOpen(ETaskOpenView vType, out string vCondition, out string vHint, out OpenLevelData vOpenLevelData)
    {
        vCondition = string.Empty;
        vHint = string.Empty;
        vOpenLevelData = null;

        Dictionary<ETaskOpenView, OpenFunctionType> dicTaskJumpTo = PlayerData.Instance.ObtainOpenRelationShip;
        if ((dicTaskJumpTo == null) || (dicTaskJumpTo.Count <= 0))
        {
            return false;
        }
        OpenFunctionType tmpOpenFunctionType = OpenFunctionType.None;
        if (dicTaskJumpTo.ContainsKey(vType))
        {
            tmpOpenFunctionType = dicTaskJumpTo[vType];
        }
        //Debug.LogError(vType.ToString() + "|" + tmpOpenFunctionType.ToString());
        vOpenLevelData = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(tmpOpenFunctionType);

        if (!CheckIsOpen(tmpOpenFunctionType, false))
        {
            if (vOpenLevelData == null)
            {
                return false;
            }

            if ((vType == ETaskOpenView.Union) || (vType == ETaskOpenView.Shop_Union) || (vType == ETaskOpenView.UnionPVE))
            {
                if (!UnionModule.Instance.HasUnion)
                {
                    vCondition = string.Format(ConstString.ERR_NO_UNION, vOpenLevelData.openLevel);
                    vHint = string.Format(ConstString.ERR_NO_UNION, vOpenLevelData.openLevel);
                    return false;
                }
            }

            if (vOpenLevelData.openLevel != -1)
            {
                vCondition = string.Format(ConstString.UNLOCK_LEVEL_0, vOpenLevelData.openLevel);//ConstString.BACKPACK_LEVELLOCKTIP//
                vHint = string.Format(ConstString.UNLOCK_LEVEL_1, vOpenLevelData.openLevel);//ConstString.FORMAT_TIP_OPEN_FUNC_NO_LV//
            }
            if (vOpenLevelData.vipLevel != -1)
            {
                if (string.IsNullOrEmpty(vCondition))
                {
                    vCondition = string.Format(ConstString.UNLOCK_VIP_0, vOpenLevelData.vipLevel);//ConstString.BACKPACK_VIPLOCKTIP//
                }
                else
                {
                    vCondition = string.Format(ConstString.UNLOCK_VIP_1, vCondition, vOpenLevelData.vipLevel);
                }
                if (string.IsNullOrEmpty(vHint))
                {
                    vHint = string.Format(ConstString.UNLOCK_VIP_0, vOpenLevelData.vipLevel);//ConstString.FORMAT_TIP_OPEN_FUNC_NO_VIPLV//
                }
                else
                {
                    vHint = string.Format(ConstString.UNLOCK_VIP_1, vHint, vOpenLevelData.vipLevel);
                }
            }
            if (vOpenLevelData.gateId != -1)
            {
                StageInfo tmpStageInfo = ConfigManager.Instance.mStageData.GetInfoByID((uint)vOpenLevelData.gateId);
                if (tmpStageInfo != null)
                {
                    if (string.IsNullOrEmpty(vCondition))
                    {
                        vCondition = string.Format(ConstString.UNLOCK_GATE_0, tmpStageInfo.GateSequence);//ConstString.BACKPACK_GATELOCKTIP//
                    }
                    else
                    {
                        vCondition = string.Format(ConstString.UNLOCK_GATE_1, vCondition, tmpStageInfo.GateSequence);
                    }
                    if (string.IsNullOrEmpty(vHint))
                    {
                        vHint = string.Format(ConstString.UNLOCK_GATE_2, tmpStageInfo.GateSequence);//ConstString.FUNCTION_LOCK//
                    }
                    else
                    {
                        vHint = string.Format(ConstString.UNLOCK_GATE_3, vHint, tmpStageInfo.GateSequence);
                    }
                }
            }

            vCondition = string.Format("{0}{1}", vCondition, ConstString.GATE_ESCORT_LOCKTIP_UNLOCK);
            vHint = string.Format(ConstString.UNLOCK_SHOWHINT, vHint);
            return false;
        }

        if (vOpenLevelData != null)
        {
            if ((vType == ETaskOpenView.Union) || (vType == ETaskOpenView.Shop_Union) || (vType == ETaskOpenView.UnionPVE))
            {
                if (!UnionModule.Instance.HasUnion)
                {
                    //vCondition = string.Format(ConstString.ERR_NO_UNION, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Sociaty).openLevel);
                    //vHint = string.Format(ConstString.ERR_NO_UNION, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Sociaty).openLevel);
                    vCondition = string.Format(ConstString.ERR_NO_UNION, vOpenLevelData.openLevel);
                    vHint = string.Format(ConstString.ERR_NO_UNION, vOpenLevelData.openLevel);
                    return false;
                }
            }
        }
        return true;
    }

    public static bool CheckFuncIsOpen(OpenFunctionType functionType, bool showHint = true)
    {
        return CheckFuncIsOpen(functionType, PlayerData.Instance._VipLv, PlayerData.Instance._Level, showHint);
    }

    public static bool CheckFuncIsOpen(OpenFunctionType functionType, uint vip, uint lv, bool showHint = true)
    {
        bool result = false;
        OpenLevelData data = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(functionType);
        if (data == null)
            return result;
        if (data.openLevel == -1)
        {
            result = vip >= data.vipLevel;
            if (showHint && !result)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_NO_VIPLV, data.vipLevel));
        }
        else if (data.vipLevel == -1)
        {
            result = lv >= data.openLevel;
            if (showHint && !result)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_NO_LV, data.openLevel));
        }
        else
        {
            result = lv >= data.openLevel || vip >= data.vipLevel;
            if (showHint && !result)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_NO_LV_AND_VIPLV, data.vipLevel, data.openLevel));
        }
        return result;
    }

    public static bool CheckIsOpen(OpenFunctionType functionType, bool ShowHint = false)
    {
        OpenLevelData data = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(functionType);
        if (data == null)
            return true;
        string Level = "";
        string vip = "";
        string gate = "";
        if (data.openLevel != -1)
        {
            if (PlayerData.Instance._Level < data.openLevel)
            {
                Level = string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_LV, data.openLevel);
            }
            else
                return true;
        }
        if (data.vipLevel != -1)
        {
            if (PlayerData.Instance._VipLv < data.vipLevel)
            {
                vip = string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_VIP, data.vipLevel);
            }
            else
                return true;

        }
        if (data.gateId != -1)
        {
            if (!PlayerData.Instance.IsPassedGate((uint)data.gateId))
            {
                StageInfo info = ConfigManager.Instance.mStageData.GetInfoByID((uint)data.gateId);
                if (info != null)
                {
                    gate = string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_GATE, info.GateSequence);
                }
            }
            else
                return true;

        }
        if (!string.IsNullOrEmpty(Level) || !string.IsNullOrEmpty(vip) || !string.IsNullOrEmpty(gate))
        {
            vip = string.IsNullOrEmpty(Level) || string.IsNullOrEmpty(vip) ? vip : ConstString.GATE_SWEEPTIP_EITHER + vip;
            gate = string.IsNullOrEmpty(vip) || string.IsNullOrEmpty(gate) ? gate : ConstString.GATE_SWEEPTIP_EITHER + gate;
            string str = Level + vip + gate + ConstString.FORMAT_TIP_OPEN_FUNC_OPEN;
            if (ShowHint)
            {
                if (!string.IsNullOrEmpty(vip))
                    CommonFunction.ShowVipLvNotEnoughTip(str);
                else
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, str);
            }
            return false;
        }
        return true;
    }

    public static string OpenTargetView(ETaskOpenView type)
    {
        return OpenTargetView(((int)type).ToString());
    }

    public static string OpenTargetView(string openStr)
    {
        string tmpResult = string.Empty;
        string[] view = openStr.Split(':');
        ETaskOpenView viewType = (ETaskOpenView)int.Parse(view[0]);
        if (viewType == ETaskOpenView.None)
            return tmpResult;

        switch (viewType)
        {
            case ETaskOpenView.ActivityDungeon:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Activity))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_ACTIVITIES;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ACTIVITIES);
                    break;
                }
            case ETaskOpenView.Arena:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Arena))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_PVPVIEW;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PVPVIEW);
                    break;
                }
            case ETaskOpenView.Bag_Equip:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Package))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_BACKPACK;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BACKPACK);
                    UISystem.Instance.BackPackView.UpdateViewInfo();
                    break;
                }
            case ETaskOpenView.Bag_GodWeapon:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Package))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_BACKPACK;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BACKPACK);
                    UISystem.Instance.BackPackView.UpdateViewInfo();
                    break;
                }
            case ETaskOpenView.DungeonWithId:
                {
                    // 当type为这个的时候
                    // 在读取 view[1] 的值 确定定位的关卡
                    if (!CheckFuncIsOpen(OpenFunctionType.PVE))
                        return tmpResult;
                    if (view.Length < 1)
                    {
                        return tmpResult;
                    }
                    uint gateID = uint.Parse(view[1]);
                    if (PlayerData.Instance.IsAvailableGate(gateID))
                    {
                        tmpResult = ViewType.DIR_VIEWNAME_GATE;
                        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATE);
                        UISystem.Instance.GateView.UpdateViewInfo(gateID);
                    }
                    else
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_GATENOTOPEN);
                        return tmpResult;
                    }
                    break;
                }
            case ETaskOpenView.Endless:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Endless))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_ENDLESS;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ENDLESS);
                    break;
                }
            case ETaskOpenView.Expedition:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Expedition))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_EXPEDITION;
                    FightRelatedModule.Instance.SendMatchEnemy((int)MatchType.None);
                    break;
                }
            case ETaskOpenView.Hero:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Hero))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_HEROATT;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_HEROATT);
                    break;
                }
            case ETaskOpenView.LastestAdvancedDungeon:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.PVE))
                        return tmpResult;
                    if (PlayerData.Instance.IsAvailableModeByType(MainBattleType.EliteCrusade))
                    {
                        tmpResult = ViewType.DIR_VIEWNAME_GATE;
                        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATE);
                        UISystem.Instance.GateView.UpdateViewInfo(MainBattleType.EliteCrusade);
                    }
                    else
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_GATENOTOPEN);
                        return tmpResult;
                    }
                    break;
                }
            case ETaskOpenView.LastestNormalDungeon:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.PVE))
                        return tmpResult;
                    if (PlayerData.Instance.IsAvailableModeByType(MainBattleType.Crusade))
                    {
                        tmpResult = ViewType.DIR_VIEWNAME_GATE;
                        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATE);
                        UISystem.Instance.GateView.UpdateViewInfo(MainBattleType.Crusade);
                    }
                    else
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_GATENOTOPEN);
                        return tmpResult;
                    }
                    break;
                }
            case ETaskOpenView.LastestProtectDungeon:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.PVE))
                        return tmpResult;
                    if (PlayerData.Instance.IsAvailableModeByType(MainBattleType.Escort))
                    {
                        tmpResult = ViewType.DIR_VIEWNAME_GATE;
                        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATE);
                        UISystem.Instance.GateView.UpdateViewInfo(MainBattleType.Escort);
                    }
                    else
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_GATENOTOPEN);
                        return tmpResult;
                    }
                    break;
                }
            case ETaskOpenView.Recruit:
                {
                    if (!CheckIsOpen(OpenFunctionType.Recruit, true))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_RECRUITVIEW;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECRUITVIEW);
                    UISystem.Instance.CloseGameUI(TaskView.UIName);
                    if (UISystem.Instance.UIIsOpen(LivenessView.UIName))
                        UISystem.Instance.RecruitView.SetFromUI(LivenessView.UIName);
                    UISystem.Instance.CloseGameUI(LivenessView.UIName);
                    break;
                }
            case ETaskOpenView.Slave:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Slave))
                        return tmpResult;
                    tmpResult = PrisonView.UIName;
                    UISystem.Instance.ShowGameUI(PrisonView.UIName);
                    break;
                }
            case ETaskOpenView.VipPage:
                {
                    tmpResult = VipRechargeView.UIName;
                    UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
                    UISystem.Instance.VipRechargeView.ShowVipPrivilege();
                    break;
                }
            case ETaskOpenView.Soldier:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Soldier))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_SOLDIERATT;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SOLDIERATT);
                    break;
                }
            case ETaskOpenView.RechargePage:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.VIP))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_RECHARGE;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECHARGE);
                    UISystem.Instance.VipRechargeView.ShowRecharge();
                    break;
                }
            case ETaskOpenView.Union:
                {
                    if (!UnionModule.Instance.HasUnion)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,
                            string.Format(ConstString.ERR_NO_UNION, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Sociaty).openLevel));
                        return tmpResult;
                    }
                    tmpResult = UnionView.UIName;
                    UnionModule.Instance.OpenUnion();
                    break;
                }
            case ETaskOpenView.Mall:
                {
                    tmpResult = MallView.UIName;
                    UISystem.Instance.ShowGameUI(MallView.UIName);
                    break;
                }
            case ETaskOpenView.Shop_Nor:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Store))
                        return tmpResult;
                    tmpResult = StoreView.UIName;
                    UISystem.Instance.ShowGameUI(StoreView.UIName);
                    UISystem.Instance.StoreView.ShowStore(ShopType.ST_NomalShop);
                    break;
                }
            case ETaskOpenView.Shop_Medal:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Arena))
                    {
                        return tmpResult;
                    }
                    tmpResult = StoreView.UIName;
                    UISystem.Instance.ShowGameUI(StoreView.UIName);
                    UISystem.Instance.StoreView.ShowStore(ShopType.ST_MedalShop);
                    break;
                }
            case ETaskOpenView.Shop_Honor:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Expedition))
                    {
                        return tmpResult;
                    }
                    tmpResult = StoreView.UIName;
                    UISystem.Instance.ShowGameUI(StoreView.UIName);
                    UISystem.Instance.StoreView.ShowStore(ShopType.ST_HonorShop);
                    break;
                }
            case ETaskOpenView.Shop_Union:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Sociaty))
                    {
                        return tmpResult;
                    }
                    if (!UnionModule.Instance.HasUnion)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,
                            string.Format(ConstString.ERR_NO_UNION, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Sociaty).openLevel));
                        return tmpResult;
                    }
                    tmpResult = StoreView.UIName;
                    UISystem.Instance.ShowGameUI(StoreView.UIName);
                    UISystem.Instance.StoreView.ShowStore(ShopType.ST_UnionShop);
                    break;
                }
            case ETaskOpenView.UnionPVE:
                {
                    Debug.LogWarning(OpenFunctionType.Sociaty.ToString());
                    if (!UnionModule.Instance.HasUnion)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,
                            string.Format(ConstString.ERR_NO_UNION, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Sociaty).openLevel));
                        return tmpResult;
                    }
                    tmpResult = UnionView.UIName;
                    UnionModule.Instance.OpenUnionPve();
                    break;
                }
            case ETaskOpenView.SignView:
                {
                    tmpResult = SignView.UIName;
                    UISystem.Instance.ShowGameUI(SignView.UIName);
                    break;
                }
            case ETaskOpenView.TaskView:
                {
                    tmpResult = TaskView.UIName;
                    UISystem.Instance.ShowGameUI(TaskView.UIName);
                    break;
                }
            case ETaskOpenView.LivenessView:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Liveness))
                        return tmpResult;
                    tmpResult = LivenessView.UIName;
                    UISystem.Instance.ShowGameUI(LivenessView.UIName);
                    break;
                }
            case ETaskOpenView.DrowEquipView:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.YuanBaoGe))
                        return tmpResult;
                    tmpResult = DrowEquipView.UIName;
                    UISystem.Instance.ShowGameUI(DrowEquipView.UIName);
                    break;
                }
            case ETaskOpenView.BindingAccount:
                {
                    if (SDKManager.Instance.IsBindingPlatform() != 0)
                    {
                        string desc = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_IDPLAYBIND_AWARDDESC);
                        SDKManager.Instance.OpenTouristBinding(desc);
                        //绑定账号回来没有刷新任务界面，故直接关闭，然后打开时会刷新
                        if (UISystem.Instance.UIIsOpen(TaskView.UIName))
                            UISystem.Instance.CloseGameUI(TaskView.UIName);
                    }
                    break;
                }
            case ETaskOpenView.GPMarket://跳转跳转到GP/APPLE市场//
                {
#if UNITY_ANDROID
                    FriendSpecialInfo tmpSpecial = ConfigManager.Instance.mFriendSpecialData.FindById(FriendSpecialEnum.APPDOWNLOADURL);
                    if (tmpSpecial != null)
                    {
                        Application.OpenURL(tmpSpecial.Descript);
                    }
#elif UNITY_IPHONE
                    Application.OpenURL("http://www.baidu.com/s?wd=appstore");
#endif
                    TaskModule.Instance.SendCommentTaskFinish();
                    UISystem.Instance.CloseGameUI(TaskView.UIName);
                    break;
                }
            case ETaskOpenView.BuyCoin://打开兑换铜钱窗口//
                {
                    tmpResult = ViewType.DIR_VIEWNAME_BUY_COIN_VIEW;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
                    break;
                }
            case ETaskOpenView.Sacrificial_Soldier://跳转到天将神兵天将页签//
                {
                    //if (!CheckFuncIsOpen(OpenFunctionType.Magical))
                    //    return tmpResult;
                    if (!CheckFuncIsOpen(OpenFunctionType.TianJIangXiTong))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_SACRIFICIAL;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SACRIFICIAL);
                    UISystem.Instance.SacrificialSystemView.SetToggle(true);
                    break;
                }
            case ETaskOpenView.Sacrificial_Equip://跳转到天将神兵神兵页签//
                {
                    //if (!CheckFuncIsOpen(OpenFunctionType.Magical))
                    //    return tmpResult;
                    if (!CheckFuncIsOpen(OpenFunctionType.ShenBingXiTong))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_SACRIFICIAL;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SACRIFICIAL);
                    UISystem.Instance.SacrificialSystemView.SetToggle(false);
                    break;
                }
            case ETaskOpenView.CaptureTerritory:
                {
                    if (!UnionModule.Instance.HasUnion)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,
                            string.Format(ConstString.ERR_NO_UNION, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Sociaty).openLevel));
                        return tmpResult;
                    }
                    if (!CheckFuncIsOpen(OpenFunctionType.GongCheng))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_CAPTURE_TERRITORY;
                    CaptureTerritoryModule.Instance.OpenCaptureTerritory();
                    break;
                }
            case ETaskOpenView.Supermacy:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.BaZhu))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_SUPERMACY;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SUPERMACY);
                    break;
                }
            case ETaskOpenView.QualifyingView:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Qualifying))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_QUALIFYING;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_QUALIFYING);
                    break;
                }
            case ETaskOpenView.ChatView:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Chat))
                        return tmpResult;
                    tmpResult = ViewType.DIR_VIEWNAME_QUALIFYING;
                    UISystem.Instance.CloseGameUI(TaskView.UIName);
                    UISystem.Instance.CloseGameUI(LivenessView.UIName);
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_CHATVIEW);
                    UISystem.Instance.ChatView.UpdateViewInfo(ChatTypeEnum.World);
                    break;
                }
            case ETaskOpenView.LifeSoulView:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.LifeSoulSystm))
                        return tmpResult;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_LIFESPIRITVIW);
                    break;
                }
            case ETaskOpenView.PetSystemView:
                {
                    if (!CheckFuncIsOpen(OpenFunctionType.Pet))
                        return tmpResult;
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PETSYSTEM);
                    break;
                }
        }
        return tmpResult;
        //if (viewType != ETaskOpenView.LivenessView)
        //    UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_LIVENESS);
        //if (viewType != ETaskOpenView.TaskView)
        //    UISystem.Instance.CloseGameUI(TaskView.UIName);
    }

    public static bool CheckBitStatus(int vValue, byte vIndex)
    {
        int tmpResult = ((vValue & (1 << vIndex)) > 0) ? 1 : 0;
        if (tmpResult == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static byte SetBitStatus(int vValue, int vIndex)
    {
        return (byte)(vValue | (1 << vIndex));
    }

    public static void OpenBuySp()
    {
        uint maxCount = ConfigManager.Instance.mVipConfig.GetVipDataByLv(PlayerData.Instance._VipLv).EnergyBuyCount;
        if (PlayerData.Instance.BuySPTimes < maxCount)
        {
            UISystem.Instance.ShowGameUI(BuySPView.UIName);
        }
        else
        {
            if (PlayerData.Instance._VipLv >= ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.Energy))
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.NO_BUY_SP_TIMES);
            }
            else
            {
                CommonFunction.ShowVipLvNotEnoughTip(ConstString.NO_BUY_SP_TIMES_TO_VIP);
            }
        }
    }

    public static string GetXmlElementStr(XmlElement element, string name)
    {
        if (element[name] != null)
        {
            return element[name].InnerText;
        }
        Debug.LogError(string.Format("cant find a xml element with name = {0}", name));
        return string.Empty;
    }

    public static T InstantiateItem<T>(GameObject prefab, Transform parent) where T : MonoBehaviour
    {
        GameObject go = CommonFunction.InstantiateObject(prefab, parent);

        if (go.GetComponent<T>() == null)
            return go.AddComponent<T>();
        else
            return go.GetComponent<T>();
    }
    public static int FilterFramId(int id)
    {
        for (int i = 0; i < EquptedName.frameId.Length; ++i)
        {
            if (id == EquptedName.frameId[i])
            {
                return id;
            }
            if (id == EquptedName.frameId[i] + 1000)
            {
                return EquptedName.frameId[i];
            }
        }
        return id;
    }

    public static string GetConsumeTypeDesc(ECurrencyType type)
    {
        string desc = string.Empty;
        switch (type)
        {
            case ECurrencyType.Honor:
                {
                    desc = ConstString.NAME_HONOR;
                    break;
                }
            case ECurrencyType.Gold:
                {
                    desc = ConstString.NAME_GOLD;
                    break;
                }
            case ECurrencyType.Diamond:
                {
                    desc = ConstString.NAME_DIAMOND;
                    break;
                }
            case ECurrencyType.Medal:
                {
                    desc = ConstString.NAME_MEDAL;
                    break;
                }
            case ECurrencyType.UnionToken:
                {
                    desc = ConstString.NAME_UNIONTOKEN;
                    break;
                }
            case ECurrencyType.RecycleCoin:
                {
                    desc = ConstString.NAME_RECYCLECOIN;
                    break;
                }
        }
        return desc;
    }

    /// <summary>
    /// 顯示離線時間
    /// </summary>
    /// <param name="lbl"></param>
    /// <param name="time"></param>
    /// <param name="vIsOffset">是否需要調整時間</param>
    public static void SetOfflineTime(UILabel lbl, long time, bool vIsOffset = true)
    {
        if (time != 0)
        {
            long offlinetime = 0;
            if (vIsOffset)
            {
                offlinetime = Main.mTime - time;
            }
            else
            {
                offlinetime = time;
            }

            if (offlinetime > 31536000) //一年365算
            {
                lbl.text = string.Format(ConstString.FORMAT_UNION_MEMBER_OFFLINE_TIME_YEAR, offlinetime / 31536000);
            }
            else if (offlinetime > 2592000) //一个月30算
            {
                lbl.text = string.Format(ConstString.FORMAT_UNION_MEMBER_OFFLINE_TIME_MONTH, offlinetime / 2592000);
            }
            else if (offlinetime > 86400) //一天
            {
                lbl.text = string.Format(ConstString.FORMAT_UNION_MEMBER_OFFLINE_TIME_DAY, offlinetime / 86400);
            }
            else if (offlinetime > 3600) //一小时
            {
                lbl.text = string.Format(ConstString.FORMAT_UNION_MEMBER_OFFLINE_TIME_HOUR, offlinetime / 3600);
            }
            else if (offlinetime > 60) //一分钟
            {
                lbl.text = string.Format(ConstString.FORMAT_UNION_MEMBER_OFFLINE_TIME_MINUTE, offlinetime / 60);
            }
            else //一分钟内
            {
                lbl.text = ConstString.FORMAT_UNION_MEMBER_OFFLINE_TIME_ONE_MINUTE;
            }
        }
        else
        {
            lbl.text = ConstString.UNION_MEMBER_ONLINE;
        }
    }

    public static MoneyFlowData ParseMoneyFlowData(string str)
    {
        MoneyFlowData data = new MoneyFlowData();
        string[] values = str.Split(':');
        if (values.Length >= 2)
        {
            data.Type = (ECurrencyType)uint.Parse(values[0]);
            data.Number = int.Parse(values[1]);
        }
        return data;
    }

    public static bool CheckActivityTime(ActivityTimeInfo timeinfo)
    {
        return Main.mTime >= timeinfo.start_time && Main.mTime <= timeinfo.end_time;
    }

    /// <summary>
    /// 判断武将消耗时，该武将上的命魂是否满足可消耗条件 true 表示可消耗
    /// </summary>
    /// <returns></returns>
    public static bool CheckSholierLifeSoul(List<ulong> soliderUIDs, bool showHint = true)
    {
        int count = 0;
        for (int i = 0; i < soliderUIDs.Count; i++)
        {
            count += PlayerData.Instance._LifeSoulDepot.IsSoldierEequpedLifeSoul(soliderUIDs[i]);
        }
        return !PlayerData.Instance._LifeSoulDepot.IsPackWillFull(count, showHint);
    }

    public static void SetLifeSpiritTypeMark(UISprite sprite, int type)
    {
        if (sprite == null)
            return;
        switch (type)
        {
            case 0:
                {
                    sprite.enabled = false;
                    SetSpriteName(sprite, string.Empty);
                } break;
            case 1:
                {
                    sprite.enabled = true;
                    SetSpriteName(sprite, GlobalConst.SpriteName.LIFESPIRIT_HERO_MARK);
                } break;
            case 2:
                {
                    sprite.enabled = true;
                    SetSpriteName(sprite, GlobalConst.SpriteName.LIFESPIRIT_SOLDIER_MARK);
                } break;



        }
    }

    /// <summary>
    /// 重置Spine物件Shader
    /// </summary>
    /// <param name="vSkeletonAnimation"></param>
    /// <param name="vShader"></param>
    public static void ReSetSpineShader(SkeletonAnimation vSkeletonAnimation, Shader vShader)
    {
        if ((vSkeletonAnimation != null) && (vShader != null))
        {
            if ((vSkeletonAnimation.skeletonDataAsset != null) && (vSkeletonAnimation.skeletonDataAsset.atlasAssets != null))
            {
                for (int i = 0; i < vSkeletonAnimation.skeletonDataAsset.atlasAssets.Length; i++)
                {
                    if ((vSkeletonAnimation.skeletonDataAsset.atlasAssets[i] != null) && (vSkeletonAnimation.skeletonDataAsset.atlasAssets[i].materials != null))
                    {
                        for (int j = 0; j < vSkeletonAnimation.skeletonDataAsset.atlasAssets[i].materials.Length; j++)
                        {
                            if (vSkeletonAnimation.skeletonDataAsset.atlasAssets[i].materials[j] != null)
                            {
                                vSkeletonAnimation.skeletonDataAsset.atlasAssets[i].materials[j].shader = vShader;
                            }
                        }
                    }
                }
            }
        }
    }
    //灰化: "Unlit/Transparent Colored Gray"//
    public static void ReSetSpineShader(SkeletonAnimation vSkeletonAnimation, string vShaderName)
    {
        if (!string.IsNullOrEmpty(vShaderName))
        {
            Shader tmpShader = Shader.Find(vShaderName);
            ReSetSpineShader(vSkeletonAnimation, tmpShader);
        }
    }

    public static List<string> GetDescByEquipCoordinatesConfig(EquipCoordinatesInfo info)
    {
        List<string> list = new List<string>();
        if (info != null)
        {
            List<string> hpList = new List<string>();
            List<string> attackList = new List<string>();
            List<string> hitList = new List<string>();
            List<string> dodgeList = new List<string>();
            List<string> critList = new List<string>();
            List<string> uprisingList = new List<string>();
            System.Text.StringBuilder sub = new StringBuilder();
            for (int i = 0; i < info.attributes.Count; i++) //由于读取配置表时已经排序 此处不再排序
            {
                EquipCoordAttribute att = info.attributes[i];
                if (att == null)
                    continue;
                string tmp = string.Empty;
                for (int j = 0; j < att.list.Count; j++)
                {
                    EquipCoordDetailAttribute detailAtt = att.list[j];
                    if (detailAtt == null)
                        continue;
                    switch (detailAtt.type)
                    {
                        case EquipCoordEnum.HP:
                            {
                                tmp = string.Format(ConstString.BACKPACK_TIP_SUITDESC, ConstString.hp_max, detailAtt.att, att.condition);
                                hpList.Add(tmp);
                            }
                            break;
                        case EquipCoordEnum.Attack:
                            {
                                tmp = string.Format(ConstString.BACKPACK_TIP_SUITDESC, ConstString.phy_atk, detailAtt.att, att.condition);
                                attackList.Add(tmp);
                            }
                            break;
                        case EquipCoordEnum.Crit:
                            {
                                tmp = string.Format(ConstString.BACKPACK_TIP_SUITDESC, ConstString.crt_rate, detailAtt.att, att.condition);
                                critList.Add(tmp);
                            }
                            break;
                        case EquipCoordEnum.Dodge:
                            {
                                tmp = string.Format(ConstString.BACKPACK_TIP_SUITDESC, ConstString.ddg_rate, detailAtt.att, att.condition);
                                dodgeList.Add(tmp);
                            }
                            break;
                        case EquipCoordEnum.Hit:
                            {
                                tmp = string.Format(ConstString.BACKPACK_TIP_SUITDESC, ConstString.acc_rate, detailAtt.att, att.condition);
                                hitList.Add(tmp);
                            }
                            break;
                        case EquipCoordEnum.Uprising:
                            {
                                tmp = string.Format(ConstString.BACKPACK_TIP_SUITDESC, ConstString.tnc_rate, detailAtt.att, att.condition);
                                uprisingList.Add(tmp);
                            }
                            break;
                    }
                }
            }
            list.AddRange(hpList);
            list.AddRange(attackList);
            list.AddRange(hitList);
            list.AddRange(critList);
            list.AddRange(dodgeList);
            list.AddRange(uprisingList);
        }
        return list;
    }

    /// <summary>
    /// 超限返回TRUE
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static bool CheckMoneyOverflow(ECurrencyType type, int count)
    {
        long money = 0;
        switch (type)
        {
            case ECurrencyType.Gold:
                money = (long)PlayerData.Instance._Gold;
                break;
            case ECurrencyType.Diamond:
                money = (long)PlayerData.Instance._Diamonds;
                break;
            case ECurrencyType.Medal:
                money = (long)PlayerData.Instance._Medal;
                break;
            case ECurrencyType.Honor:
                money = (long)PlayerData.Instance._Honor;
                break;
            case ECurrencyType.UnionToken:
                money = (long)PlayerData.Instance.UnionToken;
                break;
            case ECurrencyType.RecycleCoin:
                money = (long)PlayerData.Instance.RecycleCoin;
                break;
            default:
                Debug.LogError("ERROR");
                break;
        }

        return (money + (long)count) > int.MaxValue;
    }

    /// <summary>
    /// 检测是否有已上阵武将可以升级
    /// </summary>
    /// <returns></returns>
    public static bool CheckFightSoldierIsCanUpdate()
    {
        bool tmpResult = false;

        //筛选所有已上阵武将//
        List<Soldier> tmpFightSoldierList = new List<Soldier>();
        if ((PlayerData.Instance._SoldierDepot != null) && (PlayerData.Instance._SoldierDepot._soldierList != null))
        {
            foreach (Soldier tmpSoldier in PlayerData.Instance._SoldierDepot._soldierList)
            {
                if (tmpSoldier == null)
                    continue;
                if (tmpSoldier.isMaxLevel())
                    continue;
                if (!CommonFunction.IsAlreadyBattle(tmpSoldier.uId))
                    continue;
                tmpFightSoldierList.Add(tmpSoldier);
            }
        }

        //判断是否有已上阵武将可以升级//
        if (tmpFightSoldierList.Count > 0)
        {
            foreach (Soldier tmpSoldier in tmpFightSoldierList)
            {
                List<ulong> tmpList = GetWholeMaterialSoldierUID(tmpSoldier.uId);
                Soldier tmpChangeInfo = PlayerData.Instance._SoldierDepot.TextStrong(tmpSoldier.uId, tmpList);
                if (tmpChangeInfo == null)
                    continue;
                if (tmpChangeInfo.Level <= tmpSoldier.Level)
                    continue;
                tmpResult = true;
                break;
            }
        }

        return tmpResult;
    }

    /// <summary>
    /// 获取除筛选武将外的所有蓝色品质以下未上阵武将UID
    /// </summary>
    /// <param name="vTargetID">用于筛选的武将UID</param>
    /// <returns></returns>
    public static List<ulong> GetWholeMaterialSoldierUID(ulong vTargetID)
    {
        List<ulong> tmpResult = new List<ulong>();
        if ((PlayerData.Instance._SoldierDepot != null) && (PlayerData.Instance._SoldierDepot._soldierList != null))
        {
            foreach (Soldier tmpSoldier in PlayerData.Instance._SoldierDepot._soldierList)
            {
                if (tmpSoldier == null)
                    continue;
                if (vTargetID == tmpSoldier.uId)
                    continue;
                if (CommonFunction.IsAlreadyBattle(tmpSoldier.uId))
                    continue;
                if (tmpSoldier.Att == null)
                    continue;
                if (tmpSoldier.Att.quality > 3)//蓝色及以下品质//
                    continue;
                tmpResult.Add(tmpSoldier.uId);
            }
        }
        return tmpResult;
    }

    /// <summary>
    /// 设置合理的间隔时间
    /// </summary>
    /// <param name="vCurTime"></param>
    /// <param name="vPreAITime"></param>
    /// <returns></returns>
    public static float GetReasonableIntervalTime(float vCurTime, float vPreAITime)
    {
        //if (vCurTime - vPreAITime >= 0.05f)
        //{
        //    return 0.02f;
        //}
        //else
        //{
            return vCurTime - vPreAITime;
        //}
    }

}


public class ReadyBattleSoldierManager : Singleton<ReadyBattleSoldierManager>
{
    private List<ulong> ExpeditionSoldier = new List<ulong>();
    private List<ulong> ActivityReadySoldier = new List<ulong>();
    private List<ulong> EndlessReadySoldier = new List<ulong>();
    private List<ulong> ExoticAdvantureSoldier = new List<ulong>();
    private List<SoldierList> SlaveReadySoldier = new List<SoldierList>();
    private List<SoldierList> HegemonySoldier = new List<SoldierList>();
    private List<SoldierList> CaptureTerritorySoldier = new List<SoldierList>();
    private List<CommonSoldierData> PVPAttackSoldier = new List<CommonSoldierData>();
    private List<SoldierList> CrossServerWarSoldier = new List<SoldierList>();


    public bool IsAlreadyBattle(ulong soldierUID)
    {
        for (int i = 0; i < ExpeditionSoldier.Count; i++)
        {
            if (ExpeditionSoldier[i] == soldierUID)
            {
                return true;
            }
        }


        for (int i = 0; i < ActivityReadySoldier.Count; i++)
        {
            if (ActivityReadySoldier[i] == soldierUID)
            {
                return true;
            }
        }

        for (int i = 0; i < EndlessReadySoldier.Count; i++)
        {
            if (EndlessReadySoldier[i] == soldierUID)
            {
                return true;
            }
        }

        for (int i = 0; i < ExoticAdvantureSoldier.Count; i++)
        {
            if (ExoticAdvantureSoldier[i] == soldierUID)
            {
                return true;
            }
        }


        for (int i = 0; i < SlaveReadySoldier.Count; i++)
        {
            SoldierList soldier = SlaveReadySoldier[i];
            if (soldier == null)
                continue;
            if (soldier.uid == soldierUID)
            {
                return true;
            }
        }
        for (int i = 0; i < HegemonySoldier.Count; i++)
        {
            SoldierList soldier = HegemonySoldier[i];
            if (soldier == null)
                continue;
            if (soldier.uid == soldierUID)
            {
                return true;
            }
        }
        for (int i = 0; i < CaptureTerritorySoldier.Count; i++)
        {
            SoldierList soldier = CaptureTerritorySoldier[i];
            if (soldier == null)
                continue;
            if (soldier.uid == soldierUID)
            {
                return true;
            }
        }
        for (int i = 0; i < CrossServerWarSoldier.Count; i++)
        {
            SoldierList soldier = CrossServerWarSoldier[i];
            if (soldier == null)
                continue;
            if (soldier.uid == soldierUID)
            {
                return true;
            }
        }
        return false;
    }


    public void UpdateReadySoldier(AppPrefEnum appenum, List<ulong> list)
    {
        switch (appenum)
        {
            case AppPrefEnum.ExpeditionSoldier:
                {
                    ExpeditionSoldier.Clear();
                    ExpeditionSoldier.AddRange(list);
                } break;
            case AppPrefEnum.ActivityReadySoldier:
                {
                    ActivityReadySoldier.Clear();
                    ActivityReadySoldier.AddRange(list);
                } break;
            case AppPrefEnum.EndlessReadySoldier:
                {
                    EndlessReadySoldier.Clear();
                    EndlessReadySoldier.AddRange(list);
                } break;
            case AppPrefEnum.ExoticAdvantureSoldier:
                {
                    ExoticAdvantureSoldier.Clear();
                    ExoticAdvantureSoldier.AddRange(list);
                } break;
        }
    }
    public void UpdateReadySoldier(AppPrefEnum appenum, List<SoldierList> list)
    {
        switch (appenum)
        {
            case AppPrefEnum.SlaveReadySoldier:
                {
                    SlaveReadySoldier.Clear();
                    SlaveReadySoldier.AddRange(list);
                } break;
            case AppPrefEnum.HegemonySoldier:
                {
                    HegemonySoldier.Clear();
                    HegemonySoldier.AddRange(list);
                } break;
            case AppPrefEnum.CaptureTerritorySoldier:
                {
                    CaptureTerritorySoldier.Clear();
                    CaptureTerritorySoldier.AddRange(list);
                } break;
            case AppPrefEnum.CrossServerWarSoldier:
                {
                    CrossServerWarSoldier.Clear();
                    CrossServerWarSoldier.AddRange(list);
                }break;
        }
    }

    public void UpdatePvpAttackSoldier(List<CommonSoldierData> list)
    {
        PVPAttackSoldier.Clear();
        PVPAttackSoldier.AddRange(list);
    }

    public void Init()
    {
        ExpeditionSoldier.Clear();
        ActivityReadySoldier.Clear();
        EndlessReadySoldier.Clear();
        ExoticAdvantureSoldier.Clear();
        SlaveReadySoldier.Clear();
        HegemonySoldier.Clear();
        CaptureTerritorySoldier.Clear();
        PVPAttackSoldier.Clear();
        CrossServerWarSoldier.Clear();
        if (PlayerPrefsTool.HasKey(AppPrefEnum.ExpeditionSoldier))
        {
            List<ulong> saveUIDs = PlayerPrefsTool.ReadObject<List<ulong>>(AppPrefEnum.ExpeditionSoldier);
            if (saveUIDs != null)
            {
                ExpeditionSoldier.AddRange(saveUIDs);
            }
        }
        if (PlayerPrefsTool.HasKey(AppPrefEnum.ActivityReadySoldier))
        {
            List<ulong> saveUIDs = PlayerPrefsTool.ReadObject<List<ulong>>(AppPrefEnum.ActivityReadySoldier);
            if (saveUIDs != null)
            {
                ActivityReadySoldier.AddRange(saveUIDs);
            }
        }
        if (PlayerPrefsTool.HasKey(AppPrefEnum.EndlessReadySoldier))
        {
            List<ulong> saveUIDs = PlayerPrefsTool.ReadObject<List<ulong>>(AppPrefEnum.EndlessReadySoldier);
            if (saveUIDs != null)
            {
                EndlessReadySoldier.AddRange(saveUIDs);
            }
        }
        if (PlayerPrefsTool.HasKey(AppPrefEnum.ExoticAdvantureSoldier))
        {
            List<ulong> saveUIDs = PlayerPrefsTool.ReadObject<List<ulong>>(AppPrefEnum.ExoticAdvantureSoldier);
            if (saveUIDs != null)
            {
                ExoticAdvantureSoldier.AddRange(saveUIDs);
            }
        }
        if (PlayerPrefsTool.HasKey(AppPrefEnum.SlaveReadySoldier))
        {
            List<SoldierList> list = PlayerPrefsTool.ReadObject<List<SoldierList>>(AppPrefEnum.SlaveReadySoldier);
            if (list != null)
            {
                SlaveReadySoldier.AddRange(list);
            }
        }
        if (PlayerPrefsTool.HasKey(AppPrefEnum.HegemonySoldier))
        {
            List<SoldierList> list = PlayerPrefsTool.ReadObject<List<SoldierList>>(AppPrefEnum.HegemonySoldier);
            if (list != null)
            {
                HegemonySoldier.AddRange(list);
            }
        }

        if (PlayerPrefsTool.HasKey(AppPrefEnum.CaptureTerritorySoldier))
        {
            List<SoldierList> list = PlayerPrefsTool.ReadObject<List<SoldierList>>(AppPrefEnum.CaptureTerritorySoldier);
            if (list != null)
            {
                CaptureTerritorySoldier.AddRange(list);
            }
        }
        if (PlayerPrefsTool.HasKey(AppPrefEnum.CrossServerWarSoldier))
        {
            List<SoldierList> list = PlayerPrefsTool.ReadObject<List<SoldierList>>(AppPrefEnum.CrossServerWarSoldier);
            if (list != null)
            {
                CrossServerWarSoldier.AddRange(list);
            }
        }
        if (PlayerPrefsTool.HasKey(AppPrefEnum.PVPAttackSoldier))
        {
            List<CommonSoldierData> list = PlayerPrefsTool.ReadObject<List<CommonSoldierData>>(AppPrefEnum.PVPAttackSoldier);
            if (list != null)
            {
                PVPAttackSoldier.AddRange(list);
            }
        }

    }


}