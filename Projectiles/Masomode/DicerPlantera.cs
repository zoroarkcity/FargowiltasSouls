using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class DicerPlantera : ModProjectile
    {
        public override string Texture => "FargowiltasSouls/Projectiles/BossWeapons/DicerProj2";

        private const int range = 250;

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.hostile = true;
            projectile.timeLeft = 1200;
        }

        public override bool CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, .2f, .6f, .2f); //glow in the dark

            if (projectile.localAI[0] == 0) //random rotation direction
                projectile.localAI[0] = Main.rand.Next(2) == 0 ? 1 : -1;

            if (projectile.ai[0] > 0) //delay before checking for nearby player
            {
                projectile.ai[0]--;
            }
            else
            {
                if (--projectile.ai[1] > 0) //waiting to explode
                {
                    int p = Player.FindClosest(projectile.Center, 0, 0);
                    if (p != -1 && Main.player[p].active && Main.player[p].Distance(projectile.Center) < range)
                    {
                        projectile.ai[1] = 0; //player nearby, immediately begin exploding
                        projectile.netUpdate = true;
                    }
                }
                else //prepare to explode
                {
                    projectile.scale += 0.06f;
                    projectile.rotation += 0.3f * projectile.localAI[0];

                    if (projectile.ai[1] < -90) //explode
                        projectile.timeLeft = 0;
                }
            }
        }

        /*public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("Infested"), 180);
            target.AddBuff(mod.BuffType("IvyVenom"), 180);
        }*/

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(2, projectile.Center, 14);

            if (Main.netMode != 1)
            {
                const int time = 20;
                const int max = 16;
                for (int i = 0; i < max; i++)
                {
                    int p = Projectile.NewProjectile(projectile.Center, range / time * Vector2.UnitX.RotatedBy(Math.PI * 2 / max * i), ProjectileID.PoisonSeedPlantera, projectile.damage, projectile.knockBack, projectile.owner);
                    if (p != Main.maxProjectiles)
                        Main.projectile[p].timeLeft = time;
                }
            }
        }
    }
}