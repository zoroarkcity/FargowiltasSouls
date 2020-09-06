using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.BossWeapons
{
    public class SmallStinger : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Small Stinger");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.HornetStinger);
            aiType = ProjectileID.Bullet;
            projectile.penetrate = 1;
            projectile.minion = false;
            projectile.ranged = true;
            projectile.timeLeft = 240;
            projectile.width = 10;
            projectile.height = 18;
        }

        public override void AI()
        {
            projectile.position += projectile.velocity * 0.5f;

            //dust from stinger
            if (Main.rand.Next(2) == 0)
            {
                int num92 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 18, 0f, 0f, 0, default(Color), 0.9f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 0.5f;
            }
        }

        /*public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 600);
            target.immune[projectile.owner] = 8;
        }*/

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = damage + (int)(target.defense * 0.5f) / 2; 
        }
    }
}