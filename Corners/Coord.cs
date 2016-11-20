using System;

namespace Corners
{
	/// <summary>
	/// Координата за пределами доски
	/// </summary>
	public class OutOfBoardException : Exception
	{
	}

	/// <summary>
	/// Координата на доске
	/// </summary>
	public class Coord
	{
		/// <summary>
		/// Строка
		/// </summary>
		int row;
		/// <summary>
		/// Столбец
		/// </summary>
		int column;

		/// <summary>
		/// Заполняет координаты на основе строки вида "a1" или "c4"
		/// </summary>
		/// <param name="s">Строка с координатами</param>
		public Coord (string s)
		{
			s = s.ToLower ().Trim ();

			int r = s[1] - '1',
				c = s[0] - 'a';

			if (r < 0 || r > 7 || c < 0 || c > 7)
				throw new OutOfBoardException ();

			row = r;
			column = c;
		}

		/// <summary>
		/// Заполняет координаты парой указанных чисел
		/// </summary>
		/// <param name='r'>
		/// Строка
		/// </param>
		/// <param name='c'>
		/// Столбец
		/// </param>
		public Coord (int r, int c)
		{
			if (r < 0 || r > 7 || c < 0 || c > 7)
				throw new OutOfBoardException ();
			row = r;
			column = c;
		}

		/// <summary>
		/// Номер строки
		/// </summary>
		/// <value>
		/// Номер строки
		/// </value>
		public int Row {
			get {
				return row;
			}
			set {
				if (value < 0 || value > 7)
					throw new OutOfBoardException ();
				row = value;
			}
		}

		/// <summary>
		/// Номер столбца
		/// </summary>
		/// <value>
		/// Номер столбца
		/// </value>
		public int Column {
			get {
				return column;
			}
			set {
				if (value < 0 || value > 7)
					throw new OutOfBoardException ();
				column = value;
			}
		}

		/// <summary>
		/// Расстояние до другой клетки доски
		/// </summary>
		/// <param name='c'>
		/// Координаты другой клетки
		/// </param>
		public double Distance (Coord coord)
		{
			return Math.Sqrt (Math.Pow (row - coord.row, 2) + Math.Pow (column - coord.column, 2));
		}

		/// <summary>
		/// Возвращает строковое представление координат
		/// </summary>
		/// <returns>
		/// Строка, представляющая координаты
		/// </returns>
		public override string ToString ()
		{
			return string.Format ("{0}{1}", (char)(column + 'a'), row + 1);
		}

		/// <summary>
		/// Сравнивает с другой координатой
		/// </summary>
		/// <param name='obj'>
		/// Объект, соответствующий другой координате
		/// </param>
		/// <returns>
		/// <c>true</c>, если координаты совпадают. Иначе <c>false</c>.
		/// </returns>
		public override bool Equals (object obj)
		{
			var c = (Coord)obj;
			return (row == c.row) && (column == c.column);
		}

		/// <summary>
		/// Возвращает хеш-код — уникальное (в идеале) число, соответствующее паре координат.
		/// </summary>
		/// <returns>
		/// Хеш-код.
		/// </returns>
		public override int GetHashCode ()
		{
			return row * 8 + column;
		}

		/// <summary>
		/// Проверка на равенство
		/// </summary>
		public static bool operator== (Coord coord1, Coord coord2)
		{
			return coord1.Equals(coord2);
		}

		/// <summary>
		/// Проверка на неравенство
		/// </summary>
		public static bool operator!= (Coord coord1, Coord coord2)
		{
			return !coord1.Equals(coord2);
		}
	}
}