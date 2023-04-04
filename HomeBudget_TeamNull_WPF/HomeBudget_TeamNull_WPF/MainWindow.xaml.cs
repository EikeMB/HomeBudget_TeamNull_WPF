using Budget;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using TextBox = System.Windows.Controls.TextBox;
using TextBlock = System.Windows.Controls.TextBlock;
using RadioButton = System.Windows.Controls.RadioButton;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using System.IO;

namespace HomeBudget_TeamNull_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,ViewInterface
    {
        private string fileName = "";
        private string folderName = "";

        Presenter presenter;
        public MainWindow()
        {

            InitializeComponent();
            presenter = new Presenter(this, fileName);
            ShowMenu();
        }

       private void ShowMenu()
        {

        }

        


        public void DisplayAddedCategory(Category category)
        {
            MessageBox.Show(category.Description, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void DisplayAddedExpense(Expense expense)
        {
            System.Windows.MessageBox.Show(expense.Description, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void DisplayError(string error)
        {
           MessageBox.Show(error,"Error",MessageBoxButton.OK,MessageBoxImage.Warning);
        }

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
                    fileName = dialog.FileName;

            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }


        public void DisplayAddedExpense(DateTime date, int catId, double amount, string desc)
        {
            throw new NotImplementedException();
        }

        public void DisplayAddedCategory(string desc, string type)
        {
            throw new NotImplementedException();
        }

        private void DescInput_GotMouseCapture(object sender, MouseEventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            if (txtbox.Text == "Description...")
            {
                txtbox.Text = string.Empty;
            }
        }

        private void add_Cat_btn_Click(object sender, RoutedEventArgs e)
        {
            string description = DescInput.Text;
            string type = "";
            foreach(RadioButton radio in radioBtns.Children)
            {
                if (radio.IsChecked == true)
                {
                    type = radio.Content.ToString();
                }
            }
            presenter.processAddCategory(description, type);
        }

        private void cat_cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            DescInput.Text = string.Empty;
            income_rdb.IsChecked = true;
        }

        private void cat_preview_btn_Click(object sender, RoutedEventArgs e)
        {
            string description = DescInput.Text;
            string type = "";
            foreach (RadioButton radio in radioBtns.Children)
            {
                if (radio.IsChecked == true)
                {
                    type = radio.Content.ToString();
                }
            }

            catDescDisplay.Text = description;
            catTypeDisplay.Text = type;
        }

        private void cat_Preview_clear_btn_Click(object sender, RoutedEventArgs e)
        {
            catTypeDisplay.Text = catDescDisplay.Text = string.Empty;
        }

        private void AddCategory(object sender, RoutedEventArgs e)
        {

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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error");
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void OpenFolder(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.InitialDirectory = "c:\\";
            folderDialog.ShowNewFolderButton = true;
            folderDialog.ShowDialog();
           
           
        }
    }
}
