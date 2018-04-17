/*
 * Created by SharpDevelop.
 * User: user
 * Date: 16/04/2018
 * Time: 12:55 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace soap_ws_client
{
	public abstract class SoapWsClient
	{
		protected string service_uri;
		protected string service_namespace;
		protected string xml_namespace;
		protected string xml_prefix;
		protected List<WsHeader> headers;
		
		protected SoapWsClient(string service_uri,
		                       string service_namespace,
		                       string xml_namespace,
		                       string xml_prefix)
		{
			this.service_uri = service_uri;
			this.service_namespace = service_namespace;
			this.xml_namespace = xml_namespace;
			this.xml_prefix = xml_prefix;
			this.headers = new List<WsHeader>();
		}
		
		public void addHeader(WsHeader header) {
			headers.Add(header);
		}
		
		public string callService(string methodName,
		                          Dictionary<string, object> parameters) {
			HttpWebRequest request;
			string requestContent;
			string rawResponseContent;
			
			request = createRequest(methodName);
			requestContent = createRequestContent(methodName, parameters);
			rawResponseContent = makeRequest(request, requestContent);
			return readContent(methodName, rawResponseContent);
		}
		
		protected abstract HttpWebRequest createRequest(string methodName);
		
		protected abstract string createRequestContent(string methodName,
		                                               Dictionary<string, object> parameters);
		
		private string makeRequest(HttpWebRequest webRequest,
		                           string requestContent) {
			IAsyncResult asyncResult;
			
			using (Stream stream = webRequest.GetRequestStream())
			{
				byte[] contentArray;

				contentArray = System.Text.Encoding.UTF8.GetBytes(requestContent);
				stream.Write(contentArray, 0, contentArray.Length);
			}
			asyncResult = webRequest.BeginGetResponse(null, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
			{
				using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
				{
					return reader.ReadToEnd();
				}
			}
		}
		
		protected abstract string readContent(string methodName, string rawResponseContent);
	}
}
