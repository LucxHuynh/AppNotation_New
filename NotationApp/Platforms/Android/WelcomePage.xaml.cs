using AndroidX.ConstraintLayout.Helper.Widget;
using System.Windows.Input;

namespace NotationApp.Platforms.Android;

public partial class WelcomePage : ContentPage, IWelcomePage
{
    private readonly CarouselView carousel;
    public ICommand NextCommand { get; private set; }
    public ICommand GetStartedCommand { get; private set; }
    public ICommand SkipCommand { get; private set; }

    public WelcomePage()
    {
        InitializeComponent();
        SetupCommands();
        BindingContext = this;
        carousel = this.FindByName<CarouselView>("welcomeCarousel");
    }

    private void SetupCommands()
    {
        NextCommand = new Command(() =>
        {
            var currentItem = carousel.CurrentItem?.ToString();
            if (int.TryParse(currentItem, out int index) && index < 4)
            {
                carousel.CurrentItem = (index + 1).ToString();
            }
        });

        GetStartedCommand = new Command(async () => await GoToSignIn());
        SkipCommand = new Command(async () => await GoToSignIn());
    }

    private async Task GoToSignIn()
    {
        await Shell.Current.GoToAsync("//SignInPage");
    }
}