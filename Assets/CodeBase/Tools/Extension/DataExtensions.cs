using System;
using System.Collections.Generic;
using System.Linq;
using Agava.YandexGames;
using CodeBase.Data;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic.Inventory;
using CodeBase.Services;
using CodeBase.Services.Localization;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Storable;
using UnityEngine;

namespace CodeBase.Tools.Extension
{
    public static class DataExtensions
    {
        private const string Anonymous = "Anonimus";

        public static Vector3Data AsVectorData(this Vector3 vector) =>
            new Vector3Data(vector.x, vector.y, vector.z);

        public static Vector3 AsUnityVector(this Vector3Data vector3Data) =>
            new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);

        public static string ToJson(this object obj) =>
            JsonUtility.ToJson(obj);

        public static T ToDeserialized<T>(this string json) =>
            JsonUtility.FromJson<T>(json);

        public static InventoryData AsInventoryData(this IInventory inventory)
        {
            List<ItemData> itemDatas = new List<ItemData>(inventory.Count);

            foreach (IReadOnlyInventoryCell cell in inventory)
            {
                itemDatas.Add(new ItemData(cell.Count, (int) cell.Item.ItemType));
            }

            return new InventoryData(itemDatas);
        }

        public static IInventory AsHeroInventory(this InventoryData inventoryData)
        {
            IGameFactory gameFactory = AllServices.Container.Single<IGameFactory>();
            IStaticDataService staticData = AllServices.Container.Single<IStaticDataService>();
            List<InventoryCell> itemDatas = new List<InventoryCell>(inventoryData.ItemDatas.Count);

            foreach (ItemData itemData in inventoryData.ItemDatas)
            {
                var item = gameFactory.CreateItem(staticData.GetForStorable((StorableType) itemData.StorableType));
                itemDatas.Add(new InventoryCell(item, itemData.Count));
            }

            return new Inventory(itemDatas);
        }

        public static StorableType ToStorableType(this Colors colors)
        {
            return colors switch
            {
                Colors.Red => StorableType.RedKey,
                Colors.Green => StorableType.GreenKey,
                Colors.Blue => StorableType.BlueKey,
                Colors.Rgb => StorableType.RgbKey,
                _ => throw new ArgumentOutOfRangeException(nameof(colors), colors, null)
            };
        }

        public static Colors ToColorsType(this StorableType storableType)
        {
            return storableType switch
            {
                StorableType.RedKey => Colors.Red,
                StorableType.GreenKey => Colors.Green,
                StorableType.BlueKey => Colors.Blue,
                StorableType.RgbKey => Colors.Rgb,
                _ => throw new ArgumentOutOfRangeException(nameof(storableType), storableType, null)
            };
        }

        public static SingleRankData ToSingleRankData(this LeaderboardEntryResponse entry)
        {
            IAssetProvider assetProvider = AllServices.Container.Single<IAssetProvider>();

            Sprite avatar = assetProvider
                .LoadAsset<Sprite>($"{AssetPath.AvatarPath}/{entry.extraData}");

            Sprite flag = assetProvider
                .LoadAsset<Sprite>($"{AssetPath.FlagPath}/{entry.player.lang}");

            return new SingleRankData(entry.rank, entry.score, avatar,
                entry.player.publicName ?? Anonymous, flag);
        }

        public static LanguageId AsLangId(this string langString)
        {
            return langString switch
            {
                "ru" => LanguageId.Russian,
                "en" => LanguageId.English,
                "tr" => LanguageId.Turkish,
                _ => LanguageId.English
            };
        }
    }
}