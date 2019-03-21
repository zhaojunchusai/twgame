using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;
public class AtlasControl
{
    public struct RectData
    {
        public int top,bottom,left,right;
        public string name;
        public RectData(string name ,int x,int y,int z,int w)
        {
            this.name = name;
            this.top = x;
            this.bottom = y;
            this.left = z;
            this.right = w;
        }
    }
    private static List<RectData> _borderData = new List<RectData>();
    private static List<RectData> _paddingData = new List<RectData>();
    private static Dictionary<string, UIAtlas> _sptToAtlas = new Dictionary<string, UIAtlas>();
    private static string _atlasPath = "Assets/AssetRes/Atlas/";
    private static string _viewPath = "Assets/ExportRes/View";

    #region menu
    //[MenuItem("AtlasTool/1.导出所有图集九宫格信息")]
    /// <summary>
    /// 导出图集九宫格信息
    /// </summary>
    /// <param name="vAltlasPath">图集的地址</param>
    /// <param name="vTxtPath">九宫格信息存储地址</param>
    private static void OutPutNineBox(string vAltlasPath,string vTxtPath)
    {
        StringBuilder stringBuilder = new StringBuilder();
        ReadBorderTxt(vTxtPath);
        List<UIAtlas> atlas = GetAtlas(vAltlasPath);
        foreach (var item in atlas)
        {
            List<UISpriteData> sprites = item.spriteList;
            foreach (var spt in sprites)
            {
                if (spt.hasBorder)
                {
                    if (_borderData != null)
                    {
                        _borderData.RemoveAll((data) =>
                        {
                            return data.name.Equals(spt.name);
                        });
                    }
                    stringBuilder.Append(string.Format("{0},b,{1},{2},{3},{4};",
                        spt.name, spt.borderTop, spt.borderBottom, spt.borderLeft, spt.borderRight));
                }
                if (spt.hasPadding)
                {
                    if (_paddingData != null)
                    {
                        _paddingData.RemoveAll((data) =>
                        {
                            return data.name.Equals(spt.name);
                        });
                    }
                    stringBuilder.Append(string.Format("{0},p,{1},{2},{3},{4};",
                        spt.name, spt.paddingTop, spt.paddingBottom, spt.paddingLeft, spt.paddingRight));
                }
            }
        }
        if (_borderData != null)
        {
            foreach (RectData tmp in _borderData)
            {
                stringBuilder.Append(string.Format("{0},b,{1},{2},{3},{4};",tmp.name, tmp.top, tmp.bottom, tmp.left, tmp.right));
            }
        }
        if (_paddingData != null)
        {
            foreach (RectData tmp in _paddingData)
            {
                stringBuilder.Append(string.Format("{0},b,{1},{2},{3},{4};", tmp.name, tmp.top, tmp.bottom, tmp.left, tmp.right));
            }
        }
        File.WriteAllText(vTxtPath, stringBuilder.ToString());
        AssetDatabase.Refresh();
    }

    //[MenuItem("AtlasTool/3-1.选择图片和文本生成或更新图集")]
    //private static void ChooseFileForAtlas()
    //{
    //    Dictionary<string, TextAsset> txtDic = new Dictionary<string, TextAsset>();
    //    Dictionary<string, Texture> pngDic = new Dictionary<string, Texture>();
    //    Dictionary<string, string> pathDic = new Dictionary<string, string>();

    //    Object[] objs = Selection.objects;
    //    foreach (var item in objs)
    //    {
    //        string str = AssetDatabase.GetAssetPath(item);
    //        if (str.Substring(str.LastIndexOf('.'), str.Length - str.LastIndexOf('.')) == ".txt")
    //        {
    //            TextAsset txt = AssetDatabase.LoadAssetAtPath(str, typeof(TextAsset)) as TextAsset;
    //            txtDic.Add(txt.name, txt);
    //        }
    //        if (str.Substring(str.LastIndexOf('.'), str.Length - str.LastIndexOf('.')) == ".png")
    //        {
    //            Texture png = AssetDatabase.LoadAssetAtPath(str, typeof(Texture)) as Texture;
    //            string path = str.Substring(0, str.LastIndexOf('/') + 1);
    //            pngDic.Add(png.name, png);
    //            pathDic.Add(png.name, path);
    //        }
    //    }

    //    foreach (string item in pathDic.Keys)
    //    {
    //        if (pngDic.ContainsKey(item) && txtDic.ContainsKey(item))
    //        {
    //            CreateOrUpdateAtlas(pngDic[item], txtDic[item], pathDic[item]);
    //        }
    //    }
    //}

    //[MenuItem("AtlasTool/3-2.根据贴图更新图集目录下所有图集,没有则生成")]
    //private static void AddOrUpdateAllAtlas()
    //{
    //    Dictionary<string, TextAsset> txtDic = new Dictionary<string, TextAsset>();
    //    Dictionary<string, Texture> pngDic = new Dictionary<string, Texture>();
    //    Dictionary<string, string> pathDic = new Dictionary<string, string>();
    //    int index;
    //    string dirpath = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + _atlasPath;
    //    DirectoryInfo dir = new DirectoryInfo(dirpath);
    //    FileInfo[] pngfiles = dir.GetFiles("*.png", SearchOption.AllDirectories);
    //    FileInfo[] txtfiles = dir.GetFiles("*.txt", SearchOption.AllDirectories);
    //    foreach (var item in txtfiles)
    //    {
    //        TextAsset txt = AssetDatabase.LoadAssetAtPath(GetAssetPathByFileFullPath(item.FullName), typeof(TextAsset)) as TextAsset;
    //        txtDic.Add(txt.name, txt);
    //    }

    //    foreach (var item in pngfiles)
    //    {
    //        string str = GetAssetPathByFileFullPath(item.FullName);
    //        Texture png = AssetDatabase.LoadAssetAtPath(str, typeof(Texture)) as Texture;
    //        string path = str.Substring(0, str.LastIndexOf('/') + 1);
    //        pngDic.Add(png.name, png);
    //        pathDic.Add(png.name, path);
    //    }

    //    foreach (string item in pathDic.Keys)
    //    {
    //        if (pngDic.ContainsKey(item) && txtDic.ContainsKey(item))
    //        {
    //            CreateOrUpdateAtlas(pngDic[item], txtDic[item], pathDic[item]);
    //        }
    //    }
    //}

