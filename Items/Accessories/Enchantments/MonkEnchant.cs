using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class MonkEnchant : ModItem
    {
        private int monkTimer;

        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Monk Enchantment");
            Tooltip.SetDefault(
@"'Hours of Meditation have led to this…'
Standing still for 2 seconds grants you a single use immune dash that works in any cardinal direction
Lightning Aura can now crit and strikes faster");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 7;
            item.value = 100000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().MonkEnchant = true;

            if(!player.HasBuff(mod.BuffType("MonkBuff")) && player.GetModPlayer<FargoPlayer>().IsStandingStill)
            {
                monkTimer++;

                if (monkTimer >= 120)
                {
                    player.AddBuff(mod.BuffType("MonkBuff"), 2);
                    monkTimer = 0;
                }
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.MonkBrows);
            recipe.AddIngredient(ItemID.MonkShirt);
            recipe.AddIngredient(ItemID.MonkPants);
            recipe.AddIngredient(ItemID.MonkBelt);
            recipe.AddIngredient(ItemID.DD2LightningAuraT2Popper);
            recipe.AddIngredient(ItemID.MonkStaffT2);
            recipe.AddIngredient(ItemID.DaoofPow);

            //Schmelze (with Thorium)
            //Rocket Fist (with Thorium)

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
