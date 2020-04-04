using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class BloodScythe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Sickle");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.DemonSickle);
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.magic = false;
            projectile.timeLeft = 300;
            projectile.tileCollide = false;
            cooldownSlot = 1;
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0)
            {
                projectile.localAI[0] = 1;
                Main.PlaySound(SoundID.Item8, projectile.Center);
            }
            projectile.rotation += 0.8f;
            if (++projectile.localAI[1] > 30 && projectile.localAI[1] < 120)
                projectile.velocity *= 1.03f;
            for (int i = 0; i < 2; i++)
            {
                int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27, 0f, 0f, 100);
                Main.dust[d].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            //target.AddBuff(mod.BuffType("Shadowflame"), 300);
            //target.AddBuff(BuffID.Bleeding, 600);
            target.AddBuff(BuffID.Obstructed, 30);
            target.AddBuff(mod.BuffType("Berserked"), 150);
        }
    }
}