    //[MenuItem("AtlasTool/4.(导入策划图集后)将九宫格信息写入图集")]
    /// <summary>
    /// 更具九宫格存储文档对图集进行九宫格信息重新赋值
    /// </summary>
    /// <param name="vTxtPath">九宫格存储文档地址</param>
    /// <param name="vAtlasPath">需要被赋值的图集地址</param>
    private static void WriteNineBoxToAtlas(string vTxtPath,string vAtlasPath)
    {
        ReadBorderTxt(vTxtPath);
        GetAtlas(vAtlasPath);

        foreach (var item in _borderData)
        {
            if (_sptToAtlas.ContainsKey(item.name))
            {
                _sptToAtlas[item.name].GetSprite(item.name).
                    SetBorder(item.left, item.bottom, item.right, item.top);

                EditorUtility.SetDirty(_sptToAtlas[item.name].gameObject);
            }
            else
            {
                Debug.LogError("ERROR: " + item.name);
            }
        }

        foreach (var item in _paddingData)
        {
            if (_sptToAtlas.ContainsKey(item.name))
            {
                _sptToAtlas[item.name].GetSprite(item.name).
                    SetPadding(item.left, item.bottom, item.right, item.top);

                EditorUtility.SetDirty(_sptToAtlas[item.name].gameObject);
            }
            else
            {
                Debug.LogError("ERROR: ");
            }
        }

        AssetDatabase.SaveAssets();
    }
    private static string FindInStringBuilderByIndex(StringBuilder vStr,int index)
    {
        string result = "";
        return result;
    }
    //[MenuItem("AtlasTool/5.给UI重新赋值图集")]
    /// <summary>
    /// 对UI预制进行图集重新赋值
    /// </summary>
    /// <param name="vAtlasPath">需要设置的图集的地址</param>
    /// <param name="vViewPath">需要被设置的界面预制的地址</param>
    private static void SetViewAtlas(string vAtlasPath,string vViewPath)
    {
        GetAtlas(vAtlasPath);
        List<UISprite> spts = GetAllSprites(vViewPath);
        Debug.LogError(spts.Count);
        for (int i = 0; i < spts.Count; i++)
        {
            if (_sptToAtlas.ContainsKey(spts[i].spriteName))
            {
                spts[i].atlas = _sptToAtlas[spts[i].spriteName];
                EditorUtility.SetDirty(spts[i].gameObject);
            }
            else
            {
                //Debug.LogError("ERROR : can't find uiatlas by spt name =" + spts[i].spriteName + "gameobjectName:" + spts[i].gameObject.name);
            }
        }
        AssetDatabase.Refresh();
    }

