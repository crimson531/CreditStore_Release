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
using System.Windows.Markup;
using ChronoArkMod.ModData.Settings;

namespace CreditStore_Release
{
    public delegate void UpgradeButtonClick(ArkUpgradeButton button, IGDEData data);
    /// <summary>
    /// Includes members for altering and creating UI elements
    /// </summary>
    [HarmonyPatch]
    public static partial class Patch_ArkStore
    {
        public static event UpgradeButtonClick OnUpgradeButtonClick;
        static ArkUpgrade arkUpgradeIns; // do not destroy
        static Transform arkTransform; // do not destroy
        static Transform upgradeList;
        static Transform upgradeListScrollView;
        static Transform upgradeListViewport;
        static Transform upgradeListClone;
        static PadScrollView upgradeListScroll;
        static RectTransform purchaseTransform;
        static TMP_Text purchaseText;
        static bool OptionFreeCreditStore =>
            ModManager.getModInfo(CreditStore.modname).GetSetting<ToggleSetting>("FreeCreditStore").Value;
        static bool OptionShowAll =>
            ModManager.getModInfo(CreditStore.modname).GetSetting<ToggleSetting>("ShowLocked").Value;
        static List<Transform> TransformTargets
        {
            get
            {
                List<Transform> list = new List<Transform>
                {
                    upgradeListClone,
                    upgradeListViewport,
                    upgradeListScrollView,
                    upgradeList
                };

                return list;
            }
        }
        static Transform Copy(Transform source) => ue.Object.Instantiate(source);
        static Action<GameObject> DestroySafe => new Action<GameObject>((go) => { if (go != null) ue.Object.Destroy(go); });
        static RectTransform MyMainRectTransform => upgradeList?.GetComponent<RectTransform>();
        static RectTransform OriginalMainRectTransform => arkUpgradeIns?.UpgradeList.parent.parent.parent.GetComponent<RectTransform>();
        static GameObject UpgradeButtonPrefab => arkUpgradeIns?.UpgradeButton;
        static GameObject LinePrefab => arkUpgradeIns.Line;
        static List<ArkUpgradeButton> UpgradeButtons = new List<ArkUpgradeButton>();
        static List<ArkUpgradeButton> PurchasedButtons = new List<ArkUpgradeButton>();
        static Dictionary<string, IGDEData> PurchasedData = new Dictionary<string, IGDEData>();
        static Dictionary<string, int> PurchasedAmount = new Dictionary<string, int>();
        static Dictionary<string, ArkUpgradeButton> KeyToPurchasedButton = new Dictionary<string, ArkUpgradeButton>();
        static Dictionary<string, int> MaxStacks = new Dictionary<string, int>();
        static List<string> PurchasedDataSpecial = new List<string>();
        static Patch_ArkStore()
        {
            OnUpgradeButtonClick += UpgradeButton_OnClick;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ArkUpgrade), nameof(ArkUpgrade.Delete))]
        public static bool Delete_Prefix()
        {
            KeyToPurchasedButton.Clear();
            PurchasedButtons.Clear();
            UpgradeButtons.Clear();

            Destroy();
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ArkUpgrade), "Init")]
        public static void Init_Prefix(ref ArkUpgrade __instance)
        {
            arkUpgradeIns = __instance;
            arkTransform = __instance.transform;
            CloneUIElements(ref __instance);
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ArkUpgrade), "Init")]
        public static void Init_Postfix(ref ArkUpgrade __instance)
        {

            LoadButtons();
        }
        static void LoadButtons()
        {
            LoadSpecialButtons();
            LoadMiscButtons();
            LoadConsumeableButtons();
            LoadPassiveButtons();
            LoadEquipButtons();
            LoadScrollButtons();
            LoadPotionsButtons();
            LoadPurchasedButtons();
        }
        static void LoadSpecialButtons()
        {
            GameObject line = ue.GameObject.Instantiate(LinePrefab, arkUpgradeIns.UpgradeList);
            line.GetComponentInChildren<TextMeshProUGUI>().text = "Credit Store - Special";
            line.transform.SetSiblingIndex(line.transform.GetSiblingIndex());

            foreach (var data in SpecialData)
            {
                ArkUpgradeButton button = ue.Object.Instantiate(UpgradeButtonPrefab, arkUpgradeIns.UpgradeList).GetComponentInChildren<ArkUpgradeButton>();

                button.Key = data;
                button.SetPrivateField("Main", arkUpgradeIns);
                button.SE.SR = arkUpgradeIns.Scroll.Main;
                button.item = null;
                button.IsItem = false;
                button.NameText.text = data;

                button.Price = GetPrice(PriceGroup.Special);
                
                button.button.onClick.RemoveAllListeners();
                button.button.onClick.AddListener(() => { UpgradeButton_Special_OnClickListener(button); });

                UpgradeButtons.Add(button);
            }
        }
        static void LoadMiscButtons()
        {
            GameObject line = ue.GameObject.Instantiate(LinePrefab, arkUpgradeIns.UpgradeList);
            line.GetComponentInChildren<TextMeshProUGUI>().text = "Credit Store - Misc";
            line.transform.SetSiblingIndex(line.transform.GetSiblingIndex());

            foreach (var data in MiscData.OrderBy(data => data.name))
            {
                ArkUpgradeButton button = ue.Object.Instantiate(UpgradeButtonPrefab, arkUpgradeIns.UpgradeList).GetComponentInChildren<ArkUpgradeButton>();
                
                button.Key = data.Key;
                button.SetPrivateField("Main", arkUpgradeIns);
                button.SE.SR = arkUpgradeIns.Scroll.Main;
                //if (SaveManager.IsUnlock(data.Key))
                //{
                //    button.Hide = true;
                //}

                button.item = ItemBase.GetItem(data.Key);
                button.IsItem = true;
                button.NameText.text = button.item.GetName;

                button.Price = GetPrice(PriceGroup.Misc);
                button.Icon.sprite = button.item.GetSprite;
                button.button.onClick.RemoveAllListeners();
                button.button.onClick.AddListener(() => { UpgradeButton_OnClickListener(button, data); });

                if (!MaxStacks.ContainsKey(data.Key))
                {
                    if (data.MaxStack == 0)
                    {
                        MaxStacks.Add(data.Key, 1);
                    }
                    else
                    {
                        MaxStacks.Add(data.Key, data.MaxStack);
                    }
                }

                UpgradeButtons.Add(button);
            }
        }
        static void LoadConsumeableButtons()
        {
            GameObject line = ue.GameObject.Instantiate(LinePrefab, arkUpgradeIns.UpgradeList);
            line.GetComponentInChildren<TextMeshProUGUI>().text = "Credit Store - Consumeables";
            line.transform.SetSiblingIndex(line.transform.GetSiblingIndex());

            foreach (var data in ConsumeableData.OrderBy(data => data.name))
            {
                ArkUpgradeButton button = ue.Object.Instantiate(UpgradeButtonPrefab, arkUpgradeIns.UpgradeList).GetComponentInChildren<ArkUpgradeButton>();

                button.Key = data.Key;
                button.SetPrivateField("Main", arkUpgradeIns);
                button.SE.SR = arkUpgradeIns.Scroll.Main;
                //if (SaveManager.IsUnlock(data.Key))
                //{
                //    button.Hide = true;
                //}

                button.item = ItemBase.GetItem(data.Key);
                button.IsItem = true;
                button.NameText.text = button.item.GetName;

                button.Price = GetPrice(PriceGroup.Consumeable);
                button.Icon.sprite = button.item.GetSprite;
                button.button.onClick.RemoveAllListeners();
                button.button.onClick.AddListener(() => { UpgradeButton_OnClickListener(button, data); });

                if (!MaxStacks.ContainsKey(data.Key))
                {
                    if (data.Stack == 0)
                    {
                        MaxStacks.Add(data.Key, 1);
                    }
                    else
                    {
                        MaxStacks.Add(data.Key, data.Stack);
                    }
                }

                UpgradeButtons.Add(button);
            }
        }
        static void LoadPassiveButtons()
        {
            GameObject line = ue.GameObject.Instantiate(LinePrefab, arkUpgradeIns.UpgradeList);
            line.GetComponentInChildren<TextMeshProUGUI>().text = "Credit Store - Relics";
            line.transform.SetSiblingIndex(line.transform.GetSiblingIndex());

            foreach (var data in PassiveDatas.OrderBy(data => data.name))
            {
                if (!OptionShowAll &&
                    !SaveManager.savemanager._NowData.unlockList.UnlockItems.Contains(data.Key))
                {
                    continue;
                }

                ArkUpgradeButton button = ue.Object.Instantiate(UpgradeButtonPrefab, arkUpgradeIns.UpgradeList).GetComponentInChildren<ArkUpgradeButton>();

                button.Key = data.Key;
                button.SetPrivateField("Main", arkUpgradeIns);
                button.SE.SR = arkUpgradeIns.Scroll.Main;
                if (OptionShowAll)
                {
                    button.Hide = false;
                }
                else if (!SaveManager.savemanager._NowData.unlockList.UnlockItems.Contains(data.Key))
                {
                    button.Hide = true;
                }

                button.item = ItemBase.GetItem(data.Key);
                button.IsItem = true;
                button.NameText.text = button.item.GetName;

                button.Price = GetPrice(PriceGroup.Relic);
                button.Icon.sprite = button.item.GetSprite;
                button.button.onClick.RemoveAllListeners();
                button.button.onClick.AddListener(() => { UpgradeButton_OnClickListener(button, data); });

                if (!MaxStacks.ContainsKey(data.Key))
                {
                    MaxStacks.Add(data.Key, 1);
                }

                UpgradeButtons.Add(button);
            }
        }
        static void LoadEquipButtons()
        {
            GameObject line = ue.GameObject.Instantiate(LinePrefab, arkUpgradeIns.UpgradeList);
            line.GetComponentInChildren<TextMeshProUGUI>().text = "Credit Store - Equips";
            line.transform.SetSiblingIndex(line.transform.GetSiblingIndex());

            foreach (var data in EquipData.OrderBy(data => data.name))
            {
                if (!OptionShowAll &&
                    !SaveManager.savemanager._NowData.unlockList.UnlockItems.Contains(data.Key))
                {
                    continue;
                }

                ArkUpgradeButton button = ue.Object.Instantiate(UpgradeButtonPrefab, arkUpgradeIns.UpgradeList).GetComponentInChildren<ArkUpgradeButton>();

                button.Key = data.Key;
                button.SetPrivateField("Main", arkUpgradeIns);
                button.SE.SR = arkUpgradeIns.Scroll.Main;
                if (OptionShowAll)
                {
                    button.Hide = false;
                }
                else if (!SaveManager.savemanager._NowData.unlockList.UnlockItems.Contains(data.Key))
                {
                    button.Hide = true;
                }

                button.item = ItemBase.GetItem(data.Key);
                button.IsItem = true;
                button.NameText.text = button.item.GetName;

                button.Price = GetPrice(PriceGroup.Equip);
                button.Icon.sprite = button.item.GetSprite;
                button.button.onClick.RemoveAllListeners();
                button.button.onClick.AddListener(() => { UpgradeButton_OnClickListener(button, data); });

                if (!MaxStacks.ContainsKey(data.Key))
                {
                    MaxStacks.Add(data.Key, 1);
                }

                UpgradeButtons.Add(button);
            }
        }
        static void LoadScrollButtons()
        {
            GameObject line = ue.GameObject.Instantiate(LinePrefab, arkUpgradeIns.UpgradeList);
            line.GetComponentInChildren<TextMeshProUGUI>().text = "Credit Store - Scrolls";
            line.transform.SetSiblingIndex(line.transform.GetSiblingIndex());

            foreach (var data in ScrollDatas.OrderBy(data => data.name))
            {
                ArkUpgradeButton button = ue.Object.Instantiate(UpgradeButtonPrefab, arkUpgradeIns.UpgradeList).GetComponentInChildren<ArkUpgradeButton>();

                button.Key = data.Key;
                button.SetPrivateField("Main", arkUpgradeIns);
                button.SE.SR = arkUpgradeIns.Scroll.Main;
                if (SaveManager.IsUnlock(data.Key))
                {
                    button.Hide = true;
                }

                button.item = ItemBase.GetItem(data.Key);
                button.IsItem = true;

                if (!PlayData.TSavedata.IdentifyItems.Contains(data.Key))
                {
                    PlayData.TSavedata.IdentifyItems.Add(data.Key);
                }

                button.NameText.text = button.item.GetName;

                button.Price = GetPrice(PriceGroup.Scroll);
                button.Icon.sprite = button.item.GetSprite;
                button.button.onClick.RemoveAllListeners();
                button.button.onClick.AddListener(() => { UpgradeButton_OnClickListener(button, data); });

                if (!MaxStacks.ContainsKey(data.Key))
                {
                    MaxStacks.Add(data.Key, button.item.MaxStack);
                }
                
                UpgradeButtons.Add(button);
            }
        }
        static void LoadPotionsButtons()
        {
            GameObject line = ue.GameObject.Instantiate(LinePrefab, arkUpgradeIns.UpgradeList);
            line.GetComponentInChildren<TextMeshProUGUI>().text = "Credit Store - Potions";
            line.transform.SetSiblingIndex(line.transform.GetSiblingIndex());

            foreach (var data in PotionsDatas.OrderBy(data => data.name))
            {
                ArkUpgradeButton button = ue.Object.Instantiate(UpgradeButtonPrefab, arkUpgradeIns.UpgradeList).GetComponentInChildren<ArkUpgradeButton>();

                button.Key = data.Key;
                button.SetPrivateField("Main", arkUpgradeIns);
                button.SE.SR = arkUpgradeIns.Scroll.Main;
                if (SaveManager.IsUnlock(data.Key))
                {
                    button.Hide = true;
                }

                button.item = ItemBase.GetItem(data.Key);
                button.IsItem = true;
                button.NameText.text = button.item.GetName;

                button.Price = GetPrice(PriceGroup.Potion);
                button.Icon.sprite = button.item.GetSprite;
                button.button.onClick.RemoveAllListeners();
                button.button.onClick.AddListener(() => { UpgradeButton_OnClickListener(button, data); });

                if (!MaxStacks.ContainsKey(data.Key))
                {
                    MaxStacks.Add(data.Key, 1);
                }

                UpgradeButtons.Add(button);
            }
        }
        static void LoadPurchasedButtons()
        {
            foreach (var key in PurchasedDataSpecial)
            {
                CreatePurchasedButtonSpecial(key);
                ModifyButtonSpecial(KeyToPurchasedButton[key]);
                KeyToPurchasedButton[key].Price = GetPrice(key);
            }
            foreach (var kvp in PurchasedData)
            {
                var data = kvp.Value;

                CreatePurchasedButton(data);
                ModifyButton(KeyToPurchasedButton[data.Key], data);
                KeyToPurchasedButton[data.Key].Price = GetPrice(data.Key);
            }
            UpdatePurchasedText();
        }
        public static void UpgradeButton_OnClickListener(ArkUpgradeButton button, IGDEData data)
        {
            OnUpgradeButtonClick?.Invoke(button, data);
            UpdatePurchasedText();
        }
        public static void UpgradeButton_OnClick(ArkUpgradeButton button, IGDEData data)
        {
            if (!PurchasedData.ContainsKey(data.Key))
            {
                PurchasedData.Add(data.Key, data);
                PurchasedAmount.Add(data.Key, data.Key.StackMultiplier());

                CreatePurchasedButton(data);
            }
            else
            {
                PurchasedAmount[data.Key] += data.Key.StackMultiplier();
                ModifyButton(KeyToPurchasedButton[data.Key], data);
                // Modify button
            }
            var pb = KeyToPurchasedButton[data.Key];
            pb.Price = button.Price;

            RemoveTimeMoney(pb.Price);
            
        }
        public static void UpgradeButton_Special_OnClickListener(ArkUpgradeButton button)
        {
            if (!PurchasedDataSpecial.Contains(button.Key))
            {
                PurchasedDataSpecial.Add(button.Key);
                PurchasedAmount.Add(button.Key, 1);

                CreatePurchasedButtonSpecial(button.Key);

                
            }
            else
            {
                PurchasedAmount[button.Key]++;

                ModifyButtonSpecial(KeyToPurchasedButton[button.Key]);
            }
            var pb = KeyToPurchasedButton[button.Key];
            pb.Price = button.Price;
            
            RemoveTimeMoney(pb.Price);

            UpdatePurchasedText();
        }
        public static void CreatePurchasedButton(IGDEData data)
        {
            ArkUpgradeButton button = ue.Object.Instantiate(UpgradeButtonPrefab, upgradeListClone).GetComponentInChildren<ArkUpgradeButton>();
            
            button.Key = data.Key;
            button.SetPrivateField("Main", arkUpgradeIns);
            button.SE.SR = upgradeListScroll.Main;
            if (SaveManager.IsUnlock(data.Key))
            {
                button.Hide = true;
            }

            button.item = ItemBase.GetItem(data.Key);
            button.IsItem = true;
            var mult = data.Key.StackMultiplier();
            if (mult > 1)
            {
                button.NameText.text = $"{button.item.GetName} - {mult}";
            }
            else
            {
                button.NameText.text = button.item.GetName;
            }
            button.NameText.color = ue.Color.black;

            
            button.Icon.sprite = button.item.GetSprite;
            button.button.onClick.RemoveAllListeners();
            button.button.onClick.AddListener(() => { PurchasedButton_OnClickListener(button, data); });

            PurchasedButtons.Add(button);
            KeyToPurchasedButton.Add(data.Key, button);
        }
        public static void CreatePurchasedButtonSpecial(string key)
        {
            ArkUpgradeButton button = ue.Object.Instantiate(UpgradeButtonPrefab, upgradeListClone).GetComponentInChildren<ArkUpgradeButton>();

            button.Key = key;
            button.SetPrivateField("Main", arkUpgradeIns);
            button.SE.SR = upgradeListScroll.Main;
            button.item = null;
            button.IsItem = false;
            button.NameText.text = key;
            button.NameText.color = ue.Color.black;
            button.button.onClick.RemoveAllListeners();
            button.button.onClick.AddListener(() => { PurchasedButton_Special_OnClickListener(button.Key); });

            PurchasedButtons.Add(button);
            KeyToPurchasedButton.Add(key, button);
        }
        public static void PurchasedButton_OnClickListener(ArkUpgradeButton button, IGDEData data)
        {
            var price = button.Price;

            AddTimeMoney(price);

            if (PurchasedAmount[data.Key] <= data.Key.StackMultiplier())
            {
                PurchasedAmount.Remove(data.Key);
                PurchasedData.Remove(data.Key);
                PurchasedButtons.Remove(button);
                KeyToPurchasedButton.Remove(data.Key);

                DestroySafe(button.gameObject);
            }
            else
            {
                PurchasedAmount[data.Key] -= data.Key.StackMultiplier();
                ModifyButton(button, data);
            }

            UpdatePurchasedText();
        }
        public static void PurchasedButton_Special_OnClickListener(string key)
        {
            var price = KeyToPurchasedButton[key].Price;

            AddTimeMoney(price);

            if (PurchasedAmount[key] <= 1)
            {
                PurchasedAmount.Remove(key);
                PurchasedDataSpecial.Remove(key);

                var button = KeyToPurchasedButton[key];
                PurchasedButtons.Remove(button);

                KeyToPurchasedButton.Remove(key);

                DestroySafe(button.gameObject);
            }
            else
            {
                PurchasedAmount[key]--;
                ModifyButtonSpecial(KeyToPurchasedButton[key]);
            }

            UpdatePurchasedText();
        }
        public static void ModifyButton(ArkUpgradeButton button, IGDEData data)
        {
            var newText = $"{button.item.GetName} - {PurchasedAmount[data.Key]}";
            button.NameText.text = newText;
        }
        public static void ModifyButtonSpecial(ArkUpgradeButton button)
        {
            var newText = $"{button.Key} - {PurchasedAmount[button.Key]}";
            button.NameText.text = newText;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ArkUpgradeButton), nameof(ArkUpgradeButton.Click))]
        public static bool ArkUpgradeButton_Click_Prefix(ArkUpgradeButton __instance)
        {
            if (UpgradeButtons.Any(b => ReferenceEquals(__instance, b)) ||
                PurchasedButtons.Any(b => ReferenceEquals(__instance, b)))
            {
                return false;
            }
            return true;
        }
        static void CloneUIElements(ref ArkUpgrade ins)
        {
            Create(ref ins);
        }
        static void Create(ref ArkUpgrade ins)
        {
            Destroy();

            upgradeList = Copy(ins.UpgradeList.parent.parent.parent);
            upgradeListViewport = Copy(ins.UpgradeList.parent.parent);
            upgradeListScrollView = Copy(ins.UpgradeList.parent);
            upgradeListClone = Copy(ins.UpgradeList);

            upgradeList.SetParent(arkTransform, false);
            upgradeListViewport.SetParent(upgradeList, false);
            upgradeListScrollView.SetParent(upgradeListViewport, false);
            upgradeListClone.SetParent(upgradeListScrollView, false);

            ClearCopiedChildren();

            PositionMyMain();
            ClonePadScrollView(ref ins);
            CloneTitle(ref ins);
        }
        static void ClearCopiedChildren()
        {
            if (upgradeListClone != null && 
                upgradeListClone.childCount > 0)
            {
                ChildClear.Clear(upgradeListClone);
            }
        }
        static void CloneTitle(ref ArkUpgrade ins)
        {
            foreach (var comp in ins.GetComponentsInChildren<Component>())
            {

                if (comp != null &&
                    comp.name == "Title" &&
                    comp.GetType() == typeof(RectTransform))
                {
                    var or = comp as RectTransform;
                    purchaseTransform = ue.Object.Instantiate<RectTransform>((RectTransform)comp);
                    purchaseTransform.SetParent(arkTransform);
                    purchaseTransform.anchoredPosition = new Vector2(or.anchoredPosition.x + or.rect.width, or.anchoredPosition.y);
                    purchaseTransform.sizeDelta = new Vector2(or.rect.width, or.rect.height);
                    purchaseTransform.localScale = new Vector3(1, 1, 1);
                    purchaseTransform.position = new Vector3(
                        purchaseTransform.position.x,
                        purchaseTransform.position.y,
                        or.position.z);
                    ////csp.Log(purchaseTransform.gameObject.ObjToDesc());
                    ////csp.Log(purchaseTransform.CompareObjDesc(or));


                    purchaseText = purchaseTransform.gameObject.GetComponent<TextMeshProUGUI>();
                    purchaseText.color = ue.Color.black;
                    purchaseText.text = "Purchased Items";

                    break;
                }
            }
        }
        static void ClonePadScrollView(ref ArkUpgrade ins)
        {
            upgradeListScroll = ue.Object.Instantiate(ins.Scroll);

            var u = upgradeListScroll;
            u.Main.content = upgradeListClone.GetComponent<RectTransform>();
            u.Main.viewport = upgradeListViewport.GetComponent<RectTransform>();
            // TODO - Try adding in the rect transforms for the scroll bars
        }
        static void PositionMyMain()
        {
            var rt = MyMainRectTransform;
            var rt2 = OriginalMainRectTransform;

            rt.sizeDelta = new Vector2(rt2.rect.width, rt2.rect.height);
            rt.anchoredPosition = 
                new Vector2(rt2.rect.width + rt2.anchoredPosition.x, rt2.anchoredPosition.y);
        }
        static void Destroy()
        {
            try
            {
                for (int i = 0; i < TransformTargets.Count; i++)
                {
                    if (TransformTargets[i] != null &&
                        TransformTargets[i].gameObject != null)
                    {
                        DestroySafe(TransformTargets[i]?.gameObject);
                    }
                }
            }
            catch (NullReferenceException e)
            {
                Debug.LogError(e.ToString());
            }

            if (upgradeListScroll != null) DestroySafe(upgradeListScroll?.gameObject);
            if (purchaseTransform != null) DestroySafe(purchaseTransform?.gameObject);

            UpgradeButtons.Clear();
            PurchasedButtons.Clear();
        }
        static void UpdatePurchasedText()
        {
            var stacksNeed = 0;

            foreach (var kvp in PurchasedData)
            {
                if (!IgnoreStacks.Any(item => item == kvp.Key))
                {
                    var key = kvp.Key;

                    var amt = PurchasedAmount[key];
                    var max = MaxStacks[key];
                    var need = (int)Math.Ceiling((double)amt / (double)max);
                    stacksNeed += need;
                }
            }

            purchaseText.text = $"Purchased - {ModifiedInventorySize -  stacksNeed} Inventory Slots Left";
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ArkUpgradeButton), "Update")]
        public static void ArkUpgradeButton_Update_Postfix(ArkUpgradeButton __instance)
        {
            if (UpgradeButtons.Any(b => ReferenceEquals(b, __instance)))
            {
                if (__instance.Key == SpecialKeys.NoRareLimit &&
                    PurchasedDataSpecial.Contains(__instance.Key))
                {
                    __instance.button.enabled = false;
                }
                else
                {
                    __instance.button.enabled = true;
                }
            }
        }
        public static int GetPrice(string key)
        {
            if (SpecialKeys.GetSpecialKeys().Any(sk => sk == key))
            {
                return GetPrice(PriceGroup.Special);
            }
            else if (MiscData.Any(item => item.Key == key))
            {
                return GetPrice(PriceGroup.Misc);
            }
            else if (ConsumeableData.Any(item => item.Key == key))
            {
                return GetPrice(PriceGroup.Consumeable);
            }
            else if (PassiveDatas.Any(item => item.Key == key))
            {
                return GetPrice(PriceGroup.Relic);
            }
            else if (EquipData.Any(item => item.Key == key))
            {
                return GetPrice(PriceGroup.Equip);
            }
            else if (ScrollDatas.Any(item => item.Key == key))
            {
                return GetPrice(PriceGroup.Scroll);
            }
            else if (PotionsDatas.Any(item => item.Key == key))
            {
                return GetPrice(PriceGroup.Potion);
            }
            return 0;
        }
        public static int GetPrice(PriceGroup group)
        {
            if (OptionFreeCreditStore) return 0;
            switch (group)
            {
                case PriceGroup.Special: return 1;
                case PriceGroup.Misc: return 1;
                case PriceGroup.Consumeable: return 1;
                case PriceGroup.Relic: return 2;
                case PriceGroup.Equip: return 3;
                case PriceGroup.Scroll: return 1;
                case PriceGroup.Potion: return 1;
                default: return 0;
            }
        }
        public static void RemoveTimeMoney(int amount)
        {
            SaveManager.NowData.TimeMoney -= amount;
            SaveManager.savemanager.Save();
        }
        public static void AddTimeMoney(int amount)
        {
            SaveManager.NowData.TimeMoney += amount;
            SaveManager.savemanager.Save();
        }
    }

}
