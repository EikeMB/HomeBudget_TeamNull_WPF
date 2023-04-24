using Budget;
using System;
using System.Collections.Generic;

namespace HomeBudget_TeamNull_WPF
{
    public interface ViewInterface
    {
        public void DisplayAddedExpense(DateTime date, string catId, double amount, string desc);

        public void DisplayAddedCategory(string desc, string type);

        public void DisplayError(string error);

        public List<String> GetCategoryList();

        public void DisplayExpenses(List<BudgetItem> budgetItems);
        public void DisplayExpensesByMonth(List<BudgetItemsByMonth> budgetItemsByMonths);
        public void DisplayExpensesByCategory(List<BudgetItemsByCategory> budgetItemsByCategories);
        public void DisplayExpensesByMonthAndCat(List<Dictionary<string, object>> budgetItemsByMonthAndCat);

    }
}