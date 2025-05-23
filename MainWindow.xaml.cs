using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Runtime.InteropServices;

namespace AutoHibernate
{
    public partial class MainWindow : Window
    {
        
        bool brake = false;
        int time;
        bool lockscreen = true;
        public MainWindow()
        {
            InitializeComponent();
            sliderTimeToSleep.Value = 3600; // Default to 1 hour
            time = 10;
            AddTextToRichTextBox("1h");
            CheckBoxLocksScreen.IsChecked= true; // Default to lock screen enabled
        }

        public void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            time = (int)e.NewValue;
            int hours = time / 3600;
            int minutes = (time % 3600) / 60;
            if (minutes == 0)
            {
                AddTextToRichTextBox($"{hours}h");
            }else
            {
                AddTextToRichTextBox($"{hours}h {minutes}m");
            }
        }
        public async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // Start the auto-hibernate process
            // This is where you would implement the logic to start the auto-hibernate functionality
            MessageBox.Show("Auto Hibernate started!");
            if (time >= 0)
            {
                if(lockscreen)
                    LockWorkStation();  

                await Task.Delay(time * 1000); 
                if (!brake)
                {
                    SetSuspendState(false, true, false);
                    Application.Current.Shutdown();
                }
                else
                {
                    brake = false;
                }
            }
            
        }

        public void LockScreenOn(object sender, RoutedEventArgs e)
        {
            lockscreen = true;
        }
        public void LockScreenOff(object sender, RoutedEventArgs e)
        {
            lockscreen = false;
        }

        public void btnStop_Click(object sender, RoutedEventArgs e)
        {
            // Stop the auto-hibernate process
            brake = true;
            MessageBox.Show("Auto Hibernate stopped!");
        }

        // P/Invoke declaration for SetSuspendState
        [DllImport("Powrprof.dll", SetLastError = true)]
        private static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);
        [DllImport("user32.dll")]
        public static extern void LockWorkStation();

        private void AddTextToRichTextBox(string text2,string text1="Currently : ", string color1="DeepSkyBlue", string color2 = "DarkRed")
        {
            txtTimeToSleep.Document.Blocks.Clear();

            Paragraph paragraph = new Paragraph { TextAlignment = TextAlignment.Center, FontSize=30}; 
            paragraph.Inlines.Add(new Run(text1) { Foreground = (Brush)new BrushConverter().ConvertFromString(color1) });
            paragraph.Inlines.Add(new Run(text2) { Foreground = (Brush)new BrushConverter().ConvertFromString(color2) });

            txtTimeToSleep.Document.Blocks.Add(paragraph);
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}