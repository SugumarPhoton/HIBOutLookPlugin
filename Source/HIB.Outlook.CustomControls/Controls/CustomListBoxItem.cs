using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CustomControls
{
    [TemplatePart(Name = "PopupDetails", Type = typeof(Popup))]
    public class CustomListBox : ListBox
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var popup = GetTemplateChild("PopupDetails") as Popup;
        }


        //public double PopUpHorizontalOffset
        //{
        //    get { return (double)GetValue(PopUpHorizontalOffsetProperty); }
        //    set { SetValue(PopUpHorizontalOffsetProperty, value); }
        //}
        //// Using a DependencyProperty as the backing store for PopUpHorizontalOffset.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty PopUpHorizontalOffsetProperty =
        //    DependencyProperty.Register("PopUpHorizontalOffset", typeof(double), typeof(CustomListBoxItem), new PropertyMetadata(0));

        //public double PopUpVerticalOffSet
        //{
        //    get { return (double)GetValue(PopUpVerticalOffSetProperty); }
        //    set { SetValue(PopUpVerticalOffSetProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for PopUpVerticalOffSet.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty PopUpVerticalOffSetProperty =
        //    DependencyProperty.Register("PopUpVerticalOffSet", typeof(double), typeof(CustomListBoxItem), new PropertyMetadata(0));









    }
}