    //[MenuItem("AtlasTool/6.给单个UI重新赋值图集")]
    //private static void SetSingleViewAtlas()
    //{
    //    GetAtlas(_atlasPath);
    //    GameObject go = Selection.activeGameObject;
    //    UISprite[] spts = go.GetComponentsInChildren<UISprite>();
    //    //List<UISprite> spts = GetAllSprites();
    //    Debug.LogError(spts.Length);
    //    for (int i = 0; i < spts.Length; i++)
    //    {
    //        if (_sptToAtlas.ContainsKey(spts[i].spriteName))
    //        {
    //            spts[i].atlas = _sptToAtlas[spts[i].spriteName];
    //            EditorUtility.SetDirty(spts[i].gameObject);
    //        }
    //        else
    //        {
    //            Debug.LogError("ERROR : can't find uiatlas by spt name =" + spts[i].spriteName);
    //        }
    //    }
    //    AssetDatabase.Refresh();
    //}
    [MenuItem("AtlasTool/7.工程下面指定文件夹内的所有图片生成图集并且重新赋值界面和九宫格信息(全部刷新功能，所有图片重新编排)")]
    private static void AltlasCreate()
    {
        OutPutNineBox("Assets/AtlasSave/CommonAtlas/", TexturePack.GetFileNameFull("Assets/AtlasSave/Txt/12345.txt"));
        //设置图片格式
        TexturePack.LoopSetTexture(TexturePack.TexturePath);
        AssetDatabase.Refresh();
        //生成图集
        TexturePack.TextureMake(TexturePack.FilePath, TexturePack.OutPutPaht, TexturePack.OutPutFileName, TexturePack.OutPutAlphaFileName, TexturePack.OutPutTxtFileName);
        AssetDatabase.Refresh();
        //设置图集格式
        TexturePack.LoopSetTexture(TexturePack.OutTexturePath, TextureImporterFormat.ETC_RGB4);
        AssetDatabase.Refresh();
        //生成unity的图集预制
        SetAtlas(TexturePack.OutCommonAtlasPath);
        AssetDatabase.Refresh();
        WriteNineBoxToAtlas(TexturePack.GetFileNameFull("Assets/AtlasSave/Txt/12345.txt"), "Assets/AtlasSave/CommonAtlas/");
        AssetDatabase.Refresh();
        //SetViewAtlas(TexturePack.OutCommonAtlasPath, _viewPath);
        //AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("", "Completed", "OK");
    }
    [MenuItem("AtlasTool/8.更改或者添加图片")]
    private static void RefreshTexture()
    {
        OutPutNineBox("Assets/AtlasSave/CommonAtlas/", TexturePack.GetFileNameFull("Assets/AtlasSave/Txt/12345.txt"));
        //设置图片格式
        TexturePack.LoopSetTexture(TexturePack.TexturePath);
        AssetDatabase.Refresh();
        //刷新图集
        TexturePack.Clear();
        RefreshAtlas("Assets/AtlasSave/CommonAtlas/", "Assets/AtlasSave/");
        AssetDatabase.Refresh();
        //设置图集格式
        TexturePack.LoopSetTexture(TexturePack.OutTexturePath, TextureImporterFormat.ETC_RGB4);
        AssetDatabase.Refresh();
        //生成unity的图集预制
        SetAtlas(TexturePack.OutCommonAtlasPath);
        AssetDatabase.Refresh();
        WriteNineBoxToAtlas(TexturePack.GetFileNameFull("Assets/AtlasSave/Txt/12345.txt"), "Assets/AtlasSave/CommonAtlas/");
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("", "Completed", "OK");
    }
    [MenuItem("AtlasTool/9.添加新图片")]
    private static void AddTexture()
    {
        TexturePack.LoopSetTexture(TexturePack.TexturePath);
        AssetDatabase.Refresh();
        //刷新图集9
        TexturePack.Clear();
        RefreshAtlas("Assets/AtlasSave/CommonAtlas/", "Assets/AtlasSave/",false);
        AssetDatabase.Refresh();
        //设置图集格式
        TexturePack.LoopSetTexture(TexturePack.OutTexturePath, TextureImporterFormat.ETC_RGB4);
        AssetDatabase.Refresh();
        //生成unity的图集预制
        SetAtlas(TexturePack.OutCommonAtlasPath);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("", "Completed", "OK");
    }
    [MenuItem("AtlasTool/10.选中界面预制刷新图集关联")]
    private static void ttttttt()
    {
        Object[] selectObjArray = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        GetAtlas("Assets/AtlasSave/CommonAtlas/");
        List<UISprite> spts = new List<UISprite>();
        foreach (var item in selectObjArray)
        {
            GameObject view = item as GameObject;
            if (view != null)
            {
                SetSprites(view.transform);
            }
        }
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("", "Completed", "OK");
    }
    [MenuItem("AtlasTool/11.测试按钮")]
    private static void FindPath()
    {
        BesiegeFindTool.StartFindPathAll(BesiegeFindTool.myArray,10,10);
    }
    #endregion
    private static void SetSprites(Transform trans)
    {
        UISprite tmpSprite = trans.GetComponent<UISprite>();
        if (tmpSprite != null)
            Debug.LogError(tmpSprite.atlas.name);
        UIPopupList tmpPop = trans.GetComponent<UIPopupList>();
        if (tmpSprite != null)
        {
            if (_sptToAtlas.ContainsKey(tmpSprite.spriteName))
            {
                if (tmpSprite.atlas == null || !tmpSprite.atlas.name.Equals("HintViewAtlas"))
                {
                    tmpSprite.atlas = _sptToAtlas[tmpSprite.spriteName];
                    EditorUtility.SetDirty(tmpSprite.gameObject);
                }
            }
        }
        if (tmpPop != null)
        {
            if (_sptToAtlas.ContainsKey(tmpPop.backgroundSprite))
            {
                if (tmpPop.atlas == null || !tmpPop.atlas.name.Equals("HintViewAtlas"))
                {
                    tmpPop.atlas = _sptToAtlas[tmpPop.backgroundSprite];
                    EditorUtility.SetDirty(tmpPop.gameObject);
                }
            }
        }
        if (trans.childCount > 0)
        {
            for (int i = 0; i < trans.childCount; i++)
            {
                SetSprites(trans.GetChild(i));
            }
        }
    }
    private static void RefreshAtlas(string atlasPath, string picturePath, bool isRefresh = true)
    {
        string path_atlas = TexturePack.GetFileNameFull(atlasPath);
        string path_picture = TexturePack.GetFileNameFull(picturePath);
        List<Texture2D> overFlowPack = new List<Texture2D>();
        if (!Directory.Exists(path_atlas) || !Directory.Exists(path_picture))
            return;
        List<string> files = new List<string>(Directory.GetFiles(path_atlas, "*.prefab"));
        files.Sort((left,right) => 
        {
            if(left.Length != right.Length)
            {
                if (left.Length < right.Length)
                    return -1;
                else
                    return 1;

            }
            if (!left.Equals(right))
            {
                if (left.CompareTo(right) < 0)
                    return -1;
                else
                    return 1;
            }
            return 0;
        });
        string[] files_picture = Directory.GetFiles(path_picture, "*.png");
        List<string> png_file = new List<string>(files_picture);
        List<string> tmp_png = new List<string>(png_file.Count + 1);
        if (png_file == null || png_file.Count <= 0)
        {
            Debug.LogError("There are no pictures");
            return;
        }
        if (files_picture != null && files_picture.Length > 0)
        {
            if(files != null && files.Count > 0)
            {
                for (int i = 0; i < files.Count; ++i)
                {
                    tmp_png.Clear();
                    string tmpPath = TexturePack.GetFileNameAsset(files[i]);
                    if (string.IsNullOrEmpty(tmpPath))
                        continue;
                    UIAtlas atlas = AssetDatabase.LoadAssetAtPath(tmpPath,
                    typeof(UIAtlas)) as UIAtlas;
                    List<UISpriteData> tmpSprite = atlas.spriteList;
                    if (tmpSprite == null || tmpSprite.Count <= 0)
                        continue;
                    for (int j = 0; j < tmpSprite.Count; ++j)
                    {
                        UISpriteData tmpData = tmpSprite[j];
                        string tmp = png_file.Find((tt) =>
                        {
                            if (string.IsNullOrEmpty(tt))
                                return false;
                            string fileName = TexturePack.GetFileName(tt);
                            string[] filesTmp = fileName.Split('.');
                            if (filesTmp.Length != 2)
                                Debug.LogError("There is a png had error name :" + fileName);
                            if (tmpData.name.Equals(filesTmp[0]))
                                return true;
                            return false;
                        });
                        if (string.IsNullOrEmpty(tmp))
                        {
                            if (isRefresh)
                                Debug.LogError("There is a png be delete:" + tmpData.name);
                            continue;
                        }
                        tmp_png.Add(tmp);
                    }
                    string mainTextureName = atlas.name + ".png";
                    string alphaTextureName = atlas.name + "_A.png";
                    string txtName = atlas.name + ".txt";
                    png_file.RemoveAll((alpha) => { return tmp_png.Contains(alpha); });
                    if (i == files.Count - 1)
                    {
                        if (overFlowPack.Count > 0)
                            Debug.LogError("there had picture canot put in" + i);
                        TexturePack.TextureMakeTheEnd(tmp_png, png_file, overFlowPack, i, TexturePack.OutPutPaht, TexturePack.OutPutFileName, TexturePack.OutPutAlphaFileName, TexturePack.OutPutTxtFileName);
                    }
                    else
                    {
                        if (isRefresh)
                            overFlowPack.AddRange(TexturePack.TextureMakeByFile(tmp_png, TexturePack.OutPutPaht, mainTextureName, alphaTextureName, txtName));
                    }
                }
            }
            else
            {
                if (overFlowPack.Count > 0)
                    Debug.LogError("there had picture canot put in");
                TexturePack.TextureMakeTheEnd(tmp_png, png_file, overFlowPack, 0, TexturePack.OutPutPaht, TexturePack.OutPutFileName, TexturePack.OutPutAlphaFileName, TexturePack.OutPutTxtFileName);
            }
        }
    }
    /// <summary>
    /// 根据png和TextAsset生成图集或更新同目录同名图集
    /// </summary>
    private static void CreateOrUpdateAtlas(Texture texture,TextAsset txt,string path,Texture2D alphaTex = null)
    {
        //AssetDatabase.DeleteAsset(string.Format("{0}{1}.prefab", path, texture.name));
        UIAtlas atlas = AssetDatabase.LoadAssetAtPath(string.Format("{0}{1}.prefab", path, texture.name),
            typeof(UIAtlas)) as UIAtlas;
        
        if (atlas == null)
        {
            Shader shader = Shader.Find("Unlit/Transparent ColoredRGBASeparate");
            
            GameObject go = new GameObject(texture.name);
            atlas = go.AddComponent<UIAtlas>();
            Material mat = new Material(shader);
            mat.mainTexture = texture;
            mat.SetTexture("_AlphaTex", alphaTex);
            atlas.spriteMaterial = mat;
            NGUIJson.LoadSpriteData(atlas, txt);
            AssetDatabase.CreateAsset(mat, string.Format("{0}{1}.mat", path, go.name));
            PrefabUtility.CreatePrefab(string.Format("{0}{1}.prefab", path, go.name), go);
            GameObject.DestroyImmediate(go);
        }
        else
        {
            //Debug.LogError("there have atlas");
            atlas.spriteMaterial.mainTexture = texture;
            NGUIJson.LoadSpriteData(atlas, txt);
        }
        //AssetDatabase.Refresh();
        //TextureToAssetbundle.Execute(atlas.gameObject);
    }
    private static void SetAtlas(string vOutPath)
    {
        for (int i = 0; i < TexturePack.OutFileNames.Count && i < TexturePack.OutPutAlphaFileNames.Count; ++i)
        {
            Object tt = AssetDatabase.LoadMainAssetAtPath(TexturePack.GetFileNameAsset(TexturePack.OutFileNames[i]));
            Texture2D texture = tt as Texture2D;
            Object al = AssetDatabase.LoadMainAssetAtPath(TexturePack.GetFileNameAsset(TexturePack.OutPutAlphaFileNames[i]));
            Texture2D textureAlpha = al as Texture2D;
            TextAsset txt = AssetDatabase.LoadAssetAtPath(TexturePack.GetFileNameAsset(TexturePack.OutPutTxtFileNames[i]), typeof(TextAsset)) as TextAsset;
            if (tt == null)
                Debug.LogError("setatlas png error:" + TexturePack.OutFileNames[i]);
            if (al == null)
                Debug.LogError("setatlas al error:" + TexturePack.OutFileNames[i]);
            if (txt == null)
                Debug.LogError("setatlas txt error:" + TexturePack.OutFileNames[i]);

            CreateOrUpdateAtlas(texture, txt, vOutPath, textureAlpha);
        }
    }
    /// <summary>
    /// 获取指定目录下所有图集
    /// </summary>
    /// <returns></returns>
    private static List<UIAtlas> GetAtlas(string vAtlasPath)
    {
        List<UIAtlas> atlas = new List<UIAtlas>();
        string dirpath = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + vAtlasPath;
        DirectoryInfo dir = new DirectoryInfo(dirpath);
        FileInfo[] files = dir.GetFiles("*.prefab", SearchOption.AllDirectories);
        _sptToAtlas.Clear();
        foreach (var item in files)
        {
            UIAtlas tatlas = (UIAtlas)Resources.LoadAssetAtPath(GetAssetPathByFileFullPath(item.FullName), typeof(UIAtlas));
            if (tatlas != null)
            {
                atlas.Add(tatlas);
                foreach (var spt in tatlas.spriteList)
                {
                    if (!_sptToAtlas.ContainsKey(spt.name))
                        _sptToAtlas.Add(spt.name, tatlas);
                    else
                        Debug.LogError("same spt name :" + spt.name);
                }
            }
        }

        return atlas;
    }
    /// <summary>
    /// 将九宫格记录文件读到内存中
    /// </summary>
    private static void ReadBorderTxt(string vTextPath)
    {
        _borderData.Clear();
        _paddingData.Clear();
        if(!File.Exists(vTextPath))
        {
            return;
        }
        string txt = File.ReadAllText(vTextPath);
        string[] strs = txt.Split(';');
        foreach (var item in strs)
        {
            string[] str = item.Split(',');
            if (str.Length != 6)
            {
                if (str.Length > 0)
                {
                    Debug.LogError(" ERROR: length != 6 ,spt name = " + str[0]);
                }
                else
                {
                    Debug.LogError(" ERROR: length == 0");
                }
                continue;
            }

            if (str[1] == "b")
            {
                _borderData.Add(new RectData(str[0], int.Parse(str[2]), int.Parse(str[3]), int.Parse(str[4]), int.Parse(str[5])));
            }
            else if (str[1] == "p")
            {
                _paddingData.Add(new RectData(str[0], int.Parse(str[2]), int.Parse(str[3]), int.Parse(str[4]), int.Parse(str[5])));
            }
        }
    }
    /// <summary>
    /// 获取所有界面上的UISprite组件
    /// </summary>
    /// <returns></returns>
    private static List<UISprite> GetAllSprites(string vViewPath)
    {
        List<UISprite> allSpts = new List<UISprite>();
        string dirpath = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + vViewPath;
        DirectoryInfo dir = new DirectoryInfo(dirpath);
        FileInfo[] files = dir.GetFiles("*.prefab",SearchOption.AllDirectories);
        foreach (var item in files)
        {
            GameObject view = AssetDatabase.LoadAssetAtPath(GetAssetPathByFileFullPath(item.FullName), typeof(GameObject)) as GameObject;
            if (view != null)
            {
                allSpts.AddRange(GetSprites(view.transform));
            }
        }
        Debug.LogError(allSpts.Count);
        return allSpts;
    }
    /// <summary>
    /// 用递归获取界面每个子节点的UISprite组件
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    private static List<UISprite> GetSprites(Transform trans)
    {
        List<UISprite> list = new List<UISprite>();
        UISprite tmpSprite = trans.GetComponent<UISprite>();
        UIPopupList tt = trans.GetComponent<UIPopupList>();
        if (tmpSprite != null)
        {
            list.Add(tmpSprite);
        }
        if (trans.childCount > 0)
        {
            for (int i = 0; i < trans.childCount; i++)
            {
                list.AddRange(GetSprites(trans.GetChild(i)));
            }
        }
        return list;
    }
    /// <summary>
    /// 获取九宫格记录文件的存放路径
    /// </summary>
    /// <returns></returns>
    private static string GetBorderTxtPath()
    {
        return string.Format("{0}\\__UI\\Border.txt", Application.dataPath.Replace('/', '\\'));
    }
    /// <summary>
    /// 根据File的硬盘绝对路径，获取AssetDatabase可用的相对路径
    /// </summary>
    /// <param name="fullPath"></param>
    /// <returns></returns>
    private static string GetAssetPathByFileFullPath(string fullPath)
    {
        int index = fullPath.IndexOf("Assets\\");
        if (index == -1)
        {
            Debug.LogError("error index = -1 :" + fullPath);
            return "";
        }
        string tpath = fullPath.Substring(index, fullPath.Length - index);
        tpath = tpath.Replace('\\', '/');
        return tpath;
    }
}
public class TexturePack : MonoBehaviour
{
    public static string TexturePath = "Assets/AtlasSave";
    public static string OutTexturePath = "Assets/AtlasSave/OutPut";
    public static string OutCommonAtlasPath = "Assets/AtlasSave/CommonAtlas/";

