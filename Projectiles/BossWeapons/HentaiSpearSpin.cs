using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.Buffs.Masomode;

namespace FargowiltasSouls.Projectiles.BossWeapons
{
    public class HentaiSpearSpin : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Penetrator");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 164;
            projectile.height = 164;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1.3f;
            projectile.hide = true;
            projectile.melee = true;
            projectile.alpha = 0;
            projectile.timeLeft = 2;
        }

        float scaletimer;
        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter);
            projectile.direction = player.direction;
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            projectile.Center = ownerMountedCenter;
            projectile.timeLeft = 2;

            if (projectile.owner == Main.myPlayer && !player.controlUseItem)
                projectile.Kill();

            if (player.dead || !player.active)
                projectile.Kill();

            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
            projectile.rotation += (float)Math.PI / 6.85f * player.direction;
            projectile.ai[0] += MathHelper.Pi/45;
            projectile.velocity = projectile.rotation.ToRotationVector2();
            projectile.position -= projectile.velocity;
            player.itemRotation = projectile.rotation;
            player.itemRotation = MathHelper.WrapAngle(player.itemRotation);

            if (++projectile.localAI[0] > 10)
            {
                projectile.localAI[0] = 0;
                Main.PlaySound(SoundID.Item1, projectile.Center);
                if (projectile.owner == Main.myPlayer)
                {
                    Vector2 speed = -Vector2.UnitY.RotatedByRandom(Math.PI / 2) * Main.rand.NextFloat(14f, 20f);
                    float ai1 = Main.rand.Next(30, 60);
                    Projectile.NewProjectile(projectile.position + Main.rand.NextVector2Square(0f, projectile.width),
                        speed, ModContent.ProjectileType<PhantasmalEyeHoming>(), projectile.damage, 0f, projectile.owner, -1, ai1);
                }
            }

            scaletimer++;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 1; //balanceing

            if (projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(target.position + new Vector2(Main.rand.Next(target.width), Main.rand.Next(target.height)),
                    Vector2.Zero, ModContent.ProjectileType<PhantasmalBlast>(), projectile.damage, projectile.knockBack * 3f, projectile.owner);
                Projectile.NewProjectile(target.position + new Vector2(Main.rand.Next(target.width), Main.rand.Next(target.height)),
                    Vector2.Zero, ModContent.ProjectileType<PhantasmalBlast>(), projectile.damage, projectile.knockBack * 3f, projectile.owner);
                Projectile.NewProjectile(target.position + new Vector2(Main.rand.Next(target.width), Main.rand.Next(target.height)),
                    Vector2.Zero, ModContent.ProjectileType<PhantasmalBlast>(), projectile.damage, projectile.knockBack * 3f, projectile.owner);
            }
            target.AddBuff(ModContent.BuffType<CurseoftheMoon>(), 600);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * projectile.Opacity;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Color color26 = lightColor;
            color26 = projectile.GetAlpha(color26);

            for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i += 0.1f)
            {
                Player player = Main.player[projectile.owner];
                Texture2D glow = mod.GetTexture("Projectiles/BossWeapons/HentaiSpearSpinGlow");
                Color color27 = Color.Lerp(new Color(51, 255, 191, 210), Color.Transparent, (float)Math.Cos(projectile.ai[0]) / 3 + 0.3f);
                float scale = projectile.scale - (float)Math.Cos(projectile.ai[0]) / 5;
                scale *= (float)(ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[projectile.type];
                int max0 = Math.Max((int)i - 1, 0);
                Vector2 center = Vector2.Lerp(projectile.oldPos[(int)i], projectile.oldPos[max0], (1 - i % 1));
                float smoothtrail = i % 1 * (float)Math.PI / 6.85f;
                bool withinangle = projectile.rotation > -Math.PI / 2 && projectile.rotation < Math.PI / 2;
                if (withinangle && player.direction == 1)
                    smoothtrail *= -1;
                else if (!withinangle && player.direction == -1)
                    smoothtrail *= -1;

                center += projectile.Size / 2;

                Vector2 offset = (projectile.Size/4).RotatedBy(projectile.oldRot[(int)i] - smoothtrail * (-projectile.direction));
                Main.spriteBatch.Draw(
                    glow,
                    center - offset - Main.screenPosition + new Vector2(0, projectile.gfxOffY),
                    null,
                    color27,
                    projectile.rotation,
                    glow.Size() / 2,
                    scale * 0.4f,
                    SpriteEffects.None,
                    0f);
            }

            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), 
                projectile.GetAlpha(lightColor), projectile.rotation, origin2, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}