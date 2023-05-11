using Budget;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using Color = System.Windows.Media.Color;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace HomeBudget_TeamNull_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        private string? fileName = "";
        private string? folderName = "";
        private List<string> categories;
        private bool changeOccured = false;
        private DateTime? startDate;
        private DateTime? endDate;
        private int index = 0;

        private Presenter presenter;

        //warning about presenter being null has to stay for code to work.
        public MainWindow()
        {
            InitializeComponent();
            LoadAppData();
            ShowMenu();
        }

        #region closeWindow

        private void Close_Window(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (changeOccured == true)
            {
                if (MessageBox.Show("Are you sure you want to exit? You will lose unsaved changes", "CLOSING", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void txt_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            changeOccured = true;
        }

        #endregion closeWindow

        #region menu

        private void HideElements()
        {
            datagrid.Visibility = Visibility.Hidden;
            optionsGrid.Visibility = Visibility.Hidden;
            toolbar.Visibility = Visibility.Hidden;
            DropDown.Visibility = Visibility.Hidden;
            searchBar.Visibility = Visibility.Hidden;
        }

        private void ShowElements()
        {
            datagrid.Visibility = Visibility.Visible;
            optionsGrid.Visibility = Visibility.Visible;
            toolbar.Visibility = Visibility.Visible;
            DropDown.Visibility = Visibility.Visible;
            searchBar.Visibility = Visibility.Visible;
            HideMenu();
        }

        private void ShowMenu()
        {
            HideElements();
            menuText.Visibility = Visibility.Visible;
            BTN_existingDB.Visibility = Visibility.Visible;
            BTN_newDB.Visibility = Visibility.Visible;
        }

        private void HideMenu()
        {
            menuText.Visibility = Visibility.Collapsed;
            BTN_existingDB.Visibility = Visibility.Collapsed;
            BTN_newDB.Visibility = Visibility.Collapsed;
        }

        #endregion menu

        #region openDBS

        private void OpenExistingDb(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (folderName == "")
                {
                    dialog.InitialDirectory = "c:\\";
                }
                else
                {
                    dialog.InitialDirectory = folderName;
                }
                dialog.Filter = "Database File (*.db)|*.db";

                if (dialog.ShowDialog() == true)
                {
                    fileName = dialog.FileName;
                    
                    MessageBox.Show("Existing DB file has been picked", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    folderName = System.IO.Path.GetDirectoryName(dialog.FileName);
                    WriteAppData();

                    presenter = new Presenter(this, fileName, false);

                    ShowElements();
                    RefreshCategories(GetCategoryList());
                    presenter.processGetBudgetItems(null, null, false, "credit", false, false, -1);
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void SaveAs(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                if (folderName == "")
                {
                    saveDialog.InitialDirectory = "c:\\";
                }
                else
                {
                    saveDialog.InitialDirectory = folderName;
                }
                saveDialog.Title = "Select location and name of the database to save as.";
                saveDialog.Filter = "Database File (*.db)|*.db";
                saveDialog.FileName = "dbName";
                saveDialog.DefaultExt = ".db";
                saveDialog.OverwritePrompt = true;
                if (saveDialog.ShowDialog() == true)
                {
                    string oldFileName = fileName;
                    fileName = saveDialog.FileName;
                  
                    try
                    {
                        File.Copy(oldFileName, fileName);
                        MessageBox.Show("New DB file has been created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        folderName = Path.GetDirectoryName(fileName);

                        WriteAppData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void OpenNewDb(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                if (folderName == "")
                {
                    saveDialog.InitialDirectory = "c:\\";
                }
                else
                {
                    saveDialog.InitialDirectory = folderName;
                }
                saveDialog.Title = "Select location and name of the new database.";
                saveDialog.Filter = "Database File (*.db)|*.db";
                saveDialog.FileName = "dbName";
                saveDialog.DefaultExt = ".db";

                if (saveDialog.ShowDialog() == true)
                {
                    fileName = saveDialog.FileName;
                    try
                    {
                        File.WriteAllText(fileName, "");
                        MessageBox.Show("New DB file has been created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        presenter = new Presenter(this, fileName, true);

                        folderName = Path.GetDirectoryName(fileName);

                        WriteAppData();

                        ShowElements();
                        RefreshCategories(GetCategoryList());
                        presenter.processGetBudgetItems(null, null, false, "credit", false, false, -1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void OpenFolder(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                folderDialog.InitialDirectory = "c:\\";
                folderDialog.ShowNewFolderButton = true;
                DialogResult result = folderDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    folderName = folderDialog.SelectedPath;

                    WriteAppData();

                    MessageBox.Show("DB folder has been chosen", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void WriteAppData()
        {
            //inspiration taken from here https://stackoverflow.com/questions/10563148/where-is-the-correct-place-to-store-my-application-specific-data
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Directory.CreateDirectory(directory + "\\TicTacToeWPF");
            string path = (Path.Combine(directory, "TicTacToeWPF", "FolderPath.txt"));

            File.WriteAllText(path, folderName);
        }

        private void LoadAppData()
        {
            try
            {
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string path = Path.Combine(directory, "TicTacToeWPF", "FolderPath.txt");
                if (File.Exists(path))
                {
                    string contents = File.ReadAllText(path);

                    folderName = contents;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        #endregion openDBS

        #region colors

        private void ColorChangeMenu(object sender, RoutedEventArgs e)
        {
            HideMenu();
        }

        private void hideColorMenu()
        {
            if (fileName == "")
            {
                ShowMenu();
            }
            else
            {
            }
        }

        private void colorMenuCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            hideColorMenu();
        }

        private SolidColorBrush colorPicker()
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowDialog();
            System.Drawing.Color color = colorDialog.Color;
            Color newColor = Color.FromArgb(color.A, color.R, color.G, color.B);

            SolidColorBrush brush = new SolidColorBrush(newColor);

            return brush;
        }

        #endregion colors

        /// <summary>
        /// Shows the user all details about the added expense in a pop up window.
        /// </summary>
        /// <param name="date">Date of the added expense</param>
        /// <param name="cat">Category of the added expense</param>
        /// <param name="amount">Dollar amount of the added expense</param>
        /// <param name="desc">Description of the added expense</param>
        public void DisplayAddedExpense(DateTime date, string cat, double amount, string desc)
        {
            string successMessage = $"Expense successfully added.\n\n" +
                $"Expense Date: {date.ToLongDateString()}\n" +
                $"Expense Amount: {amount}\n" +
                $"Expense Description: {desc}\n" +
                $"Expense Category: {cat}";
            MessageBox.Show(successMessage);
        }

        /// <summary>
        /// Shows the user all details baout the added category in a pop up window
        /// </summary>
        /// <param name="desc">Description of the added category</param>
        /// <param name="type">Category Type of the added category</param>
        public void DisplayAddedCategory(string desc, string type)
        {
            string successMessage = $"Category successfully added.\n" +
                $"Category Description: {desc}\n" +
                $"Category Type: {type}";
            MessageBox.Show(successMessage);
        }

        /// <summary>
        /// Shows the user the error message in a pop up window
        /// </summary>
        /// <param name="error">The error message to display</param>
        public void DisplayError(string error)
        {

            System.Media.SoundPlayer player = new System.Media.SoundPlayer("../../../../ErrorSound.wav");
            player.Play();
            MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Returns a list of string with all the category names
        /// </summary>
        /// <returns>String list of category names</returns>
        public List<string> GetCategoryList()
        {
            List<string> cats = new List<string>();
            cats = presenter.GetCategoryDescriptionList();
            catCB.ItemsSource = cats;
            return cats;
        }

        private void RefreshCategories(List<string> categoriesList)
        {
            catCB.ItemsSource = categoriesList;
            catCB.Items.Refresh();
        }

        private void catCB_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                string cat = catCB.Text;
                string type = "Expense";
                presenter.processAddCategory(cat, type);
                RefreshCategories(GetCategoryList());
            }
        }

        private void OpenAddWindow(object sender, RoutedEventArgs e)
        {
            AddWindow window2 = new AddWindow(presenter);
            window2.Show();
        }

        private void Start_DP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            GetFilters();
        }

        private void End_DP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            GetFilters();
        }

        private void GetFilters()
        {
            startDate = Start_DP.SelectedDate;
            endDate = End_DP.SelectedDate;
            bool filter = false;
            if (filterchk.IsChecked == true)
            {
                filter = true;
            }
            string cat = "";

            if (catCB.SelectedValue != null)
            {
                cat = catCB.Text;
            }
            if (presenter != null)
            {
                presenter.processGetBudgetItems(startDate, endDate, filter, cat, (bool)monthchk.IsChecked, (bool)catchk.IsChecked, datagrid.SelectedIndex);
            }

        }

        private void filterchk_Click(object sender, RoutedEventArgs e)
        {
            GetFilters();
        }

        private void catCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetFilters();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void catCB_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetFilters();
        }

        private void BackGroundColor(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();
            WindowBox.Background = brush;
        }

        private void GridColour(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();
            datagrid.Background = brush;
        }

        private void ButtonColour(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();

            foreach (Button btn in FindVisualChildren<Button>(this))
            {
                btn.Background = brush;
            }
        }


        //taken from the following link:
        //https://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield return (T)Enumerable.Empty<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) continue;
                if (ithChild is T t) yield return t;
                foreach (T childOfChild in FindVisualChildren<T>(ithChild)) yield return childOfChild;
            }
        }

        private void updateCM_Click(object sender, RoutedEventArgs e)
        {
            BudgetItem selected = datagrid.SelectedItem as BudgetItem;
            if (selected != null)
            {
                UpdateWindow update = new UpdateWindow(presenter, selected, datagrid.SelectedIndex, datagrid.Items.Count);
                update.ShowDialog();
                GetFilters();
            }
      
        }

        private void deleteCM_Click(object sender, RoutedEventArgs e)
        {
            BudgetItem selected = datagrid.SelectedItem as BudgetItem;

            
            presenter.processDeleteExpense(selected);
            if (datagrid.SelectedIndex >= datagrid.Items.Count - 1)
            {
                datagrid.SelectedIndex -= 1;
            }
            GetFilters();
            
        }

        private void catCB_DropDownOpened(object sender, EventArgs e)
        {
            RefreshCategories(GetCategoryList());
        }

        private void datagrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (monthchk.IsChecked == false && catchk.IsChecked == false)
                {
                    BudgetItem selected = datagrid.SelectedItem as BudgetItem;
                    if(selected != null)
                    {
                        UpdateWindow uw = new UpdateWindow(presenter, selected, datagrid.SelectedIndex, datagrid.Items.Count);
                        uw.ShowDialog();
                        GetFilters();

                    }
               
                }
                else if (monthchk.IsChecked == true && catchk.IsChecked == false)
                {
                    BudgetItemsByMonth selected = datagrid.SelectedItem as BudgetItemsByMonth;
                    MonthFilter filter = new MonthFilter(presenter, selected);
                    filter.ShowDialog();
                }
            }
            catch (Exception)
            {
            }
        }

        public void SetupDataGridDefault(List<BudgetItem> budgetItems, int index)
        {
            if (budgetItems.Count == 0)
            {
                searchBtn.IsEnabled = false;
            }
            else
            {
                searchBtn.IsEnabled = true;
            }
            datagrid.ItemsSource = budgetItems;
            datagrid.Columns.Clear();

            var column1 = new DataGridTextColumn();
            column1.Header = "Date";
            column1.Binding = new System.Windows.Data.Binding("Date");
            column1.Binding.StringFormat = "{0:MM/dd/yyyy}";
             datagrid.Columns.Add(column1);

            var column2 = new DataGridTextColumn();
            column2.Header = "Category";
            column2.Binding = new System.Windows.Data.Binding("Category");

            datagrid.Columns.Add(column2);

            var column3 = new DataGridTextColumn();
            column3.Header = "Description";
            column3.Binding = new System.Windows.Data.Binding("ShortDescription");

            datagrid.Columns.Add(column3);

            var column4 = new DataGridTextColumn();
            column4.Header = "Amount";
            column4.Binding = new System.Windows.Data.Binding("Amount");

            column4.Binding.StringFormat = "F2";
            Style s = new Style();
            s.Setters.Add(new Setter(TextBlock.TextAlignmentProperty,
                                    TextAlignment.Right));
            column4.CellStyle = s;

            datagrid.Columns.Add(column4);

            var column5 = new DataGridTextColumn();
            column5.Header = "Balance";
            column5.Binding = new System.Windows.Data.Binding("Balance");

            column5.Binding.StringFormat = "F2";
            Style s2 = new Style();
            s2.Setters.Add(new Setter(TextBlock.TextAlignmentProperty,
                                    TextAlignment.Right));
            column5.CellStyle = s2;

            datagrid.Columns.Add(column5);

            if (index >= 0)
            {



                if (datagrid.Items.Count >= index)
                {
                    datagrid.UpdateLayout();
                    datagrid.ScrollIntoView(datagrid.Items[index]);
                    DataGridRow row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(index);
                    SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 135, 206, 250));
                    row.Background = brush;
                }
            }
        }

        public void SetupDataGridMonth(List<BudgetItemsByMonth> budgetItemsByMonth)
        {
            searchBtn.IsEnabled = false;
            datagrid.ItemsSource = budgetItemsByMonth;
            datagrid.Columns.Clear();

            var column1 = new DataGridTextColumn();
            column1.Header = "Month";
            column1.Binding = new System.Windows.Data.Binding("Month");

            datagrid.Columns.Add(column1);

            var column2 = new DataGridTextColumn();
            column2.Header = "Total";
            column2.Binding = new System.Windows.Data.Binding("Total");

            datagrid.Columns.Add(column2);
        }

        public void SetupDataGridCategory(List<BudgetItemsByCategory> budgetItemsByCategory)
        {
            searchBtn.IsEnabled = false;
            datagrid.ItemsSource = budgetItemsByCategory;
            datagrid.Columns.Clear();

            var column1 = new DataGridTextColumn();
            column1.Header = "Categories";
            column1.Binding = new System.Windows.Data.Binding("Category");

            datagrid.Columns.Add(column1);

            var column2 = new DataGridTextColumn();
            column2.Header = "Total";
            column2.Binding = new System.Windows.Data.Binding("Total");

            datagrid.Columns.Add(column2);
        }

        public void SetupDataGridMonthCategory(List<Dictionary<string, object>> budgetItemsByMonthCategory)
        {
            searchBtn.IsEnabled = false;
            datagrid.ItemsSource = budgetItemsByMonthCategory;
            datagrid.Columns.Clear();

            var column = new DataGridTextColumn();
            column.Header = "Month";
            column.Binding = new System.Windows.Data.Binding("[Month]");
            datagrid.Columns.Add(column);

            foreach (string category in presenter.GetCategoryDescriptionList())
            {
                var column2 = new DataGridTextColumn();
                column2.Header = category;
                column2.Binding = new System.Windows.Data.Binding($"[{category}]");
                datagrid.Columns.Add(column2);
            }

            var column3 = new DataGridTextColumn();
            column3.Header = "Total";
            column3.Binding = new System.Windows.Data.Binding("[Total]");
            datagrid.Columns.Add(column3);
        }

        private void datagrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (monthchk.IsChecked != true && catchk.IsChecked != true)
            {
                DgCm.Visibility = Visibility.Visible;
            }
            else
            {
                DgCm.Visibility = Visibility.Collapsed;
            }
        }

        private void searchBtn_click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < datagrid.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(i);
                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0, 135, 206, 250));
                row.Background = brush;
            }
            if(datagrid.SelectedIndex > 0)
            {
                this.index = datagrid.SelectedIndex;
            }
            presenter.processSearch(searchTxt.Text, datagrid.ItemsSource, this.index);
        }

        public void HighlightSearch(int index)
        {
            this.datagrid.SelectedIndex = -1;
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 135, 206, 250));
            DataGridRow row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(index);
            row.Background = brush;
            this.index = (index + 1) % datagrid.Items.Count;
        }
        

        private void searchTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTxt.Text == "Expense")
            {
                searchTxt.Text = "";
            }
        }

    }
}