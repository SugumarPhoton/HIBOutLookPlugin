using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControls
{
    [TemplatePart(Name = "Border", Type = typeof(Border))]
    public class CustomTabItemControl : TabItem
    {
       static Border Par_border = null;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Par_border = GetTemplateChild("Border") as Border;


        }

        public ImageSource InActiveBackgroundImage
        {
            get { return (ImageSource)GetValue(InActiveBackgroundImageProperty); }
            set { SetValue(InActiveBackgroundImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InActiveBackgroundImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InActiveBackgroundImageProperty =
            DependencyProperty.Register("InActiveBackgroundImage", typeof(ImageSource), typeof(CustomTabItemControl), new PropertyMetadata(default(ImageSource)));


        public ImageSource ActiveBackgroundImage
        {
            get { return (ImageSource)GetValue(ActiveBackgroundImageProperty); }
            set { SetValue(ActiveBackgroundImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveBackgroundImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveBackgroundImageProperty =
            DependencyProperty.Register("ActiveBackgroundImage", typeof(ImageSource), typeof(CustomTabItemControl), new PropertyMetadata(default(ImageSource), OnCurrentTimePropertyChanged));

        private static void OnCurrentTimePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            //  Par_border.Background = new ImageBrush()
            var br = Par_border;
        }
    }
}
