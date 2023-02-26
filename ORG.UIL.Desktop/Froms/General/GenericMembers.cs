using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;

namespace ORG.UIL.Desktop
{
    #region Public Member
    public enum TypeLang { AR, EN }
    public class OtherObject { public int ID { get; set; } public string Name { get; set; } }
    #endregion

    public class GenericMembers
    {
        #region Varible
        static string TableName = "", WhereStatement = "";
        //public static PublicInfo AllPublicInfo;
        #endregion

        #region Properties
        static TypeLang _CurrLang = TypeLang.EN;
        public static TypeLang CurrLang { get { return _CurrLang; } set { _CurrLang = value; } }
        #endregion

        #region Function
        #region Users
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="BranchID"></param>
        /// <param name="Sector"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        //public static bool Login(string UserName, string Password, int BranchID, int Sector, int Year)
        //{
        //    AllPublicInfo = (PublicInfo)Generic_Pr.GetAllPublicInfo(UserName, Password, BranchID, Sector, Year);
        //    return AllPublicInfo != null;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <param name="BranchID"></param>
        /// <param name="Sector"></param>
        /// <param name="Year"></param>
        /// <returns></returns>
        //public static bool SetPublicInfo(AUTUsers User, int BranchID, int Sector, int Year)
        //{
        //    AllPublicInfo = (PublicInfo)Generic_Pr.GetAllPublicInfo(User, BranchID, Sector, Year);
        //    return AllPublicInfo != null;
        //}
        #endregion       

        #region Search
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="_TableName"></param>
        /// <param name="_WhereStatement"></param>
        /// <param name="ScreenName"></param>
        /// <param name="OBList"></param>
        /// <returns></returns>
        public static object OpenSearch<t>(string _TableName, string _WhereStatement = null, string ScreenName = null, object OBList = null) where t : class
        {
            try
            {
                TableName = _TableName;
                WhereStatement = _WhereStatement;             
                return null;
            }
            catch
            {
                throw;
            }
        }

       
       
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        #endregion
    }
}
