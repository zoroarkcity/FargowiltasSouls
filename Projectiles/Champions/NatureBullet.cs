using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Champions
{
    public class NatureBullet : ModProjectile
    {
        public override string Texture => "FargowiltasSouls/Projectiles/Masomode/SniperBullet";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nature Bullet");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ExplosiveBullet);
            aiType = ProjectileID.Bullet;
            projectile.friendly = false;
            projectile.ranged = false;
            projectile.hostile = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 180);
            target.AddBuff(BuffID.Chilled, 180);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);

            for (int index1 = 0; index1 < 20; ++index1)
            {
                int index2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 68, 0f, 0f, 0, default(Color), 1f);
                Main.dust[index2].noGravity = true;
                Main.dust[index2].velocity *= 1.5f;
                Main.dust[index2].scale *= 0.9f;
            }

            if (Main.netMode != 1)
            {
                for (int index = 0; index < 6; ++index)
                {
                    float SpeedX = projectile.velocity.Length() * Main.rand.Next(-60, 61) * 0.01f + Main.rand.Next(-20, 21) * 0.4f;
                    float SpeedY = projectile.velocity.Length() * Main.rand.Next(-60, 61) * 0.01f + Main.rand.Next(-20, 21) * 0.4f;
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, SpeedX, SpeedY,
                        ModContent.ProjectileType<CrystalBombShard>(), projectile.damage, 0f, projectile.owner);
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}