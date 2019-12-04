using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class EskimoEnchant : ModItem
    {
    public override bool Autoload(ref string name)
        {
            return false;
        }
        
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eskimo Enchantment");
            Tooltip.SetDefault(
@"'It's Burning Cold Outside'

you have a small area around you that frostburns enemies and slows projectiles
if you wear both it applies frozen to enemies and projectiles for like .5 seconds upon entering



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
            player.GetModPlayer<FargoPlayer>().EskimoEnchant = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.EskimoHood);
            recipe.AddIngredient(ItemID.EskimoCoat);
            recipe.AddIngredient(ItemID.EskimoPants);
            //recipe.AddIngredient(ItemID.IceRod);
            recipe.AddIngredient(ItemID.FrostMinnow);
            recipe.AddIngredient(ItemID.AtlanticCod);
            recipe.AddIngredient(ItemID.MarshmallowonaStick);
            
            //goes into frost along with pink eskimo

            //hand warmer
            //ice skates
            //xmas painting
            //present drops


            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
