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
using ue = UnityEngine;
using TMPro;
using UseItem;


namespace CreditStore_Release
{
    /// <summary>
    /// Contains members for saving custom data to file and loading data
    /// Save triggers on stage start when stage num is equal to zero. 
    /// Load triggers on stage start when stage num is greater than zero.
    /// Boolean flag to run save and load only once during those calls
    /// </summary>
    [HarmonyPatch]
    public static partial class Patch_ArkStore
    {
        public static string GetSavePath => ModExt.GetSaveManagerSavePath();
        public static string GetFileName => Path.GetFileName(GetSavePath);
        public static string GetFileNameNoExt => Path.GetFileNameWithoutExtension(GetFileName);
        public static string GetDir => Path.GetDirectoryName(GetSavePath);
        public static string MyFileName => $"{GetFileNameNoExt}_ArkStore.txt";
        public static string MyFilePath => Path.Combine(GetDir, MyFileName);
        public static List<string> DataFieldsToSave
        {
            get
            {
                List<string> data = new List<string>();

                data.Add($"{nameof(AddEquipSlots)};{AddEquipSlots.GetType().AssemblyQualifiedName};{AddEquipSlots}");
                data.Add($"{nameof(EquipSlotsToAdd)};{EquipSlotsToAdd.GetType().AssemblyQualifiedName};{EquipSlotsToAdd}");
                data.Add($"{nameof(NoRareLimit)};{NoRareLimit.GetType().AssemblyQualifiedName};{NoRareLimit}");
                data.Add($"{nameof(UpgradeArtifactPouch)};{UpgradeArtifactPouch.GetType().AssemblyQualifiedName};{UpgradeArtifactPouch}");
                data.Add($"{nameof(ArtifactUpgrades)};{ArtifactUpgrades.GetType().AssemblyQualifiedName};{ArtifactUpgrades}");

                return data;
            }
        }
        public static void SaveMyData()
        {
            StringBuilder data = new StringBuilder();

            DataFieldsToSave.ForEach(line => data.AppendLine(line));

            File.WriteAllText(MyFilePath, data.ToString());
        }
        public static void DataToLoad(string data)
        {
            var lines = data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var parts = line.Split(new char[] { ';' });
                var field = parts[0];
                var type = parts[1];
                var value = parts[2];

                FieldInfo fi = typeof(Patch_ArkStore).GetField(field, BindingFlags.Static | BindingFlags.Public);
                if (fi != null)
                {
                    fi.SetValue(null, Convert.ChangeType(value, Type.GetType(type)));
                }
            }
        }
        public static void LoadMyData()
        {
            if (File.Exists(MyFilePath))
            {
                DataToLoad(File.ReadAllText(MyFilePath));
            }
        }
        public static void DeleteMyData()
        {
            if (File.Exists(MyFilePath)) { File.Delete(MyFilePath); }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ArkCode), nameof(ArkCode.LucyRoomStart))]
        public static void ArkCode_LucyRoomStart_Postfix()
        {
            DeleteMyData();
            ResetArkStore();
        }
    }
}
