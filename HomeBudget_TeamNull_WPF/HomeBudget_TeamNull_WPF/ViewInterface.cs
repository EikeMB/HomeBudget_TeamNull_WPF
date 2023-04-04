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

        public void DisplayAddedExpense(DateTime date, int catId, double amount, string desc);
        public void DisplayAddedCategory(string desc, string type);

        public void DisplayError(string error);

       
    }
}
