using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.BossWeapons
{
    public class Hungry2 : Hungry
    {
        public override string Texture => "FargowiltasSouls/Projectiles/BossWeapons/Hungry";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hungry");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.minion = false;
            projectile.minionSlots = 1;
            projectile.magic = true;
        }
    }
}