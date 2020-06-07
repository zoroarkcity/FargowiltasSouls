using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class Bee : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_566";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bee");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.hostile = true;
            projectile.timeLeft = 600;
            projectile.aiStyle = -1;
        }

        public override void AI()
        {
            projectile.spriteDirection = Math.Sign(projectile.velocity.X);
            projectile.rotation = projectile.velocity.X * .1f;

            if (++projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 3)
                    projectile.frame = 0;
            }

            if ((projectile.wet || projectile.lavaWet) && !projectile.honeyWet) //die in liquids
            {
                projectile.Kill();
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("Infested"), 300);
            target.AddBuff(mod.BuffType("Swarming"), 600);
        }
    }
}