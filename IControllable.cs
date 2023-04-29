public interface IControllable
{
    public void SetMovement(Vector2 direction);
    public void RotateAim(Vector2 delta);
    public void Jump();
    public void Fire();
}
