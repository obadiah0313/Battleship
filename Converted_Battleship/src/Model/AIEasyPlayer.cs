using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
/// <summary>
/// AIHardPlayer is a type of player. This AI will know directions of ships
/// when it has found 2 ship tiles and will try to destroy that ship. If that ship
/// is not destroyed it will shoot the other way. Ship still not destroyed, then
/// the AI knows it has hit multiple ships. Then will try to destoy all around tiles
/// that have been hit.
/// </summary>
public class AIEasyPlayer : AIPlayer
{
	/// <summary>
	/// Private enumarator for AI states. currently there are two states,
	/// the AI can be searching for a ship, or if it has found a ship it will
	/// target the same ship
	/// </summary>
	private enum AIStates
	{
		/// <summary>
		/// The AI is searching for its next target
		/// </summary>
		Searching,

		/// <summary>
		/// The AI is trying to target a ship
		/// </summary>
		TargetingShip,

		/// <summary>
		/// The AI is locked onto a ship
		/// </summary>
		HittingShip
	}

	private AIStates _CurrentState = AIStates.Searching;
	private Stack<Location> _Targets = new Stack<Location> ();
	public AIEasyPlayer(BattleShipsGame game) : base(game)
	{
	}

	/// <summary>
	/// GenerateCoords will call upon the right methods to generate the appropriate shooting
	/// coordinates
	/// </summary>
	/// <param name="row">the row that will be shot at</param>
	/// <param name="column">the column that will be shot at</param>
	protected override void GenerateCoords(ref int row, ref int column)
	{
		do {
			SearchCoords(ref row, ref column);
			switch (_CurrentState) {
			case AIStates.Searching:
			case AIStates.TargetingShip:
				SearchCoords (ref row, ref column);
				break;
			default:
				throw new ApplicationException ("AI has gone in an imvalid state");
			}
		} while ((row < 0 || column < 0 || row >= EnemyGrid.Height || column >= EnemyGrid.Width || EnemyGrid[row, column] != TileView.Sea));
		//while inside the grid and not a sea tile do the search
	}
		

	/// <summary>
	/// SearchCoords will randomly generate shots within the grid as long as its not hit that tile already
	/// </summary>
	/// <param name="row">the generated row</param>
	/// <param name="column">the generated column</param>
	private void SearchCoords(ref int row, ref int column)
	{
		row = _Random.Next(0, EnemyGrid.Height);
		column = _Random.Next(0, EnemyGrid.Width);
	}

	/// <summary>
	/// ProcessShot is able to process each shot that is made and call the right methods belonging
	/// to that shot. For example, if its a miss = do nothing, if it's a hit = process that hit location
	/// </summary>
	/// <param name="row">the row that was shot at</param>
	/// <param name="col">the column that was shot at</param>
	/// <param name="result">the result from that hit</param>
	protected override void ProcessShot(int row, int col, AttackResult result)
	{
		switch (result.Value) {
		case ResultOfAttack.Miss:
			break;
		case ResultOfAttack.Hit:
			_CurrentState = AIStates.TargetingShip;
			AddTarget (row - 1, col);
			AddTarget (row, col - 1);
			AddTarget (row + 1, col);
			AddTarget (row, col + 1);
			break;
		case ResultOfAttack.Destroyed:
			break;
		case ResultOfAttack.ShotAlready:
			throw new ApplicationException("Error in AI");
		}
	}
	private void AddTarget (int row, int column)
	{

		if (row >= 0 && column >= 0 && row < EnemyGrid.Height && column < EnemyGrid.Width && EnemyGrid [row, column] == TileView.Sea) {
			_Targets.Push (new Location (row, column));
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================

