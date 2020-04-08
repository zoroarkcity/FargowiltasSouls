using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Armor;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Fishing;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Calamity
{
    public class WulfrumEnchant : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("CalamityMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wulfrum Enchantment");
            Tooltip.SetDefault(
@"'Not to be confused with Tungsten Enchantment…'
+5 defense when below 50% life
Effects of Trinket of Chi");
            DisplayName.AddTranslation(GameCulture.Chinese, "钨钢魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'别和钨金魔石搞混了...'
生命值低于50%时, 增加5点防御
拥有灵魂浮雕, 掠袭者护符和气之挂坠的效果");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 1;
            item.value = 40000;
            item.defense = 3;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(129, 168, 109);
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            if (player.statLife <= (int)(player.statLifeMax2 * 0.5))
            {
                player.statDefense += 5;
            }
            //trinket of chi
            calamity.GetItem("TrinketofChi").UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddRecipeGroup("FargowiltasSouls:AnyWulfrumHelmet");
            recipe.AddIngredient(ModContent.ItemType<WulfrumArmor>());
            recipe.AddIngredient(ModContent.ItemType<WulfrumLeggings>());
            recipe.AddIngredient(ModContent.ItemType<TrinketofChi>());
            recipe.AddIngredient(ModContent.ItemType<Spadefish>());
            recipe.AddIngredient(ModContent.ItemType<MandibleBow>());
            recipe.AddIngredient(ModContent.ItemType<BurntSienna>());
            recipe.AddIngredient(ModContent.ItemType<MarniteSpear>());
            recipe.AddIngredient(ModContent.ItemType<SparkSpreader>());
            recipe.AddIngredient(ModContent.ItemType<Pumpler>());

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
