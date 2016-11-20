using System;
using Corners;
using System.Collections.Generic;

namespace CornersCLI
{
	class Program
	{
		static void Main (string[] args)
		{
			Board board = new Board ();					// Доска
			Player? winner;								// Цвет игрока-победителя (или null)

			var side = new Dictionary<Player, Side>();	// Игроки

			// Игроки:
			side[Player.WHITE] = new HumanSide(board, Player.WHITE);	// Человек играет белыми
			side[Player.BLACK] = new AISide(board, Player.BLACK);		// Компьютер играет чёрными

			Player currentColor = Player.WHITE;
			do {
				// Сделать ход
				side[currentColor].MakeMove();
				// Сменить цвет игрока
				currentColor = currentColor == Player.WHITE ? Player.BLACK : Player.WHITE;
				// Есть ли победитель?
				winner = board.Winner ();
			} while (winner == null);

			// Вывести финальное состояние доски
			Console.WriteLine("\n{0}", board);

			// Вывести сообщение о победе
			Console.WriteLine ("Выиграли {0}", winner == Player.WHITE ? "белые" : "чёрные");
		}
	}
}