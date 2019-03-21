using UnityEngine;
using System.Collections;
using fogs.proto.msg;

public class SortTool
{
    public static int SortUnionMember(UnionMember item1, UnionMember item2)
    {
        int result = 0;

        if (item1 == null && item2 == null)
        {
            result = 0;
        }
        else if (item1 == null)
        {
            result = 1;
        }
        else if (item2 == null)
        {
            result = -1;
        }
        else
        {
            if (item1.position == UnionPosition.UP_CHAIRMAN && item2.position != UnionPosition.UP_CHAIRMAN)
            {
                result = -1;
            }
            else if (item2.position == UnionPosition.UP_CHAIRMAN && item1.position != UnionPosition.UP_CHAIRMAN)
            {
                result = 1;
            }
            else if (item1.position == UnionPosition.UP_VICE_CHAIRMAN && item2.position != UnionPosition.UP_VICE_CHAIRMAN)
            {
                result = -1;
            }
            else if (item2.position == UnionPosition.UP_VICE_CHAIRMAN && item1.position != UnionPosition.UP_VICE_CHAIRMAN)
            {
                result = 1;
            }
            else if (item2.position == UnionPosition.UP_VICE_CHAIRMAN && item1.position == UnionPosition.UP_VICE_CHAIRMAN)
            {
                if (item1.level != item2.level)
                {
                    result = item2.level.CompareTo(item1.level);
                }
                else if (item1.level == item2.level)
                {
                    if (item1.offline_tick == 0 && item2.offline_tick != 0)
                    {
                        result = -1;
                    }
                    else if (item1.offline_tick != 0 && item2.offline_tick == 0)
                    {
                        result = 1;
                    }
                    else if (item1.offline_tick == item2.offline_tick)
                    {
                        result = item1.charid.CompareTo(item2.charid);
                    }
                    else
                    {
                        result = item2.offline_tick.CompareTo(item1.offline_tick);
                    }
                }
            }
            else if (item2.position == UnionPosition.UP_MEMBER && item1.position == UnionPosition.UP_MEMBER)
            {
                if (item1.level != item2.level)
                {
                    result = item2.level.CompareTo(item1.level);
                }
                else if (item1.level == item2.level)
                {
                    if (item1.offline_tick == 0 && item2.offline_tick != 0)
                    {
                        result = -1;
                    }
                    else if (item1.offline_tick != 0 && item2.offline_tick == 0)
                    {
                        result = 1;
                    }
                    else if (item1.offline_tick == item2.offline_tick)
                    {
                        result = item1.charid.CompareTo(item2.charid);
                    }
                    else
                    {
                        result = item2.offline_tick.CompareTo(item1.offline_tick);
                    }
                }
            }

        }
            
        return result;
    }
    public static int SortLivenessTask(LivenessTask item1, LivenessTask item2)
    {
        int result = 0;

        if (item1 == null && item2 == null)
        {
            result = 0;
        }
        else if (item1 == null)
        {
            result = 1;
        }
        else if (item2 == null)
        {
            result = -1;
        }
        else
        {
            if (item1.num >= ConfigManager.Instance.mLivenessConfig.GetLivenessDataByID(item1.id).TimesLimit &&
                item2.num < ConfigManager.Instance.mLivenessConfig.GetLivenessDataByID(item2.id).TimesLimit)
            {
                result = 1;
            }
            else if (item1.num < ConfigManager.Instance.mLivenessConfig.GetLivenessDataByID(item1.id).TimesLimit &&
                item2.num >= ConfigManager.Instance.mLivenessConfig.GetLivenessDataByID(item2.id).TimesLimit)
            {
                result = -1;
            }
            else
            {
                result = item1.id.CompareTo(item2.id);
            }
        }

        return result;
    }
    public static int SortCommonItemDataUp(CommonItemData item1 ,CommonItemData item2)
    {
        int result = 0;

        if (item1 == null && item2 == null)
        {
            result = 0;
        }
        else if (item1 == null)
        {
            result = 1;
        }
        else if (item2 == null)
        {
            result = -1;
        }
        else
        {
            if (item1.Quality > item2.Quality)
            {
                result = 1;
            }
            else if (item1.Quality < item2.Quality)
            {
                result = -1;
            }
            else
            {
                result = item1.ID.CompareTo(item2.ID);
            }
        }
        return result;
    }
}

public class CommonItemDataComparer : System.Collections.Generic.IComparer<CommonItemData>
{
    public int Compare(CommonItemData item1, CommonItemData item2)
    {
        int result = 0;

        if (item1 == null && item2 == null)
        {
            result = 0;
        }
        else if (item1 == null)
        {
            result = 1;
        }
        else if (item2 == null)
        {
            result = -1;
        }
        else
        {
            if (item1.Level > item2.Level)
            {
                result = 1;
            }
            else if (item1.Level < item2.Level)
            {
                result = -1;
            }
            else
            {
                if (item1.StarLv > item2.StarLv)
                {
                    result = 1;
                }
                else if (item1.StarLv < item2.StarLv)
                {
                    result = -1;
                }
                else
                {
                    if (item1.Quality > item2.Quality)
                    {
                        result = 1;
                    }
                    else if (item1.Quality < item2.Quality)
                    {
                        result = -1;
                    }
                    else
                    {
                        result = item1.ID.CompareTo(item2.ID);
                    }
                }
            }            
        }
        return result;
    }
}