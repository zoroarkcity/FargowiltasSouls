using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.JungleMimic
{
	public class MahogunyLeafProjectile : ModProjectile
	{
		public override void SetStaticDefaults(){
			Main.projFrames[projectile.type] = 4;
		}
        public override void SetDefaults()
        {
             aiType = 14;
            projectile.width = 5;
            projectile.height = 9;
            projectile.friendly = true;
             projectile.hostile = false;
             projectile.ranged = true;
             projectile.penetrate = 1;
             projectile.ignoreWater = false;
             projectile.tileCollide = true;
        }
        public override void AI(){
        projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
        if (++projectile.frameCounter >= 5)
                 {
	            projectile.frameCounter = 0;
	            projectile.frame = ++projectile.frame % Main.projFrames[projectile.type];
                }
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Grass, projectile.position);
        }
    }
}
