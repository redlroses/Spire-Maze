using CodeBase.FileRead;

namespace CodeBase.Extensions
{
    public static class DataExtensions
    {
        public static MapData ToMapData(this string text)
            => new MapData(text);
    }
}