using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FargowiltasSouls.Items.Accessories.Essences
{
    public class SlingersEssence : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        private readonly Mod fargos = ModLoader.GetMod("Fargowiltas");
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slinger's Essence");

            string tooltip = 
@"'This is only the beginning..'
18% increased throwing damage
5% increased throwing critical chance
5% increased throwing velocity";
            string tooltip_ch =
@"'这才刚刚开始..'
增加18%投掷伤害
增加5%投掷暴击率
增加5%投掷物速度";

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "投手精华");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color?(new Color(85, 5, 230));
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.value = 150000;
            item.rare = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.thrownDamage += 0.18f;
            player.thrownCrit += 5;
            player.thrownVelocity += 0.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            if (Fargowiltas.Instance.ThoriumLoaded)
            {
                recipe.AddIngredient(thorium.ItemType("NinjaEmblem"));
                recipe.AddIngredient(fargos.ItemType("WoodYoyoThrown"));
                recipe.AddIngredient(fargos.ItemType("BloodyMacheteThrown"));
                recipe.AddIngredient(fargos.ItemType("IceBoomerangThrown"));
                recipe.AddIngredient(ItemID.AleThrowingGlove);
                recipe.AddIngredient(thorium.ItemType("EnchantedKnife"));
                recipe.AddIngredient(thorium.ItemType("StarfishSlicer"), 300);
                recipe.AddIngredient(fargos.ItemType("JungleYoyoThrown"));
                recipe.AddIngredient(ItemID.Beenade, 300);
                recipe.AddIngredient(ItemID.BoneGlove);
                recipe.AddIngredient(fargos.ItemType("BlueMoonThrown"));
                recipe.AddIngredient(thorium.ItemType("ChampionsGodHand"));
                recipe.AddIngredient(thorium.ItemType("GaussKnife"));
                recipe.AddIngredient(fargos.ItemType("FlamarangThrown"));
            }
            else
            {
                //no others
                recipe.AddIngredient(fargos.ItemType("WoodYoyoThrown"));
                recipe.AddIngredient(fargos.ItemType("BloodyMacheteThrown"));
                recipe.AddIngredient(fargos.ItemType("IceBoomerangThrown"));
                recipe.AddIngredient(ItemID.AleThrowingGlove);
                recipe.AddIngredient(ItemID.PartyGirlGrenade, 300);
                recipe.AddIngredient(fargos.ItemType("TheMeatballThrown"));
                recipe.AddIngredient(fargos.ItemType("JungleYoyoThrown"));
                recipe.AddIngredient(ItemID.Beenade, 300);
                recipe.AddIngredient(ItemID.BoneGlove);
                recipe.AddIngredient(fargos.ItemType("BlueMoonThrown"));
                recipe.AddIngredient(fargos.ItemType("FlamarangThrown"));
            }

            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
