using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Buffs.Masomode;
using FargowiltasSouls.Items.Accessories.Enchantments;
using FargowiltasSouls.Projectiles.Masomode;
using FargowiltasSouls.Projectiles.Champions;

namespace FargowiltasSouls.NPCs.Champions
{
    [AutoloadBossHead]
    public class NatureChampion : ModNPC
    {
        /* order of heads:
         * 0 = crimson
         * 1 = molten
         * 2 = rain
         * 3 = frost
         * 4 = chloro
         * 5 = shroomite
         */
        public int[] heads = { -1, -1, -1, -1, -1, -1 };
        public int lastSet = 0;
        public static readonly KeyValuePair<int, int>[] configurations = {
            new KeyValuePair<int, int>(0, 1),
            new KeyValuePair<int, int>(1, 3),
            new KeyValuePair<int, int>(3, 5),
            new KeyValuePair<int, int>(3, 4),
            new KeyValuePair<int, int>(2, 4),
            new KeyValuePair<int, int>(0, 5),
            new KeyValuePair<int, int>(1, 2),
            new KeyValuePair<int, int>(2, 5),
            new KeyValuePair<int, int>(0, 4)
        };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Champion of Nature");
            NPCID.Sets.TrailCacheLength[npc.type] = 6;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.width = 224;
            npc.height = 112;
            npc.damage = 130;
            npc.defense = 150;
            npc.lifeMax = 700000;
            npc.HitSound = SoundID.NPCHit6;
            npc.DeathSound = SoundID.NPCDeath1;
            //npc.noGravity = true;
            //npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.aiStyle = -1;
            npc.value = Item.buyPrice(0, 5);

            npc.boss = true;
            music = MusicID.Boss5;
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
            for (int i = 0; i < heads.Length; i++)
                writer.Write(heads[i]);
            writer.Write(lastSet);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            for (int i = 0; i < heads.Length; i++)
                heads[i] = reader.ReadInt32();
            lastSet = reader.ReadInt32();
        }

