using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Armor;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.CalPlayer;
using CalamityMod;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Pets;
using CalamityMod.Buffs.Pets;
using CalamityMod.Projectiles.Pets;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Calamity
{
    public class SulphurousEnchant : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("CalamityMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sulphurous Enchantment");
            Tooltip.SetDefault(
@"''
Attacking and being attacked by enemies inflicts poison
Grants a sulphurous bubble jump that applies venom on hit
Slightly reduces breath loss in the abyss
Effects of Sand Cloak and Alluring Bait
Summons a Danny Devito and Radiator pet");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 2;
            item.value = 50000;
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

            CalamityPlayer calamityPlayer = player.Calamity();
            calamityPlayer.sulfurSet = true;
            player.doubleJumpSandstorm = true;
            //calamity.Call("SetSetBonus", player, "sulphur", true); hopefully soon
            calamity.GetItem("SandCloak").UpdateAccessory(player, hideVisual);
            calamity.GetItem("AlluringBait").UpdateAccessory(player, hideVisual);

            //pets
            FargoPlayer fargoPlayer = player.GetModPlayer<FargoPlayer>();
            fargoPlayer.SulphurEnchant = true;
            fargoPlayer.AddPet(SoulConfig.Instance.calamityToggles.DannyPet, hideVisual, ModContent.BuffType<DannyDevito>(), ModContent.ProjectileType<DannyDevitoPet>());
            fargoPlayer.AddPet(SoulConfig.Instance.calamityToggles.RadiatorPet, hideVisual, ModContent.BuffType<RadiatorBuff>(), ModContent.ProjectileType<RadiatorPet>());
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ModContent.ItemType<SulfurHelmet>());
            recipe.AddIngredient(ModContent.ItemType<SulfurBreastplate>());
            recipe.AddIngredient(ModContent.ItemType<SulfurLeggings>());
            recipe.AddIngredient(ModContent.ItemType<SandCloak>());
            recipe.AddIngredient(ModContent.ItemType<AlluringBait>());
            recipe.AddIngredient(ModContent.ItemType<DuneHopper>());
            recipe.AddIngredient(ModContent.ItemType<UrchinStinger>(), 300);
            recipe.AddIngredient(ModContent.ItemType<CausticCroakerStaff>());
            recipe.AddIngredient(ModContent.ItemType<TrashmanTrashcan>());
            recipe.AddIngredient(ModContent.ItemType<RadiatingCrystal>());

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
