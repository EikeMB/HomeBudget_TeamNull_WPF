using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window, ViewInterface
    {

        private Presenter p;

        public UpdateWindow(Presenter presenter)
        {
            p = presenter;

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
            cats = p.GetCategoryDescriptionList();

            return cats;
        }
        private void RefreshCategories(List<string> categoriesList)
        {
            update_CB.ItemsSource = categoriesList;
            update_CB.Items.Refresh();
        }

        private void Amount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }


        private void UpdateBTN_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            DateTime date = (DateTime)Update_DP.SelectedDate;
            string? category = update_CB.SelectedItem.ToString();
            string? description = Desc_TB.Text;
            double amount = double.Parse(Amount_TB.Text);

            p.processUpdateExpense(id, date, category, amount, description);
        }

        private void CancelBTN_Click(object sender, RoutedEventArgs e)
        {
            update_CB.SelectedIndex = -1;
            Amount_TB.Clear();
            Desc_TB.Clear();
            Update_DP.SelectedDate = DateTime.Today;
        }

        private void DeleteBTN_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
