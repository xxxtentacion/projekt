using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auth.Interface
{
    public partial class Login : Form
    {
        // #DET HER ER TIL AT MAN KAN DRAG FORMEN UDEN EN FORM BORDER STYLE#
        private readonly HttpClient _httpClient;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public Login()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        // #DET HER ER TIL AT MAN KAN DRAG FORMEN UDEN EN FORM BORDER STYLE#
        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void siticonePanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                string licenseKey = siticoneTextBox1.Text; // gem value fra formens textbox "license input"
                string hwid = GetHWID(); // Methode til at få gemt hwid i variablen hwid

                // her bliver api kaldet gjort færdigt
                // {Uri.EscapeDataString(string)} er en metode i C# som encoder en string til at virke i et API call uden fejl.
                // for eksempel hvis min licensekey havde et mellemrum som "license key 1" så ville det blive ændret til "license%20key%201"
                // altså mellemrummet er blevet ændret til %20
                string requestUrl = $"https://localhost:7123/api/License/Login?licenseKey={Uri.EscapeDataString(licenseKey)}&hwid={Uri.EscapeDataString(hwid)}";

                // empty HttpContent
                var content = new StringContent(string.Empty);

                // Send HTTP POST request
                HttpResponseMessage response = await _httpClient.PostAsync(requestUrl, content);

                // tjekker om requesten var successful
                response.EnsureSuccessStatusCode();

                // Read the response content
                string responseBody = await response.Content.ReadAsStringAsync();

                // LoginResponse.cs bliver brugt her til at lave responses mere simpel ved at decode/deserialize den af en art.
                var loginResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponse>(responseBody);

                // admin tjek
                if (loginResponse != null && loginResponse.isAdmin)
                {
                    // Open Admin Form
                    var adminForm = new Admin(licenseKey);
                    adminForm.Show();
                    var userForm = new User(licenseKey);
                    userForm.Show();
                    this.Hide();
                }
                else if (loginResponse != null && loginResponse.isAdmin == false)
                {
                    // Open User Form
                    var userForm = new User(licenseKey);
                    userForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Wrong HWID or banned", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Her får vi serie nummeret af brugerens C drev og det bliver så gemt til at tjekke når brugeren logger ind og kaldet login api"
        public static string GetHWID()
        {
            try
            {
                string volumeSerialNumber = "";
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk WHERE DeviceID='C:'"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        volumeSerialNumber = obj["VolumeSerialNumber"].ToString();
                    }
                }
                return volumeSerialNumber;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving HWID: " + ex.Message);
                return "Error";
            }
        }

        // #DET HER ER TIL AT MAN KAN DRAG FORMEN UDEN EN FORM BORDER STYLE#
        private void siticoneLabel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        // #DET HER ER TIL AT MAN KAN DRAG FORMEN UDEN EN FORM BORDER STYLE#
        private void siticonePanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        // Custom minimize knap til vores navbar nu hvor vi ikke har en form border style som laver det automatisk
        private void siticoneButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Custom minimize knap til vores navbar nu hvor vi ikke har en form border style som laver det automatisk
        private void siticoneButton2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        // #DET HER ER TIL AT MAN KAN DRAG FORMEN UDEN EN FORM BORDER STYLE#
        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
