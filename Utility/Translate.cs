namespace Utility
{
    public class Translate
    {
        public static string GetTranslate(string value)
        {
            return Domain.Resource.Translate.GetTranslate(value);
        }
    }
}
