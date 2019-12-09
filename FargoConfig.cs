
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;

namespace FargowiltasSouls
{
    class SoulConfig : ModConfig
    {

        public override ConfigScope Mode => ConfigScope.ClientSide;
        public static SoulConfig Instance;

        //[JsonIgnore]
        //public Dictionary<string, bool> Toggles = new Dictionary<string, bool>();

        [Header("$Mods.FargowiltasSouls.WoodHeader")]
        [Label("$Mods.FargowiltasSouls.BorealConfig")]
        [DefaultValue(true)]
        public bool BorealSnowballs;

        [Label("$Mods.FargowiltasSouls.EbonConfig")]
        [DefaultValue(true)]
        public bool EbonwoodAura;

        [Label("$Mods.FargowiltasSouls.ShadeConfig")]
        [DefaultValue(true)]
        public bool ShadewoodEffect;

        //[Label("[" +  "i:19][c/00FF00: Green Text]")]
        [Label("$Mods.FargowiltasSouls.MahoganyConfig")]
        [BackgroundColor(181, 108, 100)]
        [DefaultValue(true)]
        public bool MahoganyHook;

        [Label("$Mods.FargowiltasSouls.PalmConfig")]
        [DefaultValue(true)]
        public bool PalmwoodSentry;

        [Label("$Mods.FargowiltasSouls.PearlConfig")]
        [DefaultValue(true)]
        public bool PearlwoodStars;

        [Header("$Mods.FargowiltasSouls.EarthHeader")]
        [Label("$Mods.FargowiltasSouls.AdamantiteConfig")]
        [DefaultValue(true)]
        public bool AdamantiteSplit;

        [Label("$Mods.FargowiltasSouls.CobaltConfig")]
        [DefaultValue(true)]
        public bool CobaltShards;

        [Label("$Mods.FargowiltasSouls.AncientCobaltConfig")]
        [DefaultValue(true)]
        public bool CobaltStingers;

        [Label("$Mods.FargowiltasSouls.MythrilConfig")]
        [DefaultValue(true)]
        public bool MythrilSpeed;

        [Label("$Mods.FargowiltasSouls.OrichalcumConfig")]
        [DefaultValue(true)]
        public bool OrichalcumFire;

        [Label("$Mods.FargowiltasSouls.PalladiumConfig")]
        [DefaultValue(true)]
        public bool PalladiumHeal;

        [Label("$Mods.FargowiltasSouls.TitaniumConfig")]
        [DefaultValue(true)]
        public bool TitaniumDodge;

        [Header("$Mods.FargowiltasSouls.TerraHeader")]
        [Label("$Mods.FargowiltasSouls.CopperConfig")]
        [DefaultValue(true)]
        public bool CopperLightning;

        [Label("$Mods.FargowiltasSouls.IronMConfig")]
        [DefaultValue(true)]
        public bool IronMagnet;

        [Label("$Mods.FargowiltasSouls.IronSConfig")]
        [DefaultValue(true)]
        public bool IronShield;

        [Label("$Mods.FargowiltasSouls.CthulhuShield")]
        [DefaultValue(true)]
        public bool CthulhuShield;

        [Label("$Mods.FargowiltasSouls.TinConfig")]
        [DefaultValue(true)]
        public bool TinCrit;

        [Label("$Mods.FargowiltasSouls.TungstenConfig")]
        [DefaultValue(true)]
        public bool TungstenSize;

        [Header("$Mods.FargowiltasSouls.WillHeader")]
        [Label("$Mods.FargowiltasSouls.GladiatorConfig")]
        [DefaultValue(true)]
        public bool GladiatorJavelins;

        [Label("$Mods.FargowiltasSouls.GoldConfig")]
        [DefaultValue(true)]
        public bool goldCoin;

        [Label("$Mods.FargowiltasSouls.RedRidingConfig")]
        [DefaultValue(true)]
        public bool redBleed;

        [Label("$Mods.FargowiltasSouls.ValhallaConfig")]
        [DefaultValue(true)]
        public bool ValhallaKB;

        [Header("$Mods.FargowiltasSouls.LifeHeader")]
        [Label("$Mods.FargowiltasSouls.BeetleConfig")]
        [DefaultValue(true)]
        public bool BeetleEffect;

        [Label("$Mods.FargowiltasSouls.CactusConfig")]
        [DefaultValue(true)]
        public bool CactusNeedles;

        [Label("$Mods.FargowiltasSouls.PumpkinConfig")]
        [DefaultValue(true)]
        public bool PumpkinFire;

        [Label("$Mods.FargowiltasSouls.SpiderConfig")]
        [DefaultValue(true)]
        public bool SpiderCrits;

        [Label("$Mods.FargowiltasSouls.TurtleConfig")]
        [DefaultValue(true)]
        public bool TurtleShell;

        [Header("$Mods.FargowiltasSouls.NatureHeader")]
        [Label("$Mods.FargowiltasSouls.ChlorophyteConfig")]
        [DefaultValue(true)]
        public bool ChlorophyteCrystals;

        [Label("$Mods.FargowiltasSouls.CrimsonConfig")]
        [DefaultValue(true)]
        public bool CrimsonRegen;

        [Label("$Mods.FargowiltasSouls.FrostConfig")]
        [DefaultValue(true)]
        public bool FrostIcicles;

