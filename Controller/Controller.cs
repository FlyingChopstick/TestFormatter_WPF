using FormatterLib;
using LocalizationLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ControllerLib
{
    /// <summary>
    /// Question number
    /// </summary>
    public enum QNum
    {
        First = 0,
        Second = 1,
        Third = 2,
        Fourth = 3
    }

    /// <summary>
    /// Question data holder
    /// </summary>
    public struct Question
    {
        public string FileName;
        public string Topic;
        public string QuestionText;
        public string ans1, ans2, ans3, ans4;
        public QNum Correct;
    }

    public static class Controller
    {
        #region Paths, prefixes, extensions
        /// <summary>
        /// Topic output folder
        /// </summary>
        public const string outputFolder = @".\Topics\";//@".\Topics\Output\";
        /// <summary>
        /// Topic output prefix
        /// </summary>
        private const string outputPrefix = "topic_";

        /// <summary>
        /// Folder containing loca files
        /// </summary>
        public const string locaFolder = @".\Localization\";
        /// <summary>
        /// Localization extension
        /// </summary>
        private const string locaExtension = ".loca";

        /// <summary>
        /// Path to active file
        /// </summary>
        public static string ActiveFile { get; private set; } = "FILE NOT SET";
        #endregion

        #region Statistics
        /// <summary>
        /// Individual question tracking
        /// </summary>
        public static Dictionary<string, int[]> TopicStats { get; private set; } = new Dictionary<string, int[]>();

        public static string GetFormattedStats(string topic)
        {
            if (TopicStats.ContainsKey(topic))
            {
                return string.Format(
                        CLoca.L_statistics,
                        TopicStats[topic][1],
                        TopicStats[topic][2],
                        TopicStats[topic][3],
                        TopicStats[topic][4]
                        );
            }
            else
            {
                return string.Format(
                        CLoca.L_statistics,
                        0,
                        0,
                        0,
                        0
                        );
            }
        }
        public static string GetQuestionNumber(string topic)
        {
            if (TopicStats.ContainsKey(topic))
            {
                return string.Format(CLoca.GB_question, TopicStats[topic][0]);// + 1);
            }
            else
            {
                return string.Format(CLoca.GB_question, 1);
            }
        }
        #endregion

        #region Language
        /// <summary>
        /// Dictionary of languages and their StringHolders
        /// </summary>
        private static Dictionary<string, StringHolder> localizations = new Dictionary<string, StringHolder>();
        /// <summary>
        /// Current language
        /// </summary>
        public static string CLang { get; private set; } = "English";
        /// <summary>
        /// Current localization StringHolder
        /// </summary>
        public static StringHolder CLoca { get; private set; } = new StringHolder();

        public static List<string> Languages { get; private set; } = new List<string>();

        /// <summary>
        /// Loads all localization files from the folder
        /// </summary>
        public static void LoadLocalizations()
        {
            string[] locas = Directory.GetFiles(locaFolder).Where(file => file.Contains(".loca")).ToArray();
            foreach (string file in locas)
            {
                try
                {
                    StringHolder newLoca = new StringHolder(File.ReadAllLines(file));

                    localizations[newLoca.Language] = newLoca;
                }
                catch (ArgumentException aex)
                {
                    //do not add the file to the list of localizations
                    throw new ArgumentException($"Could not load {file}: {aex.Message}");
                }
            }

            Languages = localizations.Keys.ToList();
        }
        /// <summary>
        /// Sets the active language and localization StringHolder
        /// </summary>
        /// <param name="lang">Language</param>
        public static void SetLocalization(string lang)
        {
            if (localizations.ContainsKey(lang))
            {
                CLang = lang;
                CLoca = localizations[lang];
            }
            else
            {
                throw new ArgumentException("Language not recognized");
            }
        }
        #endregion

        #region File actions
        /// <summary>
        /// Formats the new Active file using fileName
        /// </summary>
        /// <param name="fileName">Topic name</param>
        public static void SetActiveFile(string fileName)
        {
            ActiveFile = $"{outputFolder}{outputPrefix}{fileName}.txt";
        }
        /// <summary>
        /// Opens Active File
        /// </summary>
        public static void OpenActiveFile()
        {
            System.Diagnostics.Process.Start(ActiveFile);
        }

        /// <summary>
        /// Counts the questions in file and updates the tracker
        /// </summary>
        /// <param name="topic">Topic to update</param>
        /// <returns><see langword="true"/> if file exists, <see langword="false"/> if not</returns>
        public static void QuestionsPerTopic(string topic)
        {
            if (TopicStats.ContainsKey(topic))
            {
                //count question in file, assuming each is 7 lines long
                int len = File.ReadAllLines(ActiveFile).Length;
                int qCount = len / 7 + 1;
                if (qCount == 0)
                {
                    TopicStats[topic][0] = 1;
                }
                else
                {
                    TopicStats[topic][0] = qCount;
                }
            }
            else
            {
                TopicStats[topic] = new int[] { 1, 0, 0, 0, 0 };
                //TopicStats[topic][0] = 1;
            }
        }
        #endregion



        /// <summary>
        /// Sets up output directory, loads localizations
        /// </summary>
        public static void Setup()
        {
            Directory.CreateDirectory(outputFolder);
            Directory.CreateDirectory(locaFolder);
            localizations["English"] = new StringHolder();

            LoadLocalizations();


            if (CultureInfo.InstalledUICulture.Name == "ru-RU")
            {
                SetLocalization("Русский");
            }
            else
            {
                SetLocalization("English");
            }
            //CLoca = localizations["Русский"];
            //localizations["English"] = localizations["Русский"];
        }
        /// <summary>
        /// Formats the question using the format in StringHolder
        /// </summary>
        /// <param name="q"></param>
        public static async Task WriteAsync(Question q)
        {
            //string fileName = q.FileName;
            string topic = string.Format(TestFormat.TopicFormat, q.Topic);
            string question = string.Format(TestFormat.QuestionFormat, q.QuestionText);
            string ans1, ans2, ans3, ans4;

            if (!TopicStats.ContainsKey(topic))
            {
                TopicStats[topic] = new int[] { 0, 0, 0, 0, 0 };
            }

            switch (q.Correct)
            {
                case QNum.First:
                    {
                        //ans1 correct
                        TopicStats[q.Topic][1]++;

                        ans1 = string.Format(TestFormat.CAnsFormat, q.ans1);
                        ans2 = string.Format(TestFormat.WAnsFormat, q.ans2);
                        ans3 = string.Format(TestFormat.WAnsFormat, q.ans3);
                        ans4 = string.Format(TestFormat.WAnsFormat, q.ans4);
                        break;
                    }
                case QNum.Second:
                    {
                        //ans2 correct
                        TopicStats[q.Topic][2]++;

                        ans1 = string.Format(TestFormat.WAnsFormat, q.ans1);
                        ans2 = string.Format(TestFormat.CAnsFormat, q.ans2);
                        ans3 = string.Format(TestFormat.WAnsFormat, q.ans3);
                        ans4 = string.Format(TestFormat.WAnsFormat, q.ans4);
                        break;
                    }
                case QNum.Third:
                    {
                        //ans3 correct
                        TopicStats[q.Topic][3]++;

                        ans1 = string.Format(TestFormat.WAnsFormat, q.ans1);
                        ans2 = string.Format(TestFormat.WAnsFormat, q.ans2);
                        ans3 = string.Format(TestFormat.CAnsFormat, q.ans3);
                        ans4 = string.Format(TestFormat.WAnsFormat, q.ans4);
                        break;
                    }
                case QNum.Fourth:
                    {
                        //ans4 correct
                        TopicStats[q.Topic][4]++;

                        ans1 = string.Format(TestFormat.WAnsFormat, q.ans1);
                        ans2 = string.Format(TestFormat.WAnsFormat, q.ans2);
                        ans3 = string.Format(TestFormat.WAnsFormat, q.ans3);
                        ans4 = string.Format(TestFormat.CAnsFormat, q.ans4);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Could not determine the correct answer");
                    }
            }

            //string[] newQuestion = { theme, question, ans1, ans2, ans3, ans4 };

            //write
            await Task.Run(() => File.AppendAllLines(ActiveFile, new string[] { topic, question, ans1, ans2, ans3, ans4 }));


            TopicStats[q.Topic][0]++;

        }

        /// <summary>
        /// Formats the question using the format in StringHolder
        /// </summary>
        /// <param name="q"></param>
        public static void WriteSync(Question q)
        {
            //string fileName = q.FileName;
            string topic = string.Format(TestFormat.TopicFormat, q.Topic);
            string question = string.Format(TestFormat.QuestionFormat, q.QuestionText);
            string ans1, ans2, ans3, ans4;

            if (!TopicStats.ContainsKey(topic))
            {
                TopicStats[topic] = new int[] { 0, 0, 0, 0, 0 };
            }

            switch (q.Correct)
            {
                case QNum.First:
                    {
                        //ans1 correct
                        TopicStats[q.Topic][1]++;

                        ans1 = string.Format(TestFormat.CAnsFormat, q.ans1);
                        ans2 = string.Format(TestFormat.WAnsFormat, q.ans2);
                        ans3 = string.Format(TestFormat.WAnsFormat, q.ans3);
                        ans4 = string.Format(TestFormat.WAnsFormat, q.ans4);
                        break;
                    }
                case QNum.Second:
                    {
                        //ans2 correct
                        TopicStats[q.Topic][2]++;

                        ans1 = string.Format(TestFormat.WAnsFormat, q.ans1);
                        ans2 = string.Format(TestFormat.CAnsFormat, q.ans2);
                        ans3 = string.Format(TestFormat.WAnsFormat, q.ans3);
                        ans4 = string.Format(TestFormat.WAnsFormat, q.ans4);
                        break;
                    }
                case QNum.Third:
                    {
                        //ans3 correct
                        TopicStats[q.Topic][3]++;

                        ans1 = string.Format(TestFormat.WAnsFormat, q.ans1);
                        ans2 = string.Format(TestFormat.WAnsFormat, q.ans2);
                        ans3 = string.Format(TestFormat.CAnsFormat, q.ans3);
                        ans4 = string.Format(TestFormat.WAnsFormat, q.ans4);
                        break;
                    }
                case QNum.Fourth:
                    {
                        //ans4 correct
                        TopicStats[q.Topic][4]++;

                        ans1 = string.Format(TestFormat.WAnsFormat, q.ans1);
                        ans2 = string.Format(TestFormat.WAnsFormat, q.ans2);
                        ans3 = string.Format(TestFormat.WAnsFormat, q.ans3);
                        ans4 = string.Format(TestFormat.CAnsFormat, q.ans4);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Could not determine the correct answer");
                    }
            }

            //string[] newQuestion = { theme, question, ans1, ans2, ans3, ans4 };

            //write
            File.AppendAllLines(ActiveFile, new string[] { topic, question, ans1, ans2, ans3, ans4 });

            TopicStats[q.Topic][0]++;
        }
    }
}
