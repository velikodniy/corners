using System;
using System.Text;
using System.Collections.Generic;

namespace Corners
{
	/// <summary>
	/// Неправильный ход
	/// </summary>
	public class IllegalMoveException : Exception
	{
	}

	/// <summary>
	/// Доска
	/// </summary>
	public class Board
	{
		/// <summary>
		/// Массив клеток доски.
		/// </summary>
		/// <remarks>
		/// Каждой клетке соответствует либо игрок, либо <c>null</c>.
		/// </remarks>
		Player?[,] board;

		/// <summary>
		/// Устанавливает начальное расположение фигур.
		/// </summary>
		public Board ()
		{
			board = new Player?[8, 8];

			// Заполнение доски белыми, черными и пустыми клетками
			for (int i = 0; i < 8; i++)             
				for (int j = 0; j < 8; j++)
					if (i <= 2 && j <= 2)		// нижний левый угол
						board [i, j] = Player.WHITE;
					else if (i >= 5 && j >= 5)	// верхний правый угол
						board [i, j] = Player.BLACK;
					else
						board [i, j] = null;
		}

		/// <summary>
		/// Конструктор копирования
		/// </summary>
		/// <param name='b'>
		/// Доска, копия которой будет создаваться
		/// </param>
		public Board (Board otherBoard)
		{
			board = (Player?[,])otherBoard.board.Clone ();
		}

		/// <summary>
		/// Значение в клетке с указанными координатами
		/// </summary>
		/// <param name='c'>
		/// Координаты клетки
		/// </param>
		public Player? this [Coord coord] {
			get {
				return board [coord.Row, coord.Column];
			}
			private set {
				board [coord.Row, coord.Column] = value;
			}
		}

		/// <summary>
		/// Значение в клетке с указанными координатами
		/// </summary>
		/// <param name='r'>
		/// Строка
		/// </param>
		/// <param name='c'>
		/// Столбец
		/// </param>
		public Player? this [int r, int c] {
			get {
				return board [r, c];
			}
			private set {
				board [r, c] = value;
			}
		}

		/// <summary>
		/// Возвращает строку, представляющую текущее состояние доски
		/// </summary>
		/// <returns>Строка с состоянием доски</returns>
		public override string ToString ()
		{
			StringBuilder s = new StringBuilder (150);

			for (int i = 7; i >= 0; i--) {
				s.Append (i + 1);
				s.Append (" ");
				for (int j = 0; j < 8; j++)
					if (board [i, j] == Player.BLACK)
						s.Append ('X');
					else if (board [i, j] == Player.WHITE)
						s.Append ('O');
					else
						s.Append ('.');
				s.Append ('\n');
			}

			s.Append ("  ");
			for (int j = 0; j < 8; j++)
				s.Append ((char)('a' + j));

			return s.ToString ();
		}

		// TODO Оптимизировать
		/// <summary>
		/// Проверяет, победил ли один из игроков
		/// </summary>
		/// <returns>
		/// Player.WHITE - победили белые,
		/// Player.BLACK - чёрные,
		/// null - выигрышной ситуации нет
		/// </returns>
		public Player? Winner ()
		{
			int black = 0, white = 0;
			for (int i = 0; i < 8; i++)
				for (int j = 0; j < 8; j++)
					if (i >= 5 && j >= 5 && board [i, j] == Player.WHITE)
						white += 1;
					else if (i <= 2 && j <= 2 && board [i, j] == Player.BLACK)
						black += 1;
			if (black == 9)
				return Player.BLACK;
			else if (white == 9)
				return Player.WHITE;
			else
				return null;
		}

		/// <summary>
		/// Сделать ход 
		/// </summary>
		/// <param name="from">откуда</param>
		/// <param name="to">куда</param>
		public void Move (Coord from, Coord to, Player player)
		{

			if (this [to] != null || this [from] != player)
				throw new IllegalMoveException ();

			if (CanJumpTo (from, to)) {
				this [from] = null;
				this [to] = player;
			} else
				throw new IllegalMoveException ();
		}

		/// <summary>
		/// Куда можно пойти из указанной точки
		/// </summary>
		/// <param name="from">Начальная точка</param>
		/// <returns>Множество координат, куда можно пойти</returns>
		public HashSet<Coord> CorrectJumps (Coord from)
		{
			Queue<Coord> toCheck = new Queue<Coord> ();		// Очередь на проверку
			HashSet<Coord> canJump = new HashSet<Coord> ();	// Возможные ходы
			canJump.Add (from);				// Добавляем текущую клетку, чтобы не проверять её в дальнейшем
			toCheck.Enqueue (from);			// Добавляем в очередь на проверку исходную клетку
			Coord currentFrom;				// Текущая проверяемая клетка
			Coord currentTo;				// Клетка, на которую прыгаем из текущей    
			Coord currentTo2;				// Клетка, на которую можно прыгнуть если currentTo не пуста

			// Проверка возможности серии прыжков
			while (toCheck.Count > 0) {   // пока в очереди есть хоть один элемент
				currentFrom = toCheck.Dequeue ();

				for (int i = -1; i <= 1; i++)
					for (int j = -1; j <= 1; j++) {
						if (i == 0 && j == 0)
							continue;
						try {
							currentTo = new Coord (currentFrom.Row + i, currentFrom.Column + j);
							currentTo2 = new Coord (currentFrom.Row + i * 2, currentFrom.Column + j * 2);
						} catch (OutOfBoardException) {
							continue;
						}

						if (this [currentTo] != null && this [currentTo2] == null && !canJump.Contains (currentTo2)) {
							toCheck.Enqueue (currentTo2);
							canJump.Add (currentTo2);
						}
					}
			}

			// Проверка одиночного хода
			for (int i = -1; i <= 1; i++)
				for (int j = -1; j <= 1; j++) {
					if (i == 0 && j == 0)
						continue;
					try {
						currentTo = new Coord (from.Row + i, from.Column + j);
					} catch (OutOfBoardException) {
						continue;
					}

					if (this [currentTo] == null && !canJump.Contains (currentTo)) {
						canJump.Add (currentTo);
					}
				}
            
			canJump.Remove (from);
			return canJump;
		}

		/// <summary>
		/// Можно ли сделать указанный ход
		/// </summary>
		/// <param name="from">откуда</param>
		/// <param name="to">куда</param>
		/// <returns><c>true</c>, если ход возможен. Иначе <c>false</c>.</returns>
		public bool CanJumpTo (Coord from, Coord to)
		{
			return CorrectJumps (from).Contains (to);
		}

		/// <summary>
		/// Множество координат фигур указанного игрока
		/// </summary>
		/// <param name='player'>
		/// Игрок
		/// </param>
		/// <returns>Множество координат</returns>
		public HashSet<Coord> Pieces(Player player) {
			HashSet<Coord> coords = new HashSet<Coord> ();  
			for (int i=0; i < 8; i++)
				for (int j=0; j < 8; j++) {
					if (board [i, j] == player)
						coords.Add (new Coord (i, j));
				}
			return coords;
		}
	}
}