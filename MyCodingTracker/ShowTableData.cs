using Spectre.Console;

namespace MyCodingTracker
{
    internal class ShowTableData
    {
        readonly DatabaseManager _dbManager = new();

        public void Show()
        {
            SelectDataToShow();
        }

        public static void DisplayTable<T>(string title, IEnumerable<T> data)
        {
            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.DarkViolet)
                .Title($"[yellow bold]Coding sessions[/]");

            // Add columns dynamically based on properties of the type
            var properties = typeof().GetProperties();
            foreach (var session in properties)
            {
                table.AddColumn(session.Name);
                table.AddColumn(session.Name);
                table.AddColumn(session.Name);
                table.AddColumn(session.Name);
                table.AddColumn(session.Name);
            }

            // Add rows
            foreach (var item in data)
            {
                table.AddRow(properties.Select(p => p.GetValue(item)?.ToString() ?? string.Empty).ToArray());
            }

            AnsiConsole.Write(table);
        }
    }
}
