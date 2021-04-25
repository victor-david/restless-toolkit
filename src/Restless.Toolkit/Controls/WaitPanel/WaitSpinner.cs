using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents a wait spinner.
    /// </summary>
    [TemplatePart(Name = PartCanvas, Type = typeof(Canvas))]
    public class WaitSpinner : Control
    {
        #region Private
        private const string PartCanvas = "PART_Canvas";
        private const int EllipseCount = 9;
        private Canvas canvas;
        private RotateTransform rotateTransform;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WaitSpinner"/> class.
        /// </summary>
        public WaitSpinner()
        {
        }

        static WaitSpinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WaitSpinner), new FrameworkPropertyMetadata(typeof(WaitSpinner)));
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets or sets the brush used on the spinner circles.
        /// </summary>
        internal Brush SpinnerBush
        {
            get => (Brush)GetValue(SpinnerBushProperty);
            set => SetValue(SpinnerBushProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SpinnerBush"/> dependency property.
        /// </summary>
        internal static readonly DependencyProperty SpinnerBushProperty = DependencyProperty.Register
            (
                nameof(SpinnerBush), typeof(Brush), typeof(WaitSpinner), new PropertyMetadata()
                {
                    DefaultValue = Brushes.Red
                }
            );
        #endregion

        /************************************************************************/

        #region Public methods
        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            canvas = GetTemplateChild(PartCanvas) as Canvas ?? throw new NotImplementedException("WaitSpinner template");
            rotateTransform = new RotateTransform();
            canvas.RenderTransform = rotateTransform;
            InitializePositions();
            InitializeAnimation();
        }
        #endregion


        /************************************************************************/

        #region Private methods
        private void InitializePositions()
        {
            if (canvas.Children.Count == EllipseCount)
            {
                SetPosition(canvas.Children[0], 0.0);
                SetPosition(canvas.Children[1], 1.0);
                SetPosition(canvas.Children[2], 2.0);
                SetPosition(canvas.Children[3], 3.0);
                SetPosition(canvas.Children[4], 4.0);
                SetPosition(canvas.Children[5], 5.0);
                SetPosition(canvas.Children[6], 6.0);
                SetPosition(canvas.Children[7], 7.0);
                SetPosition(canvas.Children[8], 8.0);
            }
        }

        private void SetPosition(DependencyObject obj, double posOffSet)
        {
            double step = Math.PI * 2 / 10.0;
            obj.SetValue(Canvas.LeftProperty, 50 + (Math.Sin(Math.PI + (posOffSet * step)) * 50));
            obj.SetValue(Canvas.TopProperty, 50 + (Math.Cos(Math.PI + (posOffSet * step)) * 50));
        }

        private void InitializeAnimation()
        {
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 0.0,
                To = 360.0,
                Duration = new Duration(TimeSpan.FromMilliseconds(1250)),
                RepeatBehavior = RepeatBehavior.Forever
            };

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, animation);
        }
        #endregion
    }
}