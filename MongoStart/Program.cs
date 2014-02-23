using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using com.techphernalia.windows.forms.techieUI;

namespace MongoStart
{
    static class Program
    {
        public static String Title = "Mongo Learning";
        public static MongoData MongoData;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMongoSetup());

            List<TreeDataList> tutorials = new List<TreeDataList>();
            tutorials.Add(new About());
            tutorials.Add(new BSONTutorial());

            Application.Run(new frmMain("Learning Mongo the techie way...", tutorials,"C#"));
        }
    }
}
