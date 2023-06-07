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
using System.Windows.Shapes;
using Microsoft.Data.Sqlite;
using TestDesign.Data;
using TestDesign.Extensions;

namespace TestDesign
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        SqliteConnection connection = new SqliteConnection();
        private string pathToDB = AppDomain.CurrentDomain.BaseDirectory + "\\resources\\AccreditationTests.db"; //путь к базе данных
        public LoginWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Открытие окна с тестами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMainWindow(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); 
            this.Hide();
            mainWindow.Show();
        }
        /// <summary>
        /// Открытие панели администратора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenAdminPanel(object sender, RoutedEventArgs e)
        {
            //проверка на ввод логина и пароля
            if(loginText.Text.Length == 0 || passwordText.Password.Length == 0)
            {
                MessageBox.Show("Заполните все поля","Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                connection.ConnectionString = "Data Source=" + pathToDB;

                connection.Open();

                //проверяем есть ли в базе такой администратор
                SqliteCommand getAdmins = new SqliteCommand($"SELECT ID FROM Administrators " +
                    $"WHERE Login = '{Cryptography.Encrypt(loginText.Text)}' and Password = '{Cryptography.Encrypt(passwordText.Password)}'", connection);

                //открытие панели администратора
                if(getAdmins.ExecuteScalar() != null)
                {
                    AdminPanel adminPanel = new AdminPanel();
                    this.Hide();
                    adminPanel.Show();
                }
                else
                {
                    MessageBox.Show("Введённые данные неверны", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                connection.Close();
            }
        }

        /*private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button selectedBut = (Button)e.Source;

            ContentPresenter myContentPresenter = FindingObjects.FindVisualChild<ContentPresenter>(selectedBut);
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            
            TextBlock butText = (TextBlock)myDataTemplate.FindName($"studentButText", myContentPresenter);

            butText.TextDecorations = TextDecorations.Underline;

            Cursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button selectedBut = (Button)e.Source;

            ContentPresenter myContentPresenter = FindingObjects.FindVisualChild<ContentPresenter>(selectedBut);
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;

            TextBlock butText = (TextBlock)myDataTemplate.FindName($"adminButText", myContentPresenter);

            butText.TextDecorations = TextDecorations.Underline;

            Cursor = Cursors.Arrow;
            selectedBut.Background = (Brush)new BrushConverter().ConvertFrom("#317F97");
        }*/
    }
}
