using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Buffs.Masomode
{
    public class LowGround : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Low Ground");
            Description.SetDefault("Cannot stand on platforms");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "FargowiltasSouls/Buffs/PlaceholderDebuff";
            return true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 pos = player.position;
                pos.X += i * 8;
                pos.Y += player.height;
                if (player.mount.Active)
                    pos.Y += player.mount.HeightBoost;

                Tile tile = Framing.GetTileSafely((int)(pos.X / 16), (int)(pos.Y / 16));
                if (tile.type == TileID.Platforms)
                    WorldGen.KillTile((int)(pos.X / 16), (int)(pos.Y / 16));
            }
        }
    }
}