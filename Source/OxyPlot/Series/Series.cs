﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Series.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Provides an abstract base class for plot series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using OxyPlot.Axes;

    /// <summary>
    /// Provides an abstract base class for plot series.
    /// </summary>
    /// <remarks>This class contains internal methods that should be called only from the PlotModel.</remarks>
    public abstract class Series : UIPlotElement, ITrackableSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Series" /> class.
        /// </summary>
        protected Series()
        {
            this.IsVisible = true;
            this.Background = OxyColors.Undefined;
        }

        /// <summary>
        /// Gets or sets the background color of the series.
        /// </summary>
        /// <remarks>This property defines the background color in the area defined by the x and y axes used by this series.</remarks>
        public OxyColor Background { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this series is visible.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the title of the series.
        /// </summary>
        /// <value>The title that is shown in the legend of the plot. The default value is <c>null</c>. When the value is <c>null</c>, this series will not be shown in the legend.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a format string used for the tracker.
        /// </summary>
        /// <remarks>
        /// The arguments for the format string may be different for each type of series. See the documentation.
        /// </remarks>
        public string TrackerFormatString { get; set; }

        /// <summary>
        /// Gets or sets the key for the tracker to use on this series.
        /// </summary>
        /// <remarks>
        /// This key may be used by the plot control to show a custom tracker for the series.
        /// </remarks>
        public string TrackerKey { get; set; }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public virtual TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            return null;
        }

        /// <summary>
        /// Renders the series on the specified render context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="model">The model.</param>
        public abstract void Render(IRenderContext rc, PlotModel model);

        /// <summary>
        /// Renders the legend symbol on the specified render context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The legend rectangle.</param>
        public abstract void RenderLegend(IRenderContext rc, OxyRect legendBox);

        /// <summary>
        /// Tests if the plot element is hit by the specified point.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// A hit test result.
        /// </returns>
        protected internal override HitTestResult HitTest(HitTestArguments args)
        {
            var thr = this.GetNearestPoint(args.Point, true) ?? this.GetNearestPoint(args.Point, false);

            if (thr != null)
            {
                double distance = thr.Position.DistanceTo(args.Point);
                if (distance > args.Tolerance)
                {
                    return null;
                }

                return new HitTestResult(thr.Position, thr.Item, thr.Index);
            }

            return null;
        }

        /// <summary>
        /// Checks if this data series requires X/Y axes. (e.g. Pie series do not require axes)
        /// </summary>
        /// <returns><c>true</c> if axes are required.</returns>
        protected internal abstract bool AreAxesRequired();

        /// <summary>
        /// Ensures that the axes of the series are defined.
        /// </summary>
        protected internal abstract void EnsureAxes();

        /// <summary>
        /// Checks if the data series is using the specified axis.
        /// </summary>
        /// <param name="axis">The axis that should be checked.</param>
        /// <returns><c>true</c> if the axis is in use.</returns>
        protected internal abstract bool IsUsing(Axis axis);

        /// <summary>
        /// Sets the default values (colors, line style etc.) from the plot model.
        /// </summary>
        /// <param name="model">A plot model.</param>
        protected internal abstract void SetDefaultValues(PlotModel model);

        /// <summary>
        /// Updates the maximum and minimum values of the axes used by this series.
        /// </summary>
        protected internal abstract void UpdateAxisMaxMin();

        /// <summary>
        /// Updates the data of the series.
        /// </summary>
        protected internal abstract void UpdateData();

        /// <summary>
        /// Updates the valid data of the series.
        /// </summary>
        protected internal abstract void UpdateValidData();

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        /// <remarks>This method is called when the <see cref="PlotModel" /> is updated with the <c>updateData</c> parameter set to <c>true</c>.</remarks>
        protected internal abstract void UpdateMaxMin();
    }
}