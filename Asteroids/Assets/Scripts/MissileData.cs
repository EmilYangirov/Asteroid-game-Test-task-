using UnityEngine;

[CreateAssetMenu(fileName = "NewMissileData", menuName = "Data/New missile data")]
public class MissileData : ScriptableObject
{
    public Sprite image;
    public float moveSpeed;
    public LayerMask hitLayers;
}
