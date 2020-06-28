using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class HuntressEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Huntress Enchantment");
            Tooltip.SetDefault(
@"'The Hunt is On'
Double tap down to create a localized rain of arrows at the cursor's position for a few seconds
This has a cooldown of 15 seconds
Explosive Traps recharge faster and oil enemies
Set oiled enemies on fire for extra damage");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 8;
            item.value = 200000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().HuntressEffect();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.HuntressWig);
            recipe.AddIngredient(ItemID.HuntressJerkin);
            recipe.AddIngredient(ItemID.HuntressPants);
            recipe.AddIngredient(ItemID.HuntressBuckler);
            recipe.AddIngredient(ItemID.DD2ExplosiveTrapT2Popper);
            //tendon bow
            recipe.AddIngredient(ItemID.DaedalusStormbow);
            //shadiwflame bow
            recipe.AddIngredient(ItemID.DD2PhoenixBow);
            //dog pet

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
