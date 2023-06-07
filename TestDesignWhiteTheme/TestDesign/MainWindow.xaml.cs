using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml;
using Microsoft.Data.Sqlite;
using TestDesign.Data;
using TestDesign.Extensions;

namespace TestDesign
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqliteConnection connection = new SqliteConnection();
        private string pathToDB = AppDomain.CurrentDomain.BaseDirectory + "\\resources\\AccreditationTests.db";

        //Хранит список вопросов
        private List<Question> questions;
        private List<Test> tests = new List<Test>();

        private int testNumber = 0;
        private int responsesCount = 0;

        private void LoadTests()
        {
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
                        int testID = reader.GetInt32(0);
                        string testName = reader.GetString(1);
                        int testResponseNumber = reader.GetInt32(2);

                        tests.Add(new Test(testID, testName, testResponseNumber));
                    }
                }
            }

            if (tests.Count != 0) TestList.DataContext = tests;
            else MessageBox.Show("Произошла ошибка загрузки данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            connection.Close();
        }

        public MainWindow()
        {
            InitializeComponent();

            LoadTests();
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Обработчик нажатия на кнопку. Опеределяет какие вопросы будут отображаться
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadTestFromMenu(object sender, MouseButtonEventArgs e)
        {

            if (MessageBox.Show("Вы уверены, что хотите перейти на другой тест? Все ваши данные не сохраняются", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //запоминаем выбранный тест
                var selectedTest = tests.Where(x => x.Name == ((TextBlock)e.Source).Text).First();

                var testResponseNumber = selectedTest.ResponseNumber;

                //строка шаблона вопроса в списке
                var dataTemplateString =
                        @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" 
                  xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" x:Name=""questionTemplate"">
                        <StackPanel Margin=""10 0 0 20"" x:Name=""questionTemplateStackPanel"">

                            <TextBlock HorizontalAlignment=""Left"" Width=""1000"" TextWrapping=""WrapWithOverflow"" FontSize=""24"" Foreground=""darkslategray"" Margin=""0 0 0 10"">
                            <Run Text=""{Binding ShowNumber}""/>. <Run Text=""{Binding Name}""/></TextBlock>";

                //заполняем radioButton-ами в зависимости от кол-ва ответов в тесте
                for (int i = 1; i <= testResponseNumber; i++)
                {
                    dataTemplateString += @"
                    <RadioButton VerticalAlignment=""Center"" Cursor=""Hand"" x:Name=""" + $"response{i}" + @"""  Margin=""0 0 0 10"" FontSize=""18"" Foreground=""CadetBlue"">
                    <TextBlock x:Name=""" + $"response{i}Text" + @""" HorizontalAlignment=""Left""  Width=""1000"" TextWrapping=""WrapWithOverflow"" Text=""{" + $"Binding Responses[{i-1}]" + @"}""></TextBlock>
                    </RadioButton>";
                }
                dataTemplateString += @"                           
                        </StackPanel>
                    </DataTemplate>";

                var stringReader = new StringReader(dataTemplateString);
                XmlReader xmlReader = XmlReader.Create(stringReader);
                DataTemplate dataTemplate = (DataTemplate)XamlReader.Load(xmlReader);

                //устанавливаем шаблон
                questionsList.ItemTemplate = dataTemplate;

                //отправоляем запрос к БД
                if (selectedTest.ID != 0 && selectedTest.ResponseNumber != 0)
                {
                    testNumber = selectedTest.ID;
                    responsesCount = selectedTest.ResponseNumber;

                    LoadTest();
                }
                else MessageBox.Show("Не было найдено такого элемента");
            }
        }
        /// <summary>
        /// Загружает данные из базы данных
        /// </summary>
        private void LoadTest()
        {
            try
            {
                connection.ConnectionString = "Data Source=" + pathToDB;
                connection.Open();

                //SqliteCommand command = new SqliteCommand
                //($"SELECT * FROM Test{testNumber} ORDER BY random()", connection);

                SqliteCommand command = new SqliteCommand
                    ($"SELECT * FROM Questions where TestID = {testNumber} ORDER BY random()", connection);

                GetQuestionListAsync(command.ExecuteReader());

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла ошибка загрузки данных");
            }
        }
        /// <summary>
        /// Асинхронно читает данные из базы и отображает их в окне
        /// </summary>
        /// <param name="reader">Параметр класса SqliteDataReader для чтения потока строк</param>
        private async void GetQuestionListAsync(SqliteDataReader reader)
        {
            try
            {
                //вспомогательная переменная для отображения номеров вопросов
                int i = 1;
                List<Question> questionListFromDB = new List<Question>();

                //асинхронное чтение строк
                using (reader)
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            List<string> responses = new List<string>();

                            var questionNumber = reader.GetValue(0).ToString();
                            var questionName = reader.GetValue(1).ToString();

                            for(int j = 0; j < responsesCount; j++)
                            {
                                responses.Add(Cryptography.Decrypt(reader.GetString(j+3)));
                            }
                            //добавление вопроса в общий список  
                            questionListFromDB.Add(new Question(testNumber, false, questionNumber, i.ToString(), Cryptography.Decrypt(questionName), responses.ToArray()));
                            i++;
                        }
                    }
                }
                questions = questionListFromDB;
                //отображение списка вопросов путём присвоения источнику данных переменной списка
                questionsList.DataContext = questionListFromDB;
                //DataContext = questionList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла ошибка: " + ex.Message);
            }
        }


        /// <summary>
        /// Обработчик нажатия на кнопку. Отображает результат теста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowTestResult(object sender, RoutedEventArgs e)
        {
            try
            {
                //список выбранных ответов
                List<string> responsesText = new List<string>();

                //запоминаем каждый вопрос из окна в переменную
                foreach (ListBoxItem item in FindingObjects.FindVisualChildren<ListBoxItem>(questionsList))
                {
                    ContentPresenter myContentPresenter = FindingObjects.FindVisualChild<ContentPresenter>(item);
                    DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;

                    //ищем выбранный ответ и запоминаем его
                    for (int i = 1; i <= responsesCount; i++)
                    {
                        RadioButton response = (RadioButton)myDataTemplate.FindName($"response{i}", myContentPresenter);

                        //добавляем ответ в список
                        if (response.IsChecked.Value)
                        {
                            TextBlock responseText = (TextBlock)myDataTemplate.FindName($"response{i}Text", myContentPresenter);

                            responsesText.Add(responseText.Text);
                        }
                    }
                }                
                //создаём список, каждый элемент которого это один совпавший ответ
                var sameReponses = from q in questions
                                   join r in responsesText on q.RightResponse equals r
                                   select new { Name = "1" };
                //выводим количество правильных ответов
                MessageBox.Show($"Вы правильно ответили на {sameReponses.Count()} из {questions.Count()} ответов");

                //очищаем окно от предыдущих вопросов
                questionsList.DataContext = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла Ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock selectedText = (TextBlock)e.Source;
            selectedText.Foreground = Brushes.DarkSlateGray;
            selectedText.Background = (Brush)new BrushConverter().ConvertFrom("#99BFCB");
            selectedText.FontSize += 2;
            Cursor = Cursors.Hand;
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock selectedText = (TextBlock)e.Source;
            selectedText.Foreground = (Brush)new BrushConverter().ConvertFrom("#317F97");
            selectedText.Background = null;
            selectedText.FontSize -= 2;
            Cursor = Cursors.Arrow;
        }

        private void RadioButtonMouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock selectedRadButText = (TextBlock)e.Source;
            selectedRadButText.TextDecorations = TextDecorations.Underline;

            Cursor = Cursors.Hand;
        }

        private void RadioButtonMouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock selectedRadButText = (TextBlock)e.Source;
            selectedRadButText.TextDecorations = null;

            Cursor = Cursors.Arrow;
        }
        /// <summary>
        /// Обработчик события закрытия формы. Отображает окно подтверждения выхода из приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Все ваши данные не сохранятся, вы уверены, что хотите выйти?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else Application.Current.Shutdown();
        }

    }
}

