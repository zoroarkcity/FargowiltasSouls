using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.NPCs.DeviBoss
{
    [AutoloadBossHead]
    public class DeviBoss : ModNPC
    {
        public int[] attackQueue = new int[4];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deviantt");
            Main.npcFrameCount[npc.type] = 4;
        }

        /*public override bool Autoload(ref string name)
        {
            return false;
        }*/

        public override void SetDefaults()
        {
            npc.width = 120;
            npc.height = 120;
            npc.damage = 64;
            npc.defense = 10;
            npc.lifeMax = 12000;
            npc.HitSound = SoundID.NPCHit9;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.npcSlots = 50f;
            npc.knockBackResist = 0f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.aiStyle = -1;
            npc.netAlways = true;
            npc.hide = true;
            npc.buffImmune[BuffID.Chilled] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Suffocation] = true;
            npc.buffImmune[mod.BuffType("Lethargic")] = true;
            npc.buffImmune[mod.BuffType("ClippedWings")] = true;
            //npc.buffImmune[mod.BuffType("MutantNibble")] = true;
            //npc.buffImmune[mod.BuffType("OceanicMaul")] = true;
            npc.timeLeft = NPC.activeTime * 30;
            npc.GetGlobalNPC<FargoSoulsGlobalNPC>().SpecialEnchantImmune = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Stigma");
            musicPriority = (MusicPriority)10;

            npc.value = Item.buyPrice(0, 5);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = (int)(npc.damage * 0.5f);
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(npc.localAI[0]);
            writer.Write(npc.localAI[1]);
            writer.Write(npc.localAI[2]);
            writer.Write(npc.localAI[3]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            npc.localAI[0] = reader.ReadSingle();
            npc.localAI[1] = reader.ReadSingle();
            npc.localAI[2] = reader.ReadSingle();
            npc.localAI[3] = reader.ReadSingle();
        }

        public override void AI()
        {
            FargoSoulsGlobalNPC.deviBoss = npc.whoAmI;

            if (npc.localAI[3] == 0)
            {
                npc.TargetClosest();
                if (npc.timeLeft < 30)
                    npc.timeLeft = 30;

                if (npc.Distance(Main.player[npc.target].Center) < 2000)
                {
                    npc.localAI[3] = 1;
                    Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                    RefreshAttackQueue();
                    if (Main.netMode != 1)
                    {
                        int number = 0;
                        for (int index = 999; index >= 0; --index)
                        {
                            if (!Main.projectile[index].active)
                            {
                                number = index;
                                break;
                            }
                        }
                        if (number >= 0)
                        {
                            if (Main.netMode == 0)
                            {
                                Projectile projectile = Main.projectile[number];
                                projectile.SetDefaults(mod.ProjectileType("DeviBoss"));
                                projectile.Center = npc.Center;
                                projectile.owner = Main.myPlayer;
                                projectile.velocity.X = 0;
                                projectile.velocity.Y = 0;
                                projectile.damage = 0;
                                projectile.knockBack = 0f;
                                projectile.identity = number;
                                projectile.gfxOffY = 0f;
                                projectile.stepSpeed = 1f;
                                projectile.ai[1] = npc.whoAmI;

                                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("DeviRitual2"), 0, 0f, Main.myPlayer, 0f, npc.whoAmI);
                            }
                            else if (Main.netMode == 2)
                            {
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("DeviRitual2"), 0, 0f, Main.myPlayer, 0f, npc.whoAmI);
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("DeviBoss"), 0, 0f, Main.myPlayer, 0, npc.whoAmI);
                            }
                        }
                    }
                }
            }
            /*else if (npc.localAI[3] == 1)
            {
                Aura(2000f, mod.BuffType("GodEater"), true, 86);
            }*/
            else if (Main.player[Main.myPlayer].active && npc.Distance(Main.player[Main.myPlayer].Center) < 3000f)
            {
                Main.player[Main.myPlayer].AddBuff(mod.BuffType("DeviPresence"), 2);
            }

            int projectileDamage = npc.damage / (npc.localAI[3] > 1 ? 5 : 6);

            Player player = Main.player[npc.target];
            npc.direction = npc.spriteDirection = npc.position.X < player.position.X ? 1 : -1;
            Vector2 targetPos;
            switch ((int)npc.ai[0])
            {
                case -2: //ACTUALLY dead
                    if (!AliveCheck(player))
                        break;
                    npc.velocity *= 0.9f;
                    for (int i = 0; i < 5; i++)
                    {
                        int d = Dust.NewDust(npc.position, npc.width, npc.height, 86, 0f, 0f, 0, default(Color), 2.5f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity *= 12f;
                    }
                    if (++npc.ai[1] > 180)
                    {
                        if (Main.netMode != 1)
                        {
                            if (!NPC.AnyNPCs(ModLoader.GetMod("Fargowiltas").NPCType("Deviantt")))
                            {
                                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModLoader.GetMod("Fargowiltas").NPCType("Deviantt"));
                                if (n != 200 && Main.netMode == 2)
                                    NetMessage.SendData(23, -1, -1, null, n);
                            }
                        }
                        npc.NPCLoot();
                        npc.life = 0;
                        npc.active = false;
                    }
                    break;

                case -1: //phase 2 transition
                    npc.velocity *= 0.9f;
                    npc.dontTakeDamage = true;
                    if (npc.buffType[0] != 0)
                        npc.DelBuff(0);
                    if (++npc.ai[1] > 120)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            int d = Dust.NewDust(npc.position, npc.width, npc.height, 86, 0f, 0f, 0, default(Color), 1.5f);
                            Main.dust[d].noGravity = true;
                            Main.dust[d].velocity *= 4f;
                        }
                        npc.localAI[3] = 2; //this marks p2
                        if (++npc.ai[2] > 15)
                        {
                            int heal = (int)(npc.lifeMax / 2 / 60 * Main.rand.NextFloat(1.5f, 2f));
                            npc.life += heal;
                            if (npc.life > npc.lifeMax)
                                npc.life = npc.lifeMax;
                            CombatText.NewText(npc.Hitbox, CombatText.HealLife, heal);
                        }
                        if (npc.ai[1] > 210)
                        {
                            RefreshAttackQueue();
                            npc.localAI[2] = 0;
                            GetNextAttack();
                            npc.dontTakeDamage = false;
                        }
                    }
                    else if (npc.ai[1] == 120)
                    {
                        /*for (int i = 0; i < Main.maxProjectiles; i++)
                            if (Main.projectile[i].active && Main.projectile[i].friendly && !Main.projectile[i].minion && Main.projectile[i].damage > 0)
                                Main.projectile[i].Kill();
                        for (int i = 0; i < Main.maxProjectiles; i++)
                            if (Main.projectile[i].active && Main.projectile[i].friendly && !Main.projectile[i].minion && Main.projectile[i].damage > 0)
                                Main.projectile[i].Kill();
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("AbomRitual"), npc.damage / 2, 0f, Main.myPlayer, 0f, npc.whoAmI);
                        }*/
                        Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                    }
                    break;

                case 0: //track player, decide which attacks to use
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    npc.dontTakeDamage = false;

                    targetPos = player.Center;
                    targetPos.X += 500 * (npc.Center.X < targetPos.X ? -1 : 1);
                    if (npc.Distance(targetPos) > 50)
                        Movement(targetPos, npc.localAI[3] > 0 ? 0.15f : 2f, npc.localAI[3] > 0 ? 12f : 1200f);

                    if (npc.localAI[3] > 0) //in range, fight has begun, choose attacks
                    {
                        npc.netUpdate = true;
                        GetNextAttack();
                    }
                    break;

                case 1: //teleport marx hammers
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    npc.velocity = Vector2.Zero;
                    if (++npc.ai[1] > (npc.localAI[3] > 1 ? 10 : 20) && npc.ai[2] < (npc.localAI[3] > 1 ? 5 : 3))
                    {
                        npc.ai[1] = 0;
                        npc.ai[2]++;

                        TeleportDust();
                        if (Main.netMode != 1)
                        {
                            bool wasOnLeft = npc.Center.X < player.Center.X;
                            npc.Center = player.Center + 200 * Vector2.UnitX.RotatedBy(Main.rand.NextFloat(0, 2 * (float)Math.PI));
                            if (wasOnLeft ? npc.Center.X < player.Center.X : npc.Center.X > player.Center.X)
                            {
                                float x = player.Center.X - npc.Center.X;
                                npc.position.X += x * 2;
                            }
                            npc.netUpdate = true;
                        }
                        TeleportDust();
                        Main.PlaySound(SoundID.Item84, npc.Center);
                    }

                    if (npc.ai[1] == 60) //finished all the prior teleports, now attack
                    {
                        for (int i = 0; i < 36; i++) //dust ring
                        {
                            Vector2 vector6 = Vector2.UnitY * 9f;
                            vector6 = vector6.RotatedBy((i - (36 / 2 - 1)) * 6.28318548f / 36) + npc.Center;
                            Vector2 vector7 = vector6 - npc.Center;
                            int d = Dust.NewDust(vector6 + vector7, 0, 0, 246, 0f, 0f, 0, default(Color), 3f);
                            Main.dust[d].noLight = true;
                            Main.dust[d].noGravity = true;
                            Main.dust[d].velocity = vector7;
                        }

                        Main.PlaySound(SoundID.Item92, npc.Center);

                        if (Main.netMode != 1) //hammers
                        {
                            const float retiRad = 150;
                            const float spazRad = 100;
                            const int retiTime = 45;
                            const int spazTime = 45;

                            float retiSpeed = 2 * (float)Math.PI * retiRad / retiTime;
                            float spazSpeed = 2 * (float)Math.PI * spazRad / spazTime;
                            float retiAcc = retiSpeed * retiSpeed / retiRad * npc.direction;
                            float spazAcc = spazSpeed * spazSpeed / spazRad * -npc.direction;

                            for (int i = 0; i < 4; i++)
                            {
                                if (npc.localAI[3] > 1) //p2 throw another set of hammers
                                    Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(Math.PI / 2 * i) * retiSpeed, mod.ProjectileType("DeviHammer"), projectileDamage, 0f, Main.myPlayer, retiAcc, retiTime);
                                Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(Math.PI / 2 * i + Math.PI / 4) * spazSpeed, mod.ProjectileType("DeviHammer"), projectileDamage, 0f, Main.myPlayer, spazAcc, spazTime);
                            }
                        }
                    }
                    else if (npc.ai[1] > 90)
                    {
                        npc.netUpdate = true;
                        if (npc.localAI[3] > 1 && ++npc.localAI[0] < 3)
                        {
                            npc.ai[2] = 0; //reset tp counter and attack again
                        }
                        else
                        {
                            GetNextAttack();
                        }
                    }
                    break;

                case 2: //heart barrages
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    targetPos = player.Center;
                    targetPos.X += 400 * (npc.Center.X < targetPos.X ? -1 : 1);
                    if (npc.Distance(targetPos) > 50)
                        Movement(targetPos, 0.2f);

                    if (--npc.ai[1] < 0)
                    {
                        npc.netUpdate = true;
                        npc.ai[1] = 75;
                        if (++npc.ai[2] > 3)
                        {
                            GetNextAttack();
                        }
                        else
                        {
                            if (Main.netMode != 1)
                            {
                                int damage = (int)(npc.damage / 3.2); //comes out to 20 raw, fake hearts ignore the usual multipliers

                                Vector2 spawnVel = npc.DirectionFrom(Main.player[npc.target].Center) * 10f;
                                for (int i = -3; i < 3; i++)
                                {
                                    Projectile.NewProjectile(npc.Center, spawnVel.RotatedBy(Math.PI / 7 * i),
                                        mod.ProjectileType("FakeHeart2"), damage, 0f, Main.myPlayer, 20, 30);
                                }
                                for (int i = -5; i < 5; i++)
                                {
                                    Projectile.NewProjectile(npc.Center, 1.5f * spawnVel.RotatedBy(Math.PI / 10 * i),
                                        mod.ProjectileType("FakeHeart2"), damage, 0f, Main.myPlayer, 20, 30 + 5 * Math.Abs(i));
                                }
                            }
                        }
                        break;
                    }
                    break;

                case 3: //slow while shooting wyvern orb spirals
                    if (!AliveCheck(player) || Phase2Check())
                        break;
                    
                    targetPos = player.Center + player.DirectionTo(npc.Center) * 375;
                    if (npc.Distance(targetPos) > 50)
                        Movement(targetPos, 0.15f);

                    if (--npc.ai[1] < 0)
                    {
                        npc.netUpdate = true;
                        npc.ai[1] = 120;

                        if (++npc.ai[2] > 3)
                        {
                            GetNextAttack();
                        }
                        else
                        {
                            if (Main.netMode != 1)
                            {
                                int max = npc.localAI[3] > 1 ? 8 : 12;
                                Vector2 vel = Vector2.Normalize(npc.velocity);
                                for (int i = 0; i < max; i++)
                                {
                                    Projectile.NewProjectile(npc.Center, vel.RotatedBy(2 * Math.PI / max * i), mod.ProjectileType("DeviLightBall"), projectileDamage, 0f, Main.myPlayer, 0f, .012f * npc.direction);
                                    if (npc.localAI[3] > 1)
                                        Projectile.NewProjectile(npc.Center, vel.RotatedBy(2 * Math.PI / max * i), mod.ProjectileType("DeviLightBall"), projectileDamage, 0f, Main.myPlayer, 0f, .012f * -npc.direction);
                                }
                            }
                        }
                    }
                    break;

                case 4: //mimics
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    targetPos = player.Center;
                    targetPos.X += 300 * (npc.Center.X < targetPos.X ? -1 : 1);
                    targetPos.Y -= 200;
                    if (npc.Distance(targetPos) > 50)
                        Movement(targetPos, 0.15f);

                    if (++npc.ai[1] < 120)
                    {
                        if (++npc.ai[2] > 20)
                        {
                            npc.ai[2] = 0;

                            Main.PlaySound(SoundID.Item84, npc.Center);

                            int delay = npc.localAI[3] > 1 ? 30 : 45;
                            Vector2 target = player.Center;
                            target.Y -= 400;
                            Vector2 speed = (target - npc.Center) / delay;

                            for (int i = 0; i < 20; i++) //dust spray
                            {
                                int d = Dust.NewDust(npc.Center, 0, 0, Main.rand.Next(2) == 0 ? DustID.GoldFlame : DustID.SilverCoin, speed.X, speed.Y, 0, default(Color), 2f);
                                Main.dust[d].noGravity = true;
                            }

                            if (Main.netMode != 1)
                            {
                                int type = mod.ProjectileType("DeviMimic");
                                float ai0 = player.position.Y - 16;

                                if (npc.localAI[3] > 1)
                                {
                                    type = mod.ProjectileType("DeviBigMimic");
                                    ai0 = player.whoAmI;
                                }

                                Projectile.NewProjectile(npc.Center, speed, type, projectileDamage, 0f, Main.myPlayer, ai0, delay);
                            }
                        }
                    }
                    else if (npc.ai[1] == 180) //big wave of mimics, aimed ahead of you
                    {
                        Main.PlaySound(SoundID.Item84, npc.Center);

                        int modifier = 150;
                        if (player.velocity.X != 0)
                            modifier *= Math.Sign(player.velocity.X);
                        else
                            modifier *= Math.Sign(player.Center.X - npc.Center.X);

                        Vector2 target = player.Center;
                        target.Y -= 400;

                        for (int j = 0; j < 7; j++)
                        {
                            int delay = npc.localAI[3] > 1 ? 30 : 60;
                            Vector2 speed = (target - npc.Center) / delay;

                            for (int i = 0; i < 20; i++) //dust spray
                            {
                                int d = Dust.NewDust(npc.Center, 0, 0, Main.rand.Next(2) == 0 ? DustID.GoldFlame : DustID.SilverCoin, speed.X, speed.Y, 0, default(Color), 2f);
                                Main.dust[d].noGravity = true;
                            }

                            if (Main.netMode != 1)
                            {
                                int type = mod.ProjectileType("DeviMimic");
                                float ai0 = player.position.Y - 16;

                                if (npc.localAI[3] > 1)
                                {
                                    type = mod.ProjectileType("DeviBigMimic");
                                    ai0 = player.whoAmI;
                                }

                                Projectile.NewProjectile(npc.Center, speed, type, projectileDamage, 0f, Main.myPlayer, ai0, delay);
                            }

                            target.X += modifier;
                        }
                    }
                    else if (npc.ai[1] > 240)
                    {
                        GetNextAttack();
                    }
                    break;

                case 5: //frostballs and nados
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    targetPos = player.Center + player.DirectionTo(npc.Center) * 350;
                    if (npc.Distance(targetPos) > 50)
                        Movement(targetPos, 0.2f);

                    if (++npc.ai[1] > 360)
                    {
                        GetNextAttack();
                    }
                    if (++npc.ai[2] > (npc.localAI[3] > 1 ? 15 : 30))
                    {
                        npc.netUpdate = true;
                        npc.ai[2] = 0;

                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center, new Vector2(4f, 0f).RotatedBy(Main.rand.NextDouble() * Math.PI * 2),
                                mod.ProjectileType("FrostfireballHostile"), projectileDamage, 0f, Main.myPlayer, npc.target, 15f);
                        }
                    }
                    if (npc.localAI[3] > 1 && --npc.ai[3] < 0) //spawn sandnado
                    {
                        npc.netUpdate = true;
                        npc.ai[3] = 110;

                        Vector2 target = player.Center;
                        target.X += player.velocity.X * 90;
                        target.Y -= 150;
                        Projectile.NewProjectile(target, Vector2.Zero, ProjectileID.SandnadoHostileMark, 0, 0f, Main.myPlayer);

                        int length = (int)npc.Distance(target) / 10;
                        Vector2 offset = npc.DirectionTo(target) * 10f;
                        for (int i = 0; i < length; i++) //dust warning line for sandnado
                        {
                            int d = Dust.NewDust(npc.Center + offset * i, 0, 0, 269, 0f, 0f, 0, new Color());
                            Main.dust[d].noLight = true;
                            Main.dust[d].scale = 1.25f;
                        }
                    }
                    break;

                case 6: //rune wizard
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    if (npc.Distance(Main.LocalPlayer.Center) < 3000f) //auras
                    {
                        if (npc.Distance(Main.LocalPlayer.Center) > 400f)
                        {
                            Main.LocalPlayer.AddBuff(mod.BuffType("Hexed"), 2);
                            Main.LocalPlayer.AddBuff(BuffID.Dazed, 2);
                        }

                        Aura(200, mod.BuffType("Hexed"), false, 119);
                        if (npc.Distance(Main.LocalPlayer.Center) < 200)
                        {
                            Main.LocalPlayer.AddBuff(BuffID.Dazed, 2);
                        }

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

                    npc.velocity = Vector2.Zero;
                    if (++npc.ai[1] == 1)
                    {
                        TeleportDust();
                        if (Main.netMode != 1)
                        {
                            bool wasOnLeft = npc.Center.X < player.Center.X;
                            npc.Center = player.Center + 400 * Vector2.UnitX.RotatedBy(Main.rand.NextFloat(0, 2 * (float)Math.PI));
                            if (wasOnLeft ? npc.Center.X < player.Center.X : npc.Center.X > player.Center.X)
                            {
                                float x = player.Center.X - npc.Center.X;
                                npc.position.X += x * 2;
                            }
                            npc.netUpdate = true;
                        }
                        TeleportDust();
                        Main.PlaySound(SoundID.Item84, npc.Center);
                    }
                    else if (npc.ai[1] == 60)
                    {
                        if (Main.netMode != 1)
                        {
                            for (int i = -1; i <= 1; i++) //rune blast spread
                                Projectile.NewProjectile(npc.Center,
                                    12f * npc.DirectionTo(player.Center).RotatedBy(MathHelper.ToRadians(5) * i),
                                    ProjectileID.RuneBlast, projectileDamage, 0f, Main.myPlayer);

                            if (npc.localAI[3] > 1) //rune blast ring
                            {
                                Vector2 vel = npc.DirectionFrom(Main.player[npc.target].Center) * 8;
                                for (int i = 0; i < 5; i++)
                                {
                                    int p = Projectile.NewProjectile(npc.Center, vel.RotatedBy(2 * Math.PI / 5 * i),
                                        ProjectileID.RuneBlast, projectileDamage, 0f, Main.myPlayer, 1);
                                    if (p != 1000)
                                        Main.projectile[p].timeLeft = 300;
                                }
                            }
                        }
                    }
                    else if (npc.ai[1] > 90)
                    {
                        if (++npc.ai[2] > 3)
                        {
                            GetNextAttack();
                        }
                        else
                        {
                            npc.netUpdate = true;
                            npc.ai[1] = 0;
                        }
                    }
                    break;

                case 7: //moth dust charges
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    if (npc.localAI[0] == 0) //teleport behind you
                    {
                        npc.localAI[0] = 1;
                        npc.ai[1] = -45;

                        TeleportDust();
                        if (Main.netMode != 1)
                        {
                            bool wasOnLeft = npc.Center.X < player.Center.X;
                            npc.Center = player.Center;
                            npc.position.X += wasOnLeft ? 400 : -400;
                            npc.netUpdate = true;
                        }
                        TeleportDust();

                        Main.PlaySound(SoundID.Item84, npc.Center);
                        Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                    }

                    if (++npc.ai[3] > 2)
                    {
                        npc.ai[3] = 0;

                        if (Main.netMode != 1) //make moth dust trail
                            Projectile.NewProjectile(npc.Center, Main.rand.NextVector2Unit() * 2f, mod.ProjectileType("MothDust"), projectileDamage, 0f, Main.myPlayer);
                    }

                    npc.velocity *= 0.9f;
                    if (++npc.ai[1] > (npc.localAI[3] > 1 ? 20 : 30))
                    {
                        npc.netUpdate = true;
                        if (++npc.ai[2] > 5)
                        {
                            GetNextAttack();
                        }
                        else
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.velocity = npc.DirectionTo(player.Center + player.velocity) * 20f;
                        }
                    }
                    break;

                case 8: //while dashing
                    if (Phase2Check())
                        break;

                    npc.direction = npc.spriteDirection = Math.Sign(npc.velocity.X);

                    if (++npc.ai[3] > 2)
                    {
                        npc.ai[3] = 0;

                        if (Main.netMode != 1) //make moth dust trail
                            Projectile.NewProjectile(npc.Center, Main.rand.NextVector2Unit() * 2f, mod.ProjectileType("MothDust"), projectileDamage, 0f, Main.myPlayer);
                    }

                    if (++npc.ai[1] > 30)
                    {
                        npc.netUpdate = true;
                        npc.ai[0]--;
                        npc.ai[1] = 0;
                    }
                    break;

                case 9: //mage skeleton attacks
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    npc.velocity = npc.DirectionTo(player.Center) * 3f;

                    if (++npc.ai[1] == 1)
                    {
                        for (int i = 0; i < 60; i++) //warning dust ring
                        {
                            Vector2 vector6 = Vector2.UnitY * 40f;
                            vector6 = vector6.RotatedBy((i - (60 / 2 - 1)) * 6.28318548f / 60) + npc.Center;
                            Vector2 vector7 = vector6 - npc.Center;
                            int d = Dust.NewDust(vector6 + vector7, 0, 0, DustID.Shadowflame, 0f, 0f, 0, default(Color), 2f);
                            Main.dust[d].velocity = vector7;
                        }
                        Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                    }
                    else if (npc.ai[1] < 120) //spam shadowbeams after delay
                    {
                        if (++npc.ai[2] > (npc.localAI[3] > 1 ? 60 : 90))
                        {
                            if (++npc.ai[3] == 1) //store rotation briefly before shooting
                            {
                                npc.localAI[0] = npc.DirectionTo(player.Center).ToRotation();
                            }
                            else if (npc.ai[3] > 6)
                            {
                                npc.ai[3] = 0;

                                if (Main.netMode != 1) //shoot a shadowbeam
                                {
                                    int p = Projectile.NewProjectile(npc.Center, 6f * Vector2.UnitX.RotatedBy(npc.localAI[0]), ProjectileID.ShadowBeamHostile, projectileDamage, 0f, Main.myPlayer);
                                    if (p != Main.maxProjectiles)
                                        Main.projectile[p].timeLeft = 300;
                                }
                            }
                        }
                    }
                    else if (npc.ai[1] < 240)
                    {
                        npc.ai[3] = 0;
                        npc.localAI[0] = 0;

                        if (++npc.ai[2] > (npc.localAI[3] > 1 ? 20 : 40)) //shoot diabolist bolts
                        {
                            npc.ai[2] = 0;
                            if (Main.netMode != 1)
                            {
                                float speed = npc.localAI[3] > 1 ? 16 : 8;
                                Vector2 blastPos = npc.Center + Main.rand.NextFloat(1, 2) * npc.Distance(player.Center) * npc.DirectionTo(player.Center);
                                int p = Projectile.NewProjectile(npc.Center, speed * npc.DirectionTo(player.Center), ProjectileID.InfernoHostileBolt, projectileDamage, 0f, Main.myPlayer, blastPos.X, blastPos.Y);
                                if (p != Main.maxProjectiles)
                                    Main.projectile[p].timeLeft = 300;
                            }
                        }
                    }
                    else
                    {
                        npc.velocity /= 2;

                        if (npc.ai[1] == 300) //spray ragged caster bolts
                        {
                            Main.PlaySound(36, (int)npc.Center.X, (int)npc.Center.Y, -1, 1f, 0f); //eoc roar

                            if (Main.netMode != 1)
                            {
                                int max = npc.localAI[3] > 1 ? 50 : 25;
                                for (int i = 0; i < max; i++)
                                {
                                    int p = Projectile.NewProjectile(npc.Center, 4f * Vector2.UnitX.RotatedBy(Main.rand.NextFloat((float)Math.PI * 2)), mod.ProjectileType("DeviLostSoul"), projectileDamage, 0f, Main.myPlayer);
                                    if (p != Main.maxProjectiles)
                                        Main.projectile[p].timeLeft = 300;
                                }
                            }
                        }

                        if (npc.ai[1] > 360)
                        {
                            GetNextAttack();
                        }
                    }
                    break;

                case 10: //baby guardians
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    if (++npc.ai[1] < 180)
                    {
                        targetPos = player.Center;
                        targetPos.Y -= 200;
                        if (npc.Distance(targetPos) > 50)
                            Movement(targetPos, 0.3f);

                        //warning dust
                        for (int i = 0; i < 3; i++)
                        {
                            int d = Dust.NewDust(npc.Center, 0, 0, DustID.Fire, 0f, 0f, 0, default(Color), 3f);
                            Main.dust[d].noGravity = true;
                            Main.dust[d].noLight = true;
                            Main.dust[d].velocity *= 12f;
                        }
                    }
                    else if (npc.ai[1] == 180)
                    {
                        npc.netUpdate = true;

                        Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);

                        if (Main.netMode != 1) //shoot guardians
                        {
                            for (int i = -1; i <= 1; i++) //left and right sides
                            {
                                if (i == 0)
                                    continue;

                                for (int j = -5; j <= 5; j++)
                                {
                                    Vector2 spawnPos = player.Center;
                                    spawnPos.X += 1200 * i;
                                    spawnPos.Y += 100 * j;
                                    Vector2 vel = 18 * Vector2.UnitX * -i;
                                    Projectile.NewProjectile(spawnPos, vel, mod.ProjectileType("DeviGuardian"), npc.damage / 3, 0f, Main.myPlayer);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (++npc.ai[2] > 1)
                        {
                            npc.ai[2] = 0;
                            Main.PlaySound(SoundID.Item21, npc.Center);

                            if (Main.netMode != 1)
                            {
                                Vector2 spawnPos = npc.Center;
                                spawnPos.X += Main.rand.Next(-150, 151);
                                spawnPos.Y += 600;
                                Vector2 vel = Main.rand.NextFloat(18f, 24f) * Vector2.UnitY;//Vector2.Normalize(player.Center - spawnPos);
                                Projectile.NewProjectile(spawnPos, vel, mod.ProjectileType("DeviGuardian"), npc.damage / 3, 0f, Main.myPlayer);
                            }
                        }

                        if (npc.ai[1] > 300)
                        {
                            GetNextAttack();
                        }

                        npc.velocity *= 0.9f;

                        if (npc.localAI[3] > 1 && npc.ai[1] == 240) //surprise!
                        {
                            Main.PlaySound(36, (int)npc.Center.X, (int)npc.Center.Y, -1, 1f, 0f); //eoc roar

                            if (Main.netMode != 1)
                            {
                                for (int i = -1; i <= 1; i++) //left and right sides
                                {
                                    if (i == 0)
                                        continue;

                                    for (int j = -5; j <= 5; j++)
                                    {
                                        Vector2 spawnPos = player.Center;
                                        spawnPos.X += 1200 * i;
                                        spawnPos.Y += 100 * j;
                                        Vector2 vel = 24 * Vector2.UnitX * -i;
                                        Projectile.NewProjectile(spawnPos, vel, mod.ProjectileType("DeviGuardian"), npc.damage / 3, 0f, Main.myPlayer);
                                    }
                                }
                            }
                        }
                    }
                    break;

                case 11: //noah/irisu geyser rain
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    if (npc.localAI[0] == 0 && npc.localAI[1] == 0)
                    {
                        npc.localAI[0] = npc.Center.X;
                        npc.localAI[1] = npc.Center.Y;
                    }

                    targetPos = player.Center;
                    targetPos.Y -= 350;
                    if (npc.Distance(targetPos) > 50)
                        Movement(targetPos, 0.15f);

                    if (++npc.ai[1] < 120)
                    {
                        if (++npc.ai[2] > 2)
                        {
                            npc.ai[2] = 0;
                            if (Main.netMode != 1)
                            {
                                Main.NewText("spew hearts upwards, beginning noah/irisu rain");
                            }
                        }
                    }
                    else if (npc.ai[1] < 360)
                    {
                        if (--npc.ai[3] < 0)
                        {
                            npc.netUpdate = true;
                            npc.ai[3] = 90;

                            if (Main.netMode != 1)
                            {
                                Main.NewText("rain down hearts");
                            }
                        }
                    }
                    else
                    {
                        GetNextAttack();
                    }
                    break;

                case 12: //lilith cross ray hearts
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    targetPos = player.Center + player.DirectionTo(npc.Center) * 400;
                    if (npc.Distance(targetPos) > 50)
                        Movement(targetPos, 0.3f);

                    if (++npc.ai[2] > 60)
                    {
                        if (++npc.ai[3] > 3)
                        {
                            npc.ai[3] = 0;
                            if (Main.netMode != 1)
                            {
                                Main.NewText("spray lilith cross ray heart near you");
                            }
                        }

                        if (npc.ai[2] > 90)
                        {
                            npc.netUpdate = true;
                            npc.ai[2] = 0;
                        }
                    }

                    if (++npc.ai[1] > 360)
                    {
                        GetNextAttack();
                    }
                    break;

                case 13: //that one boss that was a bunch of gems burst rain but with butterflies
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    npc.velocity *= 0.95f;

                    if (npc.ai[2] == 0)
                    {
                        npc.ai[2] = 1;

                        if (Main.netMode != 1)
                        {
                            Main.NewText("spawn the butterflies");
                        }
                    }

                    if (++npc.ai[1] > 120)
                    {
                        GetNextAttack();
                    }
                    break;

                case 14: //medusa ray
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    if (npc.localAI[0] == 0)
                    {
                        npc.localAI[0] = 1;
                        npc.velocity = Vector2.Zero;
                    }

                    if (++npc.ai[2] > 60)
                    {
                        npc.ai[2] = 0;

                        int threshold = 7;// npc.localAI[3] > 1 ? 7 : 6;

                        //only make rings in p2 and before firing ray
                        if (npc.localAI[3] > 1 && npc.ai[3] < threshold)
                        {
                            if (Main.netMode != 1)
                            {
                                SpawnSphereRing(6, 6f, projectileDamage, 0.5f);
                                SpawnSphereRing(6, 6f, projectileDamage, -.5f);
                            }
                        }

                        if (++npc.ai[3] < threshold - 3) //medusa warning
                        {
                            npc.netUpdate = true;
                            Main.PlaySound(36, (int)npc.Center.X, (int)npc.Center.Y, -1, 1f, 0f); //eoc roar

                            for (int i = 0; i < 60; i++) //warning dust ring
                            {
                                Vector2 vector6 = Vector2.UnitY * 20f;
                                vector6 = vector6.RotatedBy((i - (60 / 2 - 1)) * 6.28318548f / 60) + npc.Center;
                                Vector2 vector7 = vector6 - npc.Center;
                                int d = Dust.NewDust(vector6 + vector7, 0, 0, DustID.GoldFlame, 0f, 0f, 0, default(Color), 2f);
                                Main.dust[d].noGravity = true;
                                Main.dust[d].velocity = vector7;
                            }
                        }
                        else if (npc.ai[3] == threshold - 3) //petrify
                        {
                            Main.PlaySound(4, npc.Center, 17);

                            if (npc.Distance(Main.LocalPlayer.Center) < 3000 && Collision.CanHitLine(npc.Center, 0, 0, Main.LocalPlayer.Center, 0, 0))
                            {
                                if (Math.Sign(Main.LocalPlayer.direction) == Math.Sign(npc.Center.X - Main.LocalPlayer.Center.X))
                                    Main.LocalPlayer.AddBuff(BuffID.Stoned, 300);

                                Vector2 target = Main.LocalPlayer.Center;
                                int length = (int)npc.Distance(target) / 10;
                                Vector2 offset = npc.DirectionTo(target) * 10f;
                                for (int i = 0; i < length; i++) //dust indicator
                                {
                                    int d = Dust.NewDust(npc.Center + offset * i, 0, 0, DustID.GoldFlame, 0f, 0f, 0, new Color());
                                    Main.dust[d].noLight = true;
                                    Main.dust[d].scale = 1f;
                                }
                            }
                        }
                        else if (npc.ai[3] < threshold) //ray warning
                        {
                            npc.netUpdate = true;

                            for (int i = 0; i < 80; i++) //warning dust ring
                            {
                                Vector2 vector6 = Vector2.UnitY * 40f;
                                vector6 = vector6.RotatedBy((i - (80 / 2 - 1)) * 6.28318548f / 80) + npc.Center;
                                Vector2 vector7 = vector6 - npc.Center;
                                int d = Dust.NewDust(vector6 + vector7, 0, 0, 86, 0f, 0f, 0, default(Color), 2.5f);
                                Main.dust[d].noGravity = true;
                                Main.dust[d].velocity = vector7;
                            }
                            
                            npc.localAI[1] = npc.DirectionTo(player.Center).ToRotation(); //store for aiming ray
                        }
                        else if (npc.ai[3] == threshold) //fire deathray
                        {
                            Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);

                            npc.velocity = -2f * Vector2.UnitX.RotatedBy(npc.localAI[1]);

                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(npc.localAI[1]), mod.ProjectileType("DeviBigDeathray"), npc.damage / 2, 0f, Main.myPlayer, 0f, npc.whoAmI);
                            }
                        }
                    }

                    if (++npc.ai[1] > 600)//(npc.localAI[3] > 1 ? 540 : 600))
                    {
                        GetNextAttack();
                    }
                    break;

                case 15: //sparkling love
                    if (!AliveCheck(player) || Phase2Check())
                        break;
                    
                    if (++npc.ai[1] < 120)
                    {
                        npc.velocity = Vector2.Zero;

                        if (npc.ai[2] == 0) //spawn weapon
                        {
                            npc.netUpdate = true;

                            double angle = npc.position.X < player.position.X ? -Math.PI / 4 : Math.PI / 4;
                            npc.ai[2] = (float)angle * -4f / 30;

                            const int spacing = 80;
                            Vector2 offset = Vector2.UnitY.RotatedBy(angle) * -spacing;

                            if (Main.netMode != 1)
                            {
                                for (int i = 0; i < 12; i++)
                                    Projectile.NewProjectile(npc.Center + offset * i, Vector2.Zero, mod.ProjectileType("MutantSword"), npc.damage / 3, 0f, Main.myPlayer, npc.whoAmI, spacing * i);
                                Projectile.NewProjectile(npc.Center + offset.RotatedBy(MathHelper.ToRadians(20)) * 7, Vector2.Zero, mod.ProjectileType("MutantSword"), npc.damage / 3, 0f, Main.myPlayer, npc.whoAmI, 60 * 4);
                                Projectile.NewProjectile(npc.Center + offset.RotatedBy(MathHelper.ToRadians(-20)) * 7, Vector2.Zero, mod.ProjectileType("MutantSword"), npc.damage / 3, 0f, Main.myPlayer, npc.whoAmI, 60 * 4);
                                Projectile.NewProjectile(npc.Center + offset.RotatedBy(MathHelper.ToRadians(40)) * 28, Vector2.Zero, mod.ProjectileType("MutantSword"), npc.damage / 3, 0f, Main.myPlayer, npc.whoAmI, 60 * 4);
                                Projectile.NewProjectile(npc.Center + offset.RotatedBy(MathHelper.ToRadians(-40)) * 28, Vector2.Zero, mod.ProjectileType("MutantSword"), npc.damage / 3, 0f, Main.myPlayer, npc.whoAmI, 60 * 4);
                            }
                        }

                        npc.direction = npc.spriteDirection = Math.Sign(npc.ai[2]);
                    }
                    else if (npc.ai[1] == 120) //start swinging
                    {
                        targetPos = player.Center;
                        targetPos.X += 200 * (npc.Center.X < targetPos.X ? -1 : 1);
                        npc.velocity = (targetPos - npc.Center) / 30;
                        npc.netUpdate = true;

                        npc.direction = npc.spriteDirection = Math.Sign(npc.ai[2]);
                    }
                    else if (npc.ai[1] < 150)
                    {
                        npc.ai[3] += npc.ai[2];
                        npc.direction = npc.spriteDirection = Math.Sign(npc.ai[2]);
                    }
                    else
                    {
                        targetPos = player.Center + player.DirectionTo(npc.Center) * 400;
                        if (npc.Distance(targetPos) > 50)
                            Movement(targetPos, 0.2f);

                        if (npc.ai[1] > 240)
                        {
                            GetNextAttack();
                        }
                    }
                    break;

                case 16: //pause between attacks
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    targetPos = player.Center + player.DirectionTo(npc.Center) * 250;
                    if (npc.Distance(targetPos) > 50)
                        Movement(targetPos, 0.125f);

                    if (++npc.ai[1] > (npc.localAI[3] > 1 ? 60 : 90))
                    {
                        npc.netUpdate = true;
                        npc.ai[0] = attackQueue[(int)npc.localAI[2]];
                        npc.ai[1] = 0;
                        if (++npc.localAI[2] >= attackQueue.Length)
                        {
                            npc.localAI[2] = 0;
                            RefreshAttackQueue();

                            if (Main.netMode != 1) //spawn ritual for strong attacks
                            {
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("DeviRitual"), npc.damage / 2, 0f, Main.myPlayer, 0f, npc.whoAmI);
                            }
                        }
                    }
                    break;

                default:
                    Main.NewText("UH OH, STINKY");
                    npc.netUpdate = true;
                    npc.ai[0] = 0;
                    goto case 0;
            }
        }

        private void GetNextAttack()
        {
            npc.TargetClosest();
            npc.netUpdate = true;
            npc.ai[0] = 16;// attackQueue[(int)npc.localAI[2]];
            npc.ai[1] = 0;
            npc.ai[2] = 0;
            npc.ai[3] = 0;
            npc.localAI[0] = 0;
            npc.localAI[1] = 0;
        }

        private void RefreshAttackQueue()
        {
            npc.netUpdate = true;

            int[] newQueue = new int[4];
            for (int i = 0; i < 3; i++)
            {
                newQueue[i] = Main.rand.Next(1, 11);

                bool repeat = false;
                if (newQueue[i] == 8) //this is the middle of an attack pattern, dont pick it
                    repeat = true;
                for (int j = 0; j < 3; j++) //cant pick attack that's queued in the previous set
                    if (newQueue[i] == attackQueue[j])
                        repeat = true;
                for (int j = i; j >= 0; j--) //can't pick attack that's already queued in this set
                    if (i != j && newQueue[i] == newQueue[j])
                        repeat = true;

                if (repeat) //retry this one if needed
                    i--;
            }

            do
            {
                newQueue[3] = Main.rand.Next(11, 16);
            }
            while (newQueue[3] == attackQueue[3]);

            attackQueue = newQueue;

            Main.NewText("queue: "
                + attackQueue[0].ToString() + " "
                + attackQueue[1].ToString() + " "
                + attackQueue[2].ToString() + " "
                + attackQueue[3].ToString());
        }

        private void SpawnSphereRing(int max, float speed, int damage, float rotationModifier)
        {
            if (Main.netMode == 1) return;
            float rotation = 2f * (float)Math.PI / max;
            Vector2 vel = Vector2.UnitY * speed;
            int type = mod.ProjectileType("DeviRingHeart");
            for (int i = 0; i < max; i++)
            {
                vel = vel.RotatedBy(rotation);
                Projectile.NewProjectile(npc.Center, vel, type, damage, 0f, Main.myPlayer, rotationModifier * npc.spriteDirection, speed);
            }
            Main.PlaySound(SoundID.Item84, npc.Center);
        }

        private void Aura(float distance, int buff, bool reverse = false, int dustid = DustID.GoldFlame, bool checkDuration = false)
        {
            //works because buffs are client side anyway :ech:
            Player p = Main.player[Main.myPlayer];
            float range = npc.Distance(p.Center);
            if (reverse ? range > distance && range < 5000f : range < distance)
                p.AddBuff(buff, checkDuration && Main.expertMode && Main.expertDebuffTime > 1 ? 1 : 2);

            for (int i = 0; i < 30; i++)
            {
                Vector2 offset = new Vector2();
                double angle = Main.rand.NextDouble() * 2d * Math.PI;
                offset.X += (float)(Math.Sin(angle) * distance);
                offset.Y += (float)(Math.Cos(angle) * distance);
                Dust dust = Main.dust[Dust.NewDust(
                    npc.Center + offset - new Vector2(4, 4), 0, 0,
                    dustid, 0, 0, 100, Color.White, 1.5f)];
                dust.velocity = npc.velocity;
                if (Main.rand.Next(3) == 0)
                    dust.velocity += Vector2.Normalize(offset) * (reverse ? 5f : -5f);
                dust.noGravity = true;
            }
        }

        private bool AliveCheck(Player player)
        {
            if ((!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 5000f) && npc.localAI[3] > 0)
            {
                npc.TargetClosest();
                player = Main.player[npc.target];
                if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 5000f)
                {
                    if (npc.timeLeft > 30)
                        npc.timeLeft = 30;
                    npc.velocity.Y -= 1f;
                    if (npc.timeLeft == 1)
                    {
                        if (npc.position.Y < 0)
                            npc.position.Y = 0;
                        if (Main.netMode != 1 && !NPC.AnyNPCs(ModLoader.GetMod("Fargowiltas").NPCType("Deviantt")))
                        {
                            for (int i = 0; i < 1000; i++)
                                if (Main.projectile[i].active && Main.projectile[i].hostile)
                                    Main.projectile[i].Kill();
                            for (int i = 0; i < 1000; i++)
                                if (Main.projectile[i].active && Main.projectile[i].hostile)
                                    Main.projectile[i].Kill();
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModLoader.GetMod("Fargowiltas").NPCType("Deviantt"));
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                    }
                    return false;
                }
            }
            if (npc.timeLeft < 600)
                npc.timeLeft = 600;
            return true;
        }

        private bool Phase2Check()
        {
            if (npc.localAI[3] > 1)
                return false;

            if (npc.life < npc.lifeMax * 0.5)
            {
                if (Main.netMode != 1)
                {
                    npc.ai[0] = -1;
                    npc.ai[1] = 0;
                    npc.ai[2] = 0;
                    npc.ai[3] = 0;
                    npc.localAI[0] = 0;
                    npc.localAI[1] = 0;
                    npc.netUpdate = true;
                    for (int i = 0; i < 1000; i++)
                        if (Main.projectile[i].active && Main.projectile[i].hostile)
                            Main.projectile[i].Kill();
                    for (int i = 0; i < 1000; i++)
                        if (Main.projectile[i].active && Main.projectile[i].hostile)
                            Main.projectile[i].Kill();
                }
                return true;
            }
            return false;
        }

        private void Movement(Vector2 targetPos, float speedModifier, float cap = 12f)
        {
            if (npc.Center.X < targetPos.X)
            {
                npc.velocity.X += speedModifier;
                if (npc.velocity.X < 0)
                    npc.velocity.X += speedModifier * 2;
            }
            else
            {
                npc.velocity.X -= speedModifier;
                if (npc.velocity.X > 0)
                    npc.velocity.X -= speedModifier * 2;
            }
            if (npc.Center.Y < targetPos.Y)
            {
                npc.velocity.Y += speedModifier;
                if (npc.velocity.Y < 0)
                    npc.velocity.Y += speedModifier * 2;
            }
            else
            {
                npc.velocity.Y -= speedModifier;
                if (npc.velocity.Y > 0)
                    npc.velocity.Y -= speedModifier * 2;
            }
            if (Math.Abs(npc.velocity.X) > cap)
                npc.velocity.X = cap * Math.Sign(npc.velocity.X);
            if (Math.Abs(npc.velocity.Y) > cap)
                npc.velocity.Y = cap * Math.Sign(npc.velocity.Y);
        }

        private void TeleportDust()
        {
            for (int index1 = 0; index1 < 25; ++index1)
            {
                int index2 = Dust.NewDust(npc.position, npc.width, npc.height, 272, 0f, 0f, 100, new Color(), 2f);
                Main.dust[index2].noGravity = true;
                Main.dust[index2].velocity *= 7f;
                Main.dust[index2].noLight = true;
                int index3 = Dust.NewDust(npc.position, npc.width, npc.height, 272, 0f, 0f, 100, new Color(), 1f);
                Main.dust[index3].velocity *= 4f;
                Main.dust[index3].noGravity = true;
                Main.dust[index3].noLight = true;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("Lovestruck"), 240);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 3; i++)
            {
                int d = Dust.NewDust(npc.position, npc.width, npc.height, 86, 0f, 0f, 0, default(Color), 1f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 3f;
            }
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (Main.LocalPlayer.loveStruck)
                damage = 0;
            return true;
        }

        public override bool CheckDead()
        {
            npc.life = 1;
            npc.active = true;
            if (npc.localAI[3] < 2)
            {
                npc.localAI[3] = 2;
                /*if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("AbomRitual"), npc.damage / 2, 0f, Main.myPlayer, 0f, npc.whoAmI);
                }*/
            }
            if (Main.netMode != 1 && npc.ai[0] > -2)
            {
                npc.ai[0] = -2;
                npc.ai[1] = 0;
                npc.ai[2] = 0;
                npc.ai[3] = 0;
                npc.localAI[0] = 0;
                npc.localAI[1] = 0;
                npc.dontTakeDamage = true;
                npc.netUpdate = true;
                for (int i = 0; i < 1000; i++)
                    if (Main.projectile[i].active && Main.projectile[i].damage > 0 && Main.projectile[i].hostile)
                        Main.projectile[i].Kill();
                for (int i = 0; i < 1000; i++)
                    if (Main.projectile[i].active && Main.projectile[i].damage > 0 && Main.projectile[i].hostile)
                        Main.projectile[i].Kill();
            }
            return false;
        }

        public override void NPCLoot()
        {
            FargoSoulsWorld.downedDevi = true;
            if (Main.netMode == 2)
                NetMessage.SendData(7); //sync world
            
            if (Main.rand.Next(10) == 0)
                Item.NewItem(npc.Hitbox, mod.ItemType("DeviTrophy"));
            
            npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("DeviatingEnergy"), Main.rand.Next(11) + 10);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        public override void FindFrame(int frameHeight)
        {
            if (++npc.frameCounter > 6)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (npc.frame.Y >= 4 * frameHeight)
                    npc.frame.Y = 0;
            }
        }

        public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
        {
            spriteEffects = npc.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }
    }
}