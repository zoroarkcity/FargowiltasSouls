using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FargowiltasSouls.Projectiles.JungleMimic
{
    public class JungleMimicSummon : ModProjectile
    {
        public int counter;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jungle Mimic");
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            Main.projFrames[projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 2f;
            projectile.penetrate = -1;
            projectile.aiStyle = 26;
            projectile.width = 52;
            projectile.height = 56;
            aiType = ProjectileID.BabySlime;
        }
        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (player.dead || !player.active)
            {
                player.ClearBuff(mod.BuffType("JungleMimicSummonBuff"));
            }

            if (player.HasBuff(mod.BuffType("JungleMimicSummonBuff")))
            {
                projectile.timeLeft = 2;
            }
            if (++counter > 10)
            {
                counter = 0;
                if (projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < Main.maxNPCs; i++) //look for nearby valid target npc
                    {
                        if (Main.npc[i].CanBeChasedBy() && Main.npc[i].Distance(projectile.Center) < 600 && Collision.CanHitLine(Main.npc[i].Center, 0, 0, projectile.Center, 0, 0))
                        {
                            Vector2 shootVel = Main.npc[i].Center - projectile.Center;
                            shootVel.Normalize();
                            Projectile.NewProjectile(projectile.Center, shootVel * 10f, mod.ProjectileType("JungleMimicSummonCoin"), projectile.damage / 3, projectile.knockBack, Main.myPlayer);
                            break;
                        }
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY - 4),
                new Microsoft.Xna.Framework.Rectangle?(rectangle), projectile.GetAlpha(lightColor), projectile.rotation, origin2,
                projectile.scale, projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
    }
}

   