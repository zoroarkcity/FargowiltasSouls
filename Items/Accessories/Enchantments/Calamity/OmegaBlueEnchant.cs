using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.CalPlayer;
using Terraria.Localization;
using System.Collections.Generic;
using CalamityMod.Items.Armor;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Calamity
{
    public class OmegaBlueEnchant : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("CalamityMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Omega Blue Enchantment");
            Tooltip.SetDefault(
@"'The darkness of the Abyss has overwhelmed you...'
Increases armor penetration by 100
Short-ranged tentacles heal you by sucking enemy life
Press Y to activate abyssal madness for 5 seconds
Abyssal madness increases damage, critical strike chance, and tentacle aggression/range
This effect has a 30 second cooldown
Two shellfishes aid you in combat
When using any weapon you have a 10% chance to throw a returning seashell projectile
Summons a sea urchin to protect you
10% increased minion damage while submerged in liquid
Provides a moderate amount of light and moderately reduces breath loss in the abyss
Attacking and being attacked by enemies inflicts poison
Grants a sulphurous bubble jump that applies venom on hit
Effects of the Abyssal Diving Suit, Mutated Truffle, and Old Duke's Scales
Effects of Giant Pearl and Amidias' Pendant
Effects of Aquatic Emblem and Enchanted Pearl
Effects of Ocean's Crest, Deep Diver, The Transformer, and Luxor's Gift
Effects of Corrosive Spine and Lumenous Amulet
Effects of Sand Cloak and Alluring Bait
Summons several pets");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 13;
            item.value = 1000000;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(35, 95, 161);
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.calamityToggles.OmegaTentacles))
            {
                calamity.Call("SetSetBonus", player, "omegablue", true);
                CalamityPlayer modPlayer = player.GetModPlayer<CalamityPlayer>();
                
                if (modPlayer.omegaBlueCooldown > 0)
                {
                    if (modPlayer.omegaBlueCooldown == 1)
                    {
                        for (int i = 0; i < 66; i++)
                        {
                            int num = Dust.NewDust(player.position, player.width, player.height, 20, 0f, 0f, 100, Color.Transparent, 2.6f);
                            Main.dust[num].noGravity = true;
                            Main.dust[num].noLight = true;
                            Main.dust[num].fadeIn = 1f;
                            Main.dust[num].velocity *= 6.6f;
                        }
                    }
                    modPlayer.omegaBlueCooldown--;
                }
                if (modPlayer.omegaBlueCooldown > 1500)
                {
                    modPlayer.omegaBlueHentai = true;
                    int num2 = Dust.NewDust(player.position, player.width, player.height, 20, 0f, 0f, 100, Color.Transparent, 1.6f);
                    Main.dust[num2].noGravity = true;
                    Main.dust[num2].noLight = true;
                    Main.dust[num2].fadeIn = 1f;
                    Main.dust[num2].velocity *= 3f;
                }
            }

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.calamityToggles.DivingSuit))
            {
                calamity.GetItem("AbyssalDivingSuit").UpdateAccessory(player, hideVisual);
            }

            if (SoulConfig.Instance.calamityToggles.ReaperToothNecklace)
            {
                calamity.GetItem("ReaperToothNecklace").UpdateAccessory(player, hideVisual);
            }

            if (SoulConfig.Instance.calamityToggles.MutatedTruffle)
            {
                calamity.GetItem("MutatedTruffle").UpdateAccessory(player, hideVisual);
            }

            calamity.GetItem("DukeScales").UpdateAccessory(player, hideVisual);

            mod.GetItem("MolluskEnchant").UpdateAccessory(player, hideVisual);
            mod.GetItem("FathomSwarmerEnchant").UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ModContent.ItemType<OmegaBlueHelmet>());
            recipe.AddIngredient(ModContent.ItemType<OmegaBlueChestplate>());
            recipe.AddIngredient(ModContent.ItemType<OmegaBlueLeggings>());
            recipe.AddIngredient(ModContent.ItemType<MolluskEnchant>());
            recipe.AddIngredient(ModContent.ItemType<FathomSwarmerEnchant>());
            recipe.AddIngredient(ModContent.ItemType<AbyssalDivingSuit>());
            recipe.AddIngredient(ModContent.ItemType<ReaperToothNecklace>());
            recipe.AddIngredient(ModContent.ItemType<MutatedTruffle>());
            recipe.AddIngredient(ModContent.ItemType<DukeScales>());
            recipe.AddIngredient(ModContent.ItemType<HalibutCannon>());
            recipe.AddIngredient(ModContent.ItemType<CalamarisLament>());
            recipe.AddIngredient(ModContent.ItemType<TheReaper>());
            recipe.AddIngredient(ModContent.ItemType<SulphuricAcidCannon>());
            recipe.AddIngredient(ModContent.ItemType<SDFMG>());
            
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
