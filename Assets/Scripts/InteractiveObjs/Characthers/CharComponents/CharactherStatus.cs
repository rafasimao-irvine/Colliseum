using UnityEngine;
using System.Collections;

public class CharactherStatus {

	// Status
	protected int _TrappedTurns = 0;
	protected int _ParalizedTurns = 0;
	protected int _BlindedTurns = 0;
	protected int _DefenseTurns = 0;
	protected int _InvisibleTurns = 0;
	protected int _AbsorbTurns = 0;
	protected int _FlyingTurns = 0;
	
	protected bool _IsBuffed = false;
	//protected int _BuffTurns = 0;
	protected int _StrBuff = 0;
	//protected int _AtkRangeBuff = 0;

	//----------------------------------------------------

	public void UpdateStatus () {
		if (_TrappedTurns>0) _TrappedTurns--;
		if (_ParalizedTurns>0) _ParalizedTurns--;
		if (_BlindedTurns>0) _BlindedTurns--;
		if (_DefenseTurns>0) _DefenseTurns--;
		if (_InvisibleTurns>0) _InvisibleTurns--;
		if (_AbsorbTurns>0) _AbsorbTurns--;
		if (_FlyingTurns>0) _FlyingTurns--;
	}

	public bool IsTrapped () {
		return (_TrappedTurns > 0);
	}

	public bool IsParalized () {
		return (_ParalizedTurns > 0);
	}

	public bool IsBlinded() {
		return (_BlindedTurns > 0);
	}

	public bool IsBuffered () {
		return _IsBuffed;
	}

	public bool IsDefensive () {
		return (_DefenseTurns > 0);
	}

	public bool IsInvisible () {
		return (_InvisibleTurns > 0);
	}

	public bool IsAbsorbing () {
		return (_AbsorbTurns > 0);
	}

	public bool IsFlying () {
		return (_FlyingTurns > 0);
	}

	#region Users
	// Status Modifiers Users -----------------------------
	public int UseStrengthModifiers (Interactive target) {
		if (!(target.GetBeAttackedTarget() is Characther))
			return 0;

		if (_IsBuffed) {
			_IsBuffed = false;
			Logger.strLog += "\n"+GetType()+" gastou o buff.";
			return _StrBuff;
		}
		return 0;
	}
	#endregion

	#region Status Activators
	// Player Effects -------------------------------------
	public void BeTrapped (int turns) {
		_TrappedTurns = turns;
		Logger.strLog += "\n"+GetType()+" ficou preso por "+turns+" turnos.";
	}
	
	public void BeBuffered (int strBuff) {
		_StrBuff = strBuff;
		//_AtkRangeBuff = atkRangeBuff;
		_IsBuffed = true;
		//_BuffTurns = turns;
		Logger.strLog += "\n"+GetType()+" recebeu buff: +"+strBuff+" dano.";
	}

	public void BeParalized (int turns) {
		_ParalizedTurns = turns;
		Logger.strLog += "\n"+GetType()+" ficou paralizado por "+turns+" turnos.";
	}

	public void BeBlinded (int turns) {
		_BlindedTurns = turns;
		Logger.strLog += "\n"+GetType()+" ficou cego por "+turns+" turnos.";
	}

	public void BeDefensive (int turns) {
		_DefenseTurns = turns;
		Logger.strLog += "\n"+GetType()+" esta em defesa por "+turns+" turnos.";
	}

	public void BeInvisible (int turns) {
		_InvisibleTurns = turns;
		Logger.strLog += "\n"+GetType()+" ficou invisivel por "+turns+" turnos.";
	}

	public void BeAbsorptive (int turns) {
		_AbsorbTurns = turns;
		Logger.strLog += "\n"+GetType()+" vai absorver danos por "+turns+" turnos.";
	}

	public void BeFlying (int turns) {
		_FlyingTurns = turns;
		Logger.strLog += "\n"+GetType()+" esta voando por "+turns+" turnos.";
	}
	#endregion
}
