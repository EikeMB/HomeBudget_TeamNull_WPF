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


        public void SetupDataGridDefault(List<BudgetItem> budgetItems, int index);

        public void SetupDataGridMonth(List<BudgetItemsByMonth> budgetItemsByMonth);

        public void SetupDataGridCategory(List<BudgetItemsByCategory> budgetItemsByCategory);

        public void SetupDataGridMonthCategory(List<Dictionary<string, object>> budgetItemsByMonthCategory);

        public void HighlightSearch(int index);
    }
}