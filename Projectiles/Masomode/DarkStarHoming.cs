using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class DarkStarHoming : DarkStar
    {
        public override string Texture => "Terraria/Projectile_12";

        public override void AI()
        {
            base.AI();

            if (projectile.ai[1] == 0)
            {
                float rotation = projectile.velocity.ToRotation();
                Vector2 vel = Main.player[(int)projectile.ai[0]].Center - projectile.Center;
                float targetAngle = vel.ToRotation();
                projectile.velocity = new Vector2(projectile.velocity.Length(), 0f).RotatedBy(rotation.AngleLerp(targetAngle, 0.2f));

                if (vel.Length() < 300 || !Main.player[(int)projectile.ai[0]].active || Main.player[(int)projectile.ai[0]].dead || Main.player[(int)projectile.ai[0]].ghost)
                {
                    projectile.ai[1] = 1;
                    projectile.netUpdate = true;
                    projectile.timeLeft = 180;
                    projectile.velocity.Normalize();
                }
            }
            else if (projectile.ai[1] > 0)
            {
                if (++projectile.ai[1] < 100)
                {
                    projectile.velocity *= 1.035f;

                    float rotation = projectile.velocity.ToRotation();
                    Vector2 vel = Main.player[(int)projectile.ai[0]].Center - projectile.Center;
                    float targetAngle = vel.ToRotation();
                    projectile.velocity = new Vector2(projectile.velocity.Length(), 0f).RotatedBy(rotation.AngleLerp(targetAngle, 0.035f));
                }
            }
            else //ai1 below 0 rn
            {
                projectile.ai[1]++;
            }
        }
    }
}