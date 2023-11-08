using UnityEngine;

public interface Moveable
{
    public void GoTo();
    public void AMove(RaycastHit hit);
    public void QueueMovement(Vector3 destination);
    public void ClearMoveQueue();
    public bool isMovingToDest();
    
}
