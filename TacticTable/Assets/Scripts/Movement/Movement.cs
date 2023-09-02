using UnityEngine;

public abstract class Movement
{
   [SerializeField]
    protected GameObject disk;
    [SerializeField]
    protected Vector3 startPos = Vector3.zero;
    [SerializeField]
    protected Vector3 endPos = Vector3.zero;

    public Movement(GameObject disk, Vector3 startPos, Vector3 endPos)
    {
        this.disk = disk;
        this.startPos = startPos;
        this.endPos = endPos;
    }

    public abstract void Animate(float time);
}
