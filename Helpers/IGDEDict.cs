using ChronoArkMod;
using GameDataEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditStore_Release
{
    public static class IGDEDict
    {
        public static List<Type> StoreItemTypes = new List<Type>();
        public static List<Type> IGDETypes = new List<Type>();
        public static Dictionary<Type, Func<List<object>>> IGDETypeToObjectDel = new Dictionary<Type, Func<List<object>>>();
        public static Dictionary<Type, Func<object, int, string>> IGDETypeToStringDel = new Dictionary<Type, Func<object, int, string>>();

        static IGDEDict()
        {
            IGDETypeToStringDel.Add(typeof(GDEArkUpgradeData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEArkUpgradeData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEBattleMapsData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEBattleMapsData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEBuffData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEBuffData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEBuffTagData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEBuffTagData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDECharacter_SkinData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDECharacter_SkinData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDECharacter_SkinTypeData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDECharacter_SkinTypeData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDECharacter_TextData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDECharacter_TextData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDECharacterData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDECharacterData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDECharRoleData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDECharRoleData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDECurseListData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDECurseListData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEEnchantListData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEEnchantListData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEEnemyCategoryData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEEnemyCategoryData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEEnemyData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEEnemyData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEEnemyQueueData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEEnemyQueueData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEFieldMapsData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEFieldMapsData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEFieldObjectData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEFieldObjectData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEGameobjectDatasData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEGameobjectDatasData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEImageDatasData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEImageDatasData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEItem_ActiveData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEItem_ActiveData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEItem_ConsumeData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEItem_ConsumeData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEItem_EquipData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEItem_EquipData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEItem_FriendshipData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEItem_FriendshipData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEItem_MiscData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEItem_MiscData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEItem_PassiveData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEItem_PassiveData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEItem_PotionsData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEItem_PotionsData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEItem_ScrollData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEItem_ScrollData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEItemArrayData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEItemArrayData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEItemClassData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEItemClassData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEPresetData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEPresetData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDERandomDropData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDERandomDropData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDERandomEventData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDERandomEventData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDERewardData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDERewardData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEs_targettypeData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEs_targettypeData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDESimpleCampDialogueData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDESimpleCampDialogueData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDESkillCategoryData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDESkillCategoryData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDESkillData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDESkillData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDESkillEffectData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDESkillEffectData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDESkillExtendedData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDESkillExtendedData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDESkillKeywordData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDESkillKeywordData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDESkillUpgradeData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDESkillUpgradeData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDESkillUserData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDESkillUserData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDESpecialKeyData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDESpecialKeyData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDESpecialRuleData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDESpecialRuleData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEStageData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEStageData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEStageListData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEStageListData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEStartingPartyData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEStartingPartyData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEStatData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEStatData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEStory_CharacterData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEStory_CharacterData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEStory_CharDepartmentData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEStory_CharDepartmentData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEStory_NameData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEStory_NameData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEUnlockItemListData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEUnlockItemListData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEUnlockWindowData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEUnlockWindowData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDEVFXSkillData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDEVFXSkillData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(IGDEData), new Func<object, int, string>((obj, lvl) => { return ConvertToIGDEData(obj)?.IGDEDataToString(lvl); }));
            IGDETypeToStringDel.Add(typeof(GDECustomSchemaData), new Func<object, int, string>((obj, lvl) => { return ConvertToGDECustomSchemaData(obj)?.IGDEDataToString(lvl); }));
            Init2();
            Init3();
            Init4();
        }
        static void Init4()
        {
            IGDETypeToObjectDel.Add(typeof(GDEArkUpgradeData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEArkUpgradeData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEBattleMapsData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEBattleMapsData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEBuffData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEBuffData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEBuffTagData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEBuffTagData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDECharacter_SkinData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDECharacter_SkinData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDECharacter_SkinTypeData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDECharacter_SkinTypeData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDECharacter_TextData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDECharacter_TextData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDECharacterData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDECharacterData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDECharRoleData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDECharRoleData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDECurseListData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDECurseListData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEEnchantListData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEEnchantListData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEEnemyCategoryData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEEnemyCategoryData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEEnemyData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEEnemyData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEEnemyQueueData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEEnemyQueueData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEFieldMapsData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEFieldMapsData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEFieldObjectData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEFieldObjectData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEGameobjectDatasData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEGameobjectDatasData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEImageDatasData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEImageDatasData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEItem_ActiveData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEItem_ActiveData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEItem_ConsumeData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEItem_ConsumeData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEItem_EquipData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEItem_EquipData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEItem_FriendshipData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEItem_FriendshipData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEItem_MiscData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEItem_MiscData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEItem_PassiveData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEItem_PassiveData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEItem_PotionsData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEItem_PotionsData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEItem_ScrollData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEItem_ScrollData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEItemArrayData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEItemArrayData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEItemClassData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEItemClassData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEPresetData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEPresetData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDERandomDropData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDERandomDropData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDERandomEventData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDERandomEventData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDERewardData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDERewardData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEs_targettypeData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEs_targettypeData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDESimpleCampDialogueData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDESimpleCampDialogueData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDESkillCategoryData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDESkillCategoryData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDESkillData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDESkillData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDESkillEffectData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDESkillEffectData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDESkillExtendedData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDESkillExtendedData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDESkillKeywordData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDESkillKeywordData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDESkillUpgradeData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDESkillUpgradeData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDESkillUserData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDESkillUserData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDESpecialKeyData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDESpecialKeyData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDESpecialRuleData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDESpecialRuleData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEStageData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEStageData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEStageListData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEStageListData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEStartingPartyData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEStartingPartyData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEStatData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEStatData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEStory_CharacterData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEStory_CharacterData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEStory_CharDepartmentData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEStory_CharDepartmentData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEStory_NameData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEStory_NameData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEUnlockItemListData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEUnlockItemListData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEUnlockWindowData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEUnlockWindowData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDEVFXSkillData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDEVFXSkillData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(IGDEData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<IGDEData>().Cast<object>().ToList(); }));
            IGDETypeToObjectDel.Add(typeof(GDECustomSchemaData), new Func<List<object>>(() => { return GDEDataManager.GetAllItems<GDECustomSchemaData>().Cast<object>().ToList(); }));

        }
        static void Init3()
        {
            StoreItemTypes.Add(typeof(GDEItem_ConsumeData));
            StoreItemTypes.Add(typeof(GDEItem_EquipData));
            StoreItemTypes.Add(typeof(GDEItem_MiscData));
            StoreItemTypes.Add(typeof(GDEItem_PassiveData));
            StoreItemTypes.Add(typeof(GDEItem_PotionsData));
            StoreItemTypes.Add(typeof(GDEItem_ScrollData));
        }
        static void Init2()
        {
            IGDETypes.Add(typeof(GDEArkUpgradeData));
            IGDETypes.Add(typeof(GDEBattleMapsData));
            IGDETypes.Add(typeof(GDEBuffData));
            IGDETypes.Add(typeof(GDEBuffTagData));
            IGDETypes.Add(typeof(GDECharacter_SkinData));
            IGDETypes.Add(typeof(GDECharacter_SkinTypeData));
            IGDETypes.Add(typeof(GDECharacter_TextData));
            IGDETypes.Add(typeof(GDECharacterData));
            IGDETypes.Add(typeof(GDECharRoleData));
            IGDETypes.Add(typeof(GDECurseListData));
            IGDETypes.Add(typeof(GDEEnchantListData));
            IGDETypes.Add(typeof(GDEEnemyCategoryData));
            IGDETypes.Add(typeof(GDEEnemyData));
            IGDETypes.Add(typeof(GDEEnemyQueueData));
            IGDETypes.Add(typeof(GDEFieldMapsData));
            IGDETypes.Add(typeof(GDEFieldObjectData));
            IGDETypes.Add(typeof(GDEGameobjectDatasData));
            IGDETypes.Add(typeof(GDEImageDatasData));
            IGDETypes.Add(typeof(GDEItem_ActiveData));
            IGDETypes.Add(typeof(GDEItem_ConsumeData));
            IGDETypes.Add(typeof(GDEItem_EquipData));
            IGDETypes.Add(typeof(GDEItem_FriendshipData));
            IGDETypes.Add(typeof(GDEItem_MiscData));
            IGDETypes.Add(typeof(GDEItem_PassiveData));
            IGDETypes.Add(typeof(GDEItem_PotionsData));
            IGDETypes.Add(typeof(GDEItem_ScrollData));
            IGDETypes.Add(typeof(GDEItemArrayData));
            IGDETypes.Add(typeof(GDEItemClassData));
            IGDETypes.Add(typeof(GDEPresetData));
            IGDETypes.Add(typeof(GDERandomDropData));
            IGDETypes.Add(typeof(GDERandomEventData));
            IGDETypes.Add(typeof(GDERewardData));
            IGDETypes.Add(typeof(GDEs_targettypeData));
            IGDETypes.Add(typeof(GDESimpleCampDialogueData));
            IGDETypes.Add(typeof(GDESkillCategoryData));
            IGDETypes.Add(typeof(GDESkillData));
            IGDETypes.Add(typeof(GDESkillEffectData));
            IGDETypes.Add(typeof(GDESkillExtendedData));
            IGDETypes.Add(typeof(GDESkillKeywordData));
            IGDETypes.Add(typeof(GDESkillUpgradeData));
            IGDETypes.Add(typeof(GDESkillUserData));
            IGDETypes.Add(typeof(GDESpecialKeyData));
            IGDETypes.Add(typeof(GDESpecialRuleData));
            IGDETypes.Add(typeof(GDEStageData));
            IGDETypes.Add(typeof(GDEStageListData));
            IGDETypes.Add(typeof(GDEStartingPartyData));
            IGDETypes.Add(typeof(GDEStatData));
            IGDETypes.Add(typeof(GDEStory_CharacterData));
            IGDETypes.Add(typeof(GDEStory_CharDepartmentData));
            IGDETypes.Add(typeof(GDEStory_NameData));
            IGDETypes.Add(typeof(GDEUnlockItemListData));
            IGDETypes.Add(typeof(GDEUnlockWindowData));
            IGDETypes.Add(typeof(GDEVFXSkillData));
            IGDETypes.Add(typeof(IGDEData));
            IGDETypes.Add(typeof(GDECustomSchemaData));

        }
        public static string GetIGDEName(IGDEData data)
        {
            return IGDEDict.GetIGDEName(data);
        }
        public static List<T> GetItemDatasByType<T>() where T : IGDEData
        {
            List<T> results = new List<T>();

            results.AddRange(GDEDataManager.GetAllItems<T>());

            return results;
        }
        public static GDEArkUpgradeData ConvertToGDEArkUpgradeData(object obj)
        {
            return (GDEArkUpgradeData)obj;
        }
        public static GDEBattleMapsData ConvertToGDEBattleMapsData(object obj)
        {
            return (GDEBattleMapsData)obj;
        }
        public static GDEBuffData ConvertToGDEBuffData(object obj)
        {
            return (GDEBuffData)obj;
        }
        public static GDEBuffTagData ConvertToGDEBuffTagData(object obj)
        {
            return (GDEBuffTagData)obj;
        }
        public static GDECharacter_SkinData ConvertToGDECharacter_SkinData(object obj)
        {
            return (GDECharacter_SkinData)obj;
        }
        public static GDECharacter_SkinTypeData ConvertToGDECharacter_SkinTypeData(object obj)
        {
            return (GDECharacter_SkinTypeData)obj;
        }
        public static GDECharacter_TextData ConvertToGDECharacter_TextData(object obj)
        {
            return (GDECharacter_TextData)obj;
        }
        public static GDECharacterData ConvertToGDECharacterData(object obj)
        {
            return (GDECharacterData)obj;
        }
        public static GDECharRoleData ConvertToGDECharRoleData(object obj)
        {
            return (GDECharRoleData)obj;
        }
        public static GDECurseListData ConvertToGDECurseListData(object obj)
        {
            return (GDECurseListData)obj;
        }
        public static GDEEnchantListData ConvertToGDEEnchantListData(object obj)
        {
            return (GDEEnchantListData)obj;
        }
        public static GDEEnemyCategoryData ConvertToGDEEnemyCategoryData(object obj)
        {
            return (GDEEnemyCategoryData)obj;
        }
        public static GDEEnemyData ConvertToGDEEnemyData(object obj)
        {
            return (GDEEnemyData)obj;
        }
        public static GDEEnemyQueueData ConvertToGDEEnemyQueueData(object obj)
        {
            return (GDEEnemyQueueData)obj;
        }
        public static GDEFieldMapsData ConvertToGDEFieldMapsData(object obj)
        {
            return (GDEFieldMapsData)obj;
        }
        public static GDEFieldObjectData ConvertToGDEFieldObjectData(object obj)
        {
            return (GDEFieldObjectData)obj;
        }
        public static GDEGameobjectDatasData ConvertToGDEGameobjectDatasData(object obj)
        {
            return (GDEGameobjectDatasData)obj;
        }
        public static GDEImageDatasData ConvertToGDEImageDatasData(object obj)
        {
            return (GDEImageDatasData)obj;
        }
        public static GDEItem_ActiveData ConvertToGDEItem_ActiveData(object obj)
        {
            return (GDEItem_ActiveData)obj;
        }
        public static GDEItem_ConsumeData ConvertToGDEItem_ConsumeData(object obj)
        {
            return (GDEItem_ConsumeData)obj;
        }
        public static GDEItem_EquipData ConvertToGDEItem_EquipData(object obj)
        {
            return (GDEItem_EquipData)obj;
        }
        public static GDEItem_FriendshipData ConvertToGDEItem_FriendshipData(object obj)
        {
            return (GDEItem_FriendshipData)obj;
        }
        public static GDEItem_MiscData ConvertToGDEItem_MiscData(object obj)
        {
            return (GDEItem_MiscData)obj;
        }
        public static GDEItem_PassiveData ConvertToGDEItem_PassiveData(object obj)
        {
            return (GDEItem_PassiveData)obj;
        }
        public static GDEItem_PotionsData ConvertToGDEItem_PotionsData(object obj)
        {
            return (GDEItem_PotionsData)obj;
        }
        public static GDEItem_ScrollData ConvertToGDEItem_ScrollData(object obj)
        {
            return (GDEItem_ScrollData)obj;
        }
        public static GDEItemArrayData ConvertToGDEItemArrayData(object obj)
        {
            return (GDEItemArrayData)obj;
        }
        public static GDEItemClassData ConvertToGDEItemClassData(object obj)
        {
            return (GDEItemClassData)obj;
        }
        public static GDEPresetData ConvertToGDEPresetData(object obj)
        {
            return (GDEPresetData)obj;
        }
        public static GDERandomDropData ConvertToGDERandomDropData(object obj)
        {
            return (GDERandomDropData)obj;
        }
        public static GDERandomEventData ConvertToGDERandomEventData(object obj)
        {
            return (GDERandomEventData)obj;
        }
        public static GDERewardData ConvertToGDERewardData(object obj)
        {
            return (GDERewardData)obj;
        }
        public static GDEs_targettypeData ConvertToGDEs_targettypeData(object obj)
        {
            return (GDEs_targettypeData)obj;
        }
        public static GDESimpleCampDialogueData ConvertToGDESimpleCampDialogueData(object obj)
        {
            return (GDESimpleCampDialogueData)obj;
        }
        public static GDESkillCategoryData ConvertToGDESkillCategoryData(object obj)
        {
            return (GDESkillCategoryData)obj;
        }
        public static GDESkillData ConvertToGDESkillData(object obj)
        {
            return (GDESkillData)obj;
        }
        public static GDESkillEffectData ConvertToGDESkillEffectData(object obj)
        {
            return (GDESkillEffectData)obj;
        }
        public static GDESkillExtendedData ConvertToGDESkillExtendedData(object obj)
        {
            return (GDESkillExtendedData)obj;
        }
        public static GDESkillKeywordData ConvertToGDESkillKeywordData(object obj)
        {
            return (GDESkillKeywordData)obj;
        }
        public static GDESkillUpgradeData ConvertToGDESkillUpgradeData(object obj)
        {
            return (GDESkillUpgradeData)obj;
        }
        public static GDESkillUserData ConvertToGDESkillUserData(object obj)
        {
            return (GDESkillUserData)obj;
        }
        public static GDESpecialKeyData ConvertToGDESpecialKeyData(object obj)
        {
            return (GDESpecialKeyData)obj;
        }
        public static GDESpecialRuleData ConvertToGDESpecialRuleData(object obj)
        {
            return (GDESpecialRuleData)obj;
        }
        public static GDEStageData ConvertToGDEStageData(object obj)
        {
            return (GDEStageData)obj;
        }
        public static GDEStageListData ConvertToGDEStageListData(object obj)
        {
            return (GDEStageListData)obj;
        }
        public static GDEStartingPartyData ConvertToGDEStartingPartyData(object obj)
        {
            return (GDEStartingPartyData)obj;
        }
        public static GDEStatData ConvertToGDEStatData(object obj)
        {
            return (GDEStatData)obj;
        }
        public static GDEStory_CharacterData ConvertToGDEStory_CharacterData(object obj)
        {
            return (GDEStory_CharacterData)obj;
        }
        public static GDEStory_CharDepartmentData ConvertToGDEStory_CharDepartmentData(object obj)
        {
            return (GDEStory_CharDepartmentData)obj;
        }
        public static GDEStory_NameData ConvertToGDEStory_NameData(object obj)
        {
            return (GDEStory_NameData)obj;
        }
        public static GDEUnlockItemListData ConvertToGDEUnlockItemListData(object obj)
        {
            return (GDEUnlockItemListData)obj;
        }
        public static GDEUnlockWindowData ConvertToGDEUnlockWindowData(object obj)
        {
            return (GDEUnlockWindowData)obj;
        }
        public static GDEVFXSkillData ConvertToGDEVFXSkillData(object obj)
        {
            return (GDEVFXSkillData)obj;
        }
        public static IGDEData ConvertToIGDEData(object obj)
        {
            return (IGDEData)obj;
        }
        public static GDECustomSchemaData ConvertToGDECustomSchemaData(object obj)
        {
            return (GDECustomSchemaData)obj;
        }

    }

}
