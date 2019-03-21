using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Serialization;

 public class PXTools
 {
    

     /// <summary>
     /// 通过物体来找到脚本，查找自身和其子类的
     /// </summary>
     /// <typeparam name="T"></typeparam>
     /// <param name="_go"></param>
     /// <returns></returns>
     public static T FindScriptChildren<T>(GameObject _go) where T : Component
     {
         foreach (T element in _go.GetComponents<T>())
         {
             return element;
         }

         foreach (T element in _go.GetComponentsInChildren<T>(true))
         {
             return element;
         }
         return null;
     }

     public static Component FindScriptInChild(GameObject _go, Type type, string elementName)
     {
         Component[] tempcom = _go.GetComponentsInChildren(type, true);
         foreach (Component element in tempcom)
         {
             if (element.gameObject.name == elementName)
             {
                 return element;
             }
         }
         return null;
     }

     /// <summary>
     /// 对象序列化成 XML String
     /// </summary>
     public static string XmlSerialize<T>(T obj)
     {
         string xmlString = string.Empty;
         XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
         using (MemoryStream ms = new MemoryStream())
         {
             xmlSerializer.Serialize(ms, obj);
             xmlString = Encoding.UTF8.GetString(ms.ToArray());
         }
         return xmlString;
     }

     /// <summary>
     ///对象序列化成 XML String
     /// </summary>
     public static T XmlDeserialize<T>(string xmlString)
     {
         T t = default(T);
         XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
         using (Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
         {
             using (XmlReader xmlReader = XmlReader.Create(xmlStream))
             {
                 System.Object obj = xmlSerializer.Deserialize(xmlReader);
                 t = (T)obj;
             }
         }
         return t;
     }


     public static string NumberToChar(uint num) 
     {
         switch (num) 
         {
             case 0: return "〇";
             case 1: return "一";
             case 2: return "二";
             case 3: return "三";
             case 4: return "四";
             case 5: return "五";
             case 6: return "六";
             case 7: return "七";
             case 8: return "八";
             case 9: return "九";
             case 10: return "十";
             default: return string.Empty;
         }
     }
 }

