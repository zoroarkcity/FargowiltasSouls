using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Weapons.SwarmDrops
{
    public class BigBrainBuster : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Big Brain Buster");
            Tooltip.SetDefault("'An old foe beaten into submission..'\n Needs 2 minion slots");
        }

        public override void SetDefaults()
        {
            item.damage = 35;
            item.summon = true;
            item.mana = 10;
            item.width = 26;
            item.height = 28;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 3;
            item.rare = 11;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("BigBrain");
            item.shootSpeed = 10f;
            item.buffType = mod.BuffType("BigBrainMinion");
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 10);
        }

        public override bool CanUseItem(Player player)
        {
            return player.maxMinions >= 2;
        }

        public override void AddRecipes()
        {
            if (Fargowiltas.Instance.FargowiltasLoaded)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "BrainStaff");
                recipe.AddIngredient(ModLoader.GetMod("Fargowiltas").ItemType("EnergizerBrain"));
                recipe.AddIngredient(ItemID.LunarBar, 10);
                recipe.AddIngredient(null, "LunarCrystal", 5);

                recipe.AddTile(mod, "CrucibleCosmosSheet");
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}
