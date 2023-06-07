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
using TestDesign.Data;
using Microsoft.Data.Sqlite;

namespace TestDesign
{
    /// <summary>
    /// Логика взаимодействия для AddEditQuestion.xaml
    /// </summary>
    public partial class AddEditQuestion : Window
    {
        private readonly string pathToDB = AppDomain.CurrentDomain.BaseDirectory + "\\resources\\AccreditationTests.db";
        string lastRecordID = String.Empty;
        string workingMode = String.Empty;

        private string qID = String.Empty;

        private int testNumber = 0;
        private List<Test> tests = new List<Test>();

        /// <summary>
        /// Конструктор окна, определяющий режим работы "добавление"
        /// </summary>
        public AddEditQuestion(int test)
        {
            InitializeComponent();

            GetLasRecordID();
            LoadTests();
            testNumber = test;

            workingMode = "add";
            WindowHeader.Text = "Добавление вопроса";

            mainButton.Content = "Добавить вопрос";

            if(tests.Where(x => x.ID == testNumber).First().ResponseNumber == 4)
            {
                response5Text.Opacity = 0.5;
                response5Text.IsEnabled = false;
            }
        }

        /// <summary>
        /// Конструктор окна, определяющий режим работы "изменение"
        /// </summary>
        /// <param name="questionID">Номер вопроса</param>
        /// <param name="questionName">Название вопроса(сам вопрос)</param>
        /// <param name="response1">первый ответ</param>
        /// <param name="response2">второй ответ</param>
        /// <param name="response3">третий ответ</param>
        /// <param name="response4">четвертый ответ</param>
        
        public AddEditQuestion(int test, string questionID, string questionName, string response1, string response2, string response3, string response4, string response5)
        {
            InitializeComponent();

            GetLasRecordID();
            LoadTests();
            testNumber = test;

            workingMode = "edit";
            WindowHeader.Text = "Изменение вопроса";

            questionText.Text = questionName;
            response1Text.Text = response1;
            response2Text.Text = response2;
            response3Text.Text = response3;
            response4Text.Text = response4;
            response5Text.Text = response5;

            qID = questionID;

            mainButton.Content = "Изменить вопрос";
            testsListContainer.Visibility = Visibility.Collapsed;

            if (tests.Where(x => x.ID == testNumber).First().ResponseNumber == 4)
            {
                response5Text.Opacity = 0.5;
                response5Text.IsEnabled = false;
            }
        }

