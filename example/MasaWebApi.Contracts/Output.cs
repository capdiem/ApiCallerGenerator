﻿namespace MasaWebApi.Contracts;

public class Output<T>
{
    public bool Success { get; set; }

    public T Data { get; set; }

    public string Message { get; set; }
}
