using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using RadioButton = System.Windows.Controls.RadioButton;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using TabControl = System.Windows.Controls.TabControl;
using TextBox = System.Windows.Controls.TextBox;

namespace HomeBudget_TeamNull_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        private string? fileName = "";
        private string? folderName = "";
        private List<string> categories;
        private bool changeOccured = false;

        private Presenter presenter;

        //warning about presenter being null has to stay for code to work.
        public MainWindow()
        {                    
            InitializeComponent();
            LoadAppData();
            ShowMenu();

        }

        #region closeWindow

        private void Close_Window(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (changeOccured == true)
            {
                if (MessageBox.Show("Are you sure you want to exit? You will lose unsaved changes", "CLOSING", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void txt_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            changeOccured = true;
        }

        #endregion closeWindow

        #region menu

        private void ShowMenu()
        {
          
            menuText.Visibility = Visibility.Visible;
            BTN_existingDB.Visibility = Visibility.Visible;
            BTN_newDB.Visibility = Visibility.Visible;
           
        }

        private void HideMenu()
        {
            menuText.Visibility = Visibility.Collapsed;
            BTN_existingDB.Visibility = Visibility.Collapsed;
            BTN_newDB.Visibility = Visibility.Collapsed;
          
        }

        #endregion menu


      
        #region openDBS

        private void OpenExistingDb(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (folderName == "")
                {
                    dialog.InitialDirectory = "c:\\";
                }
                else
                {
                    dialog.InitialDirectory = folderName;
                }
                dialog.Filter = "Database File (*.db)|*.db";

                if (dialog.ShowDialog() == true)
                {
                    fileName = dialog.FileName;
                    MessageBox.Show("Existing DB file has been picked", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    folderName = System.IO.Path.GetDirectoryName(dialog.FileName);
                    WriteAppData();

                    presenter = new Presenter(this, fileName, false);

                    HideMenu();

                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        public void DisplayError(string error)
        {
            MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void OpenNewDb(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                if (folderName == "")
                {
                    saveDialog.InitialDirectory = "c:\\";
                }
                else
                {
                    saveDialog.InitialDirectory = folderName;
                }
                saveDialog.Filter = "Database File (*.db)|*.db";
                saveDialog.FileName = "dbName";
                saveDialog.DefaultExt = ".db";

                if (saveDialog.ShowDialog() == true)
                {
                    fileName = saveDialog.FileName;
                    try
                    {
                        File.WriteAllText(fileName, "");
                        MessageBox.Show("New DB file has been created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        presenter = new Presenter(this, fileName, true);

                        folderName = Path.GetDirectoryName(fileName);

                        WriteAppData();

               
                        HideMenu();
                 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void OpenFolder(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                folderDialog.InitialDirectory = "c:\\";
                folderDialog.ShowNewFolderButton = true;
                DialogResult result = folderDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    folderName = folderDialog.SelectedPath;

                    WriteAppData();

                    MessageBox.Show("DB folder has been chosen", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void WriteAppData()
        {
            //inspiration taken from here https://stackoverflow.com/questions/10563148/where-is-the-correct-place-to-store-my-application-specific-data
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Directory.CreateDirectory(directory + "\\TicTacToeWPF");
            string path = (Path.Combine(directory, "TicTacToeWPF", "FolderPath.txt"));

            File.WriteAllText(path, folderName);
        }

        private void LoadAppData()
        {
            try
            {
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string path = Path.Combine(directory, "TicTacToeWPF", "FolderPath.txt");
                if (File.Exists(path))
                {
                    string contents = File.ReadAllText(path);

                    folderName = contents;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        #endregion openDBS
        #region colors
        private void ColorChangeMenu(object sender, RoutedEventArgs e)
        {
            
            HideMenu();
          

        }

        private void hideColorMenu()
        {
          
            if (fileName == "")
            {
                ShowMenu();
            }
            else
            {
                
            }
        }

        private void colorMenuCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            hideColorMenu();
         
        }

        private void buttonColor_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();

           

        }

        private void BackgroundColorBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();
            WindowBox.Background = brush;
        }

        private void txtfieildBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();
           
        }

        private void boxColorBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();
           
        }

        private SolidColorBrush colorPicker()
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowDialog();
            System.Drawing.Color color = colorDialog.Color;
            Color newColor = Color.FromArgb(color.A, color.R, color.G, color.B);

            SolidColorBrush brush = new SolidColorBrush(newColor);

            return brush;
        }

        public void DisplayAddedExpense(DateTime date, string catId, double amount, string desc)
        {
            throw new NotImplementedException();
        }

        public void DisplayAddedCategory(string desc, string type)
        {
            throw new NotImplementedException();
        }

        public List<string> GetCategoryList()
        {
            throw new NotImplementedException();
        }
        #endregion

        private void OpenAddWindow(object sender, RoutedEventArgs e)
        {
            AddWindow window2 = new AddWindow(presenter);
            window2.Show();
           
        }
    }
}