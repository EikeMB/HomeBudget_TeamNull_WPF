using Budget;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace HomeBudget_TeamNull_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,ViewInterface
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowMenu();
        }

       private void ShowMenu()
        {

        }




        public void DisplayAddedCategory(Category category)
        {
            MessageBox.Show(category.Description, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void DisplayAddedExpense(Expense expense)
        {
            MessageBox.Show(expense.Description, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void DisplayError(string error)
        {
           MessageBox.Show(error,"Error",MessageBoxButton.OK,MessageBoxImage.Warning);
        }

        private void OpenExistingDb(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
        }

        private void OpenNewDb(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Integration.WindowsFormsHost host =
            new System.Windows.Forms.Integration.WindowsFormsHost();

            FolderBrowserDialog dialog = new FolderBrowserDialog(host);

            
        
        }

    }
}
