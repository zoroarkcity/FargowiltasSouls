using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.NPCs;

namespace FargowiltasSouls.Projectiles.Champions
{
    public class CosmosInvader : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_539";

        private bool spawned;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Invader");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.aiStyle = -1;
            cooldownSlot = 1;
        }

        public override bool PreAI()
        {
            if (!spawned)
            {
                spawned = true;
                projectile.frame = Main.rand.Next(4);

                projectile.timeLeft = (int)projectile.ai[0];

                Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 7, 1f, 0.0f);
                for (int index1 = 0; index1 < 4; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                    Main.dust[index2].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.14159274101257) * (float)Main.rand.NextDouble() * (float)projectile.width / 2f;
                }
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 176, 0.0f, 0.0f, 200, new Color(), 3.7f);
                    Main.dust[index2].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.14159274101257) * (float)Main.rand.NextDouble() * (float)projectile.width / 2f;
                    Main.dust[index2].noGravity = true;
                    Dust dust = Main.dust[index2];
                    dust.velocity = dust.velocity * 3f;
                }
                for (int index1 = 0; index1 < 20; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 180, 0.0f, 0.0f, 0, new Color(), 2.7f);
                    Main.dust[index2].position = projectile.Center + Vector2.UnitX.RotatedByRandom(3.14159274101257).RotatedBy((double)projectile.velocity.ToRotation(), new Vector2()) * (float)projectile.width / 2f;
                    Main.dust[index2].noGravity = true;
                    Dust dust = Main.dust[index2];
                    dust.velocity = dust.velocity * 3f;
                }
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0.0f, 0.0f, 0, new Color(), 1.5f);
                    Main.dust[index2].position = projectile.Center + Vector2.UnitX.RotatedByRandom(3.14159274101257).RotatedBy((double)projectile.velocity.ToRotation(), new Vector2()) * (float)projectile.width / 2f;
                    Main.dust[index2].noGravity = true;
                    Dust dust = Main.dust[index2];
                    dust.velocity = dust.velocity * 3f;
                }
            }

            if (Main.rand.Next(2) == 0)
            {
                int index = Dust.NewDust(projectile.position, projectile.width, projectile.height, 180, 0.0f, 0.0f, 100, new Color(), 1f);
                Dust dust1 = Main.dust[index];
                dust1.scale = dust1.scale + Main.rand.Next(50) * 0.01f;
                Main.dust[index].noGravity = true;
                Dust dust2 = Main.dust[index];
                dust2.velocity = dust2.velocity * 0.1f;
                Main.dust[index].fadeIn = Main.rand.NextFloat() * 1.5f;
            }
            if (Main.rand.Next(3) == 0)
            {
                int index = Dust.NewDust(projectile.position, projectile.width, projectile.height, 176, 0.0f, 0.0f, 100, new Color(), 1f);
                Dust dust1 = Main.dust[index];
                dust1.scale = dust1.scale + 0.3f + Main.rand.Next(50) * 0.01f;
                Main.dust[index].noGravity = true;
                Dust dust2 = Main.dust[index];
                dust2.velocity = dust2.velocity * 0.1f;
                Main.dust[index].fadeIn = Main.rand.NextFloat() * 1.5f;
            }

            return true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + 1.570796f;

            int num4 = projectile.frameCounter + 1;
            projectile.frameCounter = num4;
            if (num4 >= 2)
            {
                projectile.frameCounter = 0;
                int num5 = projectile.frame + 1;
                projectile.frame = num5;
                if (num5 >= Main.projFrames[projectile.type])
                    projectile.frame = 0;
            }
        }

        public override void Kill(int timeLeft) //vanilla explosion code echhhhhhhhhhh
        {
            projectile.position = projectile.Center;
            projectile.width = projectile.height = 80;
            projectile.Center = projectile.position;
            Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 7, 1f, 0.0f);
            for (int index1 = 0; index1 < 2; ++index1)
            {
                int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0.0f, 0.0f, 100, new Color(), 1.5f);
                Main.dust[index2].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.14159274101257) * (float)Main.rand.NextDouble() * (float)projectile.width / 2f;
            }
            for (int index1 = 0; index1 < 5; ++index1)
            {
                int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 176, 0.0f, 0.0f, 200, new Color(), 3.7f);
                Main.dust[index2].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.14159274101257) * (float)Main.rand.NextDouble() * (float)projectile.width / 2f;
                Main.dust[index2].noGravity = true;
                Dust dust = Main.dust[index2];
                dust.velocity = dust.velocity * 3f;
            }
            for (int index1 = 0; index1 < 5; ++index1)
            {
                int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 180, 0.0f, 0.0f, 0, new Color(), 2.7f);
                Main.dust[index2].position = projectile.Center + Vector2.UnitX.RotatedByRandom(3.14159274101257).RotatedBy((double)projectile.velocity.ToRotation(), new Vector2()) * (float)projectile.width / 2f;
                Main.dust[index2].noGravity = true;
                Dust dust = Main.dust[index2];
                dust.velocity = dust.velocity * 3f;
            }
            for (int index1 = 0; index1 < 2; ++index1)
            {
                int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0.0f, 0.0f, 0, new Color(), 1.5f);
                Main.dust[index2].position = projectile.Center + Vector2.UnitX.RotatedByRandom(3.14159274101257).RotatedBy((double)projectile.velocity.ToRotation(), new Vector2()) * (float)projectile.width / 2f;
                Main.dust[index2].noGravity = true;
                Dust dust = Main.dust[index2];
                dust.velocity = dust.velocity * 3f;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 180);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * projectile.Opacity;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), projectile.GetAlpha(lightColor), projectile.rotation, origin2, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}