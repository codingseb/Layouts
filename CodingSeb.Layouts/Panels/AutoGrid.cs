//*****************************************************************************************************
// Copied from  : https://github.com/SpicyTaco/SpicyTaco.AutoGrid
// Licence    : MIT (https://github.com/SpicyTaco/SpicyTaco.AutoGrid/blob/master/license)
// Refactored : CodingSeb
// Remark     : Include in this project to make it accessible without namespace
// ----------------------------------------------------------------------------------------------------
// | 24.09.2020 | CodingSeb | Correction of cellskips when multiple consecutive RowSpan or ColumnSpan |
// ----------------------------------------------------------------------------------------------------
// | 13.04.2021 | CodingSeb | Add Management for RowsMinCount, ColumnsMinCount, RowsSharedSizeGroups  |
// |            |           | and ColumnsSharedSizeGroups                                             |
// ----------------------------------------------------------------------------------------------------
//*****************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CodingSeb.Layouts
{
    /// <summary>
    /// <para>
    /// Defines a flexible grid area that consists of columns and rows.
    /// Depending on the orientation, either the rows or the columns are auto-generated,
    /// and the children's position is set according to their index.
    /// </para>
    /// <para>Partially based on work at http://rachel53461.wordpress.com/2011/09/17/wpf-grids-rowcolumn-count-properties/</para>
    /// </summary>
    public class AutoGrid : Grid
    {
        /// <summary>
        /// Gets or sets the child horizontal alignment.
        /// </summary>
        /// <value>The child horizontal alignment.</value>
        [Category("Layout"), Description("Presets the horizontal alignment of all child controls")]
        public HorizontalAlignment? ChildHorizontalAlignment
        {
            get { return (HorizontalAlignment?)GetValue(ChildHorizontalAlignmentProperty); }
            set { SetValue(ChildHorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the child margin.
        /// </summary>
        /// <value>The child margin.</value>
        [Category("Layout"), Description("Presets the margin of all child controls")]
        public Thickness? ChildMargin
        {
            get { return (Thickness?)GetValue(ChildMarginProperty); }
            set { SetValue(ChildMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the child vertical alignment.
        /// </summary>
        /// <value>The child vertical alignment.</value>
        [Category("Layout"), Description("Presets the vertical alignment of all child controls")]
        public VerticalAlignment? ChildVerticalAlignment
        {
            get { return (VerticalAlignment?)GetValue(ChildVerticalAlignmentProperty); }
            set { SetValue(ChildVerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the columns
        /// </summary>
        [Category("Layout"), Description("Defines all columns using comma separated grid length notation")]
        public string Columns
        {
            get { return (string)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum columns count to create in the grid.
        /// Dfault value is 1
        /// </summary>
        [Category("Layout"), Description("Defines the minimum columns count to have in the grid")]
        public int ColumnsMinCount
        {
            get { return (int)GetValue(ColumnsMinCountProperty); }
            set { SetValue(ColumnsMinCountProperty, value); }
        }

        /// <summary>
        /// Gets or sets the columns SharedSizeGroups
        /// </summary>
        [Category("Layout"), Description("Defines all columns using comma separated SharedSizeGroups, leave empty if to not set intermediate groups")]
        public string ColumnsSharedSizeGroups
        {
            get { return (string)GetValue(ColumnsSharedSizeGroupsProperty); }
            set { SetValue(ColumnsSharedSizeGroupsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the fixed column width
        /// </summary>
        [Category("Layout"), Description("Presets the width of all columns set using the ColumnCount property")]
        public GridLength ColumnWidth
        {
            get { return (GridLength)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the children are automatically indexed.
        /// <remarks>
        /// The default is <c>true</c>.
        /// Note that if children are already indexed, setting this property to <c>false</c> will not remove their indices.
        /// </remarks>
        /// </summary>
        [Category("Layout"), Description("Set to false to disable the auto layout functionality")]
        public bool IsAutoIndexing
        {
            get { return (bool)GetValue(IsAutoIndexingProperty); }
            set { SetValue(IsAutoIndexingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the orientation.
        /// <remarks>The default is Vertical.</remarks>
        /// </summary>
        /// <value>The orientation.</value>
        [Category("Layout"),
            Description(
                "Defines the directionality of the autolayout. Use vertical for a column first layout, horizontal for a row first layout."
                )]
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the fixed row height
        /// </summary>
        [Category("Layout"), Description("Presets the height of all rows set using the RowCount property")]
        public GridLength RowHeight
        {
            get { return (GridLength)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the rows
        /// </summary>
        [Category("Layout"), Description("Defines all rows using comma separated grid length notation")]
        public string Rows
        {
            get { return (string)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum rows count to create in the grid.
        /// Default value is 1
        /// </summary>
        [Category("Layout"), Description("Defines the minimum rows count to have in the grid")]
        public int RowsMinCount
        {
            get { return (int)GetValue(RowsMinCountProperty); }
            set { SetValue(RowsMinCountProperty, value); }
        }

        /// <summary>
        /// Gets or sets the rows SharedSizeGroups
        /// </summary>
        [Category("Layout"), Description("Defines all rows using comma separated SharedSizeGroups, leave empty if to not set intermediate groups")]
        public string RowsSharedSizeGroups
        {
            get { return (string)GetValue(RowsSharedSizeGroupsProperty); }
            set { SetValue(RowsSharedSizeGroupsProperty, value); }
        }

        // AutoIndex attached property

        public static readonly DependencyProperty AutoIndexProperty = DependencyProperty.RegisterAttached(
            "AutoIndex", typeof(bool), typeof(AutoGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static void SetAutoIndex(DependencyObject element, bool value)
        {
            element.SetValue(AutoIndexProperty, value);
        }

        public static bool GetAutoIndex(DependencyObject element)
        {
            return (bool)element.GetValue(AutoIndexProperty);
        }

        // RowHeight override attached property

        public static readonly DependencyProperty RowHeightOverrideProperty = DependencyProperty.RegisterAttached(
            "RowHeightOverride", typeof(GridLength?), typeof(AutoGrid),
            new FrameworkPropertyMetadata(default(GridLength?), FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static void SetRowHeightOverride(DependencyObject element, GridLength? value)
        {
            element.SetValue(RowHeightOverrideProperty, value);
        }

        public static GridLength? GetRowHeightOverride(DependencyObject element)
        {
            return (GridLength?)element.GetValue(RowHeightOverrideProperty);
        }

        public static readonly DependencyProperty ColumnWidthOverrideProperty = DependencyProperty.RegisterAttached(
            "ColumnWidthOverride", typeof(GridLength?), typeof(AutoGrid),
            new FrameworkPropertyMetadata(default(GridLength?), FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static void SetColumnWidthOverride(DependencyObject element, GridLength? value)
        {
            element.SetValue(ColumnWidthOverrideProperty, value);
        }

        public static GridLength? GetColumnWidthOverride(DependencyObject element)
        {
            return (GridLength?)element.GetValue(ColumnWidthOverrideProperty);
        }

        /// <summary>
        /// Handle the columns changed event
        /// </summary>
        public static void ColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AutoGrid grid)
            {
                grid.RefreshColumns();
                grid.RefreshRows();
            }
        }

        protected void RefreshColumns()
        {
            ColumnDefinitions.Clear();

            GridLength[] widths = Parse(Columns, ColumnWidth);
            string[] groups = ColumnsSharedSizeGroups.Split(',');

            for (int i = 0; i < Math.Max(1, Math.Max(ColumnsMinCount, widths.Length)); i++)
            {
                var column = new ColumnDefinition()
                {
                    Width = widths.Length > i ? widths[i] : ColumnWidth,
                };

                if (groups.Length > i && !string.IsNullOrWhiteSpace(groups[i]))
                    column.SharedSizeGroup = groups[i].Trim();

                ColumnDefinitions.Add(column);
            }
        }

        /// <summary>
        /// Parse an array of grid lengths from comma delim text
        /// </summary>
        public static GridLength[] Parse(string text, GridLength defaultLength = default)
        {
            var tokens = text.Split(',');
            var definitions = new GridLength[tokens.Length];
            for (var i = 0; i < tokens.Length; i++)
            {
                var str = tokens[i];
                double value;

                // ratio
                if (str.Contains('*'))
                {
                    if (!double.TryParse(str.Replace("*", ""), out value))
                        value = 1.0;

                    definitions[i] = new GridLength(value, GridUnitType.Star);
                    continue;
                }

                // pixels
                if (double.TryParse(str, out value))
                {
                    definitions[i] = new GridLength(value);
                    continue;
                }

                // auto
                if (str.Trim().Equals("auto", StringComparison.OrdinalIgnoreCase))
                {
                    definitions[i] = GridLength.Auto;
                    continue;
                }

                definitions[i] = defaultLength;
            }
            return definitions;
        }

        /// <summary>
        /// Handle the rows changed event
        /// </summary>
        public static void RowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AutoGrid grid)
            {
                grid.RefreshRows();
                grid.RefreshColumns();
            }
        }

        protected void RefreshRows()
        {
            RowDefinitions.Clear();

            GridLength[] heights = Parse(Rows, RowHeight);
            string[] groups = RowsSharedSizeGroups.Split(',');

            for (int i = 0; i < Math.Max(1, Math.Max(RowsMinCount, heights.Length)); i++)
            {
                var row = new RowDefinition()
                {
                    Height = heights.Length > i ? heights[i] : RowHeight,
                };

                if (groups.Length > i && !string.IsNullOrWhiteSpace(groups[i]))
                    row.SharedSizeGroup = groups[i].Trim();

                RowDefinitions.Add(row);
            }
        }

        /// <summary>
        /// Called when [child horizontal alignment changed].
        /// </summary>
        private static void OnChildHorizontalAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AutoGrid grid)
            {
                foreach (UIElement child in grid.Children)
                {
                    if (grid.ChildHorizontalAlignment.HasValue)
                        child.SetValue(HorizontalAlignmentProperty, grid.ChildHorizontalAlignment);
                    else
                        child.SetValue(HorizontalAlignmentProperty, DependencyProperty.UnsetValue);
                }
            }
        }

        /// <summary>
        /// Called when [child layout changed].
        /// </summary>
        private static void OnChildMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AutoGrid grid)
            {
                foreach (UIElement child in grid.Children)
                {
                    if (grid.ChildMargin.HasValue)
                        child.SetValue(MarginProperty, grid.ChildMargin);
                    else
                        child.SetValue(MarginProperty, DependencyProperty.UnsetValue);
                }
            }
        }

        /// <summary>
        /// Called when [child vertical alignment changed].
        /// </summary>
        private static void OnChildVerticalAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AutoGrid grid)
            {
                foreach (UIElement child in grid.Children)
                {
                    if (grid.ChildVerticalAlignment.HasValue)
                        child.SetValue(VerticalAlignmentProperty, grid.ChildVerticalAlignment);
                    else
                        child.SetValue(VerticalAlignmentProperty, DependencyProperty.UnsetValue);
                }
            }
        }

        /// <summary>
        /// Handled the redraw properties changed event
        /// </summary>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AutoGrid grid)
            {
                grid.shouldReIndex = true;
            }
        }

        public void PerformLayout()
        {
            bool isVertical = Orientation == Orientation.Vertical;

            if (shouldReIndex || (IsAutoIndexing
                && ((isVertical && rowOrColumnCount != ColumnDefinitions.Count)
                || (!isVertical && rowOrColumnCount != RowDefinitions.Count))))
            {
                shouldReIndex = false;

                if (IsAutoIndexing)
                {
                    rowOrColumnCount = (isVertical) ? ColumnDefinitions.Count : RowDefinitions.Count;
                    if (rowOrColumnCount == 0) rowOrColumnCount = 1;

                    int cellCount = 0;
                    foreach (UIElement child in Children)
                    {
                        if (!GetAutoIndex(child))
                        {
                            continue;
                        }
                        cellCount += (ColumnDefinitions.Count != 1) ? GetColumnSpan(child) : GetRowSpan(child);
                    }

                    //  Update the number of rows/columns
                    if (!isVertical)
                    {
                        var newRowCount = (int)Math.Ceiling(cellCount / (double)rowOrColumnCount);
                        while (RowDefinitions.Count < newRowCount)
                        {
                            var rowDefinition = new RowDefinition
                            {
                                Height = RowHeight
                            };

                            RowDefinitions.Add(rowDefinition);
                        }
                        if (RowDefinitions.Count > newRowCount)
                        {
                            RowDefinitions.RemoveRange(newRowCount, RowDefinitions.Count - newRowCount);
                        }
                    }
                    else // rows defined
                    {
                        var newColumnCount = (int)Math.Ceiling(cellCount / (double)rowOrColumnCount);
                        while (ColumnDefinitions.Count < newColumnCount)
                        {
                            var columnDefinition = new ColumnDefinition
                            {
                                Width = ColumnWidth
                            };
                            ColumnDefinitions.Add(columnDefinition);
                        }
                        if (ColumnDefinitions.Count > newColumnCount)
                        {
                            ColumnDefinitions.RemoveRange(newColumnCount, ColumnDefinitions.Count - newColumnCount);
                        }
                    }
                }

                //  Update children indices
                int cellPosition = 0;
                var cellsToSkip = new Queue<int>();
                foreach (UIElement child in Children)
                {
                    if (IsAutoIndexing && GetAutoIndex(child))
                    {
                        while (cellsToSkip.Count > 0 && cellsToSkip.Peek() == cellPosition)
                        {
                            cellsToSkip.Dequeue();
                            cellPosition++;
                        }

                        if (!isVertical) // horizontal (default)
                        {
                            var rowIndex = cellPosition / Math.Max(ColumnDefinitions.Count, 1);
                            SetRow(child, rowIndex);

                            var columnIndex = cellPosition % ColumnDefinitions.Count;
                            SetColumn(child, columnIndex);

                            var rowSpan = GetRowSpan(child);
                            if (rowSpan > 1)
                            {
                                Enumerable.Range(1, rowSpan - 1).ToList()
                                    .ForEach(x => cellsToSkip.Enqueue(cellPosition + (ColumnDefinitions.Count * x)));
                            }

                            var overrideRowHeight = GetRowHeightOverride(child);
                            if (overrideRowHeight != null)
                            {
                                RowDefinitions[rowIndex].Height = overrideRowHeight.Value;
                            }

                            var overrideColumnWidth = GetColumnWidthOverride(child);
                            if (overrideColumnWidth != null)
                            {
                                ColumnDefinitions[columnIndex].Width = overrideColumnWidth.Value;
                            }

                            cellPosition += GetColumnSpan(child);
                        }
                        else
                        {
                            var rowIndex = cellPosition % RowDefinitions.Count;
                            SetRow(child, rowIndex);

                            var columnIndex = cellPosition / RowDefinitions.Count;
                            SetColumn(child, columnIndex);

                            var columnSpan = GetColumnSpan(child);
                            if (columnSpan > 1)
                            {
                                Enumerable.Range(1, columnSpan - 1).ToList()
                                    .ForEach(x => cellsToSkip.Enqueue(cellPosition + (RowDefinitions.Count * x)));
                            }

                            var overrideRowHeight = GetRowHeightOverride(child);
                            if (overrideRowHeight != null)
                            {
                                RowDefinitions[rowIndex].Height = overrideRowHeight.Value;
                            }

                            var overrideColumnWidth = GetColumnWidthOverride(child);
                            if (overrideColumnWidth != null)
                            {
                                ColumnDefinitions[columnIndex].Width = overrideColumnWidth.Value;
                            }

                            cellPosition += GetRowSpan(child);
                        }
                    }

                    // Set margin and alignment
                    if (ChildMargin != null)
                    {
                        child.SetIfDefault(MarginProperty, ChildMargin.Value);
                    }
                    if (ChildHorizontalAlignment != null)
                    {
                        child.SetIfDefault(HorizontalAlignmentProperty, ChildHorizontalAlignment.Value);
                    }
                    if (ChildVerticalAlignment != null)
                    {
                        child.SetIfDefault(VerticalAlignmentProperty, ChildVerticalAlignment.Value);
                    }
                }
            }
        }

        public static readonly DependencyProperty ChildHorizontalAlignmentProperty =
            DependencyProperty.Register("ChildHorizontalAlignment", typeof(HorizontalAlignment?), typeof(AutoGrid),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    new PropertyChangedCallback(OnChildHorizontalAlignmentChanged)));

        public static readonly DependencyProperty ChildMarginProperty =
            DependencyProperty.Register("ChildMargin", typeof(Thickness?), typeof(AutoGrid),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    new PropertyChangedCallback(OnChildMarginChanged)));

        public static readonly DependencyProperty ChildVerticalAlignmentProperty =
            DependencyProperty.Register("ChildVerticalAlignment", typeof(VerticalAlignment?), typeof(AutoGrid),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    new PropertyChangedCallback(OnChildVerticalAlignmentChanged)));

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.RegisterAttached("Columns", typeof(string), typeof(AutoGrid),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    new PropertyChangedCallback(ColumnsChanged)));

        // Using a DependencyProperty as the backing store for MinColumnsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsMinCountProperty =
            DependencyProperty.Register("ColumnsMinCount", typeof(int), typeof(AutoGrid),
                new FrameworkPropertyMetadata(1,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    new PropertyChangedCallback(ColumnsChanged)));

        public static readonly DependencyProperty ColumnsSharedSizeGroupsProperty =
            DependencyProperty.RegisterAttached("ColumnsSharedSizeGroups", typeof(string), typeof(AutoGrid),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    new PropertyChangedCallback(ColumnsChanged)));

        public static readonly DependencyProperty ColumnWidthProperty =
            DependencyProperty.RegisterAttached("ColumnWidth", typeof(GridLength), typeof(AutoGrid),
                new FrameworkPropertyMetadata(GridLength.Auto,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    new PropertyChangedCallback(ColumnsChanged)));

        public static readonly DependencyProperty IsAutoIndexingProperty =
            DependencyProperty.Register("IsAutoIndexing", typeof(bool), typeof(AutoGrid),
                new FrameworkPropertyMetadata(true,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    new PropertyChangedCallback(OnPropertyChanged)));

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(AutoGrid),
                new FrameworkPropertyMetadata(Orientation.Horizontal,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    new PropertyChangedCallback(OnPropertyChanged)));

        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.RegisterAttached("RowHeight", typeof(GridLength), typeof(AutoGrid),
                new FrameworkPropertyMetadata(GridLength.Auto,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    new PropertyChangedCallback(RowsChanged)));

        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.RegisterAttached("Rows", typeof(string), typeof(AutoGrid),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    new PropertyChangedCallback(RowsChanged)));

        // Using a DependencyProperty as the backing store for MinRowsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowsMinCountProperty =
            DependencyProperty.RegisterAttached("RowsMinCount", typeof(int), typeof(AutoGrid),
                new FrameworkPropertyMetadata(1,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    new PropertyChangedCallback(RowsChanged)));

        // Using a DependencyProperty as the backing store for RowsSharedSizeGroups.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowsSharedSizeGroupsProperty =
            DependencyProperty.RegisterAttached("RowsSharedSizeGroups", typeof(string), typeof(AutoGrid),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    new PropertyChangedCallback(RowsChanged)));

        private bool shouldReIndex = true;
        private int rowOrColumnCount;

        #region Overrides

        /// <summary>
        /// Measures the children of a <see cref="T:System.Windows.Controls.Grid"/> in anticipation of arranging them during the <see cref="M:ArrangeOverride"/> pass.
        /// </summary>
        /// <param name="constraint">Indicates an upper limit size that should not be exceeded.</param>
        /// <returns>
        /// 	<see cref="Size"/> that represents the required size to arrange child content.
        /// </returns>
        protected override Size MeasureOverride(Size constraint)
        {
            PerformLayout();
            return base.MeasureOverride(constraint);
        }

        /// <summary>
        /// Called when the visual children of a <see cref="Grid"/> element change.
        /// <remarks>Used to mark that the grid children have changed.</remarks>
        /// </summary>
        /// <param name="visualAdded">Identifies the visual child that's added.</param>
        /// <param name="visualRemoved">Identifies the visual child that's removed.</param>
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            shouldReIndex = true;
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        #endregion Overrides
    }
}
