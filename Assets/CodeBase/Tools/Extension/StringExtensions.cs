using I2.Loc;

namespace CodeBase.Tools.Extension
{
    public static class StringExtensions
    {
        public static string TranslateTerm(this string term) =>
            LocalizationManager.GetTranslation(term);
    }
}