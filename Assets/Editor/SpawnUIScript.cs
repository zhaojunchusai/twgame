/*****************************************
 * 
 * 功能如下：
 * 
 * 
 * UI产出：包括UI UIControl 产出: 后者继承自 UIBase
 * ItemUI（绑定到元件上的脚本）产出：继承自Monobehavior
 * 
 * 自动屏蔽：需忽略的组件，将名字写入到List NotSpawnComponentNames 中   （Initialize函数）
 * 
 * 自动使用：组件简写命名，将名字写入到Dictionary ComponentShortNames 中    （Initialize函数）
 * 
 * 特殊处理：
 * 1.若需要导出 GameObject 组件，在UI预制上，该GameObject使用 "gobj_" 作为前缀
 * 
 * 2.父级为input， button ，slider时候，该物体名字会自动加入父级名字和类型
 *   例如: button 名为Back 子物体包含Background（uisprite） 和Label(uilabel) 两个子物体，产出结果为  Spt_BtnBackBackground  和 Spt_BtnBacklabel
 * 
 * ***************************************/
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class SpawnUIScript
{
    public struct UIElementData
    {
        public string elementName;
        public string typeName;
        public string path;
        public string parentName;
    }

    public struct LabelItem
    {
        public string name;
        public string value;
    }

    public static List<string> NotSpawnComponentNames = new List<string>();     // 忽略的UI组件//
    public static Dictionary<string, string> ComponentShortNames = new Dictionary<string, string>();    // 组件简写//
    public static List<UIElementData> UIElementDatas = new List<UIElementData>();
    public static List<LabelItem> LabelItems = new List<LabelItem>();
    

    static string UIName = "";

    const string BOM_UTF8 = "\uFEFF";
    //Export UI Prefab to "Assets/__UI/output/*.cs"
    const string targetDir = "/__UI/output/";
    //const string targetDirUI = "/Scripts/UIScripts/{0}/";
    const string targetFileExt = ".cs";
    const string defaultElementNameFormat = "{0}_{1}";


    [MenuItem("Tools/Spawn UI Links And Control")]
    static void SpawnUI()
    {
        Initialize();

        string outputDir = Application.dataPath + targetDir;
        if (!Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);

        if (Selection.activeGameObject == null)
            return;
        GameObject goRoot = Selection.activeGameObject;

        UIName = goRoot.name;
        string outputPath = outputDir + UIName + targetFileExt;
        string outputPathControl = outputDir + UIName + "Controller" + targetFileExt;

        SpawnUILinks(goRoot, outputPath);

        SpawnUIControler(outputPathControl);

        AssetDatabase.Refresh();

        Uninitialize();

    }

    [MenuItem("Tools/Spawn UI Links")]
    static void SpawnUILinks()
    {

        Initialize();

        string outputDir = Application.dataPath + targetDir;
        if (!Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);

        if (Selection.activeGameObject == null)
            return;
        GameObject goRoot = Selection.activeGameObject;

        UIName = goRoot.name;
        string outputPath = outputDir + UIName + targetFileExt;
        //string outputPathControl = outputDir + UIName + "Control" + targetFileExt;

        SpawnUILinks(goRoot, outputPath);

        AssetDatabase.Refresh();

        Uninitialize();

    }

    [MenuItem("Tools/Spawn UI Item Script")]
    static void SpawnUIItem()
    {
        Initialize();
        SpawnItemScript();
        //CreateFolder();
        AssetDatabase.Refresh();
        Uninitialize();

    }

    static void SpawnUILinks(GameObject goRoot, string outputPath)
    {
        string outputText;
        string path = "";
        AddObjPathAndName(goRoot.transform, "");
        StringBuilder sbVar = new StringBuilder();
        sbVar.Append(BOM_UTF8);
        sbVar.Append("using UnityEngine;\r\n");
        sbVar.Append("using System;\r\n");
        sbVar.Append("using System.Collections;\r\n");
        sbVar.Append("\r\n");
        sbVar.Append(string.Format("public class {0}\r\n", UIName));
        sbVar.Append("{\r\n");
        sbVar.Append(string.Format("    public static string UIName =\"{0}\";\r\n", UIName));
        sbVar.Append("    public GameObject _uiRoot;\r\n");
        for (int i = 0; i < UIElementDatas.Count; i++)
        {
            sbVar.Append(string.Format("    public {0} {1};\r\n", UIElementDatas[i].typeName, UIElementDatas[i].elementName));
        }
        sbVar.Append("\r\n");
        sbVar.Append("    public void Initialize()\r\n");
        sbVar.Append("    {\r\n");
        sbVar.Append(string.Format("        _uiRoot =  GameObject.Find(\"UISystem/UICamera/Anchor/Panel/{0}\");\r\n", UIName));

        for (int i = 0; i < UIElementDatas.Count; i++)
        {
            if (UIElementDatas[i].path == UIName)
            {
                sbVar.Append(string.Format("        {0} = _uiRoot.GetComponent<{1}>();\r\n",
                UIElementDatas[i].elementName, UIElementDatas[i].typeName));
            }
            else
            {
                path = UIElementDatas[i].path.Substring(UIElementDatas[i].path.IndexOf(UIName + "/") + UIName.Length + 1);
                if (UIElementDatas[i].typeName == "GameObject")
                {
                    sbVar.Append(string.Format("        {0} = _uiRoot.transform.FindChild(\"{1}\").gameObject;\r\n",
                    UIElementDatas[i].elementName, path));
                }
                else
                {
                    sbVar.Append(string.Format("        {0} = _uiRoot.transform.FindChild(\"{1}\").gameObject.GetComponent<{2}>();\r\n",
                    UIElementDatas[i].elementName, path, UIElementDatas[i].typeName));
                }

            }
        }
        //sbVar.Append("        SetLabelValues();\r\n");

        //for (int i = 0; i < LabelItems.Count; i++)
        //{
        //    sbVar.Append(string.Format("        {0}.text = \"{1}\";\r\n", LabelItems[i].name, LabelItems[i].value));
        //}


        sbVar.Append("    }\r\n");
        sbVar.Append("\r\n");

        List<string> functionContent = new List<string>();
        
        for (int i = 0; i < LabelItems.Count; i++)
        {
            functionContent.Add(string.Format("        {0}.text = \"{1}\";\r\n", LabelItems[i].name, LabelItems[i].value));
        }
        if (functionContent.Count > 0)
        {
            StrBuildFunction(sbVar, "public void SetLabelValues()", functionContent);
        }
        functionContent.Clear();

        StrBuildFunctionOneLine(sbVar, "public void Uninitialize()","");

        StrBuildFunctionOneLine(sbVar, "public void SetVisible(bool isVisible)", "         _uiRoot.SetActive(isVisible);");

        sbVar.Append("}\r\n");
        outputText = sbVar.ToString();
        byte[] outputData = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, Encoding.Unicode.GetBytes(outputText));
        File.WriteAllBytes(outputPath, outputData);
        sbVar.Remove(0, sbVar.Length);
    }

    static void SpawnUIControler(string outputPath)
    {
        string outputText;

        StringBuilder sbVar = new StringBuilder();
        sbVar.Append(BOM_UTF8);
        sbVar.Append("using UnityEngine;\r\n");
        sbVar.Append("using System;\r\n");
        sbVar.Append("using System.Collections;\r\n");
        sbVar.Append("\r\n");
        sbVar.Append(string.Format("public class {0} : UIBase \r\n", string.Format("{0}Controller", UIName)));
        sbVar.Append("{\r\n");
        sbVar.Append(string.Format("    public {0} view;\r\n", UIName));
        sbVar.Append("\r\n");
        sbVar.Append("    public override void Initialize()\r\n");
        sbVar.Append("    {\r\n");
        sbVar.Append("        if (view == null)\r\n");
        sbVar.Append(string.Format("            view = new {0}();\r\n", UIName));
        sbVar.Append("        view.Initialize();\r\n");
        
        List<string> btnEvents = new List<string>();
        List<string> btnlistens = new List<string>();
        for (int i = 0; i < UIElementDatas.Count; i++)
        {
            if (UIElementDatas[i].typeName == "UIButton")
            {
                string name = UIElementDatas[i].elementName;
                string functionName = string.Format("ButtonEvent_{0}", name.Substring(name.IndexOf("_") + 1));

                string btnfunctionDesc = string.Format("public void {0}(GameObject btn)", functionName);
                string btnlistener = string.Format("        UIEventListener.Get(view.{0}.gameObject).onClick = {1};\r\n",
                                                   name,
                                                   functionName);

                //sbVar.Append(string.Format("        UIEventListener.Get(_ui.{0}.gameObject).onClick = {1};\r\n",
                //    name,
                //   functionName));

                btnEvents.Add(btnfunctionDesc);
                btnlistens.Add(btnlistener);
            }

        }
        
        sbVar.Append("        BtnEventBinding();\r\n");
        sbVar.Append("    }\r\n");
        sbVar.Append("\r\n");

        for (int i = 0; i < btnEvents.Count; i++)
        {
            StrBuildFunction(sbVar, btnEvents[i], null, 1);
        }
        
        StrBuildFunctionOneLine(sbVar, "public override void Uninitialize()", "");
        
        StrBuildFunction(sbVar, "public void BtnEventBinding()", btnlistens, 1);
        sbVar.Append("\r\n");
        sbVar.Append("}\r\n");
        outputText = sbVar.ToString();
        byte[] outputData = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, Encoding.Unicode.GetBytes(outputText));
        File.WriteAllBytes(outputPath, outputData);
        sbVar.Remove(0, sbVar.Length);
    }

    static void SpawnItemScript()
    {
        string outputDir = Application.dataPath + targetDir;
        if (!Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);

        if (Selection.activeGameObject == null)
            return;
        GameObject goRoot = Selection.activeGameObject;

        UIName = goRoot.name;
        string outputPath = outputDir + UIName + targetFileExt;
        string outputText;
        string path = "";
        AddObjPathAndName(goRoot.transform, "");
        StringBuilder sbVar = new StringBuilder();
        sbVar.Append(BOM_UTF8);
        sbVar.Append("using UnityEngine;\r\n");
        sbVar.Append("using System;\r\n");
        sbVar.Append("using System.Collections;\r\n");
        sbVar.Append("\r\n");
        sbVar.Append(string.Format("public class {0} : MonoBehaviour\r\n", UIName));
        sbVar.Append("{\r\n");
        for (int i = 0; i < UIElementDatas.Count; i++)
        {
            sbVar.Append(string.Format("    [HideInInspector]public {0} {1};\r\n", UIElementDatas[i].typeName, UIElementDatas[i].elementName));
        }
        sbVar.Append("\r\n");
        sbVar.Append("    public void Initialize()\r\n");
        sbVar.Append("    {\r\n");

        for (int i = 0; i < UIElementDatas.Count; i++)
        {
            if (UIElementDatas[i].path == UIName)
            {
                sbVar.Append(string.Format("        {0} = GetComponent<{1}>();\r\n",
                UIElementDatas[i].elementName, UIElementDatas[i].typeName));
            }
            else
            {
                path = UIElementDatas[i].path.Substring(UIElementDatas[i].path.IndexOf(UIName + "/") + UIName.Length + 1);
                if (UIElementDatas[i].typeName == "GameObject")
                {
                    sbVar.Append(string.Format("        {0} = transform.FindChild(\"{1}\").gameObject;\r\n",
                    UIElementDatas[i].elementName, path));
                }
                else
                {
                    sbVar.Append(string.Format("        {0} = transform.FindChild(\"{1}\").gameObject.GetComponent<{2}>();\r\n",
                    UIElementDatas[i].elementName, path, UIElementDatas[i].typeName));
                }

            }
        }
        //sbVar.Append("        SetLabelValues();\r\n");
        sbVar.Append("\r\n");

        List<string> btnEvents = new List<string>();
        for (int i = 0; i < UIElementDatas.Count; i++)
        {
            if (UIElementDatas[i].typeName == "UIButton")
            {
                string name = UIElementDatas[i].elementName;
                string functionName = string.Format("ButtonEvent_{0}", name.Substring(name.IndexOf("_") + 1));
                string functionDesc = string.Format("public void {0}(GameObject btn)", functionName);
                sbVar.Append(string.Format("        UIEventListener.Get({0}.gameObject).onClick = {1};\r\n",
                    name,
                   functionName));

                btnEvents.Add(functionDesc);
            }
        }

        sbVar.Append("    }\r\n");
        sbVar.Append("\r\n");
        StrBuildFunctionOneLine(sbVar, "void Awake()", "        Initialize(); ", 1);

        List<string> functionContent = new List<string>();

        for (int i = 0; i < LabelItems.Count; i++)
        {
            functionContent.Add(string.Format("        {0}.text = \"{1}\";\r\n", LabelItems[i].name, LabelItems[i].value));
        }
        if (functionContent.Count > 0)
        {
            StrBuildFunction(sbVar, "public void SetLabelValues()", functionContent);
        }
        functionContent.Clear();
        
        for (int i = 0; i < btnEvents.Count; i++)
        {
            StrBuildFunction(sbVar, btnEvents[i], null, 1);
        }
        StrBuildFunction(sbVar, "public void Uninitialize()", null, 1);

        sbVar.Append("\r\n");
        sbVar.Append("}\r\n");
        outputText = sbVar.ToString();
        byte[] outputData = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, Encoding.Unicode.GetBytes(outputText));
        File.WriteAllBytes(outputPath, outputData);
        sbVar.Remove(0, sbVar.Length);
    }

    static bool AddObjPathAndName(Transform vTransform, string vParentPath)
    {
        string tPath = "";
        if (string.IsNullOrEmpty(vParentPath))
        {
            tPath = vTransform.gameObject.name;
        }
        else
        {
            tPath = string.Format("{0}/{1}", vParentPath, vTransform.gameObject.name);
        }
        MonoBehaviour[] components = vTransform.GetComponents<MonoBehaviour>();

        // 若需要提取该物体的Gameobject组件//
        CheckGetGameobjectComponent(vTransform, tPath);

        MonoBehaviour component;
        for (int i = 0; i < components.Length; i++)
        {
            component = components[i];
            string typeName = component.GetType().Name;
            //判断该组件是否跳过//
            if (NotSpawnComponentNames.Contains(typeName))
            {
                continue;
            }
            string shortTypeName = "";
            UIElementData data = new UIElementData();
            string elementNameFormat = defaultElementNameFormat;

            //判断父级是否是按钮//
            if (vTransform.parent != null && vTransform.parent.GetComponent<UIButton>() != null)
            {
                data.parentName = vTransform.parent.name;
                elementNameFormat = "{0}_Btn" + data.parentName + "{1}";
            }
            //判断父级是否是输入框//
            if (vTransform.parent != null && vTransform.parent.GetComponent<UIInput>() != null)
            {
                data.parentName = vTransform.parent.name;
                elementNameFormat = "{0}_Input" + data.parentName + "{1}";
            }
            //判断父级是否是进度条//
            if (vTransform.parent != null && vTransform.parent.GetComponent<UISlider>() != null)
            {
                data.parentName = vTransform.parent.name;
                elementNameFormat = "{0}_Slider" + data.parentName + "{1}";
            }

            //判断该组件是否有简称//
            if (ComponentShortNames.TryGetValue(typeName, out shortTypeName))
            {
                data.elementName = string.Format(elementNameFormat, shortTypeName, component.name);
            }
            else
            {
                data.elementName = string.Format(elementNameFormat, typeName, component.name);
            }
            data.typeName = typeName;
            data.path = tPath;
            UIElementDatas.Add(data);

            if (data.typeName == "UILabel")
            {
                LabelItem item = new LabelItem();
                item.name = data.elementName;
                item.value = component.GetComponent<UILabel>().text;
                LabelItems.Add(item);
            }
        }
        if (vTransform.childCount == 0 || vTransform.gameObject.name.Contains("ignore"))
            return true;
        for (int i = 0; i < vTransform.childCount; )
        {
            if (AddObjPathAndName(vTransform.GetChild(i), tPath))
            {
                i++;
                if (string.IsNullOrEmpty(vParentPath))
                    tPath = vTransform.gameObject.name;
                else
                    tPath = string.Format("{0}/{1}", vParentPath, vTransform.gameObject.name);
            }
        }
        return true;
    }

    static void CheckGetGameobjectComponent(Transform trans, string path)
    {
        string prefix = "gobj_";

        if (trans.name.Length > prefix.Length && trans.name.Substring(0, prefix.Length).Equals(prefix))
        {
            string shortTypeName = "";
            UIElementData data = new UIElementData();
            string typeName = "GameObject";
            string name = trans.name.Substring(prefix.Length);

            if (ComponentShortNames.TryGetValue(typeName, out shortTypeName))
            {
                data.elementName = string.Format(defaultElementNameFormat, shortTypeName, name);
            }
            else
            {
                data.elementName = string.Format(defaultElementNameFormat, typeName, name);
            }
            data.typeName = typeName;
            data.path = path;
            UIElementDatas.Add(data);
        }
    }

    static void CreateFolder()
    {
        string dir = string.Format(Application.dataPath + "/Scripts/UIScripts/{0}/", UIName);

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }

    static string onetabstr = "    ";

