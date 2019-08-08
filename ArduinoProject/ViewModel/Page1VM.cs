using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using page2 = ArduinoProject.ViewModel.Page2VM;
using VM = ArduinoProject.MainWindowVM;

namespace ArduinoProject.ViewModel
{
    public class Page1VM : ReactiveObject
    {
        public static Page1VM page1 = new Page1VM();
        public Page1VM()
        {
            page1 = this;
            addConnect = ReactiveCommand.Create(() => 
            {
                textInfo = "Процесс синхронизации оборудования, ожидайте...";
                VM.mainwindowWM.OpenPort(Port, port2);
            });
            VM.mainwindowWM?.WhenAnyValue(x => x.isCurrectNumbers).Subscribe(x => goPage2());
        }

        void goPage2()
        {
            if (VM.mainwindowWM.isCurrectNumbers)
            {
                VM.mainwindowWM.contentControl = page2.page2;
            }
        }

        void page2Return ()
        {
            VM.mainwindowWM.contentControl = page2.page2;
        }

        public bool IsTrue { get; set; }
        public ICommand addConnect { get; set; }
        [Reactive] public string textInfo { get; set; }
        [Reactive] public string Port { get; set; } = "COM4";
        [Reactive] public int port2 { get; set; } = 9600;
        
    }
}
