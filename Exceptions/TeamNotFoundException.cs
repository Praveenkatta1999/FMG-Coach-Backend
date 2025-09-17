using System;

public class TeamNotFoundException : Exception
{
    public TeamNotFoundException()
        : base($"Team not found.")
    {
    }
}
