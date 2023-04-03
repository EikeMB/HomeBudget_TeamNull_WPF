﻿using Budget;
using Microsoft.VisualBasic;
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
    public partial class MainWindow : Window, ViewInterface
    {

        Presenter presenter;

        public MainWindow()
        {
            InitializeComponent();
            ShowMenu();
            dp.SelectedDate = DateTime.Today;
            catCB.ItemsSource= new List<Category>();
            
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
            MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = (DateTime)dp.SelectedDate;
            string category = catCB.SelectedItem.ToString();
            double amount = double.Parse(amountTB.Text);
            string description = descriptionTB.Text;

            catCB.SelectedIndex= 0;
            amountTB.Clear();
            descriptionTB.Clear();
            
            presenter.processAddExpense(date,category,amount,description);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            catCB.SelectedIndex = 0;
            amountTB.Clear();
            descriptionTB.Clear();
        }
    }
    }
