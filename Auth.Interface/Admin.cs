using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;


namespace Auth.Interface
{
    public partial class Admin : Form
    {
        private readonly HttpClient _httpClient;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private string licenseKey;
        private List<DataGridViewRow> modifiedRows = new List<DataGridViewRow>();

        public Admin(string licenseKey)
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            this.licenseKey = licenseKey;  // Store the license key for use in API call
        }

        private void siticonePanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void siticoneLabel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void siticonePanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void siticoneButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void siticoneButton2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Admin_Load(object sender, EventArgs e)
        {

        }

        private void Admin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public class License
        {
            public int Id { get; set; }
            public string LicenseKey { get; set; }
            public bool IsBanned { get; set; }
            public bool IsAdmin { get; set; }
            public string Hwid { get; set; }
        }

        public class GameStats
        {
            public int Id { get; set; }
            public int gamesPlayed { get; set; }
            public int gamesWon { get; set; }
            public int gamesLost { get; set; }
        }

        public class LicenseStats
        {
            public int Id { get; set; }
            public string licenseKey { get; set; }
            public int gamesPlayed { get; set; }
            public int gamesWon { get; set; }
            public int gamesLost { get; set; }
        }

        private async void siticoneButton3_Click(object sender, EventArgs e)
        {
            string url = "https://localhost:7123/api/License/GetAll";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Make the GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    // Read the response content as a string
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON into a List of License objects
                    List<License> licenses = JsonConvert.DeserializeObject<List<License>>(responseBody);

                    // Bind the data to the DataGridView
                    dataGridView1.DataSource = licenses;
                    dataGridView1.Visible = true;
                }
                catch (Exception ex)
                {
                    label2.Text = "Status: Error fetching data.";
                }
            }
        }

        private async void siticoneButton4_Click(object sender, EventArgs e)
        {
            // Build the URL with the license key
            string url = $"https://localhost:7123/api/License/GetBannedLicences?licenseKey={licenseKey}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Make the GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    // Read the response content as a string
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON into a List of License objects
                    List<License> licenses = JsonConvert.DeserializeObject<List<License>>(responseBody);

                    // Bind the data to the DataGridView
                    dataGridView1.DataSource = licenses;
                    dataGridView1.Visible = true;
                }
                catch (Exception ex)
                {
                    label2.Text = "Status: Error fetching data.";
                }
            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Get the updated row
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            // Add the row to the list of modified rows if it's not already there
            if (!modifiedRows.Contains(row))
            {
                modifiedRows.Add(row);
            }
        }

        private async Task UpdateLicense(string adminLicenseKey, string licenseId, bool isBanned, bool isAdmin)
        {
            // Construct the URL with adminLicenseKey and licenseId parameters
            string url = $"https://localhost:7123/api/License/Update?adminLicenseKey={adminLicenseKey}&licenseId={licenseId}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Create MultipartFormDataContent to send form data
                    var formData = new MultipartFormDataContent
            {
                { new StringContent(isBanned.ToString().ToLower()), "isBanned" },
                { new StringContent(isAdmin.ToString().ToLower()), "isAdmin" }
            };

                    // Send the PUT request
                    HttpResponseMessage response = await client.PutAsync(url, formData);

                    // Ensure the request was successful
                    response.EnsureSuccessStatusCode();

                    //MessageBox.Show("License updated successfully.");
                }
                catch (Exception ex)
                {
                    label2.Text = "Status: Error updating license.";
                }
            }
        }

        private async void siticoneButton5_Click(object sender, EventArgs e)
        {
            string adminLicenseKey = licenseKey;  // Replace with actual admin license key

            foreach (DataGridViewRow row in modifiedRows)
            {
                string licenseId = row.Cells["Id"].Value.ToString(); // License ID from the row
                bool isBanned = Convert.ToBoolean(row.Cells["IsBanned"].Value);
                bool isAdmin = Convert.ToBoolean(row.Cells["IsAdmin"].Value);

                // Call the API to save changes to the database for each modified row
                await UpdateLicense(adminLicenseKey, licenseId, isBanned, isAdmin);
            }

            // Clear the modified rows list after saving
            modifiedRows.Clear();
            label2.Text = "Status: All changes have been saved successfully.";
        }

        private async void siticoneButton6_Click(object sender, EventArgs e)
        {
            // Show an input dialog for the admin to input the licenseId
            string licenseId = Microsoft.VisualBasic.Interaction.InputBox("Enter the License ID to delete:", "Delete License", "");

            if (!string.IsNullOrEmpty(licenseId))
            {
                // Get the admin license key (replace with actual admin license key retrieval)
                string adminLicenseKey = licenseKey;  // Replace with actual admin license key

                // Confirm deletion
                var result = MessageBox.Show($"Are you sure you want to delete license with ID: {licenseId}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    // Call the DeleteLicense function to delete the license
                    await DeleteLicense(adminLicenseKey, licenseId);
                }
            }
            else
            {
                label2.Text = "Status: License ID cannot be empty.";
            }
        }

        private async Task DeleteLicense(string adminLicenseKey, string licenseId)
        {
            string url = $"https://localhost:7123/api/License/Delete?adminLicenseKey={adminLicenseKey}&licenseId={licenseId}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Make the DELETE request
                    HttpResponseMessage response = await client.DeleteAsync(url);
                    response.EnsureSuccessStatusCode();
                    label2.Text = "Status: License deleted successfully from the database.";

                    // Refresh the DataGridView by calling siticoneButton3_Click to get all licenses
                    siticoneButton3_Click(null, null);
                }
                catch (Exception ex)
                {
                    label2.Text = "Status: Error deleting license.";
                }
            }
        }

        private async void siticoneButton7_Click(object sender, EventArgs e)
        {
            // Get the admin license key (replace with actual admin license key retrieval)
            string adminLicenseKey = licenseKey;  // Replace with actual admin license key

            // Call the DeleteLicense function to delete the license
            await GenerateLicense(adminLicenseKey);
        }

        private async Task GenerateLicense(string adminLicenseKey)
        {
            string url = $"https://localhost:7123/api/License/Generate?adminLicenseKey={adminLicenseKey}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Make the POST request to generate a new license
                    HttpResponseMessage response = await client.PostAsync(url, null);
                    response.EnsureSuccessStatusCode();

                    // Show a success message
                    label2.Text = "Status: New license generated successfully.";
                    // Optionally, refresh the DataGridView by calling GetAll to show the new license
                    siticoneButton3_Click(null, null); // Refresh the license list
                }
                catch (Exception ex)
                {
                    label2.Text = "Status: Error generating license.";
                }
            }
        }

        private async void siticoneButton8_Click(object sender, EventArgs e)
        {
            string url = "https://localhost:7123/api/GameStats";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Make the GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    // Read the response content as a string
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON into a List of GameStats objects
                    List<GameStats> gameStats = JsonConvert.DeserializeObject<List<GameStats>>(responseBody);

                    // Bind the data to the DataGridView
                    dataGridView1.DataSource = gameStats;
                    dataGridView1.Visible = true;
                    dataGridView1.ReadOnly = true;
                }
                catch (Exception ex)
                {
                    label2.Text = "Status: Error fetching data.";
                }
            }
        }

        private async void siticoneButton9_Click(object sender, EventArgs e)
        {
            string url = "https://localhost:7123/api/LicenseStats";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Make the GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    // Read the response content as a string
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON into a List of LicenseStats objects
                    List<LicenseStats> licenseStats = JsonConvert.DeserializeObject<List<LicenseStats>>(responseBody);

                    // Bind the data to the DataGridView
                    dataGridView1.DataSource = licenseStats;
                    dataGridView1.Visible = true;
                    dataGridView1.ReadOnly = true;
                }
                catch (Exception ex)
                {
                    label2.Text = "Status: Error fetching data.";
                }
            }
        }
    }
}
