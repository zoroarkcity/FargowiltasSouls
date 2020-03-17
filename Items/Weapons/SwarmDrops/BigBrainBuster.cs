using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Fargowiltas.Items.Tiles;

namespace FargowiltasSouls.Items.Weapons.SwarmDrops
{
    public class BigBrainBuster : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Big Brain Buster");
            Tooltip.SetDefault("'An old foe beaten into submission..'\nNeeds 2 minion slots\nMinions do reduced damage when not holding a summon weapon");
            ItemID.Sets.StaffMinionSlotsRequired[item.type] = 2;
        }

        public override void SetDefaults()
        {
            item.damage = 380;
            item.summon = true;
            item.mana = 10;
            item.width = 26;
            item.height = 28;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 3;
            item.rare = 11;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("BigBrainProj");
            item.shootSpeed = 10f;
            item.buffType = mod.BuffType("BigBrainMinion");
            item.buffTime = 3600;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 10);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(item.buffType, 2);
            Vector2 spawnPos = Main.MouseWorld;
            Projectile.NewProjectile(spawnPos, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, -1);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BrainStaff");
            recipe.AddIngredient(ModLoader.GetMod("Fargowiltas").ItemType("EnergizerBrain"));
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddIngredient(null, "LunarCrystal", 5);

            recipe.AddTile(ModContent.TileType<CrucibleCosmosSheet>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
