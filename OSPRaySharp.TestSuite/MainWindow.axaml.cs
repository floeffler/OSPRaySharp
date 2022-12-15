using Avalonia.Controls;
using System;

namespace OSPRay.TestSuite
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            renderControl.Initialize();
        }

        protected override void OnClosed(EventArgs e)
        {
            renderControl.Shutdown();
            base.OnClosed(e);
        }
    }
}
