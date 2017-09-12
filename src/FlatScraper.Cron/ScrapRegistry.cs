using FluentScheduler;

namespace FlatScraper.Cron
{
    public class ScrapRegistry : Registry
    {
        public ScrapRegistry()
        {
            Schedule<ScrapJob>().WithName("Scrap").ToRunNow().AndEvery(2).Minutes();
        }
    }
}
