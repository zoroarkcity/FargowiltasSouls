using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Capture;
using FargowiltasSouls.NPCs;
using FargowiltasSouls.Projectiles;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Buffs;
using FargowiltasSouls.Buffs.Souls;
using FargowiltasSouls.Buffs.Masomode;
using FargowiltasSouls.Projectiles.Souls;
using FargowiltasSouls.Projectiles.BossWeapons;
using FargowiltasSouls.Projectiles.Masomode;
using FargowiltasSouls.Items.Weapons.SwarmDrops;
using FargowiltasSouls.NPCs.MutantBoss;
using FargowiltasSouls.Buffs.Boss;
using FargowiltasSouls.Buffs;
using FargowiltasSouls.Projectiles.Minions;
using FargowiltasSouls.Items.Summons;

namespace FargowiltasSouls
{
    public partial class FargoPlayer
    {
        public void FlowerBoots()
        {
            if (!SoulConfig.Instance.GetValue(SoulConfig.Instance.ChlorophyteFlowerBoots))
                return;

            int x = (int)player.Center.X / 16;
            int y = (int)(player.position.Y + player.height - 1f) / 16;

            if (Main.tile[x, y] == null)
            {
                Main.tile[x, y] = new Tile();
            }

            if (!Main.tile[x, y].active() && Main.tile[x, y].liquid == 0 && Main.tile[x, y + 1] != null && WorldGen.SolidTile(x, y + 1))
            {
                Main.tile[x, y].frameY = 0;
                Main.tile[x, y].slope(0);
                Main.tile[x, y].halfBrick(false);

                if (Main.tile[x, y + 1].type == 2)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Main.tile[x, y].active(true);
                        Main.tile[x, y].type = 3;
                        Main.tile[x, y].frameX = (short)(18 * Main.rand.Next(6, 11));
                        while (Main.tile[x, y].frameX == 144)
                        {
                            Main.tile[x, y].frameX = (short)(18 * Main.rand.Next(6, 11));
                        }
                    }
                    else
                    {
                        Main.tile[x, y].active(true);
                        Main.tile[x, y].type = 73;
                        Main.tile[x, y].frameX = (short)(18 * Main.rand.Next(6, 21));

                        while (Main.tile[x, y].frameX == 144)
                        {
                            Main.tile[x, y].frameX = (short)(18 * Main.rand.Next(6, 21));
                        }
                    }

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendTileSquare(-1, x, y, 1, TileChangeType.None);
                    }
                }
                else if (Main.tile[x, y + 1].type == 109)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Main.tile[x, y].active(true);
                        Main.tile[x, y].type = 110;
                        Main.tile[x, y].frameX = (short)(18 * Main.rand.Next(4, 7));

