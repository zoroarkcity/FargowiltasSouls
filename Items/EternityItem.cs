using System;
using System.Collections.Generic;
using System.IO;
using FargowiltasSouls.Buffs.Souls;
using FargowiltasSouls.Projectiles;
using FargowiltasSouls.Projectiles.Masomode;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FargowiltasSouls.Items
{
    public class EternityItem : GlobalItem
    {
        public bool Eternity;
        public EternityItem()
        {
            Eternity = false;
        }

        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(Eternity);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            Eternity = reader.ReadBoolean();
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.GetGlobalItem<EternityItem>().Eternity)
            {
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.mod == "Terraria" && line2.Name == "ItemName")
                    {
                        line2.overrideColor = Fargowiltas.EModeColor();
                    }
                }
                tooltips.Add(new TooltipLine(mod, "Eternity", "Eternity"));
            }
        }
    }
}