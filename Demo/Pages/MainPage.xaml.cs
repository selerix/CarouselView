using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CarouselView.FormsPlugin.Abstractions;
using System.Linq;

namespace Demo
{
	public partial class MainPage : ContentPage
	{
		public MainViewModel _vm;

        private void MovePage(bool left)
        {
            var count = _vm.ItemsSource.Count;

            if (count <= 1) return;

            //var pos = _vm.Position;
            var pos = myCarousel.Position;

            if (left && pos == 0) return;

            if (!left && pos == count - 1) return;

            var oldIndex = pos;
            var newIndex = oldIndex + (left ? - 1 : 1);

            _vm.ItemsSource.Move(oldIndex, newIndex);
        }

		public MainPage ()
		{
			InitializeComponent ();

			BindingContext = _vm = new MainViewModel();

			//myCarousel.ItemsSource = _vm.ItemsSource;

			//myCarousel.ItemTemplate = new DataTemplate(typeof(MyFirstView));

			MessagingCenter.Subscribe<MyFirstView> (this, "RemoveMe", (sender) => {
				_vm.ItemsSource.RemoveAt(myCarousel.Position);
			});

            MessagingCenter.Subscribe<MyFirstView>(this, "ShiftLeft", (sender) => {
                MovePage(true);
            });

            MessagingCenter.Subscribe<MyFirstView>(this, "ShiftRight", (sender) => {
                MovePage(false);
            });

            ConfigureButtons();

			ToolbarItems.Add(new ToolbarItem
			{
				Text = "Source",
				Order = ToolbarItemOrder.Primary,
				Command = new Command(() => {
                    _vm.ItemsSource.Clear();
                    //_vm.ItemsSource.Move(0, 4); // Position selected not called
                    //_vm.ItemsSource[0] = new MySecondView();
                    //myCarousel.IndicatorsShape = IndicatorsShape.Circle;
                    //myCarousel.ShowIndicators = !myCarousel.ShowIndicators;
                    //_vm.Position = _vm.Position == 0 ? 3 : 0;
				})
			});

			ToolbarItems.Add(new ToolbarItem
			{
				Text = "Orientation",
				Order = ToolbarItemOrder.Primary,
				Command = new Command(() =>
				{
					//Navigation.PushAsync(new NoSwipePage());
					//Navigation.PushAsync(new SecondPage());
					//Navigation.PushAsync(new MyTabbedPage());
                    myCarousel.Orientation = myCarousel.Orientation == CarouselViewOrientation.Horizontal ? CarouselViewOrientation.Vertical : CarouselViewOrientation.Horizontal;
				})
			});

            var gesture = new TapGestureRecognizer();
            gesture.Command = new Command<int>((obj) => {
                Debug.WriteLine("Tapped with " + obj + " as parameter.");
            });
            gesture.CommandParameter = 1;

            myCarousel.GestureRecognizers.Add(gesture);
		}

		void ConfigureButtons()
		{
			prevBtn.IsVisible = _vm.Position > 0;
			addPageBtn.IsVisible = _vm.ItemsSource != null;
			nextBtn.IsVisible = _vm.Position < _vm.ItemsSource?.Count - 1;
		}

        void Handle_PositionSelected(object sender, PositionSelectedEventArgs e)
        {
			ConfigureButtons();
			Debug.WriteLine("Position " + myCarousel.Position + " selected");
        }

		public void OnPrev (object sender, TappedEventArgs e)
		{
			if (_vm.Position > 0)
				_vm.Position--;
		}

		public void OnNext(object sender, TappedEventArgs e)
		{
			if (_vm.Position < _vm.ItemsSource?.Count - 1)
				_vm.Position++;
		}

		public void OnAdd(object sender, TappedEventArgs e)
		{
			if (_vm.ItemsSource != null)
			{
                _vm.ItemsSource.Add(new MyFirstView() { BindingContext = myCarousel.ItemsSource.GetCount() });
				// Do this to trigger PositionSelected
				//if (_vm.ItemsSource.Count > 0)
				///{
                    //_vm.ItemsSource.Add(_vm.ItemsSource.Max() + 1);
					//_vm.Position = _vm.ItemsSource.Count - 1;
				//}
				//else
				//{
					//_vm.ItemsSource.Add(0);
				//}

				// Do this to refresh Prev/Next visibility
                //ConfigureButtons();
		    }
		}
	}
}

