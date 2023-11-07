using System;
using CodeBase.LevelSpecification;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Lift;
using CodeBase.StaticData;
using UnityEngine;
 
 namespace CodeBase.Logic
 {
     public class Spire : MonoBehaviour
     {
         private Vector3 _position;
         private Level _level;
         private LevelStaticData _levelStaticData;
         private float _radius;

         public Vector3 Position => _position;
         public float Radius => _radius;

         private void Awake()
         {
             _position = transform.position;
         }

         public Transform GetContainerAtPosition(CellPosition position)
         {
             foreach (Cell cell in _level)
             {
                 if (cell.Position == position)
                 {
                     return cell.Container;
                 }
             }

             throw new ArgumentOutOfRangeException(nameof(position));
         }

         public void Construct(Level level, float radius)
         {
             _level = level;
             _radius = radius;
         }
     }
 }