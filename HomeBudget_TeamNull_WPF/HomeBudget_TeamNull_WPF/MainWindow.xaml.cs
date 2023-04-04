using Budget;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace HomeBudget_TeamNull_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        private string fileName = "";
        private string folderName = "";
        private Presenter presenter;

        public MainWindow()
        {

            InitializeComponent();
           
            ShowMenu();
            dp.SelectedDate = DateTime.Today;
            catCB.ItemsSource = GetCategoryList();


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
            MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                {
                    fileName = dialog.FileName;
                    MessageBox.Show("Existing DB file has been picked", "Success",MessageBoxButton.OK,MessageBoxImage.Information);
                    presenter = new Presenter(this, fileName);
                }  

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
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
                        presenter = new Presenter(this, fileName);
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
                    MessageBox.Show("DB folder has been chosen", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

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

        /
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

        private void Exp_SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = (DateTime)dp.SelectedDate;
            string category = catCB.SelectedItem.ToString();
            string description = descriptionTB.Text;

            double amount = 0;

            bool success = double.TryParse(amountTB.Text, out amount);
            if (success)
            {
                dp.SelectedDate = DateTime.Today;
                catCB.SelectedIndex = 0;
                amountTB.Clear();
                descriptionTB.Clear();

                presenter.processAddExpense(date, category, amount, description);
            }
            else
            {
                MessageBox.Show("Value entered for Amount is not a double", "Error", MessageBoxButton.OK);
            }

        }

        private void Exp_CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            catCB.SelectedIndex = 0;
            amountTB.Clear();
            descriptionTB.Clear();
        }

        public List<string> GetCategoryList()
        {

            List<string> cats = new List<string>();
            cats = presenter.GetCategoryDescriptionList();

            return cats;
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
        


    }
}