using Microsoft.Xna.Framework;
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
    public class TimberChampion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Champion of Timber");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.width = 160;
            npc.height = 228;
            npc.damage = 150;
            npc.defense = 50;
            npc.lifeMax = 320000;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = false;
            npc.noTileCollide = false;
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
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

        public override void AI()
        {
            EModeGlobalNPC.championBoss = npc.whoAmI;

            Player player = Main.player[npc.target];
            npc.direction = npc.spriteDirection = npc.position.X < player.position.X ? 1 : -1;
            
            switch ((int)npc.ai[0])
            {
                case 0: //jump at player
                    npc.noTileCollide = false;
                    npc.noGravity = false;

                    if (++npc.ai[1] == 60)
                    {
                        npc.TargetClosest();

                        if (npc.localAI[0] == 0 && npc.life < npc.lifeMax * .66f) //spawn palm tree supports
                        {
                            npc.localAI[0] = 1;
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<PalmTreeHostile>(), npc.damage / 4, 0f, Main.myPlayer, npc.whoAmI);
                            }
                        }

                        if (npc.localAI[1] == 0 && npc.life < npc.lifeMax * .33f) //spawn palm tree supports
                        {
                            npc.localAI[1] = 1;
                            if (Main.netMode != 1)
                            {
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<PalmTreeHostile>(), npc.damage / 4, 0f, Main.myPlayer, npc.whoAmI);
                            }
                        }

                        const float gravity = 0.4f;
                        const float time = 90f;
                        
                        Vector2 distance = player.Center - npc.Center;
                        distance.Y -= npc.height;

                        distance.X = distance.X / time;
                        distance.Y = distance.Y / time - 0.5f * gravity * time;
                        npc.velocity = distance;
                        npc.netUpdate = true;

                        if (Main.netMode != 1) //explosive jump
                        {
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.DD2OgreSmash, npc.damage / 4, 0, Main.myPlayer);
                        }

                        Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 14);

                        for (int k = -2; k <= 2; k++) //explosions
                        {
                            Vector2 dustPos = npc.position;
                            int width = npc.width / 5;
                            dustPos.X += width * k + Main.rand.NextFloat(-width, width);
                            dustPos.Y += npc.height - width / 2 + Main.rand.NextFloat(-width, width) / 2;

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
                    else if (npc.ai[1] > 60)
                    {
                        npc.noTileCollide = true;
                        npc.noGravity = true;
                        npc.velocity.Y += 0.4f;

                        if (npc.ai[1] > 60 + 90)
                        {
                            npc.TargetClosest();
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.netUpdate = true;
                        }
                    }
                    else //less than 60
                    {
                        if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y))
                            npc.velocity.X = Math.Abs(npc.velocity.Y) * Math.Sign(npc.velocity.X);
                        if (npc.velocity.Y == 0)
                            npc.velocity.X *= 0.99f;

                        if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 2500f)
                        {
                            npc.TargetClosest();
                            if (npc.timeLeft > 30)
                                npc.timeLeft = 30;

                            npc.noTileCollide = true;
                            npc.noGravity = true;
                            npc.velocity.Y -= 1f;

                            npc.ai[0] = 0; //prevent proceeding to next steps of ai while despawning
                        }
                        else
                        {
                            npc.timeLeft = 600;
                        }
                    }
                    break;

                case 1: //acorn sprays
                    if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y))
                        npc.velocity.X = Math.Abs(npc.velocity.Y) * Math.Sign(npc.velocity.X);
                    if (npc.velocity.Y == 0)
                        npc.velocity.X *= 0.99f;
                    npc.noTileCollide = false;
                    npc.noGravity = false;

                    if (++npc.ai[2] > 35)
                    {
                        npc.ai[2] = 0;
                        const float gravity = 0.2f;
                        float time = 60f;
                        Vector2 distance = player.Center - npc.Center;// + player.velocity * 30f;
                        distance.X = distance.X / time;
                        distance.Y = distance.Y / time - 0.5f * gravity * time;
                        for (int i = 0; i < 20; i++)
                        {
                            Projectile.NewProjectile(npc.Center, distance + Main.rand.NextVector2Square(-0.5f, 0.5f) * 3,
                                ModContent.ProjectileType<Acorn>(), npc.damage / 5, 0f, Main.myPlayer);
                        }
                    }
                    
                    if (++npc.ai[1] > 120)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.netUpdate = true;
                        npc.TargetClosest();
                    }
                    break;

                case 2:
                    goto case 0;

                case 3: //snowball barrage
                    if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y))
                        npc.velocity.X = Math.Abs(npc.velocity.Y) * Math.Sign(npc.velocity.X);
                    if (npc.velocity.Y == 0)
                        npc.velocity.X *= 0.99f;
                    npc.noTileCollide = false;
                    npc.noGravity = false;

                    if (++npc.ai[2] > 5)
                    {
                        npc.ai[2] = 0;
                        if (Main.netMode != 1 && npc.ai[1] > 30 && npc.ai[1] < 120)
                        {
                            Vector2 offset;
                            offset.X = Main.rand.NextFloat(0, npc.width / 2) * npc.direction;
                            offset.Y = 16;
                            Projectile.NewProjectile(npc.Center + offset,
                                Vector2.UnitY * -12f, ModContent.ProjectileType<Snowball>(), npc.damage / 4, 0f, Main.myPlayer);
                        }
                    }

                    if (++npc.ai[1] > 150)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.netUpdate = true;
                        npc.TargetClosest();
                    }
                    break;

                case 4:
                    goto case 0;

                case 5: //spray squirrels
                    if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y))
                        npc.velocity.X = Math.Abs(npc.velocity.Y) * Math.Sign(npc.velocity.X);
                    if (npc.velocity.Y == 0)
                        npc.velocity.X *= 0.99f;
                    npc.noTileCollide = false;
                    npc.noGravity = false;

                    if (++npc.ai[2] > 6)
                    {
                        npc.ai[2] = 0;
                        if (Main.netMode != 1)
                        {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<LesserSquirrel>());
                            if (n != Main.maxNPCs)
                            {
                                Main.npc[n].velocity.X = Main.rand.NextFloat(-10, 10);
                                Main.npc[n].velocity.Y = Main.rand.NextFloat(-20, -10);
                                Main.npc[n].netUpdate = true;
                                if (Main.netMode == 2)
                                    NetMessage.SendData(23, -1, -1, null, n);
                            }
                        }
                    }

                    if (++npc.ai[1] > 180)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.netUpdate = true;
                        npc.TargetClosest();
                    }
                    break;

                case 6:
                    goto case 0;

                case 7:
                    goto case 3;

                case 8:
                    goto case 0;

                case 9: //grappling hook
                    if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y))
                        npc.velocity.X = Math.Abs(npc.velocity.Y) * Math.Sign(npc.velocity.X);
                    if (npc.velocity.Y == 0)
                        npc.velocity.X *= 0.99f;
                    npc.noTileCollide = false;
                    npc.noGravity = false;

                    if (npc.ai[2] == 0) //shoot hook
                    {
                        npc.ai[2] = 1;
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(player.Center) * 16, ModContent.ProjectileType<SquirrelHook>(), 0, 0f, Main.myPlayer, npc.whoAmI);
                        }
                    }

                    if (++npc.ai[1] < 240) //charge power
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            int d = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Shadowflame, 0f, 0f, 0, default(Color), 2f);
                            Main.dust[d].noGravity = true;
                            Main.dust[d].velocity *= 8f;
                            d = Dust.NewDust(npc.Center, 0, 0, DustID.Shadowflame, 0f, 0f, 0, default(Color), 3f);
                            Main.dust[d].noGravity = true;
                            Main.dust[d].velocity *= 16f;
                        }

                        for (int i = 0; i < 20; i++)
                        {
                            Vector2 offset = new Vector2();
                            double angle = Main.rand.NextDouble() * 2d * Math.PI;
                            offset.X += (float)(Math.Sin(angle) * 500);
                            offset.Y += (float)(Math.Cos(angle) * 500);
                            Dust dust = Main.dust[Dust.NewDust(npc.Center + offset - new Vector2(4, 4), 0, 0, DustID.Shadowflame, 0, 0, 100, Color.White, 2f)];
                            dust.velocity = npc.velocity;
                            if (Main.rand.Next(3) == 0)
                                dust.velocity += Vector2.Normalize(offset) * -5f;
                            dust.noGravity = true;
                        }
                    }
                    else if (npc.ai[1] == 240) //explode
                    {
                        if (Main.netMode != 1)
                        {
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<ShadowflameBlast>(), npc.damage / 2, 0f, Main.myPlayer);
                        }
                    }
                    else if (npc.ai[1] > 300)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.netUpdate = true;
                        npc.TargetClosest();
                    }
                    break;

                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (++npc.frameCounter > 6)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (npc.frame.Y >= frameHeight * Main.npcFrameCount[npc.type])
                    npc.frame.Y = 0;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Masomode.Guilty>(), 600);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void NPCLoot()
        {
            int[] drops = {
                ModContent.ItemType<WoodEnchant>(),
                ModContent.ItemType<BorealWoodEnchant>(),
                ModContent.ItemType<RichMahoganyEnchant>(),
                ModContent.ItemType<EbonwoodEnchant>(),
                ModContent.ItemType<ShadewoodEnchant>(),
                ModContent.ItemType<PalmWoodEnchant>(),
                ModContent.ItemType<PearlwoodEnchant>()
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
    }
}