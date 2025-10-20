using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogisticDashboard.Forms
{
    public partial class VesselRouteMap: Form
    {
        public int shipId;
        public string vesselName;
        public VesselRouteMap(int shipId, string vesselName)
        {
            InitializeComponent();
            this.shipId = shipId;
            this.vesselName = vesselName;

            webView21.Source = new Uri(@"https://www.marinetraffic.com/en/ais/details/ships/shipid:" + shipId.ToString());
        }
    }
}