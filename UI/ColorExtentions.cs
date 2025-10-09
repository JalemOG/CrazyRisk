using System;
using System.Drawing;

namespace CrazyRisk.UI
{
    internal static class ColorExtensions
    {
        public static Color FromConsoleColor(ConsoleColor cc) => cc switch
        {
            ConsoleColor.Black => Color.Black,
            ConsoleColor.DarkBlue => Color.DarkBlue,
            ConsoleColor.DarkGreen => Color.DarkGreen,
            ConsoleColor.DarkCyan => Color.DarkCyan,
            ConsoleColor.DarkRed => Color.DarkRed,
            ConsoleColor.DarkMagenta => Color.DarkMagenta,
            ConsoleColor.DarkYellow => Color.Olive,
            ConsoleColor.Gray => Color.Gray,
            ConsoleColor.DarkGray => Color.DarkGray,
            ConsoleColor.Blue => Color.RoyalBlue,
            ConsoleColor.Green => Color.SeaGreen,
            ConsoleColor.Cyan => Color.CadetBlue,
            ConsoleColor.Red => Color.IndianRed,
            ConsoleColor.Magenta => Color.MediumVioletRed,
            ConsoleColor.Yellow => Color.Goldenrod,
            ConsoleColor.White => Color.White,
            _ => Color.SlateGray
        };
    }
}