        [Label("$Mods.FargowiltasSouls.JungleConfig")]
        [DefaultValue(true)]
        public bool JungleSpores;

        [Label("$Mods.FargowiltasSouls.MoltenConfig")]
        [DefaultValue(true)]
        public bool MoltenInferno;

        [Label("$Mods.FargowiltasSouls.ShroomiteConfig")]
        [DefaultValue(true)]
        public bool ShroomiteStealth;

        [Header("$Mods.FargowiltasSouls.ShadowHeader")]
        [Label("$Mods.FargowiltasSouls.DarkArtConfig")]
        [DefaultValue(true)]
        public bool DarkArtistEffect;

        [Label("$Mods.FargowiltasSouls.NecroConfig")]
        [DefaultValue(true)]
        public bool NecroGuardian;

        [Label("$Mods.FargowiltasSouls.AncientShadowConfig")]
        [DefaultValue(true)]
        public bool AncientShadow;

        [Label("$Mods.FargowiltasSouls.ShadowConfig")]
        [DefaultValue(true)]
        public bool ShadowDarkness;

        [Label("$Mods.FargowiltasSouls.ShinobiConfig")]
        [DefaultValue(true)]
        public bool ShinobiWalls;

        [Label("$Mods.FargowiltasSouls.ShinobiTabiConfig")]
        [DefaultValue(true)]
        public bool ShinobiTabi;

        [Label("$Mods.FargowiltasSouls.SpookyConfig")]
        [DefaultValue(true)]
        public bool SpookyScythes;

        [Header("$Mods.FargowiltasSouls.SpiritHeader")]
        [Label("$Mods.FargowiltasSouls.ForbiddenConfig")]
        [DefaultValue(true)]
        public bool ForbiddenStorm;

        [Label("$Mods.FargowiltasSouls.HallowedConfig")]
        [DefaultValue(true)]
        public bool HallowSword;

        [Label("$Mods.FargowiltasSouls.HalllowSConfig")]
        [DefaultValue(true)]
        public bool HallowShield;

        [Label("$Mods.FargowiltasSouls.SilverConfig")]
        [DefaultValue(true)]
        public bool SilverSword;

        [Label("$Mods.FargowiltasSouls.SpectreConfig")]
        [DefaultValue(true)]
        public bool SpectreOrbs;

        [Label("$Mods.FargowiltasSouls.TikiConfig")]
        [DefaultValue(true)]
        public bool TikiMinions;

        [Header("$Mods.FargowiltasSouls.CosmoHeader")]
        [Label("$Mods.FargowiltasSouls.MeteorConfig")]
        [DefaultValue(true)]
        public bool MeteorShower;

        [Label("$Mods.FargowiltasSouls.NebulaConfig")]
        [DefaultValue(true)]
        public bool NebulaBoost;

        [Label("$Mods.FargowiltasSouls.SolarConfig")]
        [DefaultValue(true)]
        public bool SolarShield;

        [Label("$Mods.FargowiltasSouls.StardustConfig")]
        [DefaultValue(true)]
        public bool StardustGuardian;

        [Label("$Mods.FargowiltasSouls.VortexSConfig")]
        [DefaultValue(true)]
        public bool VortexStealth;

        [Label("$Mods.FargowiltasSouls.VortexVConfig")]
        [DefaultValue(true)]
        public bool VortexVoid;

        #region maso accessories
        //death fairy
        [Label("$Mods.FargowiltasSouls.MasoSlimeConfig")]
        [DefaultValue(true)]
        public bool slimeShield;

        [Label("$Mods.FargowiltasSouls.MasoEyeConfig")]
        [DefaultValue(true)]
        public bool eyeScythes;

        [Label("$Mods.FargowiltasSouls.MasoSkeleConfig")]
        [DefaultValue(true)]
        public bool skeleArms;

        //pure heart
        [Label("$Mods.FargowiltasSouls.MasoEaterConfig")]
        [DefaultValue(true)]
        public bool tinyEaters;

        [Label("$Mods.FargowiltasSouls.MasoBrainConfig")]
        [DefaultValue(true)]
        public bool awwMan;

        //bionomic cluster
        [Label("$Mods.FargowiltasSouls.MasoConcoctionConfig")]
        [DefaultValue(true)]
        public bool concoction;

        [Label("$Mods.FargowiltasSouls.MasoRainbowConfig")]
        [DefaultValue(true)]
        public bool rainSlime;

        [Label("$Mods.FargowiltasSouls.MasoFrigidConfig")]
        [DefaultValue(true)]
        public bool frostFire;

        [Label("$Mods.FargowiltasSouls.MasoNymphConfig")]
        [DefaultValue(true)]
        public bool heartAttacks;

        [Label("$Mods.FargowiltasSouls.MasoSqueakConfig")]
        [DefaultValue(true)]
        public bool squeakToy;

        [Label("$Mods.FargowiltasSouls.MasoPouchConfig")]
        [DefaultValue(true)]
        public bool pouchTentacles;

        [Label("$Mods.FargowiltasSouls.MasoClippedConfig")]
        [DefaultValue(true)]
        public bool clipAttack;

        //dubious circutry
        [Label("$Mods.FargowiltasSouls.MasoLightningConfig")]
        [DefaultValue(true)]
        public bool lightRod;

        [Label("$Mods.FargowiltasSouls.MasoProbeConfig")]
        [DefaultValue(true)]
        public bool destroyProbe;

