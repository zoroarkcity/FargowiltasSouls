using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.DeviBoss
{
    public class DeviAxe : ModProjectile
    {
        public override string Texture => "FargowiltasSouls/Projectiles/DeviBoss/DeviSparklingLove";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sparkling Love");
        }

        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 46;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 180;
            projectile.hide = true;
            projectile.GetGlobalProjectile<FargoGlobalProjectile>().ImmuneToMutantBomb = true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void AI()
        {
            //the important part
            int ai0 = (int)projectile.ai[0];
            if (ai0 > -1 && ai0 < 200 && Main.npc[ai0].active /*&& Main.npc[ai0].type == mod.NPCType("MutantBoss")*/)
            {
                if (projectile.localAI[0] == 0)
                {
                    projectile.localAI[0] = 1;
                    projectile.localAI[1] = projectile.DirectionFrom(Main.npc[ai0].Center).ToRotation();
                }

                Vector2 offset = new Vector2(projectile.ai[1], 0).RotatedBy(Main.npc[ai0].ai[3] + projectile.localAI[1]);
                projectile.Center = Main.npc[ai0].Center + offset;
            }
            else
            {
                projectile.Kill();
                return;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.velocity.X = target.Center.X < Main.npc[(int)projectile.ai[0]].Center.X ? -15f : 15f;
            target.velocity.Y = -10f;
            target.AddBuff(mod.BuffType("Lovestruck"), 240);
        }
    }
}