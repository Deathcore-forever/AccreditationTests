using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
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

namespace AccreditationTests.Admin
{
    /// <summary>
    /// Логика взаимодействия для AddEditTest.xaml
    /// </summary>
    public partial class AddEditTest : Window
    {
        private readonly string pathToDB = AppDomain.CurrentDomain.BaseDirectory + "\\resources\\AccreditationTests.db";
        string lastRecordID = String.Empty;
        string workingMode = String.Empty;

        private string testID = String.Empty;
        public AddEditTest()
        {
            InitializeComponent();
            GetLasRecordID();

            workingMode = "add";
            WindowHeader.Text = "Добавление теста";
            addTestButton.Content = "Добавить тест";
        }
        public AddEditTest(string _testID, string testName, int responsesCount)
        {
            InitializeComponent();
            GetLasRecordID();

            workingMode = "edit";
            WindowHeader.Text = "Изменение теста";
            testNameText.Text = testName;
            responseNumberText.Text = responsesCount.ToString();
            testID = _testID;
            addTestButton.Content = "Изменить тест";
        }

        private void GetLasRecordID()
        {
            SqliteConnection connection = new SqliteConnection();
            connection.ConnectionString = "Data Source=" + pathToDB;

            connection.Open();

            //SqliteCommand command = new SqliteCommand
            //($"SELECT * FROM Test{testNumber} ORDER BY ID DESC LIMIT 1", connection);

            SqliteCommand command = new SqliteCommand
            ($"SELECT * FROM Tests ORDER BY ID DESC LIMIT 1", connection);

            lastRecordID = command.ExecuteScalar().ToString();
            connection.Close();
        }

        private void MainButtonClick(object sender, RoutedEventArgs e)
        {
            if (testNameText.Text.Length == 0 || responseNumberText.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                SqliteConnection connection = new SqliteConnection();
                connection.ConnectionString = "Data Source=" + pathToDB;

                connection.Open();

                ComboBoxItem selectedItem = (ComboBoxItem)responseNumberText.SelectedItem;
                switch (workingMode)
                {
                    case "edit":

                        //запрос к БД для изменения данных
                        SqliteCommand editTest = new SqliteCommand
                            ($"update Tests set (Name, ResponseNumber) = ('{testNameText.Text}', {selectedItem.Content.ToString()}) where ID = {testID}", connection);


                        //проверка на успешное выполнение
                        if (editTest.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Изменение было успешно выполнено!", "Успешное выполнение", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при изменении", "Неудачное выполнение", MessageBoxButton.OK, MessageBoxImage.Error);
                            this.Hide();
                        }
                        break;

                    case "add":

                        SqliteCommand addTest = new SqliteCommand
                            ($"INSERT INTO Tests VALUES ({Convert.ToInt32(lastRecordID) + 1}, '{testNameText.Text}', {selectedItem.Content.ToString()})", connection);

                        if (addTest.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Вопрос успешно добавлен, чтобы увидеть изменения, перезагрузите приложение", "Добавление успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Вопрос не был добавлен, попробуйте снова", "Добавление неудачно", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        connection.Close();
                        break;
                }

            }
            //обновление номера последнего вопроса
            GetLasRecordID();
        }
    }
}
