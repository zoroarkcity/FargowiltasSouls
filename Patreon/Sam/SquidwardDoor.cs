using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Patreon.Sam
{
    public class SquidwardDoor : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'After you Mr. Squidward'");
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 28;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 150;
            item.createTile = mod.TileType("SquidwardDoorClosed");
        }
    }
}