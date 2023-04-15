using Agava.YandexGames;
using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Logic.Inventory;
using CodeBase.Services;
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
            new InventoryData(inventory.ReadAll());

        public static IInventory AsHeroInventory(this InventoryData inventoryData) =>
            new Inventory(inventoryData.InventoryCells);

        public static SingleRankData ToSingleRankData(this LeaderboardEntryResponse entry)
        {
            IAssetProvider assetProvider = AllServices.Container.Single<IAssetProvider>();
            
            Sprite avatar = assetProvider
                .Instantiate($"{AssetPath.AvatarPath}/{entry.extraData}")
                .GetComponent<Sprite>();
            
            Sprite flag = assetProvider
                .Instantiate($"{AssetPath.AvatarPath}/{entry.extraData}")
                .GetComponent<Sprite>();

            return new SingleRankData(entry.rank, entry.score, avatar,
                entry.player.publicName ?? Anonymous, flag);
        }
    }
}