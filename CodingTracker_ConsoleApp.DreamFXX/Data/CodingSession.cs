class CodingSession
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime _endTime {  get; set; }

    public string? Duration { get; set; }
    public string? Name { get; set; }
    public bool ActiveStat { get; set; }

    public CodingSession()
    {
        Name = "Coding Session";
        ActiveStat = true;
    }
    public CodingSession(bool isActive)
    {
        Name = "Coding Session";
        ActiveStat = isActive;
    }
}

    

