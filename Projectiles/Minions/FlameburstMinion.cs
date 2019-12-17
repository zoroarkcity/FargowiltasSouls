using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Minions
{
    public class FlameburstMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pungent Eyeball");
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
            if (player.active && !player.dead && player.GetModPlayer<FargoPlayer>().DarkEnchant)
            {
                projectile.timeLeft = 2;
            }

            //float above player
            projectile.position.X = player.Center.X - (float)(projectile.width / 2);
            projectile.position.Y = player.Center.Y - (float)(projectile.height / 2) + player.gfxOffY - 60f;

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

            //3 seconds
            const float chargeTime = 180f;
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
                    const int num226 = 12;
                    for (int i = 0; i < num226; i++)
                    {
                        Vector2 vector6 = Vector2.UnitX.RotatedBy(projectile.rotation) * 6f;
                        vector6 = vector6.RotatedBy(((i - (num226 / 2 - 1)) * 6.28318548f / num226), default(Vector2)) + projectile.Center;
                        Vector2 vector7 = vector6 - projectile.Center;
                        int num228 = Dust.NewDust(vector6 + vector7, 0, 0, DustID.FlameBurst, 0f, 0f, 0, default(Color), 1.5f);
                        Main.dust[num228].noGravity = true;
                        Main.dust[num228].velocity = vector7;
                    }
                }
                //charging further
                if (projectile.localAI[0] > chargeTime)
                {
                    int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.FlameBurst, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f);
                    Main.dust[d].noGravity = true;
                }
                //charge level 2
                if (projectile.localAI[0] == chargeTime * 2f)
                {
                    if (projectile.owner == Main.myPlayer)
                        projectile.netUpdate = true;
                    const int num226 = 24; //dusts indicate charged up
                    for (int i = 0; i < num226; i++)
                    {
                        Vector2 vector6 = Vector2.UnitX.RotatedBy(projectile.rotation) * 9f;
                        vector6 = vector6.RotatedBy(((i - (num226 / 2 - 1)) * 6.28318548f / num226), default(Vector2)) + projectile.Center;
                        Vector2 vector7 = vector6 - projectile.Center;
                        int num228 = Dust.NewDust(vector6 + vector7, 0, 0, DustID.FlameBurst, 0f, 0f, 0, default(Color), 2.5f);
                        Main.dust[num228].noGravity = true;
                        Main.dust[num228].velocity = vector7;
                    }
                }
                //fully charged
                if (projectile.localAI[0] > chargeTime * 2f)
                {
                    int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.FlameBurst, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].scale += 0.5f;
                    d = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.FlameBurst, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f);
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
                        Vector2 velocity = Vector2.UnitX.RotatedBy(projectile.rotation) * 15 * (projectile.spriteDirection == 1 ? 1 : -1);

                        //add another custom fireball

                        int type = (projectile.localAI[0] >= chargeTime * 2f) ? mod.ProjectileType("LunarCultistFireball") : ProjectileID.DD2FlameBurstTowerT3Shot;

                        Projectile.NewProjectile(projectile.Center, velocity, type,
                            projectile.damage, 4f, projectile.owner, projectile.whoAmI);
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