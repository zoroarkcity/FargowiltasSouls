using System;
using System.Collections.Generic;
using System.Linq;
using CalamityMod.Buffs.Pets;
using Fargowiltas.Projectiles;
using FargowiltasSouls.Buffs.Boss;
using FargowiltasSouls.Buffs.Masomode;
using FargowiltasSouls.Buffs.Souls;
using FargowiltasSouls.NPCs;
using FargowiltasSouls.Projectiles.Masomode;
using FargowiltasSouls.Projectiles.Minions;
using FargowiltasSouls.Projectiles.Souls;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Buffs.Healer;
using ThoriumMod.Buffs.Pet;
using ThoriumMod.Buffs.Summon;
using ThoriumMod.Projectiles.Pets;

namespace FargowiltasSouls.Projectiles
{
    public class FargoGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        private bool townNPCProj = false;
        private int counter;
        public bool CanSplit = true;
        private int numSplits = 1;
        private static int adamantiteCD = 0;
        private int numSpeedups = 3;
        private bool ninjaTele;
        public bool IsRecolor = false;
        private bool stormBoosted = false;
        private int stormTimer;
        private bool tungstenProjectile = false;
        private bool tikiMinion = false;
        private int tikiTimer = 300;
        public bool rainbowTrail = false;
        private int rainbowCounter = 0;
        public bool Rainbow = false;
        public int GrazeCD;

        public Func<Projectile, bool> GrazeCheck = projectile => projectile.Distance(Main.LocalPlayer.Center) < Math.Min(projectile.width, projectile.height) / 2 + Player.defaultHeight + 100 && Collision.CanHit(projectile.Center, 0, 0, Main.LocalPlayer.Center, 0, 0);

        public bool Rotate = false;
        public int RotateDist = 32;
        public int RotateDir = 1;

        private bool firstTick = true;
        private bool squeakyToy = false;
        public int TimeFrozen = 0;
        public bool TimeFreezeImmune;
        public bool TimeFreezeCheck;
        public bool HasKillCooldown;
        public bool ImmuneToMutantBomb;

        public bool masobool;

        public int ModProjID;

