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
using AccreditationTests.Admin;
using Microsoft.Data.Sqlite;
using TestDesign.Extensions;
using TestDesign.Data;

namespace TestDesign
{
    /// <summary>
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        SqliteConnection connection = new SqliteConnection();
        private string pathToDB = AppDomain.CurrentDomain.BaseDirectory + "\\resources\\AccreditationTests.db";

        private List<Question> questions = new List<Question>();
        private List<Test> tests = new List<Test>();

        private int testNumber = 0;
        private int responseNumber = 0;
      
        public AdminPanel()
        {
            InitializeComponent();
            LoadTests();
        }

        private void LoadTests()
        {
            tests.Clear();

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

        private void LoadTestFromMenu(object sender, MouseButtonEventArgs e)
        {
            var selectedTest = tests.Where(x => x.Name == ((TextBlock)e.Source).Text).First();
            responseNumber = selectedTest.ResponseNumber;

            if (selectedTest.ID != 0)
            {
                testNumber = selectedTest.ID;
                if (questions.Count() != 0) questionsList.DataContext = questions.Where(x => x.TestID == testNumber);
                else LoadTest();

                questionsList.Visibility = Visibility.Visible;
                adminsList.Visibility = Visibility.Hidden;
            }
            else MessageBox.Show("Не было найдено такого элемента");
        }

        private void LoadTest()
        {
            connection.ConnectionString = "Data Source=" + pathToDB;

            connection.Open();

            /*SqliteCommand command = new SqliteCommand
                ($"SELECT * FROM Test{testNumber}", connection);*/

            SqliteCommand command = new SqliteCommand
                ($"SELECT * FROM Questions", connection);

            GetQuestionListAsync(command.ExecuteReader());

            connection.Close();
        }

        private async void GetQuestionListAsync(SqliteDataReader reader)
        {
            int i = 1;
            using (reader)
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        var questionNumber = reader.GetString(0);
                        var questionName = reader.GetString(1);
                        var testID = reader.GetInt32(2);

                        List<string> responses = new List<string>();

                        responses.Add(Cryptography.Decrypt(reader.GetString(3)));
                        responses.Add(Cryptography.Decrypt(reader.GetString(4)));
                        responses.Add(Cryptography.Decrypt(reader.GetString(5)));

                        string fourthReponse = reader.IsDBNull(6) ? String.Empty : Cryptography.Decrypt(reader.GetString(6));
                        responses.Add(fourthReponse);
                        
                        string fifthResponse = reader.IsDBNull(7) ? String.Empty : Cryptography.Decrypt(reader.GetString(7));
                        responses.Add(fifthResponse);

                        questions.Add(new Question(testID, true, questionNumber, i.ToString(), Cryptography.Decrypt(questionName), responses.ToArray()));
                        //questionList.Add(new Question(testNumber, true, questionNumber, i.ToString(), questionName, firstResponse, secondResponse, thirdResponse, fourthResponse, fifthResponse));
                        i++;
                    }
                }
            }
            questionsList.DataContext = questions.Where(x => x.TestID == testNumber);
        }

        private void OpenInstruction(object sender, MouseButtonEventArgs e)
        {
            var proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\resources\\Инструкция.pdf";
            proc.StartInfo.UseShellExecute = true;
            proc.Start();

        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock selectedText = (TextBlock)e.Source;
            selectedText.Foreground = Brushes.DarkSlateGray;
            selectedText.FontSize += 2;
            Cursor = Cursors.Hand;
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock selectedText = (TextBlock)e.Source;
            selectedText.Foreground = (Brush)new BrushConverter().ConvertFrom("#317F97");
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
        

        private void EditQuestion(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem mi = sender as MenuItem;
                if (mi != null)
                {
                    ContextMenu cm = mi.CommandParameter as ContextMenu;
                    if (cm != null)
                    {
                        ListBoxItem item = cm.PlacementTarget as ListBoxItem;

                        if (item != null)
                        {
                            ContentPresenter myContentPresenter = FindingObjects.FindVisualChild<ContentPresenter>(item);
                            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;

                            TextBlock questionID = (TextBlock)myDataTemplate.FindName("qId", myContentPresenter);
                            TextBlock questionTitle = (TextBlock)myDataTemplate.FindName("questionTitle", myContentPresenter);
                            TextBlock response1 = (TextBlock)myDataTemplate.FindName("response1Text", myContentPresenter);
                            TextBlock response2 = (TextBlock)myDataTemplate.FindName("response2Text", myContentPresenter);
                            TextBlock response3 = (TextBlock)myDataTemplate.FindName("response3Text", myContentPresenter);
                            TextBlock response4 = (TextBlock)myDataTemplate.FindName("response4Text", myContentPresenter);
                            TextBlock response5 = (TextBlock)myDataTemplate.FindName("response5Text", myContentPresenter);

                            AddEditQuestion workWindow = new AddEditQuestion(testNumber, questionID.Text, questionTitle.Text, response1.Text, response2.Text, response3.Text, response4.Text, response5.Text);
                            workWindow.Show();
                        }
                        else MessageBox.Show("Не было найдено такого элемента");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла непредвиденная ошикбка, попробуйте перезапустить приложение");
            }
        }

        private void DeleteQuestion(object sender, RoutedEventArgs e)
        {
            connection.ConnectionString = "Data Source=" + pathToDB;
            try
            {
                MenuItem mi = sender as MenuItem;
                if (mi != null)
                {
                    ContextMenu cm = mi.CommandParameter as ContextMenu;
                    if (cm != null)
                    {
                        ListBoxItem item = cm.PlacementTarget as ListBoxItem;

                        if (item != null)
                        {
                            ContentPresenter myContentPresenter = FindingObjects.FindVisualChild<ContentPresenter>(item);
                            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;

                            TextBlock questionID = (TextBlock)myDataTemplate.FindName("qId", myContentPresenter);

                            connection.Open();

                            //SqliteCommand deleteQuestion = new SqliteCommand
                            //($"DELETE FROM Test{testNumber} where ID = {questionID.Text}", connection);

                            if (MessageBox.Show("Вы уверены, что хотите удалить этот вопрос?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {
                                SqliteCommand deleteQuestion = new SqliteCommand
                                    ($"DELETE FROM Questions where ID = {questionID.Text}", connection);

                                if (deleteQuestion.ExecuteNonQuery() != 0) MessageBox.Show("Удаление прошло успешно");
                                else MessageBox.Show("Ошибка при удалении", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            connection.Close();
                        }
                        else MessageBox.Show("Не было найдено такого элемента");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла непредвиденная ошикбка, попробуйте перезапустить приложение");
            }
        }        

        private void AddQuestion(object sender, RoutedEventArgs e)
        {
            AddEditQuestion addWindow = new AddEditQuestion(testNumber);
            addWindow.Show();
        }

        private void DeleteTest(object sender, RoutedEventArgs e)
        {
            connection.ConnectionString = "Data Source=" + pathToDB;
            try
            {
                MenuItem mi = sender as MenuItem;
                if (mi != null)
                {
                    ContextMenu cm = mi.CommandParameter as ContextMenu;
                    if (cm != null)
                    {
                        ListBoxItem item = cm.PlacementTarget as ListBoxItem;

                        if (item != null)
                        {
                            ContentPresenter myContentPresenter = FindingObjects.FindVisualChild<ContentPresenter>(item);
                            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;

                            TextBlock testID = (TextBlock)myDataTemplate.FindName("testID", myContentPresenter);

                            connection.Open();

                            //SqliteCommand deleteQuestion = new SqliteCommand
                            //($"DELETE FROM Test{testNumber} where ID = {questionID.Text}", connection);

                            if(MessageBox.Show("Вы уверены, что хотите удалить этот тест?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {
                                SqliteCommand deleteTest = new SqliteCommand
                                ($"DELETE FROM Tests where ID = {testID.Text}", connection);

                                if (deleteTest.ExecuteNonQuery() != 0)
                                {
                                    MessageBox.Show("Удаление прошло успешно");

                                    LoadTests();
                                }
                                else MessageBox.Show("Ошибка при удалении", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            connection.Close();

                        }
                        else MessageBox.Show("Не было найдено такого элемента");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла непредвиденная ошикбка, попробуйте перезапустить приложение"+ex.Message);
            }
        }

        private void LoadAdmins(object sender, MouseButtonEventArgs e)
        {
            connection.ConnectionString = "Data Source=" + pathToDB;

            connection.Open();

            SqliteCommand command = new SqliteCommand
                ($"SELECT * FROM Administrators", connection);

            List<Administrator> admins = new List<Administrator>();

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var id = Convert.ToInt16(reader.GetValue(0).ToString());
                        var login = reader.GetValue(1).ToString();

                        admins.Add(new Administrator(id, Cryptography.Decrypt(login)));
                    }
                }
            }
            adminsList.DataContext = admins;

            adminsList.Visibility = Visibility.Visible;
            questionsList.Visibility = Visibility.Hidden;

            connection.Close();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<Question> searchQuestions = new List<Question>();

            if (questions.Count != 0)
            {
                searchQuestions.AddRange(questions.Where(x => x.Name.ToLower().Contains(searchText.Text.Trim().ToLower()) == true));
                questionsList.DataContext = searchQuestions;
            }

            else MessageBox.Show("Ошибка поиска, сначала загрузите вопросы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Instruction_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock selectedText = (TextBlock)e.Source;
            selectedText.Foreground = Brushes.DarkSlateGray;
            selectedText.TextDecorations = TextDecorations.Underline;
            Cursor = Cursors.Hand;
        }

        private void Instruction_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock selectedText = (TextBlock)e.Source;
            selectedText.Foreground = Brushes.Black;
            selectedText.TextDecorations = null;
            Cursor = Cursors.Arrow;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AddTest(object sender, RoutedEventArgs e)
        {
            AddEditTest addTestWindow = new AddEditTest();
            addTestWindow.Show();
        }

        private void EditTest(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem mi = sender as MenuItem;
                if (mi != null)
                {
                    ContextMenu cm = mi.CommandParameter as ContextMenu;
                    if (cm != null)
                    {
                        ListBoxItem item = cm.PlacementTarget as ListBoxItem;

                        if (item != null)
                        {
                            ContentPresenter myContentPresenter = FindingObjects.FindVisualChild<ContentPresenter>(item);
                            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;

                            TextBlock testID = (TextBlock)myDataTemplate.FindName("testID", myContentPresenter);
                            TextBlock testName = (TextBlock)myDataTemplate.FindName("testName", myContentPresenter);
                            int testResponseNumber = tests.Where(x => x.Name == testName.Text).First().ResponseNumber;

                            AddEditTest editTestWindow = new AddEditTest(testID.Text, testName.Text, testResponseNumber);
                            editTestWindow.Show();

                        }
                        else MessageBox.Show("Не было найдено такого элемента");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла непредвиденная ошикбка, попробуйте перезапустить приложение");
            }

        }

    }
}
