using System.Collections;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private int poolCount;
    [SerializeField] private bool autoExpand;
    [SerializeField] private Asteroid prefab;
    [SerializeField] private int startCount;
    [SerializeField] private float maxOffsetAngle = 45;

    [SerializeField] private AsteroidData[] smallData, midData, bigData;

    private PoolObject<Asteroid> pool;
    
    private MapBorders mapBorders;
    private Score score;

    [HideInInspector]
    public SoundPlayer asteroidSoundPlayer { get; private set; }
        
    private void Start()
    {
        mapBorders = Camera.main.transform.GetComponent<MapBorders>();

        var scoreGameObject = GameObject.FindGameObjectWithTag("Score");
        score = scoreGameObject.GetComponent<Score>();

        pool = new PoolObject<Asteroid>(prefab, transform, autoExpand, poolCount);
        asteroidSoundPlayer = new SoundPlayer(gameObject);
        GenerateAsteroids(startCount);
    }

    private void GenerateAsteroids(int count)
    {
        for (int i = 0; i < count; i++)
            CreateAsteroid();
    }

    private void CreateAsteroid()
    {
        Asteroid newAsteroid = pool.GetFreeElement();
        newAsteroid.spawner = this;
        var direction = new Vector2();
        newAsteroid.transform.position = mapBorders.GetSpawnPosition(out direction);
        direction = NewDirection(newAsteroid.transform.position);
        newAsteroid.ChangeParameter(GetRandomData(bigData), direction, scores: score);
    }

    public void CreateAsteroidsByCount(Asteroid creator, int count)
    {
        AsteroidData data = new AsteroidData();

        //get correct data from datasets
        switch (creator.type)
        {
            case Type.big:
                data = GetRandomData(midData);
                break;
            case Type.mid:
                data = GetRandomData(smallData);
                break;
        }

        float speed = Random.Range(data.minSpeed, data.maxSpeed);

        for (int i = 0; i < count; i++)
        {
            Asteroid newAsteroid = pool.GetFreeElement();
            newAsteroid.spawner = this;

            newAsteroid.transform.position = creator.transform.position;
            var dir = NewDirection(creator);
            newAsteroid.ChangeParameter(data, dir, speed, score);
        }
    }

    public void OnSmallAsteroidDie(Asteroid asteroid)
    {
        if (!pool.HasActiveElement(asteroid))
        {
            StartCoroutine(SpawnAfterSeconds());
            return;
        }
    }

    private IEnumerator SpawnAfterSeconds()
    {
        yield return new WaitForSeconds(2);
        startCount++;
        GenerateAsteroids(startCount);
    }    

    private Vector2 NewDirection(Vector2 position)
    {
        return (mapBorders.RandomPointInBounds() - position).normalized;
    }
    private Vector2 NewDirection(Asteroid creator)
    {
        float angle = Random.Range(-maxOffsetAngle, maxOffsetAngle);
        var offset = Quaternion.Euler(0, 0, angle);
        return offset * creator.GetDirection();
    }    

    private AsteroidData GetRandomData(AsteroidData[] data)
    {
        return data[Random.Range(0, data.Length - 1)];
    }
}
