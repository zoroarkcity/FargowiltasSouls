using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.BossWeapons
{
    internal class DicerSpray : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_484";

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.SeedlerThorn);
            projectile.penetrate = 2;
        }
    }
}