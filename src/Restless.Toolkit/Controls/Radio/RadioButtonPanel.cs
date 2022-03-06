using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Provides a panel to hold <see cref="RadioButton"/> objects
    /// and enable one central binding for values assigned to each.
    /// </summary>
    public class RadioButtonPanel : Grid
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButtonPanel"/> class.
        /// </summary>
        public RadioButtonPanel()
        {
            AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(CheckedEventHandler));
        }
        #endregion

        /************************************************************************/

        #region SelectedValue
        /// <summary>
        /// Gets or sets the selected value
        /// </summary>
        public int SelectedValue
        {
            get => (int)GetValue(SelectedValueProperty);
            set => SetValue(SelectedValueProperty, value);
        }
        
        /// <summary>
        /// Identifies the <see cref="SelectedValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedValueProperty =  DependencyProperty.Register
            (
                nameof(SelectedValue), typeof(int), typeof(RadioButtonPanel), new FrameworkPropertyMetadata()
                {
                    DefaultValue = 0,
                    PropertyChangedCallback = OnSelectedValueChanged,
                    BindsTwoWayByDefault = true,
                }
            );

        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as RadioButtonPanel)?.UpdateSelectedChild();
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Called when the initialization process is complete
        /// </summary>
        public override void EndInit()
        {
            UpdateSelectedChild();
            base.EndInit();
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void CheckedEventHandler(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is RadioButton button)
            {
                SelectedValue = button.Value;
                e.Handled = true;
            }
        }

        private void UpdateSelectedChild()
        {
            foreach (RadioButton child in Children.OfType<RadioButton>())
            {
                child.IsChecked = child.Value == SelectedValue;
            }
        }
        #endregion
    }
}