using Org.Visiontech.Credential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using VisiontechCommons;
using Xamarin.Forms.Extended;

namespace CalcolatoreXamarin.ViewModels.Abstraction
{
    public class ScrollableListViewModel<D, S, I> where D : class where S : ClientBase<I> where I : class
    {

        protected S service = Container.ServiceProvider.GetService(typeof(S)) as S;

        public InfiniteScrollCollection<D> Items { get; set; }

        public int PageSize { get; set; } = 20;
        public int Count { get; set; } = int.MaxValue;

        public ScrollableListViewModel(Func<S, int, int, IEnumerable<D>> LoadMore)
        {

            Items = new InfiniteScrollCollection<D>
            {
                OnCanLoadMore = () => {
                    return Count>Items.Count;
                },

                OnLoadMore = () =>
                {

                    return Task.Run(() => {

                        IEnumerable<D> result = LoadMore.Invoke(service, Math.Min(PageSize, Count - Items.Count), Items.Count / PageSize);

                        Count += result.Count();

                        return result;

                    });
                }
            };
        }

    }
}
