using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using FargowiltasSouls.Projectiles.BossWeapons;
using Terraria;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Weapons.SwarmDrops
{
    public class DragonBreath2 : BossDrops.DragonBreath
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dragon's Breath EX");
            Tooltip.SetDefault(@"Uses gel for ammo
66% chance to not consume ammo
'The reward for slaughtering many..'");
        }

        public override void SetDefaults()
        {
            item.damage = 210;
            item.ranged = true;
            item.width = 24;
            item.height = 24;
            item.useTime = 45;
            item.useAnimation = 45;
            item.channel = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1.5f;
            item.UseSound = SoundID.DD2_BetsyFlameBreath;
            item.useAmmo = AmmoID.Gel;
            //Item.staff[item.type] = true;
            item.value = Item.sellPrice(0, 15);
            item.rare = 11;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<DragonBreathProj2>();
            item.shootSpeed = 35f;
            item.noUseGraphic = false;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.Next(3) == 0;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DragonBreath");
            recipe.AddIngredient(null, "MutantScale", 10);
            recipe.AddIngredient(ModLoader.GetMod("Fargowiltas").ItemType("EnergizerBetsy"));
            recipe.AddTile(ModLoader.GetMod("Fargowiltas").TileType("CrucibleCosmosSheet"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}