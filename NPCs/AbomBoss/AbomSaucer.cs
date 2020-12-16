using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.IO;

namespace FargowiltasSouls.NPCs.AbomBoss
{
    public class AbomSaucer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Saucer");
        }

        public override void SetDefaults()
        {
            npc.width = 25;
            npc.height = 25;
            npc.defense = 90;
            npc.lifeMax = 600;
            npc.scale = 2f;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.aiStyle = -1;
            npc.buffImmune[BuffID.Chilled] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Suffocation] = true;
            npc.buffImmune[mod.BuffType("Lethargic")] = true;
            npc.buffImmune[mod.BuffType("ClippedWings")] = true;
            npc.buffImmune[mod.BuffType("MutantNibble")] = true;
            npc.buffImmune[mod.BuffType("OceanicMaul")] = true;
            npc.GetGlobalNPC<FargoSoulsGlobalNPC>().SpecialEnchantImmune = true;

            npc.dontTakeDamage = true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = (int)(npc.damage * 0.5f);
            npc.lifeMax = (int)(npc.lifeMax /** 0.5f*/ * bossLifeScale);
        }

        public override void AI()
        {
            if (npc.ai[0] < 0 || npc.ai[0] >= Main.maxNPCs || !Main.npc[(int)npc.ai[0]].active ||
                Main.npc[(int)npc.ai[0]].type != mod.NPCType("AbomBoss") || Main.npc[(int)npc.ai[0]].dontTakeDamage)
            {
                npc.StrikeNPCNoInteraction(999999, 0f, 0);
                return;
            }

            NPC abom = Main.npc[(int)npc.ai[0]];
            npc.target = abom.target;

            npc.dontTakeDamage = abom.ai[0] == 0;

            if (++npc.ai[1] > 90) //pause before attacking
            {
                npc.velocity = Vector2.Zero;

                if (npc.ai[3] == 0) //store angle for attack
                {
                    npc.localAI[2] = npc.Distance(Main.player[npc.target].Center);
                    npc.ai[3] = npc.DirectionTo(Main.player[npc.target].Center).ToRotation();
                }

                if (npc.ai[1] > 120) //attack and reset
                {
                    Main.PlaySound(SoundID.Item12, npc.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Vector2 speed = 16f * npc.ai[3].ToRotationVector2().RotatedBy((Main.rand.NextDouble() - 0.5) * 0.785398185253143 / 12.0);
                            speed *= Main.rand.NextFloat(0.9f, 1.1f);
                            int p = Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("AbomLaser"), abom.damage / 4, 0f, Main.myPlayer);
                            if (p != Main.maxProjectiles)
                                Main.projectile[p].timeLeft = (int)(npc.localAI[2] / speed.Length()) + 1;
                        }
                    }
                    npc.netUpdate = true;
                    npc.ai[1] = 0;
                    npc.ai[3] = 0;
                }
            }
            else
            {
                Vector2 target = Main.player[npc.target].Center; //targeting
                target += Vector2.UnitX.RotatedBy(npc.ai[2]) * 400;

                Vector2 distance = target - npc.Center;
                float length = distance.Length();
                distance /= 8f;
                npc.velocity = (npc.velocity * 23f + distance) / 24f;
            }

            npc.ai[2] -= 0.03f; //spin around target
            if (npc.ai[2] < (float)-Math.PI)
                npc.ai[2] += 2 * (float)Math.PI;

            if (npc.localAI[1] == 0) //visuals
                npc.localAI[1] = Main.rand.Next(2) == 0 ? 1 : -1;
            npc.rotation = (float)Math.Sin(2 * Math.PI * npc.localAI[0]++ / 90) * (float)Math.PI / 8f * npc.localAI[1];
            if (npc.localAI[0] > 180)
                npc.localAI[0] = 0;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 3; i++)
            {
                int d = Dust.NewDust(npc.position, npc.width, npc.height, 87, 0f, 0f, 0, default(Color), 1f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 3f;
            }
            if (npc.life <= 0)
            {
                for (int i = 0; i < 30; i++)
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 87, 0f, 0f, 0, default(Color), 2.5f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 12f;
                }
            }
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }

        public override bool CheckActive()
        {
            return false;
        }
    }
}