﻿using SPAD.neXt.Interfaces.Base;
using SPAD.neXt.Interfaces.Configuration;
using SPAD.neXt.Interfaces.Profile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAD.neXt.Interfaces.Events
{

    public interface IEventManager
    {

        void UnsubscribeFromSystemEvent(string eventName, SPADEventHandler oldDeleagte);
        void SubscribeToSystemEvent(string eventName, SPADEventHandler newDelegate, EventPriority priority = EventPriority.Low);
        //void Subscribe(string eventName, IEventHandler eventDelegate, ISPADComparator comparator);
        //void Unsubscribe(IMonitorableValue monitorableValue, string eventName, IEventHandler eventDelegate);
        event PropertyChangedEventHandler AircraftChanged;

        void Raise(object sender, string eventname, ISPADEventArgs e);

        void RerouteVariable(string target, string routedTarget);
        bool IsRegisteredVariable(string tag);
        IMonitorableValue GetRegisteredVariable(string tag);
        IMonitorableValue CreateMonitorableValue(string Name, VARIABLE_SCOPE scope = VARIABLE_SCOPE.SESSION, object defaultValue = null);
        IDataDefinition GetDataDefinition(string name);
        void RemoveRegisteredVariable(string variableName, VARIABLE_SCOPE scope = VARIABLE_SCOPE.SESSION);
        List<string> GetAllMonitoredValues();        

    }

    public interface IEventHandler
    {
        void HandleEvent(ISPADEventArgs e);
    }

    public interface IObserverTicket : IDisposable
    {
        Guid ID { get; }
        string EventName { get; }
        string SubscriptionID { get; }
        int Priority { get; }
        object UserData { get; set; }
        bool IsStatic { get; set; }
        bool NeedNotify { get; }

        void Subscribe(IMonitorableValue monitorableValue);
        void Unsubscribe(IMonitorableValue monitorableValue);

        IObserverTicket AsStatic();
    }

    public interface IValueTranscriber
    {
        IMonitorableValue TranscribeValue(string varDatum, ref string varUnit, ref string convExpression);
        IDataDefinition TranscribeCommand(string commandName);

        string TranscribeUnit(string inUnit);

        double ConvertValue(string expression, double value);
        double ConvertUnit(double value, string unitIn, string unitOut);

    }

    public interface IValueProviderInfomation
    {
        string Name { get; }
        string DisplayName { get; }
        string Information { get; }
        string StatusInformation { get; }
        IValueProvider Provider { get; }
        bool IsVisible { get; }
        bool IsConnected { get; }
    }

    public interface IValueConnector : ISimulationEventProvider
    {
        bool SupportsDynamicDefinitions { get; }
        bool IsConnected { get; }
        void ForceUpdate(string dataRef, bool doMonitor);
        void SetValue(string dataRef, double newValue);
        void ExecuteCommand(string commandRef, uint parameter);
        void SendMessage(string message);
        void Stop();

    }

    public interface ISimConnectValueProvider : IValueProvider
    { }

    public interface ICDUValueProvider
    {
        void SendCDUControl(uint control, uint parameter);
    }

    public interface IStaticValueProvider
    {
        
    }

    public interface IDeviceValueProvider
    {
        string CustomizeDatadefintion(string dataRef, IDeviceProfile deviceProfile);
    }

    public interface IValueProvider
    {
        string Name { get; }
        string StatusInformation { get; }
        bool IsInitialized { get; }
        bool IsPaused { get; }
        bool IsVisible { get; }
        bool IsConnected { get; }

        string ExtraStatusInformation { get; }

        object GetValue(IMonitorableValue value);
        void SetValue(IMonitorableValue value, Guid sender, int delay = 0);

        void SendControl(IDataDefinition control, uint parameter);

        void ForceUpdate(IMonitorableValue value);
        void StartMonitoring(IMonitorableValue value);
        void StopMonitoring(IMonitorableValue value);
        bool IsMonitoring(IMonitorableValue value);

        void Initialize();
        void Pause();
        void Continue();

        void EventCallback(object callbackvalue);
        IDataDefinition CreateDynamic(string name, string normalizer = null, VARIABLE_SCOPE scope = VARIABLE_SCOPE.SESSION, object defaultValue = null);
        void RemoveDynamic(string name, VARIABLE_SCOPE scope);
        void SendMessage(string message);
    }

    public interface ISessionValueProvider : IValueProvider
    {
        event EventHandler<string, bool> MonitoringChanged;
        void UpdateValue(string name, object newVal, object oldVal, Guid sender);
    }

    public interface ITransparentValueProvider
    {

        event EventHandler DataUpdated;
        string Name { get; }
        ulong GetLastChange();
        bool HasValue(string valueName);
        double GetValue(string valueName);
        void SetValue(string valueName, double value);
        IEnumerable<string> GetAllValueNames(Func<string, bool> predicate = null);
        void StartUpdates();
    }

    public interface ISimulationInterface
    {
        bool IsConnected { get; }
        bool HasConnectionStatusChanged { get; }

        SimulationGamestate SimulationGamestate { get; }
    }

    public interface ISimulationController : ISimulationInterface
    {
        void InitController();
        void StartProcessing();
        void StopProcessing();
        void PauseProcessing();
        void ContinueProcessing();

    }

    public interface ISimulationEventProvider
    {
        /*
        event EventHandler Connected;
        event EventHandler Disconnected;
        event EventHandler AircraftLoaded;
        */
        event EventHandler<SPADEventArgs> ClientEvent;

        void StartMonitoringEvents();
        void StopMonitoringEvents();
    }

    public interface ICacheableValue 
    {
        int CacheIndex { get; }
        void SetCacheIndex(int idx);
    }

    public interface IMonitorableValue : IDisposable, IComparer<IMonitorableValue>, ICacheableValue
    {
        Guid ID { get; }
        Guid Owner { get; set; }
        string Name { get; }
        string InternalName { get; set; }
        object CurrentValue { get; }
        object PreviousValue { get; }
        double MinimumChange { get; }
        bool IsDisposed { get; }
        VARIABLE_SCOPE Scope { get; set; }
        ValueDataTypes ValueDataType { get; set; }
        IValueProvider ValueProvider { get; }
        IValueNormalizer ValueNormalizer { get; }
        IDataDefinition DataDefinition { get; }
        event EventHandler<BooleanEventArgs> MonitoredChanged;

        void SetRawValue(object newValue);
        Double ConvertValue(object newValue);
        object GetRawValue(bool secondary = false);
        string GetValueString();

        bool AsBool { get; set; }
        byte AsByte { get; set; }
        double AsDouble { get; set; }
        decimal AsDecimal { get; set; }
        short AsInt16 { get; set; }
        int AsInt32 { get; set; }
        long AsInt64 { get; set; }
        sbyte AsSByte { get; set; }
        float AsSingle { get; set; }
        string AsString { get; set; }
        ushort AsUInt16 { get; set; }
        uint AsUInt32 { get; set; }
        ulong AsUInt64 { get; set; }


        bool HasChanged();

        void SetValue(object newValue, int delay = 0, Guid? sender = null);
        Decimal ChangeValue(Decimal valChange);

        void StartMonitoring();
        void StopMonitoring();
        void ForceUpdate();

        bool IsActive { get; }
        bool IsUndefined();
        bool HasObservers { get; }

        void Subscribe(IObserverTicket observerTicket);
        void Unsubscribe(IObserverTicket observerTicket);
        IObserverTicket Subscribe(string subscriptionID, string eventName, ISPADEventDelegate eventDelegate, int priority = 0);

        void Raise(string eventName, object sender, ISPADEventArgs eventArgs);

        void SetPassive(); // This Monitorable will neever raise an event
        bool IsPassive { get; }
        bool NeedEvent { get; }
        bool NeedsMonitoring { get; }
        bool AlwaysUpdate { get; set; }
        bool DebugMonitorable { get; set; }
    }

    public enum ValueDataTypes
    {
        Unkown,
        Byte,
        Int16,
        Int32,
        Int64,
        Double,
        Single,
        ByteArray,
        UInt16,
        UInt32,
        UInt64,
        String,
        BitArray,
        SByte,
        Bit
    }

}
