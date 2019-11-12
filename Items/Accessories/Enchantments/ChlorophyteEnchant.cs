using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;
using System.Collections.Generic;
using FargowiltasSouls.Items.Accessories.Enchantments.Thorium;
using ThoriumMod.Items.ArcaneArmor;
using ThoriumMod.Items.ThrownItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class ChlorophyteEnchant : EnchantmentItem
    {
        public int timer;


        public ChlorophyteEnchant() : base("Chlorophyte Enchantment", "", 20, 20,
            TileID.CrystalBall, Item.sellPrice(gold: 3), ItemRarityID.Lime, new Color(36, 137, 0))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            string tooltip =
@"'The jungle's essence crystallizes around you'
Summons a ring of leaf crystals to shoot at nearby enemies
Taking damage will release a lingering spore explosion
All herb collection is doubled
";
            string tooltip_ch = 
@"'丛林的精华凝结在你周围'
召唤一圈叶绿水晶射击附近的敌人
受到伤害时会释放出有毒的孢子爆炸
所有草药收获翻倍
";

            if(Fargowiltas.Instance.ThoriumLoaded)
            {
                tooltip +=
@"Your attacks have a chance to poison hit enemies with a spore cloud
Effects of Night Shade Petal, Petal Shield, Toxic Subwoofer, and Flower Boots
";
                tooltip_ch +=
@"攻击有概率释放孢子云使敌人中毒
拥有影缀花, 花之盾, 剧毒音箱和花之靴的效果
";
            }
            else
            {
                tooltip += "Effects of Guide to Plant Fiber Cordage and Flower Boots\n";
                tooltip_ch += "拥有植物纤维绳索指南的效果\n";
            }

            tooltip += "Summons a pet Seedling";
            tooltip_ch += "召唤一颗宠物幼苗";

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "叶绿魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            //crystal and pet
            modPlayer.ChloroEffect(hideVisual, 100);
            //herb double and bulb effect with thorium
            modPlayer.ChloroEnchant = true;
            modPlayer.FlowerBoots();
            modPlayer.JungleEffect();

            if (Fargowiltas.Instance.ThoriumLoaded) Thorium(player, hideVisual);
        }

        private void Thorium(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();

            //subwoofer
            /*thoriumPlayer.bardRangeBoost += 450;
            for (int i = 0; i < 255; i++)
            {
                Player player2 = Main.player[i];
                if (player2.active && !player2.dead && Vector2.Distance(player2.Center, player.Center) < 450f)
                {
                    thoriumPlayer.empowerPoison = true;
                }
            }*/
            //bulb

            modPlayer.BulbEnchant = true;

            //petal shield
            ModContent.GetInstance<PetalShield>().UpdateAccessory(player, hideVisual);
            player.statDefense -= 2;

            //night shade petal
            thoriumPlayer.nightshadeBoost = true;
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddRecipeGroup("FargowiltasSouls:AnyChloroHead");
            recipe.AddIngredient(ItemID.ChlorophytePlateMail);
            recipe.AddIngredient(ItemID.ChlorophyteGreaves);
            recipe.AddIngredient(ModContent.ItemType<JungleEnchant>());

            recipe.AddIngredient(ItemID.Seedling);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<BulbEnchant>());
            recipe.AddIngredient(ItemID.FlowerBoots);
            recipe.AddIngredient(ItemID.StaffofRegrowth);
            recipe.AddIngredient(ItemID.LeafBlower);
            recipe.AddIngredient(ModContent.ItemType<BudBomb>(), 300);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.FlowerBoots);
            recipe.AddIngredient(ItemID.StaffofRegrowth);
        }
    }
}
