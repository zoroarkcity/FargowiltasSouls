using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.DeviBoss
{
    public class DeviRitual : BaseArena
    {
        public DeviRitual() : base(MathHelper.Pi / 140f, 1000f, ModContent.NPCType<NPCs.DeviBoss.DeviBoss>(), 86, 3) { }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deviantt Seal");
        }

        protected override void Movement(NPC npc)
        {
            if (npc.ai[0] <= 10)
                projectile.Kill();
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("Lovestruck"), 240);
        }
    }
}