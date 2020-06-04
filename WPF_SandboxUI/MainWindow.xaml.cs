using ControllerLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPF_SandboxUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                Controller.Setup();
            }
            catch (ArgumentException aex)
            {
                Console.WriteLine($"{aex.Message}");
            }
            languagesList = Controller.Languages;

            InitializeComponent();

            if (languagesList.Count > 1)
            {
                cb_languages.ItemsSource = languagesList;
                cb_languages.SelectedItem = Controller.CLoca.Language;
                cb_languages.SelectionChanged += Cb_languages_SelectionChanged;
            }
            else
            {
                cb_languages.IsEnabled = false;
                cb_languages.Visibility = Visibility.Collapsed;
            }


            ReloadNames();

            tb_FileName.Background = Brushes.PaleGreen;
            tb_TopicName.IsEnabled = false;
            LockFields(true);
            LockFileOpen(true);

            MainGrid.MouseEnter += MainGrid_MouseEnter;

            tb_FileName.TextChanged += Tb_FileName_TextChanged;
            tb_TopicName.TextChanged += Tb_TopicName_TextChanged;
            tb_Question.TextChanged += Tb_Question_TextChanged;
            tb_ans1.TextChanged += Tb_ans1_TextChanged;
            tb_ans2.TextChanged += Tb_ans2_TextChanged;
            tb_ans3.TextChanged += Tb_ans3_TextChanged;
            tb_ans4.TextChanged += Tb_ans4_TextChanged;


            b_FileOpen.Click += B_FileOpen_Click;
            b_DirOpen.Click += B_DirOpen_Click;
            b_Generate.Click += B_Generate_Click;
        }

        private void Cb_languages_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Controller.SetLocalization(cb_languages.SelectedItem.ToString());
            ReloadNames();
        }

        public List<string> languagesList { get; private set; } = new List<string>();


        private void MainGrid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (File.Exists(Controller.ActiveFile))
            {
                Update();
            }
            else
            {
                LockFileOpen(true);
            }
        }


        #region TB Events
        /// <summary>
        /// FileName Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_FileName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            tb_TopicName.Text = string.Empty;
            ClearFields();

            string fileName = tb_FileName.Text;
            if (fileName.Length != 0)
            {
                tb_FileName.Background = Brushes.Transparent;
                tb_TopicName.Background = Brushes.PaleGreen;
                tb_TopicName.IsEnabled = true;
                //FieldLock(false);
                Controller.SetActiveFile(fileName);
                CheckIfExists();
            }
            else
            {
                tb_FileName.Background = Brushes.PaleGreen;
                tb_TopicName.Background = Brushes.Transparent;
                tb_TopicName.IsEnabled = false;

                LockFileOpen(true);
                LockFields(true);
            }
        }
        /// <summary>
        /// Topic Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_TopicName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (tb_TopicName.Text.Length != 0)
            {
                tb_TopicName.Background = Brushes.Transparent;
                LockFields(false);

                Update();
            }
            else
            {
                tb_TopicName.Background = Brushes.PaleGreen;
                LockFields(true);
            }
        }
        /// <summary>
        /// Question Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_Question_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            tb_Question.Background = Brushes.Transparent;
        }
        /// <summary>
        /// ans1 Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_ans1_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            tb_ans1.Background = Brushes.Transparent;
        }
        /// <summary>
        /// ans1 Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_ans2_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            tb_ans2.Background = Brushes.Transparent;
        }
        /// <summary>
        /// ans2 Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_ans3_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            tb_ans3.Background = Brushes.Transparent;
        }
        /// <summary>
        /// ans3 Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_ans4_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            tb_ans4.Background = Brushes.Transparent;
        }
        #endregion

        #region B Events

        private void B_DirOpen_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Controller.outputFolder);
        }
        private void B_FileOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Controller.OpenActiveFile();
            }
            catch (Exception)
            {
                MessageBox.Show("Could not open the file. Check that it is present", "File error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void B_Generate_Click(object sender, RoutedEventArgs e)
        {
            if (AllFilled())
            {
                try
                {
                    LockFields(true);
                    b_Generate.Content = Controller.CLoca.B_generateWait;
                    b_Generate.IsEnabled = false;
                    await GenerateAsync();

                    ClearFields();
                    Update();
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not write the question", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    b_Generate.Content = Controller.CLoca.B_generate;
                    b_Generate.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Fill all required fields", "Write error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        #endregion


        private void ReloadNames()
        {
            l_FileName.Content = Controller.CLoca.L_fileName;
            l_TopicName.Content = Controller.CLoca.L_topic;
            l_FileExists.Content = Controller.CLoca.L_fileExistsN;

            b_DirOpen.Content = Controller.CLoca.B_openDir;
            b_FileOpen.Content = Controller.CLoca.B_openFile;
            b_Generate.Content = Controller.CLoca.B_generate;


            string topic = tb_TopicName.Text;
            if (topic.Length == 0)
            {
                gb_Question.Header = string.Format(Controller.CLoca.GB_question, "--");
                l_Stats.Visibility = Visibility.Collapsed;
            }
            else
            {
                gb_Question.Header = Controller.GetQuestionNumber(topic);
                l_Stats.Content = Controller.GetFormattedStats(topic);
            }

        }

        private void Update()
        {
            string topic = tb_TopicName.Text;
            Controller.QuestionsPerTopic(topic);
            gb_Question.Header = Controller.GetQuestionNumber(topic);
            l_Stats.Content = Controller.GetFormattedStats(topic);

            CheckIfExists();
        }
        /// <summary>
        /// Locks or unlocks input in Question
        /// </summary>
        /// <param name="locked">Should the fields be locked</param>
        private void LockFields(bool locked)
        {
            if (locked)
            {
                rb_ans1.IsChecked = false;
                rb_ans2.IsChecked = false;
                rb_ans3.IsChecked = false;
                rb_ans4.IsChecked = false;
                gb_Question.Header = string.Format(Controller.CLoca.GB_question, "--");
                gb_Question.IsEnabled = false;
                b_Generate.IsEnabled = false;
                l_Stats.Visibility = Visibility.Hidden;
                //b_FileOpen.Visibility = Visibility.Collapsed;
            }
            else
            {
                rb_ans1.IsChecked = true;
                gb_Question.IsEnabled = true;
                b_Generate.IsEnabled = true;
                l_Stats.Visibility = Visibility.Visible;
                //b_FileOpen.Visibility = Visibility.Visible;
            }
        }
        private void LockFileOpen(bool locked)
        {
            if (locked)
            {
                l_FileExists.Content = Controller.CLoca.L_fileExistsN;
                b_FileOpen.Visibility = Visibility.Collapsed;
            }
            else
            {
                l_FileExists.Content = Controller.CLoca.L_fileExistsY;
                b_FileOpen.IsEnabled = true;
                b_FileOpen.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Clears Topic and the Question GB elements
        /// </summary>
        private void ClearFields()
        {
            tb_Question.Text = string.Empty;
            tb_ans1.Text = string.Empty;
            tb_ans2.Text = string.Empty;
            tb_ans3.Text = string.Empty;
            tb_ans4.Text = string.Empty;
        }
        /// <summary>
        /// Checks if all required fields are filled
        /// </summary>
        /// <returns><see langword="true"/> if all required fields are filled <see langword="false"/> otherwise</returns>
        private bool AllFilled()
        {
            if (tb_Question.Text.Length == 0)
            {
                tb_Question.Background = Brushes.PaleVioletRed;
                return false;
            }
            if (tb_ans1.Text.Length == 0)
            {
                tb_ans1.Background = Brushes.PaleVioletRed;
                return false;
            }
            if (tb_ans2.Text.Length == 0)
            {
                tb_ans2.Background = Brushes.PaleVioletRed;
                return false;
            }
            if (tb_ans3.Text.Length == 0)
            {
                tb_ans3.Background = Brushes.PaleVioletRed;
                return false;
            }
            if (tb_ans4.Text.Length == 0)
            {
                tb_ans4.Background = Brushes.PaleVioletRed;
                return false;
            }

            return true;
        }
        private bool CheckIfExists()
        {
            if (File.Exists(Controller.ActiveFile))
            {
                LockFileOpen(false);
                return true;
            }
            else
            {
                LockFileOpen(true);
                return false;
            }
        }
        private async Task GenerateAsync()
        {
            QNum correct;
            if (rb_ans1.IsChecked == true)
            {
                correct = QNum.First;
            }
            else
            {
                if (rb_ans2.IsChecked == true)
                {
                    correct = QNum.Second;
                }
                else
                {
                    if (rb_ans3.IsChecked == true)
                    {
                        correct = QNum.Third;
                    }
                    else
                    {
                        correct = QNum.Fourth;
                    }
                }
            }

            Question q = new Question
            {
                FileName = tb_FileName.Text,
                Topic = tb_TopicName.Text,
                QuestionText = tb_Question.Text,
                ans1 = tb_ans1.Text,
                ans2 = tb_ans2.Text,
                ans3 = tb_ans3.Text,
                ans4 = tb_ans4.Text,
                Correct = correct
            };

            await Task.Run(() => Controller.WriteSync(q));
        }

    }
}
