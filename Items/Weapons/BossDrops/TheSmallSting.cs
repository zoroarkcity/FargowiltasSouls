using Microsoft.Xna.Framework;
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
            item.damage = 36;
            item.crit += 15;
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

        //make them hold it different
        /*public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }*/

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.Next(2) == 0;
        }
    }
}



//Uses any type of bullets as ammunition, though it will only inherit the damage bonuses of any ammo it uses.
//Fires a fast, high-damage stinger, but has about the use speed of the Sniper Rifle.
//Fired stingers ignore all enemy defense; this trait carries over across both upgrades.
