using UnityEngine;

public interface Moveable
{
    public void GoTo(Vector3 destination);
    void QueueMovement(Vector3 destination);
    void ClearMoveQueue();
}