    public static string FilePath = Application.dataPath + "/AtlasSave";
    public static string OutPutPaht = FilePath + "/OutPut";
    public static string OutPutFileName = "CommonAltlas{0}.png";
    public static string OutPutAlphaFileName = "CommonAltlas{0}_A.png";

    public static string OutPutTxtFileName = "CommonAltlas{0}.txt";
    public static List<string> OutFileNames = new List<string>();
    public static List<string> OutPutAlphaFileNames = new List<string>();
    public static List<string> OutPutTxtFileNames = new List<string>();
    public static string txt = "";
    static int maxsize = 1024;
    /// <summary>
    /// 每张图片间隔像素
    /// </summary>
    public static int interval = 1;
    void Awake()
    {
    }
    /// <summary>
    /// 生成图集
    /// </summary>
    /// <param name="vFilePath"></param>
    /// <param name="vOutPutPath"></param>
    /// <param name="vOutPutFileName"></param>
    /// <param name="vOutPutAlphaFileName"></param>
    /// <param name="vOutPutTxtFileName"></param>
    public static void TextureMake(string vFilePath, string vOutPutPath, string vOutPutFileName, string vOutPutAlphaFileName, string vOutPutTxtFileName)
    {
        OutFileNames.Clear();
        OutPutAlphaFileNames.Clear();
        OutPutTxtFileNames.Clear();
        if (!Directory.Exists(vFilePath))
            return;
        var files = Directory.GetFiles(vFilePath, "*.png");
        if (!Directory.Exists(vOutPutPath))
        {
            Directory.CreateDirectory(vOutPutPath);
        }
        List<Texture2D> pack = new List<Texture2D>(files.Length + 1);
        for (int i = 0; i < files.Length; ++i)
        {
            Texture2D tmp = new Texture2D(0, 0, TextureFormat.ARGB32, false);
            tmp.LoadImage(File.ReadAllBytes(files[i]));
            tmp.name = GetFileName(files[i]);
            pack.Add(tmp);
        }
        pack.RemoveAll((value) => { if (value == null) return true; if (value.width > maxsize || value.height > maxsize) { Debug.LogError(value.name); return true; } return false; });
        pack.Sort((left, right) =>
        {
            if (left.width != right.width)
            {
                if (left.width > right.width)
                    return -1;
                else
                    return 1;
            }
            if (!left.name.Equals(right.name))
            {
                if (left.name.CompareTo(right.name) > 0)
                    return -1;
                else
                    return 1;
            }
            return 0;
        });
        int count = 0;
        while (pack != null && pack.Count > 0)
        {
            Texture2D mapTexture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);
            Texture2D alphaTexture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);

