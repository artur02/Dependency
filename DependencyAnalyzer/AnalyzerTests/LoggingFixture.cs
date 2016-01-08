namespace AnalyzerTests
{
    public class LoggingFixture
    {
        public LoggingFixture()
        {
            LoggingExtensions.Logging.Log.InitializeWith<LoggingExtensions.log4net.Log4NetLog>();
        }
    }
}