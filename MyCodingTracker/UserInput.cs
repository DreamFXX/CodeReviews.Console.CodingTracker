using Spectre.Console;
using System.Globalization;

namespace MyCodingTracker
{
    internal class UserInput
    {
        private string? Date { get; set; } = string.Empty;
        private string? StartTime { get; set; } = string.Empty;
        private string? EndTime { get; set; } = string.Empty;

        public string GetDate()
        {
            this.Date = AnsiConsole.Ask<string>(
                ("[yellow]Please, enter the date of your session. Specify date in this exact format![/]\n-[green](dd-MM-yy)[/] -> "));

            return this.Date;
        }

        public string GetStartTime()
        {
            this.StartTime = AnsiConsole.Ask<string>
                ("[yellow]Enter the time your session started. Specify time in this format![/]\n-[green](hh:mm)[/] -> ");    

            return this.StartTime;
        }

        public string GetEndTime()
        {
            this.EndTime = AnsiConsole.Ask<string>
                ("[yellow]Enter the time your session ended. Specify time in this format![/]\n-[green](hh:mm)[/] -> ");

            return this.EndTime;
        }

        public string GetDuration()
        {
            DateTime parsedStartTime = DateTime.ParseExact(StartTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
            DateTime parsedEndTime = DateTime.ParseExact(EndTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);

            TimeSpan duration = parsedEndTime.Subtract(parsedStartTime);
            if (duration < TimeSpan.Zero)
            {
                duration += TimeSpan.FromDays(1);
            }
            return duration.ToString();
        }
    }
}
