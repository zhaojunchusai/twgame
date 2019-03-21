using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 富文本处理类
/// 
/// 可处理动态字的富文本,ngui自带的富文本太坑了,不能处理动态字,汉字那么多,怎么做图集,坑爹!!
/// 
/// 实现原理:根据总宽度分割字符串,把每一行内容归类,显示时,根据该行最大高度的组件来定位该行的起始Y坐标.
/// 最终显示结果为,图片和文字底对齐
/// 
/// 要求挂接在需要富文本的label上,该label必须为左上对齐,pivot为topleft,并有固定的width
/// 
/// 富文本以 *#开始 #*结束
///
/// 如*#img=Button#* 
/// 此为图片,Button为资源名
///
/// 如*#select=funcname,value#* 
/// 此为超链接,=号后为函数名和参数,中间以英文逗号隔开
/// </summary>

public class RichTextProcessing : MonoBehaviour
{
    public enum RichTextInfoType
    {
        None,               // 纯文本
        Select,             // 超链接
        Sprite              // 图片
    }

    public enum HyperlinksType
    {
        item = 0,
        hero,
        url,
    }

    public class RichTextStruct
    {
        public bool bNewLine;
        public string info;
        public RichTextInfoType type;
        public string hyperlinksType;
        public string data;
    }

    public class RichGo
    {
        public float xPosition;
        public GameObject go;

        public void Set(float tempXPos, GameObject tempGo)
        {
            xPosition = tempXPos;
            go = tempGo;
        }
    }

    public class RichLine
    {
        public int maxHeight;
        public List<RichGo> goList = new List<RichGo>();
    }

    private GameObject TextLabelTemplate_;

    private float PositionX_ = 0;
    private List<RichTextStruct> RichTextlist_ = new List<RichTextStruct>();
    private List<RichLine> RichLineList = new List<RichLine>();

    private int Depth_;
    private int LabelHeight_;
    private int LabelSpacingX_;
    private int LabelSpacingY_;
    private int LineHeight_;
    private float LineWidth_ = 0;

    private int StartIndex_ = 0;
    private int EndIndex_ = 0;

    private string Command_ = "";

    private string Command_first_ = "";
    private string Command_last_ = "";
    private int EqualIndex_ = -1;
    private List<UITexture> UITextureList;
    /*
    private void Awake() {
        UILabel label = gameObject.GetComponent<UILabel>();
        Depth_ = label.depth + 1;
        LabelHeight_ = label.fontSize;
        LabelSpacingX_ = label.spacingX;
        LabelSpacingY_ = label.spacingY;
        LineWidth_ = label.width;
        Debug.Log(label.localSize.x);
        Debug.Log(label.printedSize.x);
        Debug.Log(label.width);
        Debug.Log(LineWidth_);
    }
    */
    private void AddGameObjectToLine(bool bNewLine, float tempXPos, GameObject tempGo)
    {
        if (bNewLine)
        {
            RichLine newRichLine = new RichLine();
            // 这里处理,第一行的特殊情况
            newRichLine.maxHeight = RichLineList.Count <= 0 ? 0 : LabelHeight_;
            RichLineList.Add(newRichLine);
        }
        if (tempGo == null)
            return;
        RichLine line = RichLineList[RichLineList.Count - 1];
        RichGo go = new RichGo();
        go.Set(tempXPos, tempGo);
        line.goList.Add(go);

        UISprite sprite = go.go.GetComponent<UISprite>();
        if (sprite != null)
        {
            if (line.maxHeight < sprite.height)
                line.maxHeight = sprite.height;
        }
    }

