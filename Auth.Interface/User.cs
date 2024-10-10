using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auth.Interface
{
    public partial class User : Form
    {
        // #DET HER ER TIL AT MAN KAN DRAG FORMEN UDEN EN FORM BORDER STYLE#
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        // Direction variables for the ball movement
        private float ballX = 5;
        private float ballY = 5;

        private string licenseKey;
        private List<Ball> balls = new List<Ball>();

        public User(string licenseKey)
        {
            InitializeComponent();
            this.licenseKey = licenseKey;  // Store the license key for use in API call
        }

        // #DET HER ER TIL AT MAN KAN DRAG FORMEN UDEN EN FORM BORDER STYLE#
        private void User_MouseDown(object sender, MouseEventArgs e)
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

        // #DET HER ER TIL AT MAN KAN DRAG FORMEN UDEN EN FORM BORDER STYLE#
        private void label1_MouseDown(object sender, MouseEventArgs e)
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

        public class Ball
        {
            public Panel BallPanel { get; set; }  // Panel representing the ball on the form
            public float BallX { get; set; }      // Horizontal movement speed
            public float BallY { get; set; }      // Vertical movement speed

            public Ball(Panel panel, float ballX, float ballY)
            {
                BallPanel = panel;
                BallX = ballX;
                BallY = ballY;
            }

            // Method to move the ball
            public void Move()
            {
                BallPanel.Left += (int)BallX;
                BallPanel.Top += (int)BallY;
            }

            // Reverse horizontal direction
            public void ReverseX() => BallX = -BallX;

            // Reverse vertical direction
            public void ReverseY() => BallY = -BallY;
        }



        private string GetRandomBrickAbility()
        {
            Random random = new Random();
            string[] abilities = { "Normal", "SpeedUp", "SlowDown", "DuplicateBall" };
            return abilities[random.Next(abilities.Length)];
        }

        private Color GetRandomBrickColor()
        {
            Random random = new Random();
            Color[] colors = { Color.Gray, Color.Red, Color.Blue, Color.Green };
            return colors[random.Next(colors.Length)];
        }

        private void DuplicateBall(Ball originalBall, List<Ball> newBalls)
        {
            // Create a new Panel for the duplicated ball
            Panel newBallPanel = new Panel
            {
                Size = originalBall.BallPanel.Size,
                BackColor = originalBall.BallPanel.BackColor,
                Left = originalBall.BallPanel.Left,
                Top = originalBall.BallPanel.Top
            };

            Controls.Add(newBallPanel);  // Add the new ball to the form

            // Create a new Ball object with slightly different speed or direction
            Ball newBall = new Ball(newBallPanel, originalBall.BallX + 1f, originalBall.BallY + 1f);
            newBalls.Add(newBall);  // Add the new ball to the temporary list
        }




        private void User_Load(object sender, EventArgs e)
        {
            // Initialize the first ball with the existing panelBall
            Ball initialBall = new Ball(panelBall, 5f, 5f);  // Original ball
            balls.Add(initialBall);

            Random random = new Random();
            int rows = 5;  // Number of rows of bricks
            int columns = 10;  // Number of columns of bricks
            int brickWidth = 50;
            int brickHeight = 20;

            // Set the starting position for the bricks
            int startX = 0;  // X position
            int startY = 25; // Y position

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    // Randomly decide to skip some bricks (optional)
                    if (random.Next(0, 100) < 70)  // 70% chance to create a brick
                    {
                        Panel panelBrick = new Panel
                        {
                            Name = $"panelBrick_{i}_{j}",
                            Width = brickWidth,
                            Height = brickHeight,
                            Left = startX + j * (brickWidth + 5),  // Adjusted for starting position
                            Top = startY + i * (brickHeight + 5),  // Adjusted for starting position
                            BackColor = GetRandomBrickColor(),  // Assign a random color
                            Tag = GetRandomBrickAbility()  // Store ability in Tag property
                        };
                        Controls.Add(panelBrick);
                    }
                }
            }

            // Center paddle and ball
            buttonPaddle.Left = (ClientSize.Width - buttonPaddle.Width) / 2;
            panelBall.Left = buttonPaddle.Left + (buttonPaddle.Width - panelBall.Width) / 2;
        }

        private void User_MouseMove(object sender, MouseEventArgs e)
        {
            // e.X = x cordinatet til hvor musen er på formen.
            // vi bruger .Left for at bruge den vandrette del af knappen "altså den lange side af button"
            // e.X - (buttonPaddle.Width / 2); gør at vi ændrer knappens position fra midten af knappen så midten af knappen altid vil følge vores mus.
            buttonPaddle.Left = e.X - (buttonPaddle.Width / 2);
        }

        private async void HandleGameWon()
        {
            timerGame.Stop();  // Stop the game
            await UpdateGameStats(1, 1, 1, 0);  // Update the game stats asynchronously
            await UpdateLicenseStats(licenseKey, 1, 1, 0);  // Update the license stats asynchronously
            Application.Exit();  // Close the game after win
        }


        private bool CheckBrickCollision(Ball ball, List<Ball> newBalls)
        {
            // Loop through all controls on the form
            foreach (Control brick in Controls)
            {
                // Only target the bricks (if it's a Panel and its name starts with "panelBrick")
                if (brick is Panel && brick.Name.StartsWith("panelBrick"))
                {
                    // If the current ball collides with a brick
                    if (ball.BallPanel.Bounds.IntersectsWith(brick.Bounds))
                    {
                        Controls.Remove(brick);  // Remove the brick from the form
                        ball.ReverseY();  // Change ball direction (only the current ball)

                        // If the brick has a special ability (e.g., green), duplicate the ball
                        if (brick.BackColor == Color.Green)
                        {
                            DuplicateBall(ball, newBalls);  // Duplicate this ball when green brick is hit
                        }

                        // Break out of the loop since a brick was hit
                        return false;
                    }
                }
            }

            // Check if no more bricks are left
            int remainingBricks = Controls.OfType<Panel>().Count(brick => brick.Name.StartsWith("panelBrick"));
            if (remainingBricks == 0)
            {
                HandleGameWon(); // Trigger the async game-winning logic
                return true;  // Indicate that the game has been won
            }

            return false;  // Return false to continue the game
        }

        private async void timerGame_Tick(object sender, EventArgs e)
        {
            List<Ball> newBalls = new List<Ball>();  // Temporary list for new duplicated balls

            // Iterate over a copy of the ball list
            foreach (var ball in balls.ToList())
            {
                ball.Move();  // Move the current ball

                // Bounce the ball off the left and right walls
                if (ball.BallPanel.Left <= 0 || ball.BallPanel.Right >= ClientSize.Width)
                {
                    ball.ReverseX();
                }

                // Bounce the ball off the top wall or navbar
                if (ball.BallPanel.Top <= 0 || ball.BallPanel.Bounds.IntersectsWith(siticonePanel1.Bounds))
                {
                    ball.ReverseY();
                }

                // Bounce off the paddle
                if (ball.BallPanel.Bounds.IntersectsWith(buttonPaddle.Bounds))
                {
                    ball.ReverseY();
                }

                // Check for brick collisions for this specific ball
                if (CheckBrickCollision(ball, newBalls))  // Pass newBalls to handle duplication
                {
                    break; // Exit the loop if a brick collision ended the game
                }

                // If the ball falls below the paddle (game over for this ball)
                if (ball.BallPanel.Top > ClientSize.Height)
                {
                    // Remove the ball from the list
                    balls.Remove(ball);

                    // Check if there are any balls left
                    if (balls.Count == 0)
                    {
                        timerGame.Stop();
                        await UpdateGameStats(1, 1, 0, 1);  // Game lost
                        await UpdateLicenseStats(licenseKey, 1, 0, 1);
                        Close();
                    }
                }
            }

            // After the loop, add any new balls created by duplicating
            balls.AddRange(newBalls);
        }

        private void siticoneButton3_Click(object sender, EventArgs e)
        {
            siticoneButton3.Visible = false;
            timerGame.Start();
        }

        public async Task UpdateGameStats(int id, int gamesPlayed, int gamesWon, int gamesLost)
        {
            string url = $"https://localhost:7123/api/GameStats/{id}";

            using (HttpClient client = new HttpClient())
            {
                // Step 1: Get the current game stats from the API
                HttpResponseMessage getResponse = await client.GetAsync(url);
                if (getResponse.IsSuccessStatusCode)
                {
                    string currentStatsJson = await getResponse.Content.ReadAsStringAsync();
                    GameStats currentStats = JsonConvert.DeserializeObject<GameStats>(currentStatsJson);

                    // Step 2: Add the new stats to the current stats
                    currentStats.GamesPlayed += gamesPlayed;
                    currentStats.GamesWon += gamesWon;
                    currentStats.GamesLost += gamesLost;

                    // Step 3: Serialize the updated GameStats object to JSON
                    string updatedStatsJson = JsonConvert.SerializeObject(currentStats);
                    StringContent content = new StringContent(updatedStatsJson, Encoding.UTF8, "application/json");

                    // Step 4: Send the PUT request with the updated stats
                    HttpResponseMessage putResponse = await client.PutAsync(url, content);

                    if (putResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Game stats updated successfully!");
                    }
                    else
                    {
                        MessageBox.Show($"Failed to update game stats: {putResponse.ReasonPhrase}");
                    }
                }
                else
                {
                    MessageBox.Show($"Failed to retrieve current game stats: {getResponse.ReasonPhrase}");
                }
            }
        }

        public async Task CreateNewLicenseStats(string licenseKey, int gamesPlayed, int gamesWon, int gamesLost)
        {
            string postUrl = "https://localhost:7123/api/LicenseStats";
            LicenseStats newStats = new LicenseStats
            {
                LicenseKey = licenseKey,
                GamesPlayed = gamesPlayed,
                GamesWon = gamesWon,
                GamesLost = gamesLost
            };

            using (HttpClient client = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(newStats);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage postResponse = await client.PostAsync(postUrl, content);

                if (postResponse.IsSuccessStatusCode)
                {
                    MessageBox.Show("New license stats created successfully!");
                }
                else
                {
                    MessageBox.Show($"Failed to create new license stats: {postResponse.ReasonPhrase}");
                }
            }
        }
        public async Task UpdateLicenseStats(string licenseKey, int gamesPlayed, int gamesWon, int gamesLost)
        {
            // The URL to get LicenseStats by licenseKey
            string getUrl = $"https://localhost:7123/api/LicenseStats/by-license/{licenseKey}";

            using (HttpClient client = new HttpClient())
            {
                // Get the current stats for the given licenseKey
                HttpResponseMessage getResponse = await client.GetAsync(getUrl);

                LicenseStats currentStats = null;

                if (getResponse.IsSuccessStatusCode)
                {
                    // Deserialize the existing stats
                    string json = await getResponse.Content.ReadAsStringAsync();
                    currentStats = JsonConvert.DeserializeObject<LicenseStats>(json);

                    if (currentStats != null)
                    {
                        // Increment the stats
                        currentStats.GamesPlayed += gamesPlayed;
                        currentStats.GamesWon += gamesWon;
                        currentStats.GamesLost += gamesLost;

                        // Now send the updated LicenseStats back to the API using the existing id
                        string updateUrl = $"https://localhost:7123/api/LicenseStats/{currentStats.Id}";
                        string updatedJson = JsonConvert.SerializeObject(currentStats);
                        StringContent content = new StringContent(updatedJson, Encoding.UTF8, "application/json");

                        HttpResponseMessage putResponse = await client.PutAsync(updateUrl, content);

                        if (putResponse.IsSuccessStatusCode)
                        {
                            MessageBox.Show("License stats updated successfully!");
                        }
                        else
                        {
                            MessageBox.Show($"Failed to update license stats: {putResponse.ReasonPhrase}");
                        }
                    }
                    else
                    {
                        // Handle if stats were not found, create new stats
                        MessageBox.Show("License stats not found, creating a new entry.");
                        await CreateNewLicenseStats(licenseKey, gamesPlayed, gamesWon, gamesLost);
                    }
                }
                else
                {
                    // If GET request fails or no stats exist, create new stats
                    MessageBox.Show("Failed to fetch license stats, creating new entry.");
                    await CreateNewLicenseStats(licenseKey, gamesPlayed, gamesWon, gamesLost);
                }
            }
        }

    }
}