            Color[] colors = new Color[1048576];
            mapTexture.SetPixels(colors);
            mapTexture.wrapMode = TextureWrapMode.Clamp;
            mapTexture.anisoLevel = 1;
            mapTexture.alphaIsTransparency = true;

            for (int i = 0; i < colors.Length; ++i)
            {
                colors[i].a = 1;
            }

            alphaTexture.SetPixels(colors);
            alphaTexture.wrapMode = TextureWrapMode.Clamp;
            alphaTexture.anisoLevel = 1;
            alphaTexture.alphaIsTransparency = true;

            txt = "{\"frames\": {";
            OneTexInPut.OnInput(0, 0, 1024, 1024, mapTexture, alphaTexture, pack);
            txt += "}}";
            string fileName = string.Format(vOutPutPath + "//" + vOutPutFileName, count);
            OutFileNames.Add(fileName);
            File.WriteAllBytes(fileName, mapTexture.EncodeToPNG());
            fileName = string.Format(vOutPutPath + "//" + vOutPutAlphaFileName, count);
            OutPutAlphaFileNames.Add(fileName);
            File.WriteAllBytes(fileName, alphaTexture.EncodeToPNG());
            fileName = string.Format(vOutPutPath + "//" + vOutPutTxtFileName, count);
            OutPutTxtFileNames.Add(fileName);
            File.WriteAllText(fileName, txt);
            ++count;
        }
    }
    public static void Clear()
    {
        OutFileNames.Clear();
        OutPutAlphaFileNames.Clear();
        OutPutTxtFileNames.Clear();
    }
    public static List<Texture2D> TextureMakeByFile(List<string> vFiles, string vOutPutPath, string vOutPutFileName, string vOutPutAlphaFileName, string vOutPutTxtFileName)
    {
        if (vFiles == null || vFiles.Count <= 0)
            return new List<Texture2D>();
        if (!Directory.Exists(vOutPutPath))
        {
            Directory.CreateDirectory(vOutPutPath);
        }
        List<Texture2D> pack = new List<Texture2D>(vFiles.Count + 1);
        for (int i = 0; i < vFiles.Count; ++i)
        {
            Texture2D tmp = new Texture2D(0, 0, TextureFormat.ARGB32, false);
            tmp.LoadImage(File.ReadAllBytes(vFiles[i]));
            tmp.name = GetFileName(vFiles[i]);
            pack.Add(tmp);
        }
        pack.RemoveAll((value) => { if (value == null) return true; if (value.width > maxsize || value.height > maxsize) { Debug.LogError("there is png big than 1024" + value.name); return true; } return false; });
        pack.Sort((left, right) =>
        {
            if (left.width != right.width)
            {
                if (left.width > right.width)
                    return -1;
                else
                    return 1;
            }
            if (!left.name.Equals(right.name))
            {
                if (left.name.CompareTo(right.name) > 0)
                    return -1;
                else
                    return 1;
            }
            return 0;
        });
        int count = 0;
        if (pack != null && pack.Count > 0)
        {
            Texture2D mapTexture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);
            Texture2D alphaTexture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);

            Color[] colors = new Color[1048576];
            mapTexture.SetPixels(colors);
            mapTexture.wrapMode = TextureWrapMode.Clamp;
            mapTexture.anisoLevel = 1;
            mapTexture.alphaIsTransparency = true;

            for (int i = 0; i < colors.Length; ++i)
            {
                colors[i].a = 1;
            }

            alphaTexture.SetPixels(colors);
            alphaTexture.wrapMode = TextureWrapMode.Clamp;
            alphaTexture.anisoLevel = 1;
            alphaTexture.alphaIsTransparency = true;

            txt = "{\"frames\": {";
            OneTexInPut.OnInput(0, 0, 1024, 1024, mapTexture, alphaTexture, pack);
            txt += "}}";
            string fileName = string.Format(vOutPutPath + "//" + vOutPutFileName, count);
            OutFileNames.Add(fileName);
            File.WriteAllBytes(fileName, mapTexture.EncodeToPNG());
            fileName = string.Format(vOutPutPath + "//" + vOutPutAlphaFileName, count);
            OutPutAlphaFileNames.Add(fileName);
            File.WriteAllBytes(fileName, alphaTexture.EncodeToPNG());
            fileName = string.Format(vOutPutPath + "//" + vOutPutTxtFileName, count);
            OutPutTxtFileNames.Add(fileName);
            File.WriteAllText(fileName, txt);
            ++count;
        }
        if (pack.Count > 0)
            Debug.LogError("There is had texture lost:" + vOutPutFileName);
        for (int i = 0; i < pack.Count;++i )
        {
            Debug.LogError(pack[i].name);
        }
        return pack;
    }
    public static void TextureMakeTheEnd(List<string> vFiles, List<string> vFilesAdd, List<Texture2D> vTextureAdd,int vCount, string vOutPutPath, string vOutPutFileName, string vOutPutAlphaFileName, string vOutPutTxtFileName)
    {
        if (vFiles == null || vFilesAdd == null || vTextureAdd == null)
            return ;
        if (!Directory.Exists(vOutPutPath))
        {
            Directory.CreateDirectory(vOutPutPath);
        }
        List<Texture2D> pack = new List<Texture2D>(vFiles.Count + 1 + vFilesAdd.Count + vTextureAdd.Count);
        for (int i = 0; i < vFiles.Count; ++i)
        {
            Texture2D tmp = new Texture2D(0, 0, TextureFormat.ARGB32, false);
            tmp.LoadImage(File.ReadAllBytes(vFiles[i]));
            tmp.name = GetFileName(vFiles[i]);
            pack.Add(tmp);
        }
        pack.RemoveAll((value) => { if (value == null) return true; if (value.width > maxsize || value.height > maxsize) { Debug.LogError(value.name); return true; } return false; });
        pack.Sort((left, right) =>
        {
            if (left.width != right.width)
            {
                if (left.width > right.width)
                    return -1;
                else
                    return 1;
            }
            if (!left.name.Equals(right.name))
            {
                if (left.name.CompareTo(right.name) > 0)
                    return -1;
                else
                    return 1;
            }
            return 0;
        });
        List<Texture2D> tmpPack = new List<Texture2D>(vFilesAdd.Count + vTextureAdd.Count);
        for (int i = 0; i < vFilesAdd.Count; ++i)
        {
            Texture2D tmp = new Texture2D(0, 0, TextureFormat.ARGB32, false);
            tmp.LoadImage(File.ReadAllBytes(vFilesAdd[i]));
            tmp.name = GetFileName(vFilesAdd[i]);
            tmpPack.Add(tmp);
        }
        tmpPack.AddRange(vTextureAdd);
        tmpPack.RemoveAll((value) => { if (value == null) return true; if (value.width > maxsize || value.height > maxsize) { Debug.LogError(value.name); return true; } return false; });
        tmpPack.Sort((left, right) =>
        {
            if (left.width != right.width)
            {
                if (left.width > right.width)
                    return -1;
                else
                    return 1;
            }
            if (!left.name.Equals(right.name))
            {
                if (left.name.CompareTo(right.name) > 0)
                    return -1;
                else
                    return 1;
            }
            return 0;
        });
        pack.AddRange(tmpPack);
        int count = vCount;
        while (pack != null && pack.Count > 0)
        {
            Texture2D mapTexture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);
            Texture2D alphaTexture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);

            Color[] colors = new Color[1048576];
            mapTexture.SetPixels(colors);
            mapTexture.wrapMode = TextureWrapMode.Clamp;
            mapTexture.anisoLevel = 1;
            mapTexture.alphaIsTransparency = true;

            for (int i = 0; i < colors.Length; ++i)
            {
                colors[i].a = 1;
            }

            alphaTexture.SetPixels(colors);
            alphaTexture.wrapMode = TextureWrapMode.Clamp;
            alphaTexture.anisoLevel = 1;
            alphaTexture.alphaIsTransparency = true;

            txt = "{\"frames\": {";
            OneTexInPut.OnInput(0, 0, 1024, 1024, mapTexture, alphaTexture, pack);
            txt += "}}";
            string fileName = string.Format(vOutPutPath + "//" + vOutPutFileName, count);
            OutFileNames.Add(fileName);
            File.WriteAllBytes(fileName, mapTexture.EncodeToPNG());
            fileName = string.Format(vOutPutPath + "//" + vOutPutAlphaFileName, count);
            OutPutAlphaFileNames.Add(fileName);
            File.WriteAllBytes(fileName, alphaTexture.EncodeToPNG());
            fileName = string.Format(vOutPutPath + "//" + vOutPutTxtFileName, count);
            OutPutTxtFileNames.Add(fileName);
            File.WriteAllText(fileName, txt);
            ++count;
        }
    }
    public bool IsFull(List<string> vFiles)
    {
        List<Texture2D> pack = new List<Texture2D>(vFiles.Count + 1);
        for (int i = 0; i < vFiles.Count; ++i)
        {
            Texture2D tmp = new Texture2D(0, 0, TextureFormat.ARGB32, false);
            tmp.LoadImage(File.ReadAllBytes(vFiles[i]));
            tmp.name = GetFileName(vFiles[i]);
            pack.Add(tmp);
        }
        pack.RemoveAll((value) => { if (value == null) return true; if (value.width > maxsize || value.height > maxsize) { Debug.LogError(value.name); return true; } return false; });
        pack.Sort((left, right) =>
        {
            if (left.width != right.width)
            {
                if (left.width > right.width)
                    return -1;
                else
                    return 1;
            }
            if (!left.name.Equals(right.name))
            {
                if (left.name.CompareTo(right.name) > 0)
                    return -1;
                else
                    return 1;
            }
            return 0;
        });
        if (pack != null && pack.Count > 0)
        {
            Texture2D mapTexture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);
            Texture2D alphaTexture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);
            alphaTexture.wrapMode = TextureWrapMode.Clamp;
            alphaTexture.anisoLevel = 1;
            alphaTexture.alphaIsTransparency = true;
            OneTexInPut.OnInput(0, 0, 1024, 1024, mapTexture, alphaTexture, pack);
        }
        if (pack.Count > 0)
            return false;
        return true;
    }
    /// <summary>
    /// 循环设置图片的格式
    /// </summary>
    /// <param name="vPath"></param>
    /// <param name="fomat"></param>
    public static void LoopSetTexture(string vPath, TextureImporterFormat fomat = TextureImporterFormat.RGBA32)
    {
        var files = Directory.GetFiles(vPath, "*.png");
        List<Object> pack = new List<Object>(files.Length + 1);
        for (int i = 0; i < files.Length; ++i)
        {
            Object tt = AssetDatabase.LoadMainAssetAtPath(files[i]);
            Texture2D texture = tt as Texture2D;
            TextureImporter texImporter = GetTextureSettings(files[i], fomat);
            if (texImporter == null)
                continue;
            TextureImporterSettings tis = new TextureImporterSettings();
            texImporter.ReadTextureSettings(tis);
            texImporter.SetTextureSettings(tis);
            AssetDatabase.ImportAsset(files[i]);
        }
    }
    static TextureImporter GetTextureSettings(string path,TextureImporterFormat fomat)
    {
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        if (textureImporter.anisoLevel == 1&&
        textureImporter.filterMode == FilterMode.Bilinear&&
        textureImporter.wrapMode == TextureWrapMode.Clamp&&
        textureImporter.textureType == TextureImporterType.Advanced&&
        textureImporter.maxTextureSize == 1024&&
        textureImporter.textureFormat == fomat&&
        textureImporter.mipmapEnabled == false&&
        textureImporter.alphaIsTransparency == true&&
        //textureImporter.generateCubemap == TextureImporterGenerateCubemap.None&&
        textureImporter.npotScale == TextureImporterNPOTScale.None)
        {
            return null;
        }
        textureImporter.anisoLevel = 1;
        textureImporter.filterMode = FilterMode.Bilinear;
        textureImporter.wrapMode = TextureWrapMode.Clamp;
        textureImporter.textureType = TextureImporterType.Advanced;
        textureImporter.maxTextureSize = 1024;
        textureImporter.textureFormat = fomat;
        textureImporter.mipmapEnabled = false;
        textureImporter.alphaIsTransparency = true;
        //textureImporter.generateCubemap = TextureImporterGenerateCubemap.None;
        textureImporter.npotScale = TextureImporterNPOTScale.None;
        return textureImporter;
    }
    void OnEnable()
    {
    }

    void Update()
    {
    }
    void InputOne()
    {

    }
    public static string GetFileName(string name)
    {
        string tmp = name;
        int index = tmp.LastIndexOf('\\');
        if (index == -1)
            index = tmp.LastIndexOf('/');
        if (index >= name.Length)
            return string.Empty;
        tmp = tmp.Substring(index + 1, name.Length - index - 1);
        return tmp;
    }
    /// <summary>
    /// 根据绝对路径获取Unity的相对路径
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetFileNameAsset(string name)
    {
        string tmp = name;
        int index = tmp.LastIndexOf("Assets/");
        if (index >= name.Length)
            return string.Empty;
        tmp = tmp.Substring(index, name.Length - index);
        tmp = tmp.Replace('\\', '/');
        tmp = tmp.Replace("//", "/");
        return tmp;
    }
    /// <summary>
    /// 根据相对路径获取绝对路径
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetFileNameFull(string name)
    {
        string tmp = name;
        int index = tmp.LastIndexOf("Assets/");
        if (index + 6 >= name.Length)
            return string.Empty;
        tmp = tmp.Substring(index + 6, name.Length - index - 6);
        tmp = tmp.Replace('\\', '/');
        tmp = tmp.Replace("//", "/");
        return Application.dataPath + tmp;
    }

}
public class OneTexInPut
{
    public static void OnInput(int posx, int posy, int sizex, int sizey, Texture2D mapTexture, Texture2D alphaTexture, List<Texture2D> SpriteList)
    {
        if (sizey <= 0 || sizex <= 0 || mapTexture == null || SpriteList == null || SpriteList.Count <= 0)
            return;
        for (int i = 0; i < SpriteList.Count; ++i)
        {
            Texture2D tmp = SpriteList[i];
            if (tmp == null)
            {
                Debug.LogError("==============");
                continue;
            }
            if (tmp.width <= sizex && tmp.height <= sizey)
            {
                Color[] tmpColors = tmp.GetPixels();
                mapTexture.SetPixels(posx, posy, tmp.width, tmp.height, tmpColors);
                Color[] alphaColors = new Color[tmpColors.Length];
                for (int j = 0; j < tmpColors.Length; ++j)
                {
                    alphaColors[j].r = tmpColors[j].a;
                    alphaColors[j].g = tmpColors[j].a;
                    alphaColors[j].b = tmpColors[j].a;
                    alphaColors[j].a = 1;
                }
                alphaTexture.SetPixels(posx, posy, tmp.width, tmp.height, alphaColors);
                TexturePack.txt += string.Format("\"{0}\":", tmp.name);
                TexturePack.txt += "{";
                TexturePack.txt += ("\"frame\":{" + string.Format("\"x\":{0},\"y\":{1},\"w\":{2},\"h\":{3}", posx, 1024 - posy - tmp.height, tmp.width, tmp.height) + "},");
                TexturePack.txt += string.Format("\"rotated\": false,");
                TexturePack.txt += string.Format("\"trimmed\": false,");
                TexturePack.txt += ("\"spriteSourceSize\":{" + string.Format("\"x\":{0},\"y\":{1},\"w\":{2},\"h\":{3}", 0, 0, tmp.width, tmp.height) + "},");
                TexturePack.txt += ("\"sourceSize\":{" + string.Format("\"w\":{0},\"h\":{1}", tmp.width, tmp.height) + "}");
                TexturePack.txt += "},";
                SpriteList.RemoveAt(i);
                OneTexInPut.OnInput(posx, posy + tmp.height + TexturePack.interval, tmp.width, sizey - tmp.height - TexturePack.interval, mapTexture, alphaTexture, SpriteList);
                OneTexInPut.OnInput(posx + TexturePack.interval + tmp.width, posy, sizex - tmp.width - TexturePack.interval, sizey, mapTexture, alphaTexture, SpriteList);
                return;
            }
        }
        return;
    }
}
public class TextureToAssetbundle
{
    private const string PRE_STRING = "atlas";
    public static void Execute(Object alonePrefabs)
    {
        if (alonePrefabs == null)
            return;

        GameObject go = alonePrefabs as GameObject;
        if (go == null)
            return;
        UIAtlas uiAtlas = go.GetComponent<UIAtlas>();
        if (uiAtlas == null)
            return;
        string path = AssetDatabase.GetAssetPath(alonePrefabs);
        TextureToAssetbundle.BuildAloneAssetbundle(path);
    }

