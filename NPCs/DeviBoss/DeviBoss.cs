using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.IO;

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

                RefreshAttackQueue();

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
            }
            else if (Main.player[Main.myPlayer].active && npc.Distance(Main.player[Main.myPlayer].Center) < 3000f)
            {
                Main.player[Main.myPlayer].AddBuff(mod.BuffType("AbomPresence"), 2);
            }*/

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
                            for (int i = 0; i < 30; i++)
                                Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(Main.rand.NextDouble() * Math.PI) * Main.rand.NextFloat(30f), mod.ProjectileType("DeviDeathHeart"), 0, 0f, Main.myPlayer);

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
                        /*if (++npc.ai[2] > 15)
                        {
                            int heal = (int)(npc.lifeMax / 2 / 60 * Main.rand.NextFloat(1.5f, 2f));
                            npc.life += heal;
                            if (npc.life > npc.lifeMax)
                                npc.life = npc.lifeMax;
                            CombatText.NewText(npc.Hitbox, CombatText.HealLife, heal);
                        }*/
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
                    if (npc.Distance(targetPos) > 25)
                        Movement(targetPos, npc.localAI[3] > 0 ? 0.15f : 2f);

                    if (npc.localAI[3] > 0) //in range, fight has begun, choose attacks
                    {
                        npc.netUpdate = true;
                        npc.ai[0]++;
                    }
                    break;

                case 1: //teleport marx hammers
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    npc.velocity = Vector2.Zero;
                    if (++npc.ai[1] > (npc.localAI[3] > 1 ? 15 : 30) && npc.ai[2] < (npc.localAI[3] > 1 ? 5 : 3))
                    {
                        npc.ai[1] = 0;
                        npc.ai[2]++;

                        TeleportDust();
                        if (Main.netMode != 1)
                        {
                            bool wasOnLeft = npc.Center.X < player.Center.X;
                            npc.Center = player.Center + 300 * Vector2.UnitX.RotatedBy(Main.rand.NextFloat(0, 2 * (float)Math.PI));
                            if (wasOnLeft && npc.Center.X < player.Center.X)
                            {
                                float x = player.Center.X - npc.Center.X;
                                npc.position.X += x * 2;
                            }
                            npc.netUpdate = true;
                        }
                        TeleportDust();
                        Main.PlaySound(SoundID.Item84, npc.Center);
                    }

                    if (npc.ai[1] == 45) //finished all the prior teleports, now attack
                    {
                        if (Main.netMode != 1)
                        {
                            Main.NewText("marx hammer attack");
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

                case 2: //circling with heart barrages
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    targetPos = player.Center + player.DirectionTo(npc.Center) * 400;
                    if (npc.Distance(targetPos) > 25)
                        Movement(targetPos, 0.15f);

                    if (++npc.ai[1] > 60)
                    {
                        npc.netUpdate = true;
                        npc.ai[1] = 0;
                        if (++npc.ai[2] > 3)
                        {
                            GetNextAttack();
                        }
                        else
                        {
                            if (Main.netMode != 1)
                            {
                                Main.NewText("heart spread");
                            }
                        }
                        break;
                    }
                    break;

                case 3: //slow while shooting wyvern orb spirals
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    npc.velocity = npc.DirectionTo(player.Center) * 2f;
                    if (++npc.ai[1] > 60)
                    {
                        npc.netUpdate = true;
                        npc.ai[1] = 0;

                        if (++npc.ai[2] > 3)
                        {
                            GetNextAttack();
                        }
                        else
                        {
                            if (Main.netMode != 1)
                            {
                                Main.NewText("wyvern orb spirals");
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
                    if (npc.Distance(targetPos) > 25)
                        Movement(targetPos, 0.15f);

                    if (++npc.ai[1] > 30 && npc.ai[1] < 120) //rapid mimic spam
                    {
                        if (++npc.ai[2] > 20)
                        {
                            npc.netUpdate = true;
                            npc.ai[2] = 0;
                            if (Main.netMode != 1)
                            {
                                Main.NewText("throw a mimic");
                            }
                        }
                    }
                    else if (npc.ai[1] == 240) //big wave of mimics, aimed ahead of you
                    {
                        if (Main.netMode != 1)
                            Main.NewText("wall of mimics");
                    }
                    else if (npc.ai[1] > 360)
                    {
                        GetNextAttack();
                    }
                    break;

                case 5: //frostballs and nados
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    targetPos = player.Center + player.DirectionTo(npc.Center) * 500;
                    if (npc.Distance(targetPos) > 25)
                        Movement(targetPos, 0.15f);

                    if (++npc.ai[1] > 360)
                    {
                        GetNextAttack();
                    }
                    if (++npc.ai[2] > (npc.localAI[3] > 1 ? 60 : 40))
                    {
                        npc.netUpdate = true;
                        npc.ai[2] = 0;
                        if (Main.netMode != 1)
                        {
                            Main.NewText("frostfireball behind self");
                        }
                    }
                    if (npc.localAI[3] > 1 && --npc.ai[3] < 0)
                    {
                        npc.netUpdate = true;
                        npc.ai[3] = 90;
                        if (Main.netMode != 1)
                        {
                            Main.NewText("make sandstorm tornado");
                        }
                    }
                    break;

                case 6: //rune wizard
                    if (!AliveCheck(player) || Phase2Check())
                        break;

                    npc.velocity = Vector2.Zero;
                    if (++npc.ai[1] == 1)
                    {
                        TeleportDust();
                        if (Main.netMode != 1)
                        {
                            bool wasOnLeft = npc.Center.X < player.Center.X;
                            npc.Center = player.Center + 300 * Vector2.UnitX.RotatedBy(Main.rand.NextFloat(0, 2 * (float)Math.PI));
                            if (wasOnLeft && npc.Center.X < player.Center.X)
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
                            Main.NewText("rune wizard bolts");
                            if (npc.localAI[3] > 1)
                            {
                                Main.NewText("rune wizard ring");
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
                        npc.ai[1] = -30;

                        TeleportDust();
                        if (Main.netMode != 1)
                        {
                            bool wasOnLeft = npc.Center.X < player.Center.X;
                            npc.Center = player.Center;
                            npc.position.X += wasOnLeft ? 300 : -300;
                            npc.netUpdate = true;
                        }
                        TeleportDust();

                        Main.PlaySound(SoundID.Item84, npc.Center);
                        Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                    }

                    if (++npc.ai[3] > 5)
                    {
                        npc.ai[3] = 0;
                        if (Main.netMode != 1)
                        {
                            Main.NewText("make moth dust");
                        }
                    }

                    npc.velocity *= 0.9f;
                    if (++npc.ai[1] > (npc.localAI[3] > 1 ? 30 : 60))
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
                    if (++npc.ai[3] > 5)
                    {
                        npc.ai[3] = 0;
                        if (Main.netMode != 1)
                        {
                            Main.NewText("make moth dust");
                        }
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

                    npc.velocity = npc.DirectionTo(player.Center) * 6f;

                    if (++npc.ai[1] == 1)
                    {
                        for (int i = 0; i < 40; i++) //warning dust ring
                        {
                            Vector2 vector6 = Vector2.UnitY * 40f;
                            vector6 = vector6.RotatedBy((i - (40 / 2 - 1)) * 6.28318548f / 40) + npc.Center;
                            Vector2 vector7 = vector6 - npc.Center;
                            int d = Dust.NewDust(vector6 + vector7, 0, 0, DustID.Shadowflame, 0f, 0f, 0, default(Color), 2f);
                            Main.dust[d].noGravity = true;
                            Main.dust[d].velocity = vector7;
                        }
                        Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                    }
                    else if (npc.ai[1] < 120) //spam shadowbeams after delay
                    {
                        if (++npc.ai[2] > 60)
                        {
                            if (++npc.ai[3] == 1) //store rotation briefly before shooting
                            {
                                npc.localAI[0] = npc.DirectionTo(player.Center).ToRotation();
                            }
                            else if (npc.ai[3] > 15)
                            {
                                npc.ai[3] = 0;
                                if (Main.netMode != 1)
                                {
                                    Main.NewText("shadowbeam using ai0");
                                }
                            }
                        }
                    }
                    else if (npc.ai[1] < 240)
                    {
                        npc.ai[3] = 0;
                        npc.localAI[0] = 0;

                        if (++npc.ai[2] > 40)
                        {
                            npc.ai[2] = 0;
                            if (Main.netMode != 1)
                            {
                                Main.NewText("diabolist blast");
                            }
                        }
                    }
                    else
                    {
                        npc.velocity /= 2;

                        if (npc.ai[1] == 360 && Main.netMode != 1)
                        {
                            Main.NewText("explode into ragged caster balls");
                        }

                        if (npc.ai[1] > 480)
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
                        if (npc.Distance(targetPos) > 25)
                            Movement(targetPos, 0.15f);

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
                        npc.velocity = Vector2.Normalize(npc.velocity) * 3f;

                        Main.NewText("baby guardians");

                        Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 0);
                    }
                    else
                    {
                        if (npc.ai[1] > (npc.localAI[3] > 1 ? 420 : 300)) //delay if in p2
                        {
                            GetNextAttack();
                        }

                        if (npc.localAI[3] > 1 && npc.ai[1] == 300) //surprise!
                        {
                            Main.PlaySound(36, (int)npc.Center.X, (int)npc.Center.Y, -1, 1f, 0f); //eoc roar

                            if (Main.netMode != 1)
                            {
                                Main.NewText("surprise bonus skeletons");
                            }
                        }
                    }
                    break;

                case 11: //noah/irisu geyser rain
                    Main.NewText("reached end of ai for now");
                    npc.netUpdate = true;
                    npc.ai[0] = 0;
                    goto case 0;
                    //break;

                case 12: //lilith cross ray hearts
                    break;

                case 13: //that one boss that was a bunch of gems burst rain but with butterflies
                    break;

                case 14: //medusa ray
                    break;

                case 15: //sparkling love
                    break;

                case 16: //pause between attacks
                    if (++npc.ai[1] > 60)
                    {
                        npc.ai[0] = attackQueue[(int)npc.localAI[2]];
                        if (++npc.localAI[2] >= attackQueue.Length)
                        {
                            npc.localAI[2] = 0;
                            RefreshAttackQueue();
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
                for (int j = 0; j < 3; j++) //cant pick attack that's already queued
                    if (newQueue[i] == attackQueue[j])
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
        }

        /*private void Aura(float distance, int buff, bool reverse = false, int dustid = DustID.GoldFlame, bool checkDuration = false)
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
        }*/

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

            if (npc.life < npc.lifeMax * 0.66)
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
            if (Math.Abs(npc.velocity.X) > 12)
                npc.velocity.X = 12 * Math.Sign(npc.velocity.X);
            if (Math.Abs(npc.velocity.Y) > 12)
                npc.velocity.Y = 12 * Math.Sign(npc.velocity.Y);
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
            target.AddBuff(mod.BuffType("Lovestruck"), 300);
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
            
            int maxEnergy = Main.rand.Next(10) + 10;
            for (int i = 0; i < maxEnergy; i++)
                npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("DeviatingEnergy"));
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
    }
}