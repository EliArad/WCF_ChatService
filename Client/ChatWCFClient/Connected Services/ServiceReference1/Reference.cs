﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChatWCFClientApi.ServiceReference1 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Message", Namespace="http://schemas.datacontract.org/2004/07/ChatServiceLib")]
    [System.SerializableAttribute()]
    public partial class Message : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ContentField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid FromServerGuidField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SenderNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime TimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid ToReceiverServerGuidField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string toReceiverNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Content {
            get {
                return this.ContentField;
            }
            set {
                if ((object.ReferenceEquals(this.ContentField, value) != true)) {
                    this.ContentField = value;
                    this.RaisePropertyChanged("Content");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid FromServerGuid {
            get {
                return this.FromServerGuidField;
            }
            set {
                if ((this.FromServerGuidField.Equals(value) != true)) {
                    this.FromServerGuidField = value;
                    this.RaisePropertyChanged("FromServerGuid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SenderName {
            get {
                return this.SenderNameField;
            }
            set {
                if ((object.ReferenceEquals(this.SenderNameField, value) != true)) {
                    this.SenderNameField = value;
                    this.RaisePropertyChanged("SenderName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Time {
            get {
                return this.TimeField;
            }
            set {
                if ((this.TimeField.Equals(value) != true)) {
                    this.TimeField = value;
                    this.RaisePropertyChanged("Time");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid ToReceiverServerGuid {
            get {
                return this.ToReceiverServerGuidField;
            }
            set {
                if ((this.ToReceiverServerGuidField.Equals(value) != true)) {
                    this.ToReceiverServerGuidField = value;
                    this.RaisePropertyChanged("ToReceiverServerGuid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string toReceiverName {
            get {
                return this.toReceiverNameField;
            }
            set {
                if ((object.ReferenceEquals(this.toReceiverNameField, value) != true)) {
                    this.toReceiverNameField = value;
                    this.RaisePropertyChanged("toReceiverName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Client", Namespace="http://schemas.datacontract.org/2004/07/ChatServiceLib")]
    [System.SerializableAttribute()]
    public partial class Client : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FreeDescField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Guid ServerGuidField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime TimeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FreeDesc {
            get {
                return this.FreeDescField;
            }
            set {
                if ((object.ReferenceEquals(this.FreeDescField, value) != true)) {
                    this.FreeDescField = value;
                    this.RaisePropertyChanged("FreeDesc");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid ServerGuid {
            get {
                return this.ServerGuidField;
            }
            set {
                if ((this.ServerGuidField.Equals(value) != true)) {
                    this.ServerGuidField = value;
                    this.RaisePropertyChanged("ServerGuid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Time {
            get {
                return this.TimeField;
            }
            set {
                if ((this.TimeField.Equals(value) != true)) {
                    this.TimeField = value;
                    this.RaisePropertyChanged("Time");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IChatService", CallbackContract=typeof(ChatWCFClientApi.ServiceReference1.IChatServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IChatService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/GetVersion", ReplyAction="http://tempuri.org/IChatService/GetVersionResponse")]
        string GetVersion();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/GetVersion", ReplyAction="http://tempuri.org/IChatService/GetVersionResponse")]
        System.Threading.Tasks.Task<string> GetVersionAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Connect", ReplyAction="http://tempuri.org/IChatService/ConnectResponse")]
        bool Connect(string userName, string freedesc, System.Guid serverGuid, System.DateTime time);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Connect", ReplyAction="http://tempuri.org/IChatService/ConnectResponse")]
        System.Threading.Tasks.Task<bool> ConnectAsync(string userName, string freedesc, System.Guid serverGuid, System.DateTime time);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Say", ReplyAction="http://tempuri.org/IChatService/SayResponse")]
        bool Say(ChatWCFClientApi.ServiceReference1.Message msg);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Say", ReplyAction="http://tempuri.org/IChatService/SayResponse")]
        System.Threading.Tasks.Task<bool> SayAsync(ChatWCFClientApi.ServiceReference1.Message msg);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/Echo")]
        void Echo(ChatWCFClientApi.ServiceReference1.Message msg);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/Echo")]
        System.Threading.Tasks.Task EchoAsync(ChatWCFClientApi.ServiceReference1.Message msg);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Broadcast", ReplyAction="http://tempuri.org/IChatService/BroadcastResponse")]
        bool Broadcast(ChatWCFClientApi.ServiceReference1.Message msg);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Broadcast", ReplyAction="http://tempuri.org/IChatService/BroadcastResponse")]
        System.Threading.Tasks.Task<bool> BroadcastAsync(ChatWCFClientApi.ServiceReference1.Message msg);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/BroadcastServer", ReplyAction="http://tempuri.org/IChatService/BroadcastServerResponse")]
        bool BroadcastServer(ChatWCFClientApi.ServiceReference1.Message msg);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/BroadcastServer", ReplyAction="http://tempuri.org/IChatService/BroadcastServerResponse")]
        System.Threading.Tasks.Task<bool> BroadcastServerAsync(ChatWCFClientApi.ServiceReference1.Message msg);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/Disconnect")]
        void Disconnect(string userName, System.Guid ServerGuid);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/Disconnect")]
        System.Threading.Tasks.Task DisconnectAsync(string userName, System.Guid ServerGuid);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/UserJoin")]
        void UserJoin(ChatWCFClientApi.ServiceReference1.Client client);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/UserLeave")]
        void UserLeave(string userName, System.Guid serverGuid, System.DateTime time);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/RefreshClients")]
        void RefreshClients(ChatWCFClientApi.ServiceReference1.Client[] clients);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/ReceiveBroadcast")]
        void ReceiveBroadcast(ChatWCFClientApi.ServiceReference1.Message msg);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/Receive")]
        void Receive(ChatWCFClientApi.ServiceReference1.Message msg);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatServiceChannel : ChatWCFClientApi.ServiceReference1.IChatService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ChatServiceClient : System.ServiceModel.DuplexClientBase<ChatWCFClientApi.ServiceReference1.IChatService>, ChatWCFClientApi.ServiceReference1.IChatService {
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public string GetVersion() {
            return base.Channel.GetVersion();
        }
        
        public System.Threading.Tasks.Task<string> GetVersionAsync() {
            return base.Channel.GetVersionAsync();
        }
        
        public bool Connect(string userName, string freedesc, System.Guid serverGuid, System.DateTime time) {
            return base.Channel.Connect(userName, freedesc, serverGuid, time);
        }
        
        public System.Threading.Tasks.Task<bool> ConnectAsync(string userName, string freedesc, System.Guid serverGuid, System.DateTime time) {
            return base.Channel.ConnectAsync(userName, freedesc, serverGuid, time);
        }
        
        public bool Say(ChatWCFClientApi.ServiceReference1.Message msg) {
            return base.Channel.Say(msg);
        }
        
        public System.Threading.Tasks.Task<bool> SayAsync(ChatWCFClientApi.ServiceReference1.Message msg) {
            return base.Channel.SayAsync(msg);
        }
        
        public void Echo(ChatWCFClientApi.ServiceReference1.Message msg) {
            base.Channel.Echo(msg);
        }
        
        public System.Threading.Tasks.Task EchoAsync(ChatWCFClientApi.ServiceReference1.Message msg) {
            return base.Channel.EchoAsync(msg);
        }
        
        public bool Broadcast(ChatWCFClientApi.ServiceReference1.Message msg) {
            return base.Channel.Broadcast(msg);
        }
        
        public System.Threading.Tasks.Task<bool> BroadcastAsync(ChatWCFClientApi.ServiceReference1.Message msg) {
            return base.Channel.BroadcastAsync(msg);
        }
        
        public bool BroadcastServer(ChatWCFClientApi.ServiceReference1.Message msg) {
            return base.Channel.BroadcastServer(msg);
        }
        
        public System.Threading.Tasks.Task<bool> BroadcastServerAsync(ChatWCFClientApi.ServiceReference1.Message msg) {
            return base.Channel.BroadcastServerAsync(msg);
        }
        
        public void Disconnect(string userName, System.Guid ServerGuid) {
            base.Channel.Disconnect(userName, ServerGuid);
        }
        
        public System.Threading.Tasks.Task DisconnectAsync(string userName, System.Guid ServerGuid) {
            return base.Channel.DisconnectAsync(userName, ServerGuid);
        }
    }
}
