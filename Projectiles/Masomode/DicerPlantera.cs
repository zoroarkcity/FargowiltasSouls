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
            Lighting.AddLight(projectile.Center, .4f, 1.2f, .4f); //glow in the dark

            if (projectile.localAI[0] == 0) //random rotation direction
                projectile.localAI[0] = Main.rand.Next(2) == 0 ? 1 : -1;

            if (projectile.ai[0] > 0) //delay before checking for nearby player
            {
                projectile.ai[0]--;
                projectile.scale = 1f;
            }
            else
            {
                projectile.ai[1]--;

                if (projectile.ai[1] > -60)
                {
                    projectile.scale += 0.09f;
                    projectile.rotation += 0.45f * projectile.localAI[0];
                }

                if (projectile.ai[1] < -75) //explode
                {
                    projectile.ai[0] = 30;
                    projectile.ai[1] = 0;
                    bool planteraAlive = NPC.plantBoss > -1 && NPC.plantBoss < Main.maxNPCs && Main.npc[NPC.plantBoss].active && Main.npc[NPC.plantBoss].type == NPCID.Plantera;
                    if (++projectile.localAI[1] > 7 || !planteraAlive) //die after this many explosions
                    {
                        projectile.timeLeft = 0;
                    }

                    Main.PlaySound(SoundID.Item, projectile.Center, 14); //spray

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        const int time = 15;
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

        /*public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("Infested"), 180);
            target.AddBuff(mod.BuffType("IvyVenom"), 180);
        }*/

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.NPCDeath1, projectile.Center);
        }
    }
}