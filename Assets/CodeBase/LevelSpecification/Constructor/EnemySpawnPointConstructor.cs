using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Enemy;

namespace CodeBase.LevelSpecification.Constructor
{
    public class EnemySpawnPointConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, Cell[] cells)
            where TCell : Cell
        {
            foreach (var cell in cells)
            {
                var enemyData = (EditorCells.EnemySpawnPoint) cell.CellData;
                string path = $"{AssetPath.Enemies}/Enemy{enemyData.Type}";

                var enemy = gameFactory.CreateEnemy(path, cell.Container.transform.position).GetComponent<IEnemy>();
                enemy.Construct(cell.Id);
            }
        }
    }
}