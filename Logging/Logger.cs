using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GardenCenter.Logging
{
    public abstract class LogBase
    {
        public abstract void Log(string message);
    }

    public class Logger : LogBase
    {
        private string CurrentDirectory{
            get;
            set;
        }

        private string FileName 
        {
            get; 
            set;
        }

        private string FilePath
        {
            get;
            set;
        }

        public Logger()
        {
            this.CurrentDirectory = Directory.GetCurrentDirectory();
            this.FileName = "Log.txt";
            this.FilePath = this.CurrentDirectory + "/" + this.FileName;
        }

        public override void Log(string message)
        {
            using (System.IO.StreamWriter w = System.IO.File.AppendText(this.FilePath))
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                w.WriteLine("  :{0}", message);
                w.WriteLine("-----------------------------------------------");
            }
        }
    }
}