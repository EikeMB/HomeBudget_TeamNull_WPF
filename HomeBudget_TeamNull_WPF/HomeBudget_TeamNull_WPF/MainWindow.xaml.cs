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

        private DateTime previousDate;
        private string? previousExpense;
        private string? previousExpCat;
        private double? previousAmount;

        private Presenter presenter;

        //warning about presenter being null has to stay for code to work.
        public MainWindow()
        {
            /*
            //Uncomment to see second window
            Window1 win2 = new Window1();
            win2.Show();
            */

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
            HideAllElements();
            menuText.Visibility = Visibility.Visible;
            BTN_existingDB.Visibility = Visibility.Visible;
            BTN_newDB.Visibility = Visibility.Visible;
            DP_select.Visibility = Visibility.Collapsed;
            tabcontrol.Visibility = Visibility.Collapsed;
        }

        private void HideMenu()
        {
            menuText.Visibility = Visibility.Collapsed;
            BTN_existingDB.Visibility = Visibility.Collapsed;
            BTN_newDB.Visibility = Visibility.Collapsed;
            DP_select.Visibility = Visibility.Visible;
            tabcontrol.Visibility = Visibility.Visible;
            colorMenuBtn.Visibility = Visibility.Visible;
        }

        #endregion menu

        #region elementViews

        private void HideAllElements()
        {
            DP_select.Visibility = Visibility.Collapsed;
            tabcontrol.Visibility = Visibility.Collapsed;
            addExpenseBtn.Visibility = Visibility.Collapsed;
            cancelExpenseBtn.Visibility = Visibility.Collapsed;
            CategoryPreviewGrid.Visibility = Visibility.Collapsed;
            cat_preview_btn.Visibility = Visibility.Collapsed;
            cat_Preview_clear_btn.Visibility = Visibility.Collapsed;
            AddCategoryGrid.Visibility = Visibility.Collapsed;
            ExpenseAddBox.Visibility = Visibility.Collapsed;
            file_TB.Visibility = Visibility.Collapsed;
            name_TB.Visibility = Visibility.Collapsed;
            file_Grid.Visibility = Visibility.Collapsed;
        }

        private void ShowExpenseTab()
        {
            addExpenseBtn.Visibility = Visibility.Visible;
            cancelExpenseBtn.Visibility = Visibility.Visible;
            CategoryPreviewGrid.Visibility = Visibility.Collapsed;
            cat_preview_btn.Visibility = Visibility.Collapsed;
            cat_Preview_clear_btn.Visibility = Visibility.Collapsed;
            AddCategoryGrid.Visibility = Visibility.Collapsed;
            ExpenseAddBox.Visibility = Visibility.Visible;
            file_TB.Visibility = Visibility.Visible;
            name_TB.Visibility = Visibility.Visible;
            file_Grid.Visibility = Visibility.Visible;
        }

        private void showCategorytab()
        {
            addExpenseBtn.Visibility = Visibility.Collapsed;
            cancelExpenseBtn.Visibility = Visibility.Collapsed;
            CategoryPreviewGrid.Visibility = Visibility.Visible;
            cat_preview_btn.Visibility = Visibility.Visible;
            cat_Preview_clear_btn.Visibility = Visibility.Visible;
            AddCategoryGrid.Visibility = Visibility.Visible;
            ExpenseAddBox.Visibility = Visibility.Collapsed;
            file_TB.Visibility = Visibility.Collapsed;
            name_TB.Visibility = Visibility.Collapsed;
            file_Grid.Visibility = Visibility.Collapsed;
            dp.SelectedDate = DateTime.Today;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int tabItem = tabcontrol.SelectedIndex;

            switch (tabItem)
            {
                case 0:
                    ShowExpenseTab();
                    break;

                case 1:
                    showCategorytab();
                    break;
            }
        }

        #endregion elementViews

        #region displays

        public void DisplayAddedExpense(DateTime date, string cat, double amount, string desc)
        {
            string successMessage = $"Expense successfully added.\n\n" +
                $"Expense Date: {date.ToLongDateString()}\n" +
                $"Expense Amount: {amount}\n" +
                $"Expense Description: {desc}\n" +
                $"Expense Category: {cat}";
            MessageBox.Show(successMessage);
        }

        public void DisplayAddedCategory(string desc, string type)
        {
            string successMessage = $"Category successfully added.\n" +
                $"Category Description: {desc}\n" +
                $"Category Type: {type}";
            MessageBox.Show(successMessage);
        }

        public void DisplayError(string error)
        {
            MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        #endregion displays

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
                    RefreshCategories(GetCategoryList());
                    dp.SelectedDate = DateTime.Now;
                    DP_select.Visibility = Visibility.Visible;
                    tabcontrol.Visibility = Visibility.Visible;
                    HideMenu();
                    ShowExpenseTab();
                    name_TB.Text = Path.GetFileName(fileName);
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
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
                        presenter = new Presenter(this, fileName, true);
                        RefreshCategories(GetCategoryList());
                        dp.SelectedDate= DateTime.Now;
                        name_TB.Text = Path.GetFileName(fileName);

                        folderName = Path.GetDirectoryName(fileName);

                        WriteAppData();

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

                    WriteAppData();

                    RefreshCategories(GetCategoryList());
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

        #region categoryInputs

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
            string? description = DescInput.Text;
            string? type = "";
            foreach (RadioButton radio in radioBtns.Children)
            {
                if (radio.IsChecked == true)
                {
                    type = radio.Content.ToString();
                }
            }
            presenter.processAddCategory(description, type);
            RefreshCategories(GetCategoryList());
            changeOccured = false;
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
            string? type = "";
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

        private void catCB_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                string cat = catCB.Text;
                string type = "Expense";
                presenter.processAddCategory(cat, type);
                RefreshCategories(GetCategoryList());
            }
        }

        #endregion categoryInputs

        #region expenseInputs

        private void Exp_Add_Click(object sender, RoutedEventArgs e)
        {
            //warnings have to stay for rest of code to work
            DateTime date = (DateTime)dp.SelectedDate;
            string? category = catCB.SelectedItem.ToString();
            string? description = descriptionTB.Text;
            bool credit = (bool)exp_credit.IsChecked;

            double amount;

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
                }
            }
            else
            {
                DisplayError("Value entered for Amount is not a double");
            }
        }

        private void Exp_CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            catCB.SelectedIndex = -1;
            amountTB.Clear();
            descriptionTB.Clear();
            changeOccured = false;
        }

        private void catCB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string cat = catCB.Text;
            List<string> remainingCats = new List<string>();
            if (cat == "")
            {
                categories = GetCategoryList();
            }
            else
            {
                foreach (string category in GetCategoryList())
                {
                    if (cat.ToLower() == category.Substring(0, cat.Length < category.Length ? cat.Length : category.Length).ToLower())
                    {
                        remainingCats.Add(category);
                    }
                }
                categories = remainingCats;
            }

            RefreshCategories(categories);
            catCB.IsDropDownOpen = true;
        }

        #endregion expenseInputs

        #region categoryList

        public List<string> GetCategoryList()
        {
            List<string> cats = new List<string>();
            cats = presenter.GetCategoryDescriptionList();

            return cats;
        }

        private void RefreshCategories(List<string> categoriesList)
        {
            catCB.ItemsSource = categoriesList;
            catCB.Items.Refresh();
        }

        #endregion categoryList

        #region colors
        private void ColorChangeMenu(object sender, RoutedEventArgs e)
        {
            
            HideMenu();
            tabcontrol.Visibility = Visibility.Collapsed;
            HideAllElements();
            colorMenuBtn.Visibility = Visibility.Collapsed;
            colorMenuCloseBtn.Visibility = Visibility.Visible;
            buttonColor.Visibility = Visibility.Visible;
            BackgroundColorBtn.Visibility = Visibility.Visible;
            txtfeildBtn.Visibility = Visibility.Visible;
            boxColorBtn.Visibility =Visibility.Visible;
        }

        private void hideColorMenu()
        {
            colorMenuBtn.Visibility = Visibility.Visible;
            buttonColor.Visibility = Visibility.Collapsed;
            BackgroundColorBtn.Visibility = Visibility.Collapsed;
            txtfeildBtn.Visibility = Visibility.Collapsed;
            boxColorBtn.Visibility = Visibility.Collapsed;
            if (fileName == "")
            {
                ShowMenu();
            }
            else
            {
                ShowExpenseTab();
            }
        }

        private void colorMenuCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            hideColorMenu();
            HideAllElements();
            DP_select.Visibility= Visibility.Visible;
            tabcontrol.Visibility = Visibility.Visible;
            colorMenuCloseBtn.Visibility = Visibility.Collapsed;
            tabcontrol.SelectedIndex= 0;
            ShowExpenseTab();
        }

        private void buttonColor_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();

            addExpenseBtn.Background= brush;
            cancelExpenseBtn.Background = brush;
            add_Cat_btn.Background= brush;
            cat_cancel_btn.Background= brush;
            cat_preview_btn.Background= brush;
            cat_Preview_clear_btn.Background= brush;
            colorMenuBtn.Background = brush;
            colorMenuCloseBtn.Background = brush;
            buttonColor.Background = brush;
            BackgroundColorBtn.Background = brush;

        }

        private void BackgroundColorBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();
            WindowBox.Background = brush;
        }

        private void txtfieildBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();
            DescInput.Background= brush;
        }

        private void boxColorBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();
            file_Grid.Background= brush;
            ExpenseAddBox.Background= brush;
            catBorderAdd.Background= brush;
            catPreviewBorder.Background= brush;
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
        #endregion
    }
}