using System;
using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTest {

	class MoqMapController : MapController{
		public void SetMapTiles (Tile[,] mapTiles) {
			_MapTiles = mapTiles;
		}
	}

	[TestFixture()]
	public class MapControllerTest {

		MoqMapController _MapController;
		Tile[,] _MapTiles;

		public MapControllerTest () {
			_MapController = CreateComponent<MoqMapController>();
			_MapTiles = new Tile[10,10];
			for (int x=0; x<_MapTiles.GetLength(0); x++) {
				for (int y=0; y<_MapTiles.GetLength(1); y++) {
					_MapTiles[x,y] = CreateComponent<Tile>();
					_MapTiles[x,y].SetPosition(x,y);
				}
			}
			_MapController.SetMapTiles(_MapTiles);
		}

		private T CreateComponent<T> () where T : MonoBehaviour {
			GameObject go = new GameObject();
			go.AddComponent<T>();
			return go.GetComponent<T>();
		}

		[Test]
		public void GetNeighbours_NullOne_Null () {
			Assert.That(_MapController.GetNeighbours(null,1) == null);
		}

		[Test]
		public void GetNeighbours_CenterZero_Null () {
			Assert.That(_MapController.GetNeighbours(_MapTiles[5,5],0) == null);
		}

		[Test]
		public void GetNeighbours_CenterOne_RightNeighbours () {
			List<Tile> neighbours = _MapController.GetNeighbours(_MapTiles[5,5],1);

			Assert.That(neighbours.Count == 6);
			Assert.That(neighbours.Contains(_MapTiles[4,6]));
			Assert.That(neighbours.Contains(_MapTiles[5,6]));
			Assert.That(neighbours.Contains(_MapTiles[4,5]));
			Assert.That(neighbours.Contains(_MapTiles[6,5]));
			Assert.That(neighbours.Contains(_MapTiles[5,4]));
			Assert.That(neighbours.Contains(_MapTiles[6,4]));
		}

		[Test]
		public void GetNeighbours_CenterTwo_RightNeighbours () {
			List<Tile> neighbours = _MapController.GetNeighbours(_MapTiles[5,5],2);
			
			Assert.That(neighbours.Count == 18);
			Assert.That(neighbours.Contains(_MapTiles[3,7]));
			Assert.That(neighbours.Contains(_MapTiles[4,7]));
			Assert.That(neighbours.Contains(_MapTiles[5,7]));
			Assert.That(neighbours.Contains(_MapTiles[3,6]));
			Assert.That(neighbours.Contains(_MapTiles[4,6]));
			Assert.That(neighbours.Contains(_MapTiles[5,6]));
			Assert.That(neighbours.Contains(_MapTiles[6,6]));
			Assert.That(neighbours.Contains(_MapTiles[3,5]));
			Assert.That(neighbours.Contains(_MapTiles[4,5]));
			Assert.That(neighbours.Contains(_MapTiles[6,5]));
			Assert.That(neighbours.Contains(_MapTiles[7,5]));
			Assert.That(neighbours.Contains(_MapTiles[4,4]));
			Assert.That(neighbours.Contains(_MapTiles[5,4]));
			Assert.That(neighbours.Contains(_MapTiles[6,4]));
			Assert.That(neighbours.Contains(_MapTiles[7,4]));
			Assert.That(neighbours.Contains(_MapTiles[5,3]));
			Assert.That(neighbours.Contains(_MapTiles[6,3]));
			Assert.That(neighbours.Contains(_MapTiles[7,3]));
		}

		[Test]
		public void GetNeighbours_MinCornerOne_RightNeighbours () {
			List<Tile> neighbours = _MapController.GetNeighbours(_MapTiles[0,0],1);
			
			Assert.That(neighbours.Count == 2);
			Assert.That(neighbours.Contains(_MapTiles[0,1]));
			Assert.That(neighbours.Contains(_MapTiles[1,0]));
		}

		[Test]
		public void GetNeighbours_MaxCornerOne_RightNeighbours () {
			List<Tile> neighbours = _MapController.GetNeighbours(
				_MapTiles[_MapTiles.GetLength(0)-1,_MapTiles.GetLength(1)-1],1);
			
			Assert.That(neighbours.Count == 2);
			Assert.That(neighbours.Contains(_MapTiles[_MapTiles.GetLength(0)-2,_MapTiles.GetLength(1)-1]));
			Assert.That(neighbours.Contains(_MapTiles[_MapTiles.GetLength(0)-1,_MapTiles.GetLength(1)-2]));
		}

		[TestFixtureTearDown]
		public void TearDown () {
			int maxX = _MapTiles.GetLength(0);
			int maxY = _MapTiles.GetLength(1);
			for (int x=0; x<maxX; x++) {
				for (int y=0; y<maxY; y++) {
					MonoBehaviour.DestroyImmediate(_MapTiles[x,y].gameObject);
				}
			}
			MonoBehaviour.DestroyImmediate(_MapController.gameObject);
		}
	}
}
