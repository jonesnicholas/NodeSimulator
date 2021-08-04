using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NodeSimulator
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            NodeLayout layout = new NodeLayout();
            layout.createRecursiveOctagon(5, 0, 0);
            layout.outputToFile("RecursiveOctagon");

            NodeLayout readLayout = new NodeLayout();
            readLayout.inputFromFile("RecursiveOctagon");
            Application.Run(new PrimaryForm());
        }
    }
}
