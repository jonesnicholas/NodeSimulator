using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeSimulator
{
    public class Simulation
    {
        public NodeLayout layout;

        Simulation(NodeLayout inLayout = null)
        {
            layout = inLayout;
        }
    }
}
