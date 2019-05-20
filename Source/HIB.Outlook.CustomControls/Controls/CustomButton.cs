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
    //  [TemplatePart(Name = "", Type = typeof(Image))]
    public class CustomButton : Button
    {
        public static readonly DependencyProperty BackgroundImageProperty = DependencyProperty.Register("BackgroundImage", typeof(ImageSource), typeof(CustomButton), new PropertyMetadata(default(ImageSource), OnBackgroundImageChanged));
        static CustomButton()
        {
            // DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomButton), new FrameworkPropertyMetadata(typeof(CustomButton)));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomButton), new FrameworkPropertyMetadata(typeof(CustomButton)));
        }

        public ImageSource BackgroundImage
        {
            get { return (ImageSource)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundImage.  This enables animation, styling, binding, etc...




        public ImageSource DisabledBackgroundImage
        {
            get { return (ImageSource)GetValue(DisabledBackgroundImageProperty); }
            set { SetValue(DisabledBackgroundImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisabledBackgroundImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisabledBackgroundImageProperty =
            DependencyProperty.Register("DisabledBackgroundImage", typeof(ImageSource), typeof(CustomButton), new PropertyMetadata(default(ImageSource)));



        private static void OnBackgroundImageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }

    }
}
