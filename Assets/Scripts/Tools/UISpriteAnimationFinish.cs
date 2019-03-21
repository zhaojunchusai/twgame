//----------------------------------------------
//  
// Copyright chwu
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/UI/Sprite Animation Finish")]
public class UISpriteAnimationFinish : MonoBehaviour
{
    [HideInInspector][SerializeField]protected int mFPS = 30;
    [HideInInspector][SerializeField]protected float mEventTime = 1;
    [HideInInspector] [SerializeField]protected int mDepth = 1;
    [HideInInspector] [SerializeField]protected int mState = 1;
    [HideInInspector][SerializeField]protected string mPrefix = "";
    [HideInInspector][SerializeField]protected bool mLoop = true;
    [HideInInspector][SerializeField] protected bool mSnap = true;
    [HideInInspector][SerializeField]protected bool mIsdisable = true;
    protected UISprite mSprite;
    protected float mDelta = 0f;
    protected int mIndex = 0;
    protected bool mActive = true;
    protected float mTime = 0f;
    protected bool isSendTimeEvent = false;
    protected List<string> mSpriteNames = new List<string>();


    public delegate void IntDelegate(GameObject go, int state);

    public IntDelegate mFinishEvent;

    public IntDelegate mTimeFinshEvent;

    public float eventTmie { get { return mEventTime; } set { mEventTime = value; } }

    public int state { get { return mState; } set { mState = value; } }

    public int depth { get { return mDepth; } set { mDepth = value; } }

    /// <summary>
    /// Number of frames in the animation.
    /// </summary>

    public int frames { get { return mSpriteNames.Count; } }

    /// <summary>
    /// Animation framerate.
    /// </summary>

    public int framesPerSecond { get { return mFPS; } set { mFPS = value; } }

    /// <summary>
    /// Set the name prefix used to filter sprites from the atlas.
    /// </summary>

    public string namePrefix { get { return mPrefix; } set { if (mPrefix != value) { mPrefix = value; RebuildSpriteList(); } } }

    /// <summary>
    /// Set the animation to be looping or not
    /// </summary>

    public bool loop { get { return mLoop; } set { mLoop = value; } }

    /// <summary>
    /// Returns is the animation is still playing or not
    /// </summary>

    public bool isPlaying { get { return mActive; } }

    /// <summary>
    /// Rebuild the sprite list first thing.
    /// </summary>
    public bool isdisable { get { return mIsdisable; } set { mIsdisable = value; } }

    protected virtual void Start() { RebuildSpriteList(); }

    /// <summary>
    /// Advance the sprite animation process.
    /// </summary>

    protected virtual void Update()
    {
        if (mActive && mSpriteNames.Count > 1 && Application.isPlaying && mFPS > 0)
        {
            mDelta += RealTime.deltaTime;
            mTime += RealTime.deltaTime;
            if (mTime >= eventTmie && !isSendTimeEvent)
            {
                isSendTimeEvent = true;
                if (mTimeFinshEvent!=null)
                    mTimeFinshEvent(gameObject,state);
            }
            float rate = 1f / mFPS;

            if (rate < mDelta)
            {

                mDelta = 0;//(rate > 0f) ? mDelta - rate : 0f;

                if (++mIndex >= mSpriteNames.Count)
                {
                    mIndex = 0;
                    mActive = mLoop;
                    if (mFinishEvent != null)
                        mFinishEvent(gameObject,state);
                    if (mIsdisable && !mLoop) 
                    {
                        gameObject.SetActive(false);
                        eventTmie = 0;
                        isSendTimeEvent = false;
                    }
                       
                }

                if (mActive)
                {
                    mSprite.spriteName = mSpriteNames[mIndex];
                    if (mSnap) mSprite.MakePixelPerfect();
                }
            }
        }
    }

    /// <summary>
    /// Rebuild the sprite list after changing the sprite name.
    /// </summary>

    public void RebuildSpriteList()
    {
        if (mSprite == null) mSprite = GetComponent<UISprite>();
        mSpriteNames.Clear();
        mSprite.depth = depth;
        eventTmie = 0;
        isSendTimeEvent = false;
        if (mSprite != null && mSprite.atlas != null)
        {
            List<UISpriteData> sprites = mSprite.atlas.spriteList;

            for (int i = 0, imax = sprites.Count; i < imax; ++i)
            {
                UISpriteData sprite = sprites[i];

                if (string.IsNullOrEmpty(mPrefix) || sprite.name.StartsWith(mPrefix))
                {
                    mSpriteNames.Add(sprite.name);
                }
            }
            mSpriteNames.Sort();
        }
    }

    /// <summary>
    /// Reset the animation to the beginning.
    /// </summary>

    public void Play() { mActive = true; }

    /// <summary>
    /// Pause the animation.
    /// </summary>

    public void Pause() { mActive = false; }

    /// <summary>
    /// Reset the animation to frame 0 and activate it.
    /// </summary>

    public void ResetToBeginning()
    {
        mActive = true;
        mIndex = 0;
        eventTmie = 0;
        isSendTimeEvent = false;
        if (mSprite != null && mSpriteNames.Count > 0)
        {
            mSprite.spriteName = mSpriteNames[mIndex];
            if (mSnap) mSprite.MakePixelPerfect();
        }
    }
}
