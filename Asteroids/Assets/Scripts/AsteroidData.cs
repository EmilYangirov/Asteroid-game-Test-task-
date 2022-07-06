using UnityEngine;

[CreateAssetMenu(fileName = "NewAsteroidData", menuName = "Data/New asteroid data")]
public class AsteroidData : ScriptableObject
{
    public Type type; 
    public Sprite image;
    public float maxSpeed;
    public float minSpeed;
    public float radius;
    public Vector3 scale;
    public int score;
}

public enum Type
{
    big,
    mid,
    small
}

