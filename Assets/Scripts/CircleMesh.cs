using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CircleMesh
{
    public static void GetCircleMeshData(Sprite pizzaSprite, int edges, out Vector2[] vertices, out ushort[] triangles)
    {
        float radius = pizzaSprite.bounds.extents.x;
        vertices = new Vector2[edges];
        triangles = new ushort[(edges - 1) * 3];

        for (int i = 0; i < vertices.Length; i++)             //задаем координаты вершин окружности
        {
            float angle = 2f * Mathf.PI * ((float)i / edges);
            vertices[i] = new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
        }
        RecalculateVertices(pizzaSprite, ref vertices);

        for (int i = 0, j = 1; i < triangles.Length - 3; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = (ushort)j;
            triangles[i + 2] = (ushort)(j + 1);
        }
    }

    private static void RecalculateVertices(Sprite pizzaSprite, ref Vector2[] vertices)  //приводим вершины к координатам спрайта
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].x = Mathf.Clamp(
                (vertices[i].x - pizzaSprite.bounds.center.x -
                    (pizzaSprite.textureRectOffset.x / pizzaSprite.texture.width) + pizzaSprite.bounds.extents.x) /
                    (2.0f * pizzaSprite.bounds.extents.x) * pizzaSprite.rect.width,
                0.0f, pizzaSprite.rect.width);

            vertices[i].y = Mathf.Clamp(
                (vertices[i].y - pizzaSprite.bounds.center.y -
                    (pizzaSprite.textureRectOffset.y / pizzaSprite.texture.height) + pizzaSprite.bounds.extents.y) /
                    (2.0f * pizzaSprite.bounds.extents.y) * pizzaSprite.rect.height,
                0.0f, pizzaSprite.rect.height);
        }
    }

    public static void CutCircleNew(Sprite pizzaSprite, Vector2 firstPoint, Vector2 secondPoint, out Vector2[] verticesFirstArray, 
        out ushort[] trianglesFirstArray, out Vector2[] verticesSecondArray, out ushort[] trianglesSecondArray)
    {
        List<Vector2> verticesSource = new List<Vector2>(pizzaSprite.vertices);

        List<Vector2> verticesFisrtSide = new List<Vector2>();
        List<Vector2> verticesSecondSide = new List<Vector2>();



        int startIndex = GetNumberOfFirstVert(firstPoint, secondPoint, verticesSource);

        for (int i = startIndex; i < verticesSource.Count + startIndex; i++)
        {
            int j = (int)Mathf.Repeat(i, verticesSource.Count);
            int side = SelectSide(firstPoint, secondPoint, verticesSource[j]);
            if (side == 1)
            {
                verticesFisrtSide.Add(verticesSource[j]);
            }
        }
        verticesFisrtSide.Add(firstPoint);
        verticesFisrtSide.Add(secondPoint);


        for (int i = startIndex; i < verticesSource.Count + startIndex; i++)
        {
            int j = (int)Mathf.Repeat(i, verticesSource.Count);
            int side = SelectSide(firstPoint, secondPoint, verticesSource[j]);
            if (side == -1)
            {
                verticesSecondSide.Add(verticesSource[j]);
            }
        }
        verticesSecondSide.Add(secondPoint);
        verticesSecondSide.Add(firstPoint);


        trianglesFirstArray = new ushort[3 * (verticesFisrtSide.Count - 1)];
        SetTriangles(ref trianglesFirstArray);
        verticesFirstArray = verticesFisrtSide.ToArray();

        trianglesSecondArray = new ushort[3 * (verticesSecondSide.Count - 1)];
        SetTriangles(ref trianglesSecondArray);
        verticesSecondArray = verticesSecondSide.ToArray();

        RecalculateVertices(pizzaSprite, ref verticesFirstArray);
        RecalculateVertices(pizzaSprite, ref verticesSecondArray);

        //pizzaSprite.OverrideGeometry(verticesArray, trianglesArray);
    }

    private static int SelectSide(Vector2 from, Vector2 to, Vector2 point)      //с какой стороны от линии находятся треугольники
    {
        return (int)Mathf.Sign((to.x - from.x) * (point.y - from.y) - (to.y - from.y) * (point.x - from.x));
    }

    private static void SetTriangles(ref ushort[] triangles)        //пересчитать треугольники
    {
        for (int i = 0, j = 1; i < triangles.Length - 3; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = (ushort)j;
            triangles[i + 2] = (ushort)(j + 1);
        }
    }

    private static int GetNumberOfFirstVert(Vector2 firstPoint, Vector2 secondPoint, List<Vector2> verticesSource)  
    //номер первой вершины в списке
    {
        int temp = SelectSide(firstPoint, secondPoint, verticesSource[verticesSource.Count - 1]);
        for (int i = 0; i < verticesSource.Count; i++)
        {
            int side = SelectSide(firstPoint, secondPoint, verticesSource[i]);
            if(side != temp)
            {
                return i;
            }
            temp = side;
        }

        return 0;
    }
}
