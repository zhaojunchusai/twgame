
using System.Collections.Generic;
using UnityEngine;
/*
 * Class Name: PlayerPrefsTool
 * Desc: 工具类，数据存储
 * Date: 2014-7-31 19:30
 * Author：taiwei
 * modify by taiwei 20150427 10:53
*/

/// <summary>
/// 当前App 保存数据类型
/// </summary>
public enum AppPrefEnum
{
    Account,
    AutoLogin,
    PVPAttackSoldier,
    PVPAttackEquip,
    PVPAttackPet,
    ExpeditionSoldier,
    GateReadySoldier,
    ActivityReadySoldier,
    EndlessReadySoldier,
    ExoticAdvantureSoldier,
    ExoticAdvantureEquip,
    ExoticAdvanturePet,
    CaptureTerritorySoldier,
    CaptureTerritoryEquip,
    CaptureTerritoryPet,
    SlaveReadySoldier,
    SlaveReadyEquip,
    SlavePet,
    HegemonySoldier,
    HegemonyEquip,
    HegemonyPet,
    AutoFightStatus,
    AutoFightMove,
    AutoFightSkill,
    AutoFightSummon,
    FightSpeed,
    NewPet,
    CrossServerWarSoldier,
    CrossServerWarEquip,
    CrossServerWarPet
}


public class PlayerPrefsTool
{
    static string GetProjectID(string key)
    {
        if (key.Equals(AppPrefEnum.Account.ToString()) || key.Equals(AppPrefEnum.AutoLogin.ToString()))
        {
            return "Pixone.LTD." + key;
        }
        else
        {
            uint accountID = PlayerData.Instance.GetAccountID();
            uint roleID = PlayerData.Instance.GetRoleID();
            return "Pixone.LTD." + accountID.ToString() + "." + roleID.ToString() + "." + key;
        }
    }


    public static void WriteString(AppPrefEnum key, string value)
    {
        WriteString(key.ToString(), value);
    }

    public static void WriteObject<T>(AppPrefEnum key, T value)
    {
        try
        {
            string mValue = PXTools.XmlSerialize(value);
            WriteString(key, mValue);
        }
        catch (System.Exception e)
        {
            Debug.LogError("can not make " + value.ToString() + "to XMLString:" + e.Message);
        }
    }

    public static void WriteObject<T>(string key, T value)
    {
        try
        {
            string mValue = PXTools.XmlSerialize(value);
            WriteString(key, mValue);
        }
        catch (System.Exception e)
        {
            Debug.LogError("can not make " + value.ToString() + "to XMLString:" + e.Message);
        }

    }

    public static void WriteString(string key, string value)
    {
        try
        {
            PlayerPrefs.SetString(GetProjectID(key), value);
            PlayerPrefs.Save();
        }
        catch (PlayerPrefsException e)
        {
            Debug.LogError("can not save to pref!!!" + e.Message);
        }
    }

    public static void WriteFloat(AppPrefEnum key, float value)
    {
        try
        {
            PlayerPrefs.SetFloat(GetProjectID(key.ToString()), value);
            PlayerPrefs.Save();
        }
        catch (PlayerPrefsException e)
        {
            Debug.LogError("can not save to pref!!!" + e.Message);
        }
    }

    public static void WriteInt(AppPrefEnum key, int value)
    {
        try
        {
            PlayerPrefs.SetInt(GetProjectID(key.ToString()), value);
            PlayerPrefs.Save();
        }
        catch (PlayerPrefsException e)
        {
            Debug.LogError("can not save to pref!!!" + e.Message);
        }
    }


    public static float ReadFloat(AppPrefEnum key, float vDefault = 1f)
    {
        return PlayerPrefs.GetFloat(GetProjectID(key.ToString()), vDefault);
    }

    public static string ReadString(AppPrefEnum key)
    {
        return ReadString(key.ToString());
    }

    public static T ReadObject<T>(AppPrefEnum key)
    {
        //try
        //{
        string value = ReadString(key);
        return PXTools.XmlDeserialize<T>(value);
        //}
        //catch (System.Exception e)
        //{
        //    Debug.LogError("can not convert key:" + key.ToString() + " that data to object;" + e.Message);
        //    return default(T);
        //}

    }

    public static T ReadObject<T>(string key)
    {
        try
        {
            string value = ReadString(key);
            return PXTools.XmlDeserialize<T>(value);
        }
        catch (System.Exception e)
        {
            Debug.LogError("can not convert key:" + key.ToString() + " that data to object;" + e.Message);
            return default(T);
        }
    }

    public static string ReadString(string key)
    {
        return PlayerPrefs.GetString(GetProjectID(key), "");
    }

    public static int ReadInt(AppPrefEnum key, int vDefault = 1)
    {
        return PlayerPrefs.GetInt(GetProjectID(key.ToString()), vDefault);
    }

    public static void WriteBool(AppPrefEnum key, bool value)
    {
        WriteInt(key, value ? 1 : 0);
    }
    public static bool ReadBool(AppPrefEnum key)
    {
        return PlayerPrefs.GetInt(GetProjectID(key.ToString()), 0) == 1;
    }

    public static void DeleteLocalData(AppPrefEnum key)
    {
        PlayerPrefs.DeleteKey(GetProjectID(key.ToString()));
    }

    public static void DeleteLocalData(string key)
    {
        PlayerPrefs.DeleteKey(GetProjectID(key));
    }

    public static void ClearLoacalData()
    {
        PlayerPrefs.DeleteAll();
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(GetProjectID(key));
    }

    public static bool HasKey(AppPrefEnum key)
    {
        return HasKey(key.ToString());
    }

}