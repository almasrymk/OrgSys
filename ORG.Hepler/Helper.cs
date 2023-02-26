using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ORG
{
    public enum SoudType { MenuMouseEnter, MenuMouseSelect, ButtonMouseEnter, ButtonMouseClick, GroupMouseEnter, GroupMouseSelect, ItemMouseEnter, ItemMouseClick, ControlCkick, LangClick, InClick, OutClick, DeleteClick, AddQuantity, SaveInvoice };
    public class Helper
    {

        public static bool ButtonSound { get; set; }
        public static bool MusicSound { get; set; }
        public static int Volume { get; set; }
        public static MediaPlayer MyMusic = new MediaPlayer();

        public static void Sound(string StartupPath, SoudType soudType)
        {
            if (ButtonSound)
            {
                SoundPlayer simpleSound;
                switch (soudType)
                {
                    case SoudType.MenuMouseEnter:
                        simpleSound = new SoundPlayer(StartupPath + "/Click2-Sebastian-759472264.wav");
                        simpleSound.Play();
                        break;
                    case SoudType.MenuMouseSelect:
                        simpleSound = new SoundPlayer(StartupPath + "/Tiny Button Push-SoundBible.com-513260752.wav");
                        simpleSound.Play();
                        break;
                    case SoudType.ButtonMouseEnter:
                        break;
                    case SoudType.ButtonMouseClick:
                        break;
                    case SoudType.GroupMouseEnter:
                        simpleSound = new SoundPlayer(StartupPath + "/media.io_multimedia_button_click_024.wav");
                        simpleSound.Play();
                        break;
                    case SoudType.GroupMouseSelect:
                        simpleSound = new SoundPlayer(StartupPath + "/Button Click On-SoundBible.com-459633989.wav");
                        simpleSound.Play();
                        break;
                    case SoudType.ItemMouseEnter:
                        simpleSound = new SoundPlayer(StartupPath + "/Button-SoundBible.com-1420500901.wav");
                        simpleSound.Play();
                        break;
                    case SoudType.ItemMouseClick:
                        simpleSound = new SoundPlayer(StartupPath + "/Button Click-SoundBible.com-1931397433.wav");
                        simpleSound.Play();
                        break;
                    case SoudType.ControlCkick:
                        break;
                    case SoudType.LangClick:
                        simpleSound = new SoundPlayer(StartupPath + "/Click2-Sebastian-759472264.wav");
                        simpleSound.Play();
                        break;
                    case SoudType.InClick:
                        simpleSound = new SoundPlayer(StartupPath + "/Button Click-SoundBible.com-1931397433.wav");
                        simpleSound.Play();
                        break;
                    case SoudType.OutClick:
                        simpleSound = new SoundPlayer(StartupPath + "/Button Click-SoundBible.com-1931397433.wav");
                        simpleSound.Play();
                        break;
                    case SoudType.DeleteClick:
                        simpleSound = new SoundPlayer(StartupPath + "/Button Clicking-SoundBible.com-1362000724.wav");
                        simpleSound.Play();
                        break;
                    case SoudType.AddQuantity:
                        simpleSound = new SoundPlayer(StartupPath + "/media.io_multimedia_button_click_024.wav");
                        simpleSound.Play();
                        break;
                    case SoudType.SaveInvoice:
                        simpleSound = new SoundPlayer(StartupPath + "/Dry Fire Gun-SoundBible.com-2053652037.wav");
                        simpleSound.Play();
                        break;
                    default:
                        break;
                }
            }
        }

        public static void Music(string StartupPath)
        {
            if (MusicSound)
            {
                MyMusic = new MediaPlayer();
                MyMusic.Open(new Uri(StartupPath + "/Huawei_-_Sunday_Morning.wav"));
                MyMusic.MediaEnded += new EventHandler(Media_Ended);
                MyMusic.Play();
                MyMusic.Volume = Volume / 100.0f;
            }
        }

        private static void Media_Ended(object sender, EventArgs e)
        {
            if (MusicSound)
            {
                MyMusic.Position = TimeSpan.Zero;
                MyMusic.Play();
                MyMusic.Volume = Volume / 100.0f;
            }
        }

        public static void SetVolume(int val)
        {
            MyMusic.Volume = val / 100.0f;
        }

        public static void PlayStop(string StartupPath ,bool status)
        {
            MusicSound = true;
            MyMusic.Open(new Uri(StartupPath + "/Huawei_-_Sunday_Morning.wav"));
            MyMusic.MediaEnded += new EventHandler(Media_Ended);
            MyMusic.Volume = Volume / 100.0f;
            MusicSound = status;

            if (MusicSound)
                MyMusic.Play();
            else
                MyMusic.Stop();
        }

        public static bool CheckInternetConnection()
        {
            String host = "organizersys.com";
            Ping p = new Ping();
            try
            {
                Ping myPing = new Ping();              
                byte[] buffer = new byte[32];
                int timeout = 5000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch (Exception ex){ }
            return false;
        }

        public static bool DownloadDatabaseOffline(string StartupPath)
        {
            bool FinishDownload = false;
            try
            {
                var url = ConfigurationManager.AppSettings["SysUpdate"];
                WebClient Client = new WebClient();           
                Uri u = new Uri(url + @"Database/Org.msi");
                Client.DownloadFile(u, StartupPath + @"\Org");
                FinishDownload = true;
            }
            catch (Exception ex)
            {
                FinishDownload = false;
            }
            return FinishDownload;
        }

        public static void OpenUpdate(string StartupPath)
        {
            try
            {
                var url = ConfigurationManager.AppSettings["SysUpdate"];
                var CurrntVersion = ConfigurationManager.AppSettings["Version"];
                var newVersion = (new WebClient()).DownloadString(url + @"AppFiles/infoVersionSysUpdate.txt");
                if (newVersion != CurrntVersion)
                {
                    var pup = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "UpdateOrg");
                    if (pup != null)
                        pup.Kill();

                    WebClient Client = new WebClient();
                    Client.DownloadFileCompleted += new AsyncCompletedEventHandler(delegate (object s, AsyncCompletedEventArgs r)
                    {
                        try
                        {
                            string[] files = Directory.GetFiles(StartupPath);
                            foreach (string file in files)
                            {
                                if (file.ToLower().Contains("updateorg"))
                                    File.Delete(file);
                            }
                            ZipFile.ExtractToDirectory(StartupPath + @"\UpdatesSysUpdate.zip", StartupPath);                  
                            Process ExternalProcess = new Process();
                            ExternalProcess.StartInfo.FileName = StartupPath + @"\UpdateOrg.exe";
                            ExternalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                            ExternalProcess.StartInfo.UseShellExecute = true;
                            ExternalProcess.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                            ExternalProcess.StartInfo.Verb = "runas";
                            ExternalProcess.Start();
                            ExternalProcess.WaitForExit();
                        }
                        catch (Exception ex)
                        {
                           
                        }
                    });
                    Uri u = new Uri(url + @"AppFiles/UpdatesSysUpdate.zip");
                    Client.DownloadFileAsync(u, StartupPath + @"\UpdatesSysUpdate.zip");
                }
                else
                {
                    var p = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "UpdateOrg");
                    if (p == null)
                    {
                        Process ExternalProcess = new Process();
                        ExternalProcess.StartInfo.FileName = StartupPath + @"\UpdateOrg.exe";
                        ExternalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                        ExternalProcess.StartInfo.UseShellExecute = true;
                        ExternalProcess.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                        ExternalProcess.StartInfo.Verb = "runas";
                        ExternalProcess.Start();
                        ExternalProcess.WaitForExit();
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
        }
    }
}
