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
using Steamworks;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CreditStore_Release
{

    public class CreditStore : ChronoArkPlugin
    {
        public const string HarmonyGuid = "org.seer531.ChronoArk.CreditStore_R";
        public const string modname = "CreditStore_Release";
        public const string version = "0.0.1";
        public const string author = "seer531";
        private static readonly Harmony myHarmony;

        static CreditStore()
        {
            // Harmony indicating bugs. Creates a file to desktop according to documentation
            Harmony.DEBUG = true;

            try
            {
                // Instantiating a new harmony object
                myHarmony = new Harmony(HarmonyGuid);
            }
            catch (Exception e)
            {
                Debug.Log("Credit Store Static Constructor: Exception\r\n\r\n" + e.ToString());
            }
        }
        public override void Dispose()
        {
            Harmony.DEBUG = false;

            try
            {
                myHarmony.UnpatchAll(HarmonyGuid);
            }
            catch (Exception e)
            {
                LogFile.LogTextDefault($"{ModExt.GetCurrentMethodName(this)} " + e.ToString());
            }
        }
        public override void Initialize()
        {
            // Patching
            try
            {
                HarmonyPatchAll();

                
            }
            catch (Exception e)
            {
                Debug.Log($"{ModExt.GetCurrentMethodName(this)} - \r\n" + e.ToString());
            }

        }
        private static void HarmonyPatchAll()
        {
            // Told to patch within executing assembly only. Helps prevent multiple instances
            // of harmony from interfering with each other.
            myHarmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}