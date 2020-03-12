using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.DeviBoss
{
    public class DeviLightBall : Masomode.LightBall
    {
        public override string Texture => "FargowiltasSouls/Masomode/LightBall";

        public override void SetDefaults()
        {
            base.SetDefaults();

            projectile.timeLeft = 240;
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
            
            if (projectile.ai[0] == 0f && Main.netMode != 1) //split
            {
                for (int i = 1; i <= 4; i++)
                    Projectile.NewProjectile(projectile.Center, 2f * Vector2.Normalize(projectile.velocity).RotatedBy(MathHelper.ToRadians(15) * i * Math.Sign(projectile.ai[1])),
                        mod.ProjectileType("DeviLightBall"), projectile.damage, projectile.knockBack, projectile.owner, 0.15f);
            }
        }
    }
}