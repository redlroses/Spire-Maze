using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic.Enemy;
using CodeBase.Services.StaticData;

namespace CodeBase.LevelSpecification.Constructor
{
    public class EnemySpawnPointConstructor : ICellConstructor
    {
        public void Construct<TCell>(IGameFactory gameFactory, IStaticDataService staticData, Cell[] cells) where TCell : Cell
        {
            foreach (Cell cell in cells)
            {
                var enemyData = (EditorCells.EnemySpawnPoint)cell.CellData;
                string path= $"{AssetPath.Enemies}/Enemy{enemyData.Type}";
                
                IEnemy enemy = gameFactory.CreateEnemy(path, cell.Container.transform.position).GetComponent<IEnemy>();
                enemy.Construct(cell.Id);
            }
        }
    }
}