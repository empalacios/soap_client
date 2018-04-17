/*
 * Created by SharpDevelop.
 * User: user
 * Date: 15/04/2018
 * Time: 11:22 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace soap_ws_client
{
	public class Soap1_2WsClient : SoapWsClient
	{
		public Soap1_2WsClient(string service_uri,
		                       string service_namespace)
			: base(service_uri, service_namespace, "http://www.w3.org/2003/05/soap-envelope", "soap12")
		{
		}
		
		protected override HttpWebRequest createRequest(string methodName) {
			HttpWebRequest request = (HttpWebRequest) WebRequest.Create(service_uri);
			
			request.ContentType = "application/soap+xml; charset=utf-8";
			request.Accept = "application/soap+xml; charset=utf-8";
			request.Method = "POST";
			return request;
		}
		
		protected override string createRequestContent(string methodName,
		                                               Dictionary<string, object> parameters) {
			XmlDocument document = new XmlDocument();
			XmlElement rootElement;
			XmlElement bodyElement;
			XmlElement methodElement;

			rootElement = document.CreateElement(xml_prefix, "Envelope", xml_namespace);
			addAttribute(document, rootElement, "xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
			addAttribute(document, rootElement, "xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
			document.AppendChild(rootElement);
			
			bodyElement = document.CreateElement(xml_prefix, "Body", xml_namespace);
			rootElement.AppendChild(bodyElement);
			
			methodElement = document.CreateElement(methodName, service_namespace);
			foreach (KeyValuePair<string, object> parameter in parameters) {
				addParameter(document, methodElement, parameter);
			}
			bodyElement.AppendChild(methodElement);
			return document.InnerXml;
		}
		
		protected override string readContent(string methodName, string rawResponseContent) {
			XmlDocument document = new XmlDocument();
			XmlNamespaceManager xmlNamespaceManager;
			XmlNodeList bodyElement;
			
			document.LoadXml(rawResponseContent);
			xmlNamespaceManager = new XmlNamespaceManager(document.NameTable);
			xmlNamespaceManager.AddNamespace(xml_prefix, xml_namespace);
			bodyElement = document.SelectNodes("/soap12:Envelope[1]/soap12:Body[1]", xmlNamespaceManager);
			return bodyElement[0][methodName + "Response"][methodName + "Result"].InnerXml;
		}
		
		private void addParameter(XmlDocument document,
		                          XmlElement methodElement,
		                          KeyValuePair<string, object> parameter) {
			XmlElement parameterElement = document.CreateElement(parameter.Key, service_namespace);
			
			parameterElement.InnerText = parameter.Value.ToString();
			methodElement.AppendChild(parameterElement);
		}
		
		private void addAttribute(XmlDocument document,
		                          XmlElement element,
		                          string attributeName,
		                          string attributeValue) {
			XmlAttribute attribute = document.CreateAttribute(attributeName);
			attribute.Value = attributeValue;
			element.Attributes.Append(attribute);
		}
	}
}
