using FargowiltasSouls.Items.Summons;
using FargowiltasSouls.Projectiles;
using FargowiltasSouls.Projectiles.Masomode;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FargowiltasSouls.NPCs
{
    public partial class EModeGlobalNPC
    {
        public void KingSlimeAI(NPC npc)
        {
            slimeBoss = npc.whoAmI;
            npc.color = Main.DiscoColor * 0.2f;
            if (masoBool[1])
            {
                if (npc.velocity.Y == 0f) //start attack
                {
                    masoBool[1] = false;
                    if (Main.netMode != 1)
                    {
                        for (int i = 0; i < 30; i++)
                        {
                            Projectile.NewProjectile(new Vector2(npc.Center.X + Main.rand.Next(-5, 5), npc.Center.Y - 15),
                                new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-8, -5)),
                                ProjectileID.SpikedSlimeSpike, npc.damage / 5, 0f, Main.myPlayer);
                        }
                    }
                }
            }
            else if (npc.velocity.Y > 0)
            {
                masoBool[1] = true;
            }

            if ((masoBool[0] || npc.life < npc.lifeMax * .5f) && npc.HasPlayerTarget)
            {
                Player p = Main.player[npc.target];

                Counter++;
                if (Counter >= 90) //slime rain
                {
                    Counter = 0;
                    Main.PlaySound(SoundID.Item21, p.Center);
                    if (Main.netMode != 1)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Vector2 spawn = p.Center;
                            spawn.X += Main.rand.Next(-200, 201);
                            spawn.Y -= Main.rand.Next(600, 901);
                            Vector2 speed = p.Center - spawn;
                            speed.Normalize();
                            speed *= masoBool[0] ? 10f : 5f;
                            speed = speed.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-5, 5)));
                            Projectile.NewProjectile(spawn, speed, ModContent.ProjectileType<SlimeBallHostile>(), npc.damage / 5, 0f, Main.myPlayer);
                        }
                    }
                }

                if (++Timer > 300)
                {
                    Timer = 0;
                    const float gravity = 0.15f;
                    float time = masoBool[0] ? 60f : 120f;
                    Vector2 distance = Main.player[npc.target].Center - npc.Center + Main.player[npc.target].velocity * 30f;
                    distance.X = distance.X / time;
                    distance.Y = distance.Y / time - 0.5f * gravity * time;
                    for (int i = 0; i < 10; i++)
                    {
                        Projectile.NewProjectile(npc.Center, distance + Main.rand.NextVector2Square(-0.5f, 0.5f) * (masoBool[0] ? 3 : 1),
                            ModContent.ProjectileType<SlimeSpike>(), npc.damage / 5, 0f, Main.myPlayer);
                    }
                }
            }

            if (!masoBool[0]) //is not berserk
            {
                SharkCount = 0;

                if (npc.HasPlayerTarget)
                {
                    Player player = Main.player[npc.target];
                    if (player.active && !player.dead && player.Center.Y < npc.position.Y && npc.Distance(player.Center) < 1000f)
                    {
                        Counter2++; //timer runs if player is above me and nearby
                        if (Counter2 >= 600 && Main.netMode != 1) //go berserk
                        {
                            masoBool[0] = true;
                            npc.netUpdate = true;
                            NetUpdateMaso(npc.whoAmI);
                            if (Main.netMode == 2)
                                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("King Slime has enraged!"), new Color(175, 75, 255));
                            else
                                Main.NewText("King Slime has enraged!", 175, 75, 255);
                        }
                    }
                    else
                    {
                        Counter2 = 0;
                    }
                }
            }
            else //is berserk
            {
                SharkCount = 1;

                if (!masoBool[2])
                {
                    masoBool[2] = true;
                    Main.PlaySound(15, npc.Center, 0);
                }

                if (Counter < 60) //slime rain much faster
                    Counter = 60;

                if (Timer < 270) //aimed spikes much faster
                    Timer = 270;

                if (npc.HasValidTarget && Main.player[npc.target].position.Y > npc.position.Y) //player went back down
                {
                    masoBool[0] = false;
                    masoBool[2] = false;
                    NetUpdateMaso(npc.whoAmI);
                }

                /*if (npc.HasPlayerTarget)
                {
                    Player p = Main.player[npc.target];

                    Counter2++;
                    if (Counter2 >= 4) //spray random slime spikes
                    {
                        Counter2 = 0;
                        if (Main.netMode != 1)
                        {
                            Vector2 speed = p.Center - npc.Center;
                            speed.Normalize();
                            speed *= 16f + Main.rand.Next(-50, 51) * 0.04f;
                            if (speed.X < 0)
                                speed = speed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-15, 31)));
                            else
                                speed = speed.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-30, 16)));
                            Projectile.NewProjectile(npc.position.X + Main.rand.Next(npc.width), npc.position.Y + Main.rand.Next(npc.height), speed.X, speed.Y, ProjectileID.SpikedSlimeSpike, npc.damage / 4, 0f, Main.myPlayer);
                        }
                    }

                    if (p.active && !p.dead)
                    {
                        npc.noTileCollide = false;
                        p.pulley = false;
                        p.controlHook = false;
                        if (p.mount.Active)
                            p.mount.Dismount(p);

                        p.AddBuff(BuffID.Slimed, 2);
                        p.AddBuff(ModContent.BuffType<Crippled>(), 2);
                        p.AddBuff(ModContent.BuffType<ClippedWings>(), 2);
                    }
                    else
                    {
                        npc.noTileCollide = true;
                    }
                }*/
            }
        }

        public void EyeOfCthulhuAI(NPC npc)
        {
            eyeBoss = npc.whoAmI;

            Counter++;
            if (Counter >= 600)
            {
                Counter = 0;
                if (npc.life <= npc.lifeMax * 0.65 && NPC.CountNPCS(NPCID.ServantofCthulhu) < 12 && Main.netMode != 1)
                {
                    Vector2 vel = new Vector2(2, 2);
                    for (int i = 0; i < 4; i++)
                    {
                        int n = NPC.NewNPC((int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), NPCID.ServantofCthulhu);
                        if (n != 200)
                        {
                            Main.npc[n].velocity = vel.RotatedBy(Math.PI / 2 * i);
                            if (Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                    }
                }
            }

            if (npc.life < npc.lifeMax / 2)
            {
                if (npc.ai[0] == 3 && (npc.ai[1] == 0 || npc.ai[1] == 5))
                {
                    if (npc.ai[2] < 2)
                    {
                        npc.ai[2]--;
                        npc.alpha += 4;
                        for (int i = 0; i < 3; i++)
                        {
                            int d = Dust.NewDust(npc.position, npc.width, npc.height, 229, 0f, 0f, 0, default(Color), 1.5f);
                            Main.dust[d].noGravity = true;
                            Main.dust[d].noLight = true;
                            Main.dust[d].velocity *= 4f;
                        }
                        if (npc.alpha > 255)
                        {
                            npc.alpha = 255;
                            if (Main.netMode != 1 && npc.HasPlayerTarget)
                            {
                                Vector2 distance = npc.Center - Main.player[npc.target].Center;
                                npc.Center = Main.player[npc.target].Center;
                                distance.X *= 1.5f;
                                if (distance.X > 1200)
                                    distance.X = 1200;
                                else if (distance.X < -1200)
                                    distance.X = -1200;
                                if (distance.Y > 0)
                                    distance.Y *= -1;
                                npc.position.X -= distance.X;
                                npc.position.Y += distance.Y;
                                npc.netUpdate = true;
                                npc.ai[2] = 60;
                                npc.ai[1] = 5f;//
                            }
                        }
                    }
                    else
                    {
                        npc.alpha -= 4;
                        if (npc.alpha < 0)
                        {
                            npc.alpha = 0;
                        }
                        else
                        {
                            npc.ai[2]--;
                            npc.position -= npc.velocity / 2;
                            for (int i = 0; i < 3; i++)
                            {
                                int d = Dust.NewDust(npc.position, npc.width, npc.height, 229, 0f, 0f, 0, default(Color), 1.5f);
                                Main.dust[d].noGravity = true;
                                Main.dust[d].noLight = true;
                                Main.dust[d].velocity *= 4f;
                            }
                        }
                    }
                }

                npc.dontTakeDamage = npc.alpha > 50;

                if (Counter2 > 0)
                {
                    if (Counter2 % 6 == 0 && Main.netMode != 1)
                        Projectile.NewProjectile(new Vector2(npc.Center.X + Main.rand.Next(-15, 15), npc.Center.Y), npc.velocity / 10, ModContent.ProjectileType<BloodScythe>(), npc.damage / 4, 1f, Main.myPlayer);
                    Counter2--;

                }
                if (npc.ai[1] == 3f) //during dashes in phase 2
                {
                    Counter2 = 30;
                    masoBool[0] = false;
                    if (Main.netMode != 1)
                        FargoGlobalProjectile.XWay(8, npc.Center, ModContent.ProjectileType<BloodScythe>(), 1.5f, npc.damage / 4, 0);
                }
                /*if (++Timer > 600)
                {
                    Timer = 0;
                    if (npc.HasValidTarget)
                    {
                        Player player = Main.player[npc.target];
                        Main.PlaySound(29, (int)player.position.X, (int)player.position.Y, 104, 1f, 0f);
                        if (Main.netMode != 1)
                        {
                            Vector2 spawnPos = player.Center;
                            int direction;
                            if (player.velocity.X == 0f)
                                direction = player.direction;
                            else
                                direction = Math.Sign(player.velocity.X);
                            spawnPos.X += 600 * direction;
                            spawnPos.Y -= 600;
                            Vector2 speed = Vector2.UnitY;
                            for (int i = 0; i < 30; i++)
                            {
                                Projectile.NewProjectile(spawnPos, speed, ModContent.ProjectileType<BloodScythe>(), npc.damage / 4, 1f, Main.myPlayer);
                                spawnPos.X += 72 * direction;
                                speed.Y += 0.15f;
                            }
                        }
                    }
                }*/
            }
            else
            {
                npc.alpha = 0;
                npc.dontTakeDamage = false;
            }
        }

        public void BrainOfCthulhuAI(NPC npc)
        {
            brainBoss = npc.whoAmI;
            if (npc.alpha == 0)
            {
                npc.damage = npc.defDamage;
            }
            else
            {
                npc.damage = 0;
                if (npc.ai[0] != -2 && npc.HasPlayerTarget && npc.Distance(Main.player[npc.target].Center) < 300) //stay at a minimum distance
                {
                    npc.Center = Main.player[npc.target].Center + Main.player[npc.target].DirectionTo(npc.Center) * 300;
                }
            }

            if (!npc.dontTakeDamage) //vulnerable
            {
                if (npc.buffType[0] != 0) //constant debuff cleanse
                {
                    npc.buffImmune[npc.buffType[0]] = true;
                    npc.DelBuff(0);
                }
                if (!masoBool[0]) //spawn illusions
                {
                    masoBool[0] = true;
                    if (Main.netMode != 1)
                    {
                        bool recolor = SoulConfig.Instance.BossRecolors && FargoSoulsWorld.MasochistMode;
                        int type = recolor ? ModContent.NPCType<BrainIllusion2>() : ModContent.NPCType<BrainIllusion>();
                        int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, type, npc.whoAmI, npc.whoAmI, -1, 1);
                        if (n != 200 && Main.netMode == 2)
                            NetMessage.SendData(23, -1, -1, null, n);
                        n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, type, npc.whoAmI, npc.whoAmI, 1, -1);
                        if (n != 200 && Main.netMode == 2)
                            NetMessage.SendData(23, -1, -1, null, n);
                        n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, type, npc.whoAmI, npc.whoAmI, 1, 1);
                        if (n != 200 && Main.netMode == 2)
                            NetMessage.SendData(23, -1, -1, null, n);
                        n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<BrainClone>(), npc.whoAmI);
                        if (n != 200 && Main.netMode == 2)
                            NetMessage.SendData(23, -1, -1, null, n);
                    }
                }

                void MakeDust(Vector2 spawn)
                {
                    for (int i = 0; i < 24; i++) //dust ring
                    {
                        Vector2 vector6 = Vector2.UnitY * 12f;
                        vector6 = vector6.RotatedBy((i - (24 / 2 - 1)) * 6.28318548f / 24) + spawn;
                        Vector2 vector7 = vector6 - spawn;
                        int d = Dust.NewDust(vector6 + vector7, 0, 0, 90, 0f, 0f, 0, default(Color), 3f);
                        Main.dust[d].scale = 3f;
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity = vector7;
                    }
                };

                if (--Counter < 0) //confusion timer
                {
                    Counter = 600;
                    Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);

                    Vector2 offset = npc.Center - Main.player[npc.target].Center;

                    Vector2 spawnPos = Main.player[npc.target].Center;
                    spawnPos.X += offset.X;
                    spawnPos.Y += offset.Y;
                    MakeDust(spawnPos);

                    spawnPos = Main.player[npc.target].Center;
                    spawnPos.X += offset.X;
                    spawnPos.Y -= offset.Y;
                    MakeDust(spawnPos);

                    spawnPos = Main.player[npc.target].Center;
                    spawnPos.X -= offset.X;
                    spawnPos.Y += offset.Y;
                    MakeDust(spawnPos);

                    spawnPos = Main.player[npc.target].Center;
                    spawnPos.X -= offset.X;
                    spawnPos.Y -= offset.Y;
                    MakeDust(spawnPos);
                }
                else if (Counter == 540) //inflict confusion after telegraph
                {
                    if (npc.Distance(Main.player[Main.myPlayer].Center) < 3000)
                        Main.player[Main.myPlayer].AddBuff(BuffID.Confused, Main.expertMode && Main.expertDebuffTime > 1 ? 150 : 300);

                    if (npc.HasValidTarget && Main.netMode != 1) //laser spreads from each illusion
                    {
                        Vector2 offset = npc.Center - Main.player[npc.target].Center;

                        const int degree = 8;

                        Vector2 spawnPos = Main.player[npc.target].Center;
                        spawnPos.X += offset.X;
                        spawnPos.Y += offset.Y;
                        Projectile.NewProjectile(spawnPos, new Vector2(0, -4), ModContent.ProjectileType<BrainofConfusion>(), 0, 0, Main.myPlayer);
                        for (int i = -1; i <= 1; i++)
                            Projectile.NewProjectile(spawnPos, Main.player[npc.target].DirectionFrom(spawnPos).RotatedBy(MathHelper.ToRadians(degree) * i), ModContent.ProjectileType<DestroyerLaser>(), npc.damage / 4, 0f, Main.myPlayer);

                        spawnPos = Main.player[npc.target].Center;
                        spawnPos.X += offset.X;
                        spawnPos.Y -= offset.Y;
                        Projectile.NewProjectile(spawnPos, new Vector2(0, -4), ModContent.ProjectileType<BrainofConfusion>(), 0, 0, Main.myPlayer);
                        for (int i = -1; i <= 1; i++)
                            Projectile.NewProjectile(spawnPos, Main.player[npc.target].DirectionFrom(spawnPos).RotatedBy(MathHelper.ToRadians(degree) * i), ModContent.ProjectileType<DestroyerLaser>(), npc.damage / 4, 0f, Main.myPlayer);

                        spawnPos = Main.player[npc.target].Center;
                        spawnPos.X -= offset.X;
                        spawnPos.Y += offset.Y;
                        Projectile.NewProjectile(spawnPos, new Vector2(0, -4), ModContent.ProjectileType<BrainofConfusion>(), 0, 0, Main.myPlayer);
                        for (int i = -1; i <= 1; i++)
                            Projectile.NewProjectile(spawnPos, Main.player[npc.target].DirectionFrom(spawnPos).RotatedBy(MathHelper.ToRadians(degree) * i), ModContent.ProjectileType<DestroyerLaser>(), npc.damage / 4, 0f, Main.myPlayer);

                        spawnPos = Main.player[npc.target].Center;
                        spawnPos.X -= offset.X;
                        spawnPos.Y -= offset.Y;
                        Projectile.NewProjectile(spawnPos, new Vector2(0, -4), ModContent.ProjectileType<BrainofConfusion>(), 0, 0, Main.myPlayer);
                        for (int i = -1; i <= 1; i++)
                            Projectile.NewProjectile(spawnPos, Main.player[npc.target].DirectionFrom(spawnPos).RotatedBy(MathHelper.ToRadians(degree) * i), ModContent.ProjectileType<DestroyerLaser>(), npc.damage / 4, 0f, Main.myPlayer);
                    }
                }

                int b = Main.LocalPlayer.FindBuffIndex(BuffID.Confused);
                if (b != -1 && Main.LocalPlayer.buffTime[b] == 60)
                {
                    Main.PlaySound(36, (int)npc.Center.X, (int)npc.Center.Y, -1, 1f, 0f);
                    MakeDust(Main.LocalPlayer.Center);
                }
            }
        }

        public bool QueenBeeAI(NPC npc)
        {
            beeBoss = npc.whoAmI;

            if (!masoBool[0] && npc.life < npc.lifeMax / 3 * 2 && npc.HasPlayerTarget)
            {
                masoBool[0] = true;

                Vector2 vector72 = new Vector2(npc.position.X + (float)(npc.width / 2) + (float)(Main.rand.Next(20) * npc.direction), npc.position.Y + (float)npc.height * 0.8f);

                int num594 = NPC.NewNPC((int)vector72.X, (int)vector72.Y, ModContent.NPCType<RoyalSubject>(), 0, 0f, 0f, 0f, 0f, 255);
                Main.npc[num594].velocity.X = (float)Main.rand.Next(-200, 201) * 0.002f;
                Main.npc[num594].velocity.Y = (float)Main.rand.Next(-200, 201) * 0.002f;
                Main.npc[num594].localAI[0] = 60f;
                Main.npc[num594].netUpdate = true;

                if (Main.netMode == 0)
                    Main.NewText("Royal Subject has awoken!", 175, 75, 255);
                else if (Main.netMode == 2)
                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Royal Subject has awoken!"), new Color(175, 75, 255));
            }

            if (!masoBool[1] && npc.life < npc.lifeMax / 3 && npc.HasPlayerTarget)
            {
                masoBool[1] = true;

                Vector2 vector72 = new Vector2(npc.position.X + (float)(npc.width / 2) + (float)(Main.rand.Next(20) * npc.direction), npc.position.Y + (float)npc.height * 0.8f);

                int num594 = NPC.NewNPC((int)vector72.X, (int)vector72.Y, ModContent.NPCType<RoyalSubject>(), 0, 0f, 0f, 0f, 0f, 255);
                Main.npc[num594].velocity.X = (float)Main.rand.Next(-200, 201) * 0.1f;
                Main.npc[num594].velocity.Y = (float)Main.rand.Next(-200, 201) * 0.1f;
                Main.npc[num594].localAI[0] = 60f;
                Main.npc[num594].netUpdate = true;

                if (Main.netMode == 0)
                    Main.NewText("Royal Subject has awoken!", 175, 75, 255);
                else if (Main.netMode == 2)
                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Royal Subject has awoken!"), new Color(175, 75, 255));

                NPC.SpawnOnPlayer(npc.target, ModContent.NPCType<RoyalSubject>()); //so that both dont stack for being spawned from qb
            }

            if (!masoBool[2] && npc.life < npc.lifeMax / 2) //enable new attack and roar below 50%
            {
                masoBool[2] = true;
                Main.PlaySound(15, npc.Center, 0);
            }

            if (NPC.AnyNPCs(ModContent.NPCType<RoyalSubject>()))
            {
                npc.ai[0] = 3; //always shoot stingers mode
                RegenTimer = 480;
            }

            //only while stationary mode
            if (npc.ai[0] == 3f || npc.ai[0] == 1f)
            {
                if (masoBool[2] && ++Timer > 600)
                {
                    if (Timer < 690) //slow down
                    {
                        if (!masoBool[3])
                        {
                            masoBool[3] = true;
                            npc.netUpdate = true;
                            for (int i = 0; i < 36; i++)
                            {
                                Vector2 vector6 = Vector2.UnitY * 9f;
                                vector6 = vector6.RotatedBy((i - (36 / 2 - 1)) * 6.28318548f / 36) + npc.Center;
                                Vector2 vector7 = vector6 - npc.Center;
                                int d = Dust.NewDust(vector6 + vector7, 0, 0, 87, 0f, 0f, 0, default(Color), 4f);
                                Main.dust[d].noGravity = true;
                                Main.dust[d].velocity = vector7;
                            }
                            Main.PlaySound(15, npc.Center, 0);
                        }

                        if (Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                        {
                            npc.velocity *= 0.975f;
                        }
                        else
                        {
                            Timer--; //stall this section until has line of sight
                            return true;
                        }
                    }
                    else if (Timer < 840) //spray bees
                    {
                        if (masoBool[3])
                        {
                            masoBool[3] = false;
                            npc.netUpdate = true;
                        }
                        npc.velocity = Vector2.Zero;
                        if (++Counter > 2)
                        {
                            Counter = 0;
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.Center + Vector2.UnitY * 15, 12f * Vector2.UnitX.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45))), ModContent.ProjectileType<Bee>(), npc.damage / 5, 0f, Main.myPlayer);
                                Projectile.NewProjectile(npc.Center + Vector2.UnitY * 15, -12f * Vector2.UnitX.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45, 45))), ModContent.ProjectileType<Bee>(), npc.damage / 5, 0f, Main.myPlayer);
                            }
                        }
                    }
                    else if (Timer > 900) //wait for 1 second then return to normal AI
                    {
                        Timer = 0;
                        npc.netUpdate = true;
                    }

                    if (npc.netUpdate)
                    {
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendData(27, -1, -1, null, npc.whoAmI);
                            NetUpdateMaso(npc.whoAmI);
                        }
                        npc.netUpdate = false;
                    }
                    return false;
                }

                Counter++;
                if (Counter >= 90)
                {
                    Counter = 0;
                    Counter2++;
                    if (Counter2 > 3)
                    {
                        if (Main.netMode != 1)
                            Projectiles.FargoGlobalProjectile.XWay(16, npc.Center, ProjectileID.Stinger, 6, 11, 1);
                        Counter2 = 0;
                    }
                    else
                    {
                        if (Main.netMode != 1)
                            Projectiles.FargoGlobalProjectile.XWay(8, npc.Center, ProjectileID.Stinger, 6, 11, 1);
                    }
                }
            }

            return true;
        }

        public void SkeletronAI(NPC npc)
        {
            skeleBoss = npc.whoAmI;
            if (!masoBool[0])
            {
                masoBool[0] = true;
                if (Main.netMode != 1 && !NPC.downedBoss3)
                    Item.NewItem(npc.Hitbox, ModContent.ItemType<BloodiedSkull>());
            }
            if (Counter != 0)
            {
                Timer++;

                if (Timer >= 3600)
                {
                    Timer = 0;

                    bool otherHandStillAlive = false;
                    for (int i = 0; i < 200; i++) //look for hand that belongs to me
                    {
                        if (Main.npc[i].active && Main.npc[i].type == NPCID.SkeletronHand && Main.npc[i].ai[1] == npc.whoAmI)
                        {
                            otherHandStillAlive = true;
                            break;
                        }
                    }

                    if (Main.netMode != 1)
                    {
                        int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.SkeletronHand, npc.whoAmI, 0f, 0f, 0f, 0f, npc.target);
                        if (n != 200)
                        {
                            Main.npc[n].ai[0] = (Counter == 1) ? 1f : -1f;
                            Main.npc[n].ai[1] = npc.whoAmI;
                            Main.npc[n].life = Main.npc[n].lifeMax / 4;
                            Main.npc[n].netUpdate = true;
                            if (Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                    }

                    if (!otherHandStillAlive)
                    {
                        if (Counter == 1)
                            Counter = 2;
                        else
                            Counter = 1;
                    }
                    else
                    {
                        Counter = 0;
                    }
                }
            }

            if (npc.ai[1] == 1f || npc.ai[1] == 2f) //spinning or DG mode
            {
                npc.localAI[2]++;
                float ratio = (float)npc.life / npc.lifeMax;
                float threshold = 20f + 100f * ratio;
                if (npc.localAI[2] >= threshold) //spray bones
                {
                    npc.localAI[2] = 0f;
                    if (threshold > 0 && npc.HasPlayerTarget && Main.netMode != 1)
                    {
                        Vector2 speed = Vector2.Normalize(Main.player[npc.target].Center - npc.Center) * 6f;
                        for (int i = 0; i < 8; i++)
                        {
                            Vector2 vel = speed.RotatedBy(Math.PI * 2 / 8 * i);
                            vel += npc.velocity * (1f - ratio);
                            vel.Y -= Math.Abs(vel.X) * 0.2f;
                            Projectile.NewProjectile(npc.Center, vel, ModContent.ProjectileType<SkeletronBone>(), npc.defDamage / 9 * 2, 0f, Main.myPlayer);
                        }
                    }
                }
            }

            if (npc.ai[1] == 2f)
            {
                npc.defense = 9999;
                npc.damage = npc.defDamage * 15;
            }
        }

        public void SkeletronHandAI(NPC npc)
        {
            if (npc.life < npc.lifeMax / 2)
            {
                if (--Counter < 0)
                {
                    Counter = (int)(60f + 120f * npc.life / npc.lifeMax);
                    if (npc.HasPlayerTarget && Main.netMode != 1)
                    {
                        Vector2 speed = new Vector2(0f, -3f);
                        for (int i = 0; i < 8; i++)
                        {
                            Vector2 vel = speed.RotatedBy(Math.PI * 2 / 8 * i);
                            vel.Y -= Math.Abs(vel.X) * 0.2f;
                            Projectile.NewProjectile(npc.Center, vel, ModContent.ProjectileType<SkeletronBone>(), npc.damage / 4, 0f, Main.myPlayer);
                        }
                    }
                }
                if (--Counter2 < 0)
                {
                    Counter2 = 300;
                    if (npc.HasPlayerTarget && Main.netMode != 1)
                    {
                        Vector2 speed = Main.player[npc.target].Center - npc.Center;
                        speed.X += Main.rand.Next(-20, 21);
                        speed.Y += Main.rand.Next(-20, 21);
                        speed.Normalize();
                        speed *= 3f;
                        Projectile.NewProjectile(npc.Center, speed, ProjectileID.Skull, npc.damage / 4, 0, Main.myPlayer, -1f, 0f);
                    }
                }
            }

            if (Main.npc[(int)npc.ai[1]].ai[1] == 1f || Main.npc[(int)npc.ai[1]].ai[1] == 2f) //spinning or DG mode
            {
                if (!masoBool[0])
                {
                    masoBool[0] = true;
                    if (Main.netMode != 1 && npc.HasPlayerTarget) //throw undead miner
                    {
                        float gravity = 0.4f; //shoot down
                        const float time = 60f;
                        Vector2 distance = Main.player[npc.target].Center - npc.Center;
                        distance.X = distance.X / time;
                        distance.Y = distance.Y / time - 0.5f * gravity * time;
                        int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.BoneThrowingSkeleton);
                        if (n != 200)
                        {
                            Main.npc[n].velocity = distance;
                            if (Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                    }
                }
            }
            else
            {
                masoBool[0] = false;
            }
        }

        public void WallOfFleshAI(NPC npc)
        {
            wallBoss = npc.whoAmI;

            if (npc.ai[3] == 0f) //when spawned in, make one eye invul
            {
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].type == NPCID.WallofFleshEye && Main.npc[i].realLife == npc.whoAmI)
                    {
                        Main.npc[i].ai[2] = -1f;
                        Main.npc[i].netUpdate = true;
                        npc.ai[3] = 1f;
                        npc.netUpdate = true;
                        break;
                    }
                }
            }

            if (masoBool[0]) //phase 2
            {
                if (++Counter > 600)
                {
                    Counter = 0;
                    Counter2 = 0;
                    masoBool[1] = !masoBool[1];
                    masoBool[2] = false;
                    npc.netUpdate = true;
                }
                else if (Counter < 240) //special attacks
                {
                    if (masoBool[1]) //cursed inferno attack
                    {
                        if (++Counter2 > 5)
                        {
                            Counter2 = 0;
                            if (!masoBool[2])
                            {
                                masoBool[2] = true;
                                Timer = (int)(npc.Center.X + Math.Sign(npc.velocity.X) * 2500);
                            }
                            if (Math.Abs(npc.Center.X - Timer) > 800)
                            {
                                Vector2 spawnPos = new Vector2(Timer, npc.Center.Y);
                                Main.PlaySound(SoundID.Item34, spawnPos);
                                Timer += Math.Sign(npc.velocity.X) * -24; //wall of flame advances closer
                                const int offsetY = 800;
                                const int speed = 14;
                                if (Main.netMode != 1)
                                {
                                    Projectile.NewProjectile(spawnPos + Vector2.UnitY * offsetY, Vector2.UnitY * -speed, ModContent.ProjectileType<CursedFlamethrower>(), npc.damage / 4, 0f, Main.myPlayer);
                                    Projectile.NewProjectile(spawnPos + Vector2.UnitY * offsetY / 2, Vector2.UnitY * speed, ModContent.ProjectileType<CursedFlamethrower>(), npc.damage / 4, 0f, Main.myPlayer);
                                    Projectile.NewProjectile(spawnPos + Vector2.UnitY * -offsetY / 2, Vector2.UnitY * -speed, ModContent.ProjectileType<CursedFlamethrower>(), npc.damage / 4, 0f, Main.myPlayer);
                                    Projectile.NewProjectile(spawnPos + Vector2.UnitY * -offsetY, Vector2.UnitY * speed, ModContent.ProjectileType<CursedFlamethrower>(), npc.damage / 4, 0f, Main.myPlayer);

                                    //Projectile.NewProjectile(spawnPos + Vector2.UnitY * offsetY, Vector2.UnitY * -speed, ProjectileID.CursedFlameHostile, npc.damage / 4, 0f, Main.myPlayer);
                                    //Projectile.NewProjectile(spawnPos + Vector2.UnitY * -offsetY, Vector2.UnitY * speed, ProjectileID.CursedFlameHostile, npc.damage / 4, 0f, Main.myPlayer);
                                }
                            }
                            else
                            {
                                Counter = 240; //immediately end
                            }
                        }
                    }
                    else //ichor attack
                    {
                        if (++Counter2 > 10)
                        {
                            Counter2 = 0;
                            if (Main.netMode != 1)
                            {
                                Vector2 target = npc.Center;
                                target.X += Math.Sign(npc.velocity.X) * 1800f * Counter / 240f; //gradually targets further and further
                                for (int i = 0; i < 4; i++)
                                {
                                    Vector2 speed = target - npc.Center;
                                    speed.Y -= Math.Abs(speed.X) * 0.2f; //account for gravity
                                                                         //speed.Normalize(); speed *= 8f;
                                    speed /= 45f * 3f; //ichor has 3 updates per tick
                                    speed += npc.velocity / 3f;
                                    speed.X += Main.rand.Next(-20, 21) * 0.08f;
                                    speed.Y += Main.rand.Next(-20, 21) * 0.08f;
                                    Projectile.NewProjectile(npc.Center, speed, ProjectileID.GoldenShowerHostile, npc.damage / 5, 0f, Main.myPlayer);
                                }
                            }
                        }
                    }
                }
            }
            else if (npc.life < npc.lifeMax / 2) //enter phase 2
            {
                masoBool[0] = true;
                npc.netUpdate = true;
                Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
            }

            /*if (--Counter < 0)
            {
                Counter = 60 + (int)(120f * npc.life / npc.lifeMax);
                if (Main.netMode != 1 && npc.HasPlayerTarget && Main.player[npc.target].active) //vanilla spaz p1 shoot fireball code
                {
                    Vector2 Speed = Main.player[npc.target].Center - npc.Center;
                    if (Speed.X * npc.velocity.X > 0) //don't shoot fireballs behind myself
                    {
                        Speed.Normalize();
                        int Damage;
                        Speed *= 10f;
                        Damage = npc.damage / 12;
                        Speed.X += Main.rand.Next(-40, 41) * 0.02f;
                        Speed.Y += Main.rand.Next(-40, 41) * 0.02f;
                        Speed += Main.player[npc.target].velocity / 5;
                        Projectile.NewProjectile(npc.Center + Speed * 4f, Speed, ProjectileID.CursedFlameHostile, Damage, 0f, Main.myPlayer);
                    }
                }
            }

            if (--Timer < 0) //ichor vomit
            {
                Timer = 300 + 300 * (int)((float)npc.life / npc.lifeMax);
                if (npc.HasPlayerTarget && Main.netMode != 1 && Main.player[npc.target].active)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 speed = Main.player[npc.target].Center - npc.Center;
                        speed.Y -= Math.Abs(speed.X) * 0.2f; //account for gravity
                        speed.Normalize();
                        speed *= 8f;
                        speed += npc.velocity / 3f;
                        speed.X += Main.rand.Next(-20, 21) * 0.08f;
                        speed.Y += Main.rand.Next(-20, 21) * 0.08f;
                        Projectile.NewProjectile(npc.Center, speed, ProjectileID.GoldenShowerHostile, npc.damage / 25, 0f, Main.myPlayer);
                    }
                }
            }*/

            if (npc.HasPlayerTarget && (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 3000))
            {
                npc.TargetClosest(true);
                if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 3000)
                {
                    npc.position.X += 60 * Math.Sign(npc.velocity.X); //move faster to despawn
                    if (!masoBool[3]) //drop a resummon
                    {
                        masoBool[3] = true;
                        if (Main.netMode != 1 && !Main.hardMode)
                            Item.NewItem(npc.Hitbox, ModContent.ItemType<FleshierDoll>());
                    }
                }
                else if (Math.Abs(npc.velocity.X) > 6f)
                {
                    npc.position.X -= (Math.Abs(npc.velocity.X) - 6f) * Math.Sign(npc.velocity.X);
                }
            }
            else if (Math.Abs(npc.velocity.X) > 6f)
                npc.position.X -= (Math.Abs(npc.velocity.X) - 6f) * Math.Sign(npc.velocity.X);

            //dont do aura with swarm active
            if (Main.player[Main.myPlayer].active & !Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].ZoneUnderworldHeight && !(bool)ModLoader.GetMod("Fargowiltas").Call("SwarmActive"))
            {
                float velX = npc.velocity.X;
                if (velX > 5f)
                    velX = 5f;
                else if (velX < -5f)
                    velX = -5f;

                for (int i = 0; i < 10; i++) //dust
                {
                    Vector2 dustPos = new Vector2(2000 * npc.direction, 0f).RotatedBy(Math.PI / 3 * (-0.5 + Main.rand.NextDouble()));
                    int d = Dust.NewDust(npc.Center + dustPos, 0, 0, DustID.Fire);
                    Main.dust[d].scale += 1f;
                    Main.dust[d].velocity.X = velX;
                    Main.dust[d].velocity.Y = npc.velocity.Y;
                    Main.dust[d].noGravity = true;
                    Main.dust[d].noLight = true;
                }

                if (++npc.localAI[1] > 15f)
                {
                    npc.localAI[1] = 0f; //tongue the player if they're 2000-2800 units away
                    if (Math.Abs(2400 - npc.Distance(Main.player[Main.myPlayer].Center)) < 400)
                    {
                        if (!Main.player[Main.myPlayer].tongued)
                            Main.PlaySound(15, Main.player[Main.myPlayer].Center, 0);
                        Main.player[Main.myPlayer].AddBuff(BuffID.TheTongue, 10);
                    }
                }
            }

            if (npc.life < npc.lifeMax / 10)
            {
                Counter++;
                if (!masoBool[3])
                {
                    masoBool[3] = true;
                    Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                }
            }
        }

        public bool WallOfFleshEyeAI(NPC npc)
        {
            if (masoBool[3])
                return true;

            if (npc.realLife != -1 && Main.npc[npc.realLife].GetGlobalNPC<EModeGlobalNPC>().masoBool[0]
                && Main.npc[npc.realLife].GetGlobalNPC<EModeGlobalNPC>().Counter < 240)
                npc.localAI[1] = 0; //dont fire during mouth's special attacks

            float maxTime = 540f;

            if (npc.realLife != -1 && Main.npc[npc.realLife].GetGlobalNPC<EModeGlobalNPC>().masoBool[3])
            {
                if (npc.ai[1] < maxTime - 180) //dont lower this if it's already telegraphing laser
                    maxTime = 240f;

                npc.localAI[1] = 0f; //no more regular lasers
            }

            if (++npc.ai[1] >= maxTime)
            {
                npc.ai[1] = 0f;
                if (npc.ai[2] == 0f)
                    npc.ai[2] = 1f;
                else
                    npc.ai[2] *= -1f;

                if (npc.ai[2] > 0 && Main.netMode != 1) //FIRE LASER
                {
                    Vector2 speed = Vector2.UnitX.RotatedBy(npc.ai[3]);
                    float ai0 = (npc.realLife != -1 && Main.npc[npc.realLife].velocity.X > 0) ? 1f : 0f;
                    if (Main.netMode != 1)
                        Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("PhantasmalDeathrayWOF"), npc.damage / 4, 0f, Main.myPlayer, ai0, npc.whoAmI);

                }
                npc.netUpdate = true;
            }

            if (npc.ai[2] >= 0f)
            {
                npc.alpha = 175;
                npc.dontTakeDamage = true;
                if (npc.ai[1] <= 90)
                {
                    masoBool[3] = true;
                    npc.AI();
                    masoBool[3] = false;
                    npc.localAI[1] = 0f;
                    npc.rotation = npc.ai[3];
                    return false;
                }
                else
                {
                    npc.ai[2] = 1;
                }
            }
            else
            {
                npc.alpha = 0;
                npc.dontTakeDamage = false;
                if (npc.ai[1] > maxTime - 180f)
                {
                    if (Main.rand.Next(4) < 3) //dust telegraphs switch
                    {
                        int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 90, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 114, default(Color), 3.5f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 1.8f;
                        Main.dust[dust].velocity.Y -= 0.5f;
                        if (Main.rand.Next(4) == 0)
                        {
                            Main.dust[dust].noGravity = false;
                            Main.dust[dust].scale *= 0.5f;
                        }
                    }

                    float stopTime = maxTime - 90f;
                    if (npc.ai[1] == stopTime) //shoot warning dust in phase 2
                    {
                        int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                        if (t != -1)
                        {
                            if (npc.Distance(Main.player[t].Center) < 3000)
                                Main.PlaySound(15, (int)Main.player[t].position.X, (int)Main.player[t].position.Y, 0);
                            npc.ai[2] = -2f;
                            npc.ai[3] = (npc.Center - Main.player[t].Center).ToRotation();
                            if (npc.realLife != -1 && Main.npc[npc.realLife].velocity.X > 0)
                                npc.ai[3] += (float)Math.PI;

                            float ai0 = (npc.realLife != -1 && Main.npc[npc.realLife].velocity.X > 0) ? 1f : 0f;
                            Vector2 speed = Vector2.UnitX.RotatedBy(npc.ai[3]);
                            if (Main.netMode != 1)
                                Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("PhantasmalDeathrayWOFS"), 0, 0f, Main.myPlayer, ai0, npc.whoAmI);
                        }
                        npc.netUpdate = true;
                    }
                    else if (npc.ai[1] > stopTime)
                    {
                        masoBool[3] = true;
                        npc.AI();
                        masoBool[3] = false;
                        npc.localAI[1] = 0f;
                        npc.rotation = npc.ai[3];
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