                        while (Main.tile[x, y].frameX == 90)
                        {
                            Main.tile[x, y].frameX = (short)(18 * Main.rand.Next(4, 7));
                        }
                    }
                    else
                    {
                        Main.tile[x, y].active(true);
                        Main.tile[x, y].type = 113;
                        Main.tile[x, y].frameX = (short)(18 * Main.rand.Next(2, 8));

                        while (Main.tile[x, y].frameX == 90)
                        {
                            Main.tile[x, y].frameX = (short)(18 * Main.rand.Next(2, 8));
                        }
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendTileSquare(-1, x, y, 1, TileChangeType.None);
                    }
                }
                else if (Main.tile[x, y + 1].type == 60)
                {
                    Main.tile[x, y].active(true);
                    Main.tile[x, y].type = 74;
                    Main.tile[x, y].frameX = (short)(18 * Main.rand.Next(9, 17));

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendTileSquare(-1, x, y, 1, TileChangeType.None);
                    }
                }
            }
        }

        #region enchantments

        public void BeeEffect(bool hideVisual)
        {
            player.strongBees = true;
            //bees ignore defense
            BeeEnchant = true;
            AddPet(SoulConfig.Instance.HornetPet, hideVisual, BuffID.BabyHornet, ProjectileID.BabyHornet);
        }

        public void BeetleEffect()
        {
            if (!SoulConfig.Instance.GetValue(SoulConfig.Instance.BeetleEffect)) return;

            if (player.beetleDefense) //don't let this stack
                return;

            player.beetleDefense = true;
            player.beetleCounter += 1f;
            int num5 = 180;
            int cap = TerrariaSoul ? 3 : 1;
            if (player.beetleCounter >= num5)
            {
                if (player.beetleOrbs > 0 && player.beetleOrbs < cap)
                {
                    for (int k = 0; k < 22; k++)
                    {
                        if (player.buffType[k] >= 95 && player.buffType[k] <= 96)
                        {
                            player.DelBuff(k);
                        }
                    }
                }
                if (player.beetleOrbs < cap)
                {
                    player.AddBuff(95 + player.beetleOrbs, 5, false);
                    player.beetleCounter = 0f;
                }
                else
                {
                    player.beetleCounter = num5;
                }
            }

            if (!player.beetleDefense && !player.beetleOffense)
            {
                player.beetleCounter = 0f;
            }
            else
            {
                player.beetleFrameCounter++;
                if (player.beetleFrameCounter >= 1)
                {
                    player.beetleFrameCounter = 0;
                    player.beetleFrame++;
                    if (player.beetleFrame > 2)
                    {
                        player.beetleFrame = 0;
                    }
                }
                for (int l = player.beetleOrbs; l < 3; l++)
                {
                    player.beetlePos[l].X = 0f;
                    player.beetlePos[l].Y = 0f;
                }
                for (int m = 0; m < player.beetleOrbs; m++)
                {
                    player.beetlePos[m] += player.beetleVel[m];
                    Vector2[] expr_6EcCp0 = player.beetleVel;
                    int expr_6EcCp1 = m;
                    expr_6EcCp0[expr_6EcCp1].X = expr_6EcCp0[expr_6EcCp1].X + Main.rand.Next(-100, 101) * 0.005f;
                    Vector2[] expr71ACp0 = player.beetleVel;
                    int expr71ACp1 = m;
                    expr71ACp0[expr71ACp1].Y = expr71ACp0[expr71ACp1].Y + Main.rand.Next(-100, 101) * 0.005f;
                    float num6 = player.beetlePos[m].X;
                    float num7 = player.beetlePos[m].Y;
                    float num8 = (float)Math.Sqrt(num6 * num6 + num7 * num7);
                    if (num8 > 100f)
                    {
                        num8 = 20f / num8;
                        num6 *= -num8;
                        num7 *= -num8;
                        int num9 = 10;
                        player.beetleVel[m].X = (player.beetleVel[m].X * (num9 - 1) + num6) / num9;
                        player.beetleVel[m].Y = (player.beetleVel[m].Y * (num9 - 1) + num7) / num9;
                    }
                    else if (num8 > 30f)
                    {
                        num8 = 10f / num8;
                        num6 *= -num8;
                        num7 *= -num8;
                        int num10 = 20;
                        player.beetleVel[m].X = (player.beetleVel[m].X * (num10 - 1) + num6) / num10;
                        player.beetleVel[m].Y = (player.beetleVel[m].Y * (num10 - 1) + num7) / num10;
                    }
                    num6 = player.beetleVel[m].X;
                    num7 = player.beetleVel[m].Y;
                    num8 = (float)Math.Sqrt(num6 * num6 + num7 * num7);
                    if (num8 > 2f)
                    {
                        player.beetleVel[m] *= 0.9f;
                    }
                    player.beetlePos[m] -= player.velocity * 0.25f;
                }
            }
        }

        public void CactusEffect()
        {
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.CactusNeedles))
            {
                CactusEnchant = true;
            }
        }

        public void ChloroEffect(bool hideVisual, int dmg)
        {
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.ChlorophyteCrystals) && player.ownedProjectileCounts[ModContent.ProjectileType<Chlorofuck>()] == 0)
            {
                const int max = 5;
                float rotation = 2f * (float)Math.PI / max;

                for (int i = 0; i < max; i++)
                {
                    Vector2 spawnPos = player.Center + new Vector2(60, 0f).RotatedBy(rotation * i);
                    int p = Projectile.NewProjectile(spawnPos, Vector2.Zero, ModContent.ProjectileType<Chlorofuck>(), dmg, 10f, player.whoAmI, 0, rotation * i);
                    Main.projectile[p].GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
                }
            }

            AddPet(SoulConfig.Instance.SeedlingPet, hideVisual, BuffID.PetSapling, ProjectileID.Sapling);
        }

        public void CopperEffect(NPC target)
        {
            int dmg = 20;
            int chance = 20;

            if (target.FindBuffIndex(BuffID.Wet) != -1 || target.wet)
            {
                dmg *= 3;
                chance /= 5;
            }

            if (TerraForce)
            {
                dmg *= 2;
            }

            if (Main.rand.Next(chance) == 0)
            {
                float closestDist = 500f;
                NPC closestNPC;

                for (int i = 0; i < 5; i++)
                {
                    closestNPC = null;

                    for (int j = 0; j < 200; j++)
                    {
                        NPC npc = Main.npc[j];
                        if (npc.active && npc != target && !npc.HasBuff(ModContent.BuffType<Shock>()) && npc.Distance(target.Center) < closestDist && npc.Distance(target.Center) >= 50)
                        {
                            closestNPC = npc;
                            break;
                        }
                    }

                    if (closestNPC != null)
                    {
                        Vector2 ai = closestNPC.Center - target.Center;
                        float ai2 = Main.rand.Next(100);
                        Vector2 velocity = Vector2.Normalize(ai) * 20;

                        Projectile p = FargoGlobalProjectile.NewProjectileDirectSafe(target.Center, velocity, ModContent.ProjectileType<LightningArc>(), HighestDamageTypeScaling(dmg), 0f, player.whoAmI, ai.ToRotation(), ai2);
                        target.AddBuff(ModContent.BuffType<Shock>(), 60);
                    }
                    else
                    {
                        break;
                    }

                    target = closestNPC;
                }

                copperCD = 300;
            }
        }

        public void CrimsonEffect(bool hideVisual)
        {
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.CrimsonRegen))
            {
                player.crimsonRegen = true;
            }

            CrimsonEnchant = true;
            AddPet(SoulConfig.Instance.FaceMonsterPet, hideVisual, BuffID.BabyFaceMonster, ProjectileID.BabyFaceMonster);
            AddPet(SoulConfig.Instance.CrimsonHeartPet, hideVisual, BuffID.CrimsonHeart, ProjectileID.CrimsonHeart);
        }

        public void DarkArtistEffect(bool hideVisual)
        {
            player.setApprenticeT3 = true;
            DarkEnchant = true;

            //spawn tower boi
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[ModContent.ProjectileType<FlameburstMinion>()] < 1)
                Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<FlameburstMinion>(), 0, 0f, player.whoAmI);



            AddPet(SoulConfig.Instance.FlickerwickPet, hideVisual, BuffID.PetDD2Ghost, ProjectileID.DD2PetGhost);
        }

        public void ForbiddenEffect()
        {
            if (!SoulConfig.Instance.GetValue(SoulConfig.Instance.ForbiddenStorm)) return;

            player.setForbidden = true;
            player.UpdateForbiddenSetLock();
            Lighting.AddLight(player.Center, 0.8f, 0.7f, 0.2f);
            //storm boosted
            ForbiddenEnchant = true;
        }

        public void FossilEffect(int dmg, bool hideVisual)
        {
            FossilEnchant = true;

            //bone zone
            if (FossilBones)
            {
                if (boneCD <= 0 && !player.dead)
                {
                    for (int i = 0; i < Main.rand.Next(4, 12); i++)
                    {
                        float randX, randY;

                        do
                        {
                            randX = Main.rand.Next(-10, 10);
                        } while (randX <= 4f && randX >= -4f);

                        do
                        {
                            randY = Main.rand.Next(-10, 10);
                        } while (randY <= 4f && randY >= -4f);

                        Projectile p = FargoGlobalProjectile.NewProjectileDirectSafe(player.Center, new Vector2(randX, randY), ProjectileID.BoneGloveProj, HighestDamageTypeScaling(dmg), 2, Main.myPlayer);
                        if (p != null)
                            p.GetGlobalProjectile<FargoGlobalProjectile>().IsRecolor = true;
                    }

                    Projectile p2 = FargoGlobalProjectile.NewProjectileDirectSafe(player.Center, Vector2.Zero, ProjectileID.Bone, HighestDamageTypeScaling((int)(dmg * 1.5f)), 0f, player.whoAmI);
                    if (p2 != null)
                    {
                        p2.GetGlobalProjectile<FargoGlobalProjectile>().Rotate = true;
                        p2.GetGlobalProjectile<FargoGlobalProjectile>().RotateDist = Main.rand.Next(32, 128);
                        p2.GetGlobalProjectile<FargoGlobalProjectile>().RotateDir = Main.rand.Next(2);
                        p2.GetGlobalProjectile<FargoGlobalProjectile>().IsRecolor = true;
                        p2.noDropItem = true;
                    }

                    boneCD = 20;
                }
                else
                {
                    boneCD--;
                }

                if (!player.immune)
                    FossilBones = false;
            }

            AddPet(SoulConfig.Instance.DinoPet, hideVisual, BuffID.BabyDinosaur, ProjectileID.BabyDino);
        }

        public void FrostEffect(int dmg, bool hideVisual)
        {
            FrostEnchant = true;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.FrostIcicles))
            {
                if (icicleCD == 0 && IcicleCount < 3 && player.ownedProjectileCounts[ModContent.ProjectileType<FrostIcicle>()] < 3)
                {
                    Projectile p = FargoGlobalProjectile.NewProjectileDirectSafe(player.Center, Vector2.Zero, ModContent.ProjectileType<FrostIcicle>(), 0, 0, player.whoAmI, 2.5f);

                    if (p != null)
                    {
                        icicles[IcicleCount] = p;
                        IcicleCount++;
                    }
                    icicleCD = 30;
                }

                if (icicleCD != 0)
                {
                    icicleCD--;
                }

                if (IcicleCount == 3 && player.controlUseItem && player.HeldItem.damage > 0)
                {
                    for (int i = 0; i < icicles.Length; i++)
                    {
                        Vector2 vel = (Main.MouseWorld - icicles[i].Center).SafeNormalize(-Vector2.UnitY) * 5;

                        int p = Projectile.NewProjectile(icicles[i].Center, vel, ProjectileID.Blizzard, dmg, 1f, player.whoAmI);
                        icicles[i].Kill();

                        Main.projectile[p].GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
                    }

                    IcicleCount = 0;
                    icicleCD = 120;
                }
            }

            AddPet(SoulConfig.Instance.SnowmanPet, hideVisual, BuffID.BabySnowman, ProjectileID.BabySnowman);
            AddPet(SoulConfig.Instance.PenguinPet, hideVisual, BuffID.BabyPenguin, ProjectileID.Penguin);
        }

        public void GladiatorEffect(bool hideVisual)
        {
            GladEnchant = true;

            if (gladCount > 0)
            {
                gladCount--;
            }


            AddPet(SoulConfig.Instance.MinotaurPet, hideVisual, BuffID.MiniMinotaur, ProjectileID.MiniMinotaur);
        }

        public void GoldEffect(bool hideVisual)
        {
            //gold ring
            player.goldRing = true;
            //lucky coin
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.LuckyCoin))
                player.coins = true;
            //discount card
            player.discount = true;
            //midas
            GoldEnchant = true;

            AddPet(SoulConfig.Instance.ParrotPet, hideVisual, BuffID.PetParrot, ProjectileID.Parrot);
        }

        public void HallowEffect(bool hideVisual, int dmg)
        {
            HallowEnchant = true;
            AddMinion(SoulConfig.Instance.HallowSword, ModContent.ProjectileType<HallowSword>(), (int)(dmg * player.minionDamage), 0f);

            //reflect proj
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.HallowShield) && !noDodge && !player.HasBuff(mod.BuffType("HallowCooldown")))
            {
                const int focusRadius = 50;

                //if (Math.Abs(player.velocity.X) < .5f && Math.Abs(player.velocity.Y) < .5f)
                for (int i = 0; i < 20; i++)
                {
                    Vector2 offset = new Vector2();
                    double angle = Main.rand.NextDouble() * 2d * Math.PI;
                    offset.X += (float)(Math.Sin(angle) * focusRadius);
                    offset.Y += (float)(Math.Cos(angle) * focusRadius);
                    Dust dust = Main.dust[Dust.NewDust(
                        player.Center + offset - new Vector2(4, 4), 0, 0,
                        DustID.GoldFlame, 0, 0, 100, Color.White, 0.5f
                        )];
                    dust.velocity = player.velocity;
                    dust.noGravity = true;
                }

                Main.projectile.Where(x => x.active && x.hostile).ToList().ForEach(x =>
                {
                    if (Vector2.Distance(x.Center, player.Center) <= focusRadius + Math.Min(x.width, x.height) / 2)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            int dustId = Dust.NewDust(new Vector2(x.position.X, x.position.Y + 2f), x.width, x.height + 5, DustID.GoldFlame, x.velocity.X * 0.2f, x.velocity.Y * 0.2f, 100, default(Color), 3f);
                            Main.dust[dustId].noGravity = true;
                        }

                        // Set ownership
                        x.hostile = false;
                        x.friendly = true;
                        x.owner = player.whoAmI;

                        // Turn around
                        x.velocity *= -1f;

                        // Flip sprite
                        if (x.Center.X > player.Center.X)
                        {
                            x.direction = 1;
                            x.spriteDirection = 1;
                        }
                        else
                        {
                            x.direction = -1;
                            x.spriteDirection = -1;
                        }

                        // Don't know if this will help but here it is
                        x.netUpdate = true;

                        player.AddBuff(mod.BuffType("HallowCooldown"), 600);
                    }
                });
            }

            AddPet(SoulConfig.Instance.FairyPet, hideVisual, BuffID.FairyBlue, ProjectileID.BlueFairy);
        }

        private int internalTimer = 0;
        private bool wasHoldingShield = false;

        public void IronEffect()
        {
            //no need when player has brand of inferno
            if (player.inventory[player.selectedItem].type == ItemID.DD2SquireDemonSword)
            {
                internalTimer = 0;
                wasHoldingShield = false;
                return;
            }

            player.shieldRaised = player.selectedItem != 58 && player.controlUseTile && !player.tileInteractionHappened && player.releaseUseItem 
                && !player.controlUseItem && !player.mouseInterface && !CaptureManager.Instance.Active && !Main.HoveringOverAnNPC 
                && !Main.SmartInteractShowingGenuine && !player.mount.Active && 
                player.itemAnimation == 0 && player.itemTime == 0 && PlayerInput.Triggers.Current.MouseRight;

            if (internalTimer > 0)
            {
                internalTimer++;
                player.shieldParryTimeLeft = internalTimer;
                if (player.shieldParryTimeLeft > 30)
                {
                    player.shieldParryTimeLeft = 0;
                    internalTimer = 0;
                }
            }

            if (player.shieldRaised)
            {
                IronGuard = true;

                for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
                {
                    if (player.shield == -1 && player.armor[i].shieldSlot != -1)
                        player.shield = player.armor[i].shieldSlot;
                }

                if (!wasHoldingShield)
                {
                    wasHoldingShield = true;

                    if (player.shield_parry_cooldown == 0)
                    {
                        internalTimer = 1;
                    }

                    player.itemAnimation = 0;
                    player.itemTime = 0;
                    player.reuseDelay = 0;
                }
            }
            else if (wasHoldingShield)
            {
                wasHoldingShield = false;
                player.shield_parry_cooldown = 120;
                player.shieldParryTimeLeft = 0;
                internalTimer = 0;
            }
        }

        public void JungleEffect()
        {
            JungleEnchant = true;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.JungleSpores) && player.jump > 0 && jungleCD == 0)
            {
                int dmg = NatureForce ? 50 : 15;
                Main.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 62, 0.5f);
                FargoGlobalProjectile.XWay(10, player.Center, ModContent.ProjectileType<SporeBoom>(), 3f, HighestDamageTypeScaling(dmg), 0f);
                jungleCD = 30;
            }

            if (jungleCD != 0)
            {
                jungleCD--;
            }

            if (NatureForce) return;

            player.cordage = true;
        }

        public void MeteorEffect(int damage)
        {
            MeteorEnchant = true;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.MeteorShower))
            {
                if (meteorShower)
                {
                    if (meteorTimer % 2 == 0)
                    {
                        int p = Projectile.NewProjectile(player.Center.X + Main.rand.Next(-1000, 1000), player.Center.Y - 1000, Main.rand.Next(-2, 2), 0f + Main.rand.Next(8, 12), Main.rand.Next(424, 427), (int)(damage * player.magicDamage), 0f, player.whoAmI, 0f, 0.5f + (float)Main.rand.NextDouble() * 0.3f);

                        Main.projectile[p].GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
                    }

                    meteorTimer--;

                    if (meteorTimer <= 0)
                    {
                        meteorCD = 300;
                        meteorTimer = 150;
                        meteorShower = false;
                    }
                }
                else
                {
                    if (player.controlUseItem)
                    {
                        meteorCD--;

                        if (meteorCD == 0)
                        {
                            meteorShower = true;
                        }
                    }
                    else
                    {
                        meteorCD = 300;
                    }
                }
            }
        }

        public void MinerEffect(bool hideVisual, float pickSpeed)
        {
            player.pickSpeed -= pickSpeed;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.MinerSpelunker))
            {
                player.findTreasure = true;
            }

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.MinerHunter))
            {
                player.detectCreature = true;
            }

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.MinerDanger))
            {
                player.dangerSense = true;
            }

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.MinerShine))
            {
                Lighting.AddLight(player.Center, 0.8f, 0.8f, 0f);
            }

            MinerEnchant = true;

            AddPet(SoulConfig.Instance.MagicLanternPet, hideVisual, BuffID.MagicLantern, ProjectileID.MagicLantern);
        }

        public void MoltenEffect(int dmg)
        {
            MoltenEnchant = true;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.MoltenInferno))
            {
                player.inferno = true;
                Lighting.AddLight((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f), 0.65f, 0.4f, 0.1f);
                int buff = BuffID.OnFire;
                float distance = 200f;
                bool doDmg = player.infernoCounter % 60 == 0;
                int damage = (int)(dmg * player.meleeDamage);

                if (player.whoAmI == Main.myPlayer)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active && !npc.friendly && npc.damage > 0 && !npc.dontTakeDamage)
                        {
                            if (Vector2.Distance(player.Center, npc.Center) <= distance)
                            {
                                if (npc.FindBuffIndex(buff) == -1)
                                {
                                    npc.AddBuff(buff, 120);
                                }

                                if (Vector2.Distance(player.Center, npc.Center) <= distance / 4)
                                {
                                    damage *= 4;
                                }
                                else if (Vector2.Distance(player.Center, npc.Center) <= distance / 2)
                                {
                                    damage *= 2;
                                }

                                if (doDmg)
                                {
                                    player.ApplyDamageToNPC(npc, damage, 0f, 0, false);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void NebulaEffect()
        {
            if (!SoulConfig.Instance.GetValue(SoulConfig.Instance.NebulaBoost, false)) return;

            Nebula = true;
        }

        public void NecroEffect(bool hideVisual)
        {
            NecroEnchant = true;

            if (necroCD != 0)
                necroCD--;

            AddPet(SoulConfig.Instance.DGPet, hideVisual, BuffID.BabySkeletronHead, ProjectileID.BabySkeletronHead);
        }

        public void NinjaEffect(bool hideVisual)
        {
            if (player.controlUseItem && player.HeldItem.type == ItemID.RodofDiscord)
            {
                player.AddBuff(ModContent.BuffType<FirstStrike>(), 60);
            }

            NinjaEnchant = true;
            AddPet(SoulConfig.Instance.BlackCatPet, hideVisual, BuffID.BlackCat, ProjectileID.BlackCat);
        }

        public void ObsidianEffect()
        {
            player.buffImmune[BuffID.OnFire] = true;
            player.fireWalk = true;

            if (TerraForce)
            {
                player.lavaImmune = true;
            }
            else
            {
                player.lavaMax += 300;
            }

            player.noFallDmg = true;

            //in lava effects
            if (player.lavaWet && !TerrariaSoul)
            {
                player.armorPenetration += 20;
                AttackSpeed += .15f;
                ObsidianEnchant = true;
            }
        }

        public void OrichalcumEffect()
        {
            if (!SoulConfig.Instance.GetValue(SoulConfig.Instance.OrichalcumFire)) return;

            player.onHitPetal = true;

            OriEnchant = true;

            int ballAmt = 6;

            if (Eternity)
                ballAmt = 30;

            if (!OriSpawn && player.ownedProjectileCounts[ModContent.ProjectileType<OriFireball>()] < ballAmt)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    for (int i = 0; i < ballAmt; i++)
                    {
                        float degree = (360 / ballAmt) * i;
                        Projectile fireball = FargoGlobalProjectile.NewProjectileDirectSafe(player.Center, Vector2.Zero, ModContent.ProjectileType<OriFireball>(), HighestDamageTypeScaling(25), 0f, player.whoAmI, 5, degree);
                    }
                }

                OriSpawn = true;
            }
        }

        public void PalladiumEffect()
        {
            //no lifesteal needed here for SoE
            if (Eternity) return;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.PalladiumHeal))
            {
                player.onHitRegen = true;
                PalladEnchant = true;

                if (EarthForce && palladiumCD > 0)
                    palladiumCD--;
            }
        }

        public void PumpkinEffect(int dmg, bool hideVisual)
        {
            //pumpkin pies
            PumpkinEnchant = true;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.PumpkinFire) && (player.controlLeft || player.controlRight) && !IsStandingStill)
            {
                if (pumpkinCD <= 0)
                {
                    int p = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileID.MolotovFire, HighestDamageTypeScaling(dmg), 1f, player.whoAmI);

                    Main.projectile[p].GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
                    pumpkinCD = 20;
                }

                pumpkinCD--;
            }

            AddPet(SoulConfig.Instance.SquashlingPet, hideVisual, BuffID.Squashling, ProjectileID.Squashling);
        }

        public void RedRidingEffect(bool hideVisual)
        {
            RedEnchant = true;

            //celestial shell
            player.accMerman = true;
            player.wolfAcc = true;

            if (hideVisual)
            {
                player.hideMerman = true;
                player.hideWolf = true;
            }

            player.setHuntressT3 = true;
            AddPet(SoulConfig.Instance.PuppyPet, hideVisual, BuffID.Puppy, ProjectileID.Puppy);
        }

        public void ShadowEffect(bool hideVisual)
        {
            ShadowEnchant = true;
            AddPet(SoulConfig.Instance.EaterPet, hideVisual, BuffID.BabyEater, ProjectileID.BabyEater);
            AddPet(SoulConfig.Instance.ShadowOrbPet, hideVisual, BuffID.ShadowOrb, ProjectileID.ShadowOrb);
        }

        public void ShinobiEffect(bool hideVisual)
        {
            player.setMonkT3 = true;
            //tele through wall until open space on dash into wall
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.ShinobiWalls) && player.dashDelay == -1 && player.mount.Type == -1 && player.velocity.X == 0)
            {
                var teleportPos = new Vector2();
                int direction = player.direction;

                teleportPos.X = player.position.X + direction;
                teleportPos.Y = player.position.Y;

                while (Collision.SolidCollision(teleportPos, player.width, player.height))
                {
                    if (direction == 1)
                    {
                        teleportPos.X++;
                    }
                    else
                    {
                        teleportPos.X--;
                    }
                }
                if (teleportPos.X > 50 && teleportPos.X < (double)(Main.maxTilesX * 16 - 50) && teleportPos.Y > 50 && teleportPos.Y < (double)(Main.maxTilesY * 16 - 50))
                {
                    player.Teleport(teleportPos, 1);
                    NetMessage.SendData(65, -1, -1, null, 0, player.whoAmI, teleportPos.X, teleportPos.Y, 1);
                }
            }

            ShinobiEnchant = true;
            AddPet(SoulConfig.Instance.GatoPet, hideVisual, BuffID.PetDD2Gato, ProjectileID.DD2PetGato);
        }

        public void ShroomiteEffect(bool hideVisual)
        {
            if (!TerrariaSoul && SoulConfig.Instance.GetValue(SoulConfig.Instance.ShroomiteStealth))
                player.shroomiteStealth = true;

            ShroomEnchant = true;
            AddPet(SoulConfig.Instance.TrufflePet, hideVisual, BuffID.BabyTruffle, ProjectileID.Truffle);
        }

        public void SolarEffect()
        {
            if (!SoulConfig.Instance.GetValue(SoulConfig.Instance.SolarShield)) return;

            Solar = true;
        }

        public void SpectreEffect(bool hideVisual)
        {
            SpectreEnchant = true;
            AddPet(SoulConfig.Instance.WispPet, hideVisual, BuffID.Wisp, ProjectileID.Wisp);
        }

        public void SpectreHeal(NPC npc, Projectile proj)
        {
            if (npc.canGhostHeal && !player.moonLeech)
            {
                float num = 0.2f;
                num -= proj.numHits * 0.05f;
                if (num <= 0f)
                {
                    return;
                }
                float num2 = proj.damage * num;
                if ((int)num2 <= 0)
                {
                    return;
                }
                if (Main.player[Main.myPlayer].lifeSteal <= 0f)
                {
                    return;
                }
                Main.player[Main.myPlayer].lifeSteal -= num2 * 5; //original damage

                float num3 = 0f;
                int num4 = proj.owner;
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && !Main.player[i].dead && ((!Main.player[proj.owner].hostile && !Main.player[i].hostile) || Main.player[proj.owner].team == Main.player[i].team))
                    {
                        float num5 = Math.Abs(Main.player[i].position.X + (Main.player[i].width / 2) - proj.position.X + (proj.width / 2)) + Math.Abs(Main.player[i].position.Y + (Main.player[i].height / 2) - proj.position.Y + (proj.height / 2));
                        if (num5 < 1200f && (Main.player[i].statLifeMax2 - Main.player[i].statLife) > num3)
                        {
                            num3 = (Main.player[i].statLifeMax2 - Main.player[i].statLife);
                            num4 = i;
                        }
                    }
                }
                Projectile.NewProjectile(proj.position.X, proj.position.Y, 0f, 0f, ProjectileID.SpiritHeal, 0, 0f, proj.owner, num4, num2);
            }
        }

        public void SpectreHurt(Projectile proj)
        {
            int num = proj.damage / 2;
            if (proj.damage / 2 <= 1)
            {
                return;
            }
            int num2 = 1000;
            if (Main.player[Main.myPlayer].ghostDmg > (float)num2)
            {
                return;
            }
            Main.player[Main.myPlayer].ghostDmg += (float)num;
            int[] array = new int[200];
            int num3 = 0;
            int num4 = 0;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].CanBeChasedBy(this, false))
                {
                    float num5 = Math.Abs(Main.npc[i].position.X + (Main.npc[i].width / 2) - proj.position.X + (proj.width / 2)) + Math.Abs(Main.npc[i].position.Y + (Main.npc[i].height / 2) - proj.position.Y + (proj.height / 2));
                    if (num5 < 800f)
                    {
                        if (Collision.CanHit(proj.position, 1, 1, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height) && num5 > 50f)
                        {
                            array[num4] = i;
                            num4++;
                        }
                        else if (num4 == 0)
                        {
                            array[num3] = i;
                            num3++;
                        }
                    }
                }
            }
            if (num3 == 0 && num4 == 0)
            {
                return;
            }
            int num6;
            if (num4 > 0)
            {
                num6 = array[Main.rand.Next(num4)];
            }
            else
            {
                num6 = array[Main.rand.Next(num3)];
            }
            float num7 = 4f;
            float num8 = Main.rand.Next(-100, 101);
            float num9 = Main.rand.Next(-100, 101);
            float num10 = (float)Math.Sqrt((double)(num8 * num8 + num9 * num9));
            num10 = num7 / num10;
            num8 *= num10;
            num9 *= num10;
            Projectile.NewProjectile(proj.position.X, proj.position.Y, num8, num9, ProjectileID.SpectreWrath, num, 0f, proj.owner, (float)num6, 0f);
        }

        public void SpiderEffect(bool hideVisual)
        {
            //minion crits
            SpiderEnchant = true;

            if (!TinEnchant)
            {
                SummonCrit = 20;
            }

            AddPet(SoulConfig.Instance.SpiderPet, hideVisual, BuffID.PetSpider, ProjectileID.Spider);
        }

        public void SpookyEffect(bool hideVisual)
        {
            //scythe doom
            SpookyEnchant = true;
            AddPet(SoulConfig.Instance.CursedSaplingPet, hideVisual, BuffID.CursedSapling, ProjectileID.CursedSapling);
            AddPet(SoulConfig.Instance.EyeSpringPet, hideVisual, BuffID.EyeballSpring, ProjectileID.EyeSpring);
        }

        public void StardustEffect()
        {
            StardustEnchant = true;
            AddPet(SoulConfig.Instance.StardustGuardian, false, BuffID.StardustGuardianMinion, ProjectileID.StardustGuardian);
            player.setStardust = true;

            if (FreezeTime && freezeLength != 0)
            {
                if (!Filters.Scene["FargowiltasSouls:TimeStop"].IsActive())
                    Filters.Scene.Activate("FargowiltasSouls:TimeStop");

                if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.mutantBoss, ModContent.NPCType<MutantBoss>()))
                    player.AddBuff(ModContent.BuffType<TimeFrozen>(), freezeLength);

                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.HasBuff(ModContent.BuffType<TimeFrozen>()))
                        npc.AddBuff(ModContent.BuffType<TimeFrozen>(), freezeLength);
                }

                for (int i = 0; i < 1000; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.active && !p.GetGlobalProjectile<FargoGlobalProjectile>().TimeFreezeImmune && p.GetGlobalProjectile<FargoGlobalProjectile>().TimeFrozen == 0)
                        p.GetGlobalProjectile<FargoGlobalProjectile>().TimeFrozen = freezeLength;
                }

                freezeLength--;

                if (freezeLength == 0)
                {
                    FreezeTime = false;
                    freezeLength = 300;

                    for (int i = 0; i < 200; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active && npc.life == 1)
                            npc.StrikeNPC(9999, 0f, 0);
                    }
                }
            }

            if (FreezeCD != 0 && !FreezeTime)
            {
                FreezeCD--;

                if (FreezeCD == 0)
                    Main.PlaySound(SoundID.MaxMana, player.Center);
            }
        }

        public void TikiEffect(bool hideVisual)
        {
            TikiEnchant = true;
            AddPet(SoulConfig.Instance.TikiPet, hideVisual, BuffID.TikiSpirit, ProjectileID.TikiSpirit);
        }

        public void TinEffect()
        {
            if (!SoulConfig.Instance.GetValue(SoulConfig.Instance.TinCrit, false)) return;

            TinEnchant = true;
            AllCritEquals(TinCrit);

            if (Eternity)
            {
                if (eternityDamage > 20000)
                    eternityDamage = 20000;
                AllDamageUp(eternityDamage);
                player.statDefense += (int)(eternityDamage * 100); //10 defense per .1 damage
            }
        }

        public void TitaniumEffect()
        {
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.TitaniumDodge))
            {
                player.onHitDodge = true;
            }
        }

        public void TurtleEffect(bool hideVisual)
        {
            TurtleEnchant = true;
            AddPet(SoulConfig.Instance.TurtlePet, hideVisual, BuffID.PetTurtle, ProjectileID.Turtle);
            AddPet(SoulConfig.Instance.LizardPet, hideVisual, BuffID.PetLizard, ProjectileID.PetLizard);

            if (!TerrariaSoul && SoulConfig.Instance.GetValue(SoulConfig.Instance.TurtleShell) && IsStandingStill && !player.controlUseItem && !noDodge)
                player.AddBuff(ModContent.BuffType<ShellHide>(), 2);
        }

        public void ValhallaEffect(bool hideVisual)
        {
            player.shinyStone = true;
            player.setSquireT2 = true;
            if (!SoulConfig.Instance.GetValue(SoulConfig.Instance.SquirePanic))
                player.buffImmune[BuffID.BallistaPanic] = true;
            player.setSquireT3 = true;
            //immune frames
            ValhallaEnchant = true;
            AddPet(SoulConfig.Instance.DragonPet, hideVisual, BuffID.PetDD2Dragon, ProjectileID.DD2PetDragon);
        }

        public void VortexEffect(bool hideVisual)
        {
            //portal spawn
            VortexEnchant = true;
            //stealth memes
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.VortexStealth) && (player.controlDown && player.releaseDown))
            {
                if (player.doubleTapCardinalTimer[0] > 0 && player.doubleTapCardinalTimer[0] != 15)
                {
                    VortexStealth = !VortexStealth;
                    if (SoulConfig.Instance.GetValue(SoulConfig.Instance.VortexVoid) && vortexCD == 0 && VortexStealth)
                    {
                        int p = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<Projectiles.Void>(), 60, 5f, player.whoAmI);

                        Main.projectile[p].GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
                        vortexCD = 1200;
                    }
                }
            }

            if (vortexCD != 0)
                vortexCD--;

            if (player.mount.Active)
                VortexStealth = false;

            if (VortexStealth)
            {
                player.moveSpeed *= 0.3f;
                player.aggro -= 1200;
                player.setVortex = true;
                player.stealth = 0f;
            }

            AddPet(SoulConfig.Instance.CompanionCubePet, hideVisual, BuffID.CompanionCube, ProjectileID.CompanionCube);
        }

        public void EbonEffect()
        {
            if (!SoulConfig.Instance.GetValue(SoulConfig.Instance.EbonwoodAura))
                return;

            int dist = 250;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 1 && npc.Distance(player.Center) < dist)
                    npc.AddBuff(BuffID.ShadowFlame, 120);
            }

            for (int i = 0; i < 20; i++)
            {
                Vector2 offset = new Vector2();
                double angle = Main.rand.NextDouble() * 2d * Math.PI;
                offset.X += (float)(Math.Sin(angle) * dist);
                offset.Y += (float)(Math.Cos(angle) * dist);
                Dust dust = Main.dust[Dust.NewDust(
                    player.Center + offset - new Vector2(4, 4), 0, 0,
                    DustID.Shadowflame, 0, 0, 100, Color.White, 1f
                    )];
                dust.velocity = player.velocity;
                if (Main.rand.Next(3) == 0)
                    dust.velocity += Vector2.Normalize(offset) * -5f;
                dust.noGravity = true;
            }
        }

        public void PalmEffect()
        {
            PalmEnchant = true;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.PalmwoodSentry) && (player.controlDown && player.releaseDown))
            {
                if (player.doubleTapCardinalTimer[0] > 0 && player.doubleTapCardinalTimer[0] != 15)
                {
                    Vector2 mouse = Main.MouseWorld;

                    if (player.ownedProjectileCounts[ModContent.ProjectileType<PalmTreeSentry>()] > 0)
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            Projectile proj = Main.projectile[i];

                            if (proj.type == ModContent.ProjectileType<PalmTreeSentry>())
                            {
                                proj.Kill();
                            }
                        }
                    }

                    Projectile.NewProjectile(mouse.X, mouse.Y - 10, 0f, 0f, ModContent.ProjectileType<PalmTreeSentry>(), WoodForce ? 45 : 15, 0f, player.whoAmI);
                }
            }
        }

        public void ApprenticeEffect()
        {
            player.setApprenticeT2 = true;

            //shadow shoot meme
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.ApprenticeEffect))
            {
                Item heldItem = player.HeldItem;

                if (apprenticeCD == 0 && heldItem.shoot > 0 && heldItem.damage > 0 && player.controlUseItem && prevPosition != null)
                {
                    if (prevPosition != null)
                    {
                        Vector2 vel = (Main.MouseWorld - prevPosition).SafeNormalize(-Vector2.UnitY);

                        Projectile.NewProjectile(prevPosition, vel * heldItem.shootSpeed, ProjectileID.DD2FlameBurstTowerT3Shot, heldItem.damage / 2, 1, player.whoAmI);

                        for (int i = 0; i < 5; i++)
                        {
                            int dustId = Dust.NewDust(new Vector2(prevPosition.X, prevPosition.Y + 2f), player.width, player.height + 5, DustID.Shadowflame, 0, 0, 100, Color.Black, 2f);
                            Main.dust[dustId].noGravity = true;
                        }
                    }

                    prevPosition = player.position;
                    apprenticeCD = 20;
                }

                if (apprenticeCD > 0)
                {
                    apprenticeCD--;
                }
            }
        }

        public void HuntressEffect()
        {
            player.setHuntressT2 = true;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.HuntressAbility) && (player.controlDown && player.releaseDown))
            {
                if (player.doubleTapCardinalTimer[0] > 0 && player.doubleTapCardinalTimer[0] != 15)
                {
                    Vector2 mouse = Main.MouseWorld;

                    if (huntressCD == 0)
                    {
                        //find arrow type to use, for red riding only
                        Item firstAmmo = player.inventory[54];
                        int arrowType = firstAmmo.shoot;
                        int damage = HighestDamageTypeScaling(firstAmmo.damage);

                        if (!RedEnchant || firstAmmo.ammo != AmmoID.Arrow)
                        {
                            arrowType = ProjectileID.WoodenArrowFriendly;
                            damage = HighestDamageTypeScaling(5); //wooden arrow dmg
                        }

                        int heatray = Projectile.NewProjectile(player.Center, new Vector2(0, -6f), ProjectileID.HeatRay, 0, 0, Main.myPlayer);
                        Main.projectile[heatray].tileCollide = false;
                        //proj spawns arrows all around it until it dies
                        Projectile.NewProjectile(mouse.X, player.Center.Y - 500, 0f, 0f, ModContent.ProjectileType<ArrowRain>(), 50, 0f, player.whoAmI, arrowType, player.direction);

                        huntressCD = 900;
                        player.AddBuff(ModContent.BuffType<HuntressCD>(), 900);
                    }
                }
            }

            if (huntressCD != 0)
            {
                huntressCD--;
            }
        }

        public void MonkEffect()
        {
            player.setMonkT2 = true;
            MonkEnchant = true;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.MonkDash) && IsStandingStill && !player.mount.Active && !player.HasBuff(ModContent.BuffType<MonkBuff>()))
            {
                monkTimer++;

                if (monkTimer >= 60)
                {
                    player.AddBuff(ModContent.BuffType<MonkBuff>(), 2);
                    monkTimer = 0;

                    double spread = 2 * Math.PI / 36;
                    for (int i = 0; i < 36; i++)
                    {
                        Vector2 velocity = new Vector2(2, 2).RotatedBy(spread * i);

                        int index2 = Dust.NewDust(player.Center, 0, 0, DustID.GoldCoin, velocity.X, velocity.Y, 100);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].noLight = true;
                    }
                }
            }
        }

        public void EskimoEffect()
        {

        }

        public void AncientShadowEffect()
        {
            AncientShadowEnchant = true;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.AncientShadow) && player.ownedProjectileCounts[ModContent.ProjectileType<AncientShadowOrb>()] == 0)
            {
                const int max = 2;
                float rotation = 2f * (float)Math.PI / max;

                for (int i = 0; i < max; i++)
                {
                    Vector2 spawnPos = player.Center + new Vector2(60, 0f).RotatedBy(rotation * i);
                    int p = Projectile.NewProjectile(spawnPos, Vector2.Zero, ModContent.ProjectileType<AncientShadowOrb>(), 0, 10f, player.whoAmI, 0, rotation * i);
                    Main.projectile[p].GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
                }
            }
        }

        #endregion

        #region souls
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public void ColossusSoul(int maxHP, float damageResist, int lifeRegen, bool hideVisual)
        {
            player.statLifeMax2 += maxHP;
            player.endurance += damageResist;
            player.lifeRegen += lifeRegen;

            //hand warmer, pocket mirror, ankh shield
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Stoned] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Confused] = true;
            player.buffImmune[BuffID.Silenced] = true;
            player.buffImmune[BuffID.Cursed] = true;
            player.buffImmune[BuffID.Darkness] = true;
            player.noKnockback = true;
            player.fireWalk = true;
            //brain of confusion
            player.brainOfConfusion = true;
            //charm of myths
            player.pStone = true;
            //bee cloak, sweet heart necklace, star veil
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.StarCloak))
            {
                player.starCloak = true;
            }
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.BeesOnHit))
            {
                player.bee = true;
            }
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.PanicOnHit))
            {
                player.panic = true;
            }
            player.longInvince = true;
            //spore sac
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.SporeSac))
            {
                player.SporeSac();
                player.sporeSac = true;
            }
            //flesh knuckles
            player.aggro += 400;
            //frozen turtle shell
            if (player.statLife <= player.statLifeMax2 * 0.5) player.AddBuff(BuffID.IceBarrier, 5, true);
            //paladins shield
            if (player.statLife > player.statLifeMax2 * .25)
            {
                player.hasPaladinShield = true;
                for (int k = 0; k < 255; k++)
                {
                    Player target = Main.player[k];

                    if (target.active && player != target && Vector2.Distance(target.Center, player.Center) < 400) target.AddBuff(BuffID.PaladinsShield, 30);
                }
            }

            if (Fargowiltas.Instance.ThoriumLoaded)
            {
                thorium.GetItem("OceanRetaliation").UpdateAccessory(player, hideVisual);
                thorium.GetItem("TerrariumDefender").UpdateAccessory(player, hideVisual);
            }

            if (Fargowiltas.Instance.CalamityLoaded)
            {
                calamity.GetItem("RampartofDeities").UpdateAccessory(player, hideVisual);
                calamity.GetItem("AsgardianAegis").UpdateAccessory(player, hideVisual);
            }
        }

        public void SupersonicSoul(bool hideVisual)
        {
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.SupersonicSpeed) && !player.GetModPlayer<FargoPlayer>().noSupersonic && !EModeGlobalNPC.AnyBossAlive())
            {
                player.runAcceleration += SoulConfig.Instance.SupersonicMultiplier * .1f;
                player.maxRunSpeed += SoulConfig.Instance.SupersonicMultiplier * 2;
                //frog legs
                player.autoJump = true;
                player.jumpSpeedBoost += 2.4f;
                player.maxFallSpeed += 5f;
                player.jumpBoost = true;
            }
            else
            {
                //6.75 same as frostspark
                player.accRunSpeed = SoulConfig.Instance.GetValue(SoulConfig.Instance.IncreasedRunSpeed) ? 18.25f : 6.75f;
            }

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.NoMomentum))
            {
                player.runSlowdown = 10;
            }

            player.moveSpeed += 0.5f;
            player.rocketBoots = 3;
            player.rocketTimeMax = 10;
            player.iceSkate = true;
            //arctic diving gear
            player.arcticDivingGear = true;
            player.accFlipper = true;
            player.accDivingHelm = true;
            //lava waders
            player.waterWalk = true;
            player.fireWalk = true;
            player.lavaImmune = true;
            player.noFallDmg = true;
            //bundle
            if (player.wingTime == 0)
            {
                player.doubleJumpCloud = true;
                player.doubleJumpSandstorm = true;
                player.doubleJumpBlizzard = true;
                player.doubleJumpFart = true;
            }
            //magic carpet
            player.carpet = true;

            if (Fargowiltas.Instance.ThoriumLoaded)
            {
                thorium.GetItem("TerrariumBoots").UpdateAccessory(player, hideVisual);
                thorium.GetItem("AirWalkers").UpdateAccessory(player, hideVisual);
                thorium.GetItem("SurvivalistBoots").UpdateAccessory(player, hideVisual);
                thorium.GetItem("WeightedWinglets").UpdateAccessory(player, hideVisual);
            }
        }

        public void FlightMasterySoul()
        {
            player.wingTimeMax = 999999;
            player.wingTime = player.wingTimeMax;
            player.ignoreWater = true;
        }

        public void TrawlerSoul(bool hideVisual)
        {
            //instacatch
            FishSoul1 = true;
            //extra lures
            FishSoul2 = true;
            AddPet(SoulConfig.Instance.ZephyrFishPet, hideVisual, BuffID.ZephyrFish, ProjectileID.ZephyrFish);
            player.fishingSkill += 60;
            player.sonarPotion = true;
            player.cratePotion = true;
            player.accFishingLine = true;
            player.accTackleBox = true;
            player.accFishFinder = true;

            if (Fargowiltas.Instance.ThoriumLoaded)
            {
                thorium.GetItem("MagmaBoundFishingLine").UpdateAccessory(player, hideVisual);
            }
        }

        public void WorldShaperSoul(bool hideVisual)
        {
            //mining speed, spelunker, dangersense, light, hunter, pet
            MinerEffect(hideVisual, .66f);
            //placing speed up
            player.tileSpeed += 0.5f;
            player.wallSpeed += 0.5f;
            //toolbox
            Player.tileRangeX += 50;
            Player.tileRangeY += 50;
            //gizmo pack
            player.autoPaint = true;
            //presserator
            player.autoActuator = true;
            //royal gel
            player.npcTypeNoAggro[1] = true;
            player.npcTypeNoAggro[16] = true;
            player.npcTypeNoAggro[59] = true;
            player.npcTypeNoAggro[71] = true;
            player.npcTypeNoAggro[81] = true;
            player.npcTypeNoAggro[138] = true;
            player.npcTypeNoAggro[121] = true;
            player.npcTypeNoAggro[122] = true;
            player.npcTypeNoAggro[141] = true;
            player.npcTypeNoAggro[147] = true;
            player.npcTypeNoAggro[183] = true;
            player.npcTypeNoAggro[184] = true;
            player.npcTypeNoAggro[204] = true;
            player.npcTypeNoAggro[225] = true;
            player.npcTypeNoAggro[244] = true;
            player.npcTypeNoAggro[302] = true;
            player.npcTypeNoAggro[333] = true;
            player.npcTypeNoAggro[335] = true;
            player.npcTypeNoAggro[334] = true;
            player.npcTypeNoAggro[336] = true;
            player.npcTypeNoAggro[537] = true;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.BuilderMode))
                BuilderMode = true;

            //cell phone
            player.accWatch = 3;
            player.accDepthMeter = 1;
            player.accCompass = 1;
            player.accFishFinder = true;
            player.accDreamCatcher = true;
            player.accOreFinder = true;
            player.accStopwatch = true;
            player.accCritterGuide = true;
            player.accJarOfSouls = true;
            player.accThirdEye = true;
            player.accCalendar = true;
            player.accWeatherRadio = true;

            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            //pets
            AddPet(SoulConfig.Instance.thoriumToggles.BoxPet, hideVisual, thorium.BuffType("LockBoxBuff"), thorium.ProjectileType("LockBoxPet"));
        }


        #endregion

        #region maso acc

        #endregion
    }
}
