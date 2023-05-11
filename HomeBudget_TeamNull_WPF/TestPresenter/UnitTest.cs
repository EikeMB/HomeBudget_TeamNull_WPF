using Budget;
using HomeBudget_TeamNull_WPF;
using System.Data;

namespace TestPresenter
{
    
    public class UnitTest : ViewInterface
    {
        public bool calledDisplayAddedCategory;
        public bool calledDisplayAddedExpense;
        public bool calledDisplayError;
        public bool calledDisplayExpense;
        public bool calledSetupDataGridDefault;
        public bool calledSetupDataGridMonth;
        public bool calledSetupDataGridCat;
        public bool calledSetupDataGridMonthCat;
        public bool calledHighlightSearch;
        public int highlightedIndex;
        public List<BudgetItem> items;


        public void DisplayAddedCategory(string desc, string type)
        {
            calledDisplayAddedCategory = true;
        }

        public void DisplayAddedExpense(DateTime date, string catId, double amount, string desc)
        {
            calledDisplayAddedExpense = true;
        }

        public void DisplayError(string error)
        {
            calledDisplayError = true;
        }

        public void DisplayExpenses(DataTable dataTable)
        {
            calledDisplayExpense = true;
        }

        public void SetupDataGridDefault(List<BudgetItem> budgetItems, int index)
        {
            calledSetupDataGridDefault = true;
            items = budgetItems;
        }

        public void SetupDataGridMonth(List<BudgetItemsByMonth> budgetItemsByMonth)
        {
            calledSetupDataGridMonth = true;
        }

        public void SetupDataGridCategory(List<BudgetItemsByCategory> budgetItemsByCategory)
        {
            calledSetupDataGridCat = true;
        }

        public void SetupDataGridMonthCategory(List<Dictionary<string, object>> budgetItemsByMonthCategory)
        {
            calledSetupDataGridMonthCat = true;
        }

        public void HighlightSearch(int index)
        {
            calledHighlightSearch = true;
            highlightedIndex = index;
        }
    }

    [Collection("Sequential")]
    public class PresenterTest
    {

    

        [Fact]
        public void TestConstructor()
        {
            if (File.Exists(Environment.ProcessPath + "testDB1.db"))
            {
                File.Delete(Environment.ProcessPath + "testDB1.db");
            }
            File.WriteAllText(Environment.ProcessPath + "testDB1.db", "");

            UnitTest view = new UnitTest();
            Presenter p = new Presenter(view, "testDB1.db", true);
            Assert.IsType<Presenter>(p);
            p.Close();
        }

        [Fact]
        public void TestCallDisplayAddedCategory()
        {
            if (File.Exists(Environment.ProcessPath + "testDB1.db"))
            {
                File.Delete(Environment.ProcessPath + "testDB1.db");
            }
            File.WriteAllText(Environment.ProcessPath + "testDB1.db", "");
            UnitTest view = new UnitTest();
            Presenter p = new Presenter(view, "testDB1.db", true);

            view.calledDisplayAddedCategory = false;
            view.calledDisplayAddedExpense = false;
            view.calledDisplayError = false;

            p.processAddCategory("test", "Expense");
            Assert.True(view.calledDisplayAddedCategory);
            Assert.False(view.calledDisplayAddedExpense);
            Assert.False(view.calledDisplayError);
            p.Close();
        }

        [Fact]
        public void TestCallDisplayAddedExpense()
        {
            if (File.Exists(Environment.ProcessPath + "testDB1.db"))
            {
                File.Delete(Environment.ProcessPath + "testDB1.db");
            }
            File.WriteAllText(Environment.ProcessPath + "testDB1.db", "");
            UnitTest view = new UnitTest();
            Presenter p = new Presenter(view, "testDB1.db", true);

            view.calledDisplayAddedCategory = false;
            view.calledDisplayAddedExpense = false;
            view.calledDisplayError = false;

            p.processAddExpense(DateTime.Now, "Utilities", 5, "test");
            Assert.True(view.calledDisplayAddedExpense);
            Assert.False(view.calledDisplayAddedCategory);
            Assert.False(view.calledDisplayError);
            p.Close();
        }

        [Fact]
        public void TestCallDisplayError()
        {
            if (File.Exists(Environment.ProcessPath + "testDB1.db"))
            {
                File.Delete(Environment.ProcessPath + "testDB1.db");
            }
            File.WriteAllText(Environment.ProcessPath + "testDB1.db", "");
            UnitTest view = new UnitTest();
            Presenter p = new Presenter(view, "testDB1.db", true);

            view.calledDisplayAddedCategory = false;
            view.calledDisplayAddedExpense = false;
            view.calledDisplayError = false;

            p.processAddExpense(DateTime.Now, "hello", 5, "test");
            Assert.True(view.calledDisplayError);
            Assert.False(view.calledDisplayAddedCategory);
            Assert.False(view.calledDisplayAddedExpense);
            p.Close();
        }