    private void ResetCom()
    {

        UILabel label = gameObject.GetComponent<UILabel>();
        Depth_ = label.depth + 1;
        LabelHeight_ = label.fontSize;
        LabelSpacingX_ = label.spacingX;
        LabelSpacingY_ = label.spacingY;
        LineWidth_ = label.width;

        if (TextLabelTemplate_ == null)
        {
            TextLabelTemplate_ = GameObject.Instantiate(gameObject) as GameObject;
            Destroy(TextLabelTemplate_.GetComponent<RichTextProcessing>());
            TextLabelTemplate_.SetActive(false);
        }
        PositionX_ = 0;
        RichTextlist_.Clear();
        RichLineList.Clear();
        AddGameObjectToLine(true, 0, null);
        CommonFunction.ClearChild(transform);
        StartIndex_ = 0;
        EndIndex_ = 0;
        Command_ = "";
        Command_first_ = "";
        Command_last_ = "";
        EqualIndex_ = -1;
    }

    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="str"></param>
    private void Analyze(string str)
    {
        // 把换行替换
        str = str.Replace("\\n", "*#%%%#*");
        // 遍历字符串
        while (str.Length > 0)
        {
            // 如果有换行或富文本
            StartIndex_ = str.IndexOf("*#");
            if (StartIndex_ >= 0)
            {
                // 如果不是第0个开始,说明前面有纯文本
                if (StartIndex_ > 0)
                {
                    RichTextStruct temp = new RichTextStruct();
                    temp.bNewLine = false;
                    string vText = str.Substring(0, StartIndex_);
                    temp.info = vText;
                    temp.type = RichTextInfoType.None;
                    RichTextlist_.Add(temp);
                    str = str.Remove(0, StartIndex_);
                }

                // 获得结尾字符
                EndIndex_ = str.IndexOf("#*");
                // 有的配对的结尾符
                if (EndIndex_ > 0)
                {
                    // 得到内容
                    Command_ = str.Substring(2, EndIndex_ - 2);
                    // 移除结尾符
                    str = str.Remove(0, EndIndex_ + 2);
                }
                // 没有配对的
                else
                {
                    // 直接入表,是纯文本
                    RichTextStruct temp = new RichTextStruct();
                    temp.bNewLine = false;
                    temp.info = str;
                    temp.type = RichTextInfoType.None;
                    RichTextlist_.Add(temp);
                    str = str.Remove(0, str.Length);
                }

                // 有内容,得到是否有换行
                if (!string.IsNullOrEmpty(Command_))
                {
                    EqualIndex_ = Command_.IndexOf("%%%");
                }
                else
                {
                    // 没有内容,继续循环
                    continue;
                }

                // 如果有换行的
                if (EqualIndex_ >= 0)
                {
                    RichTextStruct temp = new RichTextStruct();
                    temp.bNewLine = true;
                    RichTextlist_.Add(temp);
                }
                else
                {
                    // 其他富文本
                    if (Command_.IndexOf("=") > 0)
                    {
                        CutString(Command_, "=", out Command_first_, out Command_last_);
                    }
                    RichTextStruct temp = new RichTextStruct();
                    temp.bNewLine = false;
                    switch (Command_first_)
                    {
                        case "img":
                            temp.info = Command_last_;
                            temp.type = RichTextInfoType.Sprite;
                            break;
                        case "select":
                            string vText = "";
                            string vEventFunc = "";
                            string vValue = "";
                            if (Command_last_.IndexOf(",") > -1)
                            {
                                // 取得点击参数
                                CutString(Command_last_, ",", out vText, out vEventFunc);
                            }
                            if (vEventFunc.IndexOf(",") > -1)
                            {
                                CutString(vEventFunc, ",", out vEventFunc, out vValue);
                            }
                            if (vText.Length <= 0)
                            {
                                temp.info = Command_last_;
                                temp.type = RichTextInfoType.None;
                            }
                            else
                            {
                                temp.info = vText;
                                temp.type = RichTextInfoType.Select;
                                temp.hyperlinksType = vEventFunc;
                                temp.data = vValue;
                            }
                            break;
                        default:
                            temp.info = Command_;
                            temp.type = RichTextInfoType.None;
                            break;
                    }
                    RichTextlist_.Add(temp);
                }
            }
            else
            {
                RichTextStruct temp = new RichTextStruct();
                temp.bNewLine = false;
                temp.info = str;
                temp.type = RichTextInfoType.None;
                RichTextlist_.Add(temp);
                str = str.Remove(0, str.Length);
            }
        }
    }

    private void CreateAll()
    {
        for (int i = 0; i < RichTextlist_.Count; i++)
        {
            if (RichTextlist_[i].bNewLine)
            {
                PositionX_ = 0;
                AddGameObjectToLine(true, 0, null);
            }
            switch (RichTextlist_[i].type)
            {
                case RichTextInfoType.None:
                    if (!string.IsNullOrEmpty(RichTextlist_[i].info))
                        CreateTextLabel(RichTextlist_[i].info);
                    break;
                case RichTextInfoType.Sprite:
                    if (!string.IsNullOrEmpty(RichTextlist_[i].info))
                        CreateSprite(RichTextlist_[i].info);
                    break;
                case RichTextInfoType.Select:
                    CreateEventLabel(RichTextlist_[i]);
                    break;
                default:
                    break;
            }
        }
    }

    private void SortAndShow()
    {
        float y = 0;
        for (int i = 0; i < RichLineList.Count; i++)
        {

            if (i != 0)
            {
                // 其他行,直接下降最大高度,以及Y间隔
                y -= RichLineList[i].maxHeight;
                y -= LabelSpacingY_;
            }
            else
            {
                // 第一行特殊处理,如果不为0,说明有图片,那么下降图片高度-字体高度
                if (RichLineList[i].maxHeight > 0)
                    y -= (RichLineList[i].maxHeight - LabelHeight_);
            }
            foreach (RichGo iter in RichLineList[i].goList)
            {
                bool isSprite = iter.go.GetComponent<UISprite>() != null;
                // 如果是图片,再下降字体高度的Y坐标
                float tempY = y;
                if (isSprite)
                {
                    tempY -= LabelHeight_;
                    tempY += LabelSpacingY_;
                }
                iter.go.transform.localPosition = new Vector3(iter.xPosition, tempY);
            }
        }
    }

