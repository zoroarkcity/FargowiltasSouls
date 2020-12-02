using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Buffs.Masomode;

namespace FargowiltasSouls.Projectiles.BossWeapons
{
    public class DragonFireballBoom : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_612";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fireball");
            Main.projFrames[projectile.type] = Main.projFrames[ProjectileID.SolarWhipSwordExplosion];
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.timeLeft = 600;
            projectile.scale = 2;
            projectile.tileCollide = false;
            //cooldownSlot = 1;
        }
        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > 3)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }

            if (projectile.frame > Main.projFrames[projectile.type])
                projectile.Kill();
        }
    }
}