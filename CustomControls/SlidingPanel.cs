using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WpfApp2.CustomControls
{
    public class SlidingPanel : Control
    {
        private ItemsControl _itemsControl;
        private int _currentIndex = 0;

        static SlidingPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SlidingPanel), new FrameworkPropertyMetadata(typeof(SlidingPanel)));
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


        protected override Size MeasureOverride(Size constraint)
        {
            UpdateItemsControlWidth();
            return base.MeasureOverride(constraint);
        }


        private void RegenerateChildren()
        {
            if (ItemsSource != null)
            {
                var itemsControl = GetTemplateChild("PART_ItemsControl") as ItemsControl;
                if (itemsControl != null)
                {
                    itemsControl.ItemsSource = ItemsSource;
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _itemsControl = GetTemplateChild("PART_ItemsControl") as ItemsControl;
            if (_itemsControl != null)
            {
                _itemsControl.ItemTemplate = this.ItemTemplate;
                _itemsControl.ItemsSource = this.ItemsSource;
            }
            UpdateItemsControlWidth();
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

        public void NavigateLeft()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                AnimateToIndex(_currentIndex);
            }
        }

        public void NavigateRight()
        {
            var itemsControl = GetTemplateChild("PART_ItemsControl") as ItemsControl;
            if (itemsControl != null && _currentIndex < itemsControl.Items.Count - VisibleItemsCount)
            {
                _currentIndex++;
                AnimateToIndex(_currentIndex);
            }
        }

        public void AnimateToIndex(int newIndex)
        {
            AnimateToIndex(newIndex, TimeSpan.FromMilliseconds(300));
        }

        public void AnimateToIndex(int newIndex, TimeSpan duration)
        {
            if (_itemsControl != null)
            {
                var animation = new DoubleAnimation
                {
                    From = -_currentIndex * (ItemWidth + ItemSpacing),
                    To = -newIndex * (ItemWidth + ItemSpacing),
                    Duration = duration,
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                _itemsControl.BeginAnimation(Canvas.LeftProperty, animation);
                _currentIndex = newIndex;
            }
        }
    }
}
