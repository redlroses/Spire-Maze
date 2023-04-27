using System;
using System.Linq;
using CodeBase.EditorCells;
using CodeBase.LevelSpecification;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelName;
        public int LevelId;
        public float Radius = 5.945f;
        public float ArchAngle = 22.5f;
        public float FloorHeight = 2f;
        [HideInInspector] public int Width = 16;
        public int Height = 0;
        [SerializeField] [SerializeReference] public CellData[] CellDataMap;

        public int Size => Width * Height;
        public Vector3 HeroInitialPosition => GetHeroInitialPosition();

        private Vector3 GetHeroInitialPosition()
        {
            for (int i = 0; i < Size; i++)
            {
                if (CellDataMap[i] is InitialPlate == false)
                {
                    continue;
                }

                float height = i / (float) Width * FloorHeight;
                float angle = i % Width * ArchAngle;
                return GetPosition(angle, Radius).ChangeY(height);
            }

            throw new NullReferenceException();
        }

        private Vector3 GetPosition(float byArcGrade, float radius)
        {
            float posX = Mathf.Cos(byArcGrade * Mathf.Deg2Rad) * radius;
            float posZ = Mathf.Sin(byArcGrade * Mathf.Deg2Rad) * radius;
            return new Vector3(posX, 0, posZ);
        }
    }
}