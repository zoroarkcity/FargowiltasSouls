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

        public override void SetDefaults()
        {
            projectile.scale *= 0.75f;
            projectile.height = (int)(projectile.height * 0.75);
            projectile.width = (int)(projectile.height * 0.75);
            projectile.tileCollide = false;
            projectile.timeLeft = 150 + Main.rand.Next(0, 10); //make them all die at slightly different times so no big audio pop on death
        }
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

        public override void Kill(int timeLeft)
        {
            for (int num249 = 0; num249 < 150; num249++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 5, 2 * projectile.direction, -2f);
            }

            int soundtype = (Main.rand.NextBool()) ? 17 : 30;
            Main.PlaySound(SoundID.NPCKilled, (int)projectile.Center.X, (int)projectile.Center.Y, soundtype, 0.75f, 0.2f);

            Gore.NewGore(projectile.Center - Vector2.UnitX * 20f * projectile.direction, projectile.velocity, mod.GetGoreSlot("Gores/Fishron/Gore_576"), projectile.scale);
            Gore.NewGore(projectile.Center - Vector2.UnitY * 30f, projectile.velocity, mod.GetGoreSlot("Gores/Fishron/Gore_574"), projectile.scale);
            Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/Fishron/Gore_575"), projectile.scale);
            Gore.NewGore(projectile.Center + Vector2.UnitX * 20f * projectile.direction, projectile.velocity, mod.GetGoreSlot("Gores/Fishron/Gore_573"), projectile.scale);
            Gore.NewGore(projectile.Center - Vector2.UnitY * 30f, projectile.velocity, mod.GetGoreSlot("Gores/Fishron/Gore_574"), projectile.scale);
            Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/Fishron/Gore_575"), projectile.scale);
        }
    }
}