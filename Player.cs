using Godot;
using System;

public partial class Player : CharacterBody3D
{
    public const float Speed = 10.0f;
    public const float JumpVelocity = 10.0f;
    public const float CameraXSensitivity = 0.005f;
    public const float CameraYSensitivity = 0.005f;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    private Camera3D _camera;
    
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _camera = GetNode<Camera3D>("Camera3D");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion)
        {
            var moveEvent = @event as InputEventMouseMotion;
            RotateY(-moveEvent.Relative.X * CameraXSensitivity);
            _camera.RotateX(-moveEvent.Relative.Y * CameraYSensitivity);
            _camera.Rotation = _camera.Rotation with {
                X = Mathf.Clamp(_camera.Rotation.X, -Mathf.Pi/2.0f, Mathf.Pi/2.0f) };
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            velocity.Y = JumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward",
        "move_backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y))
            .Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
