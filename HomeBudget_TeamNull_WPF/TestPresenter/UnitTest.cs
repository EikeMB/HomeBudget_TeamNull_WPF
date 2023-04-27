using HomeBudget_TeamNull_WPF;
using System;
using System.Data;
using System.DirectoryServices;

namespace TestPresenter
{
    [Collection("Sequential")]
    public class UnitTest : ViewInterface
    {

        private bool calledDisplayAddedCategory;
        private bool calledDisplayAddedExpense;
        private bool calledDisplayError;
        private bool calledDisplayExpense;
        

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



        [Fact]
        public void TestConstructor()
        {
            if(File.Exists(Environment.ProcessPath + "testDB1.db"))
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
            view.calledDisplayExpense = false;

            p.processGetBudgetItems(null, null, false, "", null);
            Assert.False(view.calledDisplayAddedCategory);
            Assert.False(view.calledDisplayAddedExpense);
            Assert.False(view.calledDisplayError);
            Assert.True(view.calledDisplayExpense);
            p.Close();

        }

        public List<string> GetCategoryList()
        {
            throw new NotImplementedException();
        }

        
    }
}