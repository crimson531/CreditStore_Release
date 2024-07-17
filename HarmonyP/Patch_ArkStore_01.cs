using CreditStore_Release;
using GameDataEditor;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditStore_Release
{
    /// <summary>
    /// Holds members for retrieving schema data
    /// </summary>
    [HarmonyPatch]
    public static partial class Patch_ArkStore
    {
        private static int defaultInventorySize = 15;
        public static int DefaultInventorySize
        {
            get
            {
                return defaultInventorySize;
            }
            private set
            {
                defaultInventorySize = value;
            }
        }
        private static int AddedInventorySlots
        {
            get
            {
                if (PurchasedAmount.ContainsKey(SpecialKeys.InventorySlot))
                {
                    return PurchasedAmount[SpecialKeys.InventorySlot];
                }
                return 0;
            }
        }
        public static int ModifiedInventorySize
        {
            get
            {
                return DefaultInventorySize + AddedInventorySlots;
            }
        }
        // references data name, not the key
        public static List<string> MiscItemFilter
        {
            get
            {
                return new List<string>() { "Gold", "Key", "Soulstone" };
            }
        }
        // references data key
        public static List<string> IgnoreStacks
        {
            get
            {
                return new List<string>() { "Gold", "Soul" };
            }
        }
        public static List<GDEItem_ConsumeData> ConsumeableData
        {
            get
            {
                List<GDEItem_ConsumeData> items = new List<GDEItem_ConsumeData>();

                items.AddRange(IGDEDict.GetItemDatasByType<GDEItem_ConsumeData>());

                return items;
            }
        }
        public static List<GDEItem_EquipData> EquipData
        {
            get
            {
                List<GDEItem_EquipData> results = new List<GDEItem_EquipData>();

                results.AddRange(IGDEDict.GetItemDatasByType<GDEItem_EquipData>());

                return results;
            }
        }
        public static List<GDEItem_PassiveData> PassiveDatas
        {
            get
            {
                List<GDEItem_PassiveData> items = new List<GDEItem_PassiveData>();

                items.AddRange(IGDEDict.GetItemDatasByType<GDEItem_PassiveData>());

                return items;
            }
        }
        public static List<GDEItem_ScrollData> ScrollDatas
        {
            get
            {
                List<GDEItem_ScrollData> items = new List<GDEItem_ScrollData>();

                items.AddRange(IGDEDict.GetItemDatasByType<GDEItem_ScrollData>());

                return items;
            }
        }
        public static List<GDEItem_PotionsData> PotionsDatas
        {
            get
            {
                List<GDEItem_PotionsData> items = new List<GDEItem_PotionsData>();

                items.AddRange(IGDEDict.GetItemDatasByType<GDEItem_PotionsData>());

                return items;
            }
        }
        public static List<GDEItem_MiscData> MiscData
        {
            get
            {
                List<GDEItem_MiscData> items = new List<GDEItem_MiscData>();


                items.AddRange(IGDEDict.GetItemDatasByType<GDEItem_MiscData>().
                    Where(data => MiscItemFilter.Contains(data.name)));


                return items;
            }
        }
        public static  List<string> SpecialData
        {
            get
            {
                //List<string> items = new List<string>();

                //items.Add(SpecialKeys.InventorySlot);
                //items.Add(SpecialKeys.EquipSlot);
                //items.Add(SpecialKeys.RelicSlot);

                //return items;
                return SpecialKeys.GetSpecialKeys();
            }
        }
        public static int StackMultiplier(this string key)
        {
            switch (key)
            {
                case "Gold":
                    return 1000;
                case "Soul":
                    return 10;
                default: return 1;
            }
        }

    }

    public enum PriceGroup
    {
        Special,
        Misc,
        Consumeable,
        Relic,
        Equip,
        Scroll,
        Potion
    }

}
