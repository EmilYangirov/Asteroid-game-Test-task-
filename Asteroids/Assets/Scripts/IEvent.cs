using UnityEngine;

public interface IEvent 
{
    public GameEvents gameEvents { get; set; }
    public Transform eventTransform { get; set; }
    public abstract void StartEvent();

    public abstract void StopEvent();
}
