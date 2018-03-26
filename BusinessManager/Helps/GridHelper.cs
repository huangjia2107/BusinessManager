using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BusinessManager.Helps
{
    public class GridHelper
    { //请注意：可以通过propa这个快捷方式生成下面三段代码

        public static bool GetShowBorder(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowBorderProperty);
        }

        public static void SetShowBorder(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowBorderProperty, value);
        }

        public static readonly DependencyProperty ShowBorderProperty =
            DependencyProperty.RegisterAttached("ShowBorder", typeof(bool), typeof(GridHelper), new PropertyMetadata(OnShowBorderChanged));


        //这是一个事件处理程序，需要手工编写，必须是静态方法
        private static void OnShowBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            if ((bool)e.OldValue)
            {
               grid.Loaded -= (s, arg) => { };
            }
            if ((bool)e.NewValue)
            {
                grid.Loaded += (s, arg) =>
                {
                    //这种做法自动将控件移动到Border里面来
                    var controls = grid.Children;
                    var count = controls.Count;

                    for (int i = 0; i < count; i++)
                    {
                        if (controls[i] is Border)
                            return;

                        var item = controls[i] as FrameworkElement;
                        var border = new Border()
                        {
                            BorderBrush = new SolidColorBrush(Colors.LightGray),
                            BorderThickness = new Thickness(0, 0, 1, 1),
                            Padding = new Thickness(5, 0, 0, 0)
                        };

                        var row = Grid.GetRow(item);
                        var column = Grid.GetColumn(item);
                        var rowspan = Grid.GetRowSpan(item);
                        var columnspan = Grid.GetColumnSpan(item);

                        Grid.SetRow(border, row);
                        Grid.SetColumn(border, column);
                        Grid.SetRowSpan(border, rowspan);
                        Grid.SetColumnSpan(border, columnspan);

                        if (row == 0)
                        {
                            if (column == 0)
                                border.BorderThickness = new Thickness(1);
                            else
                                border.BorderThickness = new Thickness(0, 1, 1, 1);
                        }
                        else if (column == 0)
                            border.BorderThickness = new Thickness(1, 0, 1, 1);

                        if (column % 2 != 0)
                            border.Padding = new Thickness(0);

                        grid.Children.RemoveAt(i);
                        border.Child = item;
                        grid.Children.Insert(i, border);

                    }
                };
            }
        }
    }
}
