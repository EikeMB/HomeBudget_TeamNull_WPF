using Budget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using RadioButton = System.Windows.Controls.RadioButton;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using TextBox = System.Windows.Controls.TextBox;


namespace HomeBudget_TeamNull_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        private string fileName = "";
        private string folderName = "";
        private List<string> categories;
        private bool changeOccured = false;

        private DateTime previousDate;
        private string previousExpense;
        private string previousExpCat;
        private double previousAmount;

        Presenter presenter;
        public MainWindow()
        {
            InitializeComponent();
            LoadAppData();
            ShowMenu();
            dp.SelectedDate = DateTime.Today;
            catCB.ItemsSource = categories;
        }

        private void Close_Window(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (changeOccured == true || catCB.SelectedIndex == 0)
            {
                if (MessageBox.Show("Are you sure you want to exit? You will lose unsaved changes", "CLOSING", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void ShowMenu()
        {
            HideAllElements();
        }

        private void HideMenu()
        {
            menuText.Visibility = Visibility.Collapsed;
            BTN_existingDB.Visibility = Visibility.Collapsed;
            BTN_newDB.Visibility = Visibility.Collapsed;
        }

        private void HideAllElements()
        {
            DP_select.Visibility = Visibility.Collapsed;
            tabcontrol.Visibility = Visibility.Collapsed;
            saveBtn.Visibility = Visibility.Collapsed;
            cancelBtn.Visibility = Visibility.Collapsed;
            CategoryPreviewGrid.Visibility = Visibility.Collapsed;
            cat_preview_btn.Visibility = Visibility.Collapsed;
            cat_Preview_clear_btn.Visibility = Visibility.Collapsed;
            AddCategoryGrid.Visibility = Visibility.Collapsed;
            ExpenseAddBox.Visibility = Visibility.Collapsed;
            file_TB.Visibility = Visibility.Collapsed;
            name_TB.Visibility = Visibility.Collapsed;
            file_Grid.Visibility = Visibility.Collapsed;
        }

        public void DisplayAddedCategory(Category category)
        {
            MessageBox.Show(category.Description, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void DisplayAddedExpense(Expense expense)
        {
            MessageBox.Show(expense.Description, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    MessageBox.Show("Existing DB file has been picked", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    presenter = new Presenter(this, fileName);
                    categories = GetCategoryList();
                    catCB.ItemsSource = categories;
                    catCB.Items.Refresh();
                    DP_select.Visibility = Visibility.Visible;
                    tabcontrol.Visibility = Visibility.Visible;
                    HideMenu();
                    ShowExpenseTab();
                    name_TB.Text = Path.GetFileName(fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        public void DisplayAddedExpense(DateTime date, int catId, double amount, string desc)
        {
            string successMesage = $"Expense successfully added.\n\n" +
                $"Expense Date: {date.ToString()}\n" +
                $"Expense Amount: {amount}\n" +
                $"Expense Description: {desc}\n" +
                $"Expense Category: {catId}";
        }

        public void DisplayAddedCategory(string desc, string type)
        {
            string successMessage = $"Category successfully added.\n" +
                $"Category Description: {desc}\n" +
                $"Category Type: {type}";
            MessageBox.Show(successMessage);
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
            foreach (RadioButton radio in radioBtns.Children)
            {
                if (radio.IsChecked == true)
                {
                    type = radio.Content.ToString();
                }
            }
            presenter.processAddCategory(description, type);
            categories = GetCategoryList();
            catCB.ItemsSource = categories;
            catCB.Items.Refresh();
            changeOccured = false;
        }

        private void Exp_SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = (DateTime)dp.SelectedDate;
            string category = catCB.SelectedItem.ToString();
            string description = descriptionTB.Text;
            bool credit = (bool)exp_credit.IsChecked;

            double amount = 0;

            bool doubleSuccess = double.TryParse(amountTB.Text, out amount);
            bool continueAdd = true;
            
            if (previousExpCat == category && previousDate == date && previousAmount == amount && previousExpense == description)
            {
                if (MessageBox.Show("Are you sure you want to add this Expense? It is the same as the previous added expense", "CLOSING", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    continueAdd = false;
                }
            }
            if (doubleSuccess)
            {
                if (continueAdd)
                {
                    previousAmount = amount;
                    previousDate = date;
                    previousExpense = description;
                    previousExpCat = category;
                    if (credit)
                    {
                        presenter.processAddExpense(date, "Credit Card", amount * -1, description);
                    }
                    amountTB.Clear();
                    descriptionTB.Clear();

                    presenter.processAddExpense(date, category, amount, description);
                    changeOccured = false;

                    MessageBox.Show("The expense has been succesfully added", "Added Expense", MessageBoxButton.OK);
                }
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
            changeOccured = false;
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
            changeOccured = false;
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
                        MessageBox.Show("New DB file has been created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        presenter = new Presenter(this, fileName);
                        categories = GetCategoryList();
                        catCB.ItemsSource = categories;
                        catCB.Items.Refresh();
                        name_TB.Text = Path.GetFileName(fileName);

                        DP_select.Visibility = Visibility.Visible;
                        tabcontrol.Visibility = Visibility.Visible;

                        HideMenu();
                        ShowExpenseTab();
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

                    //inspiration taken from here https://stackoverflow.com/questions/10563148/where-is-the-correct-place-to-store-my-application-specific-data
                    var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    Directory.CreateDirectory(directory + "\\TicTacToeWPF");
                    string path = (Path.Combine(directory, "TicTacToeWPF", "FolderPath.txt"));

                    File.WriteAllText(path, folderName);

                    categories = GetCategoryList();
                    catCB.ItemsSource = categories;
                    catCB.Items.Refresh();
                    MessageBox.Show("DB folder has been chosen", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void catCB_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                string cat = catCB.Text;
                string type = "Expense";
                presenter.processAddCategory(cat, type);
                categories = GetCategoryList();
                catCB.ItemsSource = categories;
                catCB.Items.Refresh();


            }
            
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int tabItem = tabcontrol.SelectedIndex;

            switch (tabItem)
            {
                case 0:
                    showCategorytab();
                    break;

                case 1:
                    ShowExpenseTab();
                    break;
            }
        }

        private void KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            changeOccured = true;
        }

        private void showCategorytab()
        {
            saveBtn.Visibility = Visibility.Visible;
            cancelBtn.Visibility = Visibility.Visible;
            CategoryPreviewGrid.Visibility = Visibility.Collapsed;
            cat_preview_btn.Visibility = Visibility.Collapsed;
            cat_Preview_clear_btn.Visibility = Visibility.Collapsed;
            AddCategoryGrid.Visibility = Visibility.Collapsed;
            ExpenseAddBox.Visibility = Visibility.Visible;
            file_TB.Visibility = Visibility.Visible;
            name_TB.Visibility = Visibility.Visible;
            file_Grid.Visibility = Visibility.Visible;
        }

        private void ShowExpenseTab()
        {
            saveBtn.Visibility = Visibility.Collapsed;
            cancelBtn.Visibility = Visibility.Collapsed;
            CategoryPreviewGrid.Visibility = Visibility.Visible;
            cat_preview_btn.Visibility = Visibility.Visible;
            cat_Preview_clear_btn.Visibility = Visibility.Visible;
            AddCategoryGrid.Visibility = Visibility.Visible;
            ExpenseAddBox.Visibility = Visibility.Collapsed;
            file_TB.Visibility = Visibility.Collapsed;
            name_TB.Visibility = Visibility.Collapsed;
            file_Grid.Visibility = Visibility.Collapsed;
        }

        private void catCB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            catCB.IsDropDownOpen = true;
            string cat = catCB.Text;
            List<string> remainingCats = new List<string>();
            if (cat == "")
            {
                categories = GetCategoryList();
            }
            else
            {
                foreach (string category in categories)
                {

                    if (cat.ToLower() == category.Substring(0,cat.Length < category.Length ? cat.Length : category.Length).ToLower())
                    {
                        remainingCats.Add(category);
                    }
                }
                categories= remainingCats;
            }

            catCB.ItemsSource = categories;
            catCB.Items.Refresh();

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
    }
}