    private static void BuildAloneAssetbundle(string prefabPath)
    {
        Object prefab = AssetDatabase.LoadMainAssetAtPath(prefabPath);
        
        string path = ExportCommand.AssetbundleDetailPath(AssetBundleResType.Atlas) + PRE_STRING + "_" + prefab.name + ".assetbundle";
        bool success = BuildPipeline.BuildAssetBundle(prefab, null, path, ExportCommand.BuildOption, ExportCommand.TargetPlatform);
        if (success)
        {
            UnityEngine.Debug.Log("Export " + path + " Success !");
        }
        else
        {
            UnityEngine.Debug.Log("Export " + path + " Error !");
        }
    }
}
public class AtlastWindowController : EditorWindow
{
    public static AtlastWindowController window;

    [MenuItem("AtlasTool/5.选择图片查找图集名", false, 1)]
    public static void GetNameByChoose()
    {
        Object obj = Selection.activeObject;
        string name = FindAtlas("Assets/AtlasSave/CommonAtlas/", obj.name);
        EditorUtility.DisplayDialog("", name, "OK");
        Debug.LogError(name);
    }

    [MenuItem("AtlasTool/6.查询图片所在图集的位置", false, 1)]
    public static void ExecuteTargetBuild()
    {
        if (window == null)
        {
            window = (AtlastWindowController)GetWindow(typeof(AtlastWindowController));
        }
        window.Show();
    }
    private string input_data = "";

