using UnityEngine;

public class MainPizza : MonoBehaviour
{
    public static System.Action<float> dispalyText;

    private new PolygonCollider2D collider;
    private Sprite pizzaSprite;

    private float fullArea;

    // Use this for initialization
    void Start()
    {
        collider = GetComponent<PolygonCollider2D>();
        pizzaSprite = GetComponent<SpriteRenderer>().sprite;


        Vector2[] vertices;
        ushort[] triangles;
        CircleMesh.GetCircleMeshData(pizzaSprite, 40, out vertices, out triangles);

        pizzaSprite.OverrideGeometry(vertices, triangles);
        RecalculateCollider();
        fullArea = CalculateArea(vertices);
    }

    private void OnEnable()
    {
        InputScissors.successfulInput += Divide;
    }

    private void OnDisable()
    {
        InputScissors.successfulInput -= Divide;
    }

    private float CalculateArea(Vector2[] vertices)
    {
        Vector3 result = Vector3.zero;
        for (int p = vertices.Length - 1, q = 0; q < vertices.Length; p = q++)
        {
            result += Vector3.Cross(vertices[q], vertices[p]);
        }
        result *= 0.5f;
        return result.magnitude;
    }

    private void RecalculateCollider()
    {
        collider.SetPath(0, pizzaSprite.vertices);
    }


    private void Divide(Vector2 firstPoint, Vector2 secondPoint)
    {
        Vector2[] verticesFirstArray;
        ushort[] trianglesFirstArray;
        Vector2[] verticesSecondArray;
        ushort[] trianglesSecondArray;

        CircleMesh.CutCircleNew(pizzaSprite, firstPoint, secondPoint, out verticesFirstArray,
        out trianglesFirstArray, out verticesSecondArray, out trianglesSecondArray);

        float firstArea = CalculateArea(verticesFirstArray);
        float secondArea = CalculateArea(verticesSecondArray);

        if(firstArea > secondArea)
        {
            pizzaSprite.OverrideGeometry(verticesFirstArray, trianglesFirstArray);
            CreatePeace(verticesSecondArray, trianglesSecondArray);
            dispalyText.Invoke((secondArea / fullArea) * 100f);
        }
        else
        {
            pizzaSprite.OverrideGeometry(verticesSecondArray, trianglesSecondArray);
            CreatePeace(verticesFirstArray, trianglesFirstArray);
            dispalyText.Invoke((firstArea / fullArea) * 100f);
        }

        RecalculateCollider();
    }

    private void CreatePeace(Vector2[] vertices, ushort[] triangles)
    {
        GameObject obj = new GameObject("Peace");
        obj.transform.parent = this.transform;
        SpriteRenderer rend = obj.AddComponent<SpriteRenderer>();
        rend.sprite = Instantiate(pizzaSprite) as Sprite;
        rend.sprite.OverrideGeometry(vertices, triangles);
        obj.AddComponent<PolygonCollider2D>();
        Rigidbody2D rigBody = obj.AddComponent<Rigidbody2D>();
        rigBody.AddForce(new Vector2(Random.Range(-2f, 2f), Random.Range(0, 3f) + 3f), ForceMode2D.Impulse);
    }

}
