using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using FargowiltasSouls.NPCs;

namespace FargowiltasSouls.Projectiles.DeviBoss
{
    public class DeviRainHeart2 : DeviRainHeart
    {
        public override string Texture => "FargowiltasSouls/Projectiles/Masomode/FakeHeart";

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);

            int ai1 = (int)projectile.ai[1];
            if (projectile.ai[1] >= 0f && projectile.ai[1] < 200f &&
                Main.npc[ai1].active && Main.npc[ai1].type == mod.NPCType("DeviBoss"))
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(projectile.Center, -Vector2.UnitY, mod.ProjectileType("DeviDeathray"), projectile.damage, projectile.knockBack, projectile.owner);
                    if (Main.player[Main.npc[ai1].target].Center.Y > projectile.Center.Y)
                        Projectile.NewProjectile(projectile.Center, Vector2.UnitY, mod.ProjectileType("DeviDeathray"), projectile.damage, projectile.knockBack, projectile.owner);
                }
            }
        }
    }
}