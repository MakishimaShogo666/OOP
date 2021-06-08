﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VehicleModel;

namespace ConsoleRead
{
    /// <summary>
    /// Класс Programm
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Точка входа в программу
        /// </summary>
        internal static void Main(string[] args)
        {
            var dictionaryInfo = new Dictionary<Type, string>
            {
                {typeof(Car), "Машина"},
                {typeof(HybridCar), "Гибрид"},
                {typeof(Helicopter), "Вертолёт"},
            };

            var typeList = Assembly.GetAssembly(typeof(VehicleBase)).GetTypes()
                .Where(type => type.Namespace == "VehicleModel" && type.Name != "VehicleBase").ToArray();

            while (true)
            {
                InputOutput.TextWriteLine("Создайте транспорт!", ConsoleColor.Blue);
                int number = CheckCount(byte.MaxValue, "Введите число объектов транспорта: ",
                    $"Введено нецелое число или число, превыщающее максимальное значение! ({byte.MaxValue})");

                List<VehicleBase> vehicleList = new List<VehicleBase>();

                MethodHandler(() => VehicleListAdd(number, typeList, dictionaryInfo, vehicleList));
                MethodHandler(() => VehicleListInput(dictionaryInfo, vehicleList));
                MethodHandler(() => VehicleListWrite(vehicleList));
                MethodHandler(() => VehicleConsumption(vehicleList));
                MethodHandler(() => VehicleConsumptionByTime(vehicleList));

                if (InputOutput.QuitOfProgram()) return;
            }
        }

        /// <summary>
        /// Метод проверки введённого числа
        /// </summary>
        /// <param name="comparedVar">Величина, с которой выполняется сравнение</param>
        /// <param name="inputMessage">Сообщение ввода</param>
        /// <param name="awareMessage">Сообщение в случае превышения</param>
        /// <returns>Введённое число</returns>
        private static int CheckCount(int comparedVar, string inputMessage, string awareMessage)
        {
            int primeVar;

            while (!int.TryParse(InputOutput.Input(inputMessage, InputOutput.InputTypeEnum.Integer), out primeVar)
                    || primeVar > comparedVar)
            {
                InputOutput.TextWriteLine(awareMessage, ConsoleColor.Red);
            }

            return primeVar;
        }

        /// <summary>
        /// Метод добавления транспорта в список
        /// </summary>
        /// <param name="typeList">Список с типами транспорта</param>
        /// <param name="vehicleInfo">Строка с типами транспорта</param>
        /// <param name="dictionaryInfo">Словарь типов транспорта</param>
        /// <param name="vehicleList">Список объектов транспорта</param>
        private static void VehicleAdd(Type[] typeList, string vehicleInfo, 
            Dictionary<Type, string> dictionaryInfo, List<VehicleBase> vehicleList)
        {
            for (int j = 0; j < typeList.Length - 1; j++)
            {
                vehicleInfo += "\n" + $"{j + 1} - " + dictionaryInfo[typeList[j]];
            }
            InputOutput.TextWriteLine(vehicleInfo, ConsoleColor.Yellow);
            var vehicleType = int.Parse(InputOutput.Input(null, InputOutput.InputTypeEnum.Integer));
            switch (vehicleType)
            {
                case 1:
                    vehicleList.Add(new Car());
                    break;
                case 2:
                    vehicleList.Add(new Helicopter());
                    break;
                case 3:
                    vehicleList.Add(new HybridCar());
                    break;
                default:
                    throw new Exception("Введённое число больше числа существующих типов транспорта!");
            }
        }

        /// <summary>
        /// Метод для ввода объектов транспорта списка vehicleList
        /// </summary>
        /// <param name="number">Число объектов в списке</param>
        /// <param name="typeList">Строка с типами транспорта</param>
        /// <param name="dictionaryInfo">Словарь типов транспорта</param>
        /// <param name="vehicleList">Список объектов транспорта</param>
        private static void VehicleListAdd(int number, Type[] typeList,
            Dictionary<Type, string> dictionaryInfo, List<VehicleBase> vehicleList)
        {
            for (int i = 0; i < number; i++)
            {
                string vehicleInfo = $"Введите тип {i + 1}-го транспорта: ";
                MethodHandler(() => VehicleAdd(typeList, vehicleInfo, dictionaryInfo, vehicleList));
            }
        }

        /// <summary>
        /// Метод печати списка объектов транспорта
        /// </summary>
        /// <param name="vehicleList">Список объектов транспорта</param>
        private static void VehicleListWrite(List<VehicleBase> vehicleList)
        {
            foreach (VehicleBase vehicle in vehicleList)
            {
                InputOutput.VehicleWrite(vehicle.GetType().Name, vehicle);
            }
        }

        /// <summai        /// Метод ввода параметров объектов в списке отов транспорта vehicleList
        /// </summary>
        /// <param name="dictionaryInfo">Словарь типов транспорта</param>
        /// <param name="vehicleList">Список объектов транспорта</param>
        private static void VehicleListInput(Dictionary<Type, string> dictionaryInfo, List<VehicleBase> vehicleList)
        {
            for (var index = 0; index < vehicleList.Count; index++)
            {
                var vehicle = vehicleList[index];
                InputOutput.TextWriteLine($"{index + 1}-й транспорт: "
                                          + dictionaryInfo[vehicle.GetType()], ConsoleColor.Yellow);
                InputOutput.InputVehicle(vehicle);
            }
        }

        /// <summary>
        /// Метод вывода информации о потреблении топлива в зависимости от расстояния для всего списка объектов транспорта
        /// </summary>
        /// <param name="vehicleList">Список объектов транспорта</param>
        private static void VehicleConsumption(List<VehicleBase> vehicleList)
        {
            for (int i = 0; i < vehicleList.Count(); i++)
            {
                double distance = double.Parse(InputOutput.Input(
                    $"Введите пройденную дистанцию {i + 1}-го транспорта: ",
                    InputOutput.InputTypeEnum.Digit));
                InputOutput.TextWriteLine($"Потребление {i + 1}-го транспорта на {distance} км: " +
                                          $"{vehicleList[i].Consumption(distance):0.###}" + " л", ConsoleColor.White);
            }
        }

        /// <summary>
        /// Метод вывода информации о потреблении топлива в зависимости 
        /// от начальной скорости и времени движения для всего списка объектов транспорта
        /// </summary>
        /// <param name="vehicleList">Список объектов транспорта</param>
        private static void VehicleConsumptionByTime(List<VehicleBase> vehicleList)
        {
            for (int i = 0; i < vehicleList.Count(); i++)
            {
                double velocity = double.Parse(InputOutput.Input($"Введите начальную скорость {i + 1}-го транспорта (км/ч): ",
                    InputOutput.InputTypeEnum.Digit));
                double time = double.Parse(InputOutput.Input($"Введите время движения {i + 1}-го транспорта, с: ",
                    InputOutput.InputTypeEnum.Digit));
                double distance = vehicleList[i].Distance(velocity, time);
                InputOutput.TextWriteLine($"Потребление {i + 1}-го транспорта с начальной скоростью "
                    + $"{velocity} км/ч за {time} c: "
                    + string.Format("{0:0.###}", vehicleList[i].Consumption(distance)) + " л", ConsoleColor.White);
            }
        }

        /// <summary>
        /// Метод для обработки исключений
        /// </summary>
        /// <param name="action">Метод, в котором обработываются возможные исключения</param>
        private static void MethodHandler(Action action)
        {
            while (true)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception exception)
                {
                    InputOutput.TextWriteLine(exception.Message, ConsoleColor.Red);
                }
            }
        }
    }
}
