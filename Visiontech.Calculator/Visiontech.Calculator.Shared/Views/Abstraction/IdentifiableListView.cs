using CalcolatoreXamarin.ViewModels.Abstraction;
using System.Diagnostics;
using System.ServiceModel;
using VisiontechCommons;
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace CalcolatoreXamarin.Views.Abstraction
{
    public class ScrollableListView<D, S, I> : ListView where D : class where S : ClientBase<I> where I : class
    {

        private readonly ScrollableListViewModel<D, S, I> scrollableListViewModel;

        public ScrollableListView(ScrollableListViewModel<D, S, I> scrollableListViewModel) : base(ListViewCachingStrategy.RecycleElement)
        {

            this.scrollableListViewModel = scrollableListViewModel;

            ItemsSource = scrollableListViewModel.Items;
            HasUnevenRows = true;

            ItemTemplate = Container.ServiceProvider.GetService(typeof(EntityTemplate<D>)) as EntityTemplate<D>;

            if (ItemTemplate is null)
            {
                Debug.WriteLine("Template Not Found : Using default for object");
                ItemTemplate = Container.ServiceProvider.GetService(typeof(EntityTemplate<object>)) as EntityTemplate<object>;
            }


            InfiniteScrollBehavior infiniteScrollBehavior = new InfiniteScrollBehavior();
            Behaviors.Add(infiniteScrollBehavior);

            Frame frame = new Frame()
            {
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Transparent,
                HasShadow = false,
                Content = new ActivityIndicator()
                {
                    IsRunning = true
                }
            };

            frame.SetBinding(IsVisibleProperty, new Binding(InfiniteScrollBehavior.IsLoadingMoreProperty.PropertyName, BindingMode.OneWay, source: infiniteScrollBehavior));

            Footer = frame;

        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            scrollableListViewModel.Items.LoadMoreAsync();
        }

    }
}
