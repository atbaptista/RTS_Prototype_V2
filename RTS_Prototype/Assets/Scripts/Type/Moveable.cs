using UnityEngine;

public interface Moveable
{
    public void GoTo();
    void QueueMovement(Vector3 destination);
    void ClearMoveQueue();
    bool isMovingToDest();
}
