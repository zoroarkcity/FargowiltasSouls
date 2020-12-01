using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Minions
{
    public class DarkStarHomingFriendly : Masomode.DarkStar
    {
        public override string Texture => "Terraria/Projectile_12";

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.timeLeft = 180;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.minion = true;
        }

        public override void AI()
        {
            base.AI();

            int ai0 = (int)projectile.ai[0];
            if (ai0 > -1 && ai0 < Main.maxNPCs && Main.npc[ai0].active && Main.npc[ai0].CanBeChasedBy())
            {
                if (++projectile.ai[1] < 60)
                {
                    projectile.velocity *= 1.065f;

                    float rotation = projectile.velocity.ToRotation();
                    Vector2 vel = Main.npc[ai0].Center - projectile.Center;
                    float targetAngle = vel.ToRotation();
                    projectile.velocity = new Vector2(projectile.velocity.Length(), 0f).RotatedBy(rotation.AngleLerp(targetAngle, 0.05f));
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 6;
        }
    }
}