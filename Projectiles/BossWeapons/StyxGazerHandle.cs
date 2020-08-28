using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Enums;

namespace FargowiltasSouls.Projectiles.BossWeapons
{
    public class StyxGazerHandle : Deathrays.BaseDeathray
    {
        public StyxGazerHandle() : base(120, "AbomDeathray") { }
        public int counter;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Styx Gazer");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.extraUpdates = 1;
            projectile.GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
        }

        public override void AI()
        {
            Vector2? vector78 = null;
            if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
            {
                projectile.velocity = -Vector2.UnitY;
            }
            int ai1 = (int)projectile.ai[1];
            if (Main.projectile[ai1].active && Main.projectile[ai1].type == ModContent.ProjectileType<StyxGazer>())
            {
                projectile.Center = Main.projectile[ai1].Center + Main.projectile[ai1].velocity * 75;
                projectile.velocity = Main.projectile[ai1].velocity.RotatedBy(projectile.ai[0]);
            }
            else
            {
                if (projectile.localAI[0] == 15)
                {
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].hostile && Main.projectile[i].type == ModContent.ProjectileType<StyxGazer>())
                        {
                            projectile.ai[1] = i;
                            return;
                        }
                    }
                }
                else if (projectile.localAI[0] > 15 || Main.netMode != NetmodeID.MultiplayerClient)
                {
                    projectile.Kill();
                }
                return;
            }
            if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
            {
                projectile.velocity = -Vector2.UnitY;
            }
            if (projectile.localAI[0] == 0f)
            {
                Main.PlaySound(SoundID.Zombie, (int)projectile.position.X, (int)projectile.position.Y, 104, 1f, 0f);
            }
            float num801 = 1f;
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] >= maxTime)
            {
                projectile.Kill();
                return;
            }
            projectile.scale = (float)Math.Sin(projectile.localAI[0] * 3.14159274f / maxTime) * num801 * 6f;
            if (projectile.scale > num801)
            {
                projectile.scale = num801;
            }
            float num804 = projectile.velocity.ToRotation();
            /*if (Main.npc[ai1].velocity != Vector2.Zero)
                num804 += projectile.ai[0];*/
            projectile.rotation = num804 - 1.57079637f;
            projectile.velocity = num804.ToRotationVector2();
            float num805 = 3f;
            float num806 = (float)projectile.width;
            Vector2 samplingPoint = projectile.Center;
            if (vector78.HasValue)
            {
                samplingPoint = vector78.Value;
            }
            float[] array3 = new float[(int)num805];
            //Collision.LaserScan(samplingPoint, projectile.velocity, num806 * projectile.scale, 3000f, array3);
            for (int i = 0; i < array3.Length; i++)
                array3[i] = 100f;
            float num807 = 0f;
            int num3;
            for (int num808 = 0; num808 < array3.Length; num808 = num3 + 1)
            {
                num807 += array3[num808];
                num3 = num808;
            }
            num807 /= num805;
            float amount = 0.5f;
            projectile.localAI[1] = MathHelper.Lerp(projectile.localAI[1], num807, amount);

            if (Main.rand.Next(2) == 0)
            {
                int d = Dust.NewDust(projectile.position + projectile.velocity * Main.rand.NextFloat(100), projectile.width, projectile.height, 87, 0f, 0f, 0, default(Color), 1.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 4f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.ShadowFlame, 300);
            target.AddBuff(ModContent.BuffType<Buffs.Masomode.MutantNibble>(), 300);
            target.immune[projectile.owner] = 6;
        }
    }
}