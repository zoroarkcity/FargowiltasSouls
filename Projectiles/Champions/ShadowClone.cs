using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Champions
{
    public class ShadowClone : ModProjectile
    {
        public override string Texture => "FargowiltasSouls/NPCs/Champions/ShadowChampion";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Champion of Shadow");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 110;
            projectile.height = 110;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.aiStyle = -1;
            cooldownSlot = 1;

            projectile.timeLeft = 720;
        }

        public override void AI()
        {
            Player player = Main.player[(int)projectile.ai[0]];

            for (int i = 0; i < 3; i++)
            {
                int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 27, 0f, 0f, 0, default(Color), 2f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 4f;
            }
            for (int i = 0; i < 3; i++)
            {
                int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 54, 0f, 0f, 0, default(Color), 5f);
                Main.dust[d].noGravity = true;
            }

            if (--projectile.ai[1] > 0)
            {
                Vector2 targetPos = player.Center + projectile.DirectionFrom(player.Center) * 400f;
                if (projectile.Distance(targetPos) > 50)
                    Movement(targetPos, 0.3f, 24f);
            }
            else if (projectile.ai[1] == 0)
            {
                projectile.velocity = Vector2.Zero;
                projectile.position += player.velocity / 4f;
                projectile.netUpdate = true;
                projectile.localAI[0] = projectile.DirectionTo(player.Center).ToRotation();
                if (Main.netMode != 1)
                {
                    Projectile.NewProjectile(projectile.Center, projectile.DirectionTo(player.Center), ModContent.ProjectileType<ShadowDeathraySmall>(), 0, 0f, Main.myPlayer);
                }
            }
            else if (projectile.ai[1] == -30)
            {
                projectile.velocity = 45f * Vector2.UnitX.RotatedBy(projectile.localAI[0]);
            }
        }

        private void Movement(Vector2 targetPos, float speedModifier, float cap = 12f, bool fastY = false)
        {
            if (projectile.Center.X < targetPos.X)
            {
                projectile.velocity.X += speedModifier;
                if (projectile.velocity.X < 0)
                    projectile.velocity.X += speedModifier * 2;
            }
            else
            {
                projectile.velocity.X -= speedModifier;
                if (projectile.velocity.X > 0)
                    projectile.velocity.X -= speedModifier * 2;
            }
            if (projectile.Center.Y < targetPos.Y)
            {
                projectile.velocity.Y += fastY ? speedModifier * 2 : speedModifier;
                if (projectile.velocity.Y < 0)
                    projectile.velocity.Y += speedModifier * 2;
            }
            else
            {
                projectile.velocity.Y -= fastY ? speedModifier * 2 : speedModifier;
                if (projectile.velocity.Y > 0)
                    projectile.velocity.Y -= speedModifier * 2;
            }
            if (Math.Abs(projectile.velocity.X) > cap)
                projectile.velocity.X = cap * Math.Sign(projectile.velocity.X);
            if (Math.Abs(projectile.velocity.Y) > cap)
                projectile.velocity.Y = cap * Math.Sign(projectile.velocity.Y);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Darkness, 300);
            if (FargoSoulsWorld.MasochistMode)
                target.AddBuff(BuffID.Blackout, 300);
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

            SpriteEffects effects = projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++)
            {
                Color color27 = color26;
                color27 *= (float)(ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[projectile.type];
                Vector2 value4 = projectile.oldPos[i];
                float num165 = projectile.oldRot[i];
                Main.spriteBatch.Draw(texture2D13, value4 + projectile.Size / 2f - Main.screenPosition + new Vector2(0, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, projectile.scale, effects, 0f);
            }

            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), projectile.GetAlpha(lightColor), projectile.rotation, origin2, projectile.scale, effects, 0f);
            return false;
        }
    }
}