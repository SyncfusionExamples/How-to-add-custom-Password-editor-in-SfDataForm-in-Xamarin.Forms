using CustomPasswordEditor;
using CustomPasswordEditor.UWP;
using CustomPasswordEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomPasswordEntry), typeof(CustomPasswordRenderer))]
namespace CustomPasswordEditor.UWP
{
    public class CustomPasswordRenderer : ViewRenderer<Xamarin.Forms.Entry, PasswordBox>
    {
        internal PasswordBox NativePasswordBox { get; set; }
        internal string PlaceholderText { get; set; }

        internal object RevealButton { get; set; }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            NativePasswordBox = new PasswordBox();

            NativePasswordBox.Loaded += NativePasswordBox_Loaded;
            if (Element is CustomPasswordEntry)
            {
                PlaceholderText = NativePasswordBox.PlaceholderText;
                NativePasswordBox.Foreground = new SolidColorBrush(Colors.Blue);
                NativePasswordBox.FontSize = 12;
                NativePasswordBox.Style = (Style)Application.Current.Resources["CustomPasswordBoxStyle"];
                NativePasswordBox.PointerEntered += Control_PointerEntered;
                NativePasswordBox.PointerExited += Control_PointerExited;

                NativePasswordBox.GotFocus += NativeTextBox_GotFocus;
                NativePasswordBox.LostFocus += NativeTextBox_LostFocus;

                NativePasswordBox.BorderBrush = new SolidColorBrush(Colors.ForestGreen);

                NativePasswordBox.PasswordChanged += NativePasswordBox_PasswordChanged;
                NativePasswordBox.PlaceholderText = "Enter value";
                this.SetNativeControl(NativePasswordBox);
            }
        }

        private void NativePasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Element.Text = NativePasswordBox.Password;
        }

        private void NativePasswordBox_Loaded(object sender, RoutedEventArgs e)
        {
            MethodInfo TemplateChildMethod = NativePasswordBox.GetType().GetMethod("GetTemplateChild",
        BindingFlags.NonPublic | BindingFlags.Instance);
            RevealButton = TemplateChildMethod.Invoke(NativePasswordBox, new object[] { "RevealButton" });
            if (RevealButton != null && RevealButton is Windows.UI.Xaml.Controls.Primitives.ToggleButton)
            {
                (RevealButton as Windows.UI.Xaml.Controls.Primitives.ToggleButton).Click += CustomPasswordRenderer_Click;
            }
        }

        private void CustomPasswordRenderer_Click(object sender, RoutedEventArgs e)
        {
            NativePasswordBox.PasswordRevealMode = PasswordRevealMode.Visible;
            if (RevealButton != null)
                (RevealButton as Windows.UI.Xaml.Controls.Primitives.ToggleButton).Visibility = Visibility.Visible;
        }

        private void Control_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.NativePasswordBox.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void Control_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.NativePasswordBox.Foreground = new SolidColorBrush(Colors.Blue);
        }

        private void NativeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            NativePasswordBox.PlaceholderText = PlaceholderText;
            NativePasswordBox.PasswordRevealMode = PasswordRevealMode.Peek;
            if (RevealButton != null)
                (RevealButton as Windows.UI.Xaml.Controls.Primitives.ToggleButton).Visibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(NativePasswordBox.Password))
            {
                NativePasswordBox.Header = string.Empty;
                NativePasswordBox.PlaceholderText = Element.Placeholder;
            }

        }

        private void NativeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            NativePasswordBox.PlaceholderText = string.Empty;
            if (RevealButton != null)
                (RevealButton as Windows.UI.Xaml.Controls.Primitives.ToggleButton).Visibility = Visibility.Visible;

            if (string.IsNullOrEmpty(NativePasswordBox.Password))
                NativePasswordBox.Password = string.Empty;
        }
    }
}
