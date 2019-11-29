using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;
using System.Collections.Generic;
using ThoriumMod.Items.Blizzard;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.NPCItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class FrostEnchant : EnchantmentItem
    {
        public FrostEnchant() : base("Frost Enchantment", "", 20, 20, 
            TileID.CrystalBall, Item.sellPrice(gold: 3), ItemRarityID.Pink, new Color(122, 189, 185))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            
            string tooltip = 
@"'Let's coat the world in a deep freeze' 
Icicles will start to appear around you
When there are three, attacking will launch them towards the cursor
Your attacks inflict Frostburn";

            string tooltip_ch =
@"'让我们给世界披上一层厚厚的冰衣'
周围将出现冰柱
当冰柱达到三个时,攻击会将它们向光标位置发射
攻击造成寒焰效果
";

            if (Fargowiltas.Instance.ThoriumLoaded)
            {
                tooltip +=
@"Summons a pet Snowman";
                tooltip_ch +=
@"召唤一个小雪人";
            }
            else
            {
                tooltip += "Summons a pet Penguin and Snowman";
                tooltip_ch += "召唤一个宠物企鹅和小雪人";
            }

            Tooltip.SetDefault(tooltip);

            DisplayName.AddTranslation(GameCulture.Chinese, "霜冻魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().FrostEffect(50, hideVisual);
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.FrostHelmet);
            recipe.AddIngredient(ItemID.FrostBreastplate);
            recipe.AddIngredient(ItemID.FrostLeggings);

            recipe.AddIngredient(ItemID.ToySled);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<BlizzardsEdge>());
            recipe.AddIngredient(ModContent.ItemType<GlacierFang>());
            recipe.AddIngredient(ModContent.ItemType<Glacieor>());
            recipe.AddIngredient(ModContent.ItemType<FreezeRay>());

            recipe.AddIngredient(ItemID.Frostbrand);
            recipe.AddIngredient(ItemID.IceBow);
        }
    }
}
