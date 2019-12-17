using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class Bee : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_181";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bee");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.hostile = true;
            projectile.timeLeft = 600;
            projectile.aiStyle = -1;
        }

        public override void AI()
        {
            projectile.spriteDirection = Math.Sign(projectile.velocity.X);

            if ((projectile.wet || projectile.lavaWet) && !projectile.honeyWet) //die in liquids
            {
                projectile.Kill();
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("Infested"), 300);
            target.AddBuff(BuffID.BrokenArmor, 300);
            target.AddBuff(mod.BuffType("Swarming"), 300);
        }
    }
}