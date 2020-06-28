using FargowiltasSouls.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Patreon.Sasha
{
    public class Bubble : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Bubble);
            aiType = ProjectileID.Bubble;

            projectile.penetrate = 4;
        }

        public override void Kill(int timeLeft)
        {
            FargoGlobalProjectile.XWay(4, projectile.position, mod.ProjectileType("WaterStream"), 5, projectile.damage / 2, projectile.knockBack / 2);
        }
    }
}