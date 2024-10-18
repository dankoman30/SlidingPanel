using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Threading.Tasks;
using WpfApp2.Commands.Base;
using System.Diagnostics;
using System.Windows.Media;

namespace WpfApp2.CustomControls
{
    public class SlidingPanel : Control, INotifyPropertyChanged
    {
        private ItemsControl _itemsControl;
        private TranslateTransform _translateTransform;
        private bool _isAnimating = false;

        static SlidingPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SlidingPanel), new FrameworkPropertyMetadata(typeof(SlidingPanel)));
        }

        public SlidingPanel()
        {
            SetValue(PreviousCommandProperty, new CommandsBase(NavigateLeft));
            SetValue(NextCommandProperty, new CommandsBase(NavigateRight));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(SlidingPanel), new FrameworkPropertyMetadata(200.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double ItemSpacing
        {
            get { return (double)GetValue(ItemSpacingProperty); }
            set { SetValue(ItemSpacingProperty, value); }
        }
        public static readonly DependencyProperty ItemSpacingProperty =
            DependencyProperty.Register("ItemSpacing", typeof(double), typeof(SlidingPanel), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
           DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(SlidingPanel), new PropertyMetadata(null, OnItemsSourceChanged));

        public int VisibleItemsCount
        {
            get { return (int)GetValue(VisibleItemsCountProperty); }
            set { SetValue(VisibleItemsCountProperty, value); }
        }
        public static readonly DependencyProperty VisibleItemsCountProperty =
            DependencyProperty.Register("VisibleItemsCount", typeof(int), typeof(SlidingPanel), new FrameworkPropertyMetadata(4, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(SlidingPanel), new PropertyMetadata(null));

        public static readonly DependencyProperty PreviousCommandProperty =
            DependencyProperty.Register(nameof(PreviousCommand), typeof(ICommand), typeof(SlidingPanel), new PropertyMetadata(null));

        public ICommand PreviousCommand
        {
            get { return (ICommand)GetValue(PreviousCommandProperty); }
            set { SetValue(PreviousCommandProperty, value); }
        }

        public static readonly DependencyProperty NextCommandProperty =
            DependencyProperty.Register(nameof(NextCommand), typeof(ICommand), typeof(SlidingPanel), new PropertyMetadata(null));

        public ICommand NextCommand
        {
            get { return (ICommand)GetValue(NextCommandProperty); }
            set { SetValue(NextCommandProperty, value); }
        }



        protected override Size MeasureOverride(Size constraint)
        {
            UpdateItemsControlWidth();
            return base.MeasureOverride(constraint);
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (SlidingPanel)d;
            panel.UpdateItemsControl();
        }

        private void UpdateItemsControl()
        {
            if (_itemsControl != null)
            {
                _itemsControl.ItemsSource = this.ItemsSource;
                UpdateItemsControlWidth();
            }
        }

        private void UpdateItemsControlWidth()
        {
            if (_itemsControl != null)
            {
                _itemsControl.Width = (ItemWidth + ItemSpacing) * VisibleItemsCount - ItemSpacing;
            }
        }

        public int CurrentStartIndex
        {
            get { return (int)GetValue(CurrentStartIndexProperty); }
            set { SetValue(CurrentStartIndexProperty, value); }
        }

        public static readonly DependencyProperty CurrentStartIndexProperty =
            DependencyProperty.Register(nameof(CurrentStartIndex), typeof(int), typeof(SlidingPanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentStartIndexChanged));


        private void NavigateLeft(object? parameter)
        {
            Debug.WriteLine("NavigateLeft called");
            if (CurrentStartIndex > 0)
            {
                CurrentStartIndex--;
            }
        }

        private void NavigateRight(object? parameter)
        {
            Debug.WriteLine("NavigateRight called");
            if (_itemsControl != null)
            {
                int maxStartIndex = Math.Max(0, _itemsControl.Items.Count - VisibleItemsCount);
                if (CurrentStartIndex < maxStartIndex)
                {
                    CurrentStartIndex++;
                }
            }
        }

        public void AnimateToIndex(int newIndex)
        {
            Debug.WriteLine($"AnimateToIndex called with newIndex: {newIndex}");
            AnimateToIndex(newIndex, TimeSpan.FromMilliseconds(300));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _itemsControl = GetTemplateChild("PART_ItemsControl") as ItemsControl;
            if (_itemsControl != null)
            {
                _itemsControl.ItemTemplate = this.ItemTemplate;
                _itemsControl.ItemsSource = this.ItemsSource;

                _translateTransform = new TranslateTransform();
                _itemsControl.RenderTransform = _translateTransform;

                Debug.WriteLine("OnApplyTemplate: TranslateTransform applied to ItemsControl");
            }
            UpdateItemsControlWidth();
        }

        private static void OnCurrentStartIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SlidingPanel panel && !panel._isAnimating)
            {
                Debug.WriteLine($"CurrentStartIndex changed from {e.OldValue} to {e.NewValue}");
                panel.AnimateToIndex((int)e.NewValue);
            }
        }

        public void AnimateToIndex(int newIndex, TimeSpan duration)
        {
            if (_itemsControl != null && _translateTransform != null && !_isAnimating)
            {
                _isAnimating = true;
                double currentOffset = _translateTransform.X;
                Debug.WriteLine($"Current TranslateTransform X: {currentOffset}");

                double targetOffset = -newIndex * (ItemWidth + ItemSpacing);
                int maxStartIndex = Math.Max(0, _itemsControl.Items.Count - VisibleItemsCount);
                targetOffset = Math.Max(targetOffset, -maxStartIndex * (ItemWidth + ItemSpacing));

                Debug.WriteLine($"Animating from {currentOffset} to {targetOffset}");

                var animation = new DoubleAnimation
                {
                    From = currentOffset,
                    To = targetOffset,
                    Duration = duration,
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                animation.Completed += (s, e) =>
                {
                    _translateTransform.X = targetOffset;
                    _isAnimating = false;
                    Debug.WriteLine($"Animation completed. TranslateTransform X set to {targetOffset}");
                };

                _translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
            }
            else
            {
                Debug.WriteLine("_itemsControl or _translateTransform is null, or animation in progress in AnimateToIndex");
            }
        }


    }
}