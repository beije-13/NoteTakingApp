using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoteTakingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RTB.SetValue(Paragraph.LineHeightProperty, 1.0);
        }
        // Context Menu Actions
        void ClickPaste(Object sender, RoutedEventArgs args) { RTB.Paste(); }
        void ClickCopy(Object sender, RoutedEventArgs args) { RTB.Copy(); }
        void ClickCut(Object sender, RoutedEventArgs args) { RTB.Cut(); }

        void ClickUndo(Object sender, RoutedEventArgs args) { RTB.Undo(); }
        void ClickRedo(Object sender, RoutedEventArgs args) { RTB.Redo(); }

        void ClickAlignCenter(Object sender, RoutedEventArgs args) { EditingCommands.AlignCenter.Execute(null, RTB); }
        void ClickAlignJustify(Object sender, RoutedEventArgs args) { EditingCommands.AlignJustify.Execute(null, RTB); }
        void ClickAlignRight(Object sender, RoutedEventArgs args) { EditingCommands.AlignRight.Execute(null, RTB); }
        void ClickAlignLeft(Object sender, RoutedEventArgs args) { EditingCommands.AlignLeft.Execute(null, RTB); }

        void ClickToggleBold(Object sender, RoutedEventArgs args) { EditingCommands.ToggleBold.Execute(null, RTB); }
        void ClickToggleItalic(Object sender, RoutedEventArgs args) { EditingCommands.ToggleItalic.Execute(null, RTB); }
        void ClickToggleUnderline(Object sender, RoutedEventArgs args) { EditingCommands.ToggleUnderline.Execute(null, RTB); }

        void ClickToggleSubscript(Object sender, RoutedEventArgs args) { EditingCommands.ToggleSubscript.Execute(null, RTB); }
        void ClickToggleSuperscript(Object sender, RoutedEventArgs args) { EditingCommands.ToggleSuperscript.Execute(null, RTB); }

        void ClickToggleNumbering(Object sender, RoutedEventArgs args) { EditingCommands.ToggleNumbering.Execute(null, RTB); }
        void ClickToggleBullets(Object sender, RoutedEventArgs args) { EditingCommands.ToggleBullets.Execute(null, RTB); }

        // Lookup font size On opening Context Menu
        private void RichContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            FontSizeTextBox.Text = RTB.FontSize.ToString();
        }

        // Change font size of selected text if Enter is pressed inside FontSize textbox
        private void FontSizeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                RTB.Selection.ApplyPropertyValue(FontSizeProperty, FontSizeTextBox.Text);
                e.Handled = true;
            }
        }
    }
}
