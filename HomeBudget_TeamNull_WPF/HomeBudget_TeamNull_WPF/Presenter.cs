using Budget;
using System;
using System.Collections.Generic;
using System.Data;

namespace HomeBudget_TeamNull_WPF
{
    public class Presenter
    {
        private readonly ViewInterface view;
        private readonly HomeBudget model;
        private List<Category> cats;
        private List<Expense> expenses;

        public Presenter(ViewInterface v, string fileName, bool newDB)
        {
            view = v;
            model = new HomeBudget(fileName, newDB);
            cats = model.categories.List();
            expenses = model.expenses.List();
        }

        public void Close()
        {
            model.CloseDB();
        }

        
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

        public void processGetBudgetItems(DateTime? start, DateTime? end, bool filter, string cat, string? methodOfGet)
        {
            int catId = 0;
            foreach (Category category in cats)
            {
                if (category.Description == cat)
                {
                    catId = category.Id;
                }
            }


            if (methodOfGet == null)
            {
                List<BudgetItem> budgetItems = model.GetBudgetItems(start, end, filter, catId);
                DataTable dataTable = new DataTable();

                dataTable.Columns.Add("Date");
                dataTable.Columns.Add("Category");
                dataTable.Columns.Add("Description");
                dataTable.Columns.Add("Amount");
                dataTable.Columns.Add("Balance");

                foreach (BudgetItem budgetItem in budgetItems)
                {
                    dataTable.Rows.Add(budgetItem.Date.ToLongDateString(), budgetItem.Category, budgetItem.ShortDescription, budgetItem.Amount, budgetItem.Balance);
                }
                view.DisplayExpenses(dataTable);
            }
            else if (methodOfGet == "month")
            {
                List<BudgetItemsByMonth> budgetItemsByMonths = model.GetBudgetItemsByMonth(start, end, filter, catId);
                DataTable dataTable = new DataTable();

                dataTable.Columns.Add("Month");
                dataTable.Columns.Add("Total");

                foreach (BudgetItemsByMonth budgetItemsByMonth in budgetItemsByMonths)
                {
                    dataTable.Rows.Add(budgetItemsByMonth.Month, budgetItemsByMonth.Total);
                }
                view.DisplayExpensesByMonth(dataTable);
            }
            else if (methodOfGet == "category")
            {
                List<BudgetItemsByCategory> budgetItemsByCategories = model.GetBudgetItemsByCategory(start, end, filter, catId);
                DataTable dataTable = new DataTable();

                dataTable.Columns.Add("Categories");
                dataTable.Columns.Add("Total");

                foreach (BudgetItemsByCategory budgetItemsByCategory in budgetItemsByCategories)
                {
                    dataTable.Rows.Add(budgetItemsByCategory.Category, budgetItemsByCategory.Total);
                }
                view.DisplayExpensesByCategory(dataTable);
            }
            else if (methodOfGet == "month/category")
            {
                List<Dictionary<string, object>> budgetItemsByMonthAndCategory = model.GetBudgetDictionaryByCategoryAndMonth(start, end, filter, catId);
                DataTable dataTable = new DataTable();
                Dictionary<string, object> keyValues = budgetItemsByMonthAndCategory[budgetItemsByMonthAndCategory.Count - 1];
                foreach (var keyValuePair in keyValues)
                {
                    dataTable.Columns.Add(keyValuePair.Key);
                }
                dataTable.Columns.Add("Total");
                List<BudgetItem> budgetItems = new List<BudgetItem>();
                for (int i = 0; i < budgetItemsByMonthAndCategory.Count - 1; i++)
                {

                    Dictionary<string, object> keyValuesMonth = budgetItemsByMonthAndCategory[i];
                    double[] catTotals = new double[dataTable.Columns.Count - 2];
                    for (int j = 0; j < catTotals.Length; j++)
                    {
                        object tempcatTotal;
                        keyValuesMonth.TryGetValue(dataTable.Columns[j + 1].ToString(), out tempcatTotal);
                        if (tempcatTotal != null)
                        {
                            catTotals[j] = (double)tempcatTotal;
                        }
                    }
                    string month = "";
                    string total = "";
                    foreach (var keyValuePair in keyValuesMonth)
                    {



                        if (keyValuePair.Key == "Month")
                        {
                            month = keyValuePair.Value.ToString();
                        }
                        else if (keyValuePair.Key == "Total")
                        {
                            total = keyValuePair.Value.ToString();
                        }





                    }
                    DataRow row = dataTable.NewRow();

                    for (int k = 0; k < dataTable.Columns.Count; k++)
                    {
                        if (k == 0)
                        {
                            row[k] = month;
                        }
                        else if (k == dataTable.Columns.Count - 1)
                        {
                            row[k] = total;
                        }
                        else
                        {
                            row[k] = catTotals[k - 1];
                        }
                    }

                    dataTable.Rows.Add(row);
                    view.DisplayExpensesByMonthAndCat(dataTable);
                }
            }
        }

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

        public void processUpdateExpense(string expense, DateTime date, string? cat, double amount, string desc)
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

                int expenseId = 0;
                foreach(Expense e in expenses)
                {
                    if (e.Description == expense)
                    {
                        expenseId = e.Id;
                    }
                }

                model.expenses.UpdateProperties(expenseId, date, catId, amount, desc);

            }
            catch (Exception e)
            {
                view.DisplayError(e.Message);
            }
        }

        public void processDeleteExpense(string expense)
        {
            int expenseId = 0;
            foreach (Expense e in expenses)
            {
                if (e.Description == expense)
                {
                    expenseId = e.Id;
                }
            }

            model.expenses.Delete(expenseId);
        }
    }
}