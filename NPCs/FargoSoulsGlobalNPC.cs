using System;
using System.Collections.Generic;
using System.Linq;
using FargowiltasSouls.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Buffs.Masomode;
using FargowiltasSouls.Items.Accessories.Masomode;
using FargowiltasSouls.Items.Misc;
using FargowiltasSouls.Items.Weapons.BossDrops;
using FargowiltasSouls.Projectiles.Masomode;
using FargowiltasSouls.NPCs.Critters;
using FargowiltasSouls.Projectiles.Deathrays;
using FargowiltasSouls.Items.Summons;
using FargowiltasSouls.Projectiles.MutantBoss;
using FargowiltasSouls.Projectiles.Souls;
using Fargowiltas.NPCs;
using FargowiltasSouls.Items.Tiles;
using FargowiltasSouls.Items.Patreon;
using Fargowiltas.Items.Tiles;
using FargowiltasSouls.Buffs.Souls;

namespace FargowiltasSouls.NPCs
{
    public class FargoSoulsGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public bool FirstTick = false;
        //debuffs
        public bool SBleed;
        public bool Shock;
        public bool Rotting;
        public bool LeadPoison;
        public bool SqueakyToy;
        public bool SolarFlare;
        public bool TimeFrozen;
        public bool HellFire;
        public bool Infested;
        public int MaxInfestTime;
        public float InfestedDust;
        public bool Needles;
        public bool Electrified;
        public bool CurseoftheMoon;
        public int lightningRodTimer;
        public bool Sadism;
        public bool OceanicMaul;
        public bool MutantNibble;
        public int LifePrevious = -1;
        public bool GodEater;
        public bool Suffocation;
        public bool Villain;
        public bool Lethargic;
        public int LethargicCounter;

        private int valhallaPlayer;
        private int valhallaCounter = 0;
        private int squireCounter = 0;
        public float originalKB;
        public bool SpecialEnchantImmune;

        public static bool Revengeance => CalamityMod.World.CalamityWorld.revenge;

        public override void ResetEffects(NPC npc)
        {
            TimeFrozen = false;
            SBleed = false;
            Shock = false;
            Rotting = false;
            LeadPoison = false;
            SqueakyToy = false;
            SolarFlare = false;
            HellFire = false;
            Infested = false;
            Needles = false;
            Electrified = false;
            CurseoftheMoon = false;
            Sadism = false;
            OceanicMaul = false;
            MutantNibble = false;
            GodEater = false;
            Suffocation = false;
        }

        public override bool PreAI(NPC npc)
        {
            if (TimeFrozen)
            {
                npc.position = npc.oldPosition;
                npc.frameCounter = 0;
                return false;
            }

            if (!FirstTick)
            {
                switch (npc.type)
                {
                    case NPCID.WallofFlesh:
                    case NPCID.WallofFleshEye:
                    case NPCID.MoonLordCore:
                    case NPCID.MoonLordHand:
                    case NPCID.MoonLordHead:
                    case NPCID.MoonLordLeechBlob:
                    case NPCID.TargetDummy:
                    case NPCID.GolemFistLeft:
                    case NPCID.GolemFistRight:
                    case NPCID.GolemHead:
                    case NPCID.DungeonGuardian:
                    case NPCID.DukeFishron:
                        SpecialEnchantImmune = true;
                        break;

                    case NPCID.Squirrel:
                    case NPCID.SquirrelRed:
                        if (Main.rand.Next(8) == 0)
                            npc.Transform(ModContent.NPCType<TophatSquirrel>());
                        break;

                    default:
                        break;
                }

                //critters
                if (Main.player[Main.myPlayer].GetModPlayer<FargoPlayer>().WoodEnchant && npc.damage == 0 && !npc.townNPC && npc.lifeMax == 5)
                {
                    npc.defense = 9999;
                    npc.defDefense = 9999;
                }

                FirstTick = true;
            }

            if (Lethargic && ++LethargicCounter > 3)
            {
                LethargicCounter = 0;
                return false;
            }

            if (valhallaCounter > 0)
            {
                valhallaCounter--;
            }
            
            return true;
        }

        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (valhallaCounter > 900)
            {
                drawColor = Color.SandyBrown;
                return drawColor;
            }

