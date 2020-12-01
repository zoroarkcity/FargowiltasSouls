using FargowiltasSouls.Projectiles.Minions;
using IL.Terraria.Chat.Commands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace FargowiltasSouls.Projectiles.BossWeapons
{
	public class DragonBreathProj2 : DragonBreathProj
	{
		public override string Texture => "Terraria/Projectile_687";

        public override void SetDefaults()
        {
            base.SetDefaults();
            lerp = 1f;
        }

        public override void AI()
		{
            base.AI();
            if (--projectile.localAI[0] < 0)
            {
                projectile.localAI[0] = 25;
                if (projectile.owner == Main.myPlayer)
                {
                    Projectile.NewProjectile(projectile.Center, 18f * Vector2.Normalize(projectile.velocity),
                        ModContent.ProjectileType<DragonPhoenix>(), projectile.damage / 2, projectile.knockBack, projectile.owner);
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
            target.immune[projectile.owner] = 5;
        }
    }
}