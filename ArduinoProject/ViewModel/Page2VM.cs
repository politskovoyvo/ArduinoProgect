using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Windows.Input;
using MW = ArduinoProject.MainWindowVM;

namespace ArduinoProject.ViewModel
{
    public class Page2VM : ReactiveObject
    {
        public static Page2VM page2 = new Page2VM();
        public Page2VM()
        {
            page2 = this;

            InitializeViewModelCommand = ReactiveCommand.Create(() =>
            {
                startNumber = (int)MW.mainwindowWM.numberNow;
                MW.mainwindowWM.WhenAnyValue(x => x.numberNow).Subscribe(_ =>
                {
                    X = (startNumber - MW.mainwindowWM.numberNow)*10;
                    //Логика от сдвижек .../> 

                    //Логика от сдвижек .../> 
                }
                ); 
            });
        }

        [Reactive] public ICommand InitializeViewModelCommand { get; set; }
        [Reactive] public double X { get; set; }

        [Reactive] public string page2number { get; set; }
        [Reactive] public string testText { get; set; }
        [Reactive] public int startNumber { get; set; } = 0; 
    }
}
