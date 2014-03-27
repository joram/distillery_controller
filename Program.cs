using Gadgeteer.Modules.GHIElectronics;
using Gadgeteer.Modules.Seeed;

namespace DistilleryNamespace
{
    public partial class Program
    {
        void ProgramStarted()
        {
            Distillery still = new Distillery(display_T35, ethernet_J11D, relays);
            still.Start();
        }
    }
}
