namespace Utility
{
    using Microsoft.Extensions.Localization;
    using System.Resources;
    public class Translate
    {
        public static string GetTranslate(string value)
        {
            ResourceManager _resourceManager = new ResourceManager("Utility.Resource.Title.Designer", typeof(Resource.Title_Designer).Assembly);
            return _resourceManager.GetString(value);
        }
    }
}
