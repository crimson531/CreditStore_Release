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


namespace CreditStore_Release
{
    /// <summary>
    /// Contains members for loading purchased items into player inventory and cleaning up purchased
    /// data
    /// </summary>
    [HarmonyPatch]
    public static partial class Patch_ArkStore
    {
        public static bool AddEquipSlots = false;
        public static int EquipSlotsToAdd = 0;
        public static bool DataLoaded = false;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FieldSystem), nameof(FieldSystem.StageStart))]
        public static void StageStart_Postfix(string StageKey = "")
        {
            if (StageKey == "") return;
            else if (PlayData.TSavedata.StageNum == 0)
            {
                AddStartItems();
                SaveMyData();
                DataLoaded = true;
            }
            else if (PlayData.TSavedata.StageNum > 0 &&
                !DataLoaded)
            {
                LoadMyData();
                DataLoaded = true;
            }
            // Reset some values to prevent carry over into next run
            //if (PlayData.TSavedata.Party.Count >= 4 ||
            //    StageKey == GDEItemKeys.Stage_Stage4_Camp)
            //{
            //    AddEquipSlots = false;
            //    EquipSlotsToAdd = 0;
            //}
        }
        static void AddStartItems()
        {
            foreach (var key in PurchasedDataSpecial)
            {
                switch (key)
                {
                    case SpecialKeys.InventorySlot:
                        for (int i = 0; i < PurchasedAmount[key]; i++)
                        {
                            PlayData.TSavedata.Inventory.Add(null);
                            PlayData.TSavedata.MaxinventoryNumPlus += 1;
                            PlayData.MaxInventory += 1;
                        }
                        break;
                    case SpecialKeys.NoRareLimit:
                        NoRareLimit = true;
                        break;
                    case SpecialKeys.UpgradeArtifactPouch:
                        UpgradeArtifactPouch = true;
                        ArtifactUpgrades += PurchasedAmount[key];
                        break;
                    case SpecialKeys.RelicSlot:
                        for (int i = 0; i < PurchasedAmount[key]; i++)
                        {
                            PlayData.TSavedata.ArkPassivePlus += 1;
                            PlayData.TSavedata.Passive_Itembase.Add(null);
                        }
                        break;
                }
            }
            foreach (var kvp in PurchasedData)
            {
                var key = kvp.Key;
                var amt = PurchasedAmount[key];
                ItemBase item;

                switch (key)
                {
                    case "Gold":
                        PlayData.Gold += amt;
                        break;
                    case "Soul":
                        PlayData.Soul += amt;
                        break;
                    default:
                        if (MaxStacks.ContainsKey(key))
                        {
                            var max = MaxStacks[key];
                            var stacksNeed = (int)Math.Ceiling((double)amt / (double)max);
                            var amtLeft = amt;
                            for (int i = 0; i < stacksNeed; i++)
                            {
                                if (amtLeft > 0)
                                {
                                    if (amtLeft >= max)
                                    {
                                        item = ItemBase.GetItem(key, max);
                                        amtLeft -= max;
                                    }
                                    else
                                    {
                                        item = ItemBase.GetItem(key, amtLeft);
                                        amtLeft -= amtLeft;
                                    }
                                    // Identifies scroll items purchased in store
                                    if ((item as Item_Scroll) != null)
                                    {
                                        PlayData.TSavedata.IdentifyItems.Add(key);
                                    }
                                    PartyInventory.InvenM.AddNewItem(item);
                                }
                            }
                        }
                        else
                        {
                            item = ItemBase.GetItem(key, amt);
                            PartyInventory.InvenM.AddNewItem(item);
                        }
                        break;
                }
            }

            PurchasedData.Clear();
            PurchasedDataSpecial.Clear();
            PurchasedAmount.Clear();

            GamepadManager.Remove(PartyInventory.Ins.InvenLayout);
            PartyInventory.Ins.InvenLayout = null;
            PartyInventory.Ins.PadLayoutSetting();
            PartyInventory.Ins.UpdateInvenUI();

        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FieldSystem), nameof(FieldSystem.PartyAdd))]
        [HarmonyPatch(new Type[] { typeof(GDECharacterData), typeof(int) })]
        public static void PartyAdd_Postfix()
        {
            InitAddEquipSlots();

            if (AddEquipSlots)
            {
                foreach (var member in PlayData.TSavedata.Party)
                {
                    if (member.Equip.Count < 2 + EquipSlotsToAdd)
                    {
                        var toAdd = (2 + EquipSlotsToAdd) - member.Equip.Count;

                        for (int i = 0; i < toAdd; i++)
                        {
                            member.Equip.Add(null);
                        }
                    }
                }

                if (FieldSystem.instance != null)
                    FieldSystem.instance.PartyWindowInit();
            }
        }
        private static void InitAddEquipSlots()
        {
            if (PurchasedDataSpecial.Contains(SpecialKeys.EquipSlot))
            {
                AddEquipSlots = true;
                EquipSlotsToAdd = PurchasedAmount[SpecialKeys.EquipSlot];
            }
        }
        public static void ResetArkStore()
        {
            AddEquipSlots = false;
            EquipSlotsToAdd = 0;
            NoRareLimit = false;
            UpgradeArtifactPouch = false;
            ArtifactUpgrades = 3;
            DataLoaded = false;
        }
    }
}
