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
            projectile.penetrate = -1;
            projectile.minion = false;
            projectile.ranged = true;
            projectile.timeLeft = 120;
            projectile.width = 10;
            projectile.height = 18;
        }

        public override void AI()
        {
            //stuck in enemy
            if(projectile.ai[0] == 1)
            {
                projectile.ignoreWater = true;
                projectile.tileCollide = false;

                int secondsStuck = 15;
                bool kill = false;
  
                projectile.localAI[0] += 1f;

                int npcIndex = (int)projectile.ai[1];
                if (projectile.localAI[0] >= (float)(60 * secondsStuck))
                {
                    kill = true;
                }
                else if (npcIndex < 0 || npcIndex >= 200)
                {
                    kill = true;
                }
                else if (Main.npc[npcIndex].active && !Main.npc[npcIndex].dontTakeDamage)
                {
                    projectile.Center = Main.npc[npcIndex].Center - projectile.velocity * 2f;
                    projectile.gfxOffY = Main.npc[npcIndex].gfxOffY;
                }
                else
                {
                    kill = true;
                }

                if (kill)
                {
                    projectile.Kill();
                }
            }
            else
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
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            //damage = damage + (int)(target.defense * 0.5f) / 2; 
            crit = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for(int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];

                if(p.active && p.type == projectile.type && p != projectile && projectile.Hitbox.Intersects(p.Hitbox))
                {
                    target.StrikeNPC(projectile.damage, 0, 0, true);
                    target.AddBuff(BuffID.Poisoned, 600);
                    DustRing(p, 16);
                    p.Kill();
                }
            }

            projectile.ai[0] = 1;
            projectile.ai[1] = (float)target.whoAmI;
            projectile.velocity = (Main.npc[target.whoAmI].Center - projectile.Center) * 1f; //distance it sticks out
            projectile.damage = 0;
            projectile.timeLeft = 300;
            projectile.netUpdate = true;
        }

        public override void Kill(int timeLeft)
        {
            if (Main.rand.Next(2) == 0)
            {
                int num92 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 18, 0f, 0f, 0, default(Color), 0.9f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 0.5f;
            }
        }

        private void DustRing(Projectile proj, int max)
        {
            //dust
            for (int i = 0; i < max; i++)
            {
                Vector2 vector6 = Vector2.UnitY * 5f;
                vector6 = vector6.RotatedBy((i - (max / 2 - 1)) * 6.28318548f / max) + proj.Center;
                Vector2 vector7 = vector6 - proj.Center;
                int d = Dust.NewDust(vector6 + vector7, 0, 0, 18, 0f, 0f, 0, default(Color), 1.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = vector7;
            }
        }
    }
}