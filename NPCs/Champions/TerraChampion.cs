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
    public class TerraChampion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Champion of Terra");
        }

        public override void SetDefaults()
        {
            npc.width = 80;
            npc.height = 80;
            npc.damage = 150;
            npc.defense = 80;
            npc.lifeMax = 390000;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.aiStyle = -1;
            npc.value = Item.buyPrice(0, 10);

            npc.boss = true;
            music = MusicID.Boss1;
            musicPriority = MusicPriority.BossMedium;

            for (int i = 0; i < npc.buffImmune.Length; i++)
                npc.buffImmune[i] = true;
            npc.GetGlobalNPC<FargoSoulsGlobalNPC>().SpecialEnchantImmune = true;

            npc.behindTiles = true;

            npc.scale *= 1.5f;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return npc.Distance(target.Center) < 30 * npc.scale;
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
            if (npc.localAI[3] == 0) //just spawned
            {
                npc.localAI[3] = 1;
                npc.TargetClosest(false);

                if (Main.netMode != 1) //spawn segments
                {
                    int prev = npc.whoAmI;
                    const int max = 99;
                    for (int i = 0; i < max; i++)
                    {
                        int type = i == max - 1 ? ModContent.NPCType<TerraChampionTail>() : ModContent.NPCType<TerraChampionBody>();
                        int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, type, npc.whoAmI);
                        if (n != 200)
                        {
                            Main.npc[n].ai[3] = npc.whoAmI;
                            Main.npc[n].realLife = npc.whoAmI;
                            Main.npc[n].ai[1] = prev;
                            Main.npc[prev].ai[0] = n;

                            if (Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);

                            prev = n;
                        }
                        else //can't spawn all segments
                        {
                            npc.active = false;
                            if (Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
                            return;
                        }
                    }
                }
            }

            EModeGlobalNPC.championBoss = npc.whoAmI;

            Player player = Main.player[npc.target];
            Vector2 targetPos;

            if (npc.HasValidTarget && player.Center.Y >= Main.worldSurface * 16 && !player.ZoneUnderworldHeight)
                npc.timeLeft = 600;

            if (npc.position.Y < 0 || npc.position.Y > Main.maxTilesY * 16) //OOB elevation, leave
            {
                npc.active = false;
                return;
            }

            if (npc.ai[1] != -1 && npc.life < npc.lifeMax / 10)
            {
                Main.PlaySound(15, npc.Center, 0);
                npc.ai[1] = -1f;
                npc.localAI[0] = 0;
                npc.netUpdate = true;
            }

            switch ((int)npc.ai[1])
            {
                case -1: //flying head alone
                    if (!player.active || player.dead || player.Center.Y < Main.worldSurface * 16 || player.ZoneUnderworldHeight) //despawn code
                    {
                        npc.TargetClosest(false);
                        if (npc.timeLeft > 30)
                            npc.timeLeft = 30;
                        npc.velocity.Y += 1f;
                        break;
                    }

                    npc.scale = 2f;
                    targetPos = player.Center;
                    Movement(targetPos, 0.22f, 32f);

                    npc.rotation = npc.DirectionTo(player.Center).ToRotation();

                    if (++npc.localAI[0] > 50)
                    {
                        npc.localAI[0] = 0;
                        float ai1New = Main.rand.Next(100);
                        Vector2 vel = Vector2.Normalize(npc.DirectionTo(player.Center).RotatedBy(Math.PI / 4 * (Main.rand.NextDouble() - 0.5))) * 6f;
                        Projectile.NewProjectile(npc.Center, vel, ProjectileID.CultistBossLightningOrbArc,
                            npc.damage / 4, 0, Main.myPlayer, npc.rotation, ai1New);
                    }
                    break;

                case 0: //ripped from destroyer
                    {
                        if (!player.active || player.dead || player.Center.Y < Main.worldSurface * 16 || player.ZoneUnderworldHeight) //despawn code
                        {
                            npc.TargetClosest(false);
                            if (npc.timeLeft > 30)
                                npc.timeLeft = 30;
                            npc.velocity.Y += 1f;
                            break;
                        }
                        
                        float num14 = 18f;    //max speed?
                        float num15 = 0.2f;   //turn speed?
                        float num16 = 0.25f;   //acceleration?

                        Vector2 target = player.Center;
                        float num17 = target.X;
                        float num18 = target.Y;

                        float num21 = num17 - npc.Center.X;
                        float num22 = num18 - npc.Center.Y;
                        float num23 = (float)Math.Sqrt((double)num21 * (double)num21 + (double)num22 * (double)num22);

                        //ground movement code but it always runs
                        float num2 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                        float num3 = Math.Abs(num21);
                        float num4 = Math.Abs(num22);
                        float num5 = num14 / num2;
                        float num6 = num21 * num5;
                        float num7 = num22 * num5;
                        if ((npc.velocity.X > 0f && num6 > 0f || npc.velocity.X < 0f && num6 < 0f) && (npc.velocity.Y > 0f && num7 > 0f || npc.velocity.Y < 0f && num7 < 0f))
                        {
                            if (npc.velocity.X < num6)
                                npc.velocity.X += num16;
                            else if (npc.velocity.X > num6)
                                npc.velocity.X -= num16;
                            if (npc.velocity.Y < num7)
                                npc.velocity.Y += num16;
                            else if (npc.velocity.Y > num7)
                                npc.velocity.Y -= num16;
                        }
                        if (npc.velocity.X > 0f && num6 > 0f || npc.velocity.X < 0f && num6 < 0f || npc.velocity.Y > 0f && num7 > 0f || npc.velocity.Y < 0f && num7 < 0f)
                        {
                            if (npc.velocity.X < num6)
                                npc.velocity.X += num15;
                            else if (npc.velocity.X > num6)
                                npc.velocity.X -= num15;
                            if (npc.velocity.Y < num7)
                                npc.velocity.Y += num15;
                            else if (npc.velocity.Y > num7)
                                npc.velocity.Y -= num15;

                            if (Math.Abs(num7) < num14 * 0.2f && (npc.velocity.X > 0f && num6 < 0f || npc.velocity.X < 0f && num6 > 0f))
                            {
                                if (npc.velocity.Y > 0f)
                                    npc.velocity.Y += num15 * 2f;
                                else
                                    npc.velocity.Y -= num15 * 2f;
                            }
                            if (Math.Abs(num6) < num14 * 0.2f && (npc.velocity.Y > 0f && num7 < 0f || npc.velocity.Y < 0f && num7 > 0f))
                            {
                                if (npc.velocity.X > 0f)
                                    npc.velocity.X += num15 * 2f;
                                else
                                    npc.velocity.X -= num15 * 2f;
                            }
                        }
                        else if (num3 > num4)
                        {
                            if (npc.velocity.X < num6)
                                npc.velocity.X += num15 * 1.1f;
                            else if (npc.velocity.X > num6)
                                npc.velocity.X -= num15 * 1.1f;

                            if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num14 * 0.5f)
                            {
                                if (npc.velocity.Y > 0f)
                                    npc.velocity.Y += num15;
                                else
                                    npc.velocity.Y -= num15;
                            }
                        }
                        else
                        {
                            if (npc.velocity.Y < num7)
                                npc.velocity.Y += num15 * 1.1f;
                            else if (npc.velocity.Y > num7)
                                npc.velocity.Y -= num15 * 1.1f;

                            if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num14 * 0.5f)
                            {
                                if (npc.velocity.X > 0f)
                                    npc.velocity.X += num15;
                                else
                                    npc.velocity.X -= num15;
                            }
                        }

                        if (++npc.localAI[0] > 360)
                        {

                        }
                    }
                    break;

                default:
                    npc.ai[1] = 0;
                    goto case 0;
            }

            npc.netUpdate = true;

            if (npc.ai[1] != -1f)
                npc.rotation = npc.velocity.ToRotation();

            Vector2 dustOffset = new Vector2(77, -41) * npc.scale; //dust from horns
            int dust = Dust.NewDust(npc.Center + npc.velocity - dustOffset.RotatedBy(npc.rotation), 0, 0, DustID.Fire, npc.velocity.X * .4f, npc.velocity.Y * 0.4f, 0, default(Color), 2f);
            Main.dust[dust].velocity *= 2;
            if (Main.rand.Next(2) == 0)
            {
                Main.dust[dust].scale++;
                Main.dust[dust].noGravity = true;
            }

            dustOffset.Y *= -1f;
            dust = Dust.NewDust(npc.Center + npc.velocity - dustOffset.RotatedBy(npc.rotation), 0, 0, DustID.Fire, npc.velocity.X * .4f, npc.velocity.Y * 0.4f, 0, default(Color), 2f);
            Main.dust[dust].velocity *= 2;
            if (Main.rand.Next(2) == 0)
            {
                Main.dust[dust].scale++;
                Main.dust[dust].noGravity = true;
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

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (npc.life < npc.lifeMax)
                damage /= 5;
            return true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 600);
            target.AddBuff(ModContent.BuffType<LivingWasteland>(), 600);
            target.AddBuff(ModContent.BuffType<LightningRod>(), 600);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void NPCLoot()
        {
            //Item.NewItem(npc.position, npc.Size, ModContent.ItemType<EarthForce>());
            int[] drops = {
                ModContent.ItemType<CopperEnchant>(),
                ModContent.ItemType<TinEnchant>(),
                ModContent.ItemType<IronEnchant>(),
                ModContent.ItemType<LeadEnchant>(),
                ModContent.ItemType<TungstenEnchant>(),
                ModContent.ItemType<ObsidianEnchant>()
            };
            int lastDrop = 0; //don't drop same ench twice
            for (int i = 0; i < 2; i++)
            {
                int thisDrop = drops[Main.rand.Next(drops.Length)];

                if (lastDrop == thisDrop && !Main.dedServ) //try again
                {
                    i--;
                    continue;
                }

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

            SpriteEffects effects = npc.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.spriteBatch.Draw(texture2D13, npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), npc.GetAlpha(lightColor), npc.rotation, origin2, npc.scale, effects, 0f);
            //Texture2D glowmask = ModContent.GetTexture("FargowiltasSouls/NPCs/Champions/TerraChampion_Glow");
            //Main.spriteBatch.Draw(glowmask, npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, npc.rotation, origin2, npc.scale, effects, 0f);
            return false;
        }
    }
}