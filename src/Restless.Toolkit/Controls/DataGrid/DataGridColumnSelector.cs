﻿using Restless.Toolkit.Core;
using Restless.Toolkit.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Restless.Toolkit.Controls
{
    /// <summary>
    /// Represents the popup control used to make column selections
    /// </summary>
    public class DataGridColumnSelector : Popup
    {
        #region Private
        private ObservableCollection<DataGridColumn> columns;
        private readonly Panel innerChild;
        private Thickness checkBoxMargin;
        private int minimumVisible;
        private readonly Separator separator;
        private readonly LinkedTextBlock linkedTextBlock;
        private string resetText;

        #endregion

        /************************************************************************/

        #region Fields / Properties
        /// <summary>
        /// Gets the default background
        /// </summary>
        public static readonly Brush DefaultBackground = Brushes.White;

        /// <summary>
        /// Gets the default border brush
        /// </summary>
        public static readonly Brush DefaultBorderBrush = Brushes.DimGray;

        /// <summary>
        /// Gets the default border thickness
        /// </summary>
        public static readonly Thickness DefaultBorderThickness = new Thickness(1);

        /// <summary>
        /// Gets the minimum uniform padding
        /// </summary>
        public const double MinimumUniformPadding = 3;

        /// <summary>
        /// Gets the maximum uniform padding
        /// </summary>
        public const double MaximumUniformPadding = 12;

        /// <summary>
        /// Gets the default uniform padding
        /// </summary>
        public const double DefaultUniformPadding = 7;

        /// <summary>
        /// Gets the default check box margin
        /// </summary>
        public static readonly Thickness DefaultCheckBoxMargin = new Thickness(2, 4, 2, 4);

        /// <summary>
        /// Gets the default minimum number of visible columns
        /// </summary>
        public const int DefaultMinimumVisible = 1;

        /// <summary>
        /// Gets the default text for the reset link
        /// </summary>
        public const string DefaultResetText = "Reset";

        /// <summary>
        /// Gets or sets the background
        /// </summary>
        public Brush Background
        {
            get => (Child as Border).Background;
            set => (Child as Border).Background = value;
        }

        /// <summary>
        /// gets or sets the border brush
        /// </summary>
        public Brush BorderBrush
        {
            get => (Child as Border).BorderBrush;
            set => (Child as Border).BorderBrush = value;
        }

        /// <summary>
        /// Gets or sets the border thickness
        /// </summary>
        public Thickness BorderThickness
        {
            get => (Child as Border).BorderThickness;
            set => (Child as Border).BorderThickness = value;
        }

        /// <summary>
        /// Gets or sets the uniform padding
        /// </summary>
        public double UniformPadding
        {
            get => (Child as Border).Padding.Left;
            set => UpdateUniformPadding(value);
        }

        /// <summary>
        /// Gets or sets the check box margin
        /// </summary>
        public Thickness CheckBoxMargin
        {
            get => checkBoxMargin;
            set => UpdateCheckBoxMargin(value);
        }

        /// <summary>
        /// Gets or sets the minimum number of visible columns
        /// </summary>
        public int MinimumVisible
        {
            get => minimumVisible;
            set => minimumVisible = Math.Max(value, 1);
        }

        /// <summary>
        /// Gets or sets the text used for the reset link
        /// </summary>
        public string ResetText
        {
            get => resetText;
            set => UpdateResetText(value);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridColumnSelector"/> class
        /// </summary>
        public DataGridColumnSelector()
        {
            PopupAnimation = PopupAnimation.Fade;
            StaysOpen = false;
            HorizontalOffset = 4;
            VerticalOffset = -4;
            IsOpen = false;
            AllowsTransparency = true;
            MinimumVisible = DefaultMinimumVisible;
            innerChild = new StackPanel();

            Child = new Border()
            {
                Background = DefaultBackground,
                BorderBrush = DefaultBorderBrush,
                BorderThickness = DefaultBorderThickness,
                Padding = new Thickness(DefaultUniformPadding, DefaultUniformPadding, DefaultUniformPadding, 0),
                Child = innerChild
            };

            /* these get added when Show() gets called */
            separator = new Separator()
            {
                Background = BorderBrush,
                Margin = new Thickness(0 - DefaultUniformPadding, DefaultUniformPadding, 0 - DefaultUniformPadding, 4)
            };

            linkedTextBlock = new LinkedTextBlock()
            {
                Text = DefaultResetText,
                Margin = new Thickness(0, 2, 0, 6),
                Command = RelayCommand.Create(RunResetCommand)
            };

            /* defaults */
            CheckBoxMargin = DefaultCheckBoxMargin;
            ResetText = DefaultResetText;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Shows the columns selector
        /// </summary>
        /// <param name="header">The header used as the placement target for the popup</param>
        /// <param name="columns">The columns</param>
        public void Show(DataGridColumnHeader header, ObservableCollection<DataGridColumn> columns)
        {
            this.columns = columns ?? throw new ArgumentNullException(nameof(columns));
            PlacementTarget = header ?? throw new ArgumentNullException(nameof(header));
            if (columns.Count != innerChild.Children.Count)
            {
                innerChild.Children.Clear();
                CreateColumnSelections();
                CreateResetLink();
            }
            IsOpen = true;
        }
        #endregion

        internal event EventHandler SelectionChanged;

        /************************************************************************/

        #region Private methods
        private void UpdateCheckBoxMargin(Thickness value)
        {
            checkBoxMargin = value;
            foreach (CheckBox checkBox in innerChild.Children.OfType<CheckBox>())
            {
                checkBox.Margin = value;
            }
        }

        private void UpdateUniformPadding(double value)
        {
            value = Math.Min(Math.Max(value, MinimumUniformPadding), MaximumUniformPadding);
            (Child as Border).Padding = new Thickness(value, value, value, 0);
            separator.Margin = new Thickness(0 - value, value, 0 - value, 4);
        }

        private void UpdateResetText(string value)
        {
            resetText = string.IsNullOrWhiteSpace(value) ? DefaultResetText : value;
            linkedTextBlock.Text = resetText;
        }

        private void CreateColumnSelections()
        {
            foreach (DataGridColumn column in columns)
            {
                innerChild.Children.Add(CreateCheckBox(column));
            }
        }

        private CheckBox CreateCheckBox(DataGridColumn column)
        {
            CheckBox checkBox = new CheckBox()
            {
                Content = column.GetValue(DataGridColumns.SelectorNameProperty) ?? column.SortMemberPath,
                Tag = column,
                Margin = CheckBoxMargin,
                IsChecked = column.Visibility == Visibility.Visible,
            };

            checkBox.Command = RelayCommand.Create(RunCheckBoxCommand, null, checkBox);
            return checkBox;
        }

        private void RunCheckBoxCommand(object parm)
        {
            if (parm is CheckBox checkBox && checkBox.Tag is DataGridColumn column)
            {
                if (checkBox.IsChecked ?? false)
                {
                    column.Visibility = Visibility.Visible;
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    TryHideColumn(checkBox, column);
                }
            }
        }

        private void TryHideColumn(CheckBox checkBox, DataGridColumn column)
        {
            if (GetVisibleColumnCount() > MinimumVisible)
            {
                /* can hide */
                column.Visibility = Visibility.Collapsed;

                if (IsColumnHeaderPlacementTarget(column))
                {
                    PlacementTarget = GetFirstVisibleColumnHeader();
                }
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                checkBox.IsChecked = true;
            }
        }

        private bool IsColumnHeaderPlacementTarget(DataGridColumn column)
        {
            return GetColumnHeaderFromColumn(column) == PlacementTarget;
        }

        private DataGridColumnHeader GetFirstVisibleColumnHeader()
        {
            foreach (DataGridColumn column in columns)
            {
                if (column.Visibility == Visibility.Visible)
                {
                    return GetColumnHeaderFromColumn(column);
                }
            }
            return null;
        }

        private DataGridColumnHeader GetColumnHeaderFromColumn(DataGridColumn column)
        {
            if (column.Header is DependencyObject dp)
            {
                if (dp is DataGridColumnHeader header1)
                {
                    return header1;
                }

                if (CoreHelper.GetVisualParent<DataGridColumnHeader>(dp) is DataGridColumnHeader header2)
                {
                    return header2;
                }
            }
            return null;
        }

        private int GetVisibleColumnCount()
        {
            int count = 0;
            foreach (DataGridColumn column in columns)
            {
                if (column.Visibility == Visibility.Visible)
                {
                    count++;
                }
            }
            return count;
        }

        private void CreateResetLink()
        {
            innerChild.Children.Add(separator);
            innerChild.Children.Add(linkedTextBlock);
        }

        private void RunResetCommand(object parm)
        {
            int displayIndex = 0;
            foreach (DataGridColumn column in columns)
            {
                column.Visibility = Visibility.Visible;
                column.DisplayIndex = displayIndex++;
            }

            foreach (CheckBox checkBox in innerChild.Children.OfType<CheckBox>())
            {
                checkBox.IsChecked = true;
            }
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}