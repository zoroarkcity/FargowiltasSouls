using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using FargowiltasSouls.Buffs.Masomode;
using FargowiltasSouls.NPCs.Champions;

namespace FargowiltasSouls.Projectiles.Champions
{
    public class CosmosRitual : BaseArena
    {
        public override string Texture => "Terraria/Projectile_454";

        public CosmosRitual() : base(MathHelper.Pi / 140f, 1000f, ModContent.NPCType<CosmosChampion>()) { }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Seal");
            Main.projFrames[projectile.type] = 2;
        }

        protected override void Movement(NPC npc)
        {
            projectile.Center = npc.Center;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (FargoSoulsWorld.MasochistMode)
            {
                target.AddBuff(BuffID.OnFire, 300);
                target.AddBuff(BuffID.Electrified, 300);
                target.AddBuff(ModContent.BuffType<Hexed>(), 300);
                target.AddBuff(BuffID.Frostburn, 300);
            }
        }
    }
}