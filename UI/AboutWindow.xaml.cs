using System;
using System.Windows;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace RCS.K2.NFLN.UI
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow
    {
        EventHandler _closeWindow;
        public AboutWindow()
        {
            InitializeComponent();

            var app = Assembly.GetExecutingAssembly();
            var title = (AssemblyTitleAttribute)app.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0];
            var product = (AssemblyProductAttribute)app.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0];
            var copyright = (AssemblyCopyrightAttribute)app.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0];
            var company = (AssemblyCompanyAttribute)app.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)[0];
            var description = (AssemblyDescriptionAttribute)app.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0];

            Title.Text = title.Title;
            Product.Text = product.Product;
            Version.Text = String.Format("Version {0}", Assembly.GetExecutingAssembly().GetName().Version);
            Copyright.Text = copyright.Copyright;
            Company.Text = company.Company;
            Description.Text = description.Description;
            _closeWindow += (sender, args) => Close();
        }

        private void CloseAbout(object sender, RoutedEventArgs e)
        {
            AnimateAboutWindow();
        }

        private void AnimateAboutWindow()
        {
            var windowOpacity = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(.1))
                };

            windowOpacity.Completed += _closeWindow;

            var heightAnimation = new DoubleAnimation
                {
                    Duration = new Duration(TimeSpan.FromSeconds(.1)),
                    From = 400,
                    To = 0,
                    FillBehavior = FillBehavior.HoldEnd
                };

            ResizeMode = ResizeMode.CanResize;
            BeginAnimation(HeightProperty, heightAnimation);
            BeginAnimation(OpacityProperty, windowOpacity);
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}