using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a panl that hosts a <see cref="WaitSpinner"/> and a text message.
    /// </summary>
    public class WaitPanel : Control
    {
        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WaitPanel"/> class.
        /// </summary>
        public WaitPanel()
        {
        }

        static WaitPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WaitPanel), new FrameworkPropertyMetadata(typeof(WaitPanel)));
        }
        #endregion

        /************************************************************************/

        /// <summary>
        /// Gets or sets the text to display.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register
            (
                nameof(Text), typeof(string), typeof(WaitPanel), new PropertyMetadata()
            );

        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register
            (
                nameof(CornerRadius), typeof(CornerRadius), typeof(WaitPanel), new PropertyMetadata()
            );

        /// <summary>
        /// Gets or sets the brush used on the spinner circles.
        /// </summary>
        public Brush SpinnerBush
        {
            get => (Brush)GetValue(SpinnerBushProperty);
            set => SetValue(SpinnerBushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SpinnerBush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinnerBushProperty = DependencyProperty.Register
            (
                nameof(SpinnerBush), typeof(Brush), typeof(WaitPanel), new PropertyMetadata()
                {
                    DefaultValue = Brushes.Red
                }
            );

        /// <summary>
        /// Gets or sets the size of the spinner. Default is auto size.
        /// </summary>
        public double SpinnerSize
        {
            get => (double)GetValue(SpinnerSizeProperty);
            set => SetValue(SpinnerSizeProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SpinnerSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinnerSizeProperty = DependencyProperty.Register
            (
                nameof(SpinnerSize), typeof(double), typeof(WaitPanel), new PropertyMetadata()
                {
                    DefaultValue = double.NaN
                }
            );

        ///// <summary>
        ///// Gets or sets whether the wait indicator is active.
        ///// </summary>
        //public bool IsActive
        //{
        //    get => (bool)GetValue(IsActiveProperty);
        //    set => SetValue(IsActiveProperty, value);
        //}

        ///// <summary>
        ///// Identifies the <see cref="IsActive"/> dependency property.
        ///// </summary>
        //public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register
        //    (
        //        nameof(IsActive), typeof(bool), typeof(WaitPanel), new PropertyMetadata()
        //        {
        //            DefaultValue = false,
        //            PropertyChangedCallback = OnIsActiveChanged
        //        }
        //    );

        //private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{

        //}
    }
}
