using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace FargowiltasSouls.NPCs.AbomBoss
{
    [AutoloadBossHead]
    public class AbomBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abominationn");
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
            npc.damage = 300;
            npc.defense = 60;
            npc.lifeMax = 1600000;
            npc.HitSound = SoundID.NPCHit57;
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
            npc.buffImmune[mod.BuffType("MutantNibble")] = true;
            npc.buffImmune[mod.BuffType("OceanicMaul")] = true;
            npc.timeLeft = NPC.activeTime * 30;
            npc.GetGlobalNPC<FargoSoulsGlobalNPC>().SpecialEnchantImmune = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Stigma");
            musicPriority = (MusicPriority)11;
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
            FargoSoulsGlobalNPC.abomBoss = npc.whoAmI;

            if (npc.localAI[3] == 0)
            {
                npc.TargetClosest();
                if (npc.timeLeft < 30)
                    npc.timeLeft = 30;
                if (npc.Distance(Main.player[npc.target].Center) < 2000)
                {
                    npc.localAI[3] = 1;
                    Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
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
                                projectile.SetDefaults(mod.ProjectileType("AbomBoss"));
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

                                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("AbomRitual2"), 0, 0f, Main.myPlayer, 0f, npc.whoAmI);
                            }
                            else if (Main.netMode == 2)
                            {
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("AbomRitual2"), 0, 0f, Main.myPlayer, 0f, npc.whoAmI);
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("AbomBoss"), 0, 0f, Main.myPlayer, 0, npc.whoAmI);
                            }
                        }
                    }
                }
            }
            else if (npc.localAI[3] == 1)
            {
                Aura(2000f, mod.BuffType("GodEater"), true, 86, false, false);
            }

            if (Main.player[Main.myPlayer].active && npc.Distance(Main.player[Main.myPlayer].Center) < 3000f)
            {
                Main.player[Main.myPlayer].AddBuff(mod.BuffType("AbomPresence"), 2);
            }

            Player player = Main.player[npc.target];
            npc.direction = npc.spriteDirection = npc.position.X < player.position.X ? 1 : -1;
            Vector2 targetPos;
            float speedModifier;
            switch ((int)npc.ai[0])
            {
                case -3: //ACTUALLY dead
                    if (!AliveCheck(player))
                        break;
                    npc.velocity *= 0.9f;
                    npc.dontTakeDamage = true;
                    for (int i = 0; i < 5; i++)
                    {
                        int d = Dust.NewDust(npc.position, npc.width, npc.height, 87, 0f, 0f, 0, default(Color), 2.5f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity *= 12f;
                    }
                    if (++npc.ai[1] > 180)
                    {
                        if (Main.netMode != 1)
                        {
                            for (int i = 0; i < 30; i++)
                                Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(Main.rand.NextDouble() * Math.PI) * Main.rand.NextFloat(30f), mod.ProjectileType("AbomDeathScythe"), 0, 0f, Main.myPlayer);

                            if (!NPC.AnyNPCs(ModLoader.GetMod("Fargowiltas").NPCType("Abominationn")))
                            {
                                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModLoader.GetMod("Fargowiltas").NPCType("Abominationn"));
                                if (n != 200 && Main.netMode == 2)
                                    NetMessage.SendData(23, -1, -1, null, n);
                            }
                        }
                        npc.life = 0;
                        npc.dontTakeDamage = false;
                        npc.checkDead();
                    }
                    break;

                case -2: //dead, begin last stand
                    if (!AliveCheck(player))
                        break;
                    npc.velocity *= 0.9f;
                    for (int i = 0; i < 5; i++)
                    {
                        int d = Dust.NewDust(npc.position, npc.width, npc.height, 87, 0f, 0f, 0, default(Color), 1.5f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity *= 4f;
                    }
                    if (++npc.ai[1] > 120)
                    {
                        npc.netUpdate = true;
                        npc.ai[0] = 15;
                        npc.ai[1] = 0;
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
                            int d = Dust.NewDust(npc.position, npc.width, npc.height, 87, 0f, 0f, 0, default(Color), 1.5f);
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
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = 0;
                            npc.netUpdate = true;
                        }
                    }
                    else if (npc.ai[1] == 120)
                    {
                        for (int i = 0; i < Main.maxProjectiles; i++)
                            if (Main.projectile[i].active && Main.projectile[i].friendly && !Main.projectile[i].minion && Main.projectile[i].damage > 0)
                                Main.projectile[i].Kill();
                        for (int i = 0; i < Main.maxProjectiles; i++)
                            if (Main.projectile[i].active && Main.projectile[i].friendly && !Main.projectile[i].minion && Main.projectile[i].damage > 0)
                                Main.projectile[i].Kill();
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("AbomRitual"), npc.damage / 2, 0f, Main.myPlayer, 0f, npc.whoAmI);
                        }
                        Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                    }
                    break;

                case 0: //track player, throw scythes (makes 4way using orig vel in p1, 8way targeting you in p2)
                    if (!AliveCheck(player) || Phase2Check())
                        break;
                    npc.dontTakeDamage = false;
                    targetPos = player.Center;
                    targetPos.X += 500 * (npc.Center.X < targetPos.X ? -1 : 1);
                    if (npc.Distance(targetPos) > 50)
                    {
                        speedModifier = npc.localAI[3] > 0 ? 0.5f : 2f;
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
                        if (npc.localAI[3] > 0)
                        {
                            if (Math.Abs(npc.velocity.X) > 24)
                                npc.velocity.X = 24 * Math.Sign(npc.velocity.X);
                            if (Math.Abs(npc.velocity.Y) > 24)
                                npc.velocity.Y = 24 * Math.Sign(npc.velocity.Y);
                        }
                    }
                    if (npc.localAI[3] > 0) //in range, fight has begun
                    {
                        npc.ai[1]++;
                        if (npc.ai[3] == 0)
                        {
                            npc.ai[3] = 1;
                            if (Main.netMode != 1) //phase 2 saucers
                            {
                                int max = 3;// npc.localAI[3] > 1 ? 5 : 3;
                                for (int i = 0; i < max; i++)
                                {
                                    float ai2 = i * 2 * (float)Math.PI / max; //rotation offset
                                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("AbomSaucer"), 0, npc.whoAmI, 0, ai2);
                                    if (n != Main.maxNPCs && Main.netMode == 2)
                                        NetMessage.SendData(23, -1, -1, null, n);
                                }
                            }
                        }
                    }
                    if (npc.ai[1] > 120)
                    {
                        npc.netUpdate = true;
                        npc.TargetClosest();
                        npc.ai[1] = 60;
                        if (++npc.ai[2] > 5)
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.velocity = npc.DirectionTo(player.Center) * 2f;
                        }
                        else if (Main.netMode != 1)
                        {
                            float ai0 = npc.Distance(player.Center) / 30 * 2f;
                            float ai1 = npc.localAI[3] > 1 ? 1f : 0f;
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(player.Center) * 30f, mod.ProjectileType("AbomScytheSplit"), npc.damage / 4, 0f, Main.myPlayer, ai0, ai1);
                        }
                    }
                    /*else if (npc.ai[1] == 90)
                    {
                        Projectile.NewProjectile(npc.Center, npc.DirectionTo(player.Center + player.velocity * 30) * 30f, mod.ProjectileType("AbomScythe"), npc.damage / 5, 0f, Main.myPlayer);
                    }*/
                    break;

                case 1: //flaming scythe spread (shoots out further in p2)
                    if (!AliveCheck(player) || Phase2Check())
                        break;
                    npc.velocity = npc.DirectionTo(player.Center) * 6f;
                    if (++npc.ai[1] > 120)
                    {
                        npc.ai[1] = 60;
                        if (++npc.ai[2] > 3)
                        {
                            npc.ai[0]++;
                            npc.ai[2] = 0;
                            npc.TargetClosest();
                        }
                        else
                        {
                            float baseDelay = npc.localAI[3] > 1 ? 40 : 20;
                            float speed = npc.localAI[3] > 1 ? 30 : 10;
                            if (Main.netMode != 1)
                                for (int i = 0; i < 6; i++)
                                    Projectile.NewProjectile(npc.Center, npc.DirectionTo(player.Center).RotatedBy(Math.PI / 3 * i + Math.PI / 6) * speed, mod.ProjectileType("AbomScytheFlaming"), npc.damage / 4, 0f, Main.myPlayer, baseDelay, baseDelay + 90);
                            Main.PlaySound(36, (int)npc.Center.X, (int)npc.Center.Y, -1, 1f, 0f);
                        }
                        npc.netUpdate = true;
                        break;
                    }
                    break;

                case 2: //pause and then initiate dash
                    if (!AliveCheck(player) || Phase2Check())
                        break;
                    npc.velocity *= 0.9f;
                    if (++npc.ai[1] > 30)
                    {
                        npc.netUpdate = true;
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        if (++npc.ai[2] > 5)
                        {
                            npc.ai[0]++; //go to next attack after dashes
                            npc.ai[2] = 0;
                        }
                        else
                        {
                            npc.velocity = npc.DirectionTo(player.Center + player.velocity) * 30f;
                        }
                    }
                    break;

                case 3: //while dashing (p2 makes side scythes)
                    if (Phase2Check())
                        break;
                    npc.direction = npc.spriteDirection = Math.Sign(npc.velocity.X);
                    if (++npc.ai[3] > 5)
                    {
                        npc.ai[3] = 0;
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity), mod.ProjectileType("AbomSickle"), npc.damage / 4, 0, Main.myPlayer);
                            if (npc.localAI[3] > 1)
                            {
                                Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(Math.PI / 2), mod.ProjectileType("AbomSickle"), npc.damage / 4, 0, Main.myPlayer);
                                Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(-Math.PI / 2), mod.ProjectileType("AbomSickle"), npc.damage / 4, 0, Main.myPlayer);
                            }
                        }
                    }
                    if (++npc.ai[1] > 30)
                    {
                        npc.netUpdate = true;
                        npc.ai[0]--;
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                    }
                    break;

                case 4: //choose the next attack
                    if (!AliveCheck(player))
                        break;
                    Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                    npc.netUpdate = true;
                    npc.TargetClosest();
                    npc.ai[0] += ++npc.localAI[0];
                    if (npc.localAI[0] >= 3) //reset p1 hard option counter
                        npc.localAI[0] = 0;
                    break;

                case 5: //mutant scythe 8way (p2 also shoots flaming scythes)
                    if (!AliveCheck(player) || Phase2Check())
                        break;
                    targetPos = player.Center;
                    if (npc.localAI[3] > 1)
                        targetPos += player.DirectionTo(npc.Center) * 400;
                    else
                        targetPos.X += 500 * (npc.Center.X < targetPos.X ? -1 : 1);
                    if (npc.Distance(targetPos) > 50)
                    {
                        Movement(targetPos, 0.5f);
                    }
                    if (++npc.ai[1] > 60)
                    {
                        npc.ai[1] = 0;
                        if (++npc.ai[2] > 3)
                        {
                            npc.ai[0] = 8;
                            npc.ai[2] = 0;
                            npc.TargetClosest();
                        }
                        else
                        {
                            if (Main.netMode != 1)
                            {
                                for (int i = 0; i < 8; i++)
                                    Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(Math.PI / 4 * i) * 10f, mod.ProjectileType("MutantScythe1"), npc.damage / 5, 0f, Main.myPlayer, npc.whoAmI);
                            }
                            Main.PlaySound(36, (int)npc.Center.X, (int)npc.Center.Y, -1, 1f, 0f);
                        }
                        npc.netUpdate = true;
                        break;
                    }
                    break;

                case 6: //flocko swarm (p2 shoots ice waves horizontally after)
                    if (Phase2Check())
                        break;
                    npc.velocity *= 0.99f;
                    if (npc.ai[2] == 0)
                    {
                        npc.ai[2] = 1;
                        if (Main.netMode != 1)
                        {
                            for (int i = -3; i <= 3; i++) //make flockos
                            {
                                if (i == 0) //dont shoot one straight up
                                    continue;
                                Vector2 speed = new Vector2(Main.rand.NextFloat(40f), Main.rand.NextFloat(-20f, 20f));
                                Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("AbomFlocko"), npc.damage / 4, 0f, Main.myPlayer, npc.whoAmI, 360 / 3 * i);
                            }
                            if (npc.localAI[3] > 1) //prepare ice waves
                            {
                                Vector2 speed = new Vector2(Main.rand.NextFloat(40f), Main.rand.NextFloat(-20f, 20f));
                                Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("AbomFlocko2"), npc.damage / 4, 0f, Main.myPlayer, npc.target, -1);
                                Projectile.NewProjectile(npc.Center, -speed, mod.ProjectileType("AbomFlocko2"), npc.damage / 4, 0f, Main.myPlayer, npc.target, 1);
                            }
                        }

                        Main.PlaySound(SoundID.Item27, npc.Center);
                        for (int index1 = 0; index1 < 30; ++index1)
                        {
                            int index2 = Dust.NewDust(npc.position, npc.width, npc.height, 76, 0.0f, 0.0f, 0, new Color(), 1f);
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].noLight = true;
                            Main.dust[index2].velocity *= 5f;
                        }
                    }
                    if (++npc.ai[1] > 420)
                    {
                        npc.netUpdate = true;
                        npc.ai[0] = 8;
                        npc.ai[1] = 0;
                    }
                    break;

                case 7: //saucer laser spam with rockets (p2 does two spams)
                    if (Phase2Check())
                        break;
                    npc.velocity *= 0.99f;
                    if (++npc.ai[1] > 420)
                    {
                        npc.netUpdate = true;
                        npc.ai[0] = 8;
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                    }
                    else if (npc.ai[1] > 60) //spam lasers, lerp aim
                    {
                        if (npc.localAI[3] > 1) //p2 lock directly onto you
                        {
                            npc.ai[3] = npc.DirectionTo(player.Center).ToRotation();
                        }
                        else //p1 lerps slowly at you
                        {
                            float targetRot = npc.DirectionTo(player.Center).ToRotation();
                            while (targetRot < -(float)Math.PI)
                                targetRot += 2f * (float)Math.PI;
                            while (targetRot > (float)Math.PI)
                                targetRot -= 2f * (float)Math.PI;
                            npc.ai[3] = npc.ai[3].AngleLerp(targetRot, 0.05f);
                        }

                        if (++npc.ai[2] > 1) //spam lasers
                        {
                            npc.ai[2] = 0;
                            Main.PlaySound(SoundID.Item12, npc.Center);
                            if (Main.netMode != 1)
                            {
                                if (npc.localAI[3] > 1) //p2 shoots to either side of you
                                {
                                    Vector2 speed = 16f * npc.ai[3].ToRotationVector2().RotatedBy((Main.rand.NextDouble() - 0.5) * 0.785398185253143 / 2.0);
                                    Projectile.NewProjectile(npc.Center, speed.RotatedBy(MathHelper.ToRadians(25)), mod.ProjectileType("AbomLaser"), npc.damage / 4, 0f, Main.myPlayer);

                                    speed = 16f * npc.ai[3].ToRotationVector2().RotatedBy((Main.rand.NextDouble() - 0.5) * 0.785398185253143 / 2.0);
                                    Projectile.NewProjectile(npc.Center, speed.RotatedBy(MathHelper.ToRadians(-25)), mod.ProjectileType("AbomLaser"), npc.damage / 4, 0f, Main.myPlayer);
                                }
                                else //p1 shoots directly
                                {
                                    Vector2 speed = 16f * npc.ai[3].ToRotationVector2().RotatedBy((Main.rand.NextDouble() - 0.5) * 0.785398185253143 / 2.0);
                                    Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("AbomLaser"), npc.damage / 4, 0f, Main.myPlayer);
                                }
                            }
                        }

                        if (++npc.localAI[2] > 60) //shoot rockets
                        {
                            npc.localAI[2] = 0;
                            if (Main.netMode != 1)
                            {
                                Vector2 vel = (npc.ai[3] + (float)Math.PI / 2).ToRotationVector2();
                                vel *= npc.localAI[3] > 1 ? 5 : 8;
                                Projectile.NewProjectile(npc.Center, vel, mod.ProjectileType("AbomRocket"), npc.damage / 4, 0f, Main.myPlayer, npc.target, 30f);
                                Projectile.NewProjectile(npc.Center, -vel, mod.ProjectileType("AbomRocket"), npc.damage / 4, 0f, Main.myPlayer, npc.target, 30f);

                                Vector2 speed = npc.ai[3].ToRotationVector2().RotatedBy((Main.rand.NextDouble() - 0.5) * 0.785398185253143 / 2.0);
                                speed *= npc.localAI[3] > 1 ? 5 : 8;
                                Projectile.NewProjectile(npc.Center, speed, mod.ProjectileType("AbomRocket"), npc.damage / 4, 0f, Main.myPlayer, npc.target, 60f);
                            }
                        }
                    }
                    else
                    {
                        npc.ai[3] = npc.DirectionFrom(player.Center).ToRotation() - 0.001f;
                        while (npc.ai[3] < -(float)Math.PI)
                            npc.ai[3] += 2f * (float)Math.PI;
                        while (npc.ai[3] > (float)Math.PI)
                            npc.ai[3] -= 2f * (float)Math.PI;

                        Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);

                        //make warning dust
                        for (int i = 0; i < 5; i++)
                        {
                            int d = Dust.NewDust(npc.position, npc.width, npc.height, 87, 0f, 0f, 0, default(Color), 1.5f);
                            Main.dust[d].noGravity = true;
                            Main.dust[d].velocity *= 4f;
                        }
                    }
                    break;

                case 8: //return to beginning in p1, proceed in p2
                    if (!AliveCheck(player) || Phase2Check())
                        break;
                    npc.velocity *= 0.9f;
                    npc.localAI[2] = 0;
                    if (++npc.ai[1] > 120)
                    {
                        Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                        npc.netUpdate = true;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        npc.TargetClosest();
                        if (npc.localAI[3] > 1) //if in p2
                        {
                            if (npc.localAI[1] == 0)
                            {
                                npc.localAI[1] = 1;
                                npc.ai[0] = 15;
                            }
                            else
                            {
                                npc.localAI[1] = 0;
                                npc.ai[0]++;
                            }
                        }
                        else //still in p1
                        {
                            npc.ai[0] = 0;
                        }
                    }
                    break;

                case 9: //beginning of scythe rows and deathray rain
                    if (!AliveCheck(player))
                        break;
                    npc.velocity = Vector2.Zero;
                    npc.localAI[2] = 0;
                    if (++npc.ai[1] == 1)
                    {
                        npc.ai[3] = npc.DirectionTo(player.Center).ToRotation();
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, npc.ai[3].ToRotationVector2(), mod.ProjectileType("AbomDeathraySmall"), 0, 0f, Main.myPlayer);
                    }
                    else if (npc.ai[1] == 61)
                    {
                        if (Main.netMode != 1)
                        {
                            const int max = 12;
                            const float gap = 1200 / max;
                            for (int i = 1; i <= max + 2; i++)
                            {
                                float speed = i * gap / 30;
                                float ai1 = i % 2 == 0 ? -1 : 1;
                                Projectile.NewProjectile(npc.Center, speed * npc.ai[3].ToRotationVector2(), mod.ProjectileType("AbomScytheSpin"), npc.damage * 3 / 8, 0f, Main.myPlayer, npc.whoAmI, ai1);
                            }
                        }
                    }
                    else if (npc.ai[1] > 61 + 330)
                    {
                        npc.netUpdate = true;
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                    }
                    break;

                case 10: //prepare deathray rain
                    if (!AliveCheck(player))
                        break;
                    for (int i = 0; i < 5; i++) //make warning dust
                    {
                        int d = Dust.NewDust(npc.position, npc.width, npc.height, 87, 0f, 0f, 0, default(Color), 1.5f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity *= 4f;
                    }
                    if (npc.ai[2] == 0 && npc.ai[3] == 0) //target one side of arena
                    {
                        npc.ai[2] = npc.Center.X + (player.Center.X < npc.Center.X ? -1400 : 1400);
                    }
                    if (npc.localAI[2] == 0) //direction to dash in next
                    {
                        npc.localAI[2] = npc.ai[2] > npc.Center.X ? -1 : 1;
                    }
                    npc.ai[3] = player.Center.Y - 300;
                    targetPos = new Vector2(npc.ai[2], npc.ai[3]);
                    Movement(targetPos, 0.7f);
                    if (++npc.ai[1] > 120)
                    {
                        Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                        npc.netUpdate = true;
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = npc.localAI[2];
                        npc.ai[3] = 0;
                    }
                    break;

                case 11: //dash and make deathrays
                    npc.localAI[2] = 0;
                    npc.velocity.X = npc.ai[2] * 21f;
                    npc.direction = npc.spriteDirection = Math.Sign(npc.velocity.X);
                    MovementY(player.Center.Y - 250, 0.7f);
                    if (++npc.ai[3] > 5)
                    {
                        npc.ai[3] = 0;
                        Main.PlaySound(SoundID.Item12, npc.Center);
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center, Vector2.UnitY.RotatedBy(MathHelper.ToRadians(20) * (Main.rand.NextDouble() - 0.5)), mod.ProjectileType("AbomDeathrayMark"), npc.damage * 3 / 8, 0f, Main.myPlayer);
                            Projectile.NewProjectile(npc.Center, -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(20) * (Main.rand.NextDouble() - 0.5)), mod.ProjectileType("AbomDeathrayMark"), npc.damage * 3 / 8, 0f, Main.myPlayer);
                        }
                    }
                    if (++npc.ai[1] > 2400 / 21f)
                    {
                        npc.netUpdate = true;
                        npc.velocity.X = npc.ai[2] * 18f;
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        //npc.ai[2] = 0; //will be reused shortly
                        npc.ai[3] = 0;
                    }
                    break;

                case 12: //prepare for next deathrain
                    if (!AliveCheck(player))
                        break;
                    npc.velocity.Y = 0f;
                    for (int i = 0; i < 5; i++) //make warning dust
                    {
                        int d = Dust.NewDust(npc.position, npc.width, npc.height, 87, 0f, 0f, 0, default(Color), 1.5f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity *= 4f;
                    }
                    npc.velocity *= 0.947f;
                    npc.ai[3] += npc.velocity.Length();
                    if (++npc.ai[1] > 180)
                    {
                        Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                        npc.netUpdate = true;
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                    }
                    break;

                case 13: //second deathray dash
                    npc.velocity.X = npc.ai[2] * -21f;
                    MovementY(player.Center.Y - 250, 0.7f);
                    npc.direction = npc.spriteDirection = Math.Sign(npc.velocity.X);
                    if (++npc.ai[3] > 5)
                    {
                        npc.ai[3] = 0;
                        Main.PlaySound(SoundID.Item12, npc.Center);
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center, Vector2.UnitY.RotatedBy(MathHelper.ToRadians(20) * (Main.rand.NextDouble() - 0.5)), mod.ProjectileType("AbomDeathrayMark"), npc.damage * 3 / 8, 0f, Main.myPlayer);
                            Projectile.NewProjectile(npc.Center, -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(20) * (Main.rand.NextDouble() - 0.5)), mod.ProjectileType("AbomDeathrayMark"), npc.damage * 3 / 8, 0f, Main.myPlayer);
                        }
                    }
                    if (++npc.ai[1] > 2400 / 21f)
                    {
                        npc.netUpdate = true;
                        npc.velocity.X = npc.ai[2] * -18f;
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }
                    break;

                case 14: //pause before looping back to first attack
                    if (!AliveCheck(player))
                        break;
                    npc.velocity *= 0.9f;
                    if (++npc.ai[1] > 120)
                    {
                        npc.netUpdate = true;
                        npc.ai[0] = npc.dontTakeDamage ? npc.ai[0] + 1 : 0;
                        npc.ai[1] = 0;
                    }
                    break;

                case 15: //beginning of laevateinn, pause and then sworddash
                    npc.velocity *= 0.9f;
                    if (++npc.ai[1] > 60)
                    {
                        npc.netUpdate = true;
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        if (++npc.ai[2] > 2)
                        {
                            npc.ai[0] += 3;
                            npc.ai[2] = 0;
                        }
                        else
                        {
                            npc.velocity = npc.DirectionTo(player.Center) * 6f;
                        }
                    }
                    else if (npc.ai[1] == 30 && Main.netMode != 1)
                    {
                        npc.netUpdate = true;
                        npc.velocity = Vector2.Zero;
                        if (npc.ai[2] < 2)
                        {
                            Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                            float ai0 = npc.ai[2] == 1 ? -1 : 1;
                            ai0 *= MathHelper.ToRadians(270) / 120;
                            Vector2 vel = npc.DirectionTo(player.Center).RotatedBy(-ai0 * 60);
                            Projectile.NewProjectile(npc.Center, vel, mod.ProjectileType("AbomSword"), npc.damage * 3 / 8, 0f, Main.myPlayer, ai0, npc.whoAmI);
                        }
                        else
                        {
                            npc.ai[0] += 3;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                        }
                    }
                    break;

                case 16: //while dashing
                    npc.direction = npc.spriteDirection = Math.Sign(npc.velocity.X);
                    if (++npc.ai[1] > 120)
                    {
                        npc.netUpdate = true;
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                    }
                    break;

                case 17: //wait for scythes to clear
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center + player.DirectionTo(npc.Center) * 500;
                    if (npc.Distance(targetPos) > 50)
                        Movement(targetPos, 0.7f);
                    if (++npc.ai[1] > 120 || (npc.dontTakeDamage && npc.ai[1] > 30))
                    {
                        npc.netUpdate = true;
                        npc.ai[0] -= 2;
                        npc.ai[1] = 0;
                    }
                    break;

                case 18: //beginning of vertical dive
                    if (!AliveCheck(player))
                        break;
                    for (int i = 0; i < 5; i++) //make warning dust
                    {
                        int d = Dust.NewDust(npc.position, npc.width, npc.height, 87, 0f, 0f, 0, default(Color), 1.5f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity *= 4f;
                    }
                    if (npc.ai[2] == 0 && npc.ai[3] == 0) //target one side of arena
                    {
                        npc.netUpdate = true;
                        npc.ai[2] = npc.Center.X + (player.Center.X < npc.Center.X ? -1200 : 1200);
                        npc.ai[3] = npc.Center.Y - 1100;
                        npc.localAI[2] = npc.ai[2] > npc.Center.X ? -1 : 1;
                        for (int i = 0; i < Main.maxProjectiles; i++)
                        {
                            if (Main.projectile[i].active && Main.projectile[i].type == mod.ProjectileType("AbomRitual") && Main.projectile[i].ai[1] == npc.whoAmI)
                            {
                                npc.ai[2] = Main.projectile[i].Center.X + (player.Center.X < Main.projectile[i].Center.X ? -1200 : 1200);
                                npc.ai[3] = Main.projectile[i].Center.Y - 1100;
                                npc.localAI[2] = npc.ai[2] > Main.projectile[i].Center.X ? -1 : 1;
                                break;
                            }
                        }
                    }
                    targetPos = new Vector2(npc.ai[2], npc.ai[3]);
                    Movement(targetPos, 0.7f);
                    if (++npc.ai[1] > 210 || (npc.dontTakeDamage && npc.ai[1] > 150))
                    {
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, Vector2.UnitX * npc.localAI[2], mod.ProjectileType("AbomSword"), npc.damage * 3 / 8, 0f, Main.myPlayer, npc.localAI[2] * 0.0001f, npc.whoAmI);

                        npc.netUpdate = true;
                        npc.velocity = Vector2.Zero;
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }
                    else if (npc.ai[1] == 180 || (npc.dontTakeDamage && npc.ai[1] == 120))
                    {
                        Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, Vector2.UnitX * npc.localAI[2], mod.ProjectileType("AbomDeathraySmall2"), 0, 0f, Main.myPlayer, 0f, npc.whoAmI);
                    }
                    break;

                case 19: //prepare to dash
                    npc.direction = npc.spriteDirection = Math.Sign(npc.localAI[2]);
                    if (++npc.ai[1] > 60)
                    {
                        npc.netUpdate = true;
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.velocity.X = (player.Center.X - npc.Center.X) / 90 / 4;
                        npc.velocity.Y = 30;
                    }
                    break;

                case 20: //while dashing down
                    npc.velocity.Y *= 0.97f;
                    npc.position += npc.velocity;
                    npc.direction = npc.spriteDirection = Math.Sign(npc.localAI[2]);
                    if (++npc.ai[1] > 90)
                    {
                        npc.netUpdate = true;
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                    }
                    break;

                case 21: //wait for scythes to clear
                    if (!AliveCheck(player))
                        break;
                    npc.localAI[2] = 0;
                    targetPos = player.Center;
                    targetPos.X += 500 * (npc.Center.X < targetPos.X ? -1 : 1);
                    if (npc.Distance(targetPos) > 50)
                        Movement(targetPos, 0.7f);
                    if (++npc.ai[1] > 120)
                    {
                        npc.netUpdate = true;
                        npc.ai[0] = npc.dontTakeDamage ? -3 : 0;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }
                    break;

                default:
                    Main.NewText("UH OH, STINKY");
                    npc.netUpdate = true;
                    npc.ai[0] = 0;
                    goto case 0;
            }

            if (npc.ai[0] >= 9 && npc.dontTakeDamage)
            {
                for (int i = 0; i < 5; i++)
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 87, 0f, 0f, 0, default(Color), 1.5f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 4f;
                }
            }
        }

        private void Aura(float distance, int buff, bool reverse = false, int dustid = DustID.GoldFlame, bool checkDuration = false, bool targetEveryone = true)
        {
            //works because buffs are client side anyway :ech:
            Player p = targetEveryone ? Main.player[Main.myPlayer] : Main.player[npc.target];
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
                        if (Main.netMode != 1 && !NPC.AnyNPCs(ModLoader.GetMod("Fargowiltas").NPCType("Abominationn")))
                        {
                            for (int i = 0; i < 1000; i++)
                                if (Main.projectile[i].active && Main.projectile[i].hostile)
                                    Main.projectile[i].Kill();
                            for (int i = 0; i < 1000; i++)
                                if (Main.projectile[i].active && Main.projectile[i].hostile)
                                    Main.projectile[i].Kill();
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModLoader.GetMod("Fargowiltas").NPCType("Abominationn"));
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

            if (npc.life < npc.lifeMax / 2)
            {
                if (Main.netMode != 1)
                {
                    npc.ai[0] = -1;
                    npc.ai[1] = 0;
                    npc.ai[2] = 0;
                    npc.ai[3] = 0;
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

        private void Movement(Vector2 targetPos, float speedModifier, bool fastX = true)
        {
            if (npc.Center.X < targetPos.X)
            {
                npc.velocity.X += speedModifier;
                if (npc.velocity.X < 0)
                    npc.velocity.X += speedModifier * (fastX ? 2 : 1);
            }
            else
            {
                npc.velocity.X -= speedModifier;
                if (npc.velocity.X > 0)
                    npc.velocity.X -= speedModifier * (fastX ? 2 : 1);
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
            if (Math.Abs(npc.velocity.X) > 24)
                npc.velocity.X = 24 * Math.Sign(npc.velocity.X);
            if (Math.Abs(npc.velocity.Y) > 24)
                npc.velocity.Y = 24 * Math.Sign(npc.velocity.Y);
        }

        private void MovementY(float targetY, float speedModifier)
        {
            if (npc.Center.Y < targetY)
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
            if (Math.Abs(npc.velocity.Y) > 24)
                npc.velocity.Y = 24 * Math.Sign(npc.velocity.Y);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("MutantNibble"), 300);
            target.AddBuff(mod.BuffType("AbomFang"), 300);
            target.AddBuff(mod.BuffType("Unstable"), 240);
            target.AddBuff(mod.BuffType("Berserked"), 120);
            target.AddBuff(BuffID.Bleeding, 600);
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

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            damage *= 0.8;
            return true;
        }

        public override bool CheckDead()
        {
            if (npc.ai[0] == -3 && npc.ai[1] >= 180)
                return true;

            npc.life = 1;
            npc.active = true;
            if (npc.localAI[3] < 2)
            {
                npc.localAI[3] = 2;
                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("AbomRitual"), npc.damage / 2, 0f, Main.myPlayer, 0f, npc.whoAmI);
                }
            }
            if (Main.netMode != 1 && npc.ai[0] > -2)
            {
                npc.ai[0] = -3;
                npc.ai[1] = 0;
                npc.ai[2] = 0;
                npc.ai[3] = 0;
                npc.localAI[2] = 0;
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
            FargoSoulsWorld.downedAbom = true;
            if (Main.netMode == 2)
                NetMessage.SendData(7); //sync world

            //npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("AbomBag"));

            npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("MutantScale"), Main.rand.Next(11) + 10);
            //npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("MutatingEnergy"), Main.rand.Next(11) + 10);
            npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("CyclonicFin"));

            if (Main.rand.Next(10) == 0)
                Item.NewItem(npc.Hitbox, mod.ItemType("AbomTrophy"));
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
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