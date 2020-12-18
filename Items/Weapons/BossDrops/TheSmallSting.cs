using FargowiltasSouls.Projectiles.BossWeapons;
using FargowiltasSouls.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Weapons.BossDrops
{
    public class TheSmallSting : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Small Sting");
            Tooltip.SetDefault("Uses darts for ammo" +
                "\n50% chance to not consume ammo" +
                "\n'Repurposed from the abdomen of a defeated foe..'");
        }

        public override void SetDefaults()
        {
            item.damage = 36;
            item.crit = 0;
            item.ranged = true;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1.5f;
            item.value = 50000;
            item.rare = ItemRarityID.Orange;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<SmallStinger>();
            item.useAmmo = AmmoID.Dart;
            item.UseSound = SoundID.Item97;
            item.shootSpeed = 40f;
            item.width = 44;
            item.height = 16;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = ModContent.ProjectileType<SmallStinger>();

            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Remove the Crit Chance line because of a custom crit method
            tooltips.Remove(tooltips.FirstOrDefault(line => line.Name == "CritChance" && line.mod == "Terraria"));

            tooltips.FirstOrDefault(line => line.Name == "ItemName" && line.mod == "Terraria").ArticlePrefixAdjustment(item.prefix, new string[1] { "The" });
        }

        //make them hold it different
        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool ConsumeAmmo(Player player) => Main.rand.Next(2) == 0;
    }
}