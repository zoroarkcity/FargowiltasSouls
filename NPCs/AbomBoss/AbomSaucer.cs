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
            npc.lifeMax = 10000;
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
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = (int)(npc.damage * 0.5f);
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
        }

        public override void AI()
        {
            if (npc.ai[0] < 0 || npc.ai[0] >= Main.maxNPCs || !Main.npc[(int)npc.ai[0]].active ||
                Main.npc[(int)npc.ai[0]].type != mod.NPCType("AbomBoss") || Main.npc[(int)npc.ai[0]].ai[0] >= 4)
            {
                npc.StrikeNPCNoInteraction(npc.lifeMax, 0f, 0);
                return;
            }

            NPC abom = Main.npc[(int)npc.ai[0]];
            npc.target = abom.target;

            if (++npc.ai[1] > 120) //pause before attacking
            {
                npc.velocity = Vector2.Zero;

                if (npc.ai[3] == 0) //store angle for attack
                    npc.ai[3] = npc.DirectionTo(Main.player[npc.target].Center).ToRotation();

                if (npc.ai[1] > 180) //attack and reset
                {
                    npc.netUpdate = true;
                    npc.ai[1] = 0;
                    npc.ai[3] = 0;
                    Main.PlaySound(SoundID.Item12, npc.Center);
                    if (Main.netMode != 1)
                    {
                        Vector2 speed = npc.ai[3].ToRotationVector2() * 16f;
                        Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("AbomLaser"), abom.damage / 4, 0f, Main.myPlayer);
                    }
                }
            }
            else
            {
                Vector2 target = Main.player[npc.target].Center; //targeting
                target += Vector2.UnitX.RotatedBy(npc.ai[2]) * 600;

                npc.ai[2] -= 0.015f; //spin around target
                if (npc.ai[2] < (float)-Math.PI)
                    npc.ai[2] += 2 * (float)Math.PI;

                Vector2 distance = target - npc.Center;
                float length = distance.Length();
                if (length > 100f)
                {
                    distance /= 8f;
                    npc.velocity = (npc.velocity * 23f + distance) / 24f;
                }
                else
                {
                    if (npc.velocity.Length() < 12f)
                        npc.velocity *= 1.05f;
                }
            }

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
        }

        public override bool CheckActive()
        {
            return false;
        }
    }
}