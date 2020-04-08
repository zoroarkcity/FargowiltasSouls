using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Fargowiltas.Items.Tiles;

namespace FargowiltasSouls.Items.Accessories.Forces.Calamity
{
    public class ExaltationForce : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("CalamityMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Force of Exaltation");
            Tooltip.SetDefault(
@"''
All armor bonuses from Tarragon, Bloodflare, and Brimflame
All armor bonuses from God Slayer, Silva, and Auric
Effects of Blazing Core, Dark Sun Ring, and Core of the Blood God
Effects of Affliction, Nebulous Core, and Draedon's Heart
Effects of the The Amalgam, Dynamo Stem Cells, and Godly Soul Artifact
Effects of Yharim's Gift, Heart of the Elements, and The Sponge
Summons several pets");
            DisplayName.AddTranslation(GameCulture.Chinese, "毁灭之力");
            Tooltip.AddTranslation(GameCulture.Chinese,
@"'让那些反抗你的人下地狱吧'
拥有软壳, 掠夺者和阿塔西亚的套装效果
拥有炫星, 龙蒿和魔影的套装效果
拥有大珍珠和阿米迪亚斯之垂饰的效果
拥有寓言龟壳和瘟疫蜂巢的效果
拥有星陨幻空石和星神游龙外壳的效果
拥有渎魂神物和蚀日尊戒的效果");

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

            mod.GetItem("TarragonEnchant").UpdateAccessory(player, hideVisual);
            mod.GetItem("BloodflareEnchant").UpdateAccessory(player, hideVisual);
            mod.GetItem("GodSlayerEnchant").UpdateAccessory(player, hideVisual);
            mod.GetItem("SilvaEnchant").UpdateAccessory(player, hideVisual);
            mod.GetItem("AuricEnchant").UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(null, "TarragonEnchant");
            recipe.AddIngredient(null, "BloodflareEnchant");
            recipe.AddIngredient(null, "GodSlayerEnchant");
            recipe.AddIngredient(null, "SilvaEnchant");
            recipe.AddIngredient(null, "AuricEnchant");

            recipe.AddTile(ModLoader.GetMod("Fargowiltas").TileType("CrucibleCosmosSheet"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