        [Fact]
        public void TestCallDisplayErrorV2()
        {
            if (File.Exists(Environment.ProcessPath + "testDB1.db"))
            {
                File.Delete(Environment.ProcessPath + "testDB1.db");
            }
            File.WriteAllText(Environment.ProcessPath + "testDB1.db", "");
            UnitTest view = new UnitTest();
            Presenter p = new Presenter(view, "testDB1.db", true);

            view.calledDisplayAddedCategory = false;
            view.calledDisplayAddedExpense = false;
            view.calledDisplayError = false;

            p.processAddCategory("test", "");
            Assert.True(view.calledDisplayError);
            Assert.False(view.calledDisplayAddedCategory);
            Assert.False(view.calledDisplayAddedExpense);
            p.Close();
        }

        [Fact]
        public void TestCallDisplayExpense()
        {
            if (File.Exists(Environment.ProcessPath + "testDB1.db"))
            {
                File.Delete(Environment.ProcessPath + "testDB1.db");
            }
            File.WriteAllText(Environment.ProcessPath + "testDB1.db", "");
            UnitTest view = new UnitTest();
            Presenter p = new Presenter(view, "testDB1.db", true);

            view.calledDisplayAddedCategory = false;
            view.calledDisplayAddedExpense = false;
            view.calledDisplayError = false;
            view.calledSetupDataGridDefault = false;

            p.processGetBudgetItems(null, null, false, "", false, false, 0);
            Assert.False(view.calledDisplayAddedCategory);
            Assert.False(view.calledDisplayAddedExpense);
            Assert.False(view.calledDisplayError);
            Assert.True(view.calledSetupDataGridDefault);
            p.Close();
        }

        [Fact]
        public void TestGetBudgetItemsByMonth()
        {
            if (File.Exists(Environment.ProcessPath + "testDB1.db"))
            {
                File.Delete(Environment.ProcessPath + "testDB1.db");
            }
            File.WriteAllText(Environment.ProcessPath + "testDB1.db", "");
            UnitTest view = new UnitTest();
            Presenter p = new Presenter(view, "testDB1.db", true);

            view.calledDisplayAddedCategory = false;
            view.calledDisplayAddedExpense = false;
            view.calledDisplayError = false;
            view.calledSetupDataGridDefault = false;
            view.calledSetupDataGridMonth = false;

            p.processGetBudgetItems(null, null, false, "", true, false, 0);
            Assert.False(view.calledDisplayAddedCategory);
            Assert.False(view.calledDisplayAddedExpense);
            Assert.False(view.calledDisplayError);
            Assert.False(view.calledSetupDataGridDefault);
            Assert.True(view.calledSetupDataGridMonth);
            p.Close();
        }
        [Fact]
        public void TestGetBudgetItemsByCategory()
        {
            if (File.Exists(Environment.ProcessPath + "testDB1.db"))
            {
                File.Delete(Environment.ProcessPath + "testDB1.db");
            }
            File.WriteAllText(Environment.ProcessPath + "testDB1.db", "");
            UnitTest view = new UnitTest();
            Presenter p = new Presenter(view, "testDB1.db", true);

            view.calledDisplayAddedCategory = false;
            view.calledDisplayAddedExpense = false;
            view.calledDisplayError = false;
            view.calledSetupDataGridDefault = false;
            view.calledSetupDataGridCat = false;

            p.processGetBudgetItems(null, null, false, "", false, true, 0);
            Assert.False(view.calledDisplayAddedCategory);
            Assert.False(view.calledDisplayAddedExpense);
            Assert.False(view.calledDisplayError);
            Assert.False(view.calledSetupDataGridDefault);
            Assert.True(view.calledSetupDataGridCat);
            p.Close();
        }
        [Fact]
        public void TestGetBudgetItemsByMonthCategory()
        {
            if (File.Exists(Environment.ProcessPath + "testDB1.db"))
            {
                File.Delete(Environment.ProcessPath + "testDB1.db");
            }
            File.WriteAllText(Environment.ProcessPath + "testDB1.db", "");
            UnitTest view = new UnitTest();
            Presenter p = new Presenter(view, "testDB1.db", true);

            view.calledDisplayAddedCategory = false;
            view.calledDisplayAddedExpense = false;
            view.calledDisplayError = false;
            view.calledSetupDataGridDefault = false;
            view.calledSetupDataGridMonthCat = false;

            p.processGetBudgetItems(null, null, false, "", true, true, 0);
            Assert.False(view.calledDisplayAddedCategory);
            Assert.False(view.calledDisplayAddedExpense);
            Assert.False(view.calledDisplayError);
            Assert.False(view.calledSetupDataGridDefault);
            Assert.True(view.calledSetupDataGridMonthCat);
            p.Close();
        }

