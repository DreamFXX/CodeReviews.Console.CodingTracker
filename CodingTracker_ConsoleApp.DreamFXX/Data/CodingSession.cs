class CodingSession
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get {return ActiveStat ? DateTime.Now : _endTime; } set { _endTime = value; } }
    public DateTime _endTime { get; set; }

    public string Duration { get { return (EndTime - StartTime).ToString(@"hh\:mm\:ss"); } set { } }
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

    

