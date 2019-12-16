using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace FargowiltasSouls
{
    internal sealed partial class Fargowiltas
    {
        public override void AddRecipeGroups() => RecipeGroups.RegisterGroups(this);


        public static class RecipeGroups
        {
            public const string
                FARGOWILTAS_SOULS_PREFIX = "FargowiltasSouls",

                ANY_EVIL_WOOD = FARGOWILTAS_SOULS_PREFIX + ":AnyEvilWood",
                
                ANY_BUTTERFLY = FARGOWILTAS_SOULS_PREFIX + ":AnyButterfly",
                ANY_SQUIRREL = FARGOWILTAS_SOULS_PREFIX + ":AnySquirrel",

                ANY_ANVIL = FARGOWILTAS_SOULS_PREFIX + ":AnyAnvil",
                ANY_FORGE = FARGOWILTAS_SOULS_PREFIX + ":AnyForge",

                ANY_BOOKCASE = FARGOWILTAS_SOULS_PREFIX + ":AnyBookcase",

                ANY_ADAMANTITE_BAR = FARGOWILTAS_SOULS_PREFIX + ":AnyAdamantite",

                ANY_GOLD_PICKAXE = FARGOWILTAS_SOULS_PREFIX + ":AnyGoldPickaxe",
                ANY_DRAX = FARGOWILTAS_SOULS_PREFIX + ":AnyDrax",
                ANY_PHASESABER = FARGOWILTAS_SOULS_PREFIX + ":AnyPhasesaber",

                ANY_COBALT_REPEATER = FARGOWILTAS_SOULS_PREFIX + ":AnyCobaltRepeater",
                ANY_MYTHRIL_REPEATER = FARGOWILTAS_SOULS_PREFIX + ":AnyMythrilRepeater",
                ANY_ADAMANTITE_REPEATER = FARGOWILTAS_SOULS_PREFIX + ":AnyAdamantiteRepeater",


                BODY_SUFFIX = "Body",
                HEADPIECE_SUFFIX = "HeadPiece",
                LEGGINGS_SUFFIX = "Leggings",

                ANY_COBALT_HEADPIECE = FARGOWILTAS_SOULS_PREFIX + ":AnyCobalt" + HEADPIECE_SUFFIX,
                ANY_PALLADIUM_HEADPIECE = FARGOWILTAS_SOULS_PREFIX + ":AnyPalladium" + HEADPIECE_SUFFIX,

                ANY_MYTHRIL_HEADPIECE = FARGOWILTAS_SOULS_PREFIX + ":AnyMythril" + HEADPIECE_SUFFIX,
                ANY_ORICHALCUM_HEADPIECE = FARGOWILTAS_SOULS_PREFIX + ":AnyOrichalcum" + HEADPIECE_SUFFIX,

                ANY_ADAMANTITE_HEADPIECE = FARGOWILTAS_SOULS_PREFIX + ":AnyAdamantite" + HEADPIECE_SUFFIX,
                ANY_TITANIUM_HEADPIECE = FARGOWILTAS_SOULS_PREFIX + ":AnyTitanium" + HEADPIECE_SUFFIX,

                ANY_HALLOWED_HEADPIECE = FARGOWILTAS_SOULS_PREFIX + ":AnyHallowed" + HEADPIECE_SUFFIX,
                ANY_CHLOROPHITE_HEADPIECE = FARGOWILTAS_SOULS_PREFIX + ":AnyChlorophite" + HEADPIECE_SUFFIX,
                ANY_BEETLE_BODY = FARGOWILTAS_SOULS_PREFIX + ":AnyBeetle" + BODY_SUFFIX,
                ANY_SPECTRE_HEADPIECE = FARGOWILTAS_SOULS_PREFIX + ":AnySpectre" + HEADPIECE_SUFFIX,
                ANY_SHROOMITE_HEADPIECE = FARGOWILTAS_SOULS_PREFIX + ":AnyShroomite" + HEADPIECE_SUFFIX,


                THORIUM_PREFIX = FARGOWILTAS_SOULS_PREFIX + ":Thorium",

                ANY_THORIUM_YOYO = THORIUM_PREFIX + ":AnyYoyo",

                ANY_THORIUM_JESTER_HEADPIECE = THORIUM_PREFIX + ":AnyJester" + HEADPIECE_SUFFIX,
                ANY_THORIUM_JESTER_BODY = THORIUM_PREFIX + ":AnyJester" + BODY_SUFFIX,
                ANY_THORIUM_JESTER_LEGGINGS = THORIUM_PREFIX + ":AnyJester" + LEGGINGS_SUFFIX,
                
                ANY_THORIUM_TAMBOURINE = THORIUM_PREFIX + ":AnyTambourine",
                
                ANY_THORIUM_FAN_LETTER = THORIUM_PREFIX + ":AnyFanLetter",
                
                ANY_THORIUM_DUNGEON_BUTTERFLY = THORIUM_PREFIX + ":AnyDungeonButterfly";


            internal static void RegisterGroups(Fargowiltas fargowiltas)
            {
                RegisterGroups(new Dictionary<string, RecipeGroup>()
                {
                    { ANY_EVIL_WOOD, new RecipeGroup(() => Lang.misc[37] + " Evil Wood", ItemID.Ebonwood, ItemID.Shadewood) },

                    /* { ANY_BUTTERFLY, new RecipeGroup(() => Lang.misc[37] + " Butterfly", ItemID.JuliaButterfly, ItemID.MonarchButterfly, ItemID.PurpleEmperorButterfly,
                        ItemID.RedAdmiralButterfly, ItemID.SulphurButterfly, ItemID.TreeNymphButterfly, ItemID.UlyssesButterfly, ItemID.ZebraSwallowtailButterfly) }, */

                    // { ANY_SQUIRREL, new RecipeGroup(() => Lang.misc[37] + " Squirrel", ItemID.Squirrel, ItemID.SquirrelRed) },

                    { ANY_ANVIL, new RecipeGroup(() => Lang.misc[37] + " Mythril Anvil", ItemID.MythrilAnvil, ItemID.OrichalcumAnvil) },
                    { ANY_FORGE, new RecipeGroup(() => Lang.misc[37] + " Adamantite Forge", ItemID.AdamantiteForge, ItemID.TitaniumForge) },

                    { ANY_BOOKCASE, new RecipeGroup(() => Lang.misc[37] + " Bookcase", new int[]
                    {
                        ItemID.Bookcase,
                        ItemID.BlueDungeonBookcase,
                        ItemID.BoneBookcase,
                        ItemID.BorealWoodBookcase,
                        ItemID.CactusBookcase,
                        ItemID.CrystalBookCase,
                        ItemID.DynastyBookcase,
                        ItemID.EbonwoodBookcase,
                        ItemID.FleshBookcase,
                        ItemID.FrozenBookcase,
                        ItemID.GlassBookcase,
                        ItemID.GoldenBookcase,
                        ItemID.GothicBookcase,
                        ItemID.GraniteBookcase,
                        ItemID.GreenDungeonBookcase,
                        ItemID.HoneyBookcase,
                        ItemID.LivingWoodBookcase,
                        ItemID.MarbleBookcase,
                        ItemID.MeteoriteBookcase,
                        ItemID.MushroomBookcase,
                        ItemID.ObsidianBookcase,
                        ItemID.PalmWoodBookcase,
                        ItemID.PearlwoodBookcase,
                        ItemID.PinkDungeonBookcase,
                        ItemID.PumpkinBookcase,
                        ItemID.RichMahoganyBookcase,
                        ItemID.ShadewoodBookcase,
                        ItemID.SkywareBookcase,
                        ItemID.SlimeBookcase,
                        ItemID.SpookyBookcase,
                        ItemID.SteampunkBookcase
                    }) },

                    { ANY_ADAMANTITE_BAR, new RecipeGroup(() => Lang.misc[37] + " Adamantite Bar", ItemID.AdamantiteBar, ItemID.TitaniumBar) },

                    { ANY_GOLD_PICKAXE, new RecipeGroup(() => Lang.misc[37] + " Gold Pickaxe", ItemID.GoldPickaxe, ItemID.PlatinumPickaxe) },
                    { ANY_DRAX, new RecipeGroup(() => Lang.misc[37] + " Drax", ItemID.Drax, ItemID.PickaxeAxe) },
                    { ANY_PHASESABER, new RecipeGroup(() => Lang.misc[37] + " Phasesaber", ItemID.RedPhasesaber, ItemID.BluePhasesaber, ItemID.GreenPhasesaber, ItemID.PurplePhasesaber, ItemID.WhitePhasesaber,
                        ItemID.YellowPhasesaber) },

                    { ANY_COBALT_REPEATER, new RecipeGroup(() => Lang.misc[37] + " Cobalt Repeater", ItemID.CobaltRepeater, ItemID.PalladiumRepeater) },
                    { ANY_MYTHRIL_REPEATER, new RecipeGroup(() => Lang.misc[37] + " Mythril Repeater", ItemID.MythrilRepeater, ItemID.OrichalcumRepeater) },
                    { ANY_ADAMANTITE_REPEATER, new RecipeGroup(() => Lang.misc[37] + " Adamantite Repeater", ItemID.AdamantiteRepeater, ItemID.TitaniumRepeater) },


                    { ANY_COBALT_HEADPIECE, new RecipeGroup(() => Lang.misc[37] + " Cobalt Head Piece", ItemID.CobaltHelmet, ItemID.CobaltHat, ItemID.CobaltMask) },
                    { ANY_PALLADIUM_HEADPIECE, new RecipeGroup(() => Lang.misc[37] + " Palladium Head Piece", ItemID.PalladiumHeadgear, ItemID.PalladiumMask, ItemID.PalladiumHelmet) },

                    { ANY_MYTHRIL_HEADPIECE, new RecipeGroup(() => Lang.misc[37] + " Mythril Head Piece", ItemID.MythrilHat, ItemID.MythrilHelmet, ItemID.MythrilHood) },
                    { ANY_ORICHALCUM_HEADPIECE, new RecipeGroup(() => Lang.misc[37] + " Orichalcum Head Piece", ItemID.OrichalcumHeadgear, ItemID.OrichalcumMask, ItemID.OrichalcumHelmet) },

                    { ANY_ADAMANTITE_HEADPIECE, new RecipeGroup(() => Lang.misc[37] + " Adamantite Head Piece", ItemID.AdamantiteHelmet, ItemID.AdamantiteMask, ItemID.AdamantiteHeadgear) },
                    { ANY_TITANIUM_HEADPIECE, new RecipeGroup(() => Lang.misc[37] + " Titanium Head Piece", ItemID.TitaniumHeadgear, ItemID.TitaniumMask, ItemID.TitaniumHelmet) },

                    { ANY_HALLOWED_HEADPIECE, new RecipeGroup(() => Lang.misc[37] + " Hallowed Head Piece", ItemID.HallowedMask, ItemID.HallowedHeadgear, ItemID.HallowedHelmet) },
                    { ANY_CHLOROPHITE_HEADPIECE, new RecipeGroup(() => Lang.misc[37] + " Chlorophyte Head Piece", ItemID.ChlorophyteMask, ItemID.ChlorophyteHelmet, ItemID.ChlorophyteHeadgear) },
                    { ANY_BEETLE_BODY, new RecipeGroup(() => Lang.misc[37] + " Beetle Body", ItemID.BeetleShell, ItemID.BeetleScaleMail) },
                    { ANY_SPECTRE_HEADPIECE, new RecipeGroup(() => Lang.misc[37] + " Spectre Head Piece", ItemID.SpectreHood, ItemID.SpectreMask) },
                    { ANY_SHROOMITE_HEADPIECE, new RecipeGroup(() => Lang.misc[37] + " Shroomite Head Piece", ItemID.ShroomiteHeadgear, ItemID.ShroomiteMask, ItemID.ShroomiteHelmet) },
                });


                fargowiltas.CalamityCompatibility?.TryAddRecipeGroups();
                fargowiltas.ThoriumCompatibility?.TryAddRecipeGroups();
                fargowiltas.SoACompatibility?.TryAddRecipeGroups();
            }

            internal static void RegisterGroups(Dictionary<string, RecipeGroup> groups)
            {
                foreach (KeyValuePair<string, RecipeGroup> group in groups)
                    RecipeGroup.RegisterGroup(group.Key, group.Value);
            }
        }
    }
}
