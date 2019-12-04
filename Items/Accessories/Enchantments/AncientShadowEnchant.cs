using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class AncientShadowEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Shadow Enchantment");
            Tooltip.SetDefault(
@"''

A Shadow Orb (or 2) will orbit around the player a set distance away. 
Attacking the Shadow Orb will cause it to release a burst of 2-3 homing shadow energy that each deal a percentage of the weapon's damage and inflicts Shadowflame for a short period of time.

After being struck, the Shadow Orb cannot be hit for 3 seconds.

");
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
            player.GetModPlayer<FargoPlayer>().AncientShadowEnchant = true;
        }

        public override void AddRecipes()
        {
            /*ModRecipe recipe = new ModRecipe(mod);
            
            armor 1
            armor 2
            armor 3
            
            shadow enchant

            Ancient Iron Helmet
Ancient Gold Helmet
Ancient Shadow Armor, Ancient Iron Helmet, Ancient Gold Helmet, Shadowflame Hades Dye, Shadowflame Bow



            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();*/
        }
    }
}
