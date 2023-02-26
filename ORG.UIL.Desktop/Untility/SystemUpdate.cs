using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORG.UIL.Desktop.Untility
{
    public class SystemUpdate
    {        
        public static bool IsRuning = false;
        public static System.Timers.Timer timer = new System.Timers.Timer(180000);

        public void StartProcessing()
        {
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }

        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!IsRuning  && Helper.CheckInternetConnection())
            {
                IsRuning = true;
                new Thread(() =>
                {
                    try
                    {
                      var  url = ConfigurationManager.AppSettings["SysUpdate"];
                       var CurrntVersion = ConfigurationManager.AppSettings["Version"];
                      var   newVersion = (new WebClient()).DownloadString(url + @"AppFiles/infoVersion.txt");
                        if (newVersion != CurrntVersion)
                        {                          
                            string title = "New version!", body = "A new version of the program has been found. Click for it.";
                            if (Application.OpenForms["frm_Main"] != null)
                                ((frm_Main)Application.OpenForms["frm_Main"]).MessageUpdate.ShowBalloonTip(1000, title, body, ToolTipIcon.Info);
                        }                       
                    }
                    catch (Exception ex)
                    {

                    }
                    IsRuning = false;
                }).Start();
            }
        }
    }
}
