namespace MyFirstGodotProject;

public partial class PlayerController : Node
{
    [Export]
    public Node Target
    {
        get
        {
            return _target as Node;
        }
        set
        {
            if (value is IControllable)
            {
                _target = value as IControllable;
            }
            throw new Exception("PlayerController target must be an IControllable");
        }
    }
    
    private IControllable _target;
    
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Process(double delta)
    {
        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward",
            "move_backward");
        if (_target != null)
        {
            _target.SetMovement(inputDir);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion)
        {
            var moveEvent = @event as InputEventMouseMotion;
            _target.RotateAim(moveEvent.Relative);
        }
        else
        {
            if (@event.IsAction("jump"))
            {
                _target?.Jump();
            }
            else if (@event.IsAction("fire"))
            {
                _target?.Fire();
            }
        }
    }
}
