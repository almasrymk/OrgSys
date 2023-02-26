using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.Tools
{
    public enum TypeLang { AR, EN }
    public enum typeText { String, Float, Integer, Decimal, Percent , Date , DateTime }
    public class GeneralMember
    {
        static TypeLang _CurrLang = TypeLang.AR;
        public static TypeLang CurrLang { get { return _CurrLang; } set { _CurrLang = value; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="_char"></param>
        /// <returns></returns>
        public static bool OnyFloat(string text, char _char)
        {
            try
            {
                float x = 0;
                if (!float.TryParse(_char.ToString(), out x) && _char != '\b' && _char != '.')
                    return true;
                if (_char == '.' && text.Contains("."))
                    return true;
                return false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="_char"></param>
        /// <returns></returns>
        public static bool OnyDateTime(string text, char _char)
        {
            try
            {
                DateTime x = DateTime.Now;
                if (!DateTime.TryParse(_char.ToString(), out x) && _char != '\b' )
                    return true;
                if (_char == '.' && text.Contains("."))
                    return true;
                return false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="_char"></param>
        /// <returns></returns>
        public static bool OnyDecimal(string text, char _char)
        {
            try
            {
                Decimal x = 0;
                if (!Decimal.TryParse(_char.ToString(), out x) && _char != '\b' && _char != '.')
                    return true;
                if (_char == '.' && text.Contains("."))
                    return true;
                return false;
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="_char"></param>
        /// <returns></returns>
        public static bool OnyInteger(string text, char _char)
        {
            try
            {
                int x = 0;
                if (!int.TryParse(_char.ToString(), out x) && _char != '\b')
                    return true;
                return false;
            }
            catch
            {
                throw;
            }
        }
    }
}
