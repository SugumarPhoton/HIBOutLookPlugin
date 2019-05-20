using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CustomControls
{
   // [TemplatePart(Name = "BD", Type = typeof(Image))]
    public class CustomRadioButton : RadioButton
    {
        //public static Image _uncheckedImage = null;
        //public static BulletDecorator _checkedImage = null;
        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //    _checkedImage = this.GetTemplateChild("BD") as BulletDecorator;
        //}
        static CustomRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomRadioButton), new PropertyMetadata(typeof(CustomRadioButton)));
        }
        public ImageSource UnCheckedImagePath
        {
            get { return (ImageSource)GetValue(UnCheckedImagePathProperty); }
            set { SetValue(UnCheckedImagePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UnCheckedImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnCheckedImagePathProperty = DependencyProperty.Register("UnCheckedImagePath", typeof(ImageSource), typeof(CustomRadioButton), new PropertyMetadata(default(ImageSource), OnUnCheckedPropertyChanged));



        public ImageSource CheckedImagePath
        {
            get { return (ImageSource)GetValue(CheckedImagePathProperty); }
            set { SetValue(CheckedImagePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckedImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckedImagePathProperty = DependencyProperty.Register("CheckedImagePath", typeof(ImageSource), typeof(CustomRadioButton), new PropertyMetadata(default(ImageSource), OnCheckedPropertyChanged));


        private static void OnCheckedPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            //  Par_border.Background = new ImageBrush()
            // var br = Par_border;
        }
        private static void OnUnCheckedPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            //  Par_border.Background = new ImageBrush()
            // var br = Par_border;
        }
    }
}
