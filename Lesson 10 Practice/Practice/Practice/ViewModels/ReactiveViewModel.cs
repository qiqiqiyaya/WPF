using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using ReactiveUI;
using System.Threading.Tasks;

namespace Practice.ViewModels
{
    public class ReactiveViewModel : ReactiveObject
    {
        public ReactiveViewModel()
        {
            //Task.Run(async () =>
            //{
            //    while (true)
            //    {
            //        if (Progress == 100)
            //        {
            //            Progress = 0;

            //        }

            //        Progress++;
            //        await Task.Delay(Progress % 10 == 0 ? 2000 : 400);
            //    }
            //});

            Observable.Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(10))
                .Subscribe(_ =>
                {
                    if (Progress == 100)
                    {
                        Progress = 0;

                    }

                    Progress++;
                });
        }

        private int _progress;

        public int Progress
        {
            get => _progress;
            set => this.RaiseAndSetIfChanged(ref _progress, value);
        }
    }
}