        private void LoadTests()
        {
            SqliteConnection connection = new SqliteConnection();
            connection.ConnectionString = "Data Source=" + pathToDB;

            connection.Open();

            SqliteCommand command = new SqliteCommand
            ($"SELECT * FROM Tests", connection);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int ID = reader.GetInt32(0);
                        string testName = reader.GetString(1);
                        int testResponseNumber = reader.GetInt32(2);

                        tests.Add(new Test(ID, testName, testResponseNumber));
                    }
                }
            }

            connection.Close();

            if (tests.Count != 0) testsList.DataContext = tests;
            else MessageBox.Show("Ошибки загрузки данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Возвращает номер последнего вопроса (замена autoincrement)
        /// </summary>
        private void GetLasRecordID()
        {
            SqliteConnection connection = new SqliteConnection();
            connection.ConnectionString = "Data Source=" + pathToDB;

            connection.Open();

            //SqliteCommand command = new SqliteCommand
            //($"SELECT * FROM Test{testNumber} ORDER BY ID DESC LIMIT 1", connection);

            SqliteCommand command = new SqliteCommand
            ($"SELECT * FROM Questions ORDER BY ID DESC LIMIT 1", connection);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if(reader.GetValue(0) == null || reader.GetValue(0) == DBNull.Value) lastRecordID = "1";
                        else lastRecordID = reader.GetValue(0).ToString();
                    }
                }
            }
            connection.Close();
        }

        /// <summary>
        /// Обработчик нажатия на кнопку. Отправляет запрос к БД, определяющийся режимом работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainButtonClick(object sender, RoutedEventArgs e)
        {
            if (questionText.Text.Length == 0 || response1Text.Text.Length == 0 || response2Text.Text.Length == 0
                || response3Text.Text.Length == 0 || response4Text.Text.Length == 0)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                SqliteConnection connection = new SqliteConnection();
                connection.ConnectionString = "Data Source=" + pathToDB;

                connection.Open();

                switch (workingMode)
                {
                    case "edit":

                        //запрос к БД для изменения данных
                        SqliteCommand editQuestion = new SqliteCommand();
                        
                        if(tests.Where(x => x.ID == testNumber).First().ResponseNumber == 4)
                        {
                            editQuestion = new SqliteCommand
                            //($"update Test{testNumber} set (Name, Response1, Response2, Response3, Response4) = " +
                            //$"('{Cryptography.Encrypt(questionText.Text)}','{Cryptography.Encrypt(response1Text.Text)}','{Cryptography.Encrypt(response2Text.Text)}','{Cryptography.Encrypt(response3Text.Text)}','{Cryptography.Encrypt(response4Text.Text)}') where ID = {qID}", connection);
                            ($"update Questions set (Name, Response1, Response2, Response3, Response4) = " +
                            $"('{Cryptography.Encrypt(questionText.Text)}','{Cryptography.Encrypt(response1Text.Text)}','{Cryptography.Encrypt(response2Text.Text)}','{Cryptography.Encrypt(response3Text.Text)}','{Cryptography.Encrypt(response4Text.Text)}') where ID = {qID}", connection);
                        }
                        else
                        {
                            editQuestion = new SqliteCommand
                            ($"update Questions set (Name, Response1, Response2, Response3, Response4, Response5) = " +
                    $"('{Cryptography.Encrypt(questionText.Text)}','{Cryptography.Encrypt(response1Text.Text)}','{Cryptography.Encrypt(response2Text.Text)}','{Cryptography.Encrypt(response3Text.Text)}','{Cryptography.Encrypt(response4Text.Text)}', '{Cryptography.Encrypt(response5Text.Text)}') where ID = {qID}", connection);
                        }

                        //проверка на успешное выполнение
                        if (editQuestion.ExecuteNonQuery() == 1)
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
                        if (testsList.SelectedIndex != -1)
                        {
                            //запрос к БД для добавления вопроса
                            SqliteCommand addQuestion = new SqliteCommand();
                            if (tests.Where(x => x.ID == testNumber).First().ResponseNumber == 4)
                            {
                                addQuestion = new SqliteCommand
                                ($"INSERT INTO Questions VALUES ({Convert.ToInt32(lastRecordID) + 1},'{Cryptography.Encrypt(questionText.Text)}', {tests.Where(x => x.Name == testsList.SelectedItem.ToString()).First().ID}, '{Cryptography.Encrypt(response1Text.Text)}', '{Cryptography.Encrypt(response2Text.Text)}', '{Cryptography.Encrypt(response3Text.Text)}', '{Cryptography.Encrypt(response4Text.Text)}', null)", connection);
                            }
                            else
                            {
                                addQuestion = new SqliteCommand
                                ($"INSERT INTO Questions VALUES ({Convert.ToInt32(lastRecordID) + 1},'{Cryptography.Encrypt(questionText.Text)}', {tests.Where(x => x.Name == testsList.SelectedItem.ToString()).First().ID}, '{Cryptography.Encrypt(response1Text.Text)}', '{Cryptography.Encrypt(response2Text.Text)}', '{Cryptography.Encrypt(response3Text.Text)}', '{Cryptography.Encrypt(response4Text.Text)}', '{Cryptography.Encrypt(response5Text.Text)}')", connection);
                            }

                            if (addQuestion.ExecuteNonQuery() == 1)
                            {
                                MessageBox.Show("Вопрос успешно добавлен, чтобы увидеть изменения, перезагрузите приложение", "Добавление успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                            }
                            else
                            {
                                MessageBox.Show("Вопрос не был добавлен, попробуйте снова", "Добавление неудачно", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            connection.Close();
                        }
                        else MessageBox.Show("Выберите тест, в который добавляется вопрос", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }              
                               
            }
            //обновление номера последнего вопроса
            GetLasRecordID();
        }
    }
}
