using System;
using System.Linq;
using Agava.YandexGames;
using CodeBase.Data;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Logic.Inventory;
using CodeBase.Services;
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

        public static InventoryData AsInventoryData(this IInventory inventory) =>
            new InventoryData(inventory.ToList());

        public static IInventory AsHeroInventory(this InventoryData inventoryData) =>
            new Inventory(inventoryData.InventoryCells);

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
                .LoadSprite($"{AssetPath.AvatarPath}/{entry.extraData}");

            Sprite flag = assetProvider
                .LoadSprite($"{AssetPath.FlagPath}/{entry.player.lang}");

            return new SingleRankData(entry.rank, entry.score, avatar,
                entry.player.publicName ?? Anonymous, flag);
        }
    }
}