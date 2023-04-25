﻿using Budget;
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
        /// <summary>
        /// Gets all budget items using the model and decides the filters based on arguments
        /// </summary>
        /// <param name="start">start date of the budgetItem search</param>
        /// <param name="end">end date of the budgetItem search</param>
        /// <param name="filter">if the search should filter categories</param>
        /// <param name="cat">category to filter with</param>
        /// <param name="methodOfGet">The type of getbudgetItems</param>
        public void processGetBudgetItems(DateTime? start, DateTime? end, bool filter, string cat, string? methodOfGet)
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


            if (methodOfGet == null)
            {
                List<BudgetItem> budgetItems = model.GetBudgetItems(start, end, filter, catId);
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Id");
                dataTable.Columns.Add("Date");
                dataTable.Columns.Add("Category");
                dataTable.Columns.Add("Description");
                dataTable.Columns.Add("Amount");
                dataTable.Columns.Add("Balance");

                foreach (BudgetItem budgetItem in budgetItems)
                {
                    dataTable.Rows.Add(budgetItem.ExpenseID,budgetItem.Date.ToLongDateString(), budgetItem.Category, budgetItem.ShortDescription, budgetItem.Amount, budgetItem.Balance);
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
        public void processDeleteExpense(int expense)
        {
            

            model.expenses.Delete(expense);
            expenses = model.expenses.List();

        }
    }
}