    /// <summary>
    /// 创建普通文本
    /// </summary>
    /// <param name="str"></param>
    private void CreateTextLabel(string str)
    {
        GameObject go = CreateLabel();
        UILabel ul = go.GetComponent<UILabel>();
        ul.width = (int)(LineWidth_ - PositionX_);
        ul.text = str;
        string sbstr = ul.processedText;
        Debug.Log(sbstr);
        ul.text = sbstr;

        // 处理万一在拖动表里,需要拖动,处理碰撞区域和大小
        BoxCollider box = go.GetComponent<BoxCollider>();
        if (box != null)
        {
            box.center = new Vector3(ul.printedSize.x / 2, -ul.printedSize.y / 2);
            box.size = new Vector3(ul.printedSize.x, ul.printedSize.y);
        }

        AddGameObjectToLine(false, PositionX_, go);
        PositionX_ += ul.printedSize.x + LabelSpacingX_;

        str = str.Remove(0, sbstr.Length);
        if (str.Length >= 1)
        {
            AddGameObjectToLine(true, 0, null);
            PositionX_ = 0;
            CreateTextLabel(str);
        }
    }

    /// <summary>
    /// 创建超链接
    /// </summary>
    /// <param name="data"></param>
    private void CreateEventLabel(RichTextStruct data, string lastStr = "")
    {

        GameObject go = CreateLabel();
        UILabel ul = go.GetComponent<UILabel>();

        ul.width = (int)(LineWidth_ - PositionX_);

        string tempStr = data.info;
        if (!string.IsNullOrEmpty(lastStr))
            tempStr = lastStr;

        ul.text = tempStr;
        string sbstr = ul.processedText;
        ul.text = "[u][url=" + data.hyperlinksType + "/" + data.data + "]" + sbstr + "[/url][/u]";

        // 处理碰撞的区域和大小
        BoxCollider box = go.GetComponent<BoxCollider>();
        box.center = new Vector3(ul.printedSize.x / 2, -ul.printedSize.y / 2);
        box.size = new Vector3(ul.printedSize.x, ul.printedSize.y);
        RichTextClickEvent tevent = go.AddComponent<RichTextClickEvent>();

        AddGameObjectToLine(false, PositionX_, go);
        PositionX_ += ul.printedSize.x + LabelSpacingX_;

        tempStr = tempStr.Remove(0, sbstr.Length);
        if (tempStr.Length >= 1)
        {
            AddGameObjectToLine(true, 0, null);
            PositionX_ = 0;
            CreateEventLabel(data, tempStr);
        }
    }

    /// <summary>
    /// 创建图片
    /// </summary>
    /// <param name="str"></param>
    private void CreateSprite(string str)
    {

        GameObject go = NGUITools.AddChild(gameObject);
        go.name = "Sprite";
        UITexture sprite = go.AddComponent<UITexture>();
        if (UITextureList == null)
            UITextureList = new List<UITexture>();
        UITextureList.Add(sprite);
        sprite.depth = Depth_;
        sprite.pivot = UIWidget.Pivot.BottomLeft;
        ResourceLoadManager.Instance.LoadAloneImage(name, (texture) =>
        {
            sprite.mainTexture = texture;

        });
        sprite.MakePixelPerfect();
        // 如果超过宽度,先换行
        if (PositionX_ + sprite.width > LineWidth_)
        {
            PositionX_ = 0;
            AddGameObjectToLine(true, 0, null);
        }
        AddGameObjectToLine(false, PositionX_, go);
        PositionX_ += sprite.width + LabelSpacingX_;
    }

    private GameObject CreateLabel()
    {
        GameObject go = NGUITools.AddChild(gameObject, TextLabelTemplate_);
        Destroy(go.GetComponent<RichTextProcessing>());
        go.name = "Label";
        UILabel label = go.GetComponent<UILabel>();
        label.depth = Depth_;
        label.overflowMethod = UILabel.Overflow.ResizeHeight;
        label.maxLineCount = 1;
        go.SetActive(true);
        return go;
    }

    private void CutString(string iString, string iCommand, out string oFirstStr, out string oLastStr)
    {
        int EqualIndex_ = iString.IndexOf(iCommand);
        oFirstStr = iString.Substring(0, EqualIndex_);
        oLastStr = iString.Substring(EqualIndex_ + iCommand.Length, iString.Length - (EqualIndex_ + iCommand.Length));
    }

    /// <summary>
    /// 触发超链接点击
    /// </summary>
    /// <param name="data"></param>
    public void ClickHyperLinks(string data)
    {
        string[] list = data.Split('/');
        if (list.Length < 2) return;
        HyperlinksType tempType = (HyperlinksType)int.Parse(list[0]);
        string tempValue = list[1];
        Debug.Log(tempType + "," + tempValue);
    }

    /// <summary>
    /// 解析字符串并显示排版
    /// </summary>
    /// <param name="str"></param>
    public void ShowStr(string str)
    {
        // 如果空,返回
        if (string.IsNullOrEmpty(str))
            return;
        ResetCom();

        Analyze(str);

        CreateAll();

        SortAndShow();
    }

    public void OnDisable()
    {
        for (int i = 0; i < UITextureList.Count; i++)
        {
            UITexture texture = UITextureList[i];
            texture.mainTexture = null;
        }
        UITextureList.Clear();
    }
}
