using Godot;
using System;

public partial class World : Node
{
	private PackedScene _playerScene;
	private PlayerController _playerController;
	private Camera3D _serverCamera;
	private ENetMultiplayerPeer _multiplayerPeer;

	public override void _Ready()
	{
		_playerScene = GD.Load<PackedScene>("res://Player.tscn");
		_playerController = GetNode<PlayerController>("PlayerController");
		_serverCamera = GetNode<Camera3D>("ServerCamera");
		_serverCamera.ClearCurrent(false);
		_multiplayerPeer = new ENetMultiplayerPeer();
		switch (Global.ClientMode)
		{
		case Global.ClientModeKind.Server:
			StartServer();
			break;
		case Global.ClientModeKind.Client:
			StartClient();
			break;
		case Global.ClientModeKind.Unknown:
		default:
			throw new ApplicationException(
				"Attempt to load world without Client or Server mode set");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("quit"))
		{
			GetTree().Quit();
		}
	}

	private int GetPort()
	{
		int port = Global.DEFAULT_PORT;
		try
		{
			port = int.Parse(Global.ConnectPort);
		}
		catch (Exception)
		{
			GD.PrintErr($"Could not parse port value '{Global.ConnectPort}', defaulting to {port}");
		}
		return port;
	}

	private void StartServer()
	{
		int port = GetPort();
		Error err = _multiplayerPeer.CreateServer(port);
		if (err != Error.Ok)
		{
			throw new ApplicationException($"Could not start server on port {port}: {err}");
		}
		_multiplayerPeer.PeerConnected += OnServerPeerConnected;
		_multiplayerPeer.PeerDisconnected += OnServerPeerDisconnected;
		Multiplayer.MultiplayerPeer = _multiplayerPeer;
		GD.Print($"✅ Started server on port {port}!");
		_serverCamera.Current = true;
	}

	private void StartClient()
	{
		int port = GetPort();
		Error err = _multiplayerPeer.CreateClient(Global.ConnectHostname, port);
		if (err != Error.Ok)
		{
			throw new ApplicationException(
				$"Could not join server at '{Global.ConnectHostname}' port {port}: {err}");
		}
		Multiplayer.MultiplayerPeer = _multiplayerPeer;
		GD.Print($"✅ Connected to '{Global.ConnectHostname}' on port {port}!");
	}

	private void OnServerPeerConnected(long peerId)
	{
		GD.Print($"➡️ New player {peerId} connected!");
		// TODO: Tell new player to spawn all the old players
		ClientSpawnPlayer(peerId);
	}

	private void OnServerPeerDisconnected(long peerId)
	{
		GD.Print($"⬅️ Player {peerId} disconnected!");
	}

	[Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = true,
		TransferMode = MultiplayerPeer.TransferModeEnum.Reliable,
		TransferChannel = 1)]
	private void ClientSpawnPlayer(long peerId)
	{
		var player = _playerScene.Instantiate<Player>();
		AddChild(player);
		if (peerId == Multiplayer.GetUniqueId())
		{
			_playerController.Target = player;
		}
	}
}
