﻿using Budget;
using System;
using System.Collections.Generic;

namespace HomeBudget_TeamNull_WPF
{
    public class Presenter
    {
        private readonly ViewInterface view;
        private readonly HomeBudget model;
        private List<Category> cats;

        public Presenter(ViewInterface v, string fileName, bool newDB)
        {
            view = v;
            model = new HomeBudget(fileName, newDB);
            cats = model.categories.List();
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
                view.DisplayExpenses(budgetItems);
            }
            else if(methodOfGet == "month")
            {
                List<BudgetItemsByMonth> budgetItemsByMonths = model.GetBudgetItemsByMonth(start, end, filter, catId);
                view.DisplayExpensesByMonth(budgetItemsByMonths);
            }
            else if(methodOfGet == "category")
            {
                List<BudgetItemsByCategory> budgetItemsByCategories = model.GetBudgetItemsByCategory(start, end, filter, catId);
                view.DisplayExpensesByCategory(budgetItemsByCategories);
            }
            else if(methodOfGet == "month/category")
            {
                List<Dictionary<string, object>> budgetItemsByMonthAndCategory = model.GetBudgetDictionaryByCategoryAndMonth(start, end, filter, catId);
                view.DisplayExpensesByMonthAndCat(budgetItemsByMonthAndCategory);
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
    }
}