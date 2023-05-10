using Budget;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HomeBudget_TeamNull_WPF
{
    /// <summary>
    /// Interaction logic for MonthFilter.xaml
    /// </summary>
    public partial class MonthFilter : Window, ViewInterface
    {
        private Presenter p;
        private BudgetItemsByMonth selectedExpense;

        public MonthFilter(Presenter presenter, BudgetItemsByMonth selectedExpense)
        {
            p = presenter;
            this.selectedExpense = selectedExpense;

            InitializeComponent();
            SetupDataGridDefault(selectedExpense.Details);
        }

        public void SetupDataGridDefault(List<BudgetItem> budgetItems)
        {
            datagrid.ItemsSource = budgetItems;
            datagrid.Columns.Clear();

            var column1 = new DataGridTextColumn();
            column1.Header = "Date";
            column1.Binding = new Binding("Date");
            column1.Binding.StringFormat = "{0:MM/dd/yyyy}";
            datagrid.Columns.Add(column1);

            var column2 = new DataGridTextColumn();
            column2.Header = "Category";
            column2.Binding = new Binding("Category");

            datagrid.Columns.Add(column2);

            var column3 = new DataGridTextColumn();
            column3.Header = "Description";
            column3.Binding = new Binding("ShortDescription");

            datagrid.Columns.Add(column3);

            var column4 = new DataGridTextColumn();
            column4.Header = "Amount";
            column4.Binding = new Binding("Amount");

            column4.Binding.StringFormat = "F2";
            Style s = new Style();
            s.Setters.Add(new Setter(TextBlock.TextAlignmentProperty,
                                    TextAlignment.Right));
            column4.CellStyle = s;

            datagrid.Columns.Add(column4);

            var column5 = new DataGridTextColumn();
            column5.Header = "Balance";
            column5.Binding = new Binding("Balance");

            column5.Binding.StringFormat = "F2";
            Style s2 = new Style();
            s2.Setters.Add(new Setter(TextBlock.TextAlignmentProperty,
                                    TextAlignment.Right));
            column5.CellStyle = s;

            datagrid.Columns.Add(column5);
        }

        #region interface

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

        public List<string> GetCategoryList()
        {
            throw new NotImplementedException();
        }

        public void HighlightSearch(int index)
        {
            throw new NotImplementedException();
        }

        public void SetupDataGridCategory(List<BudgetItemsByCategory> budgetItemsByCategory)
        {
            throw new NotImplementedException();
        }

        public void SetupDataGridMonth(List<BudgetItemsByMonth> budgetItemsByMonth)
        {
            throw new NotImplementedException();
        }

        public void SetupDataGridMonthCategory(List<Dictionary<string, object>> budgetItemsByMonthCategory)
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

        #endregion interface
    }
}