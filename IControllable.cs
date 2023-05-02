using Godot;
using System;

public interface IControllable
{
    public void SetCamera(bool isCurrent);
    public void SetMovement(Vector2 direction);
    public void RotateAim(Vector2 delta);
    public void Jump();
    public void Fire();
}
