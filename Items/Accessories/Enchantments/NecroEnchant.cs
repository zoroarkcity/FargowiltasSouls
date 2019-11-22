using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.NPCItems;
using ThoriumMod.Items.RangedItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class NecroEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'Welcome to the bone zone' 
A Dungeon Guardian will occasionally annihilate a foe when struck by any attack
Summons a pet Skeletron Head";


        public NecroEnchant() : base("Necro Enchantment", TOOLTIP, 20, 20,
            TileID.CrystalBall, Item.sellPrice(gold: 1), ItemRarityID.Orange, new Color(86, 86, 67))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.AddTranslation(GameCulture.Chinese, "死灵魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'欢迎来到骸骨领域'
地牢守卫者偶尔会在你受到攻击时消灭敌人
召唤一个小骷髅头");
        }

        
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().NecroEffect(hideVisual);
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.NecroHelmet);
            recipe.AddIngredient(ItemID.NecroBreastplate);
            recipe.AddIngredient(ItemID.NecroGreaves);
            recipe.AddIngredient(ItemID.BoneSword);

            recipe.AddIngredient(ItemID.BoneKey);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<BoneFlayerTail>());
            recipe.AddIngredient(ModContent.ItemType<Slugger>());

            recipe.AddIngredient(ItemID.BoneGlove);
            recipe.AddIngredient(ItemID.Marrow);
            recipe.AddIngredient(ItemID.TheGuardiansGaze);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.Marrow);
            recipe.AddIngredient(ItemID.TheGuardiansGaze);
        }
    }
}
