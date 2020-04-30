using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
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
        public int[] heads = new int[5];

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
            npc.damage = 150;
            npc.defense = 120;
            npc.lifeMax = 700000;
            npc.HitSound = SoundID.NPCHit6;
            npc.DeathSound = SoundID.NPCDeath1;
            //npc.noGravity = true;
            //npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.aiStyle = -1;
            npc.value = Item.buyPrice(0, 10);
            
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
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            for (int i = 0; i < heads.Length; i++)
                heads[i] = reader.ReadInt32();
        }

        public override void AI()
        {
            if (npc.localAI[3] == 0) //spawn friends
            {
                npc.localAI[3] = 1;
                npc.TargetClosest(false);

                if (Main.netMode != 1)
                {
                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<NatureChampionHead>(), npc.whoAmI, 0f, npc.whoAmI, 0f, -2f, npc.target);
                    if (n != Main.maxNPCs)
                    {
                        Main.npc[n].velocity.X = Main.rand.NextFloat(-24f, 24f);
                        Main.npc[n].velocity.Y = Main.rand.NextFloat(-24f, 24f);
                        if (Main.netMode != 2)
                            NetMessage.SendData(23, -1, -1, null, n);
                    }
                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<NatureChampionHead>(), npc.whoAmI, 0f, npc.whoAmI, 0f, -1f, npc.target);
                    if (n != Main.maxNPCs)
                    {
                        Main.npc[n].velocity.X = Main.rand.NextFloat(-24f, 24f);
                        Main.npc[n].velocity.Y = Main.rand.NextFloat(-24f, 24f);
                        if (Main.netMode != 2)
                            NetMessage.SendData(23, -1, -1, null, n);
                    }
                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<NatureChampionHead>(), npc.whoAmI, 0f, npc.whoAmI, 0f, 0f, npc.target);
                    if (n != Main.maxNPCs)
                    {
                        Main.npc[n].velocity.X = Main.rand.NextFloat(-24f, 24f);
                        Main.npc[n].velocity.Y = Main.rand.NextFloat(-24f, 24f);
                        if (Main.netMode != 2)
                            NetMessage.SendData(23, -1, -1, null, n);
                    }
                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<NatureChampionHead>(), npc.whoAmI, 0f, npc.whoAmI, 0f, 1f, npc.target);
                    if (n != Main.maxNPCs)
                    {
                        Main.npc[n].velocity.X = Main.rand.NextFloat(-24f, 24f);
                        Main.npc[n].velocity.Y = Main.rand.NextFloat(-24f, 24f);
                        if (Main.netMode != 2)
                            NetMessage.SendData(23, -1, -1, null, n);
                    }
                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<NatureChampionHead>(), npc.whoAmI, 0f, npc.whoAmI, 0f, 2f, npc.target);
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
            
            if (npc.HasValidTarget && npc.Distance(player.Center) < 2500 && player.Center.Y >= Main.worldSurface * 16 && !player.ZoneUnderworldHeight)
                npc.timeLeft = 600;
            
            switch ((int)npc.ai[0])
            {
                case 0: //float to player
                    if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 2500f
                        || player.Center.Y < Main.worldSurface * 16 || player.ZoneUnderworldHeight) //despawn code
                    {
                        npc.TargetClosest(false);
                        if (npc.timeLeft > 30)
                            npc.timeLeft = 30;

                        npc.noTileCollide = true;
                        npc.noGravity = true;
                        npc.velocity.Y += 1f;

                        return;
                    }

                    if (++npc.ai[1] > 60)
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
            target.AddBuff(BuffID.Frostburn, 300);
            target.AddBuff(BuffID.OnFire, 300);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void NPCLoot()
        {
            int[] drops = {
                ModContent.ItemType<CrimsonEnchant>(),
                ModContent.ItemType<MoltenEnchant>(),
                ModContent.ItemType<RainEnchant>(),
                ModContent.ItemType<FrostEnchant>(),
                ModContent.ItemType<ChlorophyteEnchant>(),
                ModContent.ItemType<ShroomiteEnchant>()
            };
            //int lastDrop = 0; //don't drop same ench twice
            for (int i = 0; i < 2; i++)
            {
                int thisDrop = drops[Main.rand.Next(drops.Length)];

                /*if (lastDrop == thisDrop && !Main.dedServ) //try again
                {
                    i--;
                    continue;
                }

                lastDrop = thisDrop;*/
                Item.NewItem(npc.position, npc.Size, thisDrop);
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