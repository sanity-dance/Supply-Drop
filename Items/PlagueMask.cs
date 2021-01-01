﻿using System.Collections.ObjectModel;
using System.Collections.Generic;
using R2API;
using RoR2;
using UnityEngine;
using TILER2;
using SupplyDrop.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Linq;

namespace SupplyDrop.Items
{
    class PlagueMask : Item_V2<PlagueMask>
    {
        public override string displayName => "Vintage Plague Mask";

        public override ItemTier itemTier => ItemTier.Tier2;

        public override ReadOnlyCollection<ItemTag> itemTags => new ReadOnlyCollection<ItemTag>(new[] { ItemTag.Utility });
        protected override string GetNameString(string langid = null) => displayName;

        protected override string GetPickupString(string langID = null) => "Heal extra the more damage items you have.";

        protected override string GetDescString(string langID = null) => "All <style=cIsHealing>healing</style> is increased by <style=cIsHealing>2%</style> <style=cStack>(+2% per stack)</style> for every <style=cIsDamage>damage item" +
            "</style> you possess.";
            
        protected override string GetLoreString(string landID = null) => "Uh oh, no lore here. Try again later.";

        private static List<CharacterBody> Playername = new List<CharacterBody>();
        public static GameObject ItemBodyModelPrefab;
        private List<int> indiciiToCheck;
        private int damageItemCount = 0;


        public PlagueMask()
        {
            modelResourcePath = "@SupplyDrop:Assets/Main/Models/Prefabs/WireBundle.prefab";
            iconResourcePath = "@SupplyDrop:Assets/Main/Textures/Icons/QSGenIcon.png";
        }

        public override void SetupAttributes()
        {
            if (ItemBodyModelPrefab == null)
            {
                ItemBodyModelPrefab = Resources.Load<GameObject>(modelResourcePath);
                displayRules = GenerateItemDisplayRules();
            }

            base.SetupAttributes();
        }

        private static ItemDisplayRuleDict GenerateItemDisplayRules()
        {

            ItemBodyModelPrefab.AddComponent<ItemDisplay>();
            ItemBodyModelPrefab.GetComponent<ItemDisplay>().rendererInfos = ItemHelpers.ItemDisplaySetup(ItemBodyModelPrefab);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Head",
                    localPos = new Vector3(-0.025f, 0.05f, -.23f),
                    localAngles = new Vector3(-138f, 0f, 0f),
                    localScale = new Vector3(.2f, .2f, .2f)

        }
            });
            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Head",
                    localPos = new Vector3(0f, 0.05f, -0.2f),
                    localAngles = new Vector3(-138f, 0f, 0f),
                    localScale = new Vector3(0.125f, 0.125f, 0.125f)
        }
            });
            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Head",
                    localPos = new Vector3(2.37f, 2.3f, -0.4f),
                    localAngles = new Vector3(-30f, 90f, 180f),
                    localScale = new Vector3(2f, 2f, 2f)
                }
            });
            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Head",
                    localPos = new Vector3(0f, 0f, -0.3f),
                    localAngles = new Vector3(-138f, 0f, 0f),
                    localScale = new Vector3(.2f, .2f, .2f)
                }
            });
            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Head",
                    localPos = new Vector3(-0.02f, -0.05f, -0.23f),
                    localAngles = new Vector3(-138f, 0f, 0f),
                    localScale = new Vector3(0.14f, 0.14f, 0.14f)
                }
            });
            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Head",
                    localPos = new Vector3(0.025f, 0.15f, -0.28f),
                    localAngles = new Vector3(-149f, 0f, 0f),
                    localScale = new Vector3(0.17f, 0.17f, 0.17f)
                }
            });
            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "FlowerBase",
                    localPos = new Vector3(0f, -1f, -0.9f),
                    localAngles = new Vector3(-138f, 0f, 0f),
                    localScale = new Vector3(0.5f, 0.5f, 0.5f)
                }
            });
            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Head",
                    localPos = new Vector3(-0.02f, 0.19f, -0.285f),
                    localAngles = new Vector3(-149f, 0f, 0f),
                    localScale = new Vector3(.2f, .2f, .2f)
                }
            });
            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Head",
                    localPos = new Vector3(0f, 0.7f, -3f),
                    localAngles = new Vector3(-138f, 0f, 0f),
                    localScale = new Vector3(2f, 2f, 2f)
                }
            });
            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Head",
                    localPos = new Vector3(0f, -0.1f, -0.28f),
                    localAngles = new Vector3(-138f, 0f, 0f),
                    localScale = new Vector3(0.15f, 0.15f, 0.15f)
                }
            });
            return rules;
        }

        public override void Install()
        {
            base.Install();

            runDuringRUNStart();
            IL.RoR2.HealthComponent.Heal += IL_AddBonusHeal;
        }

        public override void Uninstall()
        {
            base.Uninstall();

            IL.RoR2.HealthComponent.Heal -= IL_AddBonusHeal;
        }
        private void runDuringRUNStart()
        {
            ItemIndex[] indiciiToCheck;
            indiciiToCheck = ItemCatalog.allItems.Where(x => ItemCatalog.GetItemDef(x).ContainsTag(ItemTag.Damage)).ToArray();
        }

        public int getTotalDamageItems(Inventory inventory)
        {
            foreach (ItemIndex x in indiciiToCheck)
            {
                damageItemCount += inventory.GetItemCount(x);
            }
            return damageItemCount;
        }

        private void IL_AddBonusHeal(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            bool found;

            found = c.TryGotoNext(
                x => x.MatchLdarg(1),
                x => x.MatchStloc(2)
                );
            
            if (found)
            {
                c.Emit(OpCodes.Ldarg, 1);
                c.Emit(OpCodes.Stloc, 2);
                c.EmitDelegate<Func<float, HealthComponent, float>>((origHeal, component) =>
                {
                    float newHeal;
                    if (component.GetComponent<CharacterBody>() is CharacterBody body)
                    {
                        if (GetCount(body) > 0)
                        {
                            newHeal = origHeal + (origHeal * (.02f * damageItemCount * GetCount(body)));
                        }
                        else
                        {
                            newHeal = origHeal;
                        }
                        return newHeal;
                    }
                    else
                    {
                        newHeal = origHeal;
                    }
                    return newHeal;
                }
                );
                c.Emit(OpCodes.Stloc, 2);
            }            
        }   
    }
}
