using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace ErasmusAppTVZ.Helpers
{
    class AnimationHelper
    {
        /// <summary>
        /// Animates property of a given object over time
        /// </summary>
        /// <param name="sender">Object that will be animated</param>
        /// <param name="value">Value of the property</param>
        /// <param name="milliseconds">Timespan of animation</param>
        /// <param name="property">Targeted property</param>
        public static void Animate(DependencyObject sender, double value, double milliseconds, PropertyPath property)
        {
            Storyboard s = new Storyboard();
            DoubleAnimation da = new DoubleAnimation();
            da.To = value;
            da.Duration = new Duration(TimeSpan.FromMilliseconds(milliseconds));

            Storyboard.SetTarget(da, sender);
            Storyboard.SetTargetProperty(da, property);

            s.Children.Add(da);
            s.Begin();
        }
    }
}
