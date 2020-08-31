using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ID;

namespace FargowiltasSouls.Items.Accessories.Souls
{
    public class ArchWizardsSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arch Wizard's Soul");

            string tooltip =
@"'Arcane to the core'
30% increased magic damage
20% increased spell casting speed
15% increased magic crit chance
Increases your maximum mana by 200
Effects of Celestial Cuffs and Mana Flower";
            string tooltip_ch =
@"'神秘核心'
增加30%魔法伤害
增加20%施法速度
增加15%魔法暴击率
增加200最大法力值
";

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "巫师之魂");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.value = 1000000;
            item.rare = 11;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color?(new Color(255, 83, 255));
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().MagicSoul = true;
            player.magicDamage += .3f;
            player.magicCrit += 15;
            player.statManaMax2 += 200;
            //accessorys
            player.manaFlower = true;
            //add mana cloak
            player.manaMagnet = true;
            player.magicCuffs = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(null, "ApprenticesEssence");
            recipe.AddIngredient(ItemID.ManaFlower); //mana cloak

            recipe.AddIngredient(ItemID.CelestialCuffs);
            recipe.AddIngredient(ItemID.CelestialEmblem);
            recipe.AddIngredient(ItemID.MedusaHead);
            //blood thorn
            //magnet sphere
            recipe.AddIngredient(ItemID.RainbowGun);

            recipe.AddIngredient(ItemID.ApprenticeStaffT3);
            //stellar tune
            recipe.AddIngredient(ItemID.RazorbladeTyphoon);
            
            recipe.AddIngredient(ItemID.BlizzardStaff);
            recipe.AddIngredient(ItemID.LaserMachinegun);
            recipe.AddIngredient(ItemID.LastPrism);

            recipe.AddTile(ModLoader.GetMod("Fargowiltas").TileType("CrucibleCosmosSheet"));

            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
