using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class CursedFireballHoming : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_96";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cursed Flame");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.alpha = 100;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft = 600;
            projectile.scale = 1.3f;
        }

        public override void AI()
        {
            //Lighting.AddLight(projectile.Center, 0.35f * 0.8f, 0.8f, 0f);

            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = 1;
                Main.PlaySound(SoundID.Item20, projectile.Center);
            }

            if (Main.rand.Next(3) == 0)
            {
                int index = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 75, projectile.velocity.X, projectile.velocity.Y, 100, default, 3f * projectile.scale);
                Main.dust[index].noGravity = true;
            }

            if (!(projectile.ai[0] > -1 && projectile.ai[0] < Main.maxPlayers))
            {
                projectile.Kill();
                return;
            }
            
            if (projectile.localAI[1]++ == 60)
            {
                projectile.velocity = Vector2.Zero;
            }
            else if (projectile.localAI[1] == 120 + projectile.ai[1]) //shoot at player much faster
            {
                projectile.velocity = projectile.DirectionTo(Main.player[(int)projectile.ai[0]].Center) * 16f;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            for (int index1 = 0; index1 < 20; ++index1)
            {
                int index2 = Dust.NewDust(new Vector2((float)projectile.position.X, (float)projectile.position.Y), projectile.width, projectile.height, 75, (float)(-projectile.velocity.X * 0.200000002980232), (float)(-projectile.velocity.Y * 0.200000002980232), 100, default, 2f * projectile.scale);
                Main.dust[index2].noGravity = true;
                Dust dust1 = Main.dust[index2];
                dust1.velocity *= 2f;
                int index3 = Dust.NewDust(new Vector2((float)projectile.position.X, (float)projectile.position.Y), projectile.width, projectile.height, 75, (float)(-projectile.velocity.X * 0.200000002980232), (float)(-projectile.velocity.Y * 0.200000002980232), 100, default, 1f * projectile.scale);
                Dust dust2 = Main.dust[index3];
                dust2.velocity *= 2f;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * projectile.Opacity;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(6) == 0)
                target.AddBuff(39, 480, true);
            else if (Main.rand.Next(4) == 0)
                target.AddBuff(39, 300, true);
            else if (Main.rand.Next(2) == 0)
                target.AddBuff(39, 180, true);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = lightColor;
            color26 = projectile.GetAlpha(color26);

            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), projectile.GetAlpha(lightColor), projectile.rotation, origin2, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}