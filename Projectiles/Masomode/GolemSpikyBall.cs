using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Buffs.Masomode;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class GolemSpikyBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spiky Ball");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.SpikyBallTrap);
            aiType = ProjectileID.SpikyBallTrap;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.trap = false;
            projectile.penetrate = 1;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            for (int index1 = 0; index1 < 7; ++index1)
            {
                int index2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 246, -projectile.velocity.X * 0.2f, -projectile.velocity.Y * 0.2f, 100, new Color(), 2f);
                Main.dust[index2].noGravity = true;
                Main.dust[index2].velocity *= 1.5f;
                int index3 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 246, -projectile.velocity.X * 0.2f, -projectile.velocity.Y * 0.2f, 100, new Color(), 1f);
                Main.dust[index3].velocity *= 1.5f;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.BrokenArmor, 600);
            target.AddBuff(ModContent.BuffType<Defenseless>(), 600);
            target.AddBuff(BuffID.WitheredArmor, 600);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)
                projectile.velocity.X = -oldVelocity.X * 0.9f;
            if (projectile.velocity.Y != oldVelocity.Y && oldVelocity.Y > 1)
                projectile.velocity.Y = -oldVelocity.Y * 0.9f;
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}