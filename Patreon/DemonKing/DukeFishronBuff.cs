using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace FargowiltasSouls.Patreon.DemonKing
{
    public class DukeFishronBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Duke Fishron");
            Description.SetDefault("Duke Fishron will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "FargowiltasSouls/Buffs/PlaceholderBuff";
            return true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<FargoPlayer>().DukeFishron = true;
            player.buffTime[buffIndex] = 2;
        }
    }
}