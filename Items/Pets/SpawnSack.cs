using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Pets
{
    public class SpawnSack : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spawn Sack");
            Tooltip.SetDefault("Summons the spawn of Mutant\n'You think you're safe?'");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.WispinaBottle);
            item.value = Item.sellPrice(0, 5);
            item.rare = -13;
            item.shoot = mod.ProjectileType("MutantSpawn");
            item.buffType = mod.BuffType("MutantSpawnBuff");
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}