using UnityEngine;

namespace CodeBase.Services.Input
{
  public abstract class InputService : IInputService
  {
    protected const string Horizontal = "Horizontal";
    protected const string Vertical = "Vertical";
    private const string Button = "Fire";

    public abstract Vector2 Axis { get; }

    public bool IsAttackButtonUp() =>
      throw new System.NotImplementedException();
  }
}