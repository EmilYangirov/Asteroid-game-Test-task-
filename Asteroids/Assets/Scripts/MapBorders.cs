using UnityEngine;

public class MapBorders : MonoBehaviour
{
    private Camera camera;
    private BoxCollider2D mapBorders;
    private Vector2 screenBounds;
    private float mapOffsetPercent = 20f;

    private void Awake()
    {
        camera = Camera.main;
        mapBorders = GetComponent<BoxCollider2D>();
        ChangeBorders();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag == "missile")
        {
            collision.gameObject.SetActive(false);
            return;
        }

        Teleport(collision.transform);
    }

    private void Teleport(Transform tr)
    {
        float horizontal = mapBorders.size.x / 2;
        float vertical = mapBorders.size.y / 2;

        int x = 1, y = 1;

        if (tr.position.x > horizontal || tr.position.x < -horizontal)
            x = -1;

        if (tr.position.y > vertical || tr.position.y < -vertical)
            y = -1;

        tr.position = new Vector2(tr.position.x * x, tr.position.y * y);

    }

    private void ChangeBorders()
    {
        screenBounds = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        mapBorders.size = screenBounds*2;
    }

    public Vector2 GetSpawnPosition(out Vector2 direction)
    {
        int borderNum = Random.Range(0, 3);
        float x = 0, y = 0;
        float xOffset = mapBorders.size.x * mapOffsetPercent / 100;
        float yOffset = mapBorders.size.y * mapOffsetPercent / 100;
        direction = new Vector2();

        switch (borderNum)
        {
            case 0:
                x = (float)(-mapBorders.size.x / 2 - 1);
                y = Random.Range(-mapBorders.size.y / 2 + yOffset, mapBorders.size.y / 2 - yOffset);
                direction = Vector2.right;
                break;
            case 1:
                x = (float)(mapBorders.size.x / 2 + 1);
                y = Random.Range(-mapBorders.bounds.size.y / 2 + yOffset, mapBorders.size.y / 2 - yOffset);
                direction = -Vector2.right;
                break;
            case 2:
                x = Random.Range(-mapBorders.size.x / 2 + xOffset, mapBorders.size.x / 2 - xOffset);
                y = (float)(mapBorders.size.y / 2 + 1);
                direction = -Vector2.up;
                break;
            case 3:
                x = Random.Range(-mapBorders.size.x / 2 + xOffset, mapBorders.size.x / 2 - xOffset);
                y = (float)(-mapBorders.size.y / 2 - 1);
                direction = Vector2.up;
                break;
        }

        return new Vector2(x, y);
    }

    public Vector2 RandomPointInBounds()
    {
        return new Vector2(
            Random.Range(mapBorders.bounds.min.x, mapBorders.bounds.max.x),
            Random.Range(mapBorders.bounds.min.y, mapBorders.bounds.max.y)
        );
    }

}
