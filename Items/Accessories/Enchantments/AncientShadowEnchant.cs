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
@"'Archaic, yet functional'
Two Shadow Orbs will orbit around you
Attacking a Shadow Orb will cause it to release a burst of homing shadow energy
After being struck, a Shadow Orb cannot be hit for 5 seconds
Your attacks may inflict Darkness
Summons a pet Eater of Souls and Shadow Orb");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 5;
            item.value = 100000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            modPlayer.AncientShadowEffect();
            modPlayer.ShadowEffect(hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.AncientShadowHelmet);
            recipe.AddIngredient(ItemID.AncientShadowScalemail);
            recipe.AddIngredient(ItemID.AncientShadowGreaves);
            recipe.AddIngredient(ItemID.AncientNecroHelmet);
            recipe.AddIngredient(ItemID.AncientGoldHelmet);
            recipe.AddIngredient(null, "ShadowEnchant");
            recipe.AddIngredient(ItemID.ShadowFlameBow);

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
