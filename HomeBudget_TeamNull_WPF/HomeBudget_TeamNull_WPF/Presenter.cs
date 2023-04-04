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
        List<Category> cats;

        public Presenter(ViewInterface v, string fileName)
        {
            view = v;
            model = new HomeBudget(fileName, true);
            cats = model.categories.List();
        }


        public void processAddExpense(DateTime date, string cat, double amount, string desc)
        {
            try
            {
                int catId = 0;
                foreach(Category category in cats)
                {
                    if(category.Description == cat)
                    {
                        catId = category.Id;
                    }
                }
                    model.expenses.Add(date, catId, amount, desc);
                    view.DisplayAddedExpense(date, catId, amount, desc);
            }
            catch (Exception e)
            {

                view.DisplayError(e.Message);
            }
        }
        public void processAddCategory(string desc, string type)
        {
            try
            {
                Category.CategoryType catType = (Category.CategoryType)Enum.Parse(typeof(Category.CategoryType), type);
                model.categories.Add(desc, catType);
                view.DisplayAddedCategory(desc, type.ToString());
            }
            catch (Exception e)
            {

                view.DisplayError(e.Message);
            }
        }
    }
}
