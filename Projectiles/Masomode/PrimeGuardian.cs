using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class PrimeGuardian : MutantBoss.MutantGuardian
    {
        public override string Texture => "Terraria/NPC_127";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dungeon Guardian Prime");
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.timeLeft = 600;
            cooldownSlot = -1;
        }

        public override bool CanHitPlayer(Player target)
        {
            return true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("NanoInjection"), 480);
            target.AddBuff(mod.BuffType("Defenseless"), 480);
            target.AddBuff(mod.BuffType("Lethargic"), 480);
        }
    }
}

