using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ArduinoProject
{
    class MainWindowVM : ReactiveObject
    {
        public SerialPort port;
        bool _stop = false;
        double nextNumber = 0.0;
        double addnumber = 0.1;

        //Получение корректного значения. 
        List<double> CurrectNumbers = new List<double>();
        [Reactive] public bool isCurrectNumbers { get; set; } = false;
        int col = 50;

        //Получение корректного значения. 
        public static MainWindowVM mainwindowWM;
        public MainWindowVM()
        {
            mainwindowWM = this;
            contentControl = ViewModel.Page1VM.page1;
        }

        public void OpenPort(string comPort, int numberport)
        {
            Task.Run(() => { });
            port = new SerialPort(comPort, numberport);
            while (!port.IsOpen)
            {
                try
                {
                    port.Open();
                }
                catch { }
            }

            if (port.IsOpen)
            {
                Start();
                StartNumber2();
            }
        }

        private async void Start()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        if (!_stop && port.IsOpen)
                        {
                            var s = port.ReadLine();
                            var str = s.Substring(0, s.IndexOf('.'));
                            double isNorm = Convert.ToDouble(str);
                            //<.. Корректное положение ультразвука../> 
                            CurrectNumbers.Add(isNorm);
                            if (CurrectNumbers.Count() == col && !isCurrectNumbers)
                            {
                                double res = CurrectNumbers.Aggregate((x, y) => x + y) / col;

                                isCurrectNumbers = (res > CurrectNumbers[0] + 1 && res > numberNow + 1) ||
                                                   (res < CurrectNumbers[0] - 1 && res < numberNow - 1) &&
                                                   (CurrectNumbers[5] > numberNow + 1 || CurrectNumbers[5] < numberNow - 1)
                                                   ? false : true;
                                CurrectNumbers.Clear();
                            }
                            //<.. Корректное положение ультразвука../> 
                            nextNumber = isNorm;
                        }
                    }
                    catch { }
                }
            });
        }

        private async void StartNumber2()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    //nextnumbers  -> Значение с порта
                    //numberNow    -> мой поток к значению порта. 
                    Thread.Sleep(4);
                    if (numberNow >= nextNumber - 0.5 && numberNow <= nextNumber + 0.5)
                    {
                        numberNow = nextNumber;
                        numbers = $"{nextNumber}, \n-> {numberNow}";
                    }
                    else
                    {
                        numberNow = numberNow == 0 ? nextNumber
                            : numberNow > nextNumber ? numberNow - addnumber
                            : numberNow + addnumber;
                        numbers = $"{nextNumber}, \n-> {numberNow}";
                    }
                    //numberNow = numberNow == 0 ? nextNumber
                    //        : numberNow > nextNumber ? numberNow - addnumber
                    //        : numberNow + addnumber;
                    //numbers = $"{nextNumber}, \n-> {numberNow}";
                }
            });
        }

        [Reactive] public object contentControl { get; set; }
        [Reactive] public int X { get; set; }
        [Reactive] public int Y { get; set; }
        [Reactive] public string numbers { get; set; } = "";
        [Reactive] public ICommand WindowLoadedCommand { get; set; }
        [Reactive] public double numberNow { get; set; } = 0.0;
    }
}
