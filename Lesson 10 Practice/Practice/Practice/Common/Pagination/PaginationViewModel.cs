using Practice.Models;
using Prism.Commands;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace Practice.Common.Pagination
{
    public class PaginationViewModel : ReactiveObject
    {
        private int _total;
        public int Total
        {
            get => _total;
            set => this.RaiseAndSetIfChanged(ref _total, value);
        }

        private int _items_per_page;
        public int items_per_page
        {
            get => _items_per_page;
            set => this.RaiseAndSetIfChanged(ref _items_per_page, value);
        }

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (value < TotalPages + 1 && value > 0)
                {
                    this.RaiseAndSetIfChanged(ref _currentPage, value);
                }

            }
        }

        private int _totalPages;
        public int TotalPages
        {
            get => _totalPages;
            set => this.RaiseAndSetIfChanged(ref _totalPages, value);
        }

        public PaginationViewModel(int total = 15, int rowNumber = 15)
        {
            Total = total;
            items_per_page = rowNumber;

            Math.DivRem(total, rowNumber, out var remainderCheck);
            TotalPages = total / rowNumber;
            if (remainderCheck != 0) TotalPages++;
            CurrentPage = 1;

            PageChangeCommand = new DelegateCommand<object>(PageChange);
        }


        public DelegateCommand<object> PageChangeCommand { get; }

        private void PageChange(object obj)
        {
            int parameter = Convert.ToInt32(obj);
            int newpage = CurrentPage;

            switch (parameter)
            {
                case 0:
                    newpage--;
                    if (newpage < 1) newpage = 1;
                    break;
                case 1:
                    newpage++;
                    if (newpage > TotalPages) newpage = TotalPages;
                    break;
            }

            CurrentPage = newpage;
        }

        public void calculatePagination()
        {
            if (items_per_page == 0) items_per_page = 1;
            if (CurrentPage > TotalPages) items_per_page = TotalPages;
            //TotalPages =  / items_per_page;
            //current_page = 1;
        }

    }
}
