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

            int bonusMinions = player.maxMinions > 5 ? 5 : player.maxMinions;

            if (player.ownedProjectileCounts[ModContent.ProjectileType<Whirlpool>()] < 1)
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<Whirlpool>(), projectile.damage * 2, 0f, projectile.owner, 16, 6 + bonusMinions);
            else
                Main.projectile.Where(x => x.active && x.type == ModContent.ProjectileType<Whirlpool>()).ToList().ForEach(x =>
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Vector2 velocity = Vector2.Normalize(target.Center - x.Center) * 20;
                        Projectile.NewProjectile(x.Center, velocity, ModContent.ProjectileType<FishStickShark>(), projectile.damage / 2, projectile.knockBack, projectile.owner);
                    }
                });
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width /= 2;
            height /= 2;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
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