using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Minions
{
    public class BigBrainProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Big Brain Proj");
            Main.projFrames[projectile.type] = 11;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 74;
            projectile.height = 70;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 2;
            projectile.penetrate = -1;
            projectile.timeLeft = 60;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;

            projectile.scale = 1.5f;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            if (player.active && !player.dead && player.GetModPlayer<FargoPlayer>().BigBrainMinion)
                projectile.timeLeft = 2;

            if (projectile.ai[0] >= 0 && projectile.ai[0] < 200) //has target
            {
                NPC minionAttackTargetNpc = projectile.OwnerMinionAttackTargetNPC;
                if (minionAttackTargetNpc != null && projectile.ai[0] != minionAttackTargetNpc.whoAmI && minionAttackTargetNpc.CanBeChasedBy(projectile))
                    projectile.ai[0] = minionAttackTargetNpc.whoAmI;

                NPC npc = Main.npc[(int)projectile.ai[0]];
                if (npc.CanBeChasedBy(projectile))
                {
                    if (projectile.Distance(npc.Center) > 300)
                    {
                        Movement(npc.Center, 0.5f);
                    }

                    if (++projectile.localAI[0] > 100)
                    {
                        projectile.localAI[0] = 0;
                        if (projectile.owner == Main.myPlayer)
                            Projectile.NewProjectile(projectile.Center, projectile.DirectionTo(npc.Center) * 16, mod.ProjectileType("CreeperProj2"), projectile.damage, projectile.knockBack, projectile.owner, 0, projectile.ai[0]);
                    }

                    if (++projectile.localAI[1] > 35)
                    {
                        projectile.localAI[1] = 0;
                        if (projectile.owner == Main.myPlayer)
                            Projectile.NewProjectile(projectile.Center, projectile.DirectionTo(npc.Center) * 16, mod.ProjectileType("BigBrainIllusion"), projectile.damage, projectile.knockBack, projectile.owner);
                    }
                }
                else //forget target
                {
                    projectile.ai[0] = HomeOnTarget();
                    projectile.netUpdate = true;
                }
            }
            else //no target
            {
                Vector2 targetPos = player.Center;
                if (projectile.Distance(targetPos) > 3000)
                    projectile.Center = player.Center;
                else if (projectile.Distance(targetPos) > 300)
                    Movement(targetPos, 0.5f);

                if (++projectile.localAI[1] > 6)
                {
                    projectile.localAI[1] = 0;
                    projectile.ai[0] = HomeOnTarget();
                    if (projectile.ai[0] != -1)
                        projectile.netUpdate = true;
                }
            }

            projectile.rotation = projectile.velocity.X * 0.02f;

            projectile.frameCounter++;
            if (projectile.frameCounter >= 8)
            {
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 11;
            }
        }

        private void Movement(Vector2 targetPos, float speedModifier)
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
                projectile.velocity.Y += speedModifier;
                if (projectile.velocity.Y < 0)
                    projectile.velocity.Y += speedModifier * 2;
            }
            else
            {
                projectile.velocity.Y -= speedModifier;
                if (projectile.velocity.Y > 0)
                    projectile.velocity.Y -= speedModifier * 2;
            }
            if (Math.Abs(projectile.velocity.X) > 24)
                projectile.velocity.X = 24 * Math.Sign(projectile.velocity.X);
            if (Math.Abs(projectile.velocity.Y) > 24)
                projectile.velocity.Y = 24 * Math.Sign(projectile.velocity.Y);
        }

        private int HomeOnTarget()
        {
            NPC minionAttackTargetNpc = projectile.OwnerMinionAttackTargetNPC;
            if (minionAttackTargetNpc != null && minionAttackTargetNpc.CanBeChasedBy(projectile))
                return minionAttackTargetNpc.whoAmI;

            const float homingMaximumRangeInPixels = 2000;
            int selectedTarget = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (n.CanBeChasedBy(projectile))
                {
                    float distance = projectile.Distance(n.Center);
                    if (distance <= homingMaximumRangeInPixels &&
                        (
                            selectedTarget == -1 || //there is no selected target
                            projectile.Distance(Main.npc[selectedTarget].Center) > distance) //or we are closer to this target than the already selected target
                    )
                        selectedTarget = i;
                }
            }

            return selectedTarget;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (!Main.player[projectile.owner].HeldItem.summon)
                damage /= 4;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;

            SpriteEffects effects = projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), projectile.GetAlpha(lightColor), projectile.rotation, origin2, projectile.scale, effects, 0f);
            return false;
        }
    }
}