        public override void AI()
        {
            if (npc.localAI[3] == 0) //spawn friends
            {
                npc.TargetClosest(false);
                Movement(Main.player[npc.target].Center, 0.8f, 32f);
                if (npc.Distance(Main.player[npc.target].Center) < 2500)
                    npc.localAI[3] = 1;
                else
                    return;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<NatureChampionHead>(), npc.whoAmI, 0f, npc.whoAmI, 0f, -3f, npc.target);
                    if (n != Main.maxNPCs)
                    {
                        heads[0] = n;
                        Main.npc[n].velocity.X = Main.rand.NextFloat(-24f, 24f);
                        Main.npc[n].velocity.Y = Main.rand.NextFloat(-24f, 24f);
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
                    }
                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<NatureChampionHead>(), npc.whoAmI, 0f, npc.whoAmI, 0f, -2f, npc.target);
                    if (n != Main.maxNPCs)
                    {
                        heads[1] = n;
                        Main.npc[n].velocity.X = Main.rand.NextFloat(-24f, 24f);
                        Main.npc[n].velocity.Y = Main.rand.NextFloat(-24f, 24f);
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
                    }
                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<NatureChampionHead>(), npc.whoAmI, 0f, npc.whoAmI, 0f, -1f, npc.target);
                    if (n != Main.maxNPCs)
                    {
                        heads[2] = n;
                        Main.npc[n].velocity.X = Main.rand.NextFloat(-24f, 24f);
                        Main.npc[n].velocity.Y = Main.rand.NextFloat(-24f, 24f);
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
                    }
                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<NatureChampionHead>(), npc.whoAmI, 0f, npc.whoAmI, 0f, 1f, npc.target);
                    if (n != Main.maxNPCs)
                    {
                        heads[3] = n;
                        Main.npc[n].velocity.X = Main.rand.NextFloat(-24f, 24f);
                        Main.npc[n].velocity.Y = Main.rand.NextFloat(-24f, 24f);
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
                    }
                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<NatureChampionHead>(), npc.whoAmI, 0f, npc.whoAmI, 0f, 2f, npc.target);
                    if (n != Main.maxNPCs)
                    {
                        heads[4] = n;
                        Main.npc[n].velocity.X = Main.rand.NextFloat(-24f, 24f);
                        Main.npc[n].velocity.Y = Main.rand.NextFloat(-24f, 24f);
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
                    }
                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<NatureChampionHead>(), npc.whoAmI, 0f, npc.whoAmI, 0f, 3f, npc.target);
                    if (n != Main.maxNPCs)
                    {
                        heads[5] = n;
                        Main.npc[n].velocity.X = Main.rand.NextFloat(-24f, 24f);
                        Main.npc[n].velocity.Y = Main.rand.NextFloat(-24f, 24f);
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);
                    }

                    for (int i = 0; i < heads.Length; i++) //failsafe, die if couldnt spawn heads
                    {
                        if (heads[i] == -1 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            npc.active = false;
                            return;
                        }
                    }
                }
            }

            EModeGlobalNPC.championBoss = npc.whoAmI;

            Player player = Main.player[npc.target];
            Vector2 targetPos;
            
            if (npc.HasValidTarget && npc.Distance(player.Center) < 3000 && player.Center.Y >= Main.worldSurface * 16 && !player.ZoneUnderworldHeight)
                npc.timeLeft = 600;

            npc.direction = npc.spriteDirection = player.Center.X < npc.Center.X ? -1 : 1;
            
            switch ((int)npc.ai[0])
            {
                case -1: //mourning wood movement
                    {
                        npc.noTileCollide = true;
                        npc.noGravity = true;

                        if (Math.Abs(player.Center.X - npc.Center.X) < npc.width / 2)
                        {
                            npc.velocity.X *= 0.9f;
                            if (Math.Abs(npc.velocity.X) < 0.1f)
                                npc.velocity.X = 0f;
                        }
                        else
                        {
                            float accel = 2f;
                            if (npc.direction > 0)
                                npc.velocity.X = (npc.velocity.X * 20 + accel) / 21;
                            else
                                npc.velocity.X = (npc.velocity.X * 20 - accel) / 21;
                        }

                        bool onPlatforms = false;
                        for (int i = (int)npc.position.X; i <= npc.position.X + npc.width; i += 16)
                        {
                            if (Framing.GetTileSafely(new Vector2(i, npc.position.Y + npc.height + npc.velocity.Y + 1)).type == TileID.Platforms)
                            {
                                onPlatforms = true;
                                break;
                            }
                        }

                        bool onCollision = Collision.SolidCollision(npc.position, npc.width, npc.height);

                        if (npc.position.X < player.position.X && npc.position.X + npc.width > player.position.X + player.width
                            && npc.position.Y + npc.height < player.position.Y + player.height - 16)
                        {
                            npc.velocity.Y += 0.5f;
                        }
                        else if (onCollision || (onPlatforms && player.position.Y + player.height <= npc.position.Y + npc.height))
                        {
                            if (npc.velocity.Y > 0f)
                                npc.velocity.Y = 0f;

                            if (onCollision)
                            {
                                if (npc.velocity.Y > -0.2f)
                                    npc.velocity.Y -= 0.025f;
                                else
                                    npc.velocity.Y -= 0.2f;

                                if (npc.velocity.Y < -4f)
                                    npc.velocity.Y = -4f;
                            }
                        }
                        else
                        {
                            if (npc.velocity.Y < 0f)
                                npc.velocity.Y = 0f;

                            if (npc.velocity.Y < 0.1f)
                                npc.velocity.Y += 0.025f;
                            else
                                npc.velocity.Y += 0.5f;
                        }

                        if (npc.velocity.Y > 10f)
                            npc.velocity.Y = 10f;
                    }
                    break;

                case 0: //think
                    if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 3000f
                        || player.Center.Y < Main.worldSurface * 16 || player.ZoneUnderworldHeight) //despawn code
                    {
                        npc.TargetClosest(false);
                        if (npc.timeLeft > 30)
                            npc.timeLeft = 30;

                        npc.noTileCollide = true;
                        npc.noGravity = true;
                        npc.velocity.Y += 1f;

                        break;
                    }

                    npc.noTileCollide = false;
                    npc.noGravity = false;

                    if (++npc.ai[1] > 45)
                    {
                        npc.TargetClosest();
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        npc.netUpdate = true;
                    }
                    goto case -1;

                case 1: //stomp
                    {
                        void StompDust()
                        {
                            Main.PlaySound(SoundID.Item, npc.Center, 14);

                            for (int k = -2; k <= 2; k++) //explosions
                            {
                                Vector2 dustPos = npc.Center;
                                int width = npc.width / 5;
                                dustPos.X += width * k + Main.rand.NextFloat(-width, width);
                                dustPos.Y += Main.rand.NextFloat(npc.height / 2);

                                for (int i = 0; i < 30; i++)
                                {
                                    int dust = Dust.NewDust(dustPos, 32, 32, 31, 0f, 0f, 100, default(Color), 3f);
                                    Main.dust[dust].velocity *= 1.4f;
                                }

                                for (int i = 0; i < 20; i++)
                                {
                                    int dust = Dust.NewDust(dustPos, 32, 32, 6, 0f, 0f, 100, default(Color), 3.5f);
                                    Main.dust[dust].noGravity = true;
                                    Main.dust[dust].velocity *= 7f;
                                    dust = Dust.NewDust(dustPos, 32, 32, 6, 0f, 0f, 100, default(Color), 1.5f);
                                    Main.dust[dust].velocity *= 3f;
                                }

                                float scaleFactor9 = 0.5f;
                                for (int j = 0; j < 4; j++)
                                {
                                    int gore = Gore.NewGore(dustPos, default(Vector2), Main.rand.Next(61, 64));
                                    Main.gore[gore].velocity *= scaleFactor9;
                                    Main.gore[gore].velocity.X += 1f;
                                    Main.gore[gore].velocity.Y += 1f;
                                }
                            }
                        }

                        const int jumpTime = 60;

                        npc.noGravity = true;
                        npc.noTileCollide = true;

                        if (npc.ai[2] == 0) //move over player
                        {
                            StompDust();

                            npc.ai[2] = 1;
                            npc.netUpdate = true;

                            targetPos = player.Center;
                            targetPos.Y -= 600;

                            npc.velocity = (targetPos - npc.Center) / jumpTime;
                        }

                        if (++npc.ai[1] > jumpTime + 18) //do the stomp
                        {
                            npc.noGravity = false;
                            npc.noTileCollide = false;

                            if (npc.velocity.Y == 0) //landed, now stomp
                            {
                                StompDust();

                                npc.TargetClosest();
                                npc.ai[0]++;
                                npc.ai[1] = 0;
                                npc.ai[2] = 0;
                                npc.ai[3] = 0;
                                npc.netUpdate = true;
                            }
                        }
                        else if (npc.ai[1] > jumpTime) //falling
                        {
                            npc.velocity.X = 0;
                            npc.velocity.Y = 30f;
                        }
                    }
                    break;

                case 2:
                    goto case 0;

                case 3: //decide an attack
                    if (npc.ai[2] == 0)
                    {
                        void ActivateHead(int targetHead)
                        {
                            Main.npc[targetHead].ai[0] += Main.npc[targetHead].ai[3];
                            Main.npc[targetHead].netUpdate = true;

                            Main.PlaySound(SoundID.ForceRoar, Main.npc[targetHead].Center, -1);

                            int dustType;
                            switch ((int)Main.npc[targetHead].ai[3])
                            {
                                case -3: dustType = 183; break;
                                case -2: dustType = 6; break;
                                case -1: dustType = 87; break;
                                case 1: dustType = 111; break;
                                case 2: dustType = 89; break;
                                case 3: dustType = 113; break;
                                default: dustType = 1; break;
                            }

                            const int num226 = 70;
                            for (int num227 = 0; num227 < num226; num227++)
                            {
                                Vector2 vector6 = Vector2.UnitX * 30f;
                                vector6 = vector6.RotatedBy(((num227 - (num226 / 2 - 1)) * 6.28318548f / num226), default(Vector2)) + Main.npc[targetHead].Center;
                                Vector2 vector7 = vector6 - Main.npc[targetHead].Center;
                                int num228 = Dust.NewDust(vector6 + vector7, 0, 0, dustType, 0f, 0f, 0, default(Color), 3f);
                                Main.dust[num228].noGravity = true;
                                Main.dust[num228].velocity = vector7;
                            }
                        };

                        npc.ai[2] = 1;
                        npc.netUpdate = true;
                        
                        int set = Main.rand.Next(configurations.Length);
                        while (heads[configurations[set].Key] == heads[configurations[lastSet].Key] //don't reuse heads you just attacked with
                            || heads[configurations[set].Key] == heads[configurations[lastSet].Value]
                            || heads[configurations[set].Value] == heads[configurations[lastSet].Key]
                            || heads[configurations[set].Value] == heads[configurations[lastSet].Value])
                            set = Main.rand.Next(configurations.Length);
                        lastSet = set;

                        ActivateHead(heads[configurations[set].Key]);
                        ActivateHead(heads[configurations[set].Value]);
                    }

                    if (++npc.ai[1] > 300) //wait
                    {
                        npc.TargetClosest();
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        npc.netUpdate = true;
                    }
                    goto case -1;

                case 4:
                    goto case 0;

                case 5:
                    goto case 3;

                case 6:
                    goto case 0;

                case 7:
                    goto case 3;

                case 8:
                    goto case 0;

                case 9:
                    goto case 1;

                case 10:
                    goto case 0;

                case 11: //deathrays
                    if (npc.ai[2] == 0)
                    {
                        npc.ai[2] = 1;

                        Main.PlaySound(SoundID.Roar, npc.Center, 0);

                        for (int i = 0; i < heads.Length; i++) //activate all heads
                        {
                            Main.npc[heads[i]].ai[0] = 4f;
                            Main.npc[heads[i]].netUpdate = true;

                            int dustType;
                            switch ((int)Main.npc[heads[i]].ai[3])
                            {
                                case -3: dustType = 183; break;
                                case -2: dustType = 6; break;
                                case -1: dustType = 87; break;
                                case 1: dustType = 111; break;
                                case 2: dustType = 89; break;
                                case 3: dustType = 113; break;
                                default: dustType = 1; break;
                            }

                            const int num226 = 70;
                            for (int num227 = 0; num227 < num226; num227++)
                            {
                                Vector2 vector6 = Vector2.UnitX * 30f;
                                vector6 = vector6.RotatedBy(((num227 - (num226 / 2 - 1)) * 6.28318548f / num226), default(Vector2)) + Main.npc[heads[i]].Center;
                                Vector2 vector7 = vector6 - Main.npc[heads[i]].Center;
                                int num228 = Dust.NewDust(vector6 + vector7, 0, 0, dustType, 0f, 0f, 0, default(Color), 3f);
                                Main.dust[num228].noGravity = true;
                                Main.dust[num228].velocity = vector7;
                            }
                        }
                    }

                    if (++npc.ai[1] > 330) //wait
                    {
                        npc.TargetClosest();
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        npc.netUpdate = true;
                    }
                    goto case -1;

                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }

            npc.direction = npc.spriteDirection = npc.position.X < player.position.X ? 1 : -1;
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
            if (FargoSoulsWorld.MasochistMode)
            {
                target.AddBuff(BuffID.Frostburn, 300);
                target.AddBuff(BuffID.OnFire, 300);
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void NPCLoot()
        {
            FargoSoulsWorld.downedChampions[3] = true;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData); //sync world

            int[] drops = {
                ModContent.ItemType<CrimsonEnchant>(),
                ModContent.ItemType<MoltenEnchant>(),
                ModContent.ItemType<RainEnchant>(),
                ModContent.ItemType<FrostEnchant>(),
                ModContent.ItemType<ChlorophyteEnchant>(),
                ModContent.ItemType<ShroomiteEnchant>()
            };
            int lastDrop = -1; //don't drop same ench twice
            for (int i = 0; i < 2; i++)
            {
                int thisDrop = Main.rand.Next(drops.Length);

                if (lastDrop == thisDrop) //try again
                {
                    if (++thisDrop >= drops.Length) //drop first ench in line if looped past array
                        thisDrop = 0;
                }

                lastDrop = thisDrop;
                Item.NewItem(npc.position, npc.Size, drops[thisDrop]);
            }
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