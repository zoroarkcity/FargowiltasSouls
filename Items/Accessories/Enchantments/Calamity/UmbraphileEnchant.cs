using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Calamity
{
    public class UmbraphileEnchant : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool Autoload(ref string name)
        {
            return false;//ModLoader.GetMod("CalamityMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Umbraphile Enchantment");
            Tooltip.SetDefault(
@":HeyMF:");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 7;
            item.value = 400000;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(70, 63, 69);
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            /*if (!Fargowiltas.Instance.CalamityLoaded) return;

            //all
            calamity.Call("SetSetBonus", player, "victide", true);

            if (player.GetModPlayer<FargoPlayer>().Eternity) return;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.calamityToggles.UrchinMinion))
            {
                //summon
                calamity.Call("SetSetBonus", player, "victide_summon", true);
                if (player.whoAmI == Main.myPlayer)
                {
                    if (player.FindBuffIndex(calamity.BuffType("Urchin")) == -1)
                    {
                        player.AddBuff(calamity.BuffType("Urchin"), 3600, true);
                    }
                    if (player.ownedProjectileCounts[calamity.ProjectileType("Urchin")] < 1)
                    {
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, calamity.ProjectileType("Urchin"), (int)(7f * player.minionDamage), 0f, Main.myPlayer, 0f, 0f);
                    }
                }
            }

            calamity.GetItem("DeepDiver").UpdateAccessory(player, hideVisual);
            calamity.GetItem("TheTransformer").UpdateAccessory(player, hideVisual);
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.calamityToggles.LuxorGift))
                calamity.GetItem("LuxorsGift").UpdateAccessory(player, hideVisual);*/
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(calamity.ItemType("UmbraphileHood"));
            recipe.AddIngredient(calamity.ItemType("UmbraphileRegalia"));
            recipe.AddIngredient(calamity.ItemType("UmbraphileBoots"));
            recipe.AddIngredient(calamity.ItemType("VampiricTalisman"));
            recipe.AddIngredient(calamity.ItemType("MomentumCapacitor"));
            recipe.AddIngredient(calamity.ItemType("Equanimity"));
            recipe.AddIngredient(calamity.ItemType("Brimblade"));
            recipe.AddIngredient(calamity.ItemType("DeepWounder"));
            recipe.AddIngredient(calamity.ItemType("TotalityBreakers"), 300);
            recipe.AddIngredient(calamity.ItemType("FantasyTalisman"), 300);
            recipe.AddIngredient(calamity.ItemType("DefectiveSpheres"), 5);
            recipe.AddIngredient(calamity.ItemType("TheSyringe"));
            recipe.AddIngredient(calamity.ItemType("CorpusAvertor"));
            recipe.AddIngredient(calamity.ItemType("TerrorTalons"));
            
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
