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
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.width = 48;
            projectile.height = 42;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);

            if (Main.netMode != 1)
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, ProjectileID.DD2OgreSmash, projectile.damage, 4, Main.myPlayer);
        }
    }
}