using System;
using CodeBase.LevelSpecification;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Lift;
using CodeBase.StaticData;
using CodeBase.Tools.Extension;
using UnityEngine;
 
 namespace CodeBase.Logic
 {
     public class Spire : MonoBehaviour
     {
         private Level _level;
         private LevelStaticData _levelStaticData;

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

         public Cell GetLeftFrom(Cell from)
         {
             int floor = from.Id / _level.Width;
             int floorOffset = _level.Width * floor;
             int indexOnFloor = from.Id - floorOffset;
             int indexLeft = (indexOnFloor - 1).ClampRound(0, _level.Width);

             return _level.GetCell(_level.Height - floor - 1, indexLeft);
         }

         public Cell GetUpFrom(Cell from)
         {
             int floor = from.Id / _level.Width;
             int floorOffset = _level.Width * floor;
             int indexOnFloor = from.Id - floorOffset;
             int indexUp = floor + 1;

             return _level.GetCell(_level.Height - indexUp - 1, indexOnFloor);
         }

         public Cell GetCellById(int id) =>
             _level.GetCell(_level.Height - 1 - id / _level.Width, -4 + id % _level.Width);

         public void Construct(Level level)
         {
             _level = level;
         }
     }
 }