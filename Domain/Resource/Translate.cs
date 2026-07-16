namespace Domain.Resource
{
    using Domain.Resource;
    using System.Resources;
    public class Translate
    {
        public static string GetTranslate(string value)
        {
            ResourceManager _resourceManager = new ResourceManager("Domain.Resource.Title.Designer", typeof(Title_Designer).Assembly);
            return _resourceManager.GetString(value);
        }
    }
}
