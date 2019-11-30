using System.Collections.Generic;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.PoEMemory.Models;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using SharpDX;

namespace Util
{
    public class ItemData
    {
        private readonly NormalInventoryItem _inventoryItem;

        public NormalInventoryItem InventoryItem => _inventoryItem;
        public string Path { get; }
        public string ClassName { get; }
        public string BaseName { get; }
        public ItemRarity Rarity { get; }
        public int ItemQuality { get; }
        public bool BIdentified { get; }
        public int ItemLevel { get; }
        public int MapTier { get; }
        public bool isElder { get; }
        public bool isShaper { get; }

        public bool isFractured { get; }

        //public List<Mods> ItemMods { get; }
        public List<ExileCore.PoEMemory.MemoryObjects.ItemMod> ItemMods { get; }
        public Vector2 clientRect { get; }
        public string UniqueName;

        public ItemData(NormalInventoryItem inventoryItem, BaseItemType baseItemType) {
            _inventoryItem = inventoryItem;
            var item = inventoryItem.Item;
            Path = item.Path;
            var baseComponent = item.GetComponent<Base>();
            isElder = baseComponent.isElder;
            isShaper = baseComponent.isShaper;
            var mods = item.GetComponent<Mods>();
            Rarity = mods?.ItemRarity ?? ItemRarity.Normal;
            BIdentified = mods?.Identified ?? true;
            ItemLevel = mods?.ItemLevel ?? 0;
            isFractured = mods?.HaveFractured ?? false;
            ItemMods = mods?.ItemMods;

            UniqueName = mods?.UniqueName;

            var quality = item.GetComponent<Quality>();
            ItemQuality = quality?.ItemQuality ?? 0;
            ClassName = baseItemType.ClassName;
            BaseName = baseItemType.BaseName;
            MapTier = item.HasComponent<Map>() ? item.GetComponent<Map>().Tier : 0;
            clientRect = _inventoryItem.GetClientRect().Center;


        }

        public Vector2 GetClickPosCache() => clientRect;

        public Vector2 GetClickPos() {
            var paddingPixels = 3;
            var clientRect = _inventoryItem.GetClientRect();

            //var x = MathHepler.Randomizer.Next((int) clientRect.TopLeft.X + paddingPixels, (int) clientRect.TopRight.X - paddingPixels);
            //var y = MathHepler.Randomizer.Next((int) clientRect.TopLeft.Y + paddingPixels, (int) clientRect.BottomLeft.Y - paddingPixels);


            //return new Vector2(x, y);
            var center = clientRect.Center;
            return center;
        }
    }


}