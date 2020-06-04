using FormatterLib;
using System;

namespace LocalizationLib
{
    /// <summary>
    /// Pack of localization strings
    /// </summary>
    public struct LocalizationStrings
    {
        public string Language;
        public string L_fileName;
        public string L_topic;
        public string L_fileExistsY;
        public string L_fileExistsN;
        public string L_statistics;
        public string B_openDir;
        public string B_openFile;
        public string B_generate;
        public string B_generateWait;
        public string GB_question;
    }


    /// <summary>
    /// Contains all localization strings
    /// </summary>
    public class StringHolder
    {
        /// <summary>
        /// The required amount of localization string
        /// </summary>
        public const int FieldCount = 11;

        /// <summary>
        /// Language name
        /// </summary>
        public string Language { get; private set; } = "English";

        #region Constructors
        /// <summary>
        /// Loads default localization (English)
        /// </summary>
        public StringHolder()
        {

        }

        /// <summary>
        /// Loads custom localization using an array of strings
        /// </summary>
        /// <param name="localizationStrings">Custom localization strings</param>
        public StringHolder(LocalizationStrings localizationStrings)
        {
            L_fileName = localizationStrings.L_fileName;
            L_topic = localizationStrings.L_topic;
            L_fileExistsY = localizationStrings.L_fileExistsY;
            L_fileExistsN = localizationStrings.L_fileExistsN;
            L_statistics = localizationStrings.L_statistics;

            B_openFile = localizationStrings.B_openFile;
            B_generate = localizationStrings.B_generate;

            GB_question = localizationStrings.GB_question;
        }
        /// <summary>
        /// Loads localization using an array
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the loca doesn't have all required fields</exception>
        /// <param name="localizationStrings"></param>
        public StringHolder(string[] localizationStrings)
        {
            int counter = 0;

            foreach (string line in localizationStrings)
            {
                if (!line.Contains("<!--") && line.Length != 0)
                {
                    if (line.Contains(LocaFormat.Language))
                    {
                        Language = line.Remove(0, LocaFormat.Language.Length);
                        counter++;
                        continue;
                    }
                    if (line.Contains(LocaFormat.L_fileName))
                    {
                        L_fileName = line.Remove(0, LocaFormat.L_fileName.Length);
                        counter++;
                        continue;
                    }
                    if (line.Contains(LocaFormat.L_topic))
                    {
                        L_topic = line.Remove(0, LocaFormat.L_topic.Length);
                        counter++;
                        continue;
                    }
                    if (line.Contains(LocaFormat.L_fileExistsY))
                    {
                        L_fileExistsY = line.Remove(0, LocaFormat.L_fileExistsY.Length);
                        counter++;
                        continue;
                    }
                    if (line.Contains(LocaFormat.L_fileExistsN))
                    {
                        L_fileExistsN = line.Remove(0, LocaFormat.L_fileExistsN.Length);
                        counter++;
                        continue;
                    }
                    if (line.Contains(LocaFormat.L_statistics))
                    {
                        L_statistics = line.Remove(0, LocaFormat.L_statistics.Length);
                        counter++;
                        continue;
                    }
                    if (line.Contains(LocaFormat.B_openDir))
                    {
                        B_openDir = line.Remove(0, LocaFormat.B_openDir.Length);
                        counter++;
                        continue;
                    }
                    if (line.Contains(LocaFormat.B_openFile))
                    {
                        B_openFile = line.Remove(0, LocaFormat.B_openFile.Length);
                        counter++;
                        continue;
                    }
                    if (line.Contains(LocaFormat.B_generate))
                    {
                        B_generate = line.Remove(0, LocaFormat.B_generate.Length);
                        counter++;
                        continue;
                    }
                    if (line.Contains(LocaFormat.B_generateWait))
                    {
                        B_generateWait = line.Remove(0, LocaFormat.B_generateWait.Length);
                        counter++;
                        continue;
                    }
                    if (line.Contains(LocaFormat.GB_question))
                    {
                        GB_question = line.Remove(0, LocaFormat.GB_question.Length);
                        counter++;
                        continue;
                    }
                }
            }
            if (counter < FieldCount)
            {
                throw new ArgumentException("File does not have all required fields");
            }
        }
        #endregion

        #region Labels
        /// <summary>
        /// Filename label
        /// </summary>
        public string L_fileName { get; private set; } = "File name";
        /// <summary>
        /// Topic label
        /// </summary>
        public string L_topic { get; private set; } = "Topic";
        /// <summary>
        /// File exists label
        /// </summary>
        public string L_fileExistsY { get; private set; } = "File exists: Yes";
        /// <summary>
        /// File does not exist label
        /// </summary>
        public string L_fileExistsN { get; private set; } = "File exists: No";
        /// <summary>
        /// Question stats label
        /// </summary>
        public string L_statistics { get; private set; } = "Since last launch:  #1: {0}   #2: {1}   #3: {2}   #4: {3}";
        #endregion

        #region Buttons
        /// <summary>
        /// Open output directory button
        /// </summary>
        public string B_openDir { get; private set; } = "Open directory";
        /// <summary>
        /// Open file button
        /// </summary>
        public string B_openFile { get; private set; } = "Open file";
        /// <summary>
        /// Generate button
        /// </summary>
        public string B_generate { get; private set; } = "Generate";
        /// <summary>
        /// Wait for write
        /// </summary>
        public string B_generateWait { get; private set; } = "Writing...";
        #endregion

        #region GroupBoxes
        /// <summary>
        /// Question number group box
        /// </summary>
        public string GB_question { get; private set; } = "Question #{0}";
        #endregion
    }
}
