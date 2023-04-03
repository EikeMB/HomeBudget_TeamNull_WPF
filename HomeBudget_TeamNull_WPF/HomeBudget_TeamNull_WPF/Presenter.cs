using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Budget;

namespace HomeBudget_TeamNull_WPF
{
    class Presenter
    {
        private readonly ViewInterface view;
        private readonly HomeBudget model;


        public Presenter(ViewInterface v, string fileName)
        {
            view = v;
            model = new HomeBudget(fileName, true);
        }

        public void processAdd(Object added)
        {
            try
            {
                if (added is Expense expense)
                {
                    model.expenses.Add(expense.Date, expense.Category, expense.Amount, expense.Description);
                    view.DisplayAddedExpense(expense);
                }
                else if (added is Category category)
                {
                    model.categories.Add(category.Description, category.Type);
                    view.DisplayAddedCategory(category);
                }
                else
                {
                    view.DisplayError("Added must be expense or Category.");
                }
            }
            catch (Exception e)
            {

                view.DisplayError(e.Message);
            }
        }
    }
}
