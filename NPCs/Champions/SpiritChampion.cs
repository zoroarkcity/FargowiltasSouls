using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using FargowiltasSouls.Buffs.Masomode;
using FargowiltasSouls.Items.Accessories.Enchantments;
using FargowiltasSouls.Projectiles.Masomode;
using FargowiltasSouls.Projectiles.Champions;
using System.IO;

namespace FargowiltasSouls.NPCs.Champions
{
    [AutoloadBossHead]
    public class SpiritChampion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Champion of Spirit");
            NPCID.Sets.TrailCacheLength[npc.type] = 6;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.width = 150;
            npc.height = 150;
            npc.damage = 140;
            npc.defense = 40;
            npc.lifeMax = 700000;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.aiStyle = -1;
            npc.value = Item.buyPrice(0, 10);

            npc.boss = true;
            music = MusicID.Boss4;
            musicPriority = MusicPriority.BossMedium;

            npc.buffImmune[BuffID.Chilled] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Suffocation] = true;
            npc.buffImmune[mod.BuffType("Lethargic")] = true;
            npc.buffImmune[mod.BuffType("ClippedWings")] = true;
            npc.GetGlobalNPC<FargoSoulsGlobalNPC>().SpecialEnchantImmune = true;
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
            if (npc.localAI[3] == 0) //spawn friends
            {
                npc.localAI[3] = 1;
                npc.TargetClosest(false);

                if (Main.netMode != 1)
                {
                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<SpiritChampionHand>(), npc.whoAmI, 0f, npc.whoAmI, -1f, -1f, npc.target);
                    if (n != Main.maxNPCs)
                    {
                        Main.npc[n].velocity.X = Main.rand.NextFloat(-24f, 24f);
                        Main.npc[n].velocity.Y = Main.rand.NextFloat(-24f, 24f);
                        if (Main.netMode != 2)
                            NetMessage.SendData(23, -1, -1, null, n);
                    }
                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<SpiritChampionHand>(), npc.whoAmI, 0f, npc.whoAmI, -1f, 1f, npc.target);
                    if (n != Main.maxNPCs)
                    {
                        Main.npc[n].velocity.X = Main.rand.NextFloat(-24f, 24f);
                        Main.npc[n].velocity.Y = Main.rand.NextFloat(-24f, 24f);
                        if (Main.netMode != 2)
                            NetMessage.SendData(23, -1, -1, null, n);
                    }
                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<SpiritChampionHand>(), npc.whoAmI, 0f, npc.whoAmI, 1f, -1f, npc.target);
                    if (n != Main.maxNPCs)
                    {
                        Main.npc[n].velocity.X = Main.rand.NextFloat(-24f, 24f);
                        Main.npc[n].velocity.Y = Main.rand.NextFloat(-24f, 24f);
                        if (Main.netMode != 2)
                            NetMessage.SendData(23, -1, -1, null, n);
                    }
                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<SpiritChampionHand>(), npc.whoAmI, 0f, npc.whoAmI, 1f, 1f, npc.target);
                    if (n != Main.maxNPCs)
                    {
                        Main.npc[n].velocity.X = Main.rand.NextFloat(-24f, 24f);
                        Main.npc[n].velocity.Y = Main.rand.NextFloat(-24f, 24f);
                        if (Main.netMode != 2)
                            NetMessage.SendData(23, -1, -1, null, n);
                    }
                }
            }

            EModeGlobalNPC.championBoss = npc.whoAmI;

            Player player = Main.player[npc.target];
            Vector2 targetPos;
            
            if (npc.HasValidTarget && npc.Distance(player.Center) < 2500 && Framing.GetTileSafely(player.Center).wall != WallID.None)
                npc.timeLeft = 600;
            
            switch ((int)npc.ai[0])
            {
                case -1:
                    targetPos = new Vector2(npc.localAI[0], npc.localAI[1]);
                    if (npc.Distance(targetPos) > 25)
                        Movement(targetPos, 0.8f, 24f);

                    if (++npc.ai[1] > 200)
                    {
                        npc.TargetClosest();
                        npc.ai[0] = 4;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        npc.netUpdate = true;

                        if (npc.Distance(player.Center) < 50)
                        {
                            player.velocity.X = player.Center.X < npc.Center.X ? -15f : 15f;
                            player.velocity.Y = -10f;
                            Main.PlaySound(15, npc.Center, 0);
                        }
                    }
                    break;

                case 0: //float to player
                    if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 2500f
                        || Framing.GetTileSafely(player.Center).wall == WallID.None) //despawn code
                    {
                        npc.TargetClosest(false);
                        if (npc.timeLeft > 30)
                            npc.timeLeft = 30;

                        npc.noTileCollide = true;
                        npc.noGravity = true;
                        npc.velocity.Y += 1f;

                        return;
                    }

                    if (npc.ai[1] == 0)
                    {
                        targetPos = player.Center;
                        npc.velocity = (targetPos - npc.Center) / 75;

                        npc.localAI[0] = targetPos.X;
                        npc.localAI[1] = targetPos.Y;
                    }

                    if (++npc.ai[1] > 75)
                    {
                        npc.TargetClosest();
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 1: //cross bone/sandnado
                    targetPos = new Vector2(npc.localAI[0], npc.localAI[1]);
                    if (npc.Distance(targetPos) > 25)
                        Movement(targetPos, 0.8f, 24f);

                    if (++npc.ai[2] > 60)
                    {
                        npc.ai[2] = 0;
                        
                        if (Main.netMode != 1)
                        {
                            if (npc.ai[1] < 180) //cross bones
                            {
                                Main.PlaySound(SoundID.Item2, npc.Center);

                                for (int i = 0; i < 12; i++)
                                {
                                    Projectile.NewProjectile(npc.position.X + Main.rand.Next(npc.width), npc.position.Y + Main.rand.Next(npc.height),
                                        Main.rand.NextFloat(-8f, 8f), Main.rand.NextFloat(-8f, 8f), ModContent.ProjectileType<SpiritCrossBone>(), npc.damage / 4, 0f, Main.myPlayer);
                                }
                            }
                            else //sandnado
                            {
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
                        }
                    }
                    
                    if (++npc.ai[1] > 400)
                    {
                        npc.TargetClosest();
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 2:
                    goto case 0;

                case 3: //grab
                    targetPos = new Vector2(npc.localAI[0], npc.localAI[1]);
                    if (npc.Distance(targetPos) > 25)
                        Movement(targetPos, 0.8f, 24f);

                    if (npc.ai[2] == 0)
                    {
                        npc.ai[2] = 1;

                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<SpiritChampionHand>() && Main.npc[i].ai[1] == npc.whoAmI)
                            {
                                Main.npc[i].ai[0] = 1f;
                                Main.npc[i].netUpdate = true;
                            }
                        }
                    }

                    if (++npc.ai[1] > 360)
                    {
                        npc.TargetClosest();
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 4:
                    goto case 0;

                case 5: //swords
                    targetPos = new Vector2(npc.localAI[0], npc.localAI[1]);
                    if (npc.Distance(targetPos) > 25)
                        Movement(targetPos, 0.8f, 24f);

                    if (++npc.ai[2] > 80)
                    {
                        npc.ai[2] = 0;

                        Main.PlaySound(SoundID.Item92, npc.Center);

                        if (Main.netMode != 1)
                        {
                            for (int i = 0; i < 15; i++)
                            {
                                float speed = Main.rand.NextFloat(4f, 8f);
                                Vector2 velocity = speed * Vector2.UnitX.RotatedBy(Main.rand.NextDouble() * 2 * Math.PI);
                                float ai1 = speed / Main.rand.NextFloat(30f, 120f);
                                Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<SpiritSword>(), npc.damage / 4, 0f, Main.myPlayer, 0f, ai1);
                            }
                        }
                    }

                    if (++npc.ai[1] > 300)
                    {
                        npc.TargetClosest();
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }
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

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Infested>(), 360);
            target.AddBuff(ModContent.BuffType<ClippedWings>(), 180);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void NPCLoot()
        {
            //Item.NewItem(npc.position, npc.Size, ModContent.ItemType<SpiritForce>());
            int[] drops = {
                ModContent.ItemType<FossilEnchant>(),
                ModContent.ItemType<ForbiddenEnchant>(),
                ModContent.ItemType<HallowEnchant>(),
                ModContent.ItemType<TikiEnchant>(),
                ModContent.ItemType<SpectreEnchant>(),
            };
            Item.NewItem(npc.position, npc.Size, drops[Main.rand.Next(drops.Length)]);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.npcTexture[npc.type];
            //int num156 = Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type]; //ypos of lower right corner of sprite to draw
            //int y3 = num156 * npc.frame.Y; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = npc.frame;//new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = lightColor;
            color26 = npc.GetAlpha(color26);

            SpriteEffects effects = npc.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int i = 0; i < NPCID.Sets.TrailCacheLength[npc.type]; i++)
            {
                Color color27 = color26 * 0.5f;
                color27 *= (float)(NPCID.Sets.TrailCacheLength[npc.type] - i) / NPCID.Sets.TrailCacheLength[npc.type];
                Vector2 value4 = npc.oldPos[i];
                float num165 = npc.rotation; //npc.oldRot[i];
                Main.spriteBatch.Draw(texture2D13, value4 + npc.Size / 2f - Main.screenPosition + new Vector2(0, npc.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, npc.scale, effects, 0f);
            }

            Main.spriteBatch.Draw(texture2D13, npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), npc.GetAlpha(lightColor), npc.rotation, origin2, npc.scale, effects, 0f);
            return false;
        }
    }
}