﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Styling;

namespace AvaloniaControls
{
    /// <summary>
    /// This is a control that displays a defined number of stars and the user
    /// can click a rating from 0 to n stars resulting in <see cref="Value"/> being
    /// set to a value between 0 and 1.
    /// </summary>
    public class RatingControl : TemplatedControl, ITemplatedControl
    {
        public static readonly StyledProperty<int> NumberOfStarsProperty =
            AvaloniaProperty.Register<RatingControl, int>(nameof(NumberOfStars), 6, validate: ValidateNumberOfStars);

        public static readonly StyledProperty<double> ValueProperty =
            AvaloniaProperty.Register<RatingControl, double>(nameof(NumberOfStars), 0, validate: ValidateValue);

        /// <summary>
        /// Defines the <see cref="StarItems"/> property.
        /// </summary>
        public static readonly DirectProperty<RatingControl, IEnumerable> StarItemsProperty =
            AvaloniaProperty.RegisterDirect<RatingControl, IEnumerable>(nameof(StarItems), o => o.StarItems);//, (o, v) => o.StarItems = v);

        private  IEnumerable _starItems;

        static RatingControl()
        {
            ContentPresenter.ContentTemplateProperty.AddOwner<RatingControl>();
            NumberOfStarsProperty.Changed.Subscribe(OnNumberOfStarsChanged);
            AffectsRender<RatingControl>(NumberOfStarsProperty, ValueProperty);
            AffectsMeasure<RatingControl>(NumberOfStarsProperty);
            //TemplateProperty.OverrideDefaultValue(typeof());
        }

        private static int ValidateNumberOfStars(RatingControl arg1, int val)
        {
            return val.LimitTo(1, 100);
        }

        private static double ValidateValue(RatingControl arg1, double val)
        {
            return val.LimitTo(0.0, 1.0);
        }

        /// <summary>
        /// Sets or gets the number of stars to use for this rating.
        /// </summary>
        public int NumberOfStars
        {
            get { return GetValue(NumberOfStarsProperty); }
            set { SetValue(NumberOfStarsProperty, value); }
        }

        /// <summary>
        /// Sets or gets the rating value of this control.
        /// </summary>
        /// <value>Between 0.0 and 1.0 (best).</value>
        public double Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public IEnumerable StarItems
        {
            get { return _starItems; }
            private set { SetAndRaise(StarItemsProperty, ref _starItems, value); }
        }

        /// <summary>
        /// ItemsProperty property changed handler.
        /// </summary>
        /// <param name="e">AvaloniaPropertyChangedEventArgs.</param>
        private static void OnNumberOfStarsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is RatingControl rating)
            {
                var newValue = (int)e.NewValue;
                UpdateStars(rating, newValue);
            }
        }

        private static void UpdateStars(RatingControl rating, int newValue)
        {
            //new ListBoxItem().
            rating.StarItems = Enumerable.Repeat("S", newValue);
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {


        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
//            OnNumberOfStarsChanged(new AvaloniaPropertyChangedEventArgs(this, NumberOfStarsProperty, 0, NumberOfStars, BindingPriority.Unset));
            UpdateStars(this, NumberOfStars);
            //RaisePropertyChanged(StarItemsProperty,  );
        }
    }
}
