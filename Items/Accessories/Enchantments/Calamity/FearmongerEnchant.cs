using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Armor;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Magic;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Calamity
{
    public class FearmongerEnchant : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("CalamityMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fearmonger Enchantment");
            Tooltip.SetDefault(
@"''
Minions deal full damage while wielding weaponry
All minion attacks grant colossal life regeneration
15% increased damage reduction during the Pumpkin and Frost Moons
This extra damage reduction ignores the soft cap
Provides cold protection in Death Mode
Effects of The Evolution, Spectral Veil, and Statis' Void Sash");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 10;
            item.value = 750000;
        }

        /*public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(70, 63, 69);
                }
            }
        }*/

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            calamity.Call("SetSetBonus", player, "fearmonger", true);
            calamity.GetItem("TheEvolution").UpdateAccessory(player, hideVisual);
            calamity.GetItem("SpectralVeil").UpdateAccessory(player, hideVisual);
            calamity.GetItem("StatisBeltOfCurses").UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ModContent.ItemType<FearmongerGreathelm>());
            recipe.AddIngredient(ModContent.ItemType<FearmongerPlateMail>());
            recipe.AddIngredient(ModContent.ItemType<FearmongerGreaves>());
            recipe.AddIngredient(ModContent.ItemType<TheEvolution>());
            recipe.AddIngredient(ModContent.ItemType<SpectralVeil>());
            recipe.AddIngredient(ModContent.ItemType<StatisBeltOfCurses>());
            recipe.AddIngredient(ModContent.ItemType<RecitationoftheBeast>());
            recipe.AddIngredient(ModContent.ItemType<Karasawa>());
            recipe.AddIngredient(ModContent.ItemType<FaceMelter>());
            recipe.AddIngredient(ModContent.ItemType<SoulPiercer>());
            recipe.AddIngredient(ModContent.ItemType<GodsParanoia>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Penumbra>());
            recipe.AddIngredient(ModContent.ItemType<Hypothermia>());
            recipe.AddIngredient(ModContent.ItemType<IceBarrage>());

            recipe.AddTile(calamity, "DraedonsForge");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
