﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WinFormApp.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.AnalyzeResumeSoap")]
    public interface AnalyzeResumeSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/HelloWorld", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string HelloWorld();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/HelloWorld", ReplyAction="*")]
        System.Threading.Tasks.Task<string> HelloWorldAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ExtractResume", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string[] ExtractResume(string filePath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ExtractResume", ReplyAction="*")]
        System.Threading.Tasks.Task<string[]> ExtractResumeAsync(string filePath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CreateFile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        bool CreateFile(string fileName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CreateFile", ReplyAction="*")]
        System.Threading.Tasks.Task<bool> CreateFileAsync(string fileName);
        
        // CODEGEN: Parameter 'buffer' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Append", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        WinFormApp.ServiceReference1.AppendResponse Append(WinFormApp.ServiceReference1.AppendRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Append", ReplyAction="*")]
        System.Threading.Tasks.Task<WinFormApp.ServiceReference1.AppendResponse> AppendAsync(WinFormApp.ServiceReference1.AppendRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Verify", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        bool Verify(string fileName, string md5);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Verify", ReplyAction="*")]
        System.Threading.Tasks.Task<bool> VerifyAsync(string fileName, string md5);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Append", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class AppendRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string fileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] buffer;
        
        public AppendRequest() {
        }
        
        public AppendRequest(string fileName, byte[] buffer) {
            this.fileName = fileName;
            this.buffer = buffer;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="AppendResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class AppendResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string AppendResult;
        
        public AppendResponse() {
        }
        
        public AppendResponse(string AppendResult) {
            this.AppendResult = AppendResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface AnalyzeResumeSoapChannel : WinFormApp.ServiceReference1.AnalyzeResumeSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AnalyzeResumeSoapClient : System.ServiceModel.ClientBase<WinFormApp.ServiceReference1.AnalyzeResumeSoap>, WinFormApp.ServiceReference1.AnalyzeResumeSoap {
        
        public AnalyzeResumeSoapClient() {
        }
        
        public AnalyzeResumeSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AnalyzeResumeSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AnalyzeResumeSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AnalyzeResumeSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string HelloWorld() {
            return base.Channel.HelloWorld();
        }
        
        public System.Threading.Tasks.Task<string> HelloWorldAsync() {
            return base.Channel.HelloWorldAsync();
        }
        
        public string[] ExtractResume(string filePath) {
            return base.Channel.ExtractResume(filePath);
        }
        
        public System.Threading.Tasks.Task<string[]> ExtractResumeAsync(string filePath) {
            return base.Channel.ExtractResumeAsync(filePath);
        }
        
        public bool CreateFile(string fileName) {
            return base.Channel.CreateFile(fileName);
        }
        
        public System.Threading.Tasks.Task<bool> CreateFileAsync(string fileName) {
            return base.Channel.CreateFileAsync(fileName);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WinFormApp.ServiceReference1.AppendResponse WinFormApp.ServiceReference1.AnalyzeResumeSoap.Append(WinFormApp.ServiceReference1.AppendRequest request) {
            return base.Channel.Append(request);
        }
        
        public string Append(string fileName, byte[] buffer) {
            WinFormApp.ServiceReference1.AppendRequest inValue = new WinFormApp.ServiceReference1.AppendRequest();
            inValue.fileName = fileName;
            inValue.buffer = buffer;
            WinFormApp.ServiceReference1.AppendResponse retVal = ((WinFormApp.ServiceReference1.AnalyzeResumeSoap)(this)).Append(inValue);
            return retVal.AppendResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WinFormApp.ServiceReference1.AppendResponse> WinFormApp.ServiceReference1.AnalyzeResumeSoap.AppendAsync(WinFormApp.ServiceReference1.AppendRequest request) {
            return base.Channel.AppendAsync(request);
        }
        
        public System.Threading.Tasks.Task<WinFormApp.ServiceReference1.AppendResponse> AppendAsync(string fileName, byte[] buffer) {
            WinFormApp.ServiceReference1.AppendRequest inValue = new WinFormApp.ServiceReference1.AppendRequest();
            inValue.fileName = fileName;
            inValue.buffer = buffer;
            return ((WinFormApp.ServiceReference1.AnalyzeResumeSoap)(this)).AppendAsync(inValue);
        }
        
        public bool Verify(string fileName, string md5) {
            return base.Channel.Verify(fileName, md5);
        }
        
        public System.Threading.Tasks.Task<bool> VerifyAsync(string fileName, string md5) {
            return base.Channel.VerifyAsync(fileName, md5);
        }

    }
}