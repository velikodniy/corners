using System;
using System.Collections.Generic;

namespace Corners
{
	/// <summary>
	/// Некуда ходить
	/// </summary>
	public class NoMoveException: Exception
	{
	}

	/// <summary>
	/// Искусственный интеллект (ИИ)
	/// </summary>
	public class AI
	{
		/// <summary>
		/// Каким цветом играет ИИ
		/// </summary>
		Player player;
		/// <summary>
		/// Текущее состояние доски
		/// </summary>
		Board board;

		/// <summary>
		/// Генератор случайных чисел
		/// </summary>
		Random rand;

		/// <summary>
		/// Инициализация нового экземпляра класса <see cref="Corners.AI"/>.
		/// </summary>
		/// <param name='player'>
		/// Каким цветом играет ИИ
		/// </param>
		/// <param name='board'>
		/// Ссылка на доску с текущим состоянием
		/// </param>
		public AI (Player player, Board board)
		{
			this.player = player;
			this.board = board;
			rand = new Random();
		}

		// TODO Предусмотреть возможность задания различных оценочных функций
		// TODO Избавиться от аргумента (перенести в Board?)
		/// <summary>
		/// Оценка текущей позиции на доске. Чем меньше значение, тем лучше позиция.
		/// </summary>
		/// <returns>Действительное цисло - численная оценка позиции</returns>
		double Cost (Board board)
		{
			Coord aim;
			if (player == Player.BLACK)
				aim = new Coord (1, 1);
			else
				aim = new Coord (6, 6);

			double cost = 0.0;
			foreach (var currentPiece in board.Pieces(player)) {
				cost += aim.Distance (currentPiece);
			}

			return cost;
		}

		/// <summary>
		/// Сделать лучший из возможных ходов
		/// </summary>
		public void MakeMove ()
		{
			double cost, minCost = double.PositiveInfinity;
			List<Coord> minFrom = new List<Coord>();
			List<Coord> minTo = new List<Coord>();

			// TODO Упростить поиск минимума
			foreach (var piece in board.Pieces(player)) {
				foreach (var to in board.CorrectJumps(piece)) {
					Board newBoard = new Board (board);
					newBoard.Move (piece, to, player);
					cost = Cost (newBoard);
					if (cost < minCost) {
						minCost = cost;
						minFrom.Clear();
						minFrom.Add(piece);
						minTo.Clear();
						minTo.Add(to);
					} else if (cost == minCost) {
						minFrom.Add(piece);
						minTo.Add(to);
					}
				}
			}

			if (minFrom.Count == 0)
				throw new NoMoveException();

			// Выбираем случайный ход из списка ходов с наименьшей ценой
			int index = rand.Next(minFrom.Count - 1);

			board.Move (minFrom[index], minTo[index], player);
		}
	}
}