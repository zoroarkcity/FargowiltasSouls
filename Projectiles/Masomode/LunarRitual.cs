using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class LunarRitual : BaseArena
    {
        public override string Texture => "Terraria/Projectile_454";

        public LunarRitual() : base(MathHelper.Pi / 140f, 1600f, NPCID.MoonLordCore) { }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lunar Ritual");
            Main.projFrames[projectile.type] = 2;
        }

        protected override void Movement(NPC npc)
        {
            if (projectile.Distance(npc.Center) <= 1)
                projectile.Center = npc.Center;
            else if (projectile.Distance(npc.Center) > threshold)
                projectile.velocity = (npc.Center - projectile.Center) / 30;
            else
                projectile.velocity = projectile.DirectionTo(npc.Center);
        }

        public override void AI()
        {
            base.AI();

            projectile.frameCounter++;
            if (projectile.frameCounter >= 6)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > 1)
                    projectile.frame = 0;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("CurseoftheMoon"), 300);
        }
    }
}