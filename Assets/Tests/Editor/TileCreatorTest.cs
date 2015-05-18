using System;
using UnityEngine;
using NUnit.Framework;

namespace UnitTest {

	[TestFixture()]
	public class TileCreatorTest {

		private TileCreator _TileCreator;

		public TileCreatorTest () {
			//_TileCreator = NSubstitute.Substitute.For<TileCreator>();
			GameObject go = new GameObject();
			go.AddComponent<TileCreator>();
			_TileCreator = go.GetComponent<TileCreator>();
			/*
			_TileCreator.Z_NTiles = 13;
			_TileCreator.X_NTiles = 13;
			_TileCreator.Tiles_Size = 2;
			_TileCreator.GenType = 2;
			_TileCreator.TilePrefab = (GameObject)Resources.Load("Prefabs/Tiles/TilePrism");
			*/
		}

		[Test]
		public void GenerateTile_Random_Between1AndNTiles () {
			int max = Tile.NTiles*Tile.NTiles;
			for (int i=0; i<max; i++) {
				int generated = _TileCreator.GenerateTile(TileCreator.GEN_RANDOM, 0,0);
				if(generated<1 || generated>Tile.NTiles)
					Assert.Fail("GenerateTile_RANDOM generated out of range.");
			}
			Assert.Pass();
		}

		[Test]
		public void	GenerateTile_INARRAY_ValidInArrayNumber () {
			int[] types = new int[]{1,4,6,7};
			_TileCreator.TilesTypes = types;
			int max = types.Length*types.Length;
			for (int i=0; i<max; i++) {
				int generated = _TileCreator.GenerateTile(TileCreator.GEN_INARRAY, 0,0);
				bool contains = false;
				for (int j=0; j<types.Length; j++) {
					if(types[j] == generated)
						contains = true;
				}
				if(!contains)
					Assert.Fail("GenerateTile_INARRAY generated out of range.");
			}
			Assert.Pass();
		}

		[Test]
		public void GenerateTile_MANUAL_SuccessiveNumbers () {
			int[] entries = _TileCreator.GetManualEntries();

			if(entries.Length<2)
				Assert.Inconclusive("GenerateTile_MANUAL: manual entries are insuficient");

			int generated = _TileCreator.GenerateTile(TileCreator.GEN_MANUAL, 0,0);
			Assert.That(generated == entries[0]);

			generated = _TileCreator.GenerateTile(TileCreator.GEN_MANUAL, 0,0);
			Assert.That(generated == entries[1]);
		}

		[TestFixtureTearDown]
		public void destroy () {
			MonoBehaviour.DestroyImmediate(_TileCreator.gameObject);
		}

	}
}
