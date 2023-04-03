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

        public void processAddExpense(DateTime date, int catId, double amount, string desc)
        {
            try
            {
                    model.expenses.Add(date, catId, amount, desc);
                    view.DisplayAddedExpense(date, catId, amount, desc);
            }
            catch (Exception e)
            {

                view.DisplayError(e.Message);
            }
        }
        public void processAddCategory(string desc, Category.CategoryType type)
        {
            try
            {
                model.categories.Add(desc, type);
                view.DisplayAddedCategory(desc, type.ToString());
            }
            catch (Exception e)
            {

                view.DisplayError(e.Message);
            }
        }
    }
}
