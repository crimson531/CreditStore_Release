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
namespace CreditStore_Release
{
    public class CreditStore_Release_ModDefinition:ModDefinition
    {

    }
    /* //Example
    public class ExampleSkill : CustomSkillGDE<CreditStore_Release_ModDefinition>
    {
        public override string Key()
        {
            return "ExampleSkill"; //it will override your "ExampleSkill" gdata in the mod editor
        }
        public override ModGDEInfo.LoadingType GetLoadingType()
        {
            return ModGDEInfo.LoadingType.Add; 
        }
        public override void SetValue()
        {
            PlusSkillView = ModKey<ExampleSkill>();//for ModDefinition gdata
            User = GDEItemKeys.Character_Azar;//for gdata of orginial game
            SkillExtended = new List<string> { typeof(ExampleSkill_SkillExtended).AssemblyQualifiedName };//for script
            //Image_0 = assetInfo.ImageFromAsset("Your AssetBundle Path", "Path in Unity"); 
        }
        public class ExampleSkill_SkillExtended:Skill_Extended
        {

        }
    }*/
}