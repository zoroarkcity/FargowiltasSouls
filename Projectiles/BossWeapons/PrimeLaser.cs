using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.BossWeapons
{
    public class PrimeLaser : ModProjectile 
    {
        public override string Texture => "Terraria/Projectile_389";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prime Laser");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.MiniRetinaLaser);
            aiType = ProjectileID.MiniRetinaLaser;
            projectile.ignoreWater = true;
            projectile.timeLeft = 150;
            projectile.magic = true;
            projectile.penetrate = 1;
        }

        //public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        //{
        //    projectile.timeLeft = 0;
        //    if (projectile.ai[0] == 0)
        //    {
        //        for (int i = 0; i < 2; i++)
        //        {
        //            Vector2 pos = projectile.Center + projectile.velocity * 30f;//new Vector2(projectile.Center.X + Main.rand.Next(-150, 150), projectile.Center.Y - 500);
        //            pos.X += Main.rand.Next(-150, 150);
        //            pos.Y += Main.rand.Next(-150, 150);
        //            Vector2 velocity = Vector2.Normalize(target.Center - pos) * 15;

        //            int p = Projectile.NewProjectile(pos, velocity, mod.ProjectileType("DarkStarFriendly"), projectile.damage, projectile.knockBack, projectile.owner);


        //            Main.projectile[p].ai[0] = 1;

        //            Main.projectile[p].tileCollide = false;
        //            Main.projectile[p].ai[1] = 0f;
        //            Main.projectile[p].netUpdate = true;
        //        }

        //        projectile.ai[0] = 1;
        //    }
        //}

        //public override void Kill(int timeLeft)
        //{
        //    Main.PlaySound(SoundID.Item10, projectile.position);
        //    int num1 = 10;
        //    int num2 = 3;

        //    for (int index = 0; index < num1; ++index)
        //        Dust.NewDust(projectile.position, projectile.width, projectile.height, 58, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, new Color(), 1.2f);
        //    for (int index = 0; index < num2; ++index)
        //    {
        //        int Type = Main.rand.Next(16, 18);
        //        if (projectile.type == 503)
        //            Type = 16;
        //        Gore.NewGore(projectile.position, new Vector2(projectile.velocity.X * 0.05f, projectile.velocity.Y * 0.05f), Type, 1f);
        //    }

        //    for (int index = 0; index < 10; ++index)
        //        Dust.NewDust(projectile.position, projectile.width, projectile.height, 57, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, new Color(), 1.2f);
        //    for (int index = 0; index < 3; ++index)
        //        Gore.NewGore(projectile.position, new Vector2(projectile.velocity.X * 0.05f, projectile.velocity.Y * 0.05f), Main.rand.Next(16, 18), 1f);
        //}

        //public override Color? GetAlpha(Color lightColor)
        //{
        //    return new Color(255, 100, 100, lightColor.A - projectile.alpha);
        //}
    }
}