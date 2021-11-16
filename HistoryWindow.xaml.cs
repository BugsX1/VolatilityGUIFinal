using System;
using System.Windows;
using System.Windows.Documents;

namespace VolatilityGUI
{
    /// <summary>
    /// Interaction logic for HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        public HistoryWindow()
        {
            InitializeComponent();
            CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | History Window Opened");
            var listTemp = CentralWindow.historyListCons;
            Paragraph paragraph = new Paragraph();
            foreach (var i in listTemp)
            {
                string[] aux = i.Split('|');
                paragraph.Inlines.Add(new Bold(new Run(aux[0] + " - ")));
                paragraph.Inlines.Add(new Run(aux[1]));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
            }
            HistoryBox.Document = new FlowDocument(paragraph);
        }
    }
}
