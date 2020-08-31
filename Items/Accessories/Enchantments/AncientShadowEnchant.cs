using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class AncientShadowEnchant : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Shadow Enchantment");
            Tooltip.SetDefault(
@"Two Shadow Orbs will orbit around you
Attacking a Shadow Orb will cause it to release a burst of homing shadow energy
Your attacks may inflict Darkness
Summons a pet Eater of Souls and Shadow Orb
'Archaic, yet functional'");
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
            recipe.AddIngredient(ItemID.ShadowFlameKnife);
            //recipe.AddIngredient(ItemID.ShadowFlameHexDoll);
            //dart rifle
            //toxicarp

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
