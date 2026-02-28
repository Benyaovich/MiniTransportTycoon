using System;
using UnityEngine;
using UniVector3 = UnityEngine.Vector3;
using SysVector3 = System.Numerics.Vector3;

public class GridManager : MonoBehaviour
{
    private Size gridSize = new Size(10, 10);
    private float gridCellSize = 10f;
    private SysVector3 gridOriginPosition = new SysVector3(0, 0, 0);

    private Grid<GridObject> grid;

    private void Start()
    {
        grid = new Grid<GridObject>(gridSize, gridCellSize, gridOriginPosition, 
            (g, l) => new GridObject(g, l));

        bool showDebug = true;
        if (showDebug) {
            TextMesh[,] debugTextArray = new TextMesh[grid.Size.Width, grid.Size.Height];

            for (int x = 0; x < grid.Size.Width; x++) {
                for (int y = 0; y < grid.Size.Height; y++) {
                    debugTextArray[x, y] = Utils.CreateWorldText(grid.GetGridObject(x,y)?.ToString(), null, grid.GetWorldPosition(x, y).UV3() + new Vector3(gridCellSize, gridCellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(grid.GetWorldPosition(x, y).UV3(), grid.GetWorldPosition(x, y + 1).UV3(), Color.white, 100f);
                    Debug.DrawLine(grid.GetWorldPosition(x, y).UV3(), grid.GetWorldPosition(x + 1, y).UV3(), Color.white, 100f);
                }
            }
            Debug.DrawLine(grid.GetWorldPosition(0, grid.Size.Height).UV3(), grid.GetWorldPosition(grid.Size.Width, grid.Size.Height).UV3(), Color.white, 100f);
            Debug.DrawLine(grid.GetWorldPosition(grid.Size.Width, 0).UV3(), grid.GetWorldPosition(grid.Size.Width, grid.Size.Height).UV3(), Color.white, 100f);

            grid.OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.location.X, eventArgs.location.Y].text = grid.GetGridObject(eventArgs.location.X, eventArgs.location.Y).ToString();
            };
        }
    }
}
