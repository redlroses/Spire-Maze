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

         public Cell GetLeft(Cell from)
         {
             int floor = from.Id / _level.Width;
             int floorOffset = _level.Width * floor;
             int index = from.Id - floorOffset;
             int leftIndex = (index - 1).ClampRound(0, _level.Width);

             Debug.Log($"Floor: {floor}, index: {index}, leftIndex: {leftIndex}");

             return _level.GetCell(_level.Height - 1 - floor, leftIndex);
         }

         public Cell GetRight(Cell from)
         {
             int floor = from.Id / _level.Width;
             int floorOffset = _level.Width * floor;
             int index = from.Id - floorOffset;
             int leftIndex = (index + 1).ClampRound(0, _level.Width);

             Debug.Log($"Floor: {floor}, index: {index}, leftIndex: {leftIndex}");

             return _level.GetCell(_level.Height - 1 - floor, leftIndex);
         }

         public Cell GetCellById(int id) =>
             _level.GetCell(_level.Height - 1 - id / _level.Width, -4 + id % _level.Width);

         public void Construct(Level level)
         {
             _level = level;
         }
     }
 }