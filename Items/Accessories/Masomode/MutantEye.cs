using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Accessories.Masomode
{
    public class MutantEye : ModItem
    {
        public override string Texture => "FargowiltasSouls/Items/Placeholder";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mutant Eye");
            Tooltip.SetDefault(@"'Only a little suspicious'
Grants immunity to Mutant Fang
Increases graze bonus cap to 100% increased critical damage
Increases critical damage gained per graze
Enables Spectral Abominationn even when toggles are sealed
Increases Spectral Abominationn respawn rate and damage
Press the Mutant Bomb key to unleash a wave of spheres and destroy most hostile projectiles
Mutant Bomb has a 60 second cooldown");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.rare = 11;
            item.value = Item.sellPrice(1);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = new Color(Main.DiscoR, 51, 255 - (int)(Main.DiscoR * 0.4));
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer fargoPlayer = player.GetModPlayer<FargoPlayer>();

            player.buffImmune[mod.BuffType("MutantFang")] = true;

            fargoPlayer.MutantEye = true;
            if (!hideVisual)
                fargoPlayer.MutantEyeVisual = true;

            if (fargoPlayer.MutantEyeCD > 0)
            {
                fargoPlayer.MutantEyeCD--;

                if (fargoPlayer.MutantEyeCD == 0)
                {
                    Main.PlaySound(SoundID.Item4, player.Center);

                    const int max = 50; //make some indicator dusts
                    for (int i = 0; i < max; i++)
                    {
                        Vector2 vector6 = Vector2.UnitY * 8f;
                        vector6 = vector6.RotatedBy((i - (max / 2 - 1)) * 6.28318548f / max) + Main.LocalPlayer.Center;
                        Vector2 vector7 = vector6 - Main.LocalPlayer.Center;
                        int d = Dust.NewDust(vector6 + vector7, 0, 0, 229, 0f, 0f, 0, default(Color), 2f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity = vector7;
                    }

                    for (int i = 0; i < 30; i++)
                    {
                        int d = Dust.NewDust(player.position, player.width, player.height, 229, 0f, 0f, 0, default(Color), 2.5f);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].noLight = true;
                        Main.dust[d].velocity *= 8f;
                    }
                }
            }

            if (player.whoAmI == Main.myPlayer && fargoPlayer.MutantEyeVisual && fargoPlayer.MutantEyeCD <= 0
                && player.ownedProjectileCounts[mod.ProjectileType("PhantasmalRing2")] <= 0)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("PhantasmalRing2"), 0, 0f, Main.myPlayer);
            }

            if (fargoPlayer.CyclonicFinCD > 0)
                fargoPlayer.CyclonicFinCD--;
        }
    }
}
