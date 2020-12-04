using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Minions
{
    public class DarkStarFriendly : Masomode.DarkStar
    {
        public override string Texture => "Terraria/Projectile_12";

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.timeLeft = 180;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            projectile.rotation = projectile.rotation + (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.01f * projectile.direction;
            projectile.soundDelay = 0;
            projectile.velocity *= 1.02f;
        }

        public override bool PreKill(int timeLeft)
        {
            return false;
        }
    }
}