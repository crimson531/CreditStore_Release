using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CreditStore_Release
{
    public static class SpecialKeys
    {
        public const string InventorySlot = "Inventory Slot";
        public const string EquipSlot = "Equip Slot";
        public const string RelicSlot = "Relic Slot";
        public const string NoRareLimit = "Remove Rare Skill Limit";
        public const string UpgradeArtifactPouch = "Artifact Pouch - Add Selection";
        public static List<string> GetSpecialKeys()
        {
            List<string> keys = new List<string>();

            FieldInfo[] fields = typeof(SpecialKeys).GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo field in fields)
            {
                if (field.IsLiteral && !field.IsInitOnly)
                {
                    keys.Add(field.GetValue(null).ToString());
                }
            }

            return keys;
        }
    }
}
