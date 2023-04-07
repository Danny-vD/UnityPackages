using Enums;
using Enums.Grid;
using Grid.Tiles;

namespace Tiles
{
	public class NormalTile : AbstractTile
	{
		public override TileType TileType => TileType.Normal;
	}
}