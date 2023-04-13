using HomeBudget_TeamNull_WPF;

namespace TestPresenter
{
    public class UnitTest : ViewInterface
    {

        private bool calledDisplayAddedCategory;

        public void DisplayAddedCategory(string desc, string type)
        {
            calledDisplayAddedCategory = true;
        }

        public void DisplayAddedExpense(DateTime date, int catId, double amount, string desc)
        {
            throw new NotImplementedException();
        }

        public void DisplayError(string error)
        {
            throw new NotImplementedException();
        }

        public List<string> GetCategoryList()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public void TestConstructor()
        {
            TestView view = new TestView();
            Presenter p = new Presenter(view);
            Assert.IsType<Presenter>(p);
        }
    }
}