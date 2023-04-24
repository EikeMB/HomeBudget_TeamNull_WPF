using Budget;
using System;
using System.Collections.Generic;
using System.Data;

namespace HomeBudget_TeamNull_WPF
{
    public interface ViewInterface
    {
        public void DisplayAddedExpense(DateTime date, string catId, double amount, string desc);

        public void DisplayAddedCategory(string desc, string type);

        public void DisplayError(string error);

        public List<String> GetCategoryList();

        public void DisplayExpenses(DataTable dataTable);
        public void DisplayExpensesByMonth(DataTable dataTable);
        public void DisplayExpensesByCategory(DataTable dataTable);
        public void DisplayExpensesByMonthAndCat(DataTable dataTable);

    }
}