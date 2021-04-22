using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    internal class MainPickerAdorner : Adorner
    {
        #region Private
        private static readonly Brush FillBrush = Brushes.Red;
        private static readonly Pen OutlinePen = new Pen(Brushes.Black, 1);
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPickerAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The adorned element</param>
        public MainPickerAdorner(UIElement adornedElement) : base(adornedElement)
        {
            IsHitTestVisible = false;
        }
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets or sets the adorner position
        /// </summary>
        public Point Position
        {
            get => (Point)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Position"/> dependency property
        /// </summary>
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register
            (
                nameof(Position), typeof(Point), typeof(MainPickerAdorner), new FrameworkPropertyMetadata()
                {
                    DefaultValue = new Point(),
                    AffectsRender = true,
                }
            );
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called when the adorner needs to render
        /// </summary>
        /// <param name="drawingContext">The drawing context</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Rect rect = new Rect(Position.X - 5, Position.Y - 5, 10, 10);
            drawingContext.DrawRectangle(FillBrush, OutlinePen, rect);
        }
        #endregion
    }
}