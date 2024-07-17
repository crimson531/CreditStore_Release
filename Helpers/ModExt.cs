using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GameDataEditor;
using I2.Loc;
using DarkTonic.MasterAudio;
using ChronoArkMod;
using ChronoArkMod.Plugin;
using ChronoArkMod.Template;
using Debug = UnityEngine.Debug;
using ChronoArkMod.ModData;
using HarmonyLib;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CreditStore_Release
{
    /// <summary>
    /// Used for extending
    /// </summary>
    public static class ModExt
    {
        public static List<string> SearchExceptions = new List<string>();
        static ModExt()
        {
            SearchExceptions.Add("EquipSkill");
        }
        public static string GetCurrentMethodName<T>(this T me) where T : class
        {
            if (me == null) throw new ArgumentNullException("Cannot get method name from a null object.");

            StackTrace trace = new StackTrace();
            StackFrame frame = trace.GetFrame(1);
            if (frame != null)
            {
                MethodBase method = frame.GetMethod();
                return $"{method.DeclaringType.Namespace}.{method.DeclaringType.Name}.{method.Name}";
            }
            else
            {
                return "Get current method name could not find method name with in stack trace.";
            }
        }
        public static string AppendText(this string text, string append)
        {
            return string.Concat(text, append);
        }
        public static string InsertText(this string text, string insert, int replicate = 0)
        {
            return text.Insert(0, insert.Replicate(replicate));
        }
        public static string Replicate(this string text, int times)
        {
            if (times == 0) return text;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < times; i++)
            {
                sb.Append(text);
            }
            return sb.ToString();
        }
        public static string IGDEDataToString<T>(this T me, int level = 0) where T : IGDEData
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("IGDE Data to String".InsertText("\t", level));
            sb.AppendLine($"IGDE Type - {me.GetType().Name}".InsertText("\t", level));
            List<string> readfn = new List<string>();
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields.Where(fld => ExceptionCheck(fld.Name)))
            {
                if (field != null)
                {
                    string fName = field.Name;
                    object value = field.GetValue(me);
                    if (typeof(IGDEData).IsAssignableFrom(field.FieldType))
                    {
                        //sb.AppendLine((Convert.ChangeType(value, field.FieldType)).IGDEDataToString(level + 2));
                        if (IGDEDict.IGDETypeToStringDel.ContainsKey(field.FieldType))
                        {
                            var temp = IGDEDict.IGDETypeToStringDel[field.FieldType](value, level + 2);
                            sb.AppendLine(temp);
                        }
                    }
                    else
                    {
                        sb.AppendLine($"Field Name - {fName}".InsertText("\t", level + 1));
                        sb.AppendLine($"My Value - {value}".InsertText("\t", level + 1));
                    }
                }
            }

            return sb.ToString();
        }
        private static bool ExceptionCheck(string value)
        {
            if (value.StartsWith("_")) return true;
            if (string.Compare(value, "EquipSkill", true) == 0) return true;
            return false;
        }
        public static IEnumerable<Type> GetAllGDETypes(IEnumerable<Type> types)
        {
            List<Type> res = new List<Type>();

            foreach (Type type in types)
            {
                if (typeof(IGDEData).IsAssignableFrom(type))
                {
                    res.Add(type);
                }

                var nestTypes = type.GetNestedTypes();
                if (nestTypes.Length > 0)
                {
                    var nests = GetAllGDETypes(nestTypes);
                    if (nests.Count() > 0) res.AddRange(nests);
                }
            }

            return res;
        }
        public static List<Type> GetTypesSafe(this Assembly me)
        {
            try
            {
                return me.GetTypes().ToList();
            }
            catch (ReflectionTypeLoadException e) 
            {
                return e.Types.Where(t => t != null).ToList();
            }
        }
        public static void SetPrivateField(this object obj, string field, object value)
        {
            FieldInfo fi = obj.GetType().GetField(field, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi != null)
            {
                fi.SetValue(obj, value);
            }
            else
            {
                throw new NullReferenceException($"Field info {field} could not be returned for type {obj.GetType().Name}");
            }
        }
        public static string GetSaveManagerSavePath()
        {
            Debug.Log("Getting save file path");
            FieldInfo field = typeof(SaveManager).GetField("DataPath_Steam_XML", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                var filePath = Convert.ToString(field.GetValue(SaveManager.savemanager));
                Debug.Log("Found file path - " + filePath);
                return filePath;
            }
            else
            {
                Debug.Log("Unable to retrieve save file path");
                return string.Empty;
            }
        }
        public static string GetIGDEStoreObjectName(this IGDEData data)
        {
            foreach (Type dataType in IGDEDict.StoreItemTypes)
            {
                try
                {
                    FieldInfo[] fields = dataType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                    foreach (FieldInfo field in fields)
                    {
                        if (field != null && 
                            Convert.ChangeType(data, field.FieldType) != null &&
                            string.Compare(field.Name, "_name", true) > 0)
                        {
                            return Convert.ToString(field.GetValue(data));
                        }
                    }
                }
                catch { }
            }
            return string.Empty;
        }
        public static object ToDefaultIfNull(this IEnumerable<object> values, object defaultIfNull) 
        {
            if (!values.Any()) return defaultIfNull;
            return values.First();
        }
    }
}