#region build string function
    /// <summary>
    /// 写一个函数的string
    /// </summary>
    /// <param name="sbVar"></param>
    /// <param name="functionNameLine">eg: "public void Function()"</param>
    /// <param name="contentLines"> </param>
    /// <param name="tabCount"> 有几个缩进，默认为1</param>
    static void StrBuildFunction(StringBuilder sbVar, string functionNameLine, List<string> contentLines, int tabCount = 1)
    {
        string tabstr = "";

        for (int i = 0; i < tabCount; i++)
        {
            tabstr += onetabstr;
        }

        sbVar.Append(string.Format("{0}{1}\r\n", tabstr, functionNameLine));
        sbVar.Append(tabstr + "{\r\n");
        if (contentLines != null && contentLines.Count > 0)
        {
            StrBuildLine(sbVar, contentLines);
        }
        else
        {
            sbVar.Append("\r\n");
        }
        sbVar.Append(tabstr + "}\r\n");
        sbVar.Append("\r\n");

    }

    static void StrBuildFunctionOneLine(StringBuilder sbVar, string functionNameLine, string content, int tabCount = 1)
    {
        string tabstr = "";

        for (int i = 0; i < tabCount; i++)
        {
            tabstr += onetabstr;
        }

        sbVar.Append(string.Format("{0}{1}\r\n", tabstr, functionNameLine));
        sbVar.Append(tabstr + "{\r\n");
        sbVar.Append(content);
        sbVar.Append("\r\n");
        sbVar.Append(tabstr + "}\r\n");
        sbVar.Append("\r\n");
    }

    static void StrBuildLine(StringBuilder sbVar, List<string> contentLines)
    {
        if (contentLines != null && contentLines.Count > 0)
        {
            for (int i = 0; i < contentLines.Count; i++)
            {
                sbVar.Append(contentLines[i]);
            }
        }

    }
