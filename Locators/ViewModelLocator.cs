/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:delete"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

// Toolkit namespace

using RCS.K2.NFLN.Services;
using RCS.K2.NFLN.UI;
using RCS.K2.NFLN.ViewModels;

namespace RCS.K2.NFLN
{
    /// <summary>
    /// This class creates ViewModels on demand for Views, supplying a
    /// ServiceAgent to the ViewModel if required.
    /// <para>
    /// Place the ViewModelLocator in the App.xaml resources:
    /// </para>
    /// <code>
    /// &lt;Application.Resources&gt;
    ///     &lt;vm:ViewModelLocator xmlns:vm="clr-namespace:delete"
    ///                                  x:Key="Locator" /&gt;
    /// &lt;/Application.Resources&gt;
    /// </code>
    /// <para>
    /// Then use:
    /// </para>
    /// <code>
    /// DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
    /// </code>
    /// <para>
    /// Use the <strong>mvvmlocator</strong> or <strong>mvvmlocatornosa</strong>
    /// code snippets to add ViewModels to this locator.
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        // Create MainPageViewModel on demand
        public SignInWindow SignInPageViewModelWindow
        {
            get { return new SignInWindow(); }
        }

        // Create MainPageViewModel on demand
        public MainPageViewModel MainPageViewModel
        {
            get { return new MainPageViewModel(); }
        }

        // Create ConfigViewModel on demand
        public ConfigViewModel ConfigViewModel
        {
            get
            {
                IConfigServiceAgent configServiceAgent = new ConfigServiceAgent();
                return new ConfigViewModel(configServiceAgent);
            }
        }
        // Create SignInViewModel on demand
        public SignInViewModel SignInViewModel
        {
            get
            {
                ISignInServiceAgent signInServiceAgent = new SignInServiceAgent();
                return new SignInViewModel(signInServiceAgent);
            }
        }

        // Create PlayerRankingViewModel on demand
        public PlayerRankingViewModel PlayerRankingViewModel
        {
            get
            {
                IRankingServiceAgent rankingServiceAgent = new RankingServiceAgent();
                return new PlayerRankingViewModel(rankingServiceAgent);
            }
        }

        // Create PlayerEditViewModel on demand
        public PlayerEditViewModel PlayerEditViewModel
        {
            get
            {
                IPlayerEditServiceAgent playerEditServiceAgent = new PlayerEditServiceAgent();
                return new PlayerEditViewModel(playerEditServiceAgent);
            }
        }

        // Create VideoViewModel on demand
        public VideoViewModel VideoViewModel
        {
            get
            {
                IVideoServiceAgent videoServiceAgent = new VideoServiceAgent();
                return new VideoViewModel(videoServiceAgent);
            }
        }
    }
}