﻿using Budget;
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

        private void DescInput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock txtblk = (TextBlock)sender;
            if (txtblk.Text == "Description...")
            {
                txtblk.Text = string.Empty;
            }
        }

        public void DisplayAddedExpense(DateTime date, int catId, double amount, string desc)
        {
            throw new NotImplementedException();
        }

        public void DisplayAddedCategory(string desc, string type)
        {
            throw new NotImplementedException();
        }
    }
}
