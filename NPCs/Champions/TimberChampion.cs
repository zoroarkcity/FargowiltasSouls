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

        public override bool Autoload(ref string name)
        {
            return false;
        }

        public override void SetDefaults()
        {
            npc.width = 340;
            npc.height = 400;
            npc.damage = 100;
            npc.defense = 50;
            npc.lifeMax = 120000;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.aiStyle = -1;
            npc.value = Item.buyPrice(0, 10);
            npc.boss = true;
        }

        public override void AI()
        {
            
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.position, npc.Size, ModContent.ItemType<TimberForce>());
        }
    }
}