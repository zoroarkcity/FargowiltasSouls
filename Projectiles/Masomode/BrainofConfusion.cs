using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class BrainofConfusion : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_565";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brain of Confusion");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BrainOfConfusion);
            projectile.scale = 4f;
            aiType = ProjectileID.BrainOfConfusion;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.5f;
        }
    }
}