        //heart of the masochist
        [Label("$Mods.FargowiltasSouls.MasoGravConfig")]
        [DefaultValue(true)]
        public bool gravGlobe;

        [Label("$Mods.FargowiltasSouls.MasoGrav2Config")]
        [DefaultValue(true)]
        public bool gravGlobe2;

        [Label("$Mods.FargowiltasSouls.MasoPump")]
        [DefaultValue(true)]
        public bool pumpCape;

        [Label("$Mods.FargowiltasSouls.MasoFlockoConfig")]
        [DefaultValue(true)]
        public bool flockoMinion;

        [Label("$Mods.FargowiltasSouls.MasoUfoConfig")]
        [DefaultValue(true)]
        public bool ufoMinion;

        [Label("$Mods.FargowiltasSouls.MasoTrueEyeConfig")]
        [DefaultValue(true)]
        public bool trueEoc;

        //chalice of the moon
        [Label("$Mods.FargowiltasSouls.MasoCelestConfig")]
        [DefaultValue(true)]
        public bool celestRune;

        [Label("$Mods.FargowiltasSouls.MasoPlantConfig")]
        [DefaultValue(true)]
        public bool plantMinion;

        [Label("$Mods.FargowiltasSouls.MasoGolemConfig")]
        [DefaultValue(true)]
        public bool golemGround;

        [Label("$Mods.FargowiltasSouls.MasoVisionConfig")]
        [DefaultValue(true)]
        public bool ancientVision;

        [Label("$Mods.FargowiltasSouls.MasoCultistConfig")]
        [DefaultValue(true)]
        public bool cultMinion;

        [Label("$Mods.FargowiltasSouls.MasoFishronConfig")]
        [DefaultValue(true)]
        public bool fishMinion;

        //lump of flesh
        [Label("$Mods.FargowiltasSouls.MasoPugentConfig")]
        [DefaultValue(true)]
        public bool pungentEye;

        //mutant armor
        [Label("$Mods.FargowiltasSouls.MasoAbomConfig")]
        [DefaultValue(true)]
        public bool abomMinion;

        [Label("$Mods.FargowiltasSouls.MasoRingConfig")]
        [DefaultValue(true)]
        public bool ringMinion;

        //other
        [Label("$Mods.FargowiltasSouls.MasoSpikeConfig")]
        [DefaultValue(true)]
        public bool spikeHit;

        [Label("$Mods.FargowiltasSouls.MasoIconConfig")]
        [DefaultValue(true)]
        public bool sinIcon;

        [Label("$Mods.FargowiltasSouls.MasoBossRecolors")]
        [DefaultValue(true)]
        public bool bossRecolors;


        [Label("Security wallet")]
        public WalletMenu wallet = new WalletMenu();

        public class WalletMenu
        {
            [Label("$Mods.FargowiltasSouls.WalletWardingConfig")]
            [DefaultValue(true)]
            public bool warding;

            [Label("$Mods.FargowiltasSouls.WalletViolentConfig")]
            [DefaultValue(true)]
            public bool violent;

            [Label("$Mods.FargowiltasSouls.WalletQuickConfig")]
            [DefaultValue(true)]
            public bool quick;

            [Label("$Mods.FargowiltasSouls.WalletLuckyConfig")]
            [DefaultValue(true)]
            public bool lucky;

            [Label("$Mods.FargowiltasSouls.WalletMenacingConfig")]
            [DefaultValue(true)]
            public bool menacing;

            [Label("$Mods.FargowiltasSouls.WalletLegendaryConfig")]
            [DefaultValue(true)]
            public bool legendary;

            [Label("$Mods.FargowiltasSouls.WalletUnrealConfig")]
            [DefaultValue(true)]
            public bool unreal;

            [Label("$Mods.FargowiltasSouls.WalletMythicalConfig")]
            [DefaultValue(true)]
            public bool mythical;

            [Label("$Mods.FargowiltasSouls.WalletGodlyConfig")]
            [DefaultValue(true)]
            public bool godly;

            [Label("$Mods.FargowiltasSouls.WalletDemonicConfig")]
            [DefaultValue(true)]
            public bool demonic;

            [Label("$Mods.FargowiltasSouls.WalletRuthlessConfig")]
            [DefaultValue(true)]
            public bool ruthless;

            [Label("$Mods.FargowiltasSouls.WalletLightConfig")]
            [DefaultValue(true)]
            public bool light;

            [Label("$Mods.FargowiltasSouls.WalletDeadlyConfig")]
            [DefaultValue(true)]
            public bool deadly;

            [Label("$Mods.FargowiltasSouls.WalletRapidConfig")]
            [DefaultValue(true)]
            public bool rapid;

            public void Change()
            {
                SoulConfig.Instance.walletToggles["Warding"] = warding;
                SoulConfig.Instance.walletToggles["Violent"] = violent;
                SoulConfig.Instance.walletToggles["Quick"] = quick;
                SoulConfig.Instance.walletToggles["Lucky"] = lucky;
                SoulConfig.Instance.walletToggles["Menacing"] = menacing;
                SoulConfig.Instance.walletToggles["Legendary"] = legendary;
                SoulConfig.Instance.walletToggles["Unreal"] = unreal;
                SoulConfig.Instance.walletToggles["Mythical"] = mythical;
                SoulConfig.Instance.walletToggles["Godly"] = godly;
                SoulConfig.Instance.walletToggles["Demonic"] = demonic;
                SoulConfig.Instance.walletToggles["Ruthless"] = ruthless;
                SoulConfig.Instance.walletToggles["Light"] = light;
                SoulConfig.Instance.walletToggles["Deadly"] = deadly;
                SoulConfig.Instance.walletToggles["Rapid"] = rapid;
            }
        }
        #endregion

