using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.Shared;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using Util;
using SharpDX;

namespace MapHelper
{
    


    public class MapHelper : BaseSettingsPlugin<MapHelperSettings>
    {
        public override void Render()
        {
            FindReflectMaps();
        }



        private void FindReflectMaps()
        {
            if (Keyboard.IsKeyDown((int)Settings.MapHelperKey.Value))
            {
                try
                {

                    ProcessReflectMaps();

                    return;


                }
                catch
                {
                    // ignored
                }
                return;
            }





        }

        private void ProcessReflectMaps()
        {

            var inventory = GameController.Game.IngameState.IngameUi.InventoryPanel[InventoryIndex.PlayerInventory];

            var stash = GameController.Game.IngameState.IngameUi.StashElement.VisibleStash; //offener stashtab

            var inventoryItems = inventory.VisibleInventoryItems;

            var stashitems = stash.VisibleInventoryItems;

            IList<NormalInventoryItem> allitems;

            if (stashitems == null)
            {
                allitems = inventoryItems;
            }
            else
            {

                allitems = inventoryItems.Concat(stashitems)
                                        .ToList();
            }


            if (allitems == null)
            {
                LogError("no items", 5);
                return;
            }

            var mapsiminventar = allitems
                .Where(x => GameController.Files.BaseItemTypes.Translate(x.Item.Path).BaseName.Contains("Map")).ToList();
            if (mapsiminventar.Count <= 0)
            {
                LogError("no maps in inventory", 5);
                return;
            }

            var badmaps = new List<RectangleF>();
            var unidmaps = new List<RectangleF>();
            var goodmaps = new List<RectangleF>(); //not yet

            foreach (NormalInventoryItem map in mapsiminventar)
            {

                var baseItemType = GameController.Files.BaseItemTypes.Translate(map.Item.Path);

                var testItem = new ItemData(map, baseItemType);

                if (testItem == null) continue;

                if (!testItem.BIdentified)//unidentified
                {
                    var drawRect = map.GetClientRect();
                    drawRect.X -= 5;
                    drawRect.Y -= 5;
                    unidmaps.Add(drawRect);
                    continue;
                }


                var mods = testItem.ItemMods;

                List<ModValue> values =
                    mods.Select(
                        it => new ModValue(it, GameController.Files, testItem.ItemLevel, GameController.Files.BaseItemTypes.Translate(testItem.Path))).ToList();

                foreach (var mod in values)
                {
                    if (mod.Record.Group == "MapMonsterElementalReflection" && Settings.EleReflect)  //ele reflect
                    {
                        //LogMessage("ELE REFLECT FOUND", 5);
                        var drawRect = map.GetClientRect();
                        drawRect.X -= 5;
                        drawRect.Y -= 5;
                        badmaps.Add(drawRect);
                    }
                    else if (mod.Record.Group == "MapPlayerReducedRegen" && Settings.NoRegen)  //no regen/ reduced regen
                    {
                        //LogMessage("ELE REFLECT FOUND", 5);
                        var drawRect = map.GetClientRect();
                        drawRect.X -= 5;
                        drawRect.Y -= 5;
                        badmaps.Add(drawRect);
                    }
                    else if (mod.Record.Group == "MapMonsterPhysicalReflection" && Settings.PhysReflect)  //phys reflect
                    {
                        var drawRect = map.GetClientRect();
                        drawRect.X -= 5;
                        drawRect.Y -= 5;
                        badmaps.Add(drawRect);
                    }
                    else
                    {
                        LogMessage("no danger", 5);

                    }


                }

            }

            DrawFrame(badmaps, 1);
            DrawFrame(unidmaps, 2);

        }

        private void DrawFrame(List<RectangleF> goodItems, int color)
        {
            Color farbe;
            if (color == 1) farbe = Color.Red;
            else farbe = Color.Yellow;

            foreach (var position in goodItems)

            {
                RectangleF border = new RectangleF { X = position.X + 8, Y = position.Y + 8, Width = position.Width - 6, Height = position.Height - 6 };
                Graphics.DrawFrame(border, farbe, 3);
            }
        }

    }
}
