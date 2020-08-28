using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class EridanusHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eridanus Hat");
            Tooltip.SetDefault(@"5% increased damage
5% increased critical strike chance
Increases your max number of minions by 3
Increases your max number of sentries by 2");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 11;
            item.value = Item.sellPrice(0, 14);
            item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<FargoPlayer>().AllDamageUp(0.5f);
            player.GetModPlayer<FargoPlayer>().AllCritUp(5);

            player.maxMinions += 3;
            player.maxTurrets += 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<EridanusBattleplate>() && legs.type == ModContent.ItemType<EridanusLegwear>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = @"The blessing of Eridanus empowers your attacks
The empowered class changes every 20 seconds
Eridanus fights alongside you when you use the empowered class
40% increased damage for the empowered class
20% increased weapon use speed";

            FargoPlayer fargoPlayer = player.GetModPlayer<FargoPlayer>();
            fargoPlayer.EridanusEmpower = true;

            if (fargoPlayer.EridanusTimer % (60 * 20) == 1) //make dust whenever changing classes
            {
                Main.PlaySound(SoundID.Item4, player.Center);

                int type;
                switch (fargoPlayer.EridanusTimer / (60 * 20))
                {
                    case 0: type = 127; break; //solar
                    case 1: type = 229; break; //vortex
                    case 2: type = 242; break; //nebula
                    default: type = 135; break; //stardust
                }

                const int max = 100; //make some indicator dusts
                for (int i = 0; i < max; i++)
                {
                    Vector2 vector6 = Vector2.UnitY * 20f;
                    vector6 = vector6.RotatedBy((i - (max / 2 - 1)) * 6.28318548f / max) + Main.LocalPlayer.Center;
                    Vector2 vector7 = vector6 - Main.LocalPlayer.Center;
                    int d = Dust.NewDust(vector6 + vector7, 0, 0, type, 0f, 0f, 0, default(Color), 3f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity = vector7;
                }

                for (int i = 0; i < 50; i++) //make some indicator dusts
                {
                    int d = Dust.NewDust(player.position, player.width, player.height, type, 0f, 0f, 0, default(Color), 2.5f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].noLight = true;
                    Main.dust[d].velocity *= 24f;
                }
            }

            if (++fargoPlayer.EridanusTimer > 60 * 20 * 4) //handle loop
            {
                fargoPlayer.EridanusTimer = 0;
            }

            switch (fargoPlayer.EridanusTimer / (60 * 20)) //damage boost according to current class
            {
                case 0: player.meleeDamage += 0.4f; break;
                case 1: player.rangedDamage += 0.4f; break;
                case 2: player.magicDamage += 0.4f; break;
                default: player.minionDamage += 0.4f; break;
            }

            fargoPlayer.AttackSpeed += .2f;

            if (player.whoAmI == Main.myPlayer)
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Minions.EridanusMinion>()] < 1)
                {
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Minions.EridanusMinion>(), 220, 12f, player.whoAmI, -1);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Minions.EridanusRitual>()] < 1)
                {
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Minions.EridanusRitual>(), 0, 0f, player.whoAmI);
                }
            }
        }
    }
}
