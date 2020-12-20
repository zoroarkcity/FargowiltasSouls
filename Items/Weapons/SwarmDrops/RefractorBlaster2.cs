using FargowiltasSouls.Projectiles.BossWeapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Weapons.SwarmDrops
{
    public class RefractorBlaster2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Diffractor Blaster");
            Tooltip.SetDefault("'The reward for slaughtering many...'");
            DisplayName.AddTranslation(GameCulture.Chinese, "暗星炮");
            Tooltip.AddTranslation(GameCulture.Chinese, "'由一个被击败的敌人的武器改装而来..'");

            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(3, 7));
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.LaserRifle);
            item.width = 98;
            item.height = 38;
            item.damage = 400;
            item.channel = true;
            item.useTime = 24;
            item.useAnimation = 24;
            item.reuseDelay = 20;
            item.shootSpeed = 15f;
            item.UseSound = SoundID.Item15;
            item.value = 100000;
            item.rare = ItemRarityID.Purple;
            item.shoot = mod.ProjectileType("RefractorBlaster2Held");
            item.noUseGraphic = true;
            item.mana = 18;
            item.knockBack = 0.5f;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture2D = mod.GetTexture("Items/Weapons/SwarmDrops/RefractorBlaster2Glow");
            Item thisitem = Main.item[whoAmI];
            int height = texture2D.Height / 7;
            int width = texture2D.Width;
            int frame = height * Main.itemFrame[whoAmI];
            SpriteEffects flipdirection = thisitem.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle Origin = new Rectangle(0, frame, width, height);
            spriteBatch.Draw(texture2D, thisitem.Center - Main.screenPosition, Origin, Color.White, rotation, Origin.Size()/2, scale, flipdirection, 0f);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "RefractorBlaster");
            recipe.AddIngredient(null, "MutantScale", 10);
            recipe.AddIngredient(ModLoader.GetMod("Fargowiltas").ItemType("EnergizerPrime"));
            recipe.AddTile(ModLoader.GetMod("Fargowiltas").TileType("CrucibleCosmosSheet"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
