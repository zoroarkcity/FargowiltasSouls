using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class CrystalAssassinEnchant : ModItem
    {
        public override bool Autoload(ref string name)
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Assassin Enchantment");

            string tooltip =
@"Effects of Volatile Gel
''";

            Tooltip.SetDefault(tooltip);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(231, 178, 28); //change e
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 5;
            item.value = 150000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.GetModPlayer<FargoPlayer>().ForbiddenEffect(); //effect tele on party girl bathwater, when tele slashes through enemies
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AncientBattleArmorHat); //head
            recipe.AddIngredient(ItemID.AncientBattleArmorShirt); //body
            recipe.AddIngredient(ItemID.AncientBattleArmorPants); //legs
            //ninja enchant
            //volatile gel
            //magic dagger
            //flying knife
            //party gitl bathwater
            //hook of dissonance
            //qs mount

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
