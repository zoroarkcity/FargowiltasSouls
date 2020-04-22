using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using FargowiltasSouls.Items.Accessories.Enchantments;
using FargowiltasSouls.Projectiles.Masomode;
using FargowiltasSouls.Projectiles.Champions;

namespace FargowiltasSouls.NPCs.Champions
{
    [AutoloadBossHead]
    public class EarthChampion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Champion of Earth");
            NPCID.Sets.TrailCacheLength[npc.type] = 6;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.width = 120;
            npc.height = 120;
            npc.damage = 150;
            npc.defense = 80;
            npc.lifeMax = 320000;
            npc.HitSound = SoundID.NPCHit41;
            npc.DeathSound = SoundID.NPCDeath44;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.aiStyle = -1;
            npc.value = Item.buyPrice(0, 10);

            npc.boss = true;
            music = MusicID.Boss1;
            musicPriority = MusicPriority.BossMedium;

            npc.buffImmune[BuffID.Chilled] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Suffocation] = true;
            npc.buffImmune[mod.BuffType("Lethargic")] = true;
            npc.buffImmune[mod.BuffType("ClippedWings")] = true;
            npc.GetGlobalNPC<FargoSoulsGlobalNPC>().SpecialEnchantImmune = true;

            npc.trapImmune = true;
            npc.scale = 1.5f;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        public override void AI()
        {
            if (npc.localAI[3] == 0) //just spawned
            {
                npc.localAI[3] = 1;
                npc.TargetClosest(false);

                if (Main.netMode != 1)
                {
                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<EarthChampionHand>(), npc.whoAmI, 0, 0, npc.whoAmI, 1);
                    if (n < Main.maxNPCs)
                    {
                        Main.npc[n].velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(32f);
                        if (Main.netMode == 2)
                        NetMessage.SendData(23, -1, -1, null, n);
                    }

                    n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<EarthChampionHand>(), npc.whoAmI, 0, 0, npc.whoAmI, -1);
                    if (n < Main.maxNPCs)
                    {
                        Main.npc[n].velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(32f);
                        if (Main.netMode == 2)
                            NetMessage.SendData(23, -1, -1, null, n);
                    }
                }
            }

            EModeGlobalNPC.championBoss = npc.whoAmI;

            Player player = Main.player[npc.target];
            Vector2 targetPos;

            if (npc.HasValidTarget && npc.Distance(player.Center) < 2500 && player.ZoneUnderworldHeight)
                npc.timeLeft = 600;

            switch ((int)npc.ai[0])
            {
                case -1:
                    npc.localAI[2] = 1;

                    if (++npc.ai[1] < 120)
                    {
                        targetPos = player.Center;
                        targetPos.Y -= 375;
                        if (npc.Distance(targetPos) > 50)
                            Movement(targetPos, 0.6f, 24f, true);
                    }
                    else if (npc.ai[1] == 120) //begin healing
                    {
                        Main.PlaySound(SoundID.NPCDeath10, npc.Center);

                        const int num226 = 80;
                        for (int num227 = 0; num227 < num226; num227++)
                        {
                            Vector2 vector6 = Vector2.UnitX * 40f;
                            vector6 = vector6.RotatedBy(((num227 - (num226 / 2 - 1)) * 6.28318548f / num226), default(Vector2)) + npc.Center;
                            Vector2 vector7 = vector6 - npc.Center;
                            int num228 = Dust.NewDust(vector6 + vector7, 0, 0, 174, 0f, 0f, 0, default(Color), 3f);
                            Main.dust[num228].noGravity = true;
                            Main.dust[num228].velocity = vector7;
                        }
                    }
                    else if (npc.ai[1] > 120) //healing
                    {
                        npc.velocity *= 0.9f;
                        
                        int heal = (int)(npc.lifeMax / 2 / 120 * Main.rand.NextFloat(1f, 1.5f));
                        npc.life += heal;
                        if (npc.life > npc.lifeMax)
                            npc.life = npc.lifeMax;
                        CombatText.NewText(npc.Hitbox, CombatText.HealLife, heal);

                        for (int i = 0; i < 5; i++)
                        {
                            int d = Dust.NewDust(npc.Center, 0, 0, 174, 0f, 0f, 0, default(Color), 1.5f);
                            Main.dust[d].noGravity = true;
                            Main.dust[d].velocity *= 8f;
                        }

                        if (npc.ai[1] > 240)
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.netUpdate = true;
                        }
                    }
                    break;

                case 0: //float over player
                    if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 2500f
                        || !player.ZoneUnderworldHeight) //despawn code
                    {
                        npc.TargetClosest(false);
                        if (npc.timeLeft > 30)
                            npc.timeLeft = 30;

                        npc.noTileCollide = true;
                        npc.noGravity = true;
                        npc.velocity.Y += 1f;

                        return;
                    }
                    else
                    {
                        targetPos = player.Center;
                        targetPos.Y -= 325;
                        if (npc.Distance(targetPos) > 50)
                            Movement(targetPos, 0.4f, 24f, true);
                    }

                    if (npc.localAI[2] == 0 && npc.life < npc.lifeMax / 2)
                    {
                        npc.ai[0] = -1;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;

                        for (int i = 0; i < Main.maxNPCs; i++) //find hands, update
                        {
                            if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<EarthChampionHand>() && Main.npc[i].ai[2] == npc.whoAmI)
                            {
                                Main.npc[i].ai[0] = -1;
                                Main.npc[i].ai[1] = 0;
                                Main.npc[i].localAI[0] = 0;
                                Main.npc[i].localAI[1] = 0;
                                Main.npc[i].netUpdate = true;
                            }
                        }
                    }
                    break;

                case 1: //fireballs
                    if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 2500f
                        || !player.ZoneUnderworldHeight) //despawn code
                    {
                        npc.TargetClosest(false);
                        if (npc.timeLeft > 30)
                            npc.timeLeft = 30;

                        npc.noTileCollide = true;
                        npc.noGravity = true;
                        npc.velocity.Y += 1f;

                        return;
                    }
                    else
                    {
                        targetPos = player.Center;
                        targetPos.Y -= 350;
                        if (npc.Distance(targetPos) > 50)
                            Movement(targetPos, 0.2f, 24f, true);

                        if (++npc.ai[2] > 75)
                        {
                            npc.ai[2] = 0;
                            if (Main.netMode != 1) //shoot spread of fireballs
                            {
                                for (int i = -1; i <= 1; i++)
                                {
                                    Projectile.NewProjectile(npc.Center + Vector2.UnitY * 60,
                                        (npc.localAI[2] == 1 ? 12 : 8) * npc.DirectionTo(player.Center).RotatedBy(MathHelper.ToRadians(8 * i)),
                                        ProjectileID.Fireball, npc.damage / 4, 0f, Main.myPlayer);
                                }
                            }
                        }

                        if (++npc.ai[1] > 480)
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.netUpdate = true;
                        }
                    }

                    if (npc.localAI[2] == 0 && npc.life < npc.lifeMax / 2)
                    {
                        npc.ai[0] = -1;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;

                        for (int i = 0; i < Main.maxNPCs; i++) //find hands, update
                        {
                            if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<EarthChampionHand>() && Main.npc[i].ai[2] == npc.whoAmI)
                            {
                                Main.npc[i].ai[0] = -1;
                                Main.npc[i].ai[1] = 0;
                                Main.npc[i].localAI[0] = 0;
                                Main.npc[i].localAI[1] = 0;
                                Main.npc[i].netUpdate = true;
                            }
                        }
                    }
                    break;

                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }
        }

        private void Movement(Vector2 targetPos, float speedModifier, float cap = 12f, bool fastY = false)
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
                npc.velocity.Y += fastY ? speedModifier * 2 : speedModifier;
                if (npc.velocity.Y < 0)
                    npc.velocity.Y += speedModifier * 2;
            }
            else
            {
                npc.velocity.Y -= fastY ? speedModifier * 2 : speedModifier;
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
            target.AddBuff(BuffID.OnFire, 300);
            target.AddBuff(BuffID.Burning, 300);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void NPCLoot()
        {
            //Item.NewItem(npc.position, npc.Size, ModContent.ItemType<EarthForce>());
            int[] drops = {
                ModContent.ItemType<CobaltEnchant>(),
                ModContent.ItemType<PalladiumEnchant>(),
                ModContent.ItemType<MythrilEnchant>(),
                ModContent.ItemType<OrichalcumEnchant>(),
                ModContent.ItemType<AdamantiteEnchant>(),
                ModContent.ItemType<TitaniumEnchant>()
            };
            int lastDrop = 0; //don't drop same ench twice
            for (int i = 0; i < 2; i++)
            {
                int thisDrop = drops[Main.rand.Next(drops.Length)];

                if (lastDrop == thisDrop) //try again
                    i--;

                lastDrop = thisDrop;
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
            Texture2D glowmask = ModContent.GetTexture("FargowiltasSouls/NPCs/Champions/EarthChampion_Glow");
            Main.spriteBatch.Draw(glowmask, npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, npc.rotation, origin2, npc.scale, effects, 0f);
            return false;
        }
    }
}