#endregion

    static void Initialize()
    {
        UIElementDatas.Clear();
        ComponentShortNames.Clear();
        NotSpawnComponentNames.Clear();
        LabelItems.Clear();

        // 以下数据均用在函数 AddObjPathAndName  中进行判断//
        //NotSpawnComponentNames.Add("UIPanel");
        //NotSpawnComponentNames.Add("UISprite");
        NotSpawnComponentNames.Add("UIButtonScale");
        NotSpawnComponentNames.Add("UIButtonOffset");
        NotSpawnComponentNames.Add("UIButtonRotation");
        NotSpawnComponentNames.Add("UIButtonColor");
        NotSpawnComponentNames.Add("UIPlaySound");
        NotSpawnComponentNames.Add("UIStretch");
        NotSpawnComponentNames.Add("UICamera");
        NotSpawnComponentNames.Add("UIAnchor");
        NotSpawnComponentNames.Add("UIDragScrollView");
        NotSpawnComponentNames.Add("UIScrollBar");
       // NotSpawnComponentNames.Add("UIToggle");
        NotSpawnComponentNames.Add("UIToggleChangeColor");
        NotSpawnComponentNames.Add("UIToggleChangeFontSize");
        NotSpawnComponentNames.Add("UIToggleChangeScale");
        NotSpawnComponentNames.Add("ShowUIAniAlpha");
        NotSpawnComponentNames.Add("ShowUIAniPosition");
        NotSpawnComponentNames.Add("ShowUIAniScale");
        NotSpawnComponentNames.Add("TweenScale");
        NotSpawnComponentNames.Add("TweenTransform");
        NotSpawnComponentNames.Add("TweenPosition");
        NotSpawnComponentNames.Add("TweenRotation");

        ComponentShortNames.Add("UILabel", "Lbl");
        ComponentShortNames.Add("UIButton", "Btn");
        ComponentShortNames.Add("UISprite", "Spt");
        ComponentShortNames.Add("UIGrid", "Grd");
        ComponentShortNames.Add("UIInput", "Ipt");
        ComponentShortNames.Add("UITexture", "Tex");
        ComponentShortNames.Add("UISlider", "Slider");
        ComponentShortNames.Add("GameObject", "Gobj");
        ComponentShortNames.Add("UIToggle", "Tog");
        //ComponentShortNames.Add("UIPanel", "Pn");
        ComponentShortNames.Add("UIScrollView", "ScrView");

    }

    static void Uninitialize()
    {
        UIElementDatas.Clear();
        ComponentShortNames.Clear();
        NotSpawnComponentNames.Clear();
        LabelItems.Clear();

        Debug.Log("UI 元件腳本已建立完成 ");
    }
}
