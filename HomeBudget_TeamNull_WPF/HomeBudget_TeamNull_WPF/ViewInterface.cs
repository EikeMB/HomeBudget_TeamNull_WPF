using System;
using System.Collections.Generic;

namespace HomeBudget_TeamNull_WPF
{
    internal interface ViewInterface
    {
        public void DisplayAddedExpense(DateTime date, int catId, double amount, string desc);

        public void DisplayAddedCategory(string desc, string type);

        public void DisplayError(string error);

        public List<String> GetCategoryList();
    }
}