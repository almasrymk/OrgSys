namespace Utility
{
    using Microsoft.Extensions.Localization;
    using Utility.Resource;
    public class Translate
    {
        IStringLocalizer<Title_Designer> _Titlelocalizer;
        IStringLocalizer<Message_Designer> _Messagelocalizer;
        public Translate(IStringLocalizer<Title_Designer> Titlelocalizer, IStringLocalizer<Message_Designer> Messagelocalizer)
        {
            this._Titlelocalizer = Titlelocalizer;
            this._Messagelocalizer = Messagelocalizer;
        }

        public string GetTranslate(string value)
        {
            var val = this._Messagelocalizer.GetString(value);
            if ("" + val == "" || val == value)
                val = this._Titlelocalizer.GetString(value);
            return val;
        }
    }
}