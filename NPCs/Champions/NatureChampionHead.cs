using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Buffs.Masomode;
using FargowiltasSouls.Projectiles.Masomode;
using FargowiltasSouls.Projectiles.Champions;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace FargowiltasSouls.NPCs.Champions
{
    [AutoloadBossHead]
    public class NatureChampionHead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Champion of Nature");
            Main.npcFrameCount[npc.type] = 6;
            NPCID.Sets.TrailCacheLength[npc.type] = 6;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.width = 100;
            npc.height = 100;
            npc.damage = 150;
            npc.defense = 150;
            npc.lifeMax = 700000;
            npc.HitSound = SoundID.NPCHit6;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.aiStyle = -1;

            npc.buffImmune[BuffID.Chilled] = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Suffocation] = true;
            npc.buffImmune[mod.BuffType("Lethargic")] = true;
            npc.buffImmune[mod.BuffType("ClippedWings")] = true;
            npc.GetGlobalNPC<FargoSoulsGlobalNPC>().SpecialEnchantImmune = true;
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

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

        public override void AI()
        {
            if (!(npc.ai[1] > -1 && npc.ai[1] < Main.maxNPCs && Main.npc[(int)npc.ai[1]].active
                && Main.npc[(int)npc.ai[1]].type == ModContent.NPCType<NatureChampion>()))
            {
                npc.life = 0;
                npc.checkDead();
                npc.active = false;
                return;
            }
            
            NPC body = Main.npc[(int)npc.ai[1]];
            
            npc.target = body.target;
            npc.realLife = body.whoAmI;
            npc.position += body.velocity * 0.75f;

            Player player = Main.player[npc.target];
            Vector2 targetPos;

            npc.direction = npc.spriteDirection = npc.position.X < player.position.X ? 1 : -1;
            npc.rotation = 0;
            
            switch ((int)npc.ai[0])
            {
                case -3: //crimson
                    if (body.Distance(player.Center) < 3000 && body.Distance(player.Center) > 300)
                        targetPos = player.Center - Vector2.UnitY * 250f;
                    else
                        targetPos = body.Center + body.DirectionTo(player.Center) * 300;
                    Movement(targetPos, 0.4f, 24f);

                    if (++npc.ai[2] > 30) //ichor every other 0.5 seconds
                    {
                        if (npc.ai[2] > 60)
                        {
                            npc.ai[2] = 0;
                        }

                        if (++npc.localAI[1] > 2) //rain piss
                        {
                            npc.localAI[1] = 0;
                            if (npc.localAI[0] > 60 && Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.Center + Main.rand.NextVector2Circular(npc.width / 2, npc.height / 2),
                                    Vector2.UnitY * Main.rand.NextFloat(4f, 8f), ProjectileID.GoldenShowerHostile, npc.damage / 4, 0f, Main.myPlayer);
                            }
                        }
                    }

                    if (++npc.localAI[0] > 300)
                    {
                        npc.ai[0] = 0;
                        npc.localAI[0] = 0;
                        npc.ai[2] = 0;
                        npc.localAI[1] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case -2: //molten
                    if (++npc.ai[2] > 60)
                    {
                        npc.ai[2] = 0;

                        Main.PlaySound(2, npc.Center, 14);

                        if (Main.netMode != 1)
                        {
                            const int max = 12;
                            for (int i = 0; i < max; i++)
                            {
                                Vector2 speed = 20f * npc.DirectionTo(player.Center).RotatedBy(2 * Math.PI / max * i);
                                Projectile.NewProjectile(npc.Center, speed, ModContent.ProjectileType<NatureFireball>(), npc.damage / 4, 0f, Main.myPlayer);
                            }
                        }
                    }

                    if (++npc.localAI[0] < 240) //stay near
                    {
                        if (body.Distance(player.Center) < 3000 && body.Distance(player.Center) > 300)
                            targetPos = player.Center;
                        else
                            targetPos = body.Center + body.DirectionTo(player.Center) * 300;
                        Movement(targetPos, 0.12f, 24f);

                        for (int i = 0; i < 20; i++) //warning ring
                        {
                            Vector2 offset = new Vector2();
                            double angle = Main.rand.NextDouble() * 2d * Math.PI;
                            offset.X += (float)(Math.Sin(angle) * 400);
                            offset.Y += (float)(Math.Cos(angle) * 400);
                            Dust dust = Main.dust[Dust.NewDust(npc.Center + offset - new Vector2(4, 4), 0, 0, DustID.Fire, 0, 0, 100, Color.White, 2f)];
                            dust.velocity = npc.velocity;
                            if (Main.rand.Next(3) == 0)
                                dust.velocity += Vector2.Normalize(offset) * -5f;
                            dust.noGravity = true;
                        }
                    }
                    else if (npc.localAI[0] == 240) //explode into attacks
                    {
                        npc.velocity = Vector2.Zero;
                        npc.netUpdate = true;

                        Main.PlaySound(15, npc.Center, 0);

                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<NatureExplosion>(), npc.damage / 4, 0f, Main.myPlayer);
                        }
                    }
                    else if (npc.localAI[0] > 300)
                    {
                        npc.ai[0] = 0;
                        npc.ai[2] = 0;
                        npc.localAI[0] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case -1: //rain
                    if (body.Distance(player.Center) < 3000 && body.Distance(player.Center) > 300)
                        targetPos = player.Center + npc.DirectionFrom(player.Center) * 300;
                    else
                        targetPos = body.Center + body.DirectionTo(player.Center) * 300;
                    Movement(targetPos, 0.25f, 24f);

                    if (++npc.localAI[1] > 60)
                    {
                        npc.localAI[1] = 0;

                        Main.PlaySound(SoundID.Item66, npc.Center);

                        if (Main.netMode != 1)
                        {
                            Vector2 dir = Main.player[npc.target].Center - npc.Center;
                            float ai1New = Main.rand.Next(100);
                            Vector2 vel = Vector2.Normalize(dir.RotatedByRandom(Math.PI / 4)) * 6f;
                            Projectile.NewProjectile(npc.Center, vel, ProjectileID.CultistBossLightningOrbArc,
                                npc.damage / 4, 0, Main.myPlayer, dir.ToRotation(), ai1New);

                            Vector2 speed = player.Center - npc.Center;
                            speed.Y -= 300;
                            speed /= 40;
                            Projectile.NewProjectile(npc.Center, speed, ModContent.ProjectileType<NatureCloudMoving>(), npc.damage / 4, 0f, Main.myPlayer);
                        }
                    }

                    if (++npc.localAI[0] > 300)
                    {
                        npc.ai[0] = 0;
                        npc.localAI[0] = 0;
                        npc.ai[2] = 0;
                        npc.localAI[1] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 0: //float near body
                    {
                        Vector2 offset;
                        offset.X = 100f * npc.ai[3] - 50 * Math.Sign(npc.ai[3]);
                        offset.Y = -350 + 75f * Math.Abs(npc.ai[3]);
                        targetPos = body.Center + offset;
                        if (npc.Distance(targetPos) > 50)
                            Movement(targetPos, 0.8f, 24f);
                    }
                    break;

                case 1: //frost
                    {
                        Vector2 offset;
                        offset.X = 100f * npc.ai[3] - 50 * Math.Sign(npc.ai[3]);
                        offset.Y = -350 + 75f * Math.Abs(npc.ai[3]);
                        targetPos = body.Center + offset;
                        if (npc.Distance(targetPos) > 50)
                            Movement(targetPos, 0.8f, 24f);

                        if (++npc.ai[2] > 40)
                        {
                            npc.ai[2] = 0;
                            npc.localAI[1] = npc.localAI[1] == 1 ? -1 : 1;
                            if (Main.netMode != 1)
                            {
                                const int max = 25;
                                for (int i = 0; i < max; i++)
                                {
                                    Vector2 speed = Main.rand.NextFloat(2f, 5f) * Vector2.UnitX.RotatedBy(2 * Math.PI / max * (i + Main.rand.NextDouble()));
                                    Projectile.NewProjectile(npc.Center, speed, ModContent.ProjectileType<NatureIcicle>(),
                                        npc.damage / 4, 0f, Main.myPlayer, 60 + Main.rand.Next(20), npc.localAI[1]);
                                }
                            }
                        }

                        if (++npc.localAI[0] > 300)
                        {
                            npc.ai[0] = 0;
                            npc.localAI[0] = 0;
                            npc.ai[2] = 0;
                            npc.localAI[1] = 0;
                            npc.netUpdate = true;
                        }
                    }
                    break;

                case 2: //chlorophyte
                    if (body.Distance(player.Center) < 3000 && body.Distance(player.Center) > 300)
                        targetPos = player.Center;
                    else
                        targetPos = body.Center + body.DirectionTo(player.Center) * 300;
                    Movement(targetPos, 0.18f, 24f);

                    if (npc.ai[2] == 0)
                    {
                        npc.ai[2] = 1;
                        if (Main.netMode != 1)
                        {
                            const int max = 5;
                            const float distance = 125f;
                            float rotation = 2f * (float)Math.PI / max;
                            for (int i = 0; i < max; i++)
                            {
                                Vector2 spawnPos = npc.Center + new Vector2(distance, 0f).RotatedBy(rotation * i);
                                Projectile.NewProjectile(spawnPos, Vector2.Zero, ModContent.ProjectileType<NatureCrystalLeaf>(), npc.damage / 4, 0f, Main.myPlayer, npc.whoAmI, rotation * i);
                            }
                        }
                    }

                    if (++npc.localAI[0] > 300)
                    {
                        npc.ai[0] = 0;
                        npc.localAI[0] = 0;
                        npc.ai[2] = 0;
                        npc.localAI[1] = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 3: //shroomite
                    {
                        Vector2 offset;
                        offset.X = 100f * npc.ai[3] - 50 * Math.Sign(npc.ai[3]);
                        offset.Y = -350 + 75f * Math.Abs(npc.ai[3]);
                        targetPos = body.Center + offset;
                        if (npc.Distance(targetPos) > 50)
                            Movement(targetPos, 0.8f, 24f);

                        if (++npc.ai[2] > 2)
                        {
                            npc.ai[2] = 0;

                            if (npc.localAI[0] > 60)
                            {
                                Main.PlaySound(SoundID.Item11, npc.Center);

                                Vector2 speed = player.Center - npc.Center;
                                speed.X += Main.rand.Next(-40, 41);
                                speed.Y += Main.rand.Next(-40, 41);
                                speed.Normalize();
                                speed *= 14f;
                                if (Main.netMode != 1)
                                {
                                    Projectile.NewProjectile(npc.Center + speed * 5, speed,
                                        ModContent.ProjectileType<NatureBullet>(), npc.damage / 4, 0f, Main.myPlayer);
                                }
                            }
                        }

                        if (++npc.localAI[0] > 300)
                        {
                            npc.ai[0] = 0;
                            npc.localAI[0] = 0;
                            npc.ai[2] = 0;
                            npc.localAI[1] = 0;
                            npc.netUpdate = true;
                        }
                    }
                    break;

                case 4: //deathrays
                    {
                        Vector2 offset = -600 * Vector2.UnitY.RotatedBy(MathHelper.ToRadians(60 / 3) * npc.ai[3]);
                        targetPos = body.Center + offset;
                        Movement(targetPos, 0.8f, 24f);

                        npc.direction = npc.spriteDirection = npc.Center.X < body.Center.X ? 1 : -1;

                        if (++npc.ai[2] == 90)
                        {
                            npc.netUpdate = true;
                            npc.localAI[1] = npc.DirectionTo(body.Center - Vector2.UnitY * 300).ToRotation();

                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(npc.localAI[1]), 
                                    ModContent.ProjectileType<NatureDeathraySmall>(), npc.damage / 3, 0f, Main.myPlayer, 0f, npc.whoAmI);
                            }
                        }
                        else if (npc.ai[2] == 150)
                        {
                            float ai0 = 2f * (float)Math.PI / 120 * Math.Sign(npc.ai[3]);
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(npc.localAI[1]),
                                    ModContent.ProjectileType<NatureDeathray>(), npc.damage / 3, 0f, Main.myPlayer, ai0, npc.whoAmI);
                            }
                        }

                        if (++npc.localAI[0] > 330)
                        {
                            npc.ai[0] = 0;
                            npc.localAI[0] = 0;
                            npc.ai[2] = 0;
                            npc.localAI[1] = 0;
                            npc.netUpdate = true;
                        }
                    }
                    break;

                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            damage /= 3;
            return true;
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

        public override bool CheckActive()
        {
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            int frameModifier = (int)npc.ai[3];
            if (frameModifier > 0)
                frameModifier--;
            frameModifier += 3;

            npc.frame.Y = frameHeight * frameModifier;
            if (npc.frame.Y < 0)
                npc.frame.Y = 0;
            if (npc.frame.Y >= frameHeight * Main.npcFrameCount[npc.type])
                npc.frame.Y = frameHeight * (Main.npcFrameCount[npc.type] - 1);
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (npc.ai[1] > -1 && npc.ai[1] < Main.maxNPCs && Main.npc[(int)npc.ai[1]].active
                && Main.npc[(int)npc.ai[1]].type == ModContent.NPCType<NatureChampion>())
            {
                Texture2D texture = Main.chainTexture;
                Vector2 position = npc.Center;
                Vector2 mountedCenter = Main.npc[(int)npc.ai[1]].Center;
                Rectangle? sourceRectangle = new Rectangle?();
                Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
                float num1 = texture.Height;
                Vector2 vector24 = mountedCenter - position;
                float rotation = (float)Math.Atan2(vector24.Y, vector24.X) - 1.57f;
                bool flag = true;
                if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                    flag = false;
                if (float.IsNaN(vector24.X) && float.IsNaN(vector24.Y))
                    flag = false;
                while (flag)
                    if (vector24.Length() < num1 + 1.0)
                    {
                        flag = false;
                    }
                    else
                    {
                        Vector2 vector21 = vector24;
                        vector21.Normalize();
                        position += vector21 * num1;
                        vector24 = mountedCenter - position;
                        Color color2 = Lighting.GetColor((int)position.X / 16, (int)(position.Y / 16.0));
                        color2 = npc.GetAlpha(color2);
                        Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                    }
            }

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