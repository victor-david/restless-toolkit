using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Restless.Toolkit.Controls
{
    public class TabItem : System.Windows.Controls.TabItem
    {
        private double tabHeightIncrease;

        public TabItem()
        {
            Panel.SetZIndex(this, 1);
            TabHeightIncrease = TabControl.DefaultTabHeightIncrease;
        }

        static TabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata(typeof(TabItem)));
        }

        public double TabHeightIncrease 
        {
            get => tabHeightIncrease; 
            internal set
            {
                tabHeightIncrease = value;
                Margin = new Thickness(0, tabHeightIncrease, 0, 0);
            }
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            if (GetParent() is TabControl parent)
            {
                double parentVal = -parent.BorderThickness.Left;
                double left = parentVal;
                if (parent.ItemContainerGenerator.ContainerFromIndex(0) == this)
                {
                    left = 0;
                }

                Margin = new Thickness(left, 0, parentVal, parentVal - 1);
                Panel.SetZIndex(this, 2);
            }
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            Margin = new Thickness(0, tabHeightIncrease, 0, 0);
            Panel.SetZIndex(this, 1);
        }


        private TabControl GetParent()
        {
            return ItemsControl.ItemsControlFromItemContainer(this) as TabControl;
        }
    }
}
