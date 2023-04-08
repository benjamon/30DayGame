using UnityEngine;

namespace Bentendo
{
    public static class LayoutUtil
	{

		[System.Serializable]
		public class Grid
		{
			#region Properties
			[SerializeField]
			int r, c;
			[SerializeField]
			float w, h;
			float cw, ch;
			Vector2 p;
			Vector2 bottomLeft;
			public int row
            {
				get => r;
				set
                {
					if (r == value)
						return;
					r = value;
					UpdateCellSize();
                }
            }
			public int col
            {
				get => c;
				set
				{
					if (c == value)
						return;
					c = value;
					UpdateCellSize();
				}
			}
			public float width
			{
				get => w;
				set
				{
					if (w == value)
						return;
					w = value;
					UpdateCellSize();
				}
			}
			public float height
			{
				get => h;
				set
				{
					if (h == value)
						return;
					h = value;
					UpdateCellSize();
				}
			}
			public Vector2 Position
            {
				get => p;
                set
                {
					p = value;
					UpdateBottomLeft();
                }
            }
			#endregion

			public Vector2 CellSize => new Vector2(cw, ch);

            void UpdateCellSize()
            {
				cw = w / c;
				ch = h / r;
				UpdateBottomLeft();
			}

			void UpdateBottomLeft()
			{
				bottomLeft = new Vector2(p.x - (w - cw) / 2f, p.y - (h - ch) / 2f);
			}

			public int CellCount => c * r;

			public Vector2 GetPosition(int x, int y)
            {
				if (x >= c || y >= r)
					throw new System.Exception($"({x},{y}) invalid for grid size of ({col},{row})");
				return bottomLeft + new Vector2(x * cw, y * ch);
			}
			
			public Vector2 GetPosition(int n)
            {
				if (n / c > r)
					throw new System.Exception($"slot {n} is out of range of grid cell count {CellCount}");
				return GetPosition(n % c, n / c);
            }

			public void ForceUpdate()
			{
				UpdateCellSize();
				UpdateBottomLeft();
            }
        }
	}
}
