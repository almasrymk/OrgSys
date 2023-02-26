using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ORG.Tools.Desktop
{
    public delegate void _Pagging(int page);
    public delegate object ClickOB(object OB);
    public delegate object SelectedOB(object OB);
    public delegate void _SelectItem(ItemControl _ItemControl);
    public class GlobalsMembers
    {
        public static Point LocationItem(int FirstTop, int TopSpace, int FirstLeft, int LeftSpace, int WidthBackGround, int HieghtItem, int WidthItem, ref int x, ref int y)
        {
            Point MyPoint = new Point();
            if (x + WidthItem + FirstLeft > WidthBackGround)
            {
                MyPoint.X = x = FirstLeft;
                if (FirstTop > y)
                    MyPoint.Y = y = y + HieghtItem + FirstTop;
                else
                    MyPoint.Y = y = y + HieghtItem + TopSpace;
            }
            else
            {
                MyPoint.X = x;
                if (FirstTop > y)
                    MyPoint.Y = FirstTop;
                else
                    MyPoint.Y = y;
            }
            x = x + WidthItem + LeftSpace;
            return MyPoint;
        }
    }
}
