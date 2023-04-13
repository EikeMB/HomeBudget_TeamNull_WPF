using HomeBudget_TeamNull_WPF;
using System.DirectoryServices;

namespace TestPresenter
{
    public class UnitTest : ViewInterface
    {

        private bool calledDisplayAddedCategory;
        private bool calledDisplayAddedExpense;
        private bool calledDisplayError;
        private bool calledGetCategoryList;
        

        public void DisplayAddedCategory(string desc, string type)
        {
            calledDisplayAddedCategory = true;
        }

        public void DisplayAddedExpense(DateTime date, int catId, double amount, string desc)
        {
            calledDisplayAddedExpense = true;
        }

        public void DisplayError(string error)
        {
            calledDisplayError = true;
        }

        public List<string> GetCategoryList()
        {
            calledGetCategoryList = true;
            return new List<string>();
        }


        [Fact]
        public void TestConstructor()
        {
            File.WriteAllText(Environment.ProcessPath + "testDB1.db", "");

            UnitTest view = new UnitTest();
            Presenter p = new Presenter(view, "testDB1.db");
            Assert.IsType<Presenter>(p);
        }

        [Fact]
        public void TestCallDisplayAddedCategory()
        {
            DisplayAddedCategory("","");
            Assert.True(calledDisplayAddedCategory);
        }

        [Fact]
        public void TestCallDisplayAddedExpense()
        {
            DisplayAddedExpense(DateTime.Now,1,1,"");
            Assert.True(calledDisplayAddedExpense);
        }

        [Fact]
        public void TestCallDisplayError()
        {
            DisplayError("");
            Assert.True(calledDisplayError);
        }

        [Fact]
        public void TestCallGetCategoryList()
        {
            GetCategoryList();
            Assert.True(calledGetCategoryList);
        }

    }
}