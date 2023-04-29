global using Godot;
global using System;

public partial class Global : Node
{
    public enum ClientModeKind
    {
        Unknown,
        Server,
        Client,
    }
    public const string DEFAULT_HOSTNAME = "127.0.0.1";
    public const int DEFAULT_PORT = 7890;
    public static ClientModeKind ClientMode { get; set; } = ClientModeKind.Unknown;
    public static string ConnectHostname { get; set; } = $"{DEFAULT_HOSTNAME}";
    public static string ConnectPort { get; set; } = $"{DEFAULT_PORT}";
}
