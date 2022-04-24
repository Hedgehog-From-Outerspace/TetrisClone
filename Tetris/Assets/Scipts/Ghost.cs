using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;
    public Board board;
    public Piece trackingpiece;

    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }

    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();

    }

    private void Clear()
    {
        for (int j = 0; j < this.cells.Length; j++)
        {
            Vector3Int tilePosition = this.cells[j] + this.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    private void Copy()
    {
        for (int j = 0; j < this.cells.Length; j++)
        {
            this.cells[j] = this.trackingpiece.cells[j];
        }
    }

    private void Drop()
    {
        Vector3Int position = this.trackingpiece.position;

        int currentRow = position.y;
        int bottom = -this.board.boardSize.y / 2 - 1;

        this.board.Clear(this.trackingpiece);

        for (int row = currentRow; row >= bottom; row--)
        {
            position.y = row;

            if (this.board.IsValidPosition(this.trackingpiece, position))
            {
                this.position = position;
            }
            else
            {
                break;
            }
        }

        this.board.Set(this.trackingpiece);
    }

    private void Set()
    {
        for (int j = 0; j < this.cells.Length; j++)
        {
            Vector3Int tilePosition = this.cells[j] + this.position;
            this.tilemap.SetTile(tilePosition, this.tile);
        }
    }
}
