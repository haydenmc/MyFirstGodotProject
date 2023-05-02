using Godot;
using System;

public partial class Player : CharacterBody3D, IControllable
{
    public const float Speed = 10.0f;
    public const float JumpVelocity = 10.0f;
    public const float CameraXSensitivity = 0.005f;
    public const float CameraYSensitivity = 0.005f;

    private Vector2 _movementDirection;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    private Camera3D _camera;
    
    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera3D");
    }

    public override void _Input(InputEvent @event)
    { }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = _movementDirection;
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

    public void SetCamera(bool isCurrent)
    {
        if (isCurrent)
        {
            _camera.MakeCurrent();
        }
        else
        {
            _camera.ClearCurrent(false);
        }
    }

    public void SetMovement(Vector2 direction)
    {
        _movementDirection = direction;
    }

    public void RotateAim(Vector2 delta)
    {
        RotateY(-delta.X * CameraXSensitivity);
        _camera.RotateX(-delta.Y * CameraYSensitivity);
        _camera.Rotation = _camera.Rotation with {
            X = Mathf.Clamp(_camera.Rotation.X, -Mathf.Pi/2.0f, Mathf.Pi/2.0f) };
    }

    public void Jump()
    {
        if (IsOnFloor())
        {
            Velocity = Velocity with { Y = JumpVelocity };
        }
    }

    public void Fire()
    {
        ;
    }
}
