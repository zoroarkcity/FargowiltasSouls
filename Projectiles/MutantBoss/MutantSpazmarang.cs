using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.MutantBoss
{
    public class MutantSpazmarang : MutantRetirang
    {
        public override string Texture => "FargowiltasSouls/Projectiles/BossWeapons/Spazmarang";
        private int counter = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spazmarang");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50;
            projectile.scale = 1.5f;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.aiStyle = -1;
            cooldownSlot = 1;
        }

        public override void AI()
        {
            if (++projectile.localAI[0] > projectile.ai[1])
                projectile.Kill();

            /*if (projectile.localAI[0] == (int)projectile.ai[1] / 2 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(projectile.Center, Vector2.Normalize(projectile.velocity).RotatedBy(Math.PI / 2) * 14, ProjectileID.CursedFlameHostile, projectile.damage, 0, Main.myPlayer);
                Projectile.NewProjectile(projectile.Center, Vector2.Normalize(projectile.velocity).RotatedBy(Math.PI / 2) * -14, ProjectileID.CursedFlameHostile, projectile.damage, 0, Main.myPlayer);
            }*/

            Vector2 acceleration = Vector2.Normalize(projectile.velocity).RotatedBy(Math.PI / 2) * projectile.ai[0];
            projectile.velocity += acceleration;

            projectile.rotation += 1f * Math.Sign(projectile.ai[0]);

            int dustId = Dust.NewDust(projectile.position, projectile.width, projectile.height, 75, projectile.velocity.X * 0.2f,
                projectile.velocity.Y * 0.2f, 100, default(Color), 2f);
            Main.dust[dustId].noGravity = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 120);
            if (FargoSoulsWorld.MasochistMode)
                target.AddBuff(mod.BuffType("MutantFang"), 180);
        }
    }
}