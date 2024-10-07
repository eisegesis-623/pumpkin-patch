namespace Match3
{
    public partial class GridObject<T>
    {

        GridSystem2D<GridObject<T>> grid;
        int x;
        int y;

        public GridObject(GridSystem2D<GridObject<T>> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }
    }
}