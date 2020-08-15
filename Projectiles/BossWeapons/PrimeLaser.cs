using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.BossWeapons
{
    public class PrimeLaser : ModProjectile 
    {
        public override string Texture => "Terraria/Projectile_389";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prime Laser");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.MiniRetinaLaser);
            aiType = ProjectileID.MiniRetinaLaser;
            projectile.ignoreWater = true;
            projectile.timeLeft = 150;
            projectile.magic = true;
            projectile.penetrate = 1;
        }
    }
}