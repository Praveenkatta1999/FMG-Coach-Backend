using System;

public class NotFoundException : Exception
{
    public NotFoundException(string param = null)
        : base($"{param} not found.")
    {
    }
}
