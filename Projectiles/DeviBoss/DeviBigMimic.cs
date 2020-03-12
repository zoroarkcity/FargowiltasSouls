using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.DeviBoss
{
    public class DeviBigMimic : DeviMimic
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Biome Mimic");
            Main.projFrames[projectile.type] = 8;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.width = 48;
            projectile.height = 42;
        }

        public override void Kill(int timeLeft)
        {
            projectile.position = projectile.Center;
            projectile.width = projectile.height = 200;
            projectile.Center = projectile.position;

            base.Kill(timeLeft);
        }
    }
}