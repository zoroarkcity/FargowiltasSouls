using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Souls
{
    public class CalamitySoul : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");
        public int dragonTimer = 60;
        public const int FireProjectiles = 2;
        public const float FireAngleSpread = 120f;
        public int FireCountdown;

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("CalamityMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul of the Tyrant");
            Tooltip.SetDefault(
@"'And the land grew quiet once more...'
All armor bonuses from Aerospec, Statigel, and Hydrothermic
All armor bonuses from Xeroc and Fearmonger
All armor bonuses from Daedalus, Snow Ruffian, Umbraphile, and Astral
All armor bonuses from Omega Blue, Mollusk, Victide, Fathom Swarmer, and Sulphurous
All armor bonuses from Wulfrum, Reaver, Plague Reaper, and Demonshade
All armor bonuses from Tarragon, Bloodflare, and Brimflame
All armor bonuses from God Slayer, Silva, and Auric
Effects of Gladiator's Locket and Unstable Prism
Effects of Counter Scarf and Fungal Symbiote
Effects of Hallowed Rune, Ethereal Extorter, and The Community
Effects of The Evolution, Spectral Veil, and Statis' Void Sash
Effects of Scuttler's Jewel, Permafrost's Concoction, and Regenator
Effects of Thief's Dime, Vampiric Talisman, and Momentum Capacitor
Effects of the Astral Arcanum, Hide of Astrum Deus, and Gravistar Sabaton
Effects of the Abyssal Diving Suit, Mutated Truffle, and Old Duke's Scales
Effects of Giant Pearl and Amidias' Pendant
Effects of Aquatic Emblem and Enchanted Pearl
Effects of Ocean's Crest, Deep Diver, The Transformer, and Luxor's Gift
Effects of Corrosive Spine and Lumenous Amulet
Effects of Sand Cloak and Alluring Bait
Effects of Trinket of Chi, Fabled Tortoise Shell, and Plague Hive
Effects of Plagued Fuel Pack, The Bee, The Camper, and Profaned Soul Crystal
Effects of Blazing Core, Dark Sun Ring, and Core of the Blood God
Effects of Affliction, Nebulous Core, and Draedon's Heart
Effects of the The Amalgam, Dynamo Stem Cells, and Godly Soul Artifact
Effects of Yharim's Gift, Heart of the Elements, and The Sponge
Summons several pets");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 10;//
            item.value = 20000000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            mod.GetItem("AnnihilationForce").UpdateAccessory(player, hideVisual);
            mod.GetItem("DesolationForce").UpdateAccessory(player, hideVisual);
            mod.GetItem("DevastationForce").UpdateAccessory(player, hideVisual);
            mod.GetItem("ExaltationForce").UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(null, "AnnihilationForce");
            recipe.AddIngredient(null, "DevastationForce");
            recipe.AddIngredient(null, "DesolationForce");
            recipe.AddIngredient(null, "ExaltationForce");
            recipe.AddIngredient(null, "MutantScale", 10);

            recipe.AddTile(calamity, "DraedonsForge");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
