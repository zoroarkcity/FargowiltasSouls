using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.NPCs;
using Terraria.Localization;
using System.Collections.Generic;
using ThoriumMod.Buffs.Healer;
using ThoriumMod.Items.DD;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.HealerItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class DarkArtistEnchant : EnchantmentItem
    {
        public DarkArtistEnchant() : base("Dark Artist Enchantment", "", 20, 20, 
            TileID.CrystalBall, Item.sellPrice(gold: 5), ItemRarityID.Yellow, new Color(155, 92, 176))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            
            string tooltip =
@"'The shadows hold more than they seem'
While attacking, Flameburst shots manifest themselves from your shadows
Greatly enhances Flameburst effectiveness
";
            string tooltip_ch =
@"'阴影比看起来更多'
攻击时, 焰爆炮塔的射击会从你的阴影中显现出来
大大增强焰爆炮塔能力
";

            if(Fargowiltas.Instance.ThoriumLoaded)
            {
                tooltip += "Effects of Dark Effigy\n";
                tooltip_ch += "拥有阴影雕塑的效果\n";
            }

            tooltip += "Summons a pet Flickerwick";
            tooltip_ch += "召唤一个闪烁烛芯";

            Tooltip.SetDefault(tooltip);

            DisplayName.AddTranslation(GameCulture.Chinese, "暗黑艺术家魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().DarkArtistEffect(hideVisual);

            if (Fargowiltas.Instance.ThoriumLoaded) Thorium(player);
        }

        private void Thorium(Player player)
        {
            //dark effigy
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();

            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && (npc.shadowFlame || npc.GetGlobalNPC<ThoriumGlobalNPC>().lightLament) && npc.DistanceSQ(player.Center) < 1000000f)
                {
                    thoriumPlayer.effigy++;
                }
            }
            if (thoriumPlayer.effigy > 0)
            {
                player.AddBuff(ModContent.BuffType< EffigyRegen>(), 2, true);
            }
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.ApprenticeAltHead);
            recipe.AddIngredient(ItemID.ApprenticeAltShirt);
            recipe.AddIngredient(ItemID.ApprenticeAltPants);

            recipe.AddIngredient(ItemID.DD2PetGhost);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<Effigy>());
            recipe.AddIngredient(ModContent.ItemType<DarkMageStaff>());
            recipe.AddIngredient(ModContent.ItemType<ShadowFlareBow>());

            recipe.AddIngredient(ItemID.DD2FlameburstTowerT3Popper);
            recipe.AddIngredient(ItemID.ShadowFlameHexDoll);
            recipe.AddIngredient(ItemID.InfernoFork);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.DD2FlameburstTowerT3Popper);
            recipe.AddIngredient(ItemID.ShadowFlameHexDoll);
            recipe.AddIngredient(ItemID.InfernoFork);
        }
    }
}
