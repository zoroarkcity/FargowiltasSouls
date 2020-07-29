using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls
{
    public class PatreonPlayer : ModPlayer
    {
        public bool Gittle;
        public bool RoombaPet;

        public bool Sasha;
        public bool FishMinion;

        public bool CompOrb;

        public bool ManliestDove;
        public bool DovePet;

        public bool Cat;
        public bool KingSlimeMinion;

        public bool WolfDashing;

        public override void ResetEffects()
        {
            Gittle = false;
            RoombaPet = false;
            Sasha = false;
            FishMinion = false;
            CompOrb = false;
            ManliestDove = false;
            DovePet = false;
            Cat = false;
            KingSlimeMinion = false;
            WolfDashing = false;
        }

        public override void OnEnterWorld(Player player)
        {
            if (Gittle || Sasha || ManliestDove || Cat)
            {
                Main.NewText("Your special patreon effects are active " + player.name + "!");
            }
        }

        public override void PostUpdateMiscEffects()
        {
            if (player.name == "iverhcamer")
            {
                Gittle = true;
                player.pickSpeed -= .15f;
                //shine effect
                Lighting.AddLight(player.Center, 0.8f, 0.8f, 0f);
            }

            if (player.name == "Sasha")
            {
                Sasha = true;

                player.lavaImmune = true;
                player.fireWalk = true;
                player.buffImmune[BuffID.OnFire] = true;
                player.buffImmune[BuffID.CursedInferno] = true;
                player.buffImmune[BuffID.Burning] = true;
            }

            if (player.name == "Dove")
            {
                ManliestDove = true;
            }

            if (player.name == "cat")
            {
                Cat = true;

                if (NPC.downedMoonlord)
                {
                    player.maxMinions += 4;
                }
                else if (Main.hardMode)
                {
                    player.maxMinions += 2;
                }

                player.minionDamage += player.maxMinions * 0.5f;
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            OnHitEither(target, damage, knockback, crit);

            
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            OnHitEither(target, damage, knockback, crit);

            
        }

        private void OnHitEither(NPC target, int damage, float knockback, bool crit)
        {
            if (Gittle)
            {
                if (Main.rand.Next(10) == 0)
                {
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];

                        if (Vector2.Distance(target.Center, npc.Center) < 50)
                        {
                            npc.AddBuff(BuffID.Venom, 300);
                        }
                    }
                }

                if (ModLoader.GetMod("CalamityMod") != null)
                {
                    target.StrikeNPC(target.lifeMax, 0f, 0);
                }

            }
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (CompOrb && !item.magic && !item.summon)
            {
                damage = (int)(damage * 1.25f);

                if (player.manaSick)
                    damage = (int)(damage * player.manaSickReduction);

                for (int num468 = 0; num468 < 20; num468++)
                {
                    int num469 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 15, -target.velocity.X * 0.2f,
                        -target.velocity.Y * 0.2f, 100, default(Color), 2f);
                    Main.dust[num469].noGravity = true;
                    Main.dust[num469].velocity *= 2f;
                    num469 = Dust.NewDust(new Vector2(target.Center.X, target.Center.Y), target.width, target.height, 15, -target.velocity.X * 0.2f,
                        -target.velocity.Y * 0.2f, 100);
                    Main.dust[num469].velocity *= 2f;
                }
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (CompOrb && !proj.magic && !proj.minion)
            {
                damage = (int)(damage * 1.25f);
                
                if (player.manaSick)
                    damage = (int)(damage * player.manaSickReduction);

                for (int num468 = 0; num468 < 20; num468++)
                {
                    int num469 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 15, -target.velocity.X * 0.2f,
                        -target.velocity.Y * 0.2f, 100, default(Color), 2f);
                    Main.dust[num469].noGravity = true;
                    Main.dust[num469].velocity *= 2f;
                    num469 = Dust.NewDust(new Vector2(target.Center.X, target.Center.Y), target.width, target.height, 15, -target.velocity.X * 0.2f,
                        -target.velocity.Y * 0.2f, 100);
                    Main.dust[num469].velocity *= 2f;
                }
            }
        }

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            if (WolfDashing) //dont draw player during dash
                while (layers.Count > 0)
                    layers.RemoveAt(0);
        }
    }
}