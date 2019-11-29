namespace CodingSeb.Layouts
{
    /// <summary>
    /// Define the way Children elements rotate on them self basing on their radial positioning angle
    /// </summary>
    public enum RadialPanelChildrenAngleFollowing
    {
        /// <summary>
        /// No Angle following applied
        /// </summary>
        None,

        /// <summary>
        /// All the element have a angle that force them to look at the origin of the radial panel
        /// (Warning Apply a RotateTransform to the LayoutTransform of the element)
        /// </summary>
        LookAtTheCenter
    }
}
