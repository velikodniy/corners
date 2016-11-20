using System;
using Corners;

namespace CornersCLI
{
	/// <summary>
	/// Сторона в игре
	/// </summary>
	abstract class Side {
		/// <summary>
		/// Ссылка на доску
		/// </summary>
		protected Board board;
		/// <summary>
		/// Цвет фигур, которыми играет сторона
		/// </summary>
		protected Player color;
		/// <summary>
		/// Инициализирует сторону
		/// </summary>
		/// <param name='board'>
		/// Ссылка на общую доску
		/// </param>
		/// <param name='color'>
		/// Цвет фигур
		/// </param>
		public Side (Board board, Player color)
		{
			this.board = board;
			this.color = color;
		}
		/// <summary>
		/// Делает очередной ход
		/// </summary>
		public abstract void MakeMove();
	}

	/// <summary>
	/// Игрок-человек
	/// </summary>
	class HumanSide : Side {
		public HumanSide(Board board, Player color) :
			base (board, color)
		{
		}

		public override void MakeMove ()
		{
			Console.WriteLine ("\n{0}\n", board);
			Console.WriteLine ("Ход {0} (вида a1b1):",
			                   color == Player.WHITE ? "белых" : "чёрных");
			bool correct = false;
			do {
				try {
					string input = Console.ReadLine();
					Coord from = new Coord(input.Substring(0, 2));
					Coord to   = new Coord(input.Substring(2, 2));
					board.Move(from, to, color);
					correct = true;
				} catch (IllegalMoveException) {
					Console.WriteLine ("Неправильный ход");
				} catch (OutOfBoardException) {
					Console.WriteLine ("Неправильные координаты");
				} catch (IndexOutOfRangeException) {
					Console.WriteLine ("Неправильный формат записи");
				}
			} while (!correct);
		}
	}

	/// <summary>
	/// Игрок-компьютер
	/// </summary>
	class AISide : Side {
		AI ai;
		public AISide(Board board, Player color) :
			base (board, color)
		{
			ai = new AI(color, board);
		}

		public override void MakeMove ()
		{
			ai.MakeMove();
		}
	}
}