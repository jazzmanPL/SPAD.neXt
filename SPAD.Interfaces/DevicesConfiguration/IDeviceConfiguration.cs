﻿using SPAD.neXt.Interfaces.Base;
using System;
using System.Collections.Generic;
namespace SPAD.neXt.Interfaces.DevicesConfiguration
{
    public interface IDeviceConfiguration : IOptionsProvider
    {
        IReadOnlyList<IDeviceSwitch> DeviceSwitches { get; }
       
        
        string Name { get; }
        string PublishName { get; }
        string Panel { get; }
        string ProductID { get; }
        IDeviceVendor Vendor { get; }
        int ProductIDAsInt { get; }
        int Order { get; }
        bool ProcessDeviceData { get; }
        string DeviceType { get; }
        string DeviceMenu { get; }
        bool NoEventsAutoRemove { get; }
        bool HasDeviceSwitch(string name);

        void CreateVirtualInputs(IInputDevice gameDevice);
        void Clear();
        void Save();
        void AddDeviceSwitch(IDeviceSwitch newSwitch);
        bool AddDeviceSwitch(string xmlSwitchFragment);
        IDeviceSwitch GetDeviceSwitch(string name);
        IDeviceSwitch CreateDeviceSwitch();
    }

    public interface IDeviceCalibration
    {
        IReadOnlyList<IAxisCalibration> Axes { get; }

        void ApplyCalibration(IInputDevice joystick);
        IAxisCalibration GetAxisCalibration(IInputAxis axis);
        void ImportJoystick(IInputDevice joystick);
        void Save();
    }

    public interface IAxisCalibration
    {
        int Deadzone { get; set; }
        string DisplayName { get; set; }
        int Maximum { get; set; }
        int Minimum { get; set; }
        string Name { get; }
        int Smoothness { get; set; }
        int MinimumDelta { get; set; }

        void ApplyTo(IInputAxis axis);
        void ImportFrom(IInputAxis axis);

    }
}