    void OnGUI()
    {
        //CurrentBuildConfig.resAddress = EditorGUILayout.TextField("资源检测服务器IP(包含端口)", CurrentBuildConfig.resAddress, GUILayout.Width(labelWidth + fieldWidth));

        input_data = EditorGUILayout.TextField("图片名:", input_data, GUILayout.Width(500), GUILayout.Height(50));
        if (GUI.Button(new Rect(10f, 80f, 200f, 50f), "查询"))
        {
            string name = FindAtlas("Assets/AtlasSave/CommonAtlas/", input_data);
            EditorUtility.DisplayDialog("", name, "OK");
            Debug.LogError(name);
        }
    }
    private static string FindAtlas(string vAtlasPath,string name)
    {
        List<UIAtlas> atlas = new List<UIAtlas>();
        string dirpath = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + vAtlasPath;
        DirectoryInfo dir = new DirectoryInfo(dirpath);
        FileInfo[] files = dir.GetFiles("*.prefab", SearchOption.AllDirectories);
        foreach (var item in files)
        {
            UIAtlas tatlas = (UIAtlas)Resources.LoadAssetAtPath(GetAssetPathByFileFullPath(item.FullName), typeof(UIAtlas));
            if (tatlas != null)
            {
                atlas.Add(tatlas);
                foreach (var spt in tatlas.spriteList)
                {
                    if (spt.name.Equals(name))
                        return tatlas.name;
                }
            }
        }
        return string.Empty;
    }
    private static string GetAssetPathByFileFullPath(string fullPath)
    {
        int index = fullPath.IndexOf("Assets\\");
        if (index == -1)
        {
            Debug.LogError("error index = -1 :" + fullPath);
            return "";
        }
        string tpath = fullPath.Substring(index, fullPath.Length - index);
        tpath = tpath.Replace('\\', '/');
        return tpath;
    }
}
public class Position
{
    public int x, y;

