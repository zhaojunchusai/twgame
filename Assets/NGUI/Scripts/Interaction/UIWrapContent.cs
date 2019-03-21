//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This script makes it possible for a scroll view to wrap its content, creating endless scroll views.
/// Usage: simply attach this script underneath your scroll view where you would normally place a UIGrid:
/// 
/// + Scroll View
/// |- UIWrappedContent
/// |-- Item 1
/// |-- Item 2
/// |-- Item 3
/// </summary>

[AddComponentMenu("NGUI/Interaction/Wrap Content")]
public class UIWrapContent : MonoBehaviour
{
    public delegate void OnInitializeItem(GameObject go, int wrapIndex, int realIndex);

    /// <summary>
    /// 用于计算
    /// </summary>
    public int itemSizeD = 100;

    /// <summary>
    /// 扩展改写 加一个宽度标示
    /// </summary>
    public int wideCount = 1;

    /// <summary>
    /// 扩展改写 加一个高度标示
    /// </summary>
    public int highCount = 1;

    /// <summary>
    /// Add By Mao, For UIWrapContentScale work with UIWrapContent
    /// </summary>
    public delegate void OnPanelClipMove(UIPanel panel);

	/// <summary>
	/// Width or height of the child items for positioning purposes.
	/// </summary>

	public int itemSize = 100;

	/// <summary>
	/// Whether the content will be automatically culled. Enabling this will improve performance in scroll views that contain a lot of items.
	/// </summary>

	public bool cullContent = true;

	/// <summary>
	/// Minimum allowed index for items. If "min" is equal to "max" then there is no limit.
	/// For vertical scroll views indices increment with the Y position (towards top of the screen).
	/// </summary>

	public int minIndex = 0;

	/// <summary>
	/// Maximum allowed index for items. If "min" is equal to "max" then there is no limit.
	/// For vertical scroll views indices increment with the Y position (towards top of the screen).
	/// </summary>

	public int maxIndex = 0;

	/// <summary>
	/// Callback that will be called every time an item needs to have its content updated.
	/// The 'wrapIndex' is the index within the child list, and 'realIndex' is the index using position logic.
	/// </summary>

	public OnInitializeItem onInitializeItem;
    
	Transform mTrans;
	UIPanel mPanel;
	UIScrollView mScroll;
	bool mHorizontal = false;
	bool mFirstTime = true;
	List<Transform> mChildren = new List<Transform>();

    /// <summary>
    /// Add By Mao 
    /// </summary>
    public OnPanelClipMove OnClipMove;

	/// <summary>
	/// Initialize everything and register a callback with the UIPanel to be notified when the clipping region moves.
	/// </summary>

	protected virtual void Start ()
	{
		SortBasedOnScrollMovement();
		WrapContent();
		if (mScroll != null) mScroll.GetComponent<UIPanel>().onClipMove = OnMove;
		mFirstTime = false;
	}

	/// <summary>
	/// Callback triggered by the UIPanel when its clipping region moves (for example when it's being scrolled).
	/// </summary>

    protected virtual void OnMove(UIPanel panel)
	{
        WrapContent();
        if (OnClipMove != null)
        {
            OnClipMove(panel);
        }
	}

	/// <summary>
	/// Immediately reposition all children.
	/// </summary>

	[ContextMenu("Sort Based on Scroll Movement")]
	public void SortBasedOnScrollMovement ()
	{
		if (!CacheScrollView()) return;

		// Cache all children and place them in order
		mChildren.Clear();
		for (int i = 0; i < mTrans.childCount; ++i)
		{
            if (mTrans.GetChild(i).gameObject.activeSelf)
		        mChildren.Add(mTrans.GetChild(i));
		}

		// Sort the list of children so that they are in order
		if (mHorizontal) mChildren.Sort(UIGrid.SortHorizontal);
		else mChildren.Sort(UIGrid.SortVertical);
		ResetChildPositions();
	}

	/// <summary>
	/// Immediately reposition all children, sorting them alphabetically.
	/// </summary>

	[ContextMenu("Sort Alphabetically")]
	public void SortAlphabetically ()
	{
		if (!CacheScrollView()) return;

		// Cache all children and place them in order
		mChildren.Clear();
		for (int i = 0; i < mTrans.childCount; ++i)
        {
            if (mTrans.GetChild(i).gameObject.activeSelf)
                mChildren.Add(mTrans.GetChild(i));
        }

		// Sort the list of children so that they are in order
		mChildren.Sort(UIGrid.SortByName);
		ResetChildPositions();
	}

    public void ReGetChild()
    {
        if (!CacheScrollView()) return;

        // Cache all children and place them in order
        mChildren.Clear();
        for (int i = 0; i < mTrans.childCount; ++i)
        {
            if (mTrans.GetChild(i).gameObject.activeSelf)
                mChildren.Add(mTrans.GetChild(i));
        }
        
    }
    public void CleanChild()
    {
        if (!CacheScrollView()) return;
        mChildren.Clear();
    }
    /// <summary>
	/// Cache the scroll view and return 'false' if the scroll view is not found.
	/// </summary>

	protected bool CacheScrollView ()
	{
		mTrans = transform;
		mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
		mScroll = mPanel.GetComponent<UIScrollView>();
		if (mScroll == null) return false;
		if (mScroll.movement == UIScrollView.Movement.Horizontal) mHorizontal = true;
		else if (mScroll.movement == UIScrollView.Movement.Vertical) mHorizontal = false;
		else return false;
		return true;
	}

