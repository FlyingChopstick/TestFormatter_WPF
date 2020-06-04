namespace FormatterLib
{
    /// <summary>
    /// Output formatting
    /// </summary>
    public struct TestFormat
    {
        public const string TopicFormat = "I: {0}; mt=0,1";
        public const string QuestionFormat = "S: {0}:";
        public const string CAnsFormat = "+: {0}";
        public const string WAnsFormat = "-: {0}";
    }

    /// <summary>
    /// Loca formatting
    /// </summary>
    public struct LocaFormat
    {
        public const string Language = "Language|";
        public const string L_fileName = "File name|";
        public const string L_topic = "Topic|";
        public const string L_fileExistsY = "File exists: Yes|";
        public const string L_fileExistsN = "File exists: No|";
        public const string L_statistics = "Since last launch:  #1: {0}   #2: {1}   #3: {2}   #4: {3}|";
        public const string B_openDir = "Open directory|";
        public const string B_openFile = "Open file|";
        public const string B_generate = "Generate|";
        public const string B_generateWait = "Writing...|";
        public const string GB_question = "Question #{0}|";
    }
}
