using FargowiltasSouls.Buffs.Souls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Souls
{
    public class GrowingPumpkin : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.aiStyle = -1;
            projectile.tileCollide = true;

            Main.projFrames[projectile.type] = 5;
        }

        public override void AI()
        {
            projectile.velocity.Y = projectile.velocity.Y + 0.2f;
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }

            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.active && !npc.friendly && npc.Hitbox.Intersects(projectile.Hitbox))
                {
                    Player player = Main.player[projectile.owner];
                    FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

                    //bonus damage if fully grown
                    int damage = projectile.frame == 4 ? 50 : 15;

                    if (modPlayer.LifeForce || modPlayer.WizardEnchant)
                    {
                        damage *= 2;
                    }

                    npc.StrikeNPC(modPlayer.HighestDamageTypeScaling(damage), 1, 0);
                    projectile.Kill();
                }
            }


            if (projectile.frame != 4)
            {
                projectile.frameCounter++;
                if (projectile.frameCounter >= 60)
                {
                    projectile.frameCounter = 0;
                    projectile.frame = (projectile.frame + 1) % 5;

                    //dust
                    if (projectile.frame == 4)
                    {
                        const int max = 16;
                        for (int i = 0; i < max; i++)
                        {
                            Vector2 vector6 = Vector2.UnitY * 5f;
                            vector6 = vector6.RotatedBy((i - (max / 2 - 1)) * 6.28318548f / max) + projectile.Center;
                            Vector2 vector7 = vector6 - projectile.Center;
                            int d = Dust.NewDust(vector6 + vector7, 0, 0, DustID.Grass, 0f, 0f, 0, default(Color), 1.5f);
                            Main.dust[d].noGravity = true;
                            Main.dust[d].velocity = vector7;
                        }
                    }
                }
            }
            else
            {
                Player player = Main.player[projectile.owner];
                FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

                if (player.Hitbox.Intersects(projectile.Hitbox))
                {
                    int heal = 15;

                    if (modPlayer.LifeForce || modPlayer.WizardEnchant)
                    {
                        heal *= 2;
                    }

                    player.statLife += heal;
                    player.HealEffect(heal);
                    projectile.Kill();
                }
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.position += projectile.velocity;
            projectile.velocity = Vector2.Zero;
            return false;
        }

        public override void Kill(int timeLeft)
        {
            const int max = 16;
            for (int i = 0; i < max; i++)
            {
                Vector2 vector6 = Vector2.UnitY * 5f;
                vector6 = vector6.RotatedBy((i - (max / 2 - 1)) * 6.28318548f / max) + projectile.Center;
                Vector2 vector7 = vector6 - projectile.Center;
                int d = Dust.NewDust(vector6 + vector7, 0, 0, DustID.Grass, 0f, 0f, 0, default(Color), 1.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = vector7;
            }
        }
    }
}