	/// <summary>
	/// Helper function that resets the position of all the children.
	/// </summary>

	void ResetChildPositions ()
	{
		for (int i = 0, imax = mChildren.Count; i < imax; ++i)
		{
			Transform t = mChildren[i];
			t.localPosition = mHorizontal ? new Vector3(i * itemSize, 0f, 0f) : new Vector3(0f, -i * itemSize, 0f);
		}
	}

	/// <summary>
	/// Wrap all content, repositioning all children as needed.
	/// </summary>

	public void WrapContent ()
	{
       float extents = 0;
       int count = 1;
       if (mHorizontal)
       {
           count = Mathf.CeilToInt(mChildren.Count / (float)highCount);
       }
       else 
       {
           count = Mathf.CeilToInt(mChildren.Count / (float)wideCount);
       }
       extents = itemSize * count * 0.5f;
	   Vector3[] corners = mPanel.worldCorners;
		for (int i = 0; i < 4; ++i)
		{
			Vector3 v = corners[i];
			v = mTrans.InverseTransformPoint(v);
			corners[i] = v;
		}
		
		Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);
		bool allWithinRange = true;
		float ext2 = extents * 2f;
		if (mHorizontal)
		{
			float min = corners[0].x - itemSize;
			float max = corners[2].x + itemSize;

			for (int i = 0, imax = mChildren.Count; i < imax; ++i)
			{
				Transform t = mChildren[i];
				float distance = t.localPosition.x - center.x;

				if (distance < -extents)
				{
					Vector3 pos = t.localPosition;
					pos.x += ext2;
					distance = pos.x - center.x;
					int realIndex = Mathf.RoundToInt(pos.x / itemSize);

					if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
					{
						t.localPosition = pos;
						UpdateItem(t, i);
						//t.name = realIndex.ToString();
					}
					else allWithinRange = false;
				}
				else if (distance > extents)
				{
					Vector3 pos = t.localPosition;
					pos.x -= ext2;
					distance = pos.x - center.x;
					int realIndex = Mathf.RoundToInt(pos.x / itemSize);

					if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
					{
						t.localPosition = pos;
						UpdateItem(t, i);
						//t.name = realIndex.ToString();
					}
					else allWithinRange = false;
				}
				else if (mFirstTime) UpdateItem(t, i);

				if (cullContent)
				{
					distance += mPanel.clipOffset.x - mTrans.localPosition.x;
					if (!UICamera.IsPressed(t.gameObject))
						NGUITools.SetActive(t.gameObject, (distance > min && distance < max), false);
				}
			}
		}
		else
		{
			float min = corners[0].y - itemSize;
			float max = corners[2].y + itemSize;
			for (int i = 0, imax = mChildren.Count; i < imax; ++i)
			{
				Transform t = mChildren[i];
				float distance = t.localPosition.y - center.y;

				if (distance < -extents)
				{
					Vector3 pos = t.localPosition;
					pos.y += ext2;
					distance = pos.y - center.y;
					int realIndex = Mathf.RoundToInt(pos.y / itemSize);
					if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
					{
						t.localPosition = pos;
						UpdateItem(t, i);
						//t.name = realIndex.ToString();
					}
					else allWithinRange = false;
				}
				else if (distance > extents)
				{
					Vector3 pos = t.localPosition;
					pos.y -= ext2;
					distance = pos.y - center.y;
					int realIndex = Mathf.RoundToInt(pos.y / itemSize);
					if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
					{
						t.localPosition = pos;
						UpdateItem(t, i);
						//t.name = realIndex.ToString();
					}
					else allWithinRange = false;
				}
				else if (mFirstTime) UpdateItem(t, i);
               
				if (cullContent)
				{
					distance += mPanel.clipOffset.y - mTrans.localPosition.y;
					if (!UICamera.IsPressed(t.gameObject))
						NGUITools.SetActive(t.gameObject, (distance > min && distance < max), false);
				}
			}
		}
		mScroll.restrictWithinPanel = !allWithinRange;
	}

	/// <summary>
	/// Sanity checks.
	/// </summary>

	void OnValidate ()
	{
		if (maxIndex < minIndex)
			maxIndex = minIndex;
		if (minIndex > maxIndex)
			maxIndex = minIndex;
	}

	/// <summary>
	/// Want to update the content of items as they are scrolled? Override this function.
	/// </summary>

	protected virtual void UpdateItem (Transform item, int index)
	{
		if (onInitializeItem != null)
		{
			int realIndex = 0;
            int count = 0;
            if(mScroll.movement == UIScrollView.Movement.Vertical)
            {
                realIndex = Mathf.Abs(Mathf.RoundToInt(item.localPosition.y / itemSize));
                if (wideCount > 1)
                {
                    realIndex = (realIndex) * wideCount;
                    count = realIndex + Mathf.Abs(Mathf.RoundToInt(item.localPosition.x / itemSizeD));
                }
                else 
                {
                    count = realIndex;
                }
              
            }
            else
            {
                realIndex = Mathf.Abs(Mathf.RoundToInt(item.localPosition.x / itemSize));
                if (highCount > 1)
                {
                    realIndex = (realIndex) * highCount;
                    count = realIndex + Mathf.Abs(Mathf.RoundToInt(item.localPosition.y / itemSizeD));
                }
                else 
                {
                    count = realIndex;
                }
            }
            onInitializeItem(item.gameObject, index, count);
		}
	}
}
