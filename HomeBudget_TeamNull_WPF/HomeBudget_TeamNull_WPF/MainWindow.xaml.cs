using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Data.Entity.Infrastructure.Design.Executor;
using Application = System.Windows.Application;
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
            datagrid.Visibility= Visibility.Hidden;
            optionsGrid.Visibility= Visibility.Hidden;
            toolbar.Visibility= Visibility.Hidden;
            DropDown.Visibility= Visibility.Hidden;
        }

        private void ShowElements()
        {
            datagrid.Visibility = Visibility.Visible;
            optionsGrid.Visibility = Visibility.Visible;
            toolbar.Visibility = Visibility.Visible;
            DropDown.Visibility = Visibility.Visible;
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
                    presenter.processGetBudgetItems(null, null, false, "credit", null);
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
                        File.Copy(oldFileName,fileName);
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
                        presenter.processGetBudgetItems(null, null, false, "credit", null);
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

        private void buttonColor_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();



        }

        private void BackgroundColorBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();
            WindowBox.Background = brush;
        }

        private void txtfieildBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();

        }

        private void boxColorBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = colorPicker();

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
        #endregion

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
            string? method = null;
            if (monthchk.IsChecked == true && catchk.IsChecked == true)
            {
                method = "month/category";
            }
            else if (monthchk.IsChecked == true)
            {
                method = "month";
            }
            else if (catchk.IsChecked == true)
            {
                method = "category";
            }
            if (presenter != null)
            {
                presenter.processGetBudgetItems(startDate, endDate, filter, cat, method);
            }
        }
        /// <summary>
        /// Updates the datagrid to show all expenses.
        /// </summary>
        /// <param name="dataTable">Datatable to updated the datagrid with.</param>
        public void DisplayExpenses(DataTable dataTable)
        {

            datagrid.ItemsSource = dataTable.DefaultView;

        }
        /// <summary>
        /// Update the datagrid to show all expenses by month.
        /// </summary>
        /// <param name="dataTable">Datatable to updated the datagrid with.</param>
        public void DisplayExpensesByMonth(DataTable dataTable)
        {
            datagrid.ItemsSource = dataTable.DefaultView;
        }
        /// <summary>
        /// Update the datagrid to show all expenses by categories.
        /// </summary>
        /// <param name="dataTable">Datatable to updated the datagrid with.</param>
        public void DisplayExpensesByCategory(DataTable dataTable)
        {
            datagrid.ItemsSource = dataTable.DefaultView;
        }
        /// <summary>
        /// Update the datagrid to show all expenses by category and month.
        /// </summary>
        /// <param name="dataTable">Datatable to updated the datagrid with.</param>
        public void DisplayExpensesByMonthAndCat(DataTable dataTable)
        {

            datagrid.ItemsSource = dataTable.DefaultView;

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

        private void datagrid_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
            if(monthchk.IsChecked == false && catchk.IsChecked == false) {
                if (datagrid.SelectedIndex > -1)
                {
                    ContextMenu menu = this.FindResource("cmButton") as ContextMenu;

                    menu.IsOpen = true;
                }
            }
            
        }

        private void updateCM_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = datagrid.SelectedIndex;

            
            
            TextBlock x = datagrid.Columns[0].GetCellContent(datagrid.Items[selectedIndex]) as TextBlock;
            int expense = int.Parse(x.Text);

            UpdateWindow update = new UpdateWindow(presenter, expense);
            update.ShowDialog();
            GetFilters();
            
        }

        private void deleteCM_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = datagrid.SelectedIndex;
            TextBlock x = datagrid.Columns[0].GetCellContent(datagrid.Items[selectedIndex]) as TextBlock;
            int expense = int.Parse(x.Text);

            presenter.processDeleteExpense(expense);
            GetFilters();
        }

        private void catCB_DropDownOpened(object sender, EventArgs e)
        {
            RefreshCategories(GetCategoryList());
        }

        
        private void datagrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(monthchk.IsChecked == false && catchk.IsChecked == false)
            {
                int selectedIndex = datagrid.SelectedIndex;
                TextBlock x = datagrid.Columns[0].GetCellContent(datagrid.Items[selectedIndex]) as TextBlock;
                int expense = int.Parse(x.Text);

                UpdateWindow uw = new UpdateWindow(presenter, expense);
                uw.ShowDialog();
                GetFilters();
            }
            
        }
    }
}