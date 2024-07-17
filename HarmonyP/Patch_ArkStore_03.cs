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
    /// Contains members for altering item behavior
    /// </summary>
    [HarmonyPatch]
    public static partial class Patch_ArkStore
    {
        public static bool NoRareLimit = false;
        public static bool UpgradeArtifactPouch = false;
        public static int ArtifactUpgrades = 3;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UseItem.SkillBookCharacter_Rare), nameof(UseItem.SkillBookCharacter_Rare.Use))]
        public static bool SkillBookCharacter_Rare_Use_Prefix(ref bool __result)
        {
            if (!NoRareLimit) return true;

            List<Skill> list = new List<Skill>();
            List<BattleAlly> battleallys = PlayData.Battleallys;
            BattleTeam tempBattleTeam = PlayData.TempBattleTeam;

            for (int i = 0; i < PlayData.TSavedata.Party.Count; i++)
            {
                List<GDESkillData> gDESkillData =
                    PlayData.GetMySkills(PlayData.TSavedata.Party[i].KeyData, Rare: true);
                foreach (var skill in gDESkillData)
                {
                    list.Add(Skill.TempSkill(skill.KeyID, battleallys[i], tempBattleTeam));
                }
            }

            foreach (Skill item in list)
            {
                if (!SaveManager.IsUnlock(item.MySkill.KeyID, SaveManager.NowData.unlockList.SkillPreView))
                {
                    SaveManager.NowData.unlockList.SkillPreView.Add(item.MySkill.KeyID);
                }
            }

            PlayData.TSavedata.UseItemKeys.Add(GDEItemKeys.Item_Consume_SkillBookCharacter_Rare);
            MasterAudio.PlaySound("BookFlip");
            FieldSystem.DelayInput(BattleSystem.I_OtherSkillSelect(list, SkillAdd, ScriptLocalization.System_Item.SkillAdd, back: false, ManaView: true, HideButtonView: true, ShowMySkillBtn: true));
            __result = true;
            return false;
        }
        public static void SkillAdd(SkillButton Mybutton)
        {
            Mybutton.Myskill.Master.Info.UseSoulStone(Mybutton.Myskill);
            UIManager.inst.CharstatUI.GetComponent<CharStatV4>().SkillUPdate();
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(UseItem.ArtifactPouch), nameof(UseItem.ArtifactPouch.Use))]
        public static bool ArtifactPouch_Use_Prefix(ref bool __result)
        {
            if (UpgradeArtifactPouch)
            {
                GetRelics();
                __result = true;
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void GetRelics()
        {
            List<ItemBase> list = new List<ItemBase>();

            for (int i = 0; i < ArtifactUpgrades; i++)
            {
                list.Add(PlayData.GetPassiveRandom(false, list.Select(ib => ib.itemkey).ToList(), false, false));
            }

            UIManager.InstantiateActive(UIManager.inst.SelectItemUI).GetComponent<SelectItemUI>()
                .Init(list);
        }
    }
}