        public override void SetDefaults(Projectile projectile)
        {
            if (FargoSoulsWorld.MasochistMode)
            {
                switch (projectile.type)
                {
                    case ProjectileID.CrystalBullet:
                    case ProjectileID.HolyArrow:
                        HasKillCooldown = true;
                        break;

                    case ProjectileID.StardustCellMinionShot:
                        projectile.minion = true; //allows it to hurt maso ML
                        break;

                    case ProjectileID.StardustGuardian:
                    case ProjectileID.StardustGuardianExplosion:
                        TimeFreezeImmune = true;
                        break;

                    case ProjectileID.SaucerLaser:
                        projectile.tileCollide = false;
                        break;

                    case ProjectileID.CultistBossFireBallClone:
                        if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.cultBoss, NPCID.CultistBoss))
                            projectile.timeLeft = 1;
                        break;

                    case ProjectileID.CursedFlameHostile:
                        /*if (FargoSoulsGlobalNPC.BossIsAlive(ref FargoSoulsGlobalNPC.wallBoss, NPCID.WallofFlesh))
                        {
                            projectile.tileCollide = false;
                            projectile.timeLeft = 120;
                            projectile.extraUpdates = 1;
                        }*/
                        break;

                    case ProjectileID.SharknadoBolt:
                        if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.fishBossEX, NPCID.DukeFishron))
                            projectile.extraUpdates++;
                        break;

                    case ProjectileID.FlamesTrap:
                        if (NPC.golemBoss != -1 && Main.npc[NPC.golemBoss].active && Main.npc[NPC.golemBoss].type == NPCID.Golem)
                            projectile.tileCollide = false;
                        break;

                    case ProjectileID.UnholyTridentHostile:
                        projectile.extraUpdates++;
                        break;

                    case ProjectileID.BulletSnowman:
                        projectile.tileCollide = false;
                        projectile.timeLeft = 600;
                        break;

                    case ProjectileID.CannonballHostile:
                        projectile.scale = 2f;
                        break;

                    case ProjectileID.EyeLaser:
                    case ProjectileID.EyeFire:
                        projectile.tileCollide = false;
                        break;

                    default:
                        break;
                }
            }

            if (projectile.type == ProjectileID.StardustGuardian || projectile.type == ProjectileID.StardustGuardianExplosion)
            {
                TimeFreezeImmune = true;
            }

            Fargowiltas.ModProjDict.TryGetValue(projectile.type, out ModProjID);
        }

        private static int[] noSplit = {
            ProjectileID.CrystalShard,
            ProjectileID.SandnadoFriendly,
            ProjectileID.LastPrism,
            ProjectileID.LastPrismLaser,
            ProjectileID.FlowerPetal,
            ProjectileID.BabySpider,
            ProjectileID.CrystalLeafShot,
            ProjectileID.Phantasm,
            ProjectileID.VortexBeater,
            ProjectileID.ChargedBlasterCannon,
            ProjectileID.MedusaHead,
            ProjectileID.WireKite,
            ProjectileID.DD2PhoenixBow
        };

        public override bool PreAI(Projectile projectile)
        {
            bool retVal = true;
            Player player = Main.player[Main.myPlayer];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            counter++;

            if (projectile.owner == Main.myPlayer)
            {
                if (firstTick)
                {
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];

                        if (npc.active && npc.townNPC && projectile.Hitbox.Intersects(npc.Hitbox))
                        {
                            townNPCProj = true;
                        }
                    }



                    if (!townNPCProj && modPlayer.FirstStrike && projectile.friendly && !Rotate && projectile.damage > 0 && !projectile.minion && projectile.aiStyle != 19 && projectile.aiStyle != 99 && CanSplit && Array.IndexOf(noSplit, projectile.type) <= -1)
                    {
                        Projectile p = NewProjectileDirectSafe(projectile.position + projectile.velocity * 2, projectile.velocity, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1]);
                        p.GetGlobalProjectile<FargoGlobalProjectile>().firstTick = false;
                        p.Opacity *= .75f;

                        p = NewProjectileDirectSafe(projectile.position - projectile.velocity * 2, projectile.velocity, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1]);
                        p.GetGlobalProjectile<FargoGlobalProjectile>().firstTick = false;
                        p.Opacity *= .75f;

                        player.ClearBuff(ModContent.BuffType<FirstStrike>());
                    }

                    if (modPlayer.TungstenEnchant && !townNPCProj && projectile.damage != 0 && !projectile.trap && projectile.aiStyle != 99 && projectile.type != ProjectileID.Arkhalis && projectile.friendly && SoulConfig.Instance.GetValue(SoulConfig.Instance.TungstenSize, false))
                    {
                        projectile.position = projectile.Center;
                        projectile.scale *= 2f;
                        projectile.width *= 2;
                        projectile.height *= 2;
                        projectile.Center = projectile.position;
                        tungstenProjectile = true;
                    }

                    if (modPlayer.TikiEnchant && modPlayer.TikiMinion && projectile.minion && projectile.minionSlots > 0)
                    {
                        tikiMinion = true;

                        if (projectile.type == ModContent.ProjectileType<EaterBody>() || projectile.type == ProjectileID.StardustDragon2 || projectile.type == ProjectileID.StardustDragon3)
                        {
                            for (int i = 0; i < 1000; i++)
                            {
                                Projectile pro = Main.projectile[i];

                                if (pro.type == ProjectileID.StardustDragon1 || pro.type == ProjectileID.StardustDragon4 || pro.type == ModContent.ProjectileType<EaterHead>() || pro.type == ModContent.ProjectileType<EaterTail>())
                                {
                                    pro.GetGlobalProjectile<FargoGlobalProjectile>().tikiMinion = true;
                                }
                            }
                        }
                    }

                    if (modPlayer.StardustEnchant && projectile.type == ProjectileID.StardustGuardianExplosion)
                    {
                        projectile.damage *= 5;
                    }

                    if (!townNPCProj && (modPlayer.AdamantiteEnchant || modPlayer.TerrariaSoul) && CanSplit && projectile.friendly && !projectile.hostile
                        && !Rotate && projectile.damage > 0 && !projectile.minion && projectile.aiStyle != 19 && projectile.aiStyle != 99
                        && SoulConfig.Instance.GetValue(SoulConfig.Instance.AdamantiteSplit) && Array.IndexOf(noSplit, projectile.type) <= -1)
                    {
                        if (adamantiteCD != 0)
                        {
                            adamantiteCD--;
                        }

                        if (adamantiteCD == 0)
                        {
                            adamantiteCD = modPlayer.TerrariaSoul ? 4 : 8;
                            SplitProj(projectile, 3);
                        }
                    }

                    if (projectile.bobber)
                    {
                        /*if (modPlayer.FishSoul1)
                        {
                            SplitProj(projectile, 5);
                        }*/
                        if (modPlayer.FishSoul2)
                        {
                            SplitProj(projectile, 11);
                        }
                    }

                    if (Rotate && !modPlayer.TerrariaSoul)
                    {
                        projectile.timeLeft = 600;
                    }

                    if (modPlayer.BeeEnchant && (projectile.type == ProjectileID.GiantBee || projectile.type == ProjectileID.Bee || projectile.type == ProjectileID.Wasp) && Main.rand.Next(2) == 0)
                    {
                        projectile.usesLocalNPCImmunity = true;
                        projectile.localNPCHitCooldown = 5;
                        projectile.penetrate *= 2;
                        projectile.timeLeft *= 2;
                        projectile.scale *= 3;
                        projectile.damage = (int)(projectile.damage * 1.5);
                    }
                }

                if (tungstenProjectile && (!modPlayer.TungstenEnchant || !SoulConfig.Instance.GetValue(SoulConfig.Instance.TungstenSize, false)))
                {
                    projectile.position = projectile.Center;
                    projectile.scale /= 2f;
                    projectile.width /= 2;
                    projectile.height /= 2;
                    projectile.Center = projectile.position;
                    tungstenProjectile = false;
                }

                if (tikiMinion)
                {
                    projectile.alpha = 120;

                    //dust
                    if (Main.rand.Next(4) < 2)
                    {
                        int dust = Dust.NewDust(new Vector2(projectile.position.X - 2f, projectile.position.Y - 2f), projectile.width + 4, projectile.height + 4, 44, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f, 100, Color.LimeGreen, .8f);
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

                    tikiTimer--;

                    if (tikiTimer <= 0)
                    {
                        for (int num468 = 0; num468 < 20; num468++)
                        {
                            int num469 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, 44, -projectile.velocity.X * 0.2f,
                                -projectile.velocity.Y * 0.2f, 100, Color.LimeGreen, 1f);
                            Main.dust[num469].noGravity = true;
                            Main.dust[num469].velocity *= 2f;
                            num469 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, 44, -projectile.velocity.X * 0.2f,
                                -projectile.velocity.Y * 0.2f, 100, Color.LimeGreen, .5f);
                            Main.dust[num469].velocity *= 2f;
                        }

                        projectile.Kill();
                    }
                }

                if (modPlayer.StardustEnchant && projectile.type == ProjectileID.StardustGuardian)
                {
                    projectile.localAI[0] = 0f;
                }

                if (rainbowTrail)
                {
                    rainbowCounter++;

                    if (rainbowCounter >= 5)
                    {
                        //rainbowCounter = 0;
                        if (player.whoAmI == Main.myPlayer)
                        {
                            int direction = projectile.velocity.X > 0 ? 1 : -1;
                            int p = Projectile.NewProjectile(projectile.Center, projectile.velocity, ProjectileID.RainbowBack, 30, 0, Main.myPlayer);
                            Projectile proj = Main.projectile[p];
                            proj.GetGlobalProjectile<FargoGlobalProjectile>().Rainbow = true;
                            proj.GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;

                            p = Projectile.NewProjectile(projectile.Center + (projectile.velocity / 2), projectile.velocity, ProjectileID.RainbowBack, 30, 0, Main.myPlayer);
                            proj = Main.projectile[p];
                            proj.GetGlobalProjectile<FargoGlobalProjectile>().Rainbow = true;
                            proj.GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
                        }
                    }
                }


                if (projectile.friendly && !projectile.hostile)
                {
                    if (modPlayer.ForbiddenEnchant && projectile.damage > 0 && projectile.type != ProjectileID.SandnadoFriendly && !stormBoosted)
                    {
                        Projectile nearestProj = null;

                        List<Projectile> projs = Main.projectile.Where(x => x.type == ProjectileID.SandnadoFriendly && x.active).ToList();

                        foreach (Projectile p in projs)
                        {
                            Vector2 stormDistance = p.Center - projectile.Center;

                            if (Math.Abs(stormDistance.X) < p.width / 2 && Math.Abs(stormDistance.Y) < p.height / 2)
                            {
                                nearestProj = p;
                                break;
                            }
                        }

                        if (nearestProj != null)
                        {
                            projectile.damage = (int)(projectile.damage * 1.5);

                            stormBoosted = true;
                            stormTimer = 120;
                        }
                    }

                    if (stormTimer > 0)
                    {
                        stormTimer--;

                        if (stormTimer == 0)
                        {
                            projectile.damage = (int)(projectile.damage * (2f / 3f));
                            stormBoosted = false;
                        }
                    }

                    if (modPlayer.Jammed && projectile.ranged && projectile.type != ProjectileID.ConfettiGun)
                    {
                        Projectile.NewProjectile(projectile.Center, projectile.velocity, ProjectileID.ConfettiGun, 0, 0f);
                        projectile.damage = 0;
                        projectile.position = new Vector2(Main.maxTilesX);
                        projectile.Kill();
                    }

                    if (modPlayer.Atrophied && projectile.thrown)
                    {
                        projectile.damage = 0;
                        projectile.position = new Vector2(Main.maxTilesX);
                        projectile.Kill();
                    }

                    if (modPlayer.SpookyEnchant && SoulConfig.Instance.GetValue(SoulConfig.Instance.SpookyScythes) && projectile.owner == Main.myPlayer
                        && projectile.minion && projectile.minionSlots > 0
                        && counter % 60 == 0 && Main.rand.Next(8 + Main.player[projectile.owner].maxMinions) == 0)
                    {
                        Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 62);
                        Projectile[] projs = XWay(8, projectile.Center, ModContent.ProjectileType<SpookyScythe>(), 5, projectile.damage / 2, 2f);
                        counter = 0;

                        for (int i = 0; i < 8; i++)
                        {
                            if (projs[i] == null) continue;
                            projs[i].GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
                        }
                    }
                }

                //hook AI
                if (modPlayer.MahoganyEnchant && projectile.aiStyle == 7 && (player.ZoneJungle || modPlayer.WoodForce) && counter >= 60
                    && SoulConfig.Instance.GetValue(SoulConfig.Instance.MahoganyHook))
                {
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC n = Main.npc[i];

                        if (n.CanBeChasedBy(projectile) && Vector2.Distance(n.Center, projectile.Center) < 400)
                        {
                            Vector2 velocity = Vector2.Normalize(n.Center - projectile.Center) * 5;

                            Projectile.NewProjectile(projectile.Center, velocity, ProjectileID.ChlorophyteBullet, 15, 1, Main.myPlayer);
                            break;
                        }
                    }

                    counter = 0;
                }
            }

            if(Rotate)
            {
                projectile.tileCollide = false;
                projectile.usesLocalNPCImmunity = true;

                Player p = Main.player[projectile.owner];

                //Factors for calculations
                double deg = projectile.ai[1];
                double rad = deg * (Math.PI / 180);

                projectile.position.X = p.Center.X - (int)(Math.Cos(rad) * RotateDist) - projectile.width / 2;
                projectile.position.Y = p.Center.Y - (int)(Math.Sin(rad) * RotateDist) - projectile.height / 2;

                //increase/decrease degrees
                if(RotateDir == 1)
                {
                    projectile.ai[1] += projectile.ai[0];
                }
                else
                {
                    projectile.ai[1] -= projectile.ai[0];
                }

                retVal = false;
            }

            if (TimeFrozen > 0 && !firstTick && !TimeFreezeImmune)
            {
                projectile.position = projectile.oldPosition;
                projectile.frameCounter--;
                projectile.timeLeft++;
                TimeFrozen--;
                retVal = false;
            }

            //masomode unicorn meme and pearlwood meme
            if (Rainbow)
            {
                Player p = Main.player[projectile.owner];

                projectile.tileCollide = false;

                counter++;
                if (counter >= 5)
                    projectile.velocity = Vector2.Zero;

                int deathTimer = 15;

                if (projectile.hostile)
                    deathTimer = 60;

                if (counter >= deathTimer)
                    projectile.Kill();
            }

            if (firstTick)
                firstTick = false;

            return retVal;
        }

        public static void SplitProj(Projectile projectile, int number)
        {
            if (projectile.type == ModContent.ProjectileType<SpawnProj>())
            {
                return;
            }

            //if its odd, we just keep the original 
            if (number % 2 != 0)
            {
                number--;
            }

            Projectile split;

            double spread = 0.6 / number;

            for (int i = 0; i < number / 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int factor = (j == 0) ? 1 : -1;
                    split = NewProjectileDirectSafe(projectile.Center, projectile.velocity.RotatedBy(factor * spread * (i + 1)), projectile.type, projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1]);

                    if (split != null)
                    {
                        split.friendly = true;
                        split.GetGlobalProjectile<FargoGlobalProjectile>().numSplits = projectile.GetGlobalProjectile<FargoGlobalProjectile>().numSplits;
                        split.GetGlobalProjectile<FargoGlobalProjectile>().firstTick = false;
                        split.GetGlobalProjectile<FargoGlobalProjectile>().tungstenProjectile = projectile.GetGlobalProjectile<FargoGlobalProjectile>().tungstenProjectile;
                    }
                }
            }
        }

        private void KillPet(Projectile projectile, Player player, int buff, bool enchant, bool toggle, bool minion = false)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (player.FindBuffIndex(buff) == -1)
            {
                if (player.dead || !(enchant || modPlayer.TerrariaSoul) || !SoulConfig.Instance.GetValue(toggle) || (!modPlayer.PetsActive && !minion))
                    projectile.Kill();
            }
        }

        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            switch (projectile.type)
            {
                #region pets

                case ProjectileID.BabyHornet:
                    KillPet(projectile, player, BuffID.BabyHornet, modPlayer.BeeEnchant, SoulConfig.Instance.HornetPet);
                    break;

                case ProjectileID.Sapling:
                    KillPet(projectile, player, BuffID.PetSapling, modPlayer.ChloroEnchant, SoulConfig.Instance.SeedlingPet);
                    break;

                case ProjectileID.BabyFaceMonster:
                    KillPet(projectile, player, BuffID.BabyFaceMonster, modPlayer.CrimsonEnchant, SoulConfig.Instance.FaceMonsterPet);
                    break;

                case ProjectileID.CrimsonHeart:
                    KillPet(projectile, player, BuffID.CrimsonHeart, modPlayer.CrimsonEnchant, SoulConfig.Instance.CrimsonHeartPet);
                    break;

                case ProjectileID.MagicLantern:
                    KillPet(projectile, player, BuffID.MagicLantern, modPlayer.MinerEnchant, SoulConfig.Instance.MagicLanternPet);
                    break;

                case ProjectileID.MiniMinotaur:
                    KillPet(projectile, player, BuffID.MiniMinotaur, modPlayer.GladEnchant, SoulConfig.Instance.MinotaurPet);
                    break;

                case ProjectileID.BlackCat:
                    KillPet(projectile, player, BuffID.BlackCat, modPlayer.NinjaEnchant, SoulConfig.Instance.BlackCatPet);
                    break;

                case ProjectileID.Wisp:
                    KillPet(projectile, player, BuffID.Wisp, modPlayer.SpectreEnchant, SoulConfig.Instance.WispPet);
                    break;

                case ProjectileID.CursedSapling:
                    KillPet(projectile, player, BuffID.CursedSapling, modPlayer.SpookyEnchant, SoulConfig.Instance.CursedSaplingPet);
                    break;

                case ProjectileID.EyeSpring:
                    KillPet(projectile, player, BuffID.EyeballSpring, modPlayer.SpookyEnchant, SoulConfig.Instance.EyeSpringPet);
                    break;

                case ProjectileID.Turtle:
                    KillPet(projectile, player, BuffID.PetTurtle, modPlayer.TurtleEnchant, SoulConfig.Instance.TurtlePet);
                    break;

                case ProjectileID.PetLizard:
                    KillPet(projectile, player, BuffID.PetLizard, modPlayer.TurtleEnchant, SoulConfig.Instance.LizardPet);
                    break;

                case ProjectileID.Truffle:
                    KillPet(projectile, player, BuffID.BabyTruffle, modPlayer.ShroomEnchant, SoulConfig.Instance.TrufflePet);
                    break;

                case ProjectileID.Spider:
                    KillPet(projectile, player, BuffID.PetSpider, modPlayer.SpiderEnchant, SoulConfig.Instance.SpiderPet);
                    break;

                case ProjectileID.Squashling:
                    KillPet(projectile, player, BuffID.Squashling, modPlayer.PumpkinEnchant, SoulConfig.Instance.SquashlingPet);
                    break;

                case ProjectileID.BlueFairy:
                    KillPet(projectile, player, BuffID.FairyBlue, modPlayer.HallowEnchant, SoulConfig.Instance.FairyPet);
                    break;

                case ProjectileID.StardustGuardian:
                    KillPet(projectile, player, BuffID.StardustGuardianMinion, modPlayer.StardustEnchant, SoulConfig.Instance.StardustGuardian, true);
                    break;

                case ProjectileID.TikiSpirit:
                    KillPet(projectile, player, BuffID.TikiSpirit, modPlayer.TikiEnchant, SoulConfig.Instance.TikiPet);
                    break;

                case ProjectileID.Penguin:
                    KillPet(projectile, player, BuffID.BabyPenguin, modPlayer.FrostEnchant, SoulConfig.Instance.PenguinPet);
                    break;

                case ProjectileID.BabySnowman:
                    KillPet(projectile, player, BuffID.BabySnowman, modPlayer.FrostEnchant, SoulConfig.Instance.SnowmanPet);
                    break;

                case ProjectileID.DD2PetGato:
                    KillPet(projectile, player, BuffID.PetDD2Gato, modPlayer.ShinobiEnchant, SoulConfig.Instance.GatoPet);
                    break;

                case ProjectileID.Parrot:
                    KillPet(projectile, player, BuffID.PetParrot, modPlayer.GoldEnchant, SoulConfig.Instance.ParrotPet);
                    break;

                case ProjectileID.Puppy:
                    KillPet(projectile, player, BuffID.Puppy, modPlayer.RedEnchant, SoulConfig.Instance.PuppyPet);
                    break;

                case ProjectileID.CompanionCube:
                    KillPet(projectile, player, BuffID.CompanionCube, modPlayer.VortexEnchant, SoulConfig.Instance.CompanionCubePet);
                    break;

                case ProjectileID.DD2PetDragon:
                    KillPet(projectile, player, BuffID.PetDD2Dragon, modPlayer.ValhallaEnchant, SoulConfig.Instance.DragonPet);
                    break;

                case ProjectileID.BabySkeletronHead:
                    KillPet(projectile, player, BuffID.BabySkeletronHead, modPlayer.NecroEnchant, SoulConfig.Instance.DGPet);
                    break;

                case ProjectileID.BabyDino:
                    KillPet(projectile, player, BuffID.BabyDinosaur, modPlayer.FossilEnchant, SoulConfig.Instance.DinoPet);
                    break;

                case ProjectileID.BabyEater:
                    KillPet(projectile, player, BuffID.BabyEater, modPlayer.ShadowEnchant, SoulConfig.Instance.EaterPet);
                    break;

                case ProjectileID.ShadowOrb:
                    KillPet(projectile, player, BuffID.ShadowOrb, modPlayer.ShadowEnchant, SoulConfig.Instance.ShadowOrbPet);
                    break;

                case ProjectileID.SuspiciousTentacle:
                    KillPet(projectile, player, BuffID.SuspiciousTentacle, modPlayer.CosmoForce, SoulConfig.Instance.SuspiciousEyePet);
                    break;

                case ProjectileID.DD2PetGhost:
                    KillPet(projectile, player, BuffID.PetDD2Ghost, modPlayer.DarkEnchant, SoulConfig.Instance.FlickerwickPet);
                    break;

                case ProjectileID.ZephyrFish:
                    KillPet(projectile, player, BuffID.ZephyrFish, modPlayer.FishSoul2, SoulConfig.Instance.ZephyrFishPet);
                    break;

                /*case ProjectileID.BabyGrinch:
                    if (player.FindBuffIndex(92) == -1)
                    {
                        if (!modPlayer.GrinchPet)
                        {
                            projectile.Kill();
                            return;
                        }
                    }
                    break;*/

                #endregion

                case ProjectileID.JavelinHostile:
                case ProjectileID.FlamingWood:
                    if (FargoSoulsWorld.MasochistMode)
                        projectile.position += projectile.velocity * .5f;
                    break;

                case ProjectileID.VortexAcid:
                    if (FargoSoulsWorld.MasochistMode)
                        projectile.position += projectile.velocity * .25f;
                    break;

                case ProjectileID.CultistRitual:
                    if (FargoSoulsWorld.MasochistMode)
                    {
                        if (!masobool) //MP sync data to server
                        {
                            masobool = true;
                            if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.cultBoss, NPCID.CultistBoss))
                            {
                                NPC cultist = Main.npc[EModeGlobalNPC.cultBoss];
                                if (Main.netMode == 1)
                                {
                                    EModeGlobalNPC fargoCultist = cultist.GetGlobalNPC<EModeGlobalNPC>();

                                    var netMessage = mod.GetPacket();
                                    netMessage.Write((byte)10);
                                    netMessage.Write((byte)EModeGlobalNPC.cultBoss);
                                    netMessage.Write(fargoCultist.Counter);
                                    netMessage.Write(fargoCultist.Counter2);
                                    netMessage.Write(fargoCultist.Timer);
                                    netMessage.Write(cultist.localAI[3]);
                                    netMessage.Send();

                                    fargoCultist.Counter = 0; //clear client side data now
                                    fargoCultist.Counter2 = 0;
                                    fargoCultist.Timer = 0;
                                    cultist.localAI[3] = 0f;
                                }
                                else //refresh ritual
                                {
                                    for (int i = 0; i < 1000; i++)
                                        if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<CultistRitual>())
                                        {
                                            Main.projectile[i].Kill();
                                            break;
                                        }
                                    Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<CultistRitual>(), 0, 0f, Main.myPlayer);
                                }
                            }
                        }

                        if (projectile.ai[0] > 120f && projectile.ai[0] < 299f) //instant ritual
                        {
                            projectile.ai[0] = 299f;
                            if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.cultBoss, NPCID.CultistBoss))
                            {
                                float ai0 = Main.rand.Next(4);

                                NPC cultist = Main.npc[EModeGlobalNPC.cultBoss];
                                EModeGlobalNPC fargoCultist = cultist.GetGlobalNPC<EModeGlobalNPC>();
                                int[] weight = new int[4];
                                weight[0] = fargoCultist.Counter;
                                weight[1] = fargoCultist.Counter2;
                                weight[2] = fargoCultist.Timer;
                                weight[3] = (int)cultist.localAI[3];
                                fargoCultist.Counter = 0;
                                fargoCultist.Counter2 = 0;
                                fargoCultist.Timer = 0;
                                cultist.localAI[3] = 0f;
                                int max = 0;
                                for (int i = 1; i < 4; i++)
                                    if (weight[max] < weight[i])
                                        max = i;
                                if (weight[max] > 0)
                                    ai0 = max;

                                if ((cultist.life < cultist.lifeMax / 2 || Fargowiltas.Instance.MasomodeEXLoaded) && Main.netMode != 1)
                                    Projectile.NewProjectile(projectile.Center, Vector2.UnitY * -10f, ModContent.ProjectileType<CelestialPillar>(),
                                        (int)(75 * (1 + FargoSoulsWorld.CultistCount * .0125)), 0f, Main.myPlayer, ai0);
                            }
                        }
                    }
                    break;

                case ProjectileID.MoonLeech:
                    if (FargoSoulsWorld.MasochistMode && projectile.ai[0] > 0f)
                    {
                        Vector2 distance = Main.player[(int)projectile.ai[1]].Center - projectile.Center - projectile.velocity;
                        if (distance != Vector2.Zero)
                            projectile.position += Vector2.Normalize(distance) * Math.Min(16f, distance.Length());
                    }
                    break;

                case ProjectileID.SandnadoHostile:
                    if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.deviBoss, mod.NPCType("DeviBoss")))
                    {
                        projectile.damage = Main.npc[EModeGlobalNPC.deviBoss].damage / 4;
                        if (Main.npc[EModeGlobalNPC.deviBoss].ai[0] != 5 && projectile.timeLeft > 90)
                            projectile.timeLeft = 90;
                    }
                    else if (FargoSoulsWorld.MasochistMode && projectile.timeLeft == 1199 && Main.netMode != 1)
                    {
                        int n = NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, NPCID.SandShark);
                        if (n < 200)
                        {
                            Main.npc[n].velocity.X = Main.rand.NextFloat(-10, 10);
                            Main.npc[n].velocity.Y = Main.rand.NextFloat(-20, -10);
                            Main.npc[n].netUpdate = true;
                            if (Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                    }
                    break;

                case ProjectileID.GoldenShowerHostile:
                    /*if (FargoSoulsWorld.MasochistMode && Main.netMode != 1 && Main.rand.Next(6) == 0
                        && !(projectile.position.Y / 16 > Main.maxTilesY - 200 && FargoSoulsGlobalNPC.BossIsAlive(ref FargoSoulsGlobalNPC.wallBoss, NPCID.WallofFlesh)))
                    {
                        int p = Projectile.NewProjectile(projectile.Center, projectile.velocity, ProjectileID.CrimsonSpray, 0, 0f, Main.myPlayer, 8f);
                        if (p != 1000)
                            Main.projectile[p].timeLeft = 6;
                    }*/
                    break;

                case ProjectileID.RuneBlast:
                    if (FargoSoulsWorld.MasochistMode && projectile.ai[0] == 1f)
                    {
                        if (projectile.localAI[0] == 0f)
                        {
                            projectile.localAI[0] = projectile.Center.X;
                            projectile.localAI[1] = projectile.Center.Y;
                        }
                        Vector2 distance = projectile.Center - new Vector2(projectile.localAI[0], projectile.localAI[1]);
                        if (distance != Vector2.Zero && distance.Length() >= 300f)
                        {
                            projectile.velocity = distance.RotatedBy(Math.PI / 2);
                            projectile.velocity.Normalize();
                            projectile.velocity *= 8f;
                        }
                    }
                    break;
    
                case ProjectileID.PhantasmalEye:
                    if (FargoSoulsWorld.MasochistMode)
                        projectile.position.X -= projectile.velocity.X / 2;
                    break;

                case ProjectileID.BombSkeletronPrime: //needs to be set every tick
                    if (FargoSoulsWorld.MasochistMode)
                        projectile.damage = (int)(40 * (1 + FargoSoulsWorld.PrimeCount * .0125));
                    break;

                default:
                        break;
            }

            if (Fargowiltas.Instance.ThoriumLoaded)
            {
                ThoriumPets(projectile);
            }

            if (Fargowiltas.Instance.CalamityLoaded)
            {
                CalamityPets(projectile);
            }

            if (stormBoosted)
            {
                int dustId = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.GoldFlame, projectile.velocity.X, projectile.velocity.Y, 100, default(Color), 1.5f);
                Main.dust[dustId].noGravity = true;
            }

            if (projectile.bobber && modPlayer.FishSoul1)
            {
                if (projectile.wet && projectile.ai[0] == 0 && projectile.localAI[1] < 655) //ai0 = in water, localai1 = counter up to catching an item
                    projectile.localAI[1] = 655; //quick catch. not 660 and up (that breaks fishron summoning)
            }
        }

        private void ThoriumPets(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            switch (ModProjID)
            {

                #region thorium pets

                case 2:
                    //KillPet(projectile, player, ModContent.BuffType<BioFeederBuff>(), modPlayer.MeteorEnchant, SoulConfig.Instance.thoriumToggles.BioFeederPet);
                    break;

                case 3:
                    KillPet(projectile, player, ModContent.BuffType<BlisterBuff>(), modPlayer.FleshEnchant, SoulConfig.Instance.thoriumToggles.BlisterPet);
                    break;

                case 4:
                    KillPet(projectile, player, ModContent.BuffType<WyvernPetBuff>(), modPlayer.DragonEnchant, SoulConfig.Instance.thoriumToggles.WyvernPet);
                    break;

                case 6:
                    KillPet(projectile, player, ModContent.BuffType<LockBoxBuff>(), modPlayer.MinerEnchant, SoulConfig.Instance.thoriumToggles.BoxPet);
                    break;

                case 9:
                    KillPet(projectile, player, ModContent.BuffType<LifeSpiritBuff>(), modPlayer.SacredEnchant, SoulConfig.Instance.thoriumToggles.SpiritPet);
                    break;

                case 11:
                    KillPet(projectile, player, ModContent.BuffType<SaplingBuff>(), modPlayer.LivingWoodEnchant, SoulConfig.Instance.thoriumToggles.SaplingMinion, true);
                    break;

                case 12:
                    KillPet(projectile, player, ModContent.BuffType<SnowyOwlBuff>(), modPlayer.CryoEnchant, SoulConfig.Instance.thoriumToggles.OwlPet);
                    break;

                case 13:
                    KillPet(projectile, player, ModContent.BuffType<JellyPet>(), modPlayer.DepthEnchant, SoulConfig.Instance.thoriumToggles.JellyfishPet);
                    break;

                case 17:
                    KillPet(projectile, player, ModContent.BuffType<ShineDust>(), modPlayer.PlatinumEnchant, SoulConfig.Instance.thoriumToggles.GlitterPet);
                    break;

                case 18:
                    KillPet(projectile, player, ModContent.BuffType<DrachmaBuff>(), modPlayer.GoldEnchant, SoulConfig.Instance.thoriumToggles.CoinPet);
                    break;

                    #endregion
            }
        }

        private void CalamityPets(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            switch (ModProjID)
            {
                case 101:
                    KillPet(projectile, player, ModContent.BuffType<Kendra>(), modPlayer.DaedalusEnchant, SoulConfig.Instance.calamityToggles.KendraPet);
                    break;

                case 102:
                    KillPet(projectile, player, ModContent.BuffType<BloodBound>(), modPlayer.StatigelEnchant, SoulConfig.Instance.calamityToggles.PerforatorPet);
                    break;

                case 103:
                    KillPet(projectile, player, ModContent.BuffType<ThirdSageBuff>(), modPlayer.DaedalusEnchant, SoulConfig.Instance.calamityToggles.ThirdSagePet);
                    break;

                case 104:
                    KillPet(projectile, player, ModContent.BuffType<BearBuff>(), modPlayer.DaedalusEnchant, SoulConfig.Instance.calamityToggles.BearPet);
                    break;

                case 105:
                    KillPet(projectile, player, ModContent.BuffType<BrimlingBuff>(), modPlayer.BrimflameEnchant, SoulConfig.Instance.calamityToggles.BrimlingPet);
                    break;

                case 106:
                    KillPet(projectile, player, ModContent.BuffType<DannyDevito>(), modPlayer.SulphurEnchant, SoulConfig.Instance.calamityToggles.DannyPet);
                    break;

                case 107:
                    KillPet(projectile, player, ModContent.BuffType<SirenLightPetBuff>(), modPlayer.FathomEnchant, SoulConfig.Instance.calamityToggles.SirenPet);
                    break;

                case 108:
                case 109:
                    KillPet(projectile, player, ModContent.BuffType<ChibiiBuff>(), modPlayer.GodSlayerEnchant, SoulConfig.Instance.calamityToggles.ChibiiPet);
                    break;

                case 110:
                    KillPet(projectile, player, ModContent.BuffType<AkatoYharonBuff>(), modPlayer.SilvaEnchant, SoulConfig.Instance.calamityToggles.AkatoPet);
                    break;

                case 111:
                    KillPet(projectile, player, ModContent.BuffType<Fox>(), modPlayer.SilvaEnchant, SoulConfig.Instance.calamityToggles.FoxPet);
                    break;

                case 112:
                    KillPet(projectile, player, ModContent.BuffType<LeviBuff>(), modPlayer.DemonShadeEnchant, SoulConfig.Instance.calamityToggles.LeviPet);
                    break;

                case 113:
                    KillPet(projectile, player, ModContent.BuffType<RotomBuff>(), modPlayer.AerospecEnchant, SoulConfig.Instance.calamityToggles.RotomPet);
                    break;

                case 114:
                    KillPet(projectile, player, ModContent.BuffType<AstrophageBuff>(), modPlayer.AstralEnchant, SoulConfig.Instance.calamityToggles.AstrophagePet);
                    break;

                case 115:
                    KillPet(projectile, player, ModContent.BuffType<SparksBuff>(), modPlayer.ReaverEnchant, SoulConfig.Instance.calamityToggles.SparksPet);
                    break;

                case 116:
                    KillPet(projectile, player, ModContent.BuffType<RadiatorBuff>(), modPlayer.SulphurEnchant, SoulConfig.Instance.calamityToggles.RadiatorPet);
                    break;

                case 117:
                    KillPet(projectile, player, ModContent.BuffType<BabyGhostBellBuff>(), modPlayer.MolluskEnchant, SoulConfig.Instance.calamityToggles.GhostBellPet);
                    break;

                case 118:
                    KillPet(projectile, player, ModContent.BuffType<FlakPetBuff>(), modPlayer.FathomEnchant, SoulConfig.Instance.calamityToggles.FlakPet);
                    break;

                case 119:
                    KillPet(projectile, player, ModContent.BuffType<SCalPetBuff>(), modPlayer.DemonShadeEnchant, SoulConfig.Instance.calamityToggles.ScalPet);
                    break;

            }
        }

        public override void PostAI(Projectile projectile)
        {
            if (!TimeFreezeCheck)
            {
                TimeFreezeCheck = true;
                if (projectile.whoAmI == Main.player[projectile.owner].heldProj)
                    TimeFreezeImmune = true;
            }

            if (projectile.hostile && projectile.damage > 0 && Main.LocalPlayer.active && !Main.LocalPlayer.dead) //graze
            {
                FargoPlayer fargoPlayer = Main.LocalPlayer.GetModPlayer<FargoPlayer>();
                if (fargoPlayer.Graze && --GrazeCD < 0 && !Main.LocalPlayer.immune && Main.LocalPlayer.hurtCooldowns[0] <= 0 && Main.LocalPlayer.hurtCooldowns[1] <= 0)
                {
                    if (GrazeCheck(projectile))
                    {
                        double grazeGain = fargoPlayer.MutantEye ? 0.04 : 0.02;
                        double grazeCap = fargoPlayer.MutantEye ? 1.0 : 0.3;

                        GrazeCD = 60;
                        fargoPlayer.GrazeBonus += grazeGain;
                        if (fargoPlayer.GrazeBonus > grazeCap)
                            fargoPlayer.GrazeBonus = grazeCap;
                        fargoPlayer.GrazeCounter = -1; //reset counter whenever successful graze

                        if (!Main.dedServ) //is this check needed...?
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Graze").WithVolume(1f), Main.LocalPlayer.Center);

                        const int max = 30; //make some indicator dusts
                        for (int i = 0; i < max; i++)
                        {
                            Vector2 vector6 = Vector2.UnitY * 5f;
                            vector6 = vector6.RotatedBy((i - (max / 2 - 1)) * 6.28318548f / max) + Main.LocalPlayer.Center;
                            Vector2 vector7 = vector6 - Main.LocalPlayer.Center;
                            //changes color when bonus is maxed
                            int d = Dust.NewDust(vector6 + vector7, 0, 0, fargoPlayer.GrazeBonus >= grazeCap ? 86 : 228, 0f, 0f, 0, default(Color), 2f);
                            Main.dust[d].noGravity = true;
                            Main.dust[d].velocity = vector7;
                        }
                    }
                    else
                    {
                        GrazeCD = 6; //don't check per tick ech
                    }
                }
            }
        }

        public override bool TileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough)
        {
            if (projectile.type == ProjectileID.SmokeBomb)
            {
                fallThrough = false;
            }

            if (tungstenProjectile)
            {
                width /= 2;
                height /= 2;
            }

            return base.TileCollideStyle(projectile, ref width, ref height, ref fallThrough);
        }

        public override Color? GetAlpha(Projectile projectile, Color lightColor)
        {
            if (IsRecolor)
            {
                if (projectile.type == ProjectileID.HarpyFeather)
                {
                    projectile.Name = "Vulture Feather";
                    return Color.Brown;
                }

                else if (projectile.type == ProjectileID.PineNeedleFriendly)
                {
                    return Color.GreenYellow;
                }

                else if(projectile.type == ProjectileID.Bone || projectile.type == ProjectileID.BoneGloveProj)
                {
                    return Color.SandyBrown;
                }

                else if (projectile.type == ProjectileID.DemonScythe)
                {
                    projectile.Name = "Blood Scythe";
                    return Color.Red;
                }
            }

            return null;
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (Fargowiltas.Instance.ThoriumLoaded) ThoriumOnHit(projectile, crit);
        }

        private void ThoriumOnHit(Projectile projectile, bool crit)
        {
            Player player = Main.player[Main.myPlayer];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.thoriumToggles.JesterBell))
            {
                //jester effect
                if (modPlayer.JesterEnchant && crit)
                {
                    for (int m = 0; m < 1000; m++)
                    {
                        Projectile projectile2 = Main.projectile[m];
                        if (projectile2.active && projectile2.type == thorium.ProjectileType("JestersBell"))
                        {
                            return;
                        }
                    }
                    Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 35, 1f, 0f);
                    Projectile.NewProjectile(player.Center.X, player.Center.Y - 50f, 0f, 0f, thorium.ProjectileType("JestersBell"), 0, 0f, projectile.owner, 0f, 0f);
                    Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, thorium.ProjectileType("JestersBell2"), 0, 0f, projectile.owner, 0f, 0f);
                }
            }
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            Player player = Main.player[Main.myPlayer];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (modPlayer.NinjaEnchant && projectile.type == ProjectileID.SmokeBomb && !ninjaTele)
            {
                ninjaTele = true;

                var teleportPos = new Vector2();

                teleportPos.X = projectile.position.X;
                teleportPos.Y = projectile.position.Y;

                //spiral out to find a save spot
                int count = 0;
                int increase = 10;
                while (Collision.SolidCollision(teleportPos, player.width, player.height))
                {
                    switch (count)
                    {
                        case 0:
                            teleportPos.X -= increase;
                            break;
                        case 1:
                            teleportPos.X += increase * 2;
                            break;
                        case 2:
                            teleportPos.Y += increase;
                            break;
                        default:
                            teleportPos.Y -= increase * 2;
                            increase += 10;
                            break;
                    }
                    count++;

                    if (count >= 4)
                    {
                        count = 0;
                    }

                }

                if (teleportPos.X > 50 && teleportPos.X < (double)(Main.maxTilesX * 16 - 50) && teleportPos.Y > 50 && teleportPos.Y < (double)(Main.maxTilesY * 16 - 50))
                {
                    player.Teleport(teleportPos, 1);
                    NetMessage.SendData(65, -1, -1, null, 0, player.whoAmI, teleportPos.X, teleportPos.Y, 1);

                    player.AddBuff(ModContent.BuffType<FirstStrike>(), 60);
                }
            }

            
            if (FargoSoulsWorld.MasochistMode && projectile.type == ProjectileID.SnowBallHostile)
            {
                projectile.active = false;
            }

            /*if (modPlayer.PearlEnchant && SoulConfig.Instance.GetValue("Pearlwood Rain") && projectile.type != ProjectileID.HallowStar)
            {
                //holy stars
                Main.PlaySound(SoundID.Item10, projectile.position);
                for (int num479 = 0; num479 < 10; num479++)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, 58, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, default(Color), 1.2f);
                }
                for (int num480 = 0; num480 < 3; num480++)
                {
                    Gore.NewGore(projectile.position, new Vector2(projectile.velocity.X * 0.05f, projectile.velocity.Y * 0.05f), Main.rand.Next(16, 18), 1f);
                }
                float x = projectile.position.X + (float)Main.rand.Next(-400, 400);
                float y = projectile.position.Y - (float)Main.rand.Next(600, 900);
                Vector2 vector12 = new Vector2(x, y);
                float num483 = projectile.position.X + (float)(projectile.width / 2) - vector12.X;
                float num484 = projectile.position.Y + (float)(projectile.height / 2) - vector12.Y;
                int num485 = 22;
                float num486 = (float)Math.Sqrt((double)(num483 * num483 + num484 * num484));
                num486 = (float)num485 / num486;
                num483 *= num486;
                num484 *= num486;
                int num487 = projectile.damage;
                int num488 = Projectile.NewProjectile(x, y, num483, num484, 92, num487, projectile.knockBack, projectile.owner, 0f, 0f);
                if (num488 != 1000)
                    Main.projectile[num488].ai[1] = projectile.position.Y;
                //Main.projectile[num488].ai[0] = 1f;

                //Main.projectile[num488].localNPCHitCooldown = 2;
                //Main.projectile[num488].usesLocalNPCImmunity = true;

                if (player.ZoneHoly || modPlayer.WoodForce)
                {
                    Main.projectile[num488].GetGlobalProjectile<FargoGlobalProjectile>().rainbowTrail = true;
                }
            }*/

            return true;
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref int damage, ref bool crit)
        {
            if(squeakyToy)
            {
                damage = 1;
                target.GetModPlayer<FargoPlayer>().Squeak(target.Center);
            }

            if (FargoSoulsWorld.MasochistMode)
            {
                switch (projectile.type)
                {
                    case ProjectileID.CursedFlameHostile: //spaz p3 balls are already scaled
                        if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.eaterBoss, NPCID.EaterofWorldsHead))
                            damage = (int)(damage * (1 + FargoSoulsWorld.EaterCount * .0125));
                        break;

                    case ProjectileID.Stinger:
                        if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.beeBoss, NPCID.QueenBee))
                            damage = (int)(damage * (1 + FargoSoulsWorld.BeeCount * .0125));
                        break;

                    case ProjectileID.DeathLaser: //may be used elsewhere?
                        if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.retiBoss, NPCID.Retinazer))
                            damage = (int)(damage * (1 + FargoSoulsWorld.TwinsCount * .0125));
                        else if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.destroyBoss, NPCID.TheDestroyer))
                            damage = (int)(damage * (1 + FargoSoulsWorld.DestroyerCount * .0125));
                        else if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.primeBoss, NPCID.SkeletronPrime))
                            damage = (int)(damage * (1 + FargoSoulsWorld.PrimeCount * .0125));
                        break;

                    case ProjectileID.PinkLaser:
                        if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.destroyBoss, NPCID.TheDestroyer))
                            damage = (int)(damage * (1 + FargoSoulsWorld.DestroyerCount * .0125));
                        break;

                    case ProjectileID.SeedPlantera:
                    case ProjectileID.PoisonSeedPlantera:
                    case ProjectileID.ThornBall:
                        damage = (int)(damage * (1 + FargoSoulsWorld.PlanteraCount * .0125));
                        break;

                    case ProjectileID.EyeBeam:
                        damage = (int)(damage * (1 + FargoSoulsWorld.GolemCount * .0125));
                        break;
                    case ProjectileID.Fireball:
                        if (EModeGlobalNPC.BossIsAlive(ref NPC.golemBoss, NPCID.Golem))
                            damage = (int)(damage * (1 + FargoSoulsWorld.GolemCount * .0125));
                        break;

                    case ProjectileID.VortexLightning:
                        if (NPC.downedGolemBoss)
                            damage *= 2;
                        break;

                    case ProjectileID.CultistBossFireBall:
                        if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.cultBoss, NPCID.CultistBoss))
                            damage = (int)(damage * 1.5 * (1 + FargoSoulsWorld.CultistCount * .0125));
                        break;

                    case ProjectileID.CultistBossLightningOrb:
                        if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.cultBoss, NPCID.CultistBoss))
                            damage = (int)(damage * (1 + FargoSoulsWorld.CultistCount * .0125));
                        break;
                    case ProjectileID.CultistBossIceMist:
                        damage = (int)(damage * (1 + FargoSoulsWorld.CultistCount * .0125));
                        break;

                    case ProjectileID.Sharknado:
                    case ProjectileID.Cthulunado:
                        if (FargoSoulsWorld.downedFishronEX || !EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.fishBossEX, NPCID.DukeFishron))
                            damage = (int)(damage * (1 + FargoSoulsWorld.FishronCount * .0125));
                        break;

                    case ProjectileID.PhantasmalBolt:
                    case ProjectileID.PhantasmalDeathray:
                    case ProjectileID.PhantasmalSphere:
                    case ProjectileID.PhantasmalEye:
                        if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.moonBoss, NPCID.MoonLordCore))
                            damage = (int)(damage * (1 + FargoSoulsWorld.MoonlordCount * .0125));
                        break;

                    default:
                        break;
                }
            }
        }

        public override void OnHitPlayer(Projectile projectile, Player target, int damage, bool crit)
        {
            if (FargoSoulsWorld.MasochistMode)
            {
                switch(projectile.type)
                {
                    case ProjectileID.JavelinHostile:
                        target.AddBuff(ModContent.BuffType<Defenseless>(), 600);
                        if (!target.HasBuff(ModContent.BuffType<Stunned>()))
                            target.AddBuff(ModContent.BuffType<Stunned>(), 60);
                        break;

                    case ProjectileID.DemonSickle:
                        target.AddBuff(BuffID.Darkness, 600);
                        target.AddBuff(ModContent.BuffType<Shadowflame>(), 300);
                        break;

                    case ProjectileID.HarpyFeather:
                        target.AddBuff(ModContent.BuffType<ClippedWings>(), 300);
                        break;
                        
                    case ProjectileID.SandBallFalling:
                        if (!target.HasBuff(ModContent.BuffType<Stunned>()) && projectile.velocity.X != 0) //so only antlion sand and not falling sand 
                            target.AddBuff(ModContent.BuffType<Stunned>(), 90);
                        break;

                    case ProjectileID.Stinger:
                        if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.beeBoss, NPCID.QueenBee))
                            target.AddBuff(ModContent.BuffType<Infested>(), 300);
                        target.AddBuff(BuffID.BrokenArmor, 300);
                        target.AddBuff(ModContent.BuffType<Swarming>(), 300);
                        break;

                    case ProjectileID.Skull:
                        if (Main.rand.Next(2) == 0)
                            target.AddBuff(BuffID.Cursed, 60);
                        if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.guardBoss, NPCID.DungeonGuardian))
                        {
                            target.AddBuff(ModContent.BuffType<GodEater>(), 420);
                            target.AddBuff(ModContent.BuffType<FlamesoftheUniverse>(), 420);
                            target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 420);
                            target.immune = false;
                            target.immuneTime = 0;
                        }
                        break;

                    case ProjectileID.EyeLaser:
                    case ProjectileID.GoldenShowerHostile:
                    case ProjectileID.CursedFlameHostile:
                        if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.wallBoss, NPCID.WallofFlesh))
                        {
                            target.AddBuff(BuffID.OnFire, 300);
                            target.AddBuff(ModContent.BuffType<ClippedWings>(), 240);
                            target.AddBuff(ModContent.BuffType<Crippled>(), 120);
                        }
                        break;

                    case ProjectileID.DeathSickle:
                        target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 300);
                        break;

                    case ProjectileID.DrManFlyFlask:
                        switch (Main.rand.Next(7))
                        {
                            case 0:
                                target.AddBuff(BuffID.Venom, 300);
                                break;
                            case 1:
                                target.AddBuff(BuffID.Confused, 300);
                                break;
                            case 2:
                                target.AddBuff(BuffID.CursedInferno, 300);
                                break;
                            case 3:
                                target.AddBuff(BuffID.OgreSpit, 300);
                                break;
                            case 4:
                                target.AddBuff(ModContent.BuffType<LivingWasteland>(), 600);
                                break;
                            case 5:
                                target.AddBuff(ModContent.BuffType<Defenseless>(), 600);
                                break;
                            case 6:
                                target.AddBuff(ModContent.BuffType<Purified>(), 600);
                                break;

                            default:
                                break;
                        }
                        target.AddBuff(BuffID.Stinky, 1200);
                        break;

                    case ProjectileID.SpikedSlimeSpike:
                        target.AddBuff(BuffID.Slimed, 120);
                        break;

                    case ProjectileID.CultistBossLightningOrb:
                    case ProjectileID.CultistBossLightningOrbArc:
                        target.AddBuff(ModContent.BuffType<LightningRod>(), 300);
                        target.AddBuff(BuffID.Electrified, 300);
                        break;

                    case ProjectileID.CultistBossIceMist:
                        if (!target.HasBuff(BuffID.Frozen))
                            target.AddBuff(BuffID.Frozen, 60);
                        target.AddBuff(BuffID.Chilled, 120);
                        break;

                    case ProjectileID.CultistBossFireBall:
                        target.AddBuff(ModContent.BuffType<Berserked>(), 300);
                        target.AddBuff(BuffID.BrokenArmor, 300);
                        target.AddBuff(BuffID.OnFire, 300);
                        break;

                    case ProjectileID.CultistBossFireBallClone:
                        target.AddBuff(ModContent.BuffType<Shadowflame>(), 600);
                        break;

                    case ProjectileID.PaladinsHammerHostile:
                        if (!target.HasBuff(ModContent.BuffType<Stunned>()))
                            target.AddBuff(ModContent.BuffType<Stunned>(), 60);
                        break;

                    case ProjectileID.RuneBlast:
                        target.AddBuff(ModContent.BuffType<FlamesoftheUniverse>(), 120);
                        target.AddBuff(ModContent.BuffType<Hexed>(), 240);
                        if (!EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.deviBoss, mod.NPCType("DeviBoss")))
                        {
                            target.AddBuff(BuffID.Suffocation, 240);
                        }

                        break;

                    case ProjectileID.ThornBall:
                    case ProjectileID.PoisonSeedPlantera:
                    case ProjectileID.SeedPlantera:
                        //target.AddBuff(BuffID.Poisoned, 300);
                        target.AddBuff(ModContent.BuffType<Infested>(), 180);
                        target.AddBuff(ModContent.BuffType<IvyVenom>(), 180);
                        break;

                    case ProjectileID.DesertDjinnCurse:
                        if (target.ZoneCorrupt)
                            target.AddBuff(BuffID.CursedInferno, 240);
                        else if (target.ZoneCrimson)
                            target.AddBuff(BuffID.Ichor, 240);
                        target.AddBuff(BuffID.Silenced, 120);
                        break;

                    case ProjectileID.BrainScramblerBolt:
                        target.AddBuff(ModContent.BuffType<Flipped>(), 240);
                        target.AddBuff(ModContent.BuffType<Unstable>(), 120);
                        break;

                    case ProjectileID.MartianTurretBolt:
                    case ProjectileID.GigaZapperSpear:
                        target.AddBuff(ModContent.BuffType<LightningRod>(), 300);
                        break;

                    case ProjectileID.RayGunnerLaser:
                        target.AddBuff(BuffID.VortexDebuff, 240);
                        break;

                    case ProjectileID.SaucerMissile:
                        target.AddBuff(ModContent.BuffType<ClippedWings>(), 300);
                        target.AddBuff(ModContent.BuffType<Crippled>(), 300);
                        break;

                    case ProjectileID.SaucerLaser:
                        target.AddBuff(BuffID.Electrified, 600);
                        break;

                    case ProjectileID.UFOLaser:
                    case ProjectileID.SaucerDeathray:
                        target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 180);
                        break;

                    case ProjectileID.FlamingWood:
                    case ProjectileID.GreekFire1:
                    case ProjectileID.GreekFire2:
                    case ProjectileID.GreekFire3:
                        target.AddBuff(BuffID.OnFire, 120);
                        target.AddBuff(BuffID.CursedInferno, 120);
                        target.AddBuff(ModContent.BuffType<Shadowflame>(), 120);
                        break;

                    case ProjectileID.VortexAcid:
                    case ProjectileID.VortexLaser:
                        target.AddBuff(ModContent.BuffType<LightningRod>(), 600);
                        target.AddBuff(ModContent.BuffType<ClippedWings>(), 300);
                        break;
                        
                    case ProjectileID.VortexLightning:
                        damage *= 2;
                        target.AddBuff(BuffID.Electrified, 300);
                        break;

                    case ProjectileID.LostSoulHostile:
                        if (!EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.deviBoss, mod.NPCType("DeviBoss")))
                        {
                            target.AddBuff(ModContent.BuffType<Hexed>(), 240);
                        }
                        target.AddBuff(ModContent.BuffType<ReverseManaFlow>(), 600);

                        break;

                    case ProjectileID.InfernoHostileBlast:
                    case ProjectileID.InfernoHostileBolt:
                        if (!EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.deviBoss, mod.NPCType("DeviBoss")))
                        {
                            if (Main.rand.Next(5) == 0)
                                target.AddBuff(ModContent.BuffType<Fused>(), 1800);
                        }
                        target.AddBuff(ModContent.BuffType<Jammed>(), 600);
                        break;

                    case ProjectileID.ShadowBeamHostile:
                        if (!EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.deviBoss, mod.NPCType("DeviBoss")))
                        {
                            target.AddBuff(ModContent.BuffType<Rotting>(), 1800);
                            target.AddBuff(ModContent.BuffType<Shadowflame>(), 300);
                        }
                        target.AddBuff(ModContent.BuffType<Atrophied>(), 600);
                        break;

                    case ProjectileID.PhantasmalDeathray:
                        target.AddBuff(ModContent.BuffType<CurseoftheMoon>(), 300);
                        break;

                    case ProjectileID.PhantasmalBolt:
                    case ProjectileID.PhantasmalEye:
                    case ProjectileID.PhantasmalSphere:
                        target.AddBuff(ModContent.BuffType<CurseoftheMoon>(), 300);
                        if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.mutantBoss, ModContent.NPCType<NPCs.MutantBoss.MutantBoss>()))
                        {
                            target.GetModPlayer<FargoPlayer>().MaxLifeReduction += 100;
                            target.AddBuff(ModContent.BuffType<OceanicMaul>(), 5400);
                            target.AddBuff(ModContent.BuffType<MutantFang>(), 180);
                        }
                        break;

                    case ProjectileID.RocketSkeleton:
                        target.AddBuff(BuffID.Dazed, 120);
                        target.AddBuff(BuffID.Confused, 120);
                        break;

                    case ProjectileID.FlamesTrap:
                    case ProjectileID.GeyserTrap:
                    case ProjectileID.Fireball:
                    case ProjectileID.EyeBeam:
                        target.AddBuff(BuffID.OnFire, 300);
                        if (NPC.golemBoss != -1 && Main.npc[NPC.golemBoss].active && Main.npc[NPC.golemBoss].type == NPCID.Golem)
                        {
                            target.AddBuff(BuffID.BrokenArmor, 600);
                            target.AddBuff(ModContent.BuffType<Defenseless>(), 600);
                            target.AddBuff(BuffID.WitheredArmor, 600);
                            if (Main.tile[(int)Main.npc[NPC.golemBoss].Center.X / 16, (int)Main.npc[NPC.golemBoss].Center.Y / 16] == null || //outside temple
                                Main.tile[(int)Main.npc[NPC.golemBoss].Center.X / 16, (int)Main.npc[NPC.golemBoss].Center.Y / 16].wall != WallID.LihzahrdBrickUnsafe)
                            {
                                target.AddBuff(BuffID.Burning, 120);
                            }
                        }
                        break;

                    case ProjectileID.SpikyBallTrap:
                        if (NPC.golemBoss != -1 && Main.npc[NPC.golemBoss].active && Main.npc[NPC.golemBoss].type == NPCID.Golem)
                        {
                            target.AddBuff(BuffID.BrokenArmor, 600);
                            target.AddBuff(ModContent.BuffType<Defenseless>(), 600);
                            target.AddBuff(BuffID.WitheredArmor, 600);
                        }
                        break;

                    case ProjectileID.DD2BetsyFireball:
                    case ProjectileID.DD2BetsyFlameBreath:
                        target.AddBuff(BuffID.OnFire, 600);
                        target.AddBuff(BuffID.Ichor, 600);
                        //target.AddBuff(BuffID.WitheredArmor, Main.rand.Next(60, 300));
                        //target.AddBuff(BuffID.WitheredWeapon, Main.rand.Next(60, 300));
                        target.AddBuff(BuffID.Burning, 300);
                        break;

                    case ProjectileID.DD2DrakinShot:
                        target.AddBuff(ModContent.BuffType<Shadowflame>(), 600);
                        break;

                    case ProjectileID.NebulaSphere:
                        target.AddBuff(BuffID.VortexDebuff, 300);
                        break;

                    case ProjectileID.NebulaLaser:
                        target.AddBuff(ModContent.BuffType<Hexed>(), 120);
                        break;

                    case ProjectileID.NebulaBolt:
                        target.AddBuff(ModContent.BuffType<Lethargic>(), 600);
                        break;

                    case ProjectileID.StardustJellyfishSmall:
                        target.AddBuff(BuffID.Frostburn, 180);
                        break;

                    case ProjectileID.StardustSoldierLaser:
                        target.AddBuff(BuffID.VortexDebuff, 120);
                        break;

                    case ProjectileID.Sharknado:
                        target.AddBuff(ModContent.BuffType<Defenseless>(), 600);
                        target.AddBuff(ModContent.BuffType<OceanicMaul>(), 1800);
                        target.GetModPlayer<FargoPlayer>().MaxLifeReduction += EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.fishBossEX, NPCID.DukeFishron) ? 100 : 10;
                        break;

                    case ProjectileID.FlamingScythe:
                        target.AddBuff(BuffID.OnFire, 900);
                        target.AddBuff(ModContent.BuffType<LivingWasteland>(), 900);
                        break;

                    case ProjectileID.SnowBallHostile:
                        if (!target.HasBuff(BuffID.Frozen) && Main.rand.Next(2) == 0)
                            target.AddBuff(BuffID.Frozen, 60);
                        break;
                        
                    case ProjectileID.BulletSnowman:
                        target.AddBuff(BuffID.Chilled, 180);
                        break;

                    case ProjectileID.UnholyTridentHostile:
                        target.AddBuff(BuffID.Darkness, 300);
                        target.AddBuff(BuffID.Blackout, 300);
                        target.AddBuff(ModContent.BuffType<Shadowflame>(), 600);
                        target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 180);
                        break;

                    case ProjectileID.BombSkeletronPrime:
                        target.AddBuff(ModContent.BuffType<Defenseless>(), 600);
                        break;

                    case ProjectileID.DeathLaser:
                        if (EModeGlobalNPC.BossIsAlive(ref EModeGlobalNPC.retiBoss, NPCID.Retinazer))
                            target.AddBuff(BuffID.Ichor, 600);
                        break;

                    case ProjectileID.BulletDeadeye:
                    case ProjectileID.CannonballHostile:
                        target.AddBuff(ModContent.BuffType<Defenseless>(), 600);
                        target.AddBuff(ModContent.BuffType<Midas>(), 900);
                        break;

                    case ProjectileID.AncientDoomProjectile:
                        target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 120);
                        target.AddBuff(ModContent.BuffType<Shadowflame>(), 300);
                        break;

                    case ProjectileID.SandnadoHostile:
                        if (!target.HasBuff(BuffID.Dazed))
                            target.AddBuff(BuffID.Dazed, 120);
                        break;

                    case ProjectileID.DD2OgreSmash:
                        target.AddBuff(BuffID.BrokenArmor, 300);
                        break;

                    case ProjectileID.DD2OgreStomp:
                        target.AddBuff(BuffID.Dazed, 120);
                        break;

                    case ProjectileID.DD2DarkMageBolt:
                        target.AddBuff(ModContent.BuffType<Hexed>(), 240);
                        break;

                    case ProjectileID.IceSpike:
                        //target.AddBuff(BuffID.Slimed, 120);
                        target.AddBuff(BuffID.Frostburn, 120);
                        break;

                    case ProjectileID.JungleSpike:
                        //target.AddBuff(BuffID.Slimed, 120);
                        target.AddBuff(ModContent.BuffType<Infested>(), 300);
                        break;

                    default:
                        break;
                }
            }
        }

        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            if (FargoSoulsWorld.MasochistMode && projectile.owner == Main.myPlayer && HasKillCooldown)
            {
                if (Main.player[projectile.owner].GetModPlayer<FargoPlayer>().MasomodeCrystalTimer <= 0)
                {
                    Main.player[projectile.owner].GetModPlayer<FargoPlayer>().MasomodeCrystalTimer = 15;
                    return true;
                }
                else
                {
                    /*if (projectile.type == ProjectileID.CrystalBullet)
                    {
                        Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0.0f);
                        for (int index1 = 0; index1 < 5; ++index1) //vanilla dusts
                        {
                            int index2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 68, 0.0f, 0.0f, 0, new Color(), 1f);
                            Main.dust[index2].noGravity = true;
                            Dust dust1 = Main.dust[index2];
                            dust1.velocity = dust1.velocity * 1.5f;
                            Dust dust2 = Main.dust[index2];
                            dust2.scale = dust2.scale * 0.9f;
                        }
                    }
                    else if (projectile.type == ProjectileID.HolyArrow || projectile.type == ProjectileID.HallowStar)
                    {
                        Main.PlaySound(SoundID.Item10, projectile.position);
                        for (int index = 0; index < 10; ++index)
                            Dust.NewDust(projectile.position, projectile.width, projectile.height, 58, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, new Color(), 1.2f);
                        for (int index = 0; index < 3; ++index)
                            Gore.NewGore(projectile.position, new Vector2(projectile.velocity.X * 0.05f, projectile.velocity.Y * 0.05f), Main.rand.Next(16, 18), 1f);
                        if (projectile.type == 12 && projectile.damage < 500)
                        {
                            for (int index = 0; index < 10; ++index)
                                Dust.NewDust(projectile.position, projectile.width, projectile.height, 57, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, new Color(), 1.2f);
                            for (int index = 0; index < 3; ++index)
                                Gore.NewGore(projectile.position, new Vector2(projectile.velocity.X * 0.05f, projectile.velocity.Y * 0.05f), Main.rand.Next(16, 18), 1f);
                        }
                    }*/
                    return false;
                }
            }


            


            return true;
        }

        public override void Kill(Projectile projectile, int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (!townNPCProj && CanSplit && projectile.friendly && projectile.damage > 0 && !projectile.minion && projectile.aiStyle != 19 && !Rotate)
            {
                if (modPlayer.CobaltEnchant && SoulConfig.Instance.GetValue(SoulConfig.Instance.CobaltShards) && modPlayer.CobaltCD == 0 && Main.rand.Next(4) == 0)
                {
                    int damage = 40;
                    if (modPlayer.EarthForce)
                        damage = 80;

                    Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 27);

                    for (int i = 0; i < 5; i++)
                    {
                        float velX = -projectile.velocity.X * Main.rand.Next(40, 70) * 0.01f + Main.rand.Next(-20, 21) * 0.4f;
                        float velY = -projectile.velocity.Y * Main.rand.Next(40, 70) * 0.01f + Main.rand.Next(-20, 21) * 0.4f;
                        int p = Projectile.NewProjectile(projectile.position.X + velX, projectile.position.Y + velY, velX, velY, ProjectileID.CrystalShard, damage, 0f, projectile.owner);

                        Main.projectile[p].GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
                    }

                    modPlayer.CobaltCD = 60;
                }
                else if (modPlayer.AncientCobaltEnchant && SoulConfig.Instance.GetValue(SoulConfig.Instance.CobaltStingers) && modPlayer.CobaltCD == 0 && Main.rand.Next(5) == 0)
                {
                   Projectile[] projs = XWay(3, projectile.Center, ProjectileID.HornetStinger, 5f, 15, 0);

                    for (int i = 0; i < projs.Length; i++)
                    {
                        projs[i].penetrate = 3;
                        projs[i].timeLeft /= 2;
                    }

                    modPlayer.CobaltCD = 60;
                }
            }
        }

        public override void GrapplePullSpeed(Projectile projectile, Player player, ref float speed)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (modPlayer.MahoganyEnchant)
            {
                speed *= 2;
            }

        }

        public override void GrappleRetreatSpeed(Projectile projectile, Player player, ref float speed)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            if (modPlayer.MahoganyEnchant)
            {
                speed *= 2;
            }
        }

        public static Projectile[] XWay(int num, Vector2 pos, int type, float speed, int damage, float knockback)
        {
            Projectile[] projs = new Projectile[num];
            double spread = 2 * Math.PI / num;
            for (int i = 0; i < num; i++)
                projs[i] = NewProjectileDirectSafe(pos, new Vector2(speed, speed).RotatedBy(spread * i), type, damage, knockback, Main.myPlayer);
            return projs;
        }

        public static int CountProj(int type)
        {
            int count = 0;

            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].type == type)
                {
                    count++;
                }
            }

            return count;
        }

        public static Projectile NewProjectileDirectSafe(Vector2 pos, Vector2 vel, int type, int damage, float knockback, int owner = 255, float ai0 = 0f, float ai1 = 0f)
        {
            int p = Projectile.NewProjectile(pos, vel, type, damage, knockback, owner, ai0, ai1);
            return (p < 1000) ? Main.projectile[p] : null;
        }
    }
}
