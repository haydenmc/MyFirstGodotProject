namespace MyFirstGodotProject;

public partial class Menu : Control
{
    private Button _hostButton;
    private LineEdit _connectIpEntry;
    private LineEdit _connectPortEntry;
    private Button _connectButton;

    public override void _Ready()
    {
        _hostButton = GetNode<Button>("CenterContainer/HFlowContainer/HostButton");
        _connectIpEntry = GetNode<LineEdit>(
            "CenterContainer/HFlowContainer/VFlowContainer/ConnectIpEntry");
        _connectPortEntry = GetNode<LineEdit>(
            "CenterContainer/HFlowContainer/VFlowContainer/ConnectPortEntry");
        _connectButton = GetNode<Button>(
            "CenterContainer/HFlowContainer/VFlowContainer/ConnectButton");

        _hostButton.Pressed += OnHostPressed;
        _connectButton.Pressed += OnConnectPressed;
    }

    private void OnHostPressed()
    {
        Global.ClientMode = Global.ClientModeKind.Server;
        Global.ConnectHostname = _connectIpEntry.Text;
        Global.ConnectPort = _connectPortEntry.Text;
        GetTree().ChangeSceneToFile("res://World.tscn");
    }

    private void OnConnectPressed()
    {
        Global.ClientMode = Global.ClientModeKind.Client;
        Global.ConnectHostname = _connectIpEntry.Text;
        Global.ConnectPort = _connectPortEntry.Text;
        GetTree().ChangeSceneToFile("res://World.tscn");
    }
}
