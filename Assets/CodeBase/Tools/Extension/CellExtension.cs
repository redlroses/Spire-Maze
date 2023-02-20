using CodeBase.LevelSpecification;
using CodeBase.LevelSpecification.Cells;

namespace CodeBase.Tools.Extension
{
    public static class CellExtension
    {
        public static bool IsTypeOf(this Cell self, CellType includeType, CellType excludeType = CellType.Air) =>
            // (self.CellType & includeType) != 0 && (self.CellType & excludeType) == 0;
            true;
    }
}