        #region souls
        [Label("$Mods.FargowiltasSouls.MeleeConfig")]
        [DefaultValue(true)]
        public bool gladSpeed;

        [Label("$Mods.FargowiltasSouls.SniperConfig")]
        [DefaultValue(true)]
        public bool sharpSniper;

        [Label("$Mods.FargowiltasSouls.UniverseConfig")]
        [DefaultValue(true)]
        public bool universeSpeed;

        [Label("$Mods.FargowiltasSouls.MiningHuntConfig")]
        [DefaultValue(true)]
        public bool mineHunt;

        [Label("$Mods.FargowiltasSouls.MiningDangerConfig")]
        [DefaultValue(true)]
        public bool mineDanger;

        [Label("$Mods.FargowiltasSouls.MiningSpelunkConfig")]
        [DefaultValue(true)]
        public bool mineSpelunk;

        [Label("$Mods.FargowiltasSouls.MiningShineConfig")]
        [DefaultValue(true)]
        public bool mineShine;

        [Label("$Mods.FargowiltasSouls.BuilderConfig")]
        [DefaultValue(true)]
        public bool worldBuild;

        [Label("$Mods.FargowiltasSouls.DefenseSporeConfig")]
        [DefaultValue(true)]
        public bool colSpore;

        [Label("$Mods.FargowiltasSouls.DefenseStarConfig")]
        [DefaultValue(true)]
        public bool colStar;

        [Label("$Mods.FargowiltasSouls.DefenseBeeConfig")]
        [DefaultValue(true)]
        public bool colBee;

        [Label("$Mods.FargowiltasSouls.SupersonicConfig")]
        [DefaultValue(true)]
        public bool supersonicSpeed;

        [Label("$Mods.FargowiltasSouls.EternityConfig")]
        [DefaultValue(true)]
        public bool eternityStack;

        #endregion

        #region pets
        [Label("$Mods.FargowiltasSouls.PetCatConfig")]
        [DefaultValue(true)]
        public bool bCat;

        [Label("$Mods.FargowiltasSouls.PetCubeConfig")]
        [DefaultValue(true)]
        public bool cCube;

        [Label("$Mods.FargowiltasSouls.PetCurseSapConfig")]
        [DefaultValue(true)]
        public bool cSapling;

        [Label("$Mods.FargowiltasSouls.PetDinoConfig")]
        [DefaultValue(true)]
        public bool dino;

        [Label("$Mods.FargowiltasSouls.PetDragonConfig")]
        [DefaultValue(true)]
        public bool dragon;

        [Label("$Mods.FargowiltasSouls.PetEaterConfig")]
        [DefaultValue(true)]
        public bool eater;

        [Label("$Mods.FargowiltasSouls.PetEyeSpringConfig")]
        [DefaultValue(true)]
        public bool eSpring;

        [Label("$Mods.FargowiltasSouls.PetFaceMonsterConfig")]
        [DefaultValue(true)]
        public bool fMonster;

        [Label("$Mods.FargowiltasSouls.PetGatoConfig")]
        [DefaultValue(true)]
        public bool gato;

        [Label("$Mods.FargowiltasSouls.PetHornetConfig")]
        [DefaultValue(true)]
        public bool hornet;

        [Label("$Mods.FargowiltasSouls.PetLizardConfig")]
        [DefaultValue(true)]
        public bool lizard;

        [Label("$Mods.FargowiltasSouls.PetMinitaurConfig")]
        [DefaultValue(true)]
        public bool mMinitaur;

        [Label("$Mods.FargowiltasSouls.PetParrotConfig")]
        [DefaultValue(true)]
        public bool parrot;

        [Label("$Mods.FargowiltasSouls.PetPenguinConfig")]
        [DefaultValue(true)]
        public bool penguin;

        [Label("$Mods.FargowiltasSouls.PetPupConfig")]
        [DefaultValue(true)]
        public bool puppy;

        [Label("$Mods.FargowiltasSouls.PetSeedConfig")]
        [DefaultValue(true)]
        public bool seedling;

        [Label("$Mods.FargowiltasSouls.PetDGConfig")]
        [DefaultValue(true)]
        public bool dGuard;

        [Label("$Mods.FargowiltasSouls.PetSnowmanConfig")]
        [DefaultValue(true)]
        public bool snowman;

        [Label("$Mods.FargowiltasSouls.PetSpiderConfig")]
        [DefaultValue(true)]
        public bool spider;

        [Label("$Mods.FargowiltasSouls.PetSquashConfig")]
        [DefaultValue(true)]
        public bool squash;

        [Label("$Mods.FargowiltasSouls.PetTikiConfig")]
        [DefaultValue(true)]
        public bool tiki;

        [Label("$Mods.FargowiltasSouls.PetShroomConfig")]
        [DefaultValue(true)]
        public bool truffle;

