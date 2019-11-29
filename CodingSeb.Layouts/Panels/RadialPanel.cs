using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CodingSeb.Layouts
{
    /// <summary>
    /// This panel allow to arranges WPF UIElements in radial coordinates
    /// It works with radius and angle in place of x y coordinates
    /// </summary>
    public class RadialPanel : Panel
    {
        private const double degreesToRadiansFactor = Math.PI / 180d;

        /// <summary>
        /// The base angle in degrees to start the positionning
        /// </summary>
        public double BaseAngle
        {
            get { return (double)GetValue(BaseAngleProperty); }
            set { SetValue(BaseAngleProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for BaseAngle. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty BaseAngleProperty =
            DependencyProperty.Register("BaseAngle", typeof(double), typeof(RadialPanel), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsArrange));

        public bool Clockwise
        {
            get { return (bool)GetValue(ClockwiseProperty); }
            set { SetValue(ClockwiseProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Clockwise.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClockwiseProperty =
            DependencyProperty.Register("Clockwise", typeof(bool), typeof(RadialPanel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// The x radius
        /// </summary>
        public double RadiusX
        {
            get { return (double)GetValue(RadiusXProperty); }
            set { SetValue(RadiusXProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for RadiusX.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty RadiusXProperty =
            DependencyProperty.Register("RadiusX", typeof(double), typeof(RadialPanel), new FrameworkPropertyMetadata(50d, FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// The y radius
        /// </summary>
        public double RadiusY
        {
            get { return (double)GetValue(RadiusYProperty); }
            set { SetValue(RadiusYProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for RadiusY.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty RadiusYProperty =
            DependencyProperty.Register("RadiusY", typeof(double), typeof(RadialPanel), new FrameworkPropertyMetadata(50d, FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Specify the way to use the 2 radius X and Y
        /// By default : SmallestRadius
        /// </summary>
        public RadialPanelRadiusUse RadiusUse
        {
            get { return (RadialPanelRadiusUse)GetValue(RadiusUseProperty); }
            set { SetValue(RadiusUseProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for RadiusUse.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty RadiusUseProperty =
            DependencyProperty.Register("RadiusUse", typeof(RadialPanelRadiusUse), typeof(RadialPanel), new FrameworkPropertyMetadata(RadialPanelRadiusUse.SmallestRadius, FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Define the way (unit in which) the RadiusX is calculated.
        /// By default : PercentageOfPanelSize
        /// </summary>
        public RadialPanelDistancesUnit RadiusXUnit
        {
            get { return (RadialPanelDistancesUnit)GetValue(RadiusXUnitProperty); }
            set { SetValue(RadiusXUnitProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for RadiusXUnit.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty RadiusXUnitProperty =
            DependencyProperty.Register("RadiusXUnit", typeof(RadialPanelDistancesUnit), typeof(RadialPanel)
                , new FrameworkPropertyMetadata(RadialPanelDistancesUnit.PercentageOfPanelSize, FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Define the way (unit in which) the RadiusY is calculated.
        /// By default : PercentageOfPanelSize
        /// </summary>
        public RadialPanelDistancesUnit RadiusYUnit
        {
            get { return (RadialPanelDistancesUnit)GetValue(RadiusYUnitProperty); }
            set { SetValue(RadiusYUnitProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for RadiusYUnit.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty RadiusYUnitProperty =
            DependencyProperty.Register("RadiusYUnit", typeof(RadialPanelDistancesUnit), typeof(RadialPanel)
                , new FrameworkPropertyMetadata(RadialPanelDistancesUnit.PercentageOfPanelSize, FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Define the way Children elements rotate on them self basing on their radial positioning angle
        /// </summary>
        public RadialPanelChildrenAngleFollowing ChildrenAngleFollowing
        {
            get { return (RadialPanelChildrenAngleFollowing)GetValue(ChildrenAngleFollowingProperty); }
            set { SetValue(ChildrenAngleFollowingProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for ChildrenAngleFollowing.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty ChildrenAngleFollowingProperty =
            DependencyProperty.Register("ChildrenAngleFollowing", typeof(RadialPanelChildrenAngleFollowing), typeof(RadialPanel), new FrameworkPropertyMetadata(RadialPanelChildrenAngleFollowing.None, FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// An offset on the x coordinate of the radial origin (starting at panel width / 2.0).
        /// By default 0
        /// </summary>
        public double CenterOffsetX
        {
            get { return (double)GetValue(CenterOffsetXProperty); }
            set { SetValue(CenterOffsetXProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for CenterOffsetX.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty CenterOffsetXProperty =
            DependencyProperty.Register("CenterOffsetX", typeof(double), typeof(RadialPanel), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// An offset on the y coordinate of the radial origin (starting at panel height / 2.0).
        /// By default 0
        /// </summary>
        public double CenterOffsetY
        {
            get { return (double)GetValue(CenterOffsetYProperty); }
            set { SetValue(CenterOffsetYProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for CenterOffsetY.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty CenterOffsetYProperty =
            DependencyProperty.Register("CenterOffsetY", typeof(double), typeof(RadialPanel), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Define the way (unit in which) CenterOffsetX is calculated.
        /// By default : PercentageOfPanelSize
        /// </summary>
        public RadialPanelDistancesUnit CenterOffsetXUnit
        {
            get { return (RadialPanelDistancesUnit)GetValue(CenterOffsetXUnitProperty); }
            set { SetValue(CenterOffsetXUnitProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for CenterOffsetX.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty CenterOffsetXUnitProperty =
            DependencyProperty.Register("CenterOffsetXUnit", typeof(RadialPanelDistancesUnit), typeof(RadialPanel)
                , new FrameworkPropertyMetadata(RadialPanelDistancesUnit.PercentageOfPanelSize, FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Define the way (unit in which) CenterOffsetY is calculated.
        /// By default : PercentageOfPanelSize
        /// </summary>
        public RadialPanelDistancesUnit CenterOffsetYUnit
        {
            get { return (RadialPanelDistancesUnit)GetValue(CenterOffsetYUnitProperty); }
            set { SetValue(CenterOffsetYUnitProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for CenterOffsetY.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty CenterOffsetYUnitProperty =
            DependencyProperty.Register("CenterOffsetYUnit", typeof(RadialPanelDistancesUnit), typeof(RadialPanel)
                , new FrameworkPropertyMetadata(RadialPanelDistancesUnit.PercentageOfPanelSize, FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Get the angle offset to add to the calculated angle for the children object that define this attached property
        /// </summary>
        /// <param name="obj">The children object which need the offset on it's angle</param>
        /// <returns>The angle offset</returns>
        public static double GetAngleOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(AngleOffsetProperty);
        }

        /// <summary>
        /// Set the angle offset to add to the calculated angle for the children object that define this attached property
        /// </summary>
        /// <param name="obj">The children object which need the offset on it's angle</param>
        /// <param name="value">The angle offset</param>
        public static void SetAngleOffset(DependencyObject obj, double value)
        {
            obj.SetValue(AngleOffsetProperty, value);
        }

        /// <summary>
        /// Using a AttachedProperty as the backing store for AngleOffset.
        /// Can be defined on a child object to to add an offset on the calculated angle on this specific children
        /// </summary>
        public static readonly DependencyProperty AngleOffsetProperty =
            DependencyProperty.RegisterAttached("AngleOffset", typeof(double), typeof(RadialPanel),
                new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(AngleOffset_Changed)));

        private static void AngleOffset_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.FindParent<RadialPanel>()?.InvalidateArrange();
        }

        public static double GetLocalAngleOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(LocalAngleOffsetProperty);
        }

        public static void SetLocalAngleOffset(DependencyObject obj, double value)
        {
            obj.SetValue(LocalAngleOffsetProperty, value);
        }

        // Using a DependencyProperty as the backing store for LocalAngleOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocalAngleOffsetProperty =
            DependencyProperty.RegisterAttached("LocalAngleOffset", typeof(double), typeof(RadialPanel),
                new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(AngleOffset_Changed)));

        /// <summary>
        /// Get the radiusX offset to add to the global radiusX for the children object that define this attached property
        /// </summary>
        /// <param name="obj">The children object which need the offset on it's radiusX</param>
        /// <returns>The radiusX offset</returns>
        public static double GetRadiusXOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(RadiusXOffsetProperty);
        }

        /// <summary>
        /// Set the radiusX offset to add to the global radiusX for the children object that define this attached property
        /// </summary>
        /// <param name="obj">The children object which need the offset on it's radiusX</param>
        /// <param name="value">The radiusX offset</param>
        public static void SetRadiusXOffset(DependencyObject obj, double value)
        {
            obj.SetValue(RadiusXOffsetProperty, value);
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for RadiusXOffset.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty RadiusXOffsetProperty =
            DependencyProperty.RegisterAttached("RadiusXOffset", typeof(double), typeof(RadialPanel),
                new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(RadiusXOffset_Changed)));

        private static void RadiusXOffset_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.FindParent<RadialPanel>()?.InvalidateArrange();
        }

        /// <summary>
        /// Get the radiusY offset to add to the global radiusY for the children object that define this attached property
        /// </summary>
        /// <param name="obj">The children object which need the offset on it's radiusY</param>
        /// <returns>The radiusY offset</returns>
        public static double GetRadiusYOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(RadiusYOffsetProperty);
        }

        /// <summary>
        /// Set the radiusY offset to add to the global radiusY for the children object that define this attached property
        /// </summary>
        /// <param name="obj">The children object which need the offset on it's radiusY</param>
        /// <param name="value">The radiusY offset</param>
        public static void SetRadiusYOffset(DependencyObject obj, double value)
        {
            obj.SetValue(RadiusYOffsetProperty, value);
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for RadiusYOffset.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty RadiusYOffsetProperty =
            DependencyProperty.RegisterAttached("RadiusYOffset", typeof(double), typeof(RadialPanel),
                new FrameworkPropertyMetadata(0d, new PropertyChangedCallback(RadiusYOffset_Changed)));

        private static void RadiusYOffset_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.FindParent<RadialPanel>()?.InvalidateArrange();
        }

        private void GetRadius(out double radiusX, out double radiusY, UIElement elem, Size finalSize)
        {
            // Calculate the X and Y radius
            radiusX = RadiusXUnit == RadialPanelDistancesUnit.PercentageOfPanelSize ? finalSize.Width * (RadiusX + GetRadiusXOffset(elem)) / 100d / 2d : (RadiusX + GetRadiusXOffset(elem));
            radiusY = RadiusYUnit == RadialPanelDistancesUnit.PercentageOfPanelSize ? finalSize.Height * (RadiusY + GetRadiusYOffset(elem)) / 100d / 2d : (RadiusY + GetRadiusYOffset(elem));

            if (RadiusUse == RadialPanelRadiusUse.RadiusXOnly)
            {
                radiusY = radiusX;
            }
            else if (RadiusUse == RadialPanelRadiusUse.RadiusYOnly)
            {
                radiusY = radiusX;
            }
            else if (RadiusUse == RadialPanelRadiusUse.SmallestRadius)
            {
                double radius = Math.Min(radiusX, radiusY);
                radiusX = radius;
                radiusY = radius;
            }
            else if (RadiusUse == RadialPanelRadiusUse.GreatestRadius)
            {
                double radius = Math.Max(radiusX, radiusY);
                radiusX = radius;
                radiusY = radius;
            }
            else if (RadiusUse == RadialPanelRadiusUse.AverageRadius)
            {
                double radius = (radiusX + radiusY) / 2d;
                radiusX = radius;
                radiusY = radius;
            }
        }

        public static double GetFinalAngleBinding(DependencyObject obj)
        {
            return (double)obj.GetValue(FinalAngleBindingProperty);
        }

        public static void SetFinalAngleBinding(DependencyObject obj, double value)
        {
            obj.SetValue(FinalAngleBindingProperty, value);
        }

        // Using a DependencyProperty as the backing store for FinalAngleBinding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FinalAngleBindingProperty =
            DependencyProperty.RegisterAttached("FinalAngleBinding", typeof(double), typeof(RadialPanel), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Measure each children and give as much room as they want 
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement elem in Children)
            {
                //Give Infinite size as the avaiable size for all the children
                elem.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }
            return base.MeasureOverride(availableSize);
        }

        /// <summary>
        /// Arrange all children based on the geometric equations for the circle.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count == 0)
                return finalSize;

            //Base Angle in degrees
            double angle = BaseAngle;

            // The default angle between 2 elements in degrees
            double incrementalAngularSpace = 360.0 / Children.Count;

            if (Clockwise)
                incrementalAngularSpace = -incrementalAngularSpace;

            // Calculate the radial coordinates origin (center)
            double centerOffsetX = CenterOffsetXUnit == RadialPanelDistancesUnit.PercentageOfPanelSize ? finalSize.Width * CenterOffsetX / 100d / 2d : CenterOffsetX;
            double centerOffsetY = CenterOffsetYUnit == RadialPanelDistancesUnit.PercentageOfPanelSize ? finalSize.Width * CenterOffsetY / 100d / 2d : CenterOffsetY;

            foreach (UIElement elem in Children)
            {
                double angleOffset = GetAngleOffset(elem);
                double localAngleOffset = GetLocalAngleOffset(elem);
                double radianAngle = (angle + angleOffset) * degreesToRadiansFactor;

                // Calculate radius X and Y
                GetRadius(out double radiusX, out double radiusY, elem, finalSize);

                //Calculate the point on the circle for the element
                Point childPoint = new Point(Math.Cos(radianAngle) * radiusX, -Math.Sin(radianAngle) * radiusY);

                //Offsetting the point to the Avalable rectangular area which is FinalSize.
                Point actualChildPoint = new Point(
                    (finalSize.Width / 2d) + centerOffsetX + childPoint.X - (elem.DesiredSize.Width / 2)
                    , (finalSize.Height / 2d) + centerOffsetY + childPoint.Y - (elem.DesiredSize.Height / 2));

                //Call Arrange method on the child element by giving the calculated point as the placementPoint.
                elem.Arrange(new Rect(actualChildPoint.X, actualChildPoint.Y, elem.DesiredSize.Width, elem.DesiredSize.Height));

                double finalAngle = localAngleOffset;

                // If necessary orient every elements
                if (ChildrenAngleFollowing == RadialPanelChildrenAngleFollowing.LookAtTheCenter)
                {
                    finalAngle = -(angle + angleOffset) + localAngleOffset;
                }

                if (elem is FrameworkElement feElem)
                {
                    feElem.LayoutTransform = new RotateTransform(finalAngle);
                }

                SetFinalAngleBinding(elem, finalAngle);

                //Calculate the new _angle for the next element
                angle += incrementalAngularSpace;
            }

            return finalSize;
        }
    }
}
