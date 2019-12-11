using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

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
This has a cooldown of ech seconds
Explosive Traps recharge faster and oil enemies
Set oiled enemies on fire for extra damage");
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
            player.GetModPlayer<FargoPlayer>().HuntressEnchant = true;


            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.HuntressAbility) && (player.controlDown && player.releaseDown))
            {
                if (player.doubleTapCardinalTimer[0] > 0 && player.doubleTapCardinalTimer[0] != 15)
                {
                    Vector2 mouse = Main.MouseWorld;

                    if(huntressCD == 0)
                    {
                        //find arrow type to use, for red riding only
                        int arrowType = ProjectileID.WoodenArrow;



                        Projectile.NewProjectile(mouse.X, mouse.Y - 100, 0f, 0f, mod.ProjectileType("ArrowRain"), 25, 0f, player.whoAmI, arrowType);
                        //proj spawns arrows all around it until it dies
                    }


                }
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.HuntressWig);
            recipe.AddIngredient(ItemID.HuntressJerkin);
            recipe.AddIngredient(ItemID.HuntressPants);
            recipe.AddIngredient(ItemID.HuntressBuckler);
            recipe.AddIngredient(ItemID.DD2ExplosiveTrapT2Popper);
            recipe.AddIngredient(ItemID.DD2PhoenixBow);
            recipe.AddIngredient(ItemID.DaedalusStormbow);


            /*  

          Cinder String (with Thorium)
          Chlorophyte Shotbow*/


            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
