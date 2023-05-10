using Budget;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace HomeBudget_TeamNull_WPF
{
    public class Presenter
    {
        private readonly ViewInterface view;
        private readonly HomeBudget model;
        private List<Category> cats;
        private List<Expense> expenses;
        private int count;

        public Presenter(ViewInterface v, string fileName, bool newDB)
        {
            view = v;
            model = new HomeBudget(fileName, newDB);
            cats = model.categories.List();
            expenses = model.expenses.List();
        }

        /// <summary>
        /// Closes the database.
        /// </summary>
        public void Close()
        {
            model.CloseDB();
        }

        /// <summary>
        /// tries to add the expense to the database using the model.
        /// </summary>
        /// <param name="date">Date of the expense to add.</param>
        /// <param name="cat">category of the expense to add.</param>
        /// <param name="amount">amount of the expense to add.</param>
        /// <param name="desc">description of the expense to add.</param>
        public void processAddExpense(DateTime date, string? cat, double amount, string desc)
        {
            try
            {
                int catId = 0;
                foreach (Category category in cats)
                {
                    if (category.Description == cat)
                    {
                        catId = category.Id;
                    }
                }
                model.expenses.Add(date, catId, amount, desc);

                view.DisplayAddedExpense(date, cat, amount, desc);
            }
            catch (Exception e)
            {
                view.DisplayError(e.Message);
            }
        }

        /// <summary>
        /// tries to add the category to the database using the model.
        /// </summary>
        /// <param name="desc">description of the category</param>
        /// <param name="type">type of the category</param>
        public void processAddCategory(string desc, string type)
        {
            try
            {
                if (GetCategoryDescriptionList().Contains(desc))
                {
                    throw new Exception($"This Category description already exist: {desc}");
                }
                Category.CategoryType catType = (Category.CategoryType)Enum.Parse(typeof(Category.CategoryType), type);
                model.categories.Add(desc, catType);
                view.DisplayAddedCategory(desc, type.ToString());
            }
            catch (Exception e)
            {
                view.DisplayError(e.Message);
            }
        }

        public void processSearch(string search, System.Collections.IEnumerable items, int index)
        {
            
            if (index < 0) { index = 0; }
            List<BudgetItem> budgetItems = new List<BudgetItem>();
            int count = 0;
            foreach (BudgetItem item in items)
            {
                if (count >= index) { budgetItems.Add(item); }
                count++;
            }
            count = 0;
            foreach (BudgetItem item in items)
            {
                if (count < index) { budgetItems.Add(item); }
                count++;
            }
            count = 0;
            bool found = false;
            
            if(!string.IsNullOrEmpty(search))
            {
                foreach (BudgetItem item in budgetItems)
                {
                    if (item.Amount.ToString().ToLower().StartsWith(search) || item.ShortDescription.ToLower().StartsWith(search))
                    {
                        view.HighlightSearch((index + count) % budgetItems.Count);
                        found = true;
                        break;
                    }
                    count++;
                }
            }
            

            if (!found)
            {
                view.DisplayError("No matching expense found");
            }
        }

        /// <summary>
        /// Gets all budget items using the model and decides the filters based on arguments
        /// </summary>
        /// <param name="start">start date of the budgetItem search</param>
        /// <param name="end">end date of the budgetItem search</param>
        /// <param name="filter">if the search should filter categories</param>
        /// <param name="cat">category to filter with</param>
        /// <param name="methodOfGet">The type of getbudgetItems</param>
        public void processGetBudgetItems(DateTime? start, DateTime? end, bool filter, string cat, bool month, bool categoryCheck, int index)
        {
            int catId = 0;
            foreach (Category category in cats)
            {
                if (category.Description == cat)
                {
                    catId = category.Id;
                    break;
                }
            }

            if (!month && !categoryCheck)
            {
                List<BudgetItem> budgetItems = model.GetBudgetItems(start, end, filter, catId);
                view.SetupDataGridDefault(budgetItems, index);
            }
            else if (month && !categoryCheck)
            {
                List<BudgetItemsByMonth> budgetItemsByMonths = model.GetBudgetItemsByMonth(start, end, filter, catId);
                view.SetupDataGridMonth(budgetItemsByMonths);
            }
            else if (!month && categoryCheck)
            {
                List<BudgetItemsByCategory> budgetItemsByCategories = model.GetBudgetItemsByCategory(start, end, filter, catId);
                view.SetupDataGridCategory(budgetItemsByCategories);
            }
            else if (month && categoryCheck)
            {
                List<Dictionary<string, object>> budgetItemsByMonthAndCategory = model.GetBudgetDictionaryByCategoryAndMonth(start, end, filter, catId);
                view.SetupDataGridMonthCategory(budgetItemsByMonthAndCategory);
            }
        }

        /// <summary>
        /// Returns a string list of all category names
        /// </summary>
        /// <returns>Returns a string list of all category names</returns>
        public List<string> GetCategoryDescriptionList()
        {
            cats = model.categories.List();
            List<string> descriptions = new List<string>();

            foreach (Category cat in cats)
            {
                descriptions.Add(cat.Description.ToString());
            }

            return descriptions;
        }

        /// <summary>
        /// updates expense through the model
        /// </summary>
        /// <param name="expense">expense id to update</param>
        /// <param name="date">new date</param>
        /// <param name="cat">new category</param>
        /// <param name="amount">new amount</param>
        /// <param name="desc">new description</param>
        public void processUpdateExpense(int expense, DateTime date, string? cat, double amount, string desc)
        {
            try
            {
                int catId = 0;
                foreach (Category category in cats)
                {
                    if (category.Description == cat)
                    {
                        catId = category.Id;
                        break;
                    }
                }

                model.expenses.UpdateProperties(expense, date, catId, amount, desc);
                expenses = model.expenses.List();
            }
            catch (Exception e)
            {
                view.DisplayError(e.Message);
            }
        }

        /// <summary>
        /// Deletes expense through the model
        /// </summary>
        /// <param name="expense">expense id of the expense to delete</param>
        public void processDeleteExpense(BudgetItem expense)
        {
            model.expenses.Delete(expense.ExpenseID);


            expenses = model.expenses.List();
        }
    }
}