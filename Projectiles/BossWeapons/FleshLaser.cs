using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.BossWeapons
{
    public class FleshLaser : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_88";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blue Laser");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PurpleLaser);
            aiType = ProjectileID.PurpleLaser;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Cyan;
        }
    }
}