using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.BossWeapons
{
    public class FishStickProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fish Stick");
        }

        public override void SetDefaults()
        {
            projectile.width = 35;
            projectile.height = 35;
            projectile.aiStyle = 1;
            aiType = ProjectileID.JavelinFriendly;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.minion = true;
        }

        public override void AI()
        {
            projectile.spriteDirection = -projectile.direction;

            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + MathHelper.ToRadians(135f);

            if (projectile.spriteDirection == -1) projectile.rotation -= MathHelper.ToRadians(90f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];

            if (player.ownedProjectileCounts[mod.ProjectileType("Whirlpool")] < 1)
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("Whirlpool"), projectile.damage, 0f, projectile.owner, 16, 11);
            else
                Main.projectile.Where(x => x.active && x.type == mod.ProjectileType("Whirlpool")).ToList().ForEach(x =>
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Vector2 velocity = Vector2.Normalize(target.Center - x.Center) * 20;
                        int shark = Projectile.NewProjectile(x.Center, velocity, ProjectileID.MiniSharkron, projectile.damage, projectile.knockBack, projectile.owner);
                        if (shark < 1000)
                        {
                            Main.projectile[shark].tileCollide = false;
                            Main.projectile[shark].timeLeft = 120;
                            Main.projectile[shark].penetrate = 1;
                            Main.projectile[shark].minion = true;
                        }
                    }
                });
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 59, -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 2f;
                dust = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, 59, -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100);
                Main.dust[dust].velocity *= 2f;
            }
        }
    }
}