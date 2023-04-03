using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace HomeBudget_TeamNull_WPF
{
    interface ViewInterface
    {

        public void DisplayAddedExpense(Expense expense);
        public void DisplayAddedCategory(Category category);

        public void DisplayError(string error);

       
    }
}