        [Label("$Mods.FargowiltasSouls.PetTurtleConfig")]
        [DefaultValue(true)]
        public bool turtle;

        [Label("$Mods.FargowiltasSouls.PetZephyrConfig")]
        [DefaultValue(true)]
        public bool zFish;

        //LIGHT PETS
        [Label("$Mods.FargowiltasSouls.PetHeartConfig")]
        [DefaultValue(true)]
        public bool cHeart;

        [Label("$Mods.FargowiltasSouls.PetNaviConfig")]
        [DefaultValue(true)]
        public bool fairy;

        [Label("$Mods.FargowiltasSouls.PetFlickerConfig")]
        [DefaultValue(true)]
        public bool flick;

        [Label("$Mods.FargowiltasSouls.PetLanturnConfig")]
        [DefaultValue(true)]
        public bool mLanturn;

        [Label("$Mods.FargowiltasSouls.PetOrbConfig")]
        [DefaultValue(true)]
        public bool sOrb;

        [Label("$Mods.FargowiltasSouls.PetSuspEyeConfig")]
        [DefaultValue(true)]
        public bool sEye;

        [Label("$Mods.FargowiltasSouls.PetWispConfig")]
        [DefaultValue(true)]
        public bool wisp;

        #endregion

        #region thorium
        [Label("$Mods.FargowiltasSouls.ThoriumAirWalkersConfig")]
        [DefaultValue(true)]
        public bool airWalkers;

        [Label("$Mods.FargowiltasSouls.ThoriumCrystalScorpionConfig")]
        [DefaultValue(true)]
        public bool crystalScorpion;

        [Label("$Mods.FargowiltasSouls.ThoriumYumasPendantConfig")]
        [DefaultValue(true)]
        public bool yumasPendant;

        [Label("$Mods.FargowiltasSouls.ThoriumHeadMirrorConfig")]
        [DefaultValue(true)]
        public bool headMirror;

        [Label("$Mods.FargowiltasSouls.ThoriumCelestialAuraConfig")]
        [DefaultValue(true)]
        public bool celestialAura;

        [Label("$Mods.FargowiltasSouls.ThoriumAscensionStatueConfig")]
        [DefaultValue(true)]
        public bool ascensionStatue;

        [Label("$Mods.FargowiltasSouls.ThoriumManaBootsConfig")]
        [DefaultValue(true)]
        public bool manaBoots;

        [Label("$Mods.FargowiltasSouls.ThoriumBronzeLightningConfig")]
        [DefaultValue(true)]
        public bool bronzeLightning;

        [Label("$Mods.FargowiltasSouls.ThoriumIllumiteMissileConfig")]
        [DefaultValue(true)]
        public bool illumiteMissile;

        [Label("$Mods.FargowiltasSouls.ThoriumJesterBellConfig")]
        [DefaultValue(true)]
        public bool jesterBell;

        [Label("$Mods.FargowiltasSouls.ThoriumBeholderEyeConfig")]
        [DefaultValue(true)]
        public bool beholderEye;

        [Label("$Mods.FargowiltasSouls.ThoriumTerrariumSpiritsConfig")]
        [DefaultValue(true)]
        public bool terrariumSpirits;

        [Label("$Mods.FargowiltasSouls.ThoriumCrietzConfig")]
        [DefaultValue(true)]
        public bool crietz;

        [Label("$Mods.FargowiltasSouls.ThoriumYewCritsConfig")]
        [DefaultValue(true)]
        public bool yewCrits;

        [Label("$Mods.FargowiltasSouls.ThoriumCryoDamageConfig")]
        [DefaultValue(true)]
        public bool cryoDamage;

        [Label("$Mods.FargowiltasSouls.ThoriumWhiteDwarfConfig")]
        [DefaultValue(true)]
        public bool whiteDwarf;

        [Label("$Mods.FargowiltasSouls.ThoriumTideFoamConfig")]
        [DefaultValue(true)]
        public bool tideFoam;

        [Label("$Mods.FargowiltasSouls.ThoriumWhisperingTentaclesConfig")]
        [DefaultValue(true)]
        public bool whisperingTentacles;

        [Label("$Mods.FargowiltasSouls.ThoriumIcyBarrierConfig")]
        [DefaultValue(true)]
        public bool icyBarrier;

        [Label("$Mods.FargowiltasSouls.ThoriumPlagueFlaskConfig")]
        [DefaultValue(true)]
        public bool plagueFlask;

        [Label("$Mods.FargowiltasSouls.ThoriumTideGlobulesConfig")]
        [DefaultValue(true)]
        public bool tideGlobules;

        [Label("$Mods.FargowiltasSouls.ThoriumTideDaggersConfig")]
        [DefaultValue(true)]
        public bool tideDaggers;

        [Label("$Mods.FargowiltasSouls.ThoriumFolvAuraConfig")]
        [DefaultValue(true)]
        public bool folvAura;

        [Label("$Mods.FargowiltasSouls.ThoriumFolvBoltsConfig")]
        [DefaultValue(true)]
        public bool folvBolts;

        [Label("$Mods.FargowiltasSouls.ThoriumVampireGlandConfig")]
        [DefaultValue(true)]
        public bool vampireGland;

        [Label("$Mods.FargowiltasSouls.ThoriumFleshDropsConfig")]
        [DefaultValue(true)]
        public bool fleshDrops;

