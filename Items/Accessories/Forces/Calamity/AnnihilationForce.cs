using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Fargowiltas.Items.Tiles;

namespace FargowiltasSouls.Items.Accessories.Forces.Calamity
{
    public class AnnihilationForce : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("CalamityMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Force of Annihilation");
            Tooltip.SetDefault(
@"'Where once there was life and light, only ruin remains...'
All armor bonuses from Aerospec, Statigel, and Hydrothermic
All armor bonuses from Xeroc and Fearmonger
Effects of Gladiator's Locket and Unstable Prism
Effects of Counter Scarf and Fungal Symbiote
Effects of Hallowed Rune, Ethereal Extorter, and The Community
Effects of The Evolution, Spectral Veil, and Statis' Void Sash
Summons several pets");
            DisplayName.AddTranslation(GameCulture.Chinese, "天启之力");
            Tooltip.AddTranslation(GameCulture.Chinese,
@"'此地曾充满生命与光明, 现在只余废墟'
拥有天蓝, 斯塔提斯, 代达罗斯和血炎的套装效果
拥有灵魂浮雕, 掠袭者护符和气之挂坠的效果
拥有角斗士链坠和不稳定棱镜的效果
拥有反击围巾和真菌共生体的效果
拥有佩码·福洛斯特之融魔台和再生器的效果
拥有血神核心和灾劫之尖啸的效果");

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

            mod.GetItem("AerospecEnchant").UpdateAccessory(player, hideVisual);
            mod.GetItem("StatigelEnchant").UpdateAccessory(player, hideVisual);
            mod.GetItem("AtaxiaEnchant").UpdateAccessory(player, hideVisual);
            mod.GetItem("XerocEnchant").UpdateAccessory(player, hideVisual);
            mod.GetItem("FearmongerEnchant").UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(null, "AerospecEnchant");
            recipe.AddIngredient(null, "StatigelEnchant");
            recipe.AddIngredient(null, "AtaxiaEnchant");
            recipe.AddIngredient(null, "XerocEnchant");
            recipe.AddIngredient(null, "FearmongerEnchant");

            recipe.AddTile(ModLoader.GetMod("Fargowiltas").TileType("CrucibleCosmosSheet"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
