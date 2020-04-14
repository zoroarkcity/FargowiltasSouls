using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using ThoriumMod.Items.ArcaneArmor;
using ThoriumMod.Items.Hero;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.ThrownItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Thorium
{
    public class FlightEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("ThoriumMod") != null;
        }
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flight Enchantment");
            Tooltip.SetDefault(
@"'The sky is your playing field'
Increases flight time by 100%
Effects of the Faberge Egg");
            DisplayName.AddTranslation(GameCulture.Chinese, "飞羽魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'天空是你的游戏场'
增加100%飞行时间");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 2;
            item.value = 60000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;

            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            modPlayer.wingTimeModifier += 1f;
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.ThoriumLoaded) return;
            
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ModContent.ItemType<FlightMask>());
            recipe.AddIngredient(ModContent.ItemType<FlightMail>());
            recipe.AddIngredient(ModContent.ItemType<FlightBoots>());
            recipe.AddIngredient(ModContent.ItemType<ChampionWing>());
            recipe.AddIngredient(ModContent.ItemType<FabergeEgg>());
            recipe.AddIngredient(ModContent.ItemType<HarpyTalon>());
            recipe.AddIngredient(ModContent.ItemType<Aerial>());
            recipe.AddIngredient(ModContent.ItemType<RodFlocking>());
            recipe.AddIngredient(ModContent.ItemType<HarpiesBarrage>(), 300);
            recipe.AddIngredient(ModContent.ItemType<Bolas>(), 300);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