        [Label("$Mods.FargowiltasSouls.ThoriumDragonFlamesConfig")]
        [DefaultValue(true)]
        public bool dragonFlames;

        [Label("$Mods.FargowiltasSouls.ThoriumHarbingerOverchargeConfig")]
        [DefaultValue(true)]
        public bool harbingerOvercharge;

        [Label("$Mods.FargowiltasSouls.ThoriumAssassinDamageConfig")]
        [DefaultValue(true)]
        public bool assassinDamage;

        [Label("$Mods.FargowiltasSouls.ThoriumpyromancerBurstsConfig")]
        [DefaultValue(true)]
        public bool pyromancerBursts;

        [Label("$Mods.FargowiltasSouls.ThoriumConduitShieldConfig")]
        [DefaultValue(true)]
        public bool conduitShield;

        [Label("$Mods.FargowiltasSouls.ThoriumIncandescentSparkConfig")]
        [DefaultValue(true)]
        public bool incandescentSpark;

        [Label("$Mods.FargowiltasSouls.ThoriumGreedyMagnetConfig")]
        [DefaultValue(true)]
        public bool greedyMagnet;

        [Label("$Mods.FargowiltasSouls.ThoriumCyberStatesConfig")]
        [DefaultValue(true)]
        public bool cyberStates;

        [Label("$Mods.FargowiltasSouls.ThoriumMetronomeConfig")]
        [DefaultValue(true)]
        public bool metronome;

        [Label("$Mods.FargowiltasSouls.ThoriumMixTapeConfig")]
        [DefaultValue(true)]
        public bool mixTape;

        [Label("$Mods.FargowiltasSouls.ThoriumLodestoneConfig")]
        [DefaultValue(true)]
        public bool lodestoneResist;

        [Label("$Mods.FargowiltasSouls.ThoriumBiotechProbeConfig")]
        [DefaultValue(true)]
        public bool biotechProbe;

        [Label("$Mods.FargowiltasSouls.ThoriumProofAvariceConfig")]
        [DefaultValue(true)]
        public bool proofAvarice;

        [Label("$Mods.FargowiltasSouls.ThoriumSlagStompersConfig")]
        [DefaultValue(true)]
        public bool slagStompers;

        [Label("$Mods.FargowiltasSouls.ThoriumSpringStepsConfig")]
        [DefaultValue(true)]
        public bool springSteps;

        [Label("$Mods.FargowiltasSouls.ThoriumBerserkerConfig")]
        [DefaultValue(true)]
        public bool berserker;

        [Label("$Mods.FargowiltasSouls.ThoriumBeeBootiesConfig")]
        [DefaultValue(true)]
        public bool beeBooties;

        [Label("$Mods.FargowiltasSouls.ThoriumGhastlyCarapaceConfig")]
        [DefaultValue(true)]
        public bool ghastlyCarapace;

        [Label("$Mods.FargowiltasSouls.ThoriumSpiritWispsConfig")]
        [DefaultValue(true)]
        public bool spiritWisps;

        [Label("$Mods.FargowiltasSouls.ThoriumWarlockWispsConfig")]
        [DefaultValue(true)]
        public bool warlockWisps;

        [Label("$Mods.FargowiltasSouls.ThoriumDreadConfig")]
        [DefaultValue(true)]
        public bool dreadSpeed;

        [Label("$Mods.FargowiltasSouls.ThoriumDiverConfig")]
        [DefaultValue(true)]
        public bool divers;

        [Label("$Mods.FargowiltasSouls.ThoriumDemonBloodConfig")]
        [DefaultValue(true)]
        public bool demonBlood;

        [Label("$Mods.FargowiltasSouls.ThoriumDevilMinionConfig")]
        [DefaultValue(true)]
        public bool devilMinion;

        [Label("$Mods.FargowiltasSouls.ThoriumCherubMinionConfig")]
        [DefaultValue(true)]
        public bool cherubMinion;

        [Label("$Mods.FargowiltasSouls.ThoriumSaplingMinionConfig")]
        [DefaultValue(true)]
        public bool saplingMinion;

        //pets
        [Label("$Mods.FargowiltasSouls.ThoriumOmegaPetConfig")]
        [DefaultValue(true)]
        public bool omegaPet;

        [Label("$Mods.FargowiltasSouls.ThoriumIFOPetConfig")]
        [DefaultValue(true)]
        public bool ifoPet;

        [Label("$Mods.FargowiltasSouls.ThoriumBioFeederPetConfig")]
        [DefaultValue(true)]
        public bool bioFeederPet;

        [Label("$Mods.FargowiltasSouls.ThoriumBlisterPetConfig")]
        [DefaultValue(true)]
        public bool blisterPet;

        [Label("$Mods.FargowiltasSouls.ThoriumWyvernPetConfig")]
        [DefaultValue(true)]
        public bool wyvernPet;

        [Label("$Mods.FargowiltasSouls.ThoriumLanternPetConfig")]
        [DefaultValue(true)]
        public bool lanternPet;

        [Label("$Mods.FargowiltasSouls.ThoriumBoxPetConfig")]
        [DefaultValue(true)]
        public bool boxPet;

        [Label("$Mods.FargowiltasSouls.ThoriumSpiritPetConfig")]
        [DefaultValue(true)]
        public bool spiritPet;

        [Label("$Mods.FargowiltasSouls.ThoriumGoatPetConfig")]
        [DefaultValue(true)]
        public bool goatPet;

