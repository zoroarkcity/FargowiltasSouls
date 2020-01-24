using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            DisplayName.SetDefault("Purple Laser");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PurpleLaser);
            aiType = ProjectileID.PurpleLaser;
            projectile.tileCollide = false;
        }
    }
}