            return null;
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (LeadPoison)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, DustID.Lead, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Dust expr_1CCF_cp_0 = Main.dust[dust];
                    expr_1CCF_cp_0.velocity.Y = expr_1CCF_cp_0.velocity.Y - 0.5f;
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
            }

            if (HellFire)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, DustID.SolarFlare, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(56, Main.LocalPlayer);

                    Dust expr_1CCF_cp_0 = Main.dust[dust];
                    expr_1CCF_cp_0.velocity.Y = expr_1CCF_cp_0.velocity.Y - 0.5f;
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
            }

            /*if (Infested)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, 44, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, Color.LimeGreen, InfestedDust);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Dust expr_1CCF_cp_0 = Main.dust[dust];
                    expr_1CCF_cp_0.velocity.Y = expr_1CCF_cp_0.velocity.Y - 0.5f;
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

                Lighting.AddLight((int)(npc.position.X / 16f), (int)(npc.position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
            }*/

            if (Suffocation)
                drawColor = Colors.RarityPurple;

            if (Villain)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, DustID.AncientLight, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Dust expr_1CCF_cp_0 = Main.dust[dust];
                    expr_1CCF_cp_0.velocity.Y = expr_1CCF_cp_0.velocity.Y - 0.5f;
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

                Lighting.AddLight((int)(npc.position.X / 16f), (int)(npc.position.Y / 16f + 1f), 1f, 0.3f, 0.1f);
            }

            if (Electrified)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, 229, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    if (Main.rand.Next(3) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

                Lighting.AddLight((int)npc.Center.X / 16, (int)npc.Center.Y / 16, 0.3f, 0.8f, 1.1f);
            }

            if (CurseoftheMoon)
            {
                int d = Dust.NewDust(npc.Center, 0, 0, 229, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 3f;
                Main.dust[d].scale += 0.5f;

                if (Main.rand.Next(4) < 3)
                {
                    d = Dust.NewDust(npc.position, npc.width, npc.height, 229, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity.Y -= 1f;
                    Main.dust[d].velocity *= 2f;
                }
            }

            if (Sadism)
            {
                int d = Dust.NewDust(npc.Center, 0, 0, 86, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 3f;
                Main.dust[d].scale += 1f;

                if (Main.rand.Next(4) < 3)
                {
                    d = Dust.NewDust(npc.position, npc.width, npc.height, 86, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity.Y -= 1f;
                    Main.dust[d].velocity *= 2f;
                    Main.dust[d].scale += 0.5f;
                }
            }

            if (GodEater)
            {
                if (Main.rand.Next(7) < 6)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 86, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.2f;
                    Main.dust[dust].velocity.Y -= 0.15f;
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
                Lighting.AddLight(npc.position, 0.15f, 0.03f, 0.09f);
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            Player player = Main.player[Main.myPlayer];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            int dmg;

            //20 dps
            if (SBleed)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 40;
                if (damage < 20)
                {
                    damage = 20;
                }

                if (Main.rand.Next(4) == 0)
                {
                    dmg = 20;
                    int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 40, 0f + Main.rand.Next(-5, 5), -5f, ModContent.ProjectileType<SuperBlood>(), dmg, 0f, Main.myPlayer);
                    if (p < 1000)
                        Main.projectile[p].GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
                }
            }

            if (Rotting)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 100;

                if (damage < 5)
                    damage = 5;
            }

            if (LeadPoison)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                npc.lifeRegen -= npc.type == NPCID.EaterofWorldsBody ? 2 : 10;
            }

            //50 dps
            if (SolarFlare)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                npc.lifeRegen -= 100;

                if (damage < 10)
                {
                    damage = 10;
                }
            }

            //.5% dps
            if (HellFire)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= npc.lifeMax / 100;

                if (damage < npc.lifeMax / 1000)
                    damage = npc.lifeMax / 1000;
            }

            if (Infested)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= InfestedExtraDot(npc);

                if (damage < 8)
                    damage = 8;
            }
            else
            {
                MaxInfestTime = 0;
            }

            if (Electrified)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 4;
                if (npc.velocity != Vector2.Zero)
                    npc.lifeRegen -= 16;

                if (damage < 4)
                    damage = 4;
            }

            if (CurseoftheMoon)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 24;

                if (damage < 6)
                    damage = 6;
            }

            if (OceanicMaul)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 48;

                if (damage < 12)
                    damage = 12;
            }

            if (Sadism)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 170;

                if (damage < 70)
                    damage = 70;
            }

            if (MutantNibble)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                if (npc.lifeRegenCount > 0)
                    npc.lifeRegenCount = 0;

                if (npc.life > LifePrevious)
                    npc.life = LifePrevious;
                else
                    LifePrevious = npc.life;
            }
            else
            {
                LifePrevious = npc.life;
            }

            if (GodEater)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 4200;
                if (damage < 777)
                {
                    damage = 777;
                }
            }

            if (Suffocation)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 40;
            }
        }

        private int InfestedExtraDot(NPC npc)
        {
            int buffIndex = npc.FindBuffIndex(ModContent.BuffType<Infested>());
            if (buffIndex == -1)
                return 0;

            int timeLeft = npc.buffTime[buffIndex];
            if (MaxInfestTime <= 0)
                MaxInfestTime = timeLeft;
            float baseVal = (MaxInfestTime - timeLeft) / 30f; //change the denominator to adjust max power of DOT
            int dmg = (int)(baseVal * baseVal + 8);

            InfestedDust = baseVal / 15 + .5f;
            if (InfestedDust > 5f)
                InfestedDust = 5f;

            return dmg;
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (modPlayer.Bloodthirsty)
            {
                //20x spawn rate
                spawnRate = (int)(spawnRate * 0.05);
                //20x max spawn
                maxSpawns = (int)(maxSpawns * 20f);
            }

            if (modPlayer.SinisterIcon)
            {
                spawnRate /= 2;
                maxSpawns *= 2;
            }

            if (modPlayer.BuilderMode)
            {
                maxSpawns = 0;
            }
        }

        

        private bool firstLoot = true;
        private bool firstIconLoot = true;

        public override bool PreNPCLoot(NPC npc)
        {
            Player player = Main.player[npc.lastInteraction];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (firstIconLoot)
            {
                firstIconLoot = false;

                if (npc.life <= 1000 && !npc.boss && modPlayer.SinisterIconDrops)
                {
                    if (!modPlayer.MasochistSoul)
                        npc.value = 0;

                    npc.NPCLoot();
                }
            }

            return true;
        }

        public override void NPCLoot(NPC npc)
        {
            Player player = Main.player[npc.lastInteraction];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (modPlayer.PlatinumEnchant && !npc.boss && firstLoot)
            {
                bool midas = npc.HasBuff(BuffID.Midas);
                int chance = 10;
                int bonus = 4;

                if (midas)
                {
                    chance /= 2;
                    bonus *= 2;
                }

                if (Main.rand.Next(chance) == 0)
                {
                    firstLoot = false;
                    for (int i = 1; i < bonus; i++)
                    {
                        npc.NPCLoot();
                    }

                    int num1 = 36;
                    for (int index1 = 0; index1 < num1; ++index1)
                    {
                        Vector2 vector2_1 = (Vector2.Normalize(npc.velocity) * new Vector2((float)npc.width / 2f, (float)npc.height) * 0.75f).RotatedBy((double)(index1 - (num1 / 2 - 1)) * 6.28318548202515 / (double)num1, new Vector2()) + npc.Center;
                        Vector2 vector2_2 = vector2_1 - npc.Center;
                        int index2 = Dust.NewDust(vector2_1 + vector2_2, 0, 0, DustID.PlatinumCoin, vector2_2.X * 2f, vector2_2.Y * 2f, 100, new Color(), 1.4f);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].noLight = true;
                        Main.dust[index2].velocity = vector2_2;
                    }
                }
            }

            firstLoot = false;

            if (npc.type == NPCID.Golem && Main.rand.Next(10) == 0)
            {
                Item.NewItem(npc.Hitbox, ModContent.ItemType<ComputationOrb>());
            }

            switch (npc.type) //cracked gem
            {
                case NPCID.BlueSlime:
                    if (npc.netID == NPCID.Pinky && Main.rand.Next(3) == 0)
                        Item.NewItem(npc.Hitbox, ModContent.ItemType<CrackedGem>(), Main.rand.Next(3) + 1);
                    break;

                case NPCID.Nymph:
                case NPCID.DoctorBones:
                case NPCID.DungeonSlime:
                case NPCID.Tim:
                case NPCID.UndeadMiner:
                    if (Main.rand.Next(3) == 0)
                        Item.NewItem(npc.Hitbox, ModContent.ItemType<CrackedGem>(), Main.rand.Next(3) + 1);
                    break;

                default:
                    break;
            }


            //boss drops
            if (Main.rand.Next(FargoSoulsWorld.MasochistMode ? 3 : 10) == 0)
            {
                switch (npc.type)
                {
                    case NPCID.KingSlime:
                        Item.NewItem(npc.Hitbox, ModContent.ItemType<SlimeKingsSlasher>());
                        break;

                    case NPCID.EyeofCthulhu:
                        Item.NewItem(npc.Hitbox, ModContent.ItemType<EyeFlail>());
                        break;

                    case NPCID.EaterofWorldsHead:
                    case NPCID.EaterofWorldsBody:
                    case NPCID.EaterofWorldsTail:
                        bool dropItems = true;
                        for (int i = 0; i < 200; i++)
                        {
                            if (Main.npc[i].active && i != npc.whoAmI && (Main.npc[i].type == 13 || Main.npc[i].type == 14 || Main.npc[i].type == 15))
                            {
                                dropItems = false;
                                break;
                            }
                        }
                        if (dropItems)
                        {
                            Item.NewItem(npc.Hitbox, ModContent.ItemType<EaterStaff>());
                        }
                        break;

                    case NPCID.BrainofCthulhu:
                        Item.NewItem(npc.Hitbox, ModContent.ItemType<BrainStaff>());
                        break;

                    case NPCID.QueenBee:
                        Item.NewItem(npc.Hitbox, ModContent.ItemType<HiveStaff>());
                        break;

                    case NPCID.SkeletronHead:
                        Item.NewItem(npc.Hitbox, ModContent.ItemType<Bonezone>());
                        break;

                    case NPCID.WallofFlesh:
                        Item.NewItem(npc.Hitbox, ModContent.ItemType<FleshHand>());
                        break;

                    case NPCID.TheDestroyer:
                        Item.NewItem(npc.Hitbox, ModContent.ItemType<DestroyerGun>());
                        break;

                    case NPCID.SkeletronPrime:
                        Item.NewItem(npc.Hitbox, ModContent.ItemType<DarkStarCannon>());
                        break;

                    case NPCID.Retinazer:
                        if (!EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.spazBoss, NPCID.Spazmatism))
                        {
                            Item.NewItem(npc.Hitbox, ModContent.ItemType<TwinRangs>());
                        }
                        break;

                    case NPCID.Spazmatism:
                        if (!EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.retiBoss, NPCID.Retinazer))
                        {
                            Item.NewItem(npc.Hitbox, ModContent.ItemType<TwinRangs>());
                        }
                        break;

                    case NPCID.Plantera:
                        Item.NewItem(npc.Hitbox, ModContent.ItemType<Dicer>());
                        break;

                    case NPCID.Golem:
                        Item.NewItem(npc.Hitbox, ModContent.ItemType<GolemTome>());
                        break;

                    case NPCID.DukeFishron:
                        Item.NewItem(npc.Hitbox, ModContent.ItemType<FishStick>());
                        break;
                }
            }

            if (Fargowiltas.Instance.CalamityLoaded && Revengeance && FargoSoulsWorld.MasochistMode && Main.bloodMoon && Main.moonPhase == 0 && Main.raining && Main.rand.Next(10) == 0)
            {
                Mod calamity = ModLoader.GetMod("CalamityMod");

                if (npc.type == calamity.NPCType("DevourerofGodsHeadS"))
                {
                    Item.NewItem(npc.Hitbox, calamity.ItemType("CosmicPlushie"));
                }
            }
        }

        public override bool CheckDead(NPC npc)
        {
            Player player = Main.player[Main.myPlayer];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (TimeFrozen)
            {
                npc.life = 1;
                return false;
            }

            /*if (npc.boss && BossIsAlive(ref mutantBoss, ModContent.NPCType<MutantBoss.MutantBoss>()) && npc.type != ModContent.NPCType<MutantBoss.MutantBoss>())
            {
                npc.active = false;
                Main.PlaySound(npc.DeathSound, npc.Center);
                return false;
            }*/

            if (modPlayer.WoodEnchant && npc.damage == 0 && !npc.townNPC && npc.lifeMax == 5)
            {
                Projectile.NewProjectile(npc.Center, new Vector2(0, -4), ProjectileID.LostSoulFriendly, 20, 0, Main.myPlayer);
            }

            if (Needles && npc.lifeMax > 1 && Main.rand.Next(2) == 0)
            {
                int dmg = 15;
                int numNeedles = 8;

                if (modPlayer.LifeForce)
                {
                    dmg = 50;
                    numNeedles = 16;
                }

                Projectile[] projs = FargoGlobalProjectile.XWay(numNeedles, npc.Center, ProjectileID.PineNeedleFriendly, 5, modPlayer.HighestDamageTypeScaling(dmg), 5f);

                for (int i = 0; i < projs.Length; i++)
                {
                    if (projs[i] == null) continue;
                    Projectile p = projs[i];
                    p.GetGlobalProjectile<FargoGlobalProjectile>().IsRecolor = true;
                    p.magic = false;
                    p.melee = true;
                    p.GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
                }
            }

            

            return true;
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (modPlayer.CactusEnchant)
            {
                Needles = true;
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (modPlayer.CactusEnchant)
                Needles = true;

            //bees ignore defense
            if (modPlayer.BeeEnchant && !modPlayer.TerrariaSoul && projectile.type == ProjectileID.GiantBee)
                damage = (int)(damage + npc.defense * .5);

            if (modPlayer.SpiderEnchant && projectile.minion && Main.rand.Next(101) <= modPlayer.SummonCrit)
            {
                crit = true;
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit)
        {
            FargoPlayer modPlayer = target.GetModPlayer<FargoPlayer>();

            if (target.HasBuff(ModContent.BuffType<ShellHide>()))
                damage *= 2;

            if (SqueakyToy)
            {
                damage = 1;
                modPlayer.Squeak(target.Center);
            }
        }

        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            Player player = Main.player[Main.myPlayer];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (OceanicMaul)
            {
                damage += 15;
                //damage *= 1.3;
            }
            if (CurseoftheMoon)
            {
                damage += 5;
                //damage *= 1.1;
            }
            if (Rotting)
            {
                damage += 5;
            }

            if (modPlayer.KnightEnchant && Villain && !npc.boss)
            {
                damage *= 1.5;
            }

            if (crit && modPlayer.ShroomEnchant && !modPlayer.TerrariaSoul && player.stealth == 0)
            {
                damage *= 1.5;
            }

            if (crit && modPlayer.Graze)
            {
                damage *= 1.0 + modPlayer.GrazeBonus;
            }

            //normal damage calc
            return true;
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (modPlayer.ValhallaEnchant && SoulConfig.Instance.GetValue(SoulConfig.Instance.ValhallaEffect)
                 && valhallaCounter == 0)
            {
                squireCounter += 5;

                if (squireCounter >= 100)
                {
                    valhallaCounter = 1020;
                    valhallaPlayer = player.whoAmI;
                    squireCounter = 0;
                }
            }
            else if (modPlayer.SquireEnchant && SoulConfig.Instance.GetValue(SoulConfig.Instance.SquireKB)
                 && !npc.GetGlobalNPC<FargoSoulsGlobalNPC>().SpecialEnchantImmune && npc.knockBackResist < 1 && !npc.HasBuff(ModContent.BuffType<SquireKBDebuff>()))
            {
                squireCounter += 5;

                if (squireCounter >= 100)
                {
                    originalKB = npc.knockBackResist;
                    npc.knockBackResist = 1f;
                    npc.AddBuff(ModContent.BuffType<SquireKBDebuff>(), 1020);
                    squireCounter = 0;
                }
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            FargoPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<FargoPlayer>();

            if (modPlayer.ValhallaEnchant && SoulConfig.Instance.GetValue(SoulConfig.Instance.ValhallaEffect))
            {
                if (valhallaCounter == 0)
                {
                    squireCounter++;

                    if (squireCounter >= 100)
                    {
                        valhallaCounter = 1020;
                        valhallaPlayer = projectile.owner;
                        squireCounter = 0;
                    }
                }
                else if (valhallaCounter > 900)
                {
                    npc.immune[valhallaPlayer] = 2;
                }
            }
            else if (modPlayer.SquireEnchant && SoulConfig.Instance.GetValue(SoulConfig.Instance.SquireKB)
                 && !npc.GetGlobalNPC<FargoSoulsGlobalNPC>().SpecialEnchantImmune && npc.knockBackResist < 1 && !npc.HasBuff(ModContent.BuffType<SquireKBDebuff>()))
            {
                squireCounter++;

                if (squireCounter >= 100)
                {
                    originalKB = npc.knockBackResist;
                    npc.knockBackResist = 1f;
                    npc.AddBuff(ModContent.BuffType<SquireKBDebuff>(), 1020);
                    squireCounter = 0;
                }
            }
        }

        public override void OnHitNPC(NPC npc, NPC target, int damage, float knockback, bool crit)
        {
            FargoPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<FargoPlayer>();

            if (!modPlayer.ThoriumSoul && modPlayer.KnightEnchant && !npc.friendly && target.townNPC)
            {
                Villain = true;
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Steampunker)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<RoombaPet>());
                shop.item[nextSlot].value = 50000;
                nextSlot++;
            }
        }
    }
}