        [Label("$Mods.FargowiltasSouls.ThoriumOwlPetConfig")]
        [DefaultValue(true)]
        public bool owlPet;

        [Label("$Mods.FargowiltasSouls.ThoriumJellyfishPetConfig")]
        [DefaultValue(true)]
        public bool jellyfishPet;

        [Label("$Mods.FargowiltasSouls.ThoriumMooglePetConfig")]
        [DefaultValue(true)]
        public bool mooglePet;

        [Label("$Mods.FargowiltasSouls.ThoriumMaidPetConfig")]
        [DefaultValue(true)]
        public bool maidPet;

        [Label("$Mods.FargowiltasSouls.ThoriumSlimePetConfig")]
        [DefaultValue(true)]
        public bool slimePet;

        [Label("$Mods.FargowiltasSouls.ThoriumGlitterPetConfig")]
        [DefaultValue(true)]
        public bool glitterPet;

        [Label("$Mods.FargowiltasSouls.ThoriumCoinPetConfig")]
        [DefaultValue(true)]
        public bool coinPet;

        #endregion

        #region calamity
        [Label("$Mods.FargowiltasSouls.CalamityUrchinConfig")]
        [DefaultValue(true)]
        public bool urchin;

        [Label("$Mods.FargowiltasSouls.CalamityProfanedArtifactConfig")]
        [DefaultValue(true)]
        public bool profanedSoulArtifact;

        [Label("$Mods.FargowiltasSouls.CalamitySlimeMinionConfig")]
        [DefaultValue(true)]
        public bool slimeMinion;

        [Label("$Mods.FargowiltasSouls.CalamityReaverMinionConfig")]
        [DefaultValue(true)]
        public bool reaverMinion;

        [Label("$Mods.FargowiltasSouls.CalamityOmegaTentaclesConfig")]
        [DefaultValue(true)]
        public bool omegaTentacles;

        [Label("$Mods.FargowiltasSouls.CalamitySilvaMinionConfig")]
        [DefaultValue(true)]
        public bool silvaMinion;

        [Label("$Mods.FargowiltasSouls.CalamityGodlyArtifactConfig")]
        [DefaultValue(true)]
        public bool godlySoulArtifact;

        [Label("$Mods.FargowiltasSouls.CalamityMechwormMinionConfig")]
        [DefaultValue(true)]
        public bool mechwormMinion;

        [Label("$Mods.FargowiltasSouls.CalamityNebulousCoreConfig")]
        [DefaultValue(true)]
        public bool nebulousCore;

        [Label("$Mods.FargowiltasSouls.CalamityDevilMinionConfig")]
        [DefaultValue(true)]
        public bool RedDevilMinion;

        [Label("$Mods.FargowiltasSouls.CalamityPermafrostPotionConfig")]
        [DefaultValue(true)]
        public bool permafrostPotion;

        [Label("$Mods.FargowiltasSouls.CalamityDaedalusMinionConfig")]
        [DefaultValue(true)]
        public bool daedalusMinion;

        [Label("$Mods.FargowiltasSouls.CalamityPolterMinesConfig")]
        [DefaultValue(true)]
        public bool polterMines;

        [Label("$Mods.FargowiltasSouls.CalamityPlagueHiveConfig")]
        [DefaultValue(true)]
        public bool plagueHive;

        [Label("$Mods.FargowiltasSouls.CalamityChaosMinionConfig")]
        [DefaultValue(true)]
        public bool chaosMinion;

        [Label("$Mods.FargowiltasSouls.CalamityValkyrieMinionConfig")]
        [DefaultValue(true)]
        public bool valkyrieMinion;

        [Label("$Mods.FargowiltasSouls.CalamityYharimGiftConfig")]
        [DefaultValue(true)]
        public bool yharimGift;

        [Label("$Mods.FargowiltasSouls.CalamityFungalMinionConfig")]
        [DefaultValue(true)]
        public bool fungalMinion;

        [Label("$Mods.FargowiltasSouls.CalamityWaifuMinionsConfig")]
        [DefaultValue(true)]
        public bool waifuMinions;

        [Label("$Mods.FargowiltasSouls.CalamityShellfishMinionConfig")]
        [DefaultValue(true)]
        public bool shellfishMinion;

        [Label("$Mods.FargowiltasSouls.CalamityAmidiasPendantConfig")]
        [DefaultValue(true)]
        public bool amidiasPendant;

        [Label("$Mods.FargowiltasSouls.CalamityGiantPearlConfig")]
        [DefaultValue(true)]
        public bool giantPearl;

        [Label("$Mods.FargowiltasSouls.CalamityPoisonSeawaterConfig")]
        [DefaultValue(true)]
        public bool poisonSeawater;

        [Label("$Mods.FargowiltasSouls.CalamityDaedalusEffectsConfig")]
        [DefaultValue(true)]
        public bool daedalusEffects;

        [Label("$Mods.FargowiltasSouls.CalamityReaverEffectsConfig")]
        [DefaultValue(true)]
        public bool reaverEffects;

        [Label("$Mods.FargowiltasSouls.CalamityFabledTurtleConfig")]
        [DefaultValue(true)]
        public bool fabledTurtle;

        [Label("$Mods.FargowiltasSouls.CalamityAstralStarsConfig")]
        [DefaultValue(true)]
        public bool astralStars;

