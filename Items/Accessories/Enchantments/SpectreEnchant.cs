using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class SpectreEnchant : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spectre Enchantment");
            Tooltip.SetDefault(
                @"'Their lifeforce will be their own undoing'
Magic damage has a chance to spawn damaging orbs
If you crit, you get a burst of healing orbs instead
Summons a Wisp to provide light");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 8;
            item.value = 250000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>(mod).SpectreEffect(hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("FargowiltasSouls:AnySpectreHead");
            
            recipe.AddIngredient(ItemID.SpectreRobe);
            recipe.AddIngredient(ItemID.SpectrePants);
            recipe.AddIngredient(ItemID.SpectreHamaxe);
            recipe.AddIngredient(ItemID.SpectreStaff);
            recipe.AddIngredient(ItemID.UnholyTrident);
            recipe.AddIngredient(ItemID.WispinaBottle);
            
            /*
            both heads
spectre wings
GhastlyCarapace kill unholy trident with thorium
MusicSheetOrgan
Ectoplasmic Butterfly
            */
            
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
