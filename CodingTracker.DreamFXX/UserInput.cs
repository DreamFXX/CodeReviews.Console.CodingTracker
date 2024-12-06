using Spectre.Console;
using System.Globalization;

namespace MyCodingTracker
{
    internal class UserInput
    {
        Validation _dateTimeValidation = new();
        private string? Date { get; set; } = string.Empty;
        private string? StartTime { get; set; } = string.Empty;
        private string? EndTime { get; set; } = string.Empty;

        public string GetDate()
        {
            Date = AnsiConsole.Ask<string>(
                ("[yellow]Please, enter the date of your session. Specify date in this exact format![/]\n-[green](dd-MM-yy)[/] -> "));
            while (!_dateTimeValidation.IsValidDate(Date))
            {
                AnsiConsole.MarkupLine("Invalid date! Expected format (mm-dd-yy)");
                Date = Console.ReadLine();
            }
            return Date;
        }

        public string GetStartTime()
        {
            StartTime = AnsiConsole.Ask<string>
                ("[yellow]Enter the time your session started. Specify time in this format![/]\n-[green](hh:mm)[/] -> ");

            while (!_dateTimeValidation.IsValidStartTime(StartTime))
            {
                AnsiConsole.MarkupLine("[red]Invalid time! Time must be in this format - (hh:mm):[/] ");
                StartTime = Console.ReadLine();
            }

            return StartTime;
        }

        public string GetEndTime()
        {
            EndTime = AnsiConsole.Ask<string>
                ("[yellow]Enter the time your session ended. Specify time in this format![/]\n-[green](hh:mm)[/] -> ");
            while (_dateTimeValidation.IsValidStartTime(EndTime))
            {
                AnsiConsole.MarkupLine("[red]Invalid time! Time must be in this format - (hh:mm):[/] ");
                EndTime = Console.ReadLine();
            }

            return EndTime;
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