        [Label("$Mods.FargowiltasSouls.CalamityAtaxiaEffectsConfig")]
        [DefaultValue(true)]
        public bool ataxiaEffects;

        [Label("$Mods.FargowiltasSouls.CalamityXerocEffectsConfig")]
        [DefaultValue(true)]
        public bool xerocEffects;

        [Label("$Mods.FargowiltasSouls.CalamityTarragonEffectsConfig")]
        [DefaultValue(true)]
        public bool tarragonEffects;

        [Label("$Mods.FargowiltasSouls.CalamityBloodflareEffectsConfig")]
        [DefaultValue(true)]
        public bool bloodflareEffects;

        [Label("$Mods.FargowiltasSouls.CalamityGodSlayerEffectsConfig")]
        [DefaultValue(true)]
        public bool godSlayerEffects;

        [Label("$Mods.FargowiltasSouls.CalamitySilvaEffectsConfig")]
        [DefaultValue(true)]
        public bool silvaEffects;

        [Label("$Mods.FargowiltasSouls.CalamityAuricEffectsConfig")]
        [DefaultValue(true)]
        public bool auricEffects;

        [Label("$Mods.FargowiltasSouls.CalamityElementalQuiverConfig")]
        [DefaultValue(true)]
        public bool elementalQuiver;

        [Label("$Mods.FargowiltasSouls.CalamityLuxorGiftConfig")]
        [DefaultValue(true)]
        public bool luxorGift;

        [Label("$Mods.FargowiltasSouls.CalamityGladiatorLocketConfig")]
        [DefaultValue(true)]
        public bool gladiatorLocket;

        [Label("$Mods.FargowiltasSouls.CalamityUnstablePrismConfig")]
        [DefaultValue(true)]
        public bool unstablePrism;

        [Label("$Mods.FargowiltasSouls.CalamityRegeneratorConfig")]
        [DefaultValue(true)]
        public bool regenerator;

        [Label("$Mods.FargowiltasSouls.CalamityDivingSuitConfig")]
        [DefaultValue(true)]
        public bool divingSuit;

        //pets
        [Label("$Mods.FargowiltasSouls.CalamityKendraConfig")]
        [DefaultValue(true)]
        public bool kendraPet;

        [Label("$Mods.FargowiltasSouls.CalamityPerforatorConfig")]
        [DefaultValue(true)]
        public bool perforatorPet;

        [Label("$Mods.FargowiltasSouls.CalamityBearConfig")]
        [DefaultValue(true)]
        public bool bearPet;

        [Label("$Mods.FargowiltasSouls.CalamityThirdSageConfig")]
        [DefaultValue(true)]
        public bool thirdSagePet;

        [Label("$Mods.FargowiltasSouls.CalamityBrimlingConfig")]
        [DefaultValue(true)]
        public bool brimlingPet;

        [Label("$Mods.FargowiltasSouls.CalamityDannyConfig")]
        [DefaultValue(true)]
        public bool dannyPet;

        [Label("$Mods.FargowiltasSouls.CalamitySirenConfig")]
        [DefaultValue(true)]
        public bool sirenPet;

        [Label("$Mods.FargowiltasSouls.CalamityChibiiConfig")]
        [DefaultValue(true)]
        public bool chibiiPet;

        [Label("$Mods.FargowiltasSouls.CalamityAkatoConfig")]
        [DefaultValue(true)]
        public bool akatoPet;

        [Label("$Mods.FargowiltasSouls.CalamityFoxConfig")]
        [DefaultValue(true)]
        public bool foxPet;

        [Label("$Mods.FargowiltasSouls.CalamityLeviConfig")]
        [DefaultValue(true)]
        public bool leviPet;

        #endregion

        #region Soa

        #endregion



        [JsonIgnore]
        public Dictionary<string, bool> walletToggles = new Dictionary<string, bool>();


        public override void OnLoaded()
        {
            Instance = this;

            walletToggles.Add("Warding", wallet.warding);
            walletToggles.Add("Violent", wallet.violent);
            walletToggles.Add("Quick", wallet.quick);
            walletToggles.Add("Lucky", wallet.lucky);
            walletToggles.Add("Menacing", wallet.menacing);
            walletToggles.Add("Legendary", wallet.legendary);
            walletToggles.Add("Unreal", wallet.unreal);
            walletToggles.Add("Mythical", wallet.mythical);
            walletToggles.Add("Godly", wallet.godly);
            walletToggles.Add("Demonic", wallet.demonic);
            walletToggles.Add("Ruthless", wallet.ruthless);
            walletToggles.Add("Light", wallet.light);
            walletToggles.Add("Deadly", wallet.deadly);
            walletToggles.Add("Rapid", wallet.rapid);

        }


        public bool GetValue2(bool toggle, bool checkForMutantPresence = true)
        {
            if (!toggle || (checkForMutantPresence && Main.player[Main.myPlayer].GetModPlayer<FargoPlayer>().MutantPresence))
                return false;

            return true;
        }

        public bool GetValue(string input, bool checkForMutantPresence = true)
        {
            if (checkForMutantPresence && Main.player[Main.myPlayer].GetModPlayer<FargoPlayer>().MutantPresence)
                return false;


            if (walletToggles.ContainsKey(input))
            {
                return walletToggles[input];
            }



            return false;
        }
    }
}
