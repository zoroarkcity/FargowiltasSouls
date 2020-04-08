using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Fargowiltas.Items.Tiles;

namespace FargowiltasSouls.Items.Accessories.Forces.Calamity
{
    public class DesolationForce : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("CalamityMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Force of Desolation");
            Tooltip.SetDefault(
@"'When the world is barren and cold, you will be all that remains'
All armor bonuses from Daedalus, Snow Ruffian, Umbraphile, and Astral
All armor bonuses from Omega Blue, Mollusk, Victide, Fathom Swarmer, and Sulphurous
Effects of Scuttler's Jewel, Permafrost's Concoction, and Regenator
Effects of Thief's Dime, Vampiric Talisman, and Momentum Capacitor
Effects of the Astral Arcanum, Hide of Astrum Deus, and Gravistar Sabaton
Effects of the Abyssal Diving Suit, Mutated Truffle, and Old Duke's Scales
Effects of Giant Pearl and Amidias' Pendant
Effects of Aquatic Emblem and Enchanted Pearl
Effects of Ocean's Crest, Deep Diver, The Transformer, and Luxor's Gift
Effects of Corrosive Spine and Lumenous Amulet
Effects of Sand Cloak and Alluring Bait
Summons several pets");
            DisplayName.AddTranslation(GameCulture.Chinese, "荒芜之力");
            Tooltip.AddTranslation(GameCulture.Chinese,
@"'你将成为这个荒芜寒冷世界的最后幸存者'
拥有胜潮, 克希洛克和蓝色欧米茄的套装效果
拥有弑神者,  始源林海和古圣金源的套装效果
拥有深潜者, 变压器和祖玛的礼物的效果
拥有归一元心石, 幽影潜渊服和流明护身符的效果
拥有海波纹章, 星云之核和嘉登之心的效果
拥有聚合之脑, 圣魂神物和魔君的礼物的效果
拥有元灵之心和化绵留香石的效果");

        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 11;
            item.value = 600000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            mod.GetItem("DaedalusEnchant").UpdateAccessory(player, hideVisual);
            mod.GetItem("UmbraphileEnchant").UpdateAccessory(player, hideVisual);
            mod.GetItem("AstralEnchant").UpdateAccessory(player, hideVisual);
            mod.GetItem("OmegaBlueEnchant").UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(null, "DaedalusEnchant");
            recipe.AddIngredient(null, "UmbraphileEnchant");
            recipe.AddIngredient(null, "AstralEnchant");
            recipe.AddIngredient(null, "OmegaBlueEnchant");

            recipe.AddTile(ModLoader.GetMod("Fargowiltas").TileType("CrucibleCosmosSheet"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
