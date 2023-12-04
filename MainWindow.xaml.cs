using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp;
using CefSharp.Wpf;
using System.IO;
using Newtonsoft.Json;

namespace Old_Client_dupe
{
    
    public partial class MainWindow : Window
    {
        private static readonly string SettingsFolderPath = System.IO.Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
      "OldClientDupe");

        private static readonly string SettingsFilePath = System.IO.Path.Combine(SettingsFolderPath, "userSettingsRememberUsername.json");
        public MainWindow()
        {
            InitializeComponent();
            EnsureSettingsFile();
            LoadUserSettings();
        }

        
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
           
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password; 

            // Perform login logic using database
            SaveUserSettings(username, RememberUsernameCheckBox.IsChecked ?? false);
            this.Close();

        }

        private void LoadUserSettings()
        {
            if (File.Exists(SettingsFilePath))
            {
                string json = File.ReadAllText(SettingsFilePath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var settings = JsonConvert.DeserializeObject<UserSettings>(json);
                    if (settings.RememberUsername)
                    {
                        UsernameTextBox.Text = settings.Username;
                        RememberUsernameCheckBox.IsChecked = true;
                    }
                }
            }
        }

        private void SaveUserSettings(string username, bool rememberUsername)
        {
            try
            {
                var settings = new UserSettings
                {
                    Username = rememberUsername ? username :"",
                    RememberUsername = rememberUsername
                };

                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving settings: " + ex.Message);
            }
        }


        private void EnsureSettingsFile()
        {
            
            if (!Directory.Exists(SettingsFolderPath))
            {
                Directory.CreateDirectory(SettingsFolderPath);
            }

            
            if (!File.Exists(SettingsFilePath))
            {
                File.Create(SettingsFilePath).Close();
                SaveUserSettings("", false); 
            }
        }


    }
    public class UserSettings
    {
        public bool RememberUsername { get; set; }
        public string Username { get; set; }
    }
}
