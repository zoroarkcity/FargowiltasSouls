using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using FargowiltasSouls.Items.Accessories.Forces;
using FargowiltasSouls.Projectiles.Champions;

namespace FargowiltasSouls.NPCs.Champions
{
    public class TimberChampion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Champion of Timber");
        }

        /*public override bool Autoload(ref string name)
        {
            return false;
        }*/

        public override void SetDefaults()
        {
            npc.width = 340;
            npc.height = 400;
            npc.damage = 100;
            npc.defense = 50;
            npc.lifeMax = 120000;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.aiStyle = -1;
            npc.value = Item.buyPrice(0, 10);
            //npc.boss = true;
            music = MusicID.TheTowers;
            musicPriority = MusicPriority.BossMedium;
        }

        public override void AI()
        {
            Player player = Main.player[npc.target];

            switch ((int)npc.ai[0])
            {
                case 0: //jump at player
                    npc.noTileCollide = false;
                    npc.noGravity = false;

                    if (++npc.ai[1] == 60)
                    {
                        npc.TargetClosest();

                        const float gravity = 0.4f;
                        const float time = 90f;
                        Vector2 distance = player.Center - npc.Center;
                        distance.Y -= npc.height / 2;

                        distance.X = distance.X / time;
                        distance.Y = distance.Y / time - 0.5f * gravity * time;
                        npc.velocity = distance;
                        npc.netUpdate = true;
                    }
                    else if (npc.ai[1] > 60)
                    {
                        npc.noTileCollide = true;
                        npc.noGravity = true;
                        npc.velocity.Y += 0.4f;

                        if (npc.ai[1] > 60 + 90)
                        {
                            npc.TargetClosest();
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.netUpdate = true;
                        }
                    }
                    else //less than 60
                    {
                        if (!player.active || player.dead || Vector2.Distance(npc.Center, player.Center) > 2000f)
                        {
                            npc.TargetClosest();
                            if (npc.timeLeft > 30)
                                npc.timeLeft = 30;

                            npc.noTileCollide = true;
                            npc.noGravity = true;
                            npc.velocity.Y -= 1f;
                        }
                        else
                        {
                            npc.timeLeft = 600;
                        }
                    }
                    break;

                case 1:
                    npc.noTileCollide = false;
                    npc.noGravity = false;

                    if (++npc.ai[2] > 35)
                    {
                        npc.ai[2] = 0;
                        const float gravity = 0.2f;
                        float time = 60f;
                        Vector2 distance = player.Center - npc.Center;// + player.velocity * 30f;
                        distance.X = distance.X / time;
                        distance.Y = distance.Y / time - 0.5f * gravity * time;
                        for (int i = 0; i < 10; i++)
                        {
                            Projectile.NewProjectile(npc.Center, distance + Main.rand.NextVector2Square(-0.5f, 0.5f) * 3,
                                ModContent.ProjectileType<Acorn>(), npc.damage / 5, 0f, Main.myPlayer);
                        }
                    }
                    
                    if (++npc.ai[1] > 35 * 3 + 1)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.netUpdate = true;
                        npc.TargetClosest();
                    }
                    break;

                case 2:
                    goto case 0;

                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            
        }

        /*public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }*/

        public override void NPCLoot()
        {
            Item.NewItem(npc.position, npc.Size, ModContent.ItemType<TimberForce>());
        }
    }
}