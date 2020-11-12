using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Masomode
{
    public class FishronFishron : MutantBoss.MutantFishron
    {
        public override string Texture => "FargowiltasSouls/NPCs/Vanilla/NPC_370";

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(mod.BuffType("Defenseless"), 600);
            //player.AddBuff(BuffID.WitheredWeapon, 600);
            if (NPCs.EModeGlobalNPC.BossIsAlive(ref NPCs.EModeGlobalNPC.fishBossEX, NPCID.DukeFishron))
            {
                player.GetModPlayer<FargoPlayer>().MaxLifeReduction += 50;
                player.AddBuff(mod.BuffType("OceanicMaul"), 1800);
            }
        }
    }
}