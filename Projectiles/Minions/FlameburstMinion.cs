using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Minions
{
    public class FlameburstMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flameburst Minion");
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 44;
            projectile.height = 30;
            projectile.timeLeft *= 5;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (player.active && !player.dead)
            {
                projectile.timeLeft = 2;
            }

            if (player.dead || !player.GetModPlayer<FargoPlayer>().DarkEnchant || !SoulConfig.Instance.GetValue(SoulConfig.Instance.DarkArtistMinion))
            {
                projectile.Kill();
                return;
            }

            //float above player
            projectile.position.X = player.Center.X - (float)(projectile.width / 2);
            projectile.position.Y = player.Center.Y - (float)(projectile.height / 2) + player.gfxOffY - 50f;

            //pulsation mumbo jumbo
            projectile.position.X = (float)((int)projectile.position.X);
            projectile.position.Y = (float)((int)projectile.position.Y);
            float num395 = (float)Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.2f;
            projectile.scale = num395 + 0.95f;

            //rotate towards and face mouse
            const float rotationModifier = 0.08f;

            if (Main.MouseWorld.X > projectile.Center.X)
            {
                projectile.spriteDirection = 1;

                projectile.rotation = projectile.rotation.AngleLerp(
                (new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y) - projectile.Center).ToRotation(), rotationModifier);
            }
            else
            {
                projectile.spriteDirection = -1;

                //absolute fuckery so it faces the right direction
                Vector2 target = new Vector2(Main.MouseWorld.X - (Main.MouseWorld.X - projectile.Center.X) * 2, Main.MouseWorld.Y - (Main.MouseWorld.Y - projectile.Center.Y) * 2) -projectile.Center;

                projectile.rotation = projectile.rotation.AngleLerp(
                target.ToRotation(), rotationModifier);
            }

            //4 seconds
            const float chargeTime = 240;
            if (projectile.localAI[1] > 0)
            {
                projectile.localAI[1]--;
                if (projectile.owner == Main.myPlayer)
                    projectile.netUpdate = true;
            }
            
            if (player.controlUseItem)
            {
                //charge up while attacking
                if (player.ownedProjectileCounts[mod.ProjectileType("PhantasmalDeathrayPungent")] < 1)
                {
                    projectile.localAI[0]++;
                }
                //charge level 1
                if (projectile.localAI[0] == chargeTime)
                {
                    if (projectile.owner == Main.myPlayer)
                        projectile.netUpdate = true;

                    double spread = 2 * Math.PI / 36;
                    for (int i = 0; i < 36; i++)
                    {
                        Vector2 velocity = new Vector2(2, 2).RotatedBy(spread * i);

                        int index2 = Dust.NewDust(projectile.Center, 0, 0, DustID.FlameBurst, velocity.X, velocity.Y, 100);
                        Main.dust[index2].noGravity = true;
                        Main.dust[index2].noLight = true;
                    }
                }
                //charging further
                if (projectile.localAI[0] > chargeTime)
                {
                    int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.FlameBurst, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f);
                    Main.dust[d].noGravity = true;
                }
            }
            else
            {
                //let go and fire
                if (projectile.localAI[0] > chargeTime)
                {
                    if (projectile.owner == Main.myPlayer)
                        projectile.netUpdate = true;
                    projectile.localAI[1] = 120f;
                    if (projectile.owner == Main.myPlayer)
                    {
                        Vector2 velocity = Vector2.UnitX.RotatedBy(projectile.rotation) * 6 * (projectile.spriteDirection == 1 ? 1 : -1);

                        int type = mod.ProjectileType("MegaFlameburst");

                        Projectile.NewProjectile(projectile.Center, velocity, type,
                            player.GetModPlayer<FargoPlayer>().HighestDamageTypeScaling(200), 4f, projectile.owner, projectile.whoAmI);
                        Main.PlayTrackedSound(SoundID.DD2_FlameburstTowerShot, projectile.Center);
                    }
                }
                projectile.localAI[0] = 0;
            }
        }

        public override bool CanDamage()
        {
            return false;
        }
    }
}