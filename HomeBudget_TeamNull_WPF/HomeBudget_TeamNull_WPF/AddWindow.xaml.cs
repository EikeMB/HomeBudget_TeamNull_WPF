using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window,ViewInterface
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

        public AddWindow(Presenter Mainpresenter)
        {
            presenter = Mainpresenter;
            InitializeComponent();
            RefreshCategories(GetCategoryList());
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


        #region elementViews

        private void HideAllElements()
        {
           
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

            


        }

        private void hideColorMenu()
        {

            if (fileName == "")
            {
                
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
            Window.Background = brush;
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


        #endregion

        private void Amount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }
    }
}
