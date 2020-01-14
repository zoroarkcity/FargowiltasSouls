using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.IO;

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
            npc.defense = 300;
            npc.lifeMax = 1000000;
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
            musicPriority = (MusicPriority)12;
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
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            npc.localAI[0] = reader.ReadSingle();
            npc.localAI[1] = reader.ReadSingle();
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
                Aura(2000f, mod.BuffType("GodEater"), true, 86);
            }
            else if (Main.player[Main.myPlayer].active && npc.Distance(Main.player[Main.myPlayer].Center) < 3000f)
            {
                Main.player[Main.myPlayer].AddBuff(mod.BuffType("AbomPresence"), 2);
            }

            Player player = Main.player[npc.target];
            npc.direction = npc.spriteDirection = npc.position.X < player.position.X ? 1 : -1;
            Vector2 targetPos;
            float speedModifier;
            switch ((int)npc.ai[0])
            {
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
                    if (!AliveCheck(player))
                        break;
                    if (Phase2Check())
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
                    if (npc.localAI[3] > 0)
                        npc.ai[1]++;
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
                            Main.NewText("throw splitter scythe");
                            //Projectile.NewProjectile(npc.Center, npc.DirectionTo(player.Center) * 25f, mod.ProjectileType("MutantSpearThrown"), npc.damage / 4, 0f, Main.myPlayer, npc.target);
                        }
                    }
                    break;

                case 1: //flaming scythe 8way (becomes homing in p2)
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center + player.DirectionTo(npc.Center) * 600;
                    if (npc.Distance(targetPos) > 50)
                    {
                        Movement(targetPos, 0.5f);
                    }
                    if (++npc.ai[1] > 60)
                    {
                        npc.ai[1] = 0;
                        if (++npc.ai[2] > 3)
                        {
                            npc.ai[0]++;
                            npc.ai[2] = 0;
                            npc.TargetClosest();
                        }
                        else
                        {
                            Main.NewText("flaming scythe 8way");
                            /*if (Main.netMode != 1)
                                for (int i = 0; i < 8; i++)
                                    Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(Math.PI / 4 * i) * 10f, mod.ProjectileType("MutantScythe1"), npc.damage / 5, 0f, Main.myPlayer, npc.whoAmI);
                            Main.PlaySound(36, (int)npc.Center.X, (int)npc.Center.Y, -1, 1f, 0f);*/
                        }
                        npc.netUpdate = true;
                        break;
                    }
                    break;

                case 2: //pause and then initiate dash
                    if (Phase2Check())
                        break;
                    npc.velocity *= 0.9f;
                    if (++npc.ai[1] > 15)
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
                    if (++npc.ai[3] > 4)
                    {
                        npc.ai[3] = 0;
                        if (npc.localAI[3] > 1 && Main.netMode != 1)
                            Main.NewText("fishron side scythes");
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
                    npc.netUpdate = true;
                    npc.ai[0] += ++npc.localAI[0];
                    if (npc.localAI[0] >= 3) //reset p1 hard option counter
                        npc.localAI[0] = 0;
                    break;

                case 5: //mutant scythe 8way (p2 also shoots flaming scythes)
                    if (++npc.ai[1] > 120)
                    {
                        Main.NewText("did scythe");
                        npc.netUpdate = true;
                        npc.ai[0] = 8;
                        npc.ai[1] = 0;
                    }
                    break;

                case 6: //flocko swarm (p2 shoots ice waves up/down after)
                    if (++npc.ai[1] > 120)
                    {
                        Main.NewText("did flocko");
                        npc.netUpdate = true;
                        npc.ai[0] = 8;
                        npc.ai[1] = 0;
                    }
                    break;

                case 7: //saucer laser spam  (p2 shoots rockets)
                    if (++npc.ai[1] > 120)
                    {
                        Main.NewText("did laser spam");
                        npc.netUpdate = true;
                        npc.ai[0] = 8;
                        npc.ai[1] = 0;
                    }
                    break;

                case 8: //return to beginning in p1, proceed in p2
                    npc.netUpdate = true;
                    if (npc.localAI[3] > 1)
                    {
                        if (npc.localAI[1] == 0)
                        {
                            npc.localAI[1] = 1;
                            npc.ai[0]++;
                        }
                        else
                        {
                            npc.localAI[1] = 0;
                            npc.ai[0] = 12;
                        }
                    }
                    else
                    {
                        npc.ai[0] = 0;
                    }
                    break;

                case 9: //beginning of scythe rows and deathray rain
                    if (++npc.ai[1] > 120)
                    {
                        Main.NewText("did scythe rows");
                        npc.netUpdate = true;
                        npc.ai[0] = 0;
                        npc.ai[1] = 0;
                    }
                    break;

                case 12: //beginning of laevateinn
                    if (++npc.ai[1] > 120)
                    {
                        Main.NewText("did laevateinns");
                        npc.netUpdate = true;
                        npc.ai[0] = 0;
                        npc.ai[1] = 0;
                    }
                    break;

                default:
                    Main.NewText("UH OH, STINKY");
                    npc.netUpdate = true;
                    npc.ai[0] = 0;
                    goto case 0;
            }
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
                if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 5000f)
                {
                    if (npc.timeLeft > 30)
                        npc.timeLeft = 30;
                    npc.velocity.Y -= 1f;
                    if (npc.timeLeft == 1 && Fargowiltas.Instance.FargowiltasLoaded)
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

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("CurseoftheMoon"), 600);
            target.AddBuff(mod.BuffType("MutantFang"), 180);
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
            damage *= 0.9;
            return true;
        }

        public override void NPCLoot()
        {
            FargoSoulsWorld.downedAbom = true;
            if (Main.netMode == 2)
                NetMessage.SendData(7); //sync world
            npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("AbomBag"));
            if (Main.rand.Next(10) == 0)
                Item.NewItem(npc.Hitbox, mod.ItemType("AbomTrophy"));

            for (int i = 0; i < 1000; i++)
                if (Main.projectile[i].active && Main.projectile[i].hostile)
                    Main.projectile[i].Kill();

            if (Main.netMode != 1 && Fargowiltas.Instance.FargowiltasLoaded && !NPC.AnyNPCs(ModLoader.GetMod("Fargowiltas").NPCType("Abominationn")))
            {
                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModLoader.GetMod("Fargowiltas").NPCType("Abominationn"));
                if (n != 200 && Main.netMode == 2)
                    NetMessage.SendData(23, -1, -1, null, n);
            }
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
    }
}