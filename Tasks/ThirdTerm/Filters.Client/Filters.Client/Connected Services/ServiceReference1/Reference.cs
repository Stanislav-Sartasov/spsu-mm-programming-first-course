﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторного создания кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceReference
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.IFilterService", CallbackContract=typeof(ServiceReference.IFilterServiceCallback))]
    public interface IFilterService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFilterService/GetFilters", ReplyAction="http://tempuri.org/IFilterService/GetFiltersResponse")]
        string[] GetFilters();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFilterService/GetFilters", ReplyAction="http://tempuri.org/IFilterService/GetFiltersResponse")]
        System.Threading.Tasks.Task<string[]> GetFiltersAsync();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFilterService/ApplyFilter")]
        void ApplyFilter(byte[] img, string filter);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFilterService/ApplyFilter")]
        System.Threading.Tasks.Task ApplyFilterAsync(byte[] img, string filter);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFilterService/StopFiltering")]
        void StopFiltering();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFilterService/StopFiltering")]
        System.Threading.Tasks.Task StopFilteringAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface IFilterServiceCallback
    {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFilterService/ProgressCallback")]
        void ProgressCallback(int progress);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IFilterService/ImageCallback")]
        void ImageCallback(byte[] img);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface IFilterServiceChannel : ServiceReference.IFilterService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class FilterServiceClientBase : System.ServiceModel.DuplexClientBase<ServiceReference.IFilterService>, ServiceReference.IFilterService
    {
        
        /// <summary>
        /// Реализуйте этот разделяемый метод для настройки конечной точки службы.
        /// </summary>
        /// <param name="serviceEndpoint">Настраиваемая конечная точка</param>
        /// <param name="clientCredentials">Учетные данные клиента.</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public FilterServiceClientBase(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance, FilterServiceClientBase.GetDefaultBinding(), FilterServiceClientBase.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.NetTcpBinding_IFilterService.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public FilterServiceClientBase(System.ServiceModel.InstanceContext callbackInstance, EndpointConfiguration endpointConfiguration) : 
                base(callbackInstance, FilterServiceClientBase.GetBindingForEndpoint(endpointConfiguration), FilterServiceClientBase.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public FilterServiceClientBase(System.ServiceModel.InstanceContext callbackInstance, EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(callbackInstance, FilterServiceClientBase.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public FilterServiceClientBase(System.ServiceModel.InstanceContext callbackInstance, EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, FilterServiceClientBase.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public FilterServiceClientBase(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress)
        {
        }
        
        public string[] GetFilters()
        {
            return base.Channel.GetFilters();
        }
        
        public System.Threading.Tasks.Task<string[]> GetFiltersAsync()
        {
            return base.Channel.GetFiltersAsync();
        }
        
        public void ApplyFilter(byte[] img, string filter)
        {
            base.Channel.ApplyFilter(img, filter);
        }
        
        public System.Threading.Tasks.Task ApplyFilterAsync(byte[] img, string filter)
        {
            return base.Channel.ApplyFilterAsync(img, filter);
        }
        
        public void StopFiltering()
        {
            base.Channel.StopFiltering();
        }
        
        public System.Threading.Tasks.Task StopFilteringAsync()
        {
            return base.Channel.StopFilteringAsync();
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.NetTcpBinding_IFilterService))
            {
                System.ServiceModel.NetTcpBinding result = new System.ServiceModel.NetTcpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.Security.Mode = System.ServiceModel.SecurityMode.None;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Не удалось найти конечную точку с именем \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.NetTcpBinding_IFilterService))
            {
                return new System.ServiceModel.EndpointAddress("net.tcp://localhost:8000/srv");
            }
            throw new System.InvalidOperationException(string.Format("Не удалось найти конечную точку с именем \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return FilterServiceClientBase.GetBindingForEndpoint(EndpointConfiguration.NetTcpBinding_IFilterService);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return FilterServiceClientBase.GetEndpointAddress(EndpointConfiguration.NetTcpBinding_IFilterService);
        }
        
        public enum EndpointConfiguration
        {
            
            NetTcpBinding_IFilterService,
        }
    }
    
    public class ProgressCallbackReceivedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        
        private object[] results;
        
        public ProgressCallbackReceivedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState)
        {
            this.results = results;
        }
        
        public int progress
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    public class ImageCallbackReceivedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        
        private object[] results;
        
        public ImageCallbackReceivedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState)
        {
            this.results = results;
        }
        
        public byte[] img
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
    
    public partial class FilterServiceClient : FilterServiceClientBase
    {
        
        public FilterServiceClient(EndpointConfiguration endpointConfiguration) : 
                this(new FilterServiceClientCallback(), endpointConfiguration)
        {
        }
        
        private FilterServiceClient(FilterServiceClientCallback callbackImpl, EndpointConfiguration endpointConfiguration) : 
                base(new System.ServiceModel.InstanceContext(callbackImpl), endpointConfiguration)
        {
            callbackImpl.Initialize(this);
        }
        
        public FilterServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                this(new FilterServiceClientCallback(), binding, remoteAddress)
        {
        }
        
        private FilterServiceClient(FilterServiceClientCallback callbackImpl, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(new System.ServiceModel.InstanceContext(callbackImpl), binding, remoteAddress)
        {
            callbackImpl.Initialize(this);
        }
        
        public FilterServiceClient() : 
                this(new FilterServiceClientCallback())
        {
        }
        
        private FilterServiceClient(FilterServiceClientCallback callbackImpl) : 
                base(new System.ServiceModel.InstanceContext(callbackImpl))
        {
            callbackImpl.Initialize(this);
        }
        
        public event System.EventHandler<ProgressCallbackReceivedEventArgs> ProgressCallbackReceived;
        
        public event System.EventHandler<ImageCallbackReceivedEventArgs> ImageCallbackReceived;
        
        private void OnProgressCallbackReceived(object state)
        {
            if ((this.ProgressCallbackReceived != null))
            {
                object[] results = ((object[])(state));
                this.ProgressCallbackReceived(this, new ProgressCallbackReceivedEventArgs(results, null, false, null));
            }
        }
        
        private void OnImageCallbackReceived(object state)
        {
            if ((this.ImageCallbackReceived != null))
            {
                object[] results = ((object[])(state));
                this.ImageCallbackReceived(this, new ImageCallbackReceivedEventArgs(results, null, false, null));
            }
        }
        
        private class FilterServiceClientCallback : object, IFilterServiceCallback
        {
            
            private FilterServiceClient proxy;
            
            public void Initialize(FilterServiceClient proxy)
            {
                this.proxy = proxy;
            }
            
            public void ProgressCallback(int progress)
            {
                this.proxy.OnProgressCallbackReceived(new object[] {
                            progress});
            }
            
            public void ImageCallback(byte[] img)
            {
                this.proxy.OnImageCallbackReceived(new object[] {
                            img});
            }
        }
    }
}
