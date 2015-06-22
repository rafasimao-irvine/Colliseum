using System;
using UnityEngine;
using NUnit.Framework;

namespace UnitTest {

	class MoqCharacther : Characther {

		protected override bool MakeTurnAction () {
			return true;
		}

		protected override void BecomeInvisible () {}
		protected override void BecomeVisible () {}
	}

	class MoqInteractive : Interactive {
		public Tile returnTile;

		public override void BeAttacked (Interactive iObj, int damage){}

		public override bool BeEntered (Characther c) {
			return true;
		}

		public override bool BeLeft (Characther c) {
			return false;
		}
	}

	class MoqTile : Tile {
		public void SetOnTop (Interactive i) {
			OnTop = i;
		}
	}

	[TestFixture]
	public class TileTest {

		private Tile _Tile1, _Tile2;

		public TileTest () {
			_Tile1 = CreateComponent<Tile>();
			_Tile2 = CreateComponent<Tile>();
		}

		private T CreateComponent<T> () where T : MonoBehaviour{
			GameObject go = new GameObject();
			go.AddComponent<T>();
			return go.GetComponent<T>();
		}

		private void DestroyGO (MonoBehaviour component) {
			MonoBehaviour.DestroyImmediate(component.gameObject);
		}

		// CompareTo ------------------------------------------
		[Test]
		public void CompareTo_LowerFCost_LowerThanZero () {
			_Tile1.GCost = 3;
			_Tile1.HCost = 1;
			_Tile2.GCost = 2;
			_Tile2.HCost = 1;

			Assert.That(_Tile1.CompareTo(_Tile2) < 0);
		}

		[Test]
		public void CompareTo_GreaterFCost_GreaterThanZero () {
			_Tile1.GCost = 3;
			_Tile1.HCost = 1;
			_Tile2.GCost = 4;
			_Tile2.HCost = 2;
			
			Assert.That(_Tile1.CompareTo(_Tile2) > 0);
		}

		[Test]
		public void CompareTo_EqualFCost_OppositeOfComparasionHCost () {
			_Tile1.GCost = 5;
			_Tile1.HCost = 1;
			_Tile2.GCost = 4;
			_Tile2.HCost = 2;
			
			Assert.That(_Tile1.CompareTo(_Tile2) == -_Tile1.HCost.CompareTo(_Tile2.HCost));
		}

		// TryGetIn -------------------------------------------------
		[Test]
		public void TryGetIn_Null_NullNoOnTopChanges () {
			MoqTile tile = CreateComponent<MoqTile>();
			Interactive moqI = CreateComponent<MoqInteractive>();
			tile.SetOnTop(moqI);
			Assert.That(tile.TryGetIn(null)); // did not get in
			Assert.That(tile.OnTop == moqI); // still moq

			DestroyGO(tile);
			DestroyGO(moqI);
		}

		[Test]
		public void TryGetIn_FreeTile_TileAndSetNewOnTop () {
			Tile tile = CreateComponent<Tile>();
			if (tile.OnTop == null) {
				Interactive moqI = CreateComponent<MoqInteractive>();
				Assert.That(tile.TryGetIn(moqI) == tile); // Returnes itself
				Assert.That(tile.OnTop == moqI); // Got in

				DestroyGO(moqI);
			} else {
				Assert.Inconclusive("_tile1 already had something on top");
			}

			DestroyGO(tile);
		}

		[Test]
		public void TryGetIn_OccupiedTile_NullNoOnTopChanges () {
			MoqTile tile = CreateComponent<MoqTile>();
			Interactive moqI = CreateComponent<MoqInteractive>();
			tile.SetOnTop(moqI);
			// OnTop is occupied, BeEntered return null;

			Interactive moqI2 = CreateComponent<MoqInteractive>();
			Assert.That(tile.TryGetIn(moqI2));
			Assert.That(tile.OnTop == moqI);

			DestroyGO(tile);
			DestroyGO(moqI);
			DestroyGO(moqI2);
		}

		[Test]
		public void TryGetIn_OccupiedTile_NullNoOnoTopChangesCallBeEntered () {
			MoqTile tile = CreateComponent<MoqTile>();
			MoqInteractive moqI = CreateComponent<MoqInteractive>();
			tile.SetOnTop(moqI);
			moqI.returnTile = _Tile1;
			// OnTop is occupied, BeEntered return null;
			
			Characther c = CreateComponent<MoqCharacther>();
			Assert.That(tile.TryGetIn(c) == _Tile1);
			Assert.That(tile.OnTop == moqI);

			DestroyGO(tile);
			DestroyGO(moqI);
			DestroyGO(c);
		}

		// TryGetOut -------------------------------------------------
		[Test]
		public void TryGetOut_Null_TrueNoOnTopChanges () {
			MoqTile tile = CreateComponent<MoqTile>();
			Interactive moqI = CreateComponent<MoqInteractive>();
			tile.SetOnTop(moqI);
			Assert.That(tile.TryGetOut(null) == true); // no problem
			Assert.That(tile.OnTop == moqI); // still moqI

			DestroyGO(tile);
			DestroyGO(moqI);
		}

		[Test]
		public void TryGetOut_RightonOnTop_TrueClearOnTop () {
			MoqTile tile = CreateComponent<MoqTile>();
			Interactive moqI = CreateComponent<MoqInteractive>();
			tile.SetOnTop(moqI);
			Assert.That(tile.TryGetOut(moqI) == true); // no problem
			Assert.That(tile.OnTop == null); // still moqI

			DestroyGO(tile);
			DestroyGO(moqI);
		}

		[Test]
		public void TryGetOut_OtherOnTop_TrueNoOnTopChanges () {
			MoqTile tile = CreateComponent<MoqTile>();
			Interactive moqI = CreateComponent<MoqInteractive>();
			tile.SetOnTop(moqI);

			Interactive moqI2 = CreateComponent<MoqInteractive>();
			Assert.That(tile.TryGetOut(moqI2) == true); // no problem
			Assert.That(tile.OnTop == moqI); // still moqI

			DestroyGO(tile);
			DestroyGO(moqI);
			DestroyGO(moqI2);
		}

		[Test]
		public void TryGetOut_OtherOnTopIsCharacther_FalseNoOnTopChanges () {
			MoqTile tile = CreateComponent<MoqTile>();
			Interactive moqI = CreateComponent<MoqInteractive>();
			tile.SetOnTop(moqI);
			
			Characther c = CreateComponent<MoqCharacther>();
			Assert.That(tile.TryGetOut(c) == false); // no problem
			Assert.That(tile.OnTop == moqI); // still moqI

			DestroyGO(tile);
			DestroyGO(moqI);
			DestroyGO(c);
		}

		[TestFixtureTearDown]
		public void destroy () {
			DestroyGO(_Tile1);
			DestroyGO(_Tile2);
		}
	}
}
