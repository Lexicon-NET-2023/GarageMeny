﻿using Exercise5.Garage;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace Exercise5
{
    public class GarageHandler
    {
        private Garage<Vehicle> garage;
        private Stack<Menu> menuStack;
        private EventLog log;
        private ConsoleUI ui;
        private const string newlines = "\n\n";

        public GarageHandler()
        {
            garage = new Garage<Vehicle>(8);
            log = new EventLog(12);
            menuStack = new Stack<Menu>();
            ui = new ConsoleUI();
        }

        public void Run()
        {
            Initialize();

            while (true)
            {
                var menu = menuStack.Peek();
                var choice = menu.Run();
                choice.Action?.Invoke(); // if the selected menu option has an action...
                if (choice.SubMenu != null) // ...or has a sub menu
                {
                    choice.SubMenu.ResetCursorIndex();
                    menuStack.Push(choice.SubMenu);
                    ui.Clear();
                }
            }
        }

        private void Initialize()
        {
            Menu.Init(ui, log);
            var mainMenu = GetMenuTree();
            menuStack.Push(mainMenu);
        }

        private void Quit()
        {
            ui.Clear();
            Environment.Exit(0);
        }

        private void GoBack()
        {
            ui.Clear();
            menuStack.Pop();
        }


        /// <summary>
        /// Gets the entire navigation tree
        /// </summary>
        /// <returns>Returns the tree in the form of a menu object</returns>
        private Menu GetMenuTree()
        {
            var quitOption = new MenuOption("Quit the application", new Action(Quit));
            var backOption = new MenuOption("Go back", new Action(GoBack));

            var parkingSingleMenu = Menu.Create("Parking a single vehicle");
            parkingSingleMenu.Add(new MenuOption("Park a car", new Action(ParkCar)));
            parkingSingleMenu.Add(new MenuOption("Park a bus", new Action(ParkBus)));
            parkingSingleMenu.Add(new MenuOption("Park an airplane", new Action(ParkAirplane)));
            parkingSingleMenu.Add(new MenuOption("Park a motorcycle", new Action(ParkMotorcycle)));
            parkingSingleMenu.Add(new MenuOption("Park a boat", new Action(ParkBoat)));
            parkingSingleMenu.Add(backOption);

            var parkingMenu = Menu.Create("Parking new vehicles");
            parkingMenu.Add(new MenuOption("Park single vehicle", parkingSingleMenu));
            parkingMenu.Add(new MenuOption("Fill garage with 5 vehicles", new Action(ParkSomeVehicles)));
            parkingMenu.Add(backOption);

            var vehicleSearchMenu = Menu.Create("Search vehicles");
            vehicleSearchMenu.Add(new MenuOption("Search via registration number", new Action(RegNoSearch)));
            vehicleSearchMenu.Add(new MenuOption("Search via parameters", new Action(ParametricSearch)));
            vehicleSearchMenu.Add(backOption);

            var garageAdminMenu = Menu.Create("Building construction");
            garageAdminMenu.Add(new MenuOption("Create new garage", new Action(CreateGarage)));
            garageAdminMenu.Add(backOption);

            var listSpecificMenu = Menu.Create("Listing specific vehicle types");
            listSpecificMenu.Add(new MenuOption("List airplanes", new Action(ListAirplanes)));
            listSpecificMenu.Add(new MenuOption("List boats", new Action(ListBoats)));
            listSpecificMenu.Add(new MenuOption("List busses", new Action(ListBusses)));
            listSpecificMenu.Add(new MenuOption("List cars", new Action(ListCars)));
            listSpecificMenu.Add(new MenuOption("List motorcycles", new Action(ListMotorcycles)));
            listSpecificMenu.Add(backOption);

            var listingMenu = Menu.Create("Listing parked vehicles");
            listingMenu.Add(new MenuOption("List all parked vehicles", new Action(ListParkedVehicles)));
            listingMenu.Add(new MenuOption("List specific types", listSpecificMenu));
            listingMenu.Add(backOption);

            var mainMenu = Menu.Create("Garage ver 0.1    Main menu");
            mainMenu.Add(new MenuOption("Park vehicle", parkingMenu));
            mainMenu.Add(new MenuOption("Unpark vehicle", new Action(UnparkVehicle)));
            mainMenu.Add(new MenuOption("List parked vehicles", listingMenu));
            mainMenu.Add(new MenuOption("Search for vehicle", vehicleSearchMenu));
            mainMenu.Add(new MenuOption("Building management", garageAdminMenu));
            mainMenu.Add(quitOption);

            return mainMenu;
        }

        private void ListMotorcycles()
        {
            ui.DisplayVehicleList(garage, "Motorcycle");
        }

        private void ListCars()
        {
            ui.DisplayVehicleList(garage, "Car");
        }

        private void ListBusses()
        {
            ui.DisplayVehicleList(garage, "Bus");
        }

        private void ListBoats()
        {
            ui.DisplayVehicleList(garage, "Boat");
        }

        private void ListAirplanes()
        {
            ui.DisplayVehicleList(garage, "Airplane");
        }

        private void ListParkedVehicles()
        {
            ui.DisplayGarage(garage);
        }

        private void ParametricSearch()
        {

        }

        private void RegNoSearch()
        {
            //use parametric search!!! :-)
        }

        private void CreateGarage()
        {

        }

        private void ParkBoat()
        {
            if(PreparedForParking("Boat"))
            {
                var dto = GetVehicleParametersFromUser();
                int length = ui.GetIntegerFromUser("Length: ");
                var boat = new Boat(dto.RegNo, dto.Color, dto.NrOfWheels, dto.FuelType, length);
                ParkVehicle(boat, true);
            }
            WaitAndClear();
        }

        private void ParkMotorcycle()
        {
            if (PreparedForParking("Motorcycle"))
            {
                var dto = GetVehicleParametersFromUser();
                string make = ui.GetTextFromUser("Brand: ");
                var motorcycle = new Motorcycle(dto.RegNo, dto.Color, dto.NrOfWheels, dto.FuelType, make);
                ParkVehicle(motorcycle, true);
            }
            WaitAndClear();
        }

        private void ParkAirplane()
        {
            if (PreparedForParking("Airplane"))
            {
                var dto = GetVehicleParametersFromUser();
                int engines = ui.GetIntegerFromUser("Number of engines: ");
                var airplane = new Airplane(dto.RegNo, dto.Color, dto.NrOfWheels, dto.FuelType, engines);
                ParkVehicle(airplane, true);
            }
            WaitAndClear();
        }

        private void ParkBus()
        {
            if (PreparedForParking("Bus"))
            {
                var dto = GetVehicleParametersFromUser();
                int seats = ui.GetIntegerFromUser("Number of seats: ");
                var bus = new Bus(dto.RegNo, dto.Color, dto.NrOfWheels, dto.FuelType, seats);
                ParkVehicle(bus, true);
            }
            WaitAndClear();
        }

        private void ParkCar()
        {
            if (PreparedForParking("Car"))
            {
                var dto = GetVehicleParametersFromUser();
                string make = ui.GetTextFromUser("Car brand: ");
                var car = new Car(dto.RegNo, dto.Color, dto.NrOfWheels, dto.FuelType, make);
                ParkVehicle(car, true);
            }
            WaitAndClear();
        }

        private bool PreparedForParking(string type)
        {
            ui.Clear();
            ui.DisplayInputHeader($"Parking a vehicle - {type}");
            ui.WriteWarning(garage.IsFull() ? "\nThe garage is full!" : "");
            return !garage.IsFull();
        }

        private VehicleDto GetVehicleParametersFromUser()
        {
            string regNo;
            while(true)
            {
                regNo = ui.GetTextFromUser("New registration number: ");
                if(garage.GetVehicle(regNo) == null) // if not already in garage
                {
                    if(regNo != "")
                    {
                        break;
                    }
                }
                else
                {
                    ui.WriteWarning("That registration number is already in use!");
                }
            }

            string color = ui.GetTextFromUser("Color: ");
            int wheels = ui.GetIntegerFromUser("Nr of wheels: ");
            string fueltype = ui.GetTextFromUser("Fuel type: ");

            var dto = new VehicleDto(regNo, color, wheels, fueltype);
            return dto;
        }

        private void UnparkVehicle()
        {
            ui.Clear();
            ui.DisplayVehicleList(garage);
            ui.Write("\n");
            if (garage.Count > 0)
            {
                ui.DisplayInputHeader("Unparking a vehicle");
                string regNo = ui.GetTextFromUser("Enter registration number: ");
                if(regNo != "")
                {
                    Vehicle vehicle = garage.UnparkVehicle(regNo);
                    if (vehicle == null)
                    {
                        ui.WriteWarning("That vehicle does not exist!");
                    }
                    else
                    {
                        var text = $"{vehicle.RegNo} was successfully unparked.";
                        ui.WriteSuccess(text);
                        log.Add(text);
                    }
                }
            }
            else
            {
                ui.WriteWarning("There are no vehicles to unpark.");
            }
            WaitAndClear();
        }

        private ParkingResult ParkVehicle(Vehicle vehicle, bool verbose)
        {
            var result = garage.ParkVehicle(vehicle);
            if (result.Success)
            {
                var msg = $"{vehicle.RegNo} is now parked";
                log.Add(msg);
                ui.WriteSuccess((verbose) ? msg : "");
            }
            else
            {
                var msg = $"Could not park - {result.Message}.";
                log.Add(msg);
                ui.WriteWarning((verbose) ? msg : "");
            }
            return result;
        }

        void ParkSomeVehicles()
        {
            ParkVehicle(new Car("ABC123", "Red", 4, "Gasoline", "Nissan"), false);
            ParkVehicle(new Bus("XYZ456", "Green", 4, "Diesel", 38), false);
            ParkVehicle(new Airplane("SE-ABCD", "Blue", 3, "JetA1", 4), false);
            ParkVehicle(new Boat("M/S Lagunia", "Yellow", 0, "Diesel", 12), false);
            ParkVehicle(new Motorcycle("HOJ345", "Maroon", 2, "Gasoline", "Harley"), false);
        }

        private void WaitAndClear()
        {
            ui.PromptForKey(newlines);
            ui.Clear();
        }
    }
}
