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
            Tooltip.SetDefault("Uses darts for ammo\n50% chance to not consume ammo\n'Repurposed from the abdomen of a defeated foe..'");
        }

        public override void SetDefaults()
        {
            item.damage = 39;
            item.crit = 0;
            item.ranged = true;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1.5f;
            item.value = 50000;
            item.rare = 3;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<Projectiles.BossWeapons.SmallStinger>();
            item.useAmmo = AmmoID.Dart;
            item.UseSound = SoundID.Item97;
            item.shootSpeed = 40f;
            item.width = 44;
            item.height = 16;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = ModContent.ProjectileType<Projectiles.BossWeapons.SmallStinger>();
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.mod == "Terraria" && line2.Name == "CritChance")
                {
                    line2.text = ""; //dont show crit chance line, because custom crit method
                }
            }
        }

        //make them hold it different
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.Next(2) == 0;
        }
    }
}
