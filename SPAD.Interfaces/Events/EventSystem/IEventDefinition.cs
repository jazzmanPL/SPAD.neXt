﻿using SPAD.neXt.Interfaces;
using SPAD.neXt.Interfaces.Configuration;
using SPAD.neXt.Interfaces.Events;
using SPAD.neXt.Interfaces.Profile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SPAD.neXt.Interfaces.Events
{

    public interface IEventBaseObject
    {
        IEventDefinition GetParentEventDefinition();
        IDataDefinition GetDataDefinition(string definitionID);


    }

    public interface IEventDefinitions : IObservableList<IEventDefinition>, ICloneableWithID<IEventDefinitions>, IConditionalSerialize
    {
        string BoundTo { get; set; }
        string ConfigString { get; }
        string Name { get; set; }
        bool DebugMode { get; set; }
        void Configure(ISPADBaseEvent baseEvent, IDeviceProfile deviceProfile);
        void Activate(string boundTo, ISPADBaseEvent baseEvent);
        void Deactivate(string boundTo, ISPADBaseEvent baseEvent);
        IReadOnlyList<IEventDefinition> GetByTrigger(string trigger, bool create = true);
        IEventDefinition CreateEvent(string trigger, Guid? singletonID = null);
        IEventDefinition GetByID(Guid eventDefinitionID);
        void RemoveAll(IEnumerable<IEventDefinition> enumerable);
    }
    public interface IEncoderAcceleration
    {
        Double Threshold { get; set; }
        Double Timeout { get; set; }
        Double Multiplier { get; set; }
        Double Max { get; set; }

    }

    public interface IEventDefinition : IEventOptions, ICloneableWithID<IEventDefinition>, IConditionalSerialize
    {
        Guid SingletonID { get; set; } 
        bool IsSingleton { get; }
        IEventActions Actions { get; }
        string BoundTo { get; }
        SPADConditionBinding ConditionBinding { get; set; }
        bool ConditionRepeat { get; set; }
        IEventConditions Conditions { get; }
        string ConfigString { get; }
        string ConfigStringToolTip { get; }
        string EventName { get; }
        bool IsFinal { get; set; }
        bool DebugMode { get; set; }
        string Comment { get; set; }
        string DisplayName { get; }
        string Trigger { get; set; }
        int Priority { get; }
        bool IgnorePowerState { get; }
        bool EnableAcceleration { get; }
        bool IsDisabled { get; set; }
        bool IsEnabled { get; set; }
        bool IsDeprecated { get; set; }
        bool HasWarning { get; }
        bool EventIsRunning { get; }
        string  WarningMessage { get;  }
        IEncoderAcceleration Acceleration { get; }

        InputModifier InputBehavior { get; set; }
        ISPADBaseEvent BaseEvent { get; }
        IDeviceProfile DeviceProfile { get;  }
        
        void Configure(IEventDefinitions eventDefinitions, ISPADBaseEvent baseEvent, IDeviceProfile deviceProfile);
        bool Activate(ISPADBaseEvent baseEvent);
        void Deactivate(ISPADBaseEvent baseEvent);
        void Execute(ISPADEventArgs e, bool force = false);
        void SetBinding(string boundTo);
        void SetDisplayName(string displayNameResource);
        void SetVariableNameTransformFunc(Func<IEventDefinition, string, string> transformFunc);
        string TransformVariableName(string varName);

        // Creating new actions
        void AddAction(IEventAction action, bool singleton = true);
        IEventAction CreateCommandAction(string command, string parameter = null);

        bool IsAction(SPADEventActions actionType);
        IEventAction GetFirstAction(SPADEventActions actionType);

    }

    public interface IEventCondition : IEventBaseObject,IHasID
    {
        string ConfigString { get; }
        bool IsConfigured { get; }
        void SetDirty();
        void Activate();
        void Deactivate();
        bool CheckConfiguration(List<string> tmpError);
        bool Evaluate(ISPADEventArgs e);
    }

    public interface IEventEventCondition : IEventCondition
    {
        string BoundEvent { get; set; }
    }

    public interface IEventConditionExpression : ICloneable<IEventConditionExpression>,IEventCondition,IIsMonitorable
    {
        string Expression { get; set; }
    }

    public interface IEventConditionSimple : ICloneableWithID<IEventConditionSimple>, IEventCondition
    {
        SPADEventValueComparator ConditionComparator { get; set; }

        bool ConditionValueSourceEnabled { get; }
        IDataDefinition ConditionValueSource { get; set; }
        string ConditionValueSourceString { get; set; }

        bool ConditionTargetValueEnabled { get; }
        string ConditionTargetValue { get; set; }
        IDataDefinition ConditionTargetValueSource { get; set; }
        string ConditionTargetValueID { get; set; }
        ActionReferenceTypes ConditionType { get; set; }
        IReadOnlyList<IDataDefinition> ObservedDataDefinitions { get; }
        IEventConditionSimple SelfProperty { get; }

        void UpdateSelfProperty();
    }

    public interface IEventConditions : IObservableList<IEventCondition>
    {
        string ConfigString { get; }
        bool Evaluate(ISPADEventArgs e,SPADConditionBinding binding, bool debugMode);
    }

    public interface IEventActions : IObservableList<IEventAction>
    {        
        string ConfigString { get; }

        bool Execute(IEventDefinition definition, ISPADEventArgs e);

        IEventAction GetBySingleton(Guid id);
    }

    public interface IEventAction : INotifyPropertyChanged , IEventBaseObject,IEventOptions, ICloneableWithID<IEventAction>, IConditionalSerialize
    {
        SPADEventActions ActionID { get; }
        string ConfigID { get; }
        string ConfigString { get; }
        string Documentation { get; }
        string ActionHash { get; }
        bool IsDirty { get; set; }
        bool IsNew { get; }
        bool CanBeDeleted { get; set; }
        bool CanTargetDevice { get; set; }
        bool CanTargetSwitch { get; set; }
        string TargetDeviceID { get; set; }
        string RawTargetDeviceID { get; }
        string TargetSwitchName { get; set; }
        string DataReferenceText { get; }
        bool IsInEditMode { get; set; }
        void AddParserValue(string key, string value);
        int TransformValue(AxisEventValue e);
        bool Execute(IEventDefinition definition, ISPADEventArgs e);
        bool Equals(IEventAction other);
        bool CheckConfiguration(List<string> errorContainer);
        
        IDeviceProfile GetTargetDevice(IEventDefinition def);
        void SetEventValueCallback(string valueName,IEventValueCallback callback);
        void SetEventTargetCallback(string targetName, IEventTargetCallback callback);
    }

    public interface IEventValueCallback
    {
        object GetEventValue(string name);
    }

    public interface IEventTargetCallback
    {
        string GetEventTargetName(string name);
    }

    public interface IEventContext
    {
        ISPADBaseEvent EventContext { get; }
        IEventDefinition EventDefintionContext { get; set; }
    }

    public interface IEventOptions
    {
        T GetOption<T>(string optionName, T defaultValue = default(T));
        bool HasOption(string optionName);
        void SetOption(string optionName, object value);

    }
}