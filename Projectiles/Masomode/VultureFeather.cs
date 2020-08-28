using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class VultureFeather : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vulture Feather");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.HarpyFeather);
            projectile.aiStyle = 1;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            cooldownSlot = 1; // do we need this?
        }
    }
}
