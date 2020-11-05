using FargowiltasSouls.NPCs.Critters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Misc
{
    public class TopHatSquirrelCaught : ModItem
    {
        public override string Texture => "FargowiltasSouls/Items/Weapons/Misc/TophatSquirrelWeapon";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tophat Squirrel");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 10;
            item.rare = 1;
            item.useStyle = 1;
            item.useAnimation = 15;
            item.useTime = 10;
            item.consumable = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item44;
            item.makeNPC = (short)ModContent.NPCType<TophatSquirrelCritter>();
            //Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, Main.npcFrameCount[ModContent.NPCType<TophatSquirrelCritter>()]));
        }
    }
}
