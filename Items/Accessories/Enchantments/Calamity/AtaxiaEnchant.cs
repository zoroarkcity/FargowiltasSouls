using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments.Calamity
{
    public class AtaxiaEnchant : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool Autoload(ref string name)
        {
            return ModLoader.GetMod("CalamityMod") != null;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ataxia Enchantment");
            Tooltip.SetDefault(
@"'Not be confused with Ataraxia Enchantment'
You have a 20% chance to emit a blazing explosion on hit
Melee attacks and projectiles cause chaos flames to erupt on enemy hits
You have a 50% chance to fire a homing chaos flare when using ranged weapons
Magic attacks summon damaging and healing flare orbs on hit
Summons a chaos spirit to protect you
Rogue weapons have a 10% chance to unleash a volley of chaos flames around the player
Effects of the Plague Hive
Summons a Brimling pet");
            DisplayName.AddTranslation(GameCulture.Chinese, "阿塔西亚魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'别和禅心搞混了'
攻击有20%概率释放炎爆
近战攻击和抛射物会对敌人造成混乱之火
使用远程武器时有50%概率发射追踪的混乱之火
魔法攻击召唤治愈和伤害的火球
召唤混乱之灵保护你
盗贼武器有10%概率在玩家周围释放一串混乱之火
拥有瘟疫蜂巢的效果");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 8;
            item.value = 1000000;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(194, 89, 89);
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            if (SoulConfig.Instance.GetValue("Ataxia Effects"))
            {
                calamity.Call("SetSetBonus", player, "ataxia", true);
                calamity.Call("SetSetBonus", player, "ataxia_melee", true);
                calamity.Call("SetSetBonus", player, "ataxia_ranged", true);
                calamity.Call("SetSetBonus", player, "ataxia_magic", true);
                calamity.Call("SetSetBonus", player, "ataxia_rogue", true);
            }
            
            if (SoulConfig.Instance.GetValue("Plague Hive"))
            {
                calamity.GetItem("PlagueHive").UpdateAccessory(player, hideVisual);
            }
            
            if (player.GetModPlayer<FargoPlayer>().Eternity) return;

            if (SoulConfig.Instance.GetValue("Chaos Spirit Minion"))
            {
                //summon
                calamity.Call("SetSetBonus", player, "ataxia_summon", true);
                if (player.whoAmI == Main.myPlayer)
                {
                    if (player.FindBuffIndex(calamity.BuffType("ChaosSpirit")) == -1)
                    {
                        player.AddBuff(calamity.BuffType("ChaosSpirit"), 3600, true);
                    }
                    if (player.ownedProjectileCounts[calamity.ProjectileType("ChaosSpirit")] < 1)
                    {
                        Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, calamity.ProjectileType("ChaosSpirit"), (int)(190f * player.minionDamage), 0f, Main.myPlayer, 0f, 0f);
                    }
                }
            }

            FargoPlayer fargoPlayer = player.GetModPlayer<FargoPlayer>();
            fargoPlayer.AtaxiaEnchant = true;
            fargoPlayer.AddPet("Brimling Pet", hideVisual, calamity.BuffType("BrimlingBuff"), calamity.ProjectileType("BrimlingPet"));
        }

        public override void AddRecipes()
        {
            if (!Fargowiltas.Instance.CalamityLoaded) return;

            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddRecipeGroup("FargowiltasSouls:AnyAtaxiaHelmet");
            recipe.AddIngredient(calamity.ItemType("AtaxiaArmor"));
            recipe.AddIngredient(calamity.ItemType("AtaxiaSubligar"));
            recipe.AddIngredient(calamity.ItemType("PlagueHive"));
            recipe.AddIngredient(calamity.ItemType("SpearofDestiny"));
            recipe.AddIngredient(calamity.ItemType("Hellborn"));
            recipe.AddIngredient(calamity.ItemType("Lucrecia"));
            recipe.AddIngredient(calamity.ItemType("BarracudaGun"));
            recipe.AddIngredient(calamity.ItemType("Vesuvius"));
            recipe.AddIngredient(calamity.ItemType("LeadWizard"));
            recipe.AddIngredient(calamity.ItemType("Malachite"));
            recipe.AddIngredient(calamity.ItemType("Impaler"));
            recipe.AddIngredient(calamity.ItemType("HolidayHalberd"));
            recipe.AddIngredient(calamity.ItemType("CharredRelic"));

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
