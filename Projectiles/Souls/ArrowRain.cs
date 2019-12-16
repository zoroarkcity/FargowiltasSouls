using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Souls
{
    public class ArrowRain : ModProjectile
    {
        private bool launchArrow = true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arrow Rain");
        }

        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 150;
        }

        public override void AI()
        {
            if (projectile.timeLeft > 120)
            {
                return;
            }

            if (launchArrow)
            {
                Vector2 position = new Vector2(projectile.Center.X + Main.rand.Next(-100, 100), projectile.Center.Y + Main.rand.Next(-50, 50));

                float direction = projectile.ai[1];
                Vector2 velocity;

                if (direction == 1)
                {
                    velocity = new Vector2(Main.rand.NextFloat(0, 2), Main.rand.NextFloat(12, 15));
                }
                else
                {
                    velocity = new Vector2(Main.rand.NextFloat(-2, 0), Main.rand.NextFloat(12, 15));
                }

                int p = Projectile.NewProjectile(position, velocity, (int)projectile.ai[0], projectile.damage, 0, projectile.owner);
                Main.projectile[p].noDropItem = true;

                launchArrow = false;
            }
            else
            {
                launchArrow = true;
            }
        }
    }
}