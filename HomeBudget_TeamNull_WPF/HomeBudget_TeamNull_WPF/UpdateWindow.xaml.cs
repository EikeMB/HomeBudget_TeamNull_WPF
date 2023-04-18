using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HomeBudget_TeamNull_WPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window, ViewInterface
    {
        private Presenter presenter;


        public Window1()
        {
            InitializeComponent();
            Update_DP.SelectedDate = DateTime.Now;
            RefreshCategories(GetCategoryList());
        }

        public void DisplayAddedCategory(string desc, string type)
        {
            throw new NotImplementedException();
        }

        public void DisplayAddedExpense(DateTime date, string catId, double amount, string desc)
        {
            throw new NotImplementedException();
        }

        public void DisplayError(string error)
        {
            throw new NotImplementedException();
        }

        public List<string> GetCategoryList()
        {
            List<string> cats = new List<string>();
            cats = presenter.GetCategoryDescriptionList();

            return cats;
        }
        private void RefreshCategories(List<string> categoriesList)
        {
            update_CB.ItemsSource = categoriesList;
            update_CB.Items.Refresh();
        }
    }
}
