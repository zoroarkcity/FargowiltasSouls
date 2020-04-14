using FargowiltasSouls.Buffs.Masomode;
using FargowiltasSouls.Projectiles.Masomode;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.NPCs
{
    public partial class EModeGlobalNPC
    {
        public void RuneWizardAI(NPC npc)
        {
            if (npc.Distance(Main.player[Main.myPlayer].Center) < 1500f)
            {
                if (npc.Distance(Main.player[Main.myPlayer].Center) > 400f)
                    Main.player[Main.myPlayer].AddBuff(ModContent.BuffType<Hexed>(), 2);

                for (int i = 0; i < 20; i++)
                {
                    Vector2 offset = new Vector2();
                    double angle = Main.rand.NextDouble() * 2d * Math.PI;
                    offset.X += (float)(Math.Sin(angle) * 400);
                    offset.Y += (float)(Math.Cos(angle) * 400);
                    Dust dust = Main.dust[Dust.NewDust(
                        npc.Center + offset - new Vector2(4, 4), 0, 0,
                        DustID.BubbleBlock, 0, 0, 100, default(Color), 1f
                        )];
                    dust.velocity = npc.velocity;
                    if (Main.rand.Next(3) == 0)
                        dust.velocity += Vector2.Normalize(offset) * 5f;
                    dust.noGravity = true;
                    dust.color = Color.GreenYellow;
                }
            }
            Aura(npc, 200, BuffID.Suffocation, false, 119);
            if (npc.Distance(Main.player[Main.myPlayer].Center) < 200)
                Main.player[Main.myPlayer].AddBuff(ModContent.BuffType<Hexed>(), 2);
            if (++Counter > 300)
            {
                Counter = 0;
                if (Main.netMode != 1 && npc.HasPlayerTarget)
                {
                    Vector2 vel = npc.DirectionFrom(Main.player[npc.target].Center) * 8f;
                    for (int i = 0; i < 5; i++)
                    {
                        int p = Projectile.NewProjectile(npc.Center, vel.RotatedBy(2 * Math.PI / 5 * i),
                            ProjectileID.RuneBlast, 25, 0f, Main.myPlayer, 1);
                        if (p != 1000)
                            Main.projectile[p].timeLeft = 300;
                    }
                }
            }
        }

        public void RainbowSlimeAI(NPC npc)
        {
            if (masoBool[0]) //small rainbow slime
            {
                if (!masoBool[1] && ++Counter > 15)
                {
                    masoBool[1] = true;
                    if (Main.netMode == 2) //MP sync
                    {
                        var netMessage = mod.GetPacket();
                        netMessage.Write((byte)3);
                        netMessage.Write((byte)npc.whoAmI);
                        netMessage.Write(npc.lifeMax);
                        netMessage.Write(npc.scale);
                        netMessage.Send();
                        npc.netUpdate = true;
                    }
                }
            }

            if (masoBool[2]) //shoot spikes whenever jumping
            {
                if (npc.velocity.Y == 0f) //start attack
                {
                    masoBool[2] = false;
                    if (npc.HasPlayerTarget && Main.netMode != 1)
                    {
                        const float gravity = 0.15f;
                        const float time = 120f;
                        Vector2 distance = Main.player[npc.target].Center - npc.Center;
                        distance += Main.player[npc.target].velocity * 30f;
                        distance.X = distance.X / time;
                        distance.Y = distance.Y / time - 0.5f * gravity * time;
                        for (int i = 0; i < 10; i++)
                        {
                            Projectile.NewProjectile(npc.Center, distance + Main.rand.NextVector2Square(-1f, 1f),
                                ModContent.ProjectileType<RainbowSlimeSpike>(), npc.damage / 8, 0f, Main.myPlayer);
                        }
                    }
                }
            }
            else if (npc.velocity.Y > 0)
            {
                masoBool[2] = true;
            }
        }
    }
}
