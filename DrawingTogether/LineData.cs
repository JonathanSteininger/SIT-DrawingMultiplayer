using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DrawingTogether
{
    public class LineData
    {


        private class LinePortion : IComparable
        {
            public long Tick;
            public string Owner;
            public bool IsVisible;

            public long UpdateTick;

            public Color Color;
            private Point _startPos;
            private Point _endPos;

            public Point StartPos { get { return _startPos; } }
            public Point EndPos { get { return _endPos; } }


            public LinePortion(long tick, string OwnerName, Color LineColor, Point start, Point end)
            {
                Tick = tick;
                Owner = OwnerName;
                Color = LineColor;

                _startPos = start;
                _endPos = end;

                //defaults
                IsVisible = true;
            }
            public LinePortion(long tick, string OwnerName, Color LineColor, int StartXpos, int StartYpos, int EndXpos, int EndYpos) 
            {
                Tick = tick;
                Owner = OwnerName;
                Color = LineColor;

                _startPos = new Point(StartXpos, StartYpos);
                _endPos = new Point(EndXpos, EndYpos);

                //defaults
                IsVisible = true;
            }
            /// <summary>
            /// Checks if two lines are equal.
            /// if they are the same, checks for any updates with the lines.
            /// updates current line if out of date.
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentNullException"></exception>
            public override bool Equals(object obj)
            {
                if(obj == null) throw new ArgumentNullException(nameof(obj));
                if (!(obj is LinePortion)) return false;
                LinePortion other = (LinePortion)obj;
                bool temp = (StartPos, EndPos, Owner, Tick) == (other.StartPos, other.EndPos, other.Owner, other.Tick);
                if (temp) CheckUpdate(other);
                return temp;
            }
            /// <summary>
            /// Checks if other Lineportion is newer.
            /// if so, updates current instances info to newer version.
            /// </summary>
            /// <param name="other"></param>
            private void CheckUpdate(LinePortion other)
            {
                if (UpdateTick >= other.UpdateTick) return;
                Color = other.Color;
                UpdateTick = other.UpdateTick;
                IsVisible = other.IsVisible;
            }
            /// <summary>
            /// used to be sorted in a list
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(object obj)
            {
                if (!(obj is LinePortion)) return -1;
                return Tick.CompareTo((obj as LinePortion).Tick);
            }

            public override int GetHashCode()
            {
                string HashString = $"{Tick} {Owner} {StartPos} {EndPos}";
                return HashString.GetHashCode();
            }
        }
    }
}