    public Position(int x,int y)
    {
        this.x = x;
        this.y = y;
    }
}
public class MarkPos
{
    public enum LineType
    {
        top,
        right,
        under,
        left
    }
    public int x = 0;
    public int y = 0;
    public int value = -1;
    public LineType type;
    public int saveIndex;
    List<Position> Portable = new List<Position>();
    public void SetInfo(int x, int y, int index)
    {
        this.x = x;
        this.y = y;
        Portable.Add(new Position(x, y - 1));
        Portable.Add(new Position(x + 1, y - 1));
        Portable.Add(new Position(x + 1, y));
        Portable.Add(new Position(x + 1, y + 1));
        Portable.Add(new Position(x, y + 1));
        Portable.Add(new Position(x - 1, y + 1));
        Portable.Add(new Position(x - 1, y));
        Portable.Add(new Position(x - 1, y - 1));
        this.value = BesiegeFindTool.myArray[y,x];
        for (int i = 0; i < Portable.Count;++i )
        {
            if (!BesiegeFindTool.Filter(Portable[i]))
            {
                Portable[i] = null;
                continue;
            }
            if (BesiegeFindTool.myArray[Portable[i].y, Portable[i].x] != this.value)
            {
                Portable[i] = null;
                continue;
            }
            if(BesiegeFindTool.SaveMark.Count <= 0)
            {
                if (Portable[i].y < this.y)
                    Portable[i] = null;
            }
        }
        int tmpNum = 0;
        if (index >= 4)
            tmpNum = index - 4;
        else
            tmpNum = index + 4;
        if (tmpNum > 7)
            Debug.LogError("the code is wrong");
        this.saveIndex = tmpNum;
        Portable[this.saveIndex] = null;
    }
    public void StartFind()
    {
        int count = 8;
        while(count > 0)
        {
            --count;
            if (this.saveIndex > 7)
                this.saveIndex = 0;

            Position tmp = Portable[saveIndex];
            if (tmp == null)
            {
                ++this.saveIndex;
                continue;
            }
            else
            {

                MarkPos tmpMark = new MarkPos();
                tmpMark.SetInfo(tmp.x, tmp.y,this.saveIndex);
                if (BesiegeFindTool.SaveMark.Count > 0)
                {
                    if (tmpMark.CompareTo(BesiegeFindTool.SaveMark[0]))
                        return;
                }
                Portable[saveIndex] = null;
                ++this.saveIndex;
                if (tmpMark.value != this.value)
                    continue;
                BesiegeFindTool.SaveMark.Add(tmpMark);
                tmpMark.StartFind();
                return;
            }
        }
        if(BesiegeFindTool.SaveMark.Count > 1)
        {
            MarkPos tmpMark = BesiegeFindTool.SaveMark[BesiegeFindTool.SaveMark.Count - 2];
            BesiegeFindTool.SaveMark.RemoveAt(BesiegeFindTool.SaveMark.Count - 1);
            tmpMark.StartFind();
        }
    }
    public bool CompareTo(MarkPos pos)
    {
        return this == pos || (this.x == pos.x && this.y == pos.y);
    }
    public bool CompareTo(Position pos)
    {
        return (this.x == pos.x && this.y == pos.y);
    }
}
public static class BesiegeFindTool
{
    public static int[,] myArray = new int[,] 
    {
    { 1, 2 ,3,4,2,3,5,7,5,4}, 
    { 1, 4 ,4,4,2,3,5,7,5,4}, 
    { 4, 2 ,3,4,4,3,5,7,5,4}, 
    { 4, 2 ,3,4,2,4,5,7,5,4}, 
    { 4, 2 ,3,4,4,3,5,7,5,4}, 
    { 1, 4 ,4,4,2,3,5,5,5,4}, 
    { 1, 2 ,3,4,2,3,5,7,5,4}, 
    { 1, 2 ,3,4,2,3,5,7,5,4}, 
    { 1, 2 ,3,4,2,3,5,7,5,4}, 
    { 1, 2 ,3,4,2,3,5,5,5,4}, 
    };

    public static int maxWidth = 10;
    public static int maxHeight = 10;
    public static List<MarkPos> SaveMark = new List<MarkPos>();

    public static bool Filter(Position pos)
    {
        if (pos == null)
            return false;
        if (pos.x < 0 || pos.x >= maxWidth)
            return false;
        if (pos.y < 0 || pos.y >= maxHeight)
            return false;
        for (int i = 1; i < SaveMark.Count;++i )
        {
            if (SaveMark[i].CompareTo(pos))
                return false;
        }
        if(SaveMark.Count > 0)
        {
            if (pos.y < SaveMark[0].y)
                return false;
        }
        return true;
    }

    public static void StartFindPathAll(int[,] vArray,int width,int height)
    {
        maxHeight = height;
        maxWidth = width;
        for (int i = 0; i < height;++i )
        {
            for (int j = 0; j < width;++j )
            {
                if (SaveMark == null)
                    SaveMark = new List<MarkPos>();
                if(SaveMark.Count > 3)
                {
                    SaveMark.Find((tt) =>
                    {
                        if(tt.y == i)
                        {
                            if (tt.x > j)
                                j = tt.x;
                        }
                        return false;
                    });
                    Debug.LogError("==================");
                    LogResult(SaveMark);
                }
                SaveMark.Clear();
                MarkPos tmpMark = new MarkPos();
                tmpMark.SetInfo(j, i, 2);
                BesiegeFindTool.SaveMark.Add(tmpMark);
                tmpMark.StartFind();
            }
        }        
    }
    public static void LogResult(List<MarkPos> vMark)
    {
        if (vMark == null)
            Debug.LogError("the result is null");
        for (int i = vMark.Count - 1; i >= 0;--i )
        {
            Debug.LogError("X:" + vMark[i].x + "Y:" + vMark[i].y);
        }
    }
}