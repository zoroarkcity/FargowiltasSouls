using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles
{
    internal class TopHatSquirrelProj : ModProjectile
    {
        public int Counter = 1;

        public override string Texture => "FargowiltasSouls/Items/Weapons/Misc/TophatSquirrelWeapon";

        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 15;
            projectile.height = 17;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.scale = 0.5f;
            projectile.timeLeft = 100;
        }

        public override void AI()
        {
            projectile.rotation += 0.2f;
            projectile.scale += .02f;
        }

        public override void Kill(int timeLeft)
        {
            const int proj2 = 88; //laser rifle

            FargoGlobalProjectile.XWay(16, projectile.Center, proj2, 4, projectile.damage, 2);

            for (int i = 0; i < 50; i++)
            {
                Vector2 pos = projectile.position + (projectile.velocity * Main.rand.Next(40, 60)) + 
                    (projectile.velocity.RotatedBy(MathHelper.Pi / 2) * Main.rand.Next(-150, 150));


                int p = Projectile.NewProjectile(pos.X, pos.Y, -projectile.velocity.X * 3, -projectile.velocity.Y * 3, proj2,
                    projectile.damage, 0f, Main.myPlayer);
                Main.projectile[p].penetrate = 1;
                Main.projectile[p].timeLeft = 180;
            }
        }
    }
}