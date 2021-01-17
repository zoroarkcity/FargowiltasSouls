using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles
{
    public class FakeTornado : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("fake tornado");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.alpha = 0;
        }

        public override bool CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            projectile.velocity = Vector2.UnitY;
            projectile.position -= projectile.velocity;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float halfheight = 220;
            float density = 50f;
            for (float i = 0; i < (int)density; i++)
            {
                Color color = new Color(212, 192, 100);
                color.A /= 2;
                float lerpamount = (Math.Abs(density / 2 - i) > ((density/2) * 0.6f)) ? Math.Abs(density / 2 - i)/(density/2) : 0f; //if too low or too high up, start making it transparent
                color = Color.Lerp(color, Color.Transparent, lerpamount);
                Texture2D texture = Main.projectileTexture[projectile.type];
                Vector2 offset = Vector2.SmoothStep(projectile.Center + Vector2.Normalize(projectile.velocity) * halfheight, projectile.Center - Vector2.Normalize(projectile.velocity) * halfheight, i / density);
                float scale = MathHelper.Lerp(projectile.scale * 0.8f, projectile.scale * 2.5f, i / density);
                Main.spriteBatch.Draw(texture, offset - Main.screenPosition,
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    projectile.GetAlpha(color),
                    i / 6f - Main.GlobalTime * 5f + projectile.rotation,
                    texture.Size() / 2,
                    scale,
                    SpriteEffects.None,
                    0);
            }
            return false;
        }
    }
}