        [Fact]
        public void TestSearch()
        {
            if (File.Exists(Environment.ProcessPath + "testDB1.db"))
            {
                File.Delete(Environment.ProcessPath + "testDB1.db");
            }
            File.WriteAllText(Environment.ProcessPath + "testDB1.db", "");
            UnitTest view = new UnitTest();
            Presenter p = new Presenter(view, "testDB1.db", true);

            view.calledDisplayAddedCategory = false;
            view.calledDisplayAddedExpense = false;
            view.calledDisplayError = false;
            view.calledSetupDataGridDefault = false;
            view.calledSetupDataGridCat = false;
            view.calledHighlightSearch = false;
            view.items = null;

            p.processAddExpense(DateTime.Now, "Credit Card", 400, "testing");
            p.processAddExpense(DateTime.Now, "Credit Card", 500, "testing2");
            p.processAddExpense(DateTime.Now, "Credit Card", 300, "hellotest");
            p.processGetBudgetItems(null, null, false, "", false, false, 0);
            p.processSearch("he", view.items, 0);
            Assert.False(view.calledDisplayAddedCategory);
            Assert.True(view.calledDisplayAddedExpense);
            Assert.False(view.calledDisplayError);
            Assert.True(view.calledSetupDataGridDefault);
            Assert.False(view.calledSetupDataGridCat);
            Assert.True(view.calledHighlightSearch);
            Assert.True(view.highlightedIndex.Equals(2));
            p.Close();
        }
        [Fact]
        public void TestSearchSecondItem()
        {
            if (File.Exists(Environment.ProcessPath + "testDB1.db"))
            {
                File.Delete(Environment.ProcessPath + "testDB1.db");
            }
            File.WriteAllText(Environment.ProcessPath + "testDB1.db", "");
            UnitTest view = new UnitTest();
            Presenter p = new Presenter(view, "testDB1.db", true);

            view.calledDisplayAddedCategory = false;
            view.calledDisplayAddedExpense = false;
            view.calledDisplayError = false;
            view.calledSetupDataGridDefault = false;
            view.calledSetupDataGridCat = false;
            view.calledHighlightSearch = false;
            view.items = null;

            p.processAddExpense(DateTime.Now, "Credit Card", 400, "testing");
            p.processAddExpense(DateTime.Now, "Credit Card", 500, "testing2");
            p.processAddExpense(DateTime.Now, "Credit Card", 300, "hellotest");
            p.processGetBudgetItems(null, null, false, "", false, false, 0);
            p.processSearch("testing", view.items, 1);
            Assert.False(view.calledDisplayAddedCategory);
            Assert.True(view.calledDisplayAddedExpense);
            Assert.False(view.calledDisplayError);
            Assert.True(view.calledSetupDataGridDefault);
            Assert.False(view.calledSetupDataGridCat);
            Assert.True(view.calledHighlightSearch);
            Assert.True(view.highlightedIndex.Equals(1));
            p.Close();
        }
        [Fact]
        public void TestSearchTwoDifferentExpenses()
        {
            if (File.Exists(Environment.ProcessPath + "testDB1.db"))
            {
                File.Delete(Environment.ProcessPath + "testDB1.db");
            }
            File.WriteAllText(Environment.ProcessPath + "testDB1.db", "");
            UnitTest view = new UnitTest();
            Presenter p = new Presenter(view, "testDB1.db", true);

            view.calledDisplayAddedCategory = false;
            view.calledDisplayAddedExpense = false;
            view.calledDisplayError = false;
            view.calledSetupDataGridDefault = false;
            view.calledSetupDataGridCat = false;
            view.calledHighlightSearch = false;
            view.items = null;

            p.processAddExpense(DateTime.Now, "Credit Card", 400, "testing");
            p.processAddExpense(DateTime.Now, "Credit Card", 500, "testing2");
            p.processAddExpense(DateTime.Now, "Credit Card", 300, "hellotest");
            p.processGetBudgetItems(null, null, false, "", false, false, 0);
            p.processSearch("testing", view.items, 1);
            Assert.False(view.calledDisplayAddedCategory);
            Assert.True(view.calledDisplayAddedExpense);
            Assert.False(view.calledDisplayError);
            Assert.True(view.calledSetupDataGridDefault);
            Assert.False(view.calledSetupDataGridCat);
            Assert.True(view.calledHighlightSearch);
            Assert.True(view.highlightedIndex.Equals(1));
            p.Close();
            p.processSearch("testing", view.items, 2);
            Assert.True(view.highlightedIndex.Equals(0));
        }


    }
}