using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using FargowiltasSouls.Items.Accessories.Enchantments;
using FargowiltasSouls.Projectiles.Masomode;
using FargowiltasSouls.Projectiles.Champions;
using Microsoft.Xna.Framework.Graphics;

namespace FargowiltasSouls.NPCs.Champions
{
    [AutoloadBossHead]
    public class TimberChampionHead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Champion of Timber");
            Main.npcFrameCount[npc.type] = 3;
            NPCID.Sets.TrailCacheLength[npc.type] = 6;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.width = 98;
            npc.height = 76;
            npc.damage = 130;
            npc.defense = 50;
            npc.lifeMax = 200000;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.aiStyle = -1;
            npc.value = Item.buyPrice(0, 15);
            npc.boss = true;

            npc.buffImmune[BuffID.Chilled] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Suffocation] = true;
            npc.buffImmune[mod.BuffType("Lethargic")] = true;
            npc.buffImmune[mod.BuffType("ClippedWings")] = true;
            npc.GetGlobalNPC<FargoSoulsGlobalNPC>().SpecialEnchantImmune = true;

            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Champions");
            musicPriority = MusicPriority.BossHigh;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

        public override void AI()
        {
            if (npc.localAI[2] == 0)
            {
                Main.PlaySound(SoundID.Roar, npc.Center, 0);
                npc.TargetClosest(false);
                npc.localAI[2] = 1;
            }

            EModeGlobalNPC.championBoss = npc.whoAmI;

            Player player = Main.player[npc.target];
            npc.direction = npc.spriteDirection = npc.position.X < player.position.X ? 1 : -1;
            Vector2 targetPos;
            
            switch ((int)npc.ai[0])
            {
                case 0:
                    if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 2500f) //despawn code
                    {
                        npc.TargetClosest(false);
                        if (npc.timeLeft > 30)
                            npc.timeLeft = 30;

                        npc.noTileCollide = true;
                        npc.noGravity = true;
                        npc.velocity.Y -= 1f;

                        break;
                    }
                    else
                    {
                        targetPos = player.Center + npc.DirectionFrom(player.Center) * 400f;
                        if (npc.Distance(targetPos) > 50)
                            Movement(targetPos, 0.2f, 24f);
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

        public override void FindFrame(int frameHeight)
        {
            if (++npc.frameCounter > 3)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (npc.frame.Y >= frameHeight * Main.npcFrameCount[npc.type])
                    npc.frame.Y = 0;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (FargoSoulsWorld.MasochistMode)
                target.AddBuff(ModContent.BuffType<Buffs.Masomode.Guilty>(), 600);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Vector2 pos = npc.position + new Vector2(Main.rand.NextFloat(npc.width), Main.rand.NextFloat(npc.height));
                Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/TimberGore1"), npc.scale);
                pos = npc.position + new Vector2(Main.rand.NextFloat(npc.width), Main.rand.NextFloat(npc.height));
                Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/TimberGore4"), npc.scale);
                pos = npc.position + new Vector2(Main.rand.NextFloat(npc.width), Main.rand.NextFloat(npc.height));
                Gore.NewGore(pos, npc.velocity, mod.GetGoreSlot("Gores/TimberGore5"), npc.scale);
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void NPCLoot()
        {
            FargoSoulsWorld.downedChampions[0] = true;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData); //sync world

            int[] drops = {
                ModContent.ItemType<WoodEnchant>(),
                ModContent.ItemType<BorealWoodEnchant>(),
                ModContent.ItemType<RichMahoganyEnchant>(),
                ModContent.ItemType<EbonwoodEnchant>(),
                ModContent.ItemType<ShadewoodEnchant>(),
                ModContent.ItemType<PalmWoodEnchant>(),
                ModContent.ItemType<PearlwoodEnchant>()
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

        public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
        {
            spriteEffects = npc.direction < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture2D13 = Main.npcTexture[npc.type];
            Texture2D texture2D14 = mod.GetTexture("NPCs/Champions/TimberChampionHead_Trail");
            Texture2D texture2D15 = mod.GetTexture("NPCs/Champions/TimberChampionHead_Glow");
            //int num156 = Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type]; //ypos of lower right corner of sprite to draw
            //int y3 = num156 * npc.frame.Y; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = npc.frame;//new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = drawColor;
            color26 = npc.GetAlpha(color26);

            SpriteEffects effects = npc.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int i = 0; i < NPCID.Sets.TrailCacheLength[npc.type]; i++)
            {
                Color color27 = Color.White * 0.5f;
                color27 *= (float)(NPCID.Sets.TrailCacheLength[npc.type] - i) / NPCID.Sets.TrailCacheLength[npc.type];
                Vector2 value4 = npc.oldPos[i];
                float num165 = npc.rotation; //npc.oldRot[i];
                Main.spriteBatch.Draw(texture2D14, value4 + npc.Size / 2f - Main.screenPosition + new Vector2(0, npc.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, npc.scale, effects, 0f);
            }

            Main.spriteBatch.Draw(texture2D13, npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), npc.GetAlpha(drawColor), npc.rotation, origin2, npc.scale, effects, 0f);
            Main.spriteBatch.Draw(texture2D15, npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, npc.rotation, origin2, npc.scale, effects, 0f);
            return false;
        }
    }
}