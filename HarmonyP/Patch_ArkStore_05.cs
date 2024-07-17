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
using DG.Tweening;
using System.Runtime.CompilerServices;

namespace CreditStore_Release
{
    /// <summary>
    /// Contains members for moving the skill window during battles. Helps when
    /// inventory surpasses 2 rows.
    /// </summary>
    [HarmonyPatch]
    public static partial class Patch_ArkStore
    {
        private static bool flag = false;
        private static bool flag3 = false;
        private static bool flag4 = false;
        private static int _shift = 0;
        [HarmonyPostfix]
        [HarmonyPatch(typeof(BattleSystem), nameof(BattleSystem.Start))]
        public static void BattleSystem_Start_Postfix()
        {
            flag = false;
            flag3 = false;
            flag4 = false;
            _shift = 0;
            
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(BattleSystem), "Update")]
        public static void BattleSystem_BattleInit_Postfix()
        {
            AdjustUIElements();
        }
        static void AdjustUIElements()
        {
            if (flag4 && flag  && flag3) return;
            try
            {
                GameObject ui = ue.GameObject.Find("UI");
                if (ui != null)
                {
                    MoveLeftWindow(ui);
                    MoveDeck(ui);
                    if (flag  && flag3)
                        flag4 = true;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        static void MoveDeck(GameObject go)
        {
            if (!flag) return;
            if (flag3) return;
            RectTransform rect = go.GetComponentsInChildren<RectTransform>().
                Where(rt => rt.name == "Deck" && rt.parent?.name == "UI").FirstOrDefault();
            if (rect != null && _shift > 0)
            {
                rect.anchoredPosition = new Vector2(
                    rect.anchoredPosition.x,
                    rect.anchoredPosition.y - _shift);
                flag3 = true;
            }
        }
        static void MoveLeftWindow(GameObject go)
        {
            if (flag) return;

            RectTransform rect = go.GetComponentsInChildren<RectTransform>().
                Where(rt => rt.name == "LeftWindow").FirstOrDefault();
            if (rect != null)
            {
                var itemCnt = PartyInventory.InvenM.InventoryItems.Count;
                var rows = 2;
                if (PartyInventory.Ins.InvenLayout.ColumnNum > 0)
                {
                    rows = (int)Math.Ceiling((double)itemCnt / (double)PartyInventory.Ins.InvenLayout.ColumnNum);
                }
                var shift = (rows * 26) + 5;
                if (shift > 5)
                {
                    rect.anchoredPosition = new Vector2(
                        rect.anchoredPosition.x,
                        rect.anchoredPosition.y - shift);
                    _shift = shift;
                    flag = true;
                }
            }
        }
    }
}
