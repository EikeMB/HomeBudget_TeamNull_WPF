using Budget;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace HomeBudget_TeamNull_WPF
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window, ViewInterface
    {
        private Presenter p;
        private BudgetItem selectedExpense;
        private int index;
        private int count;

        public UpdateWindow(Presenter presenter, BudgetItem selectedExpense, int index, int count)
        {
            p = presenter;
            this.selectedExpense = selectedExpense;
            this.index = index;
            InitializeComponent();
            Update_DP.SelectedDate = DateTime.Now;
            RefreshCategories(GetCategoryList());
            this.count = count;
        }

        private void RefreshCategories(List<string> categoriesList)
        {
            update_CB.ItemsSource = categoriesList;
            update_CB.Items.Refresh();
        }

        private void Amount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        #region Buttons

        private void UpdateBTN_Click(object sender, RoutedEventArgs e)
        {
            int exp = selectedExpense.ExpenseID;
            DateTime date = (DateTime)Update_DP.SelectedDate;
            string? category = update_CB.SelectedItem.ToString();
            string? description = Desc_TB.Text;
            double amount = double.Parse(Amount_TB.Text);

            p.processUpdateExpense(exp, date, category, amount, description);
            this.Close();
        }

        private void CancelBTN_Click(object sender, RoutedEventArgs e)
        {
            update_CB.SelectedIndex = -1;
            Amount_TB.Clear();
            Desc_TB.Clear();
            Update_DP.SelectedDate = DateTime.Today;
        }

        private void DeleteBTN_Click(object sender, RoutedEventArgs e)
        {
            p.processDeleteExpense(selectedExpense);
            this.Close();
        }

        #endregion Buttons

        #region InterfaceMethods

        public void DisplayAddedCategory(string desc, string type)
        {
            throw new NotImplementedException();
        }

        public void DisplayAddedExpense(DateTime date, string catId, double amount, string desc)
        {
            throw new NotImplementedException();
        }

        public void DisplayError(string error)
        {
            throw new NotImplementedException();
        }

        public void DisplayExpenses(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        public void DisplayExpensesByMonth(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        public void DisplayExpensesByCategory(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        public void DisplayExpensesByMonthAndCat(DataTable dataTable)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a list of string with all the category names
        /// </summary>
        /// <returns>String list of category names</returns>
        public List<string> GetCategoryList()
        {
            List<string> cats = new List<string>();
            cats = p.GetCategoryDescriptionList();

            return cats;
        }

        public void SetupDataGridDefault(List<BudgetItem> budgetItems)
        {
            throw new NotImplementedException();
        }

        public void SetupDataGridMonth(List<BudgetItemsByMonth> budgetItemsByMonth)
        {
            throw new NotImplementedException();
        }

        public void SetupDataGridCategory(List<BudgetItemsByCategory> budgetItemsByCategory)
        {
            throw new NotImplementedException();
        }

        public void SetupDataGridMonthCategory(List<Dictionary<string, object>> budgetItemsByMonthCategory)
        {
            throw new NotImplementedException();
        }

        public void HighlightSearch(int index)
        {
            throw new NotImplementedException();
        }

        public void HighlightRow(int index)
        {
            throw new NotImplementedException();
        }

        public void SetupDataGridDefault(List<BudgetItem> budgetItems, int index)
        {
            throw new NotImplementedException();
        }

        #endregion InterfaceMethods
    }
}