using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogisticDashboard.Forms;
using System.Data.SqlClient;

namespace LogisticDashboard
{
    public partial class MainForm: Form
    {
        // Constants
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        // Import user32.dll for sending messages
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        public static string localIP;
        public static string UserIdNumber = "";

        private string centralizedLoginConnString = "Data Source=APBIPHBPSDB02;Initial Catalog=Centralized_LOGIN_DB;Persist Security Info=True;User ID=CAS_access;Password=@BIPH2024";
        public static string GetLocalIPAddress()
        {
            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private void GetIPAddressFromCentralizedLogin()
        {
            SqlConnection CentralizedLogin = new SqlConnection(centralizedLoginConnString);

            CentralizedLogin.Open();
            SqlCommand SelectUserAccount = new SqlCommand("SP_SelectLoginRequestFromCentralizedLogin", CentralizedLogin);
            SelectUserAccount.CommandType = CommandType.StoredProcedure;
            SelectUserAccount.Parameters.AddWithValue("@IPAddress", localIP);
            SelectUserAccount.Parameters.AddWithValue("@SystemID", "64"); //palitan ng assigned system id
            SqlDataAdapter da = new SqlDataAdapter(SelectUserAccount);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                SqlDataReader reader = SelectUserAccount.ExecuteReader();

                if (reader.Read())
                {
                    UserIdNumber = reader["USERNAME"].ToString(); //ito ay depende kong anong gamit nyo na user name (ADID/ID number)
                }
            }
            else
            {

                LoginReminders loginReminders = new LoginReminders();
                loginReminders.ShowDialog();
                return;
            }
        }

        public MainForm()
        {
            localIP = GetLocalIPAddress();
            GetIPAddressFromCentralizedLogin();

            InitializeComponent();
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks == 2) // Detect double-click
                {
                    this.WindowState = this.WindowState == FormWindowState.Normal
                        ? FormWindowState.Maximized
                        : FormWindowState.Normal;
                }
                else
                {
                    // Handle dragging the form
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = this.WindowState == FormWindowState.Normal
               ? FormWindowState.Maximized
               : FormWindowState.Normal;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
