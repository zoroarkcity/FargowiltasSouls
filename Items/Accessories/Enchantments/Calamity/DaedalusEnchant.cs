using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Armor;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Pets;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Calamity
{
    public class DaedalusEnchant : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("CalamityMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Daedalus Enchantment");
            Tooltip.SetDefault(
@"'Icy magic envelopes you...'
You have a 33% chance to reflect projectiles back at enemies
If you reflect a projectile you are also healed for 1/5 of that projectile's damage
Getting hit causes you to emit a blast of crystal shards
You have a 10% chance to absorb physical attacks and projectiles when hit
If you absorb an attack you are healed for 1/2 of that attack's damage
A daedalus crystal floats above you to protect you
Rogue projectiles throw out crystal shards as they travel
You can glide to negate fall damage
Effects of Scuttler's Jewel, Permafrost's Concoction, and Regenator
Summons a Bear, Kendra, and Third Sage pet");
            DisplayName.AddTranslation(GameCulture.Chinese, "代达罗斯魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'冰霜魔法保护着你...'
33%概率将抛射物反射回敌人
反射时将回复此抛射物伤害1/5的生命值
被攻击时爆发魔晶碎片
受攻击时有10%概率吸收物理攻击和抛射物
吸收时将回复此攻击伤害1/2的生命值
代达罗斯水晶将保护你
盗贼抛射物会在飞行中会射出魔晶碎片
拥有佩码·福洛斯特之融魔台和再生器的效果");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 5;
            item.value = 500000;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(64, 115, 164);
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.calamityToggles.DaedalusEffects))
            {
                calamity.Call("SetSetBonus", player, "daedalus_melee", true);
                calamity.Call("SetSetBonus", player, "daedalus_ranged", true);
                calamity.Call("SetSetBonus", player, "daedalus_magic", true);
                calamity.Call("SetSetBonus", player, "daedalus_summon", true);
                calamity.Call("SetSetBonus", player, "daedalus_rogue", true);
            }
            
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.calamityToggles.PermafrostPotion))
            {
                //permafrost concoction
                calamity.GetItem("PermafrostsConcoction").UpdateAccessory(player, hideVisual);
            }
            
            if (player.GetModPlayer<FargoPlayer>().Eternity) return;

            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.calamityToggles.DaedalusMinion) && player.whoAmI == Main.myPlayer)
            {
                if (player.FindBuffIndex(calamity.BuffType("DaedalusCrystal")) == -1)
                {
                    player.AddBuff(calamity.BuffType("DaedalusCrystal"), 3600, true);
                }
                if (player.ownedProjectileCounts[calamity.ProjectileType("DaedalusCrystal")] < 1)
                {
                    Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, calamity.ProjectileType("DaedalusCrystal"), (int)(95f * player.minionDamage), 0f, Main.myPlayer, 0f, 0f);
                }
            }

            //regenerator
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.calamityToggles.Regenerator))
                calamity.GetItem("Regenator").UpdateAccessory(player, hideVisual);

            mod.GetItem("SnowRuffianEnchant").UpdateAccessory(player, hideVisual);

            FargoPlayer fargoPlayer = player.GetModPlayer<FargoPlayer>();
            fargoPlayer.DaedalusEnchant = true;
            fargoPlayer.AddPet(SoulConfig.Instance.calamityToggles.ThirdSagePet, hideVisual, calamity.BuffType("ThirdSageBuff"), calamity.ProjectileType("ThirdSage"));
            fargoPlayer.AddPet(SoulConfig.Instance.calamityToggles.BearPet, hideVisual, calamity.BuffType("BearBuff"), calamity.ProjectileType("Bear"));
            fargoPlayer.AddPet(SoulConfig.Instance.calamityToggles.KendraPet, hideVisual, calamity.BuffType("Kendra"), calamity.ProjectileType("KendraPet"));
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddRecipeGroup("FargowiltasSouls:AnyDaedalusHelmet");
            recipe.AddIngredient(ModContent.ItemType<DaedalusBreastplate>());
            recipe.AddIngredient(ModContent.ItemType<DaedalusLeggings>());
            recipe.AddIngredient(ModContent.ItemType<SnowRuffianEnchant>());
            recipe.AddIngredient(ModContent.ItemType<PermafrostsConcoction>());
            recipe.AddIngredient(ModContent.ItemType<Regenator>());
            recipe.AddIngredient(ModContent.ItemType<CrystalBlade>());
            recipe.AddIngredient(ModContent.ItemType<KelvinCatalyst>());
            recipe.AddIngredient(ModContent.ItemType<SlagMagnum>());
            recipe.AddIngredient(ModContent.ItemType<Arbalest>());
            recipe.AddIngredient(ModContent.ItemType<SHPC>());
            recipe.AddIngredient(calamity.ItemType("ColdDivinity"));//e
            recipe.AddIngredient(ModContent.ItemType<IbarakiBox>());
            recipe.AddIngredient(ModContent.ItemType<PrimroseKeepsake>());

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
