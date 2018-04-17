/*
 * Created by SharpDevelop.
 * User: user
 * Date: 15/04/2018
 * Time: 11:08 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace soap_ws_client
{
	public class Soap1_1WsClient : SoapWsClient
	{
		public Soap1_1WsClient(string service_uri,
		                       string service_namespace)
			: base(service_uri, service_namespace, "http://schemas.xmlsoap.org/soap/envelope/", "soap")
		{
		}
		
		protected override HttpWebRequest createRequest(string methodName) {
			string soapAction;
			HttpWebRequest request = (HttpWebRequest) WebRequest.Create(service_uri);
			
			if (service_namespace.EndsWith("/")) {
				soapAction = service_namespace + methodName;
			} else {
				soapAction = service_namespace + "/" + methodName;
			}
			request.Headers.Add("SOAPAction", soapAction);
			request.ContentType = "text/xml; charset=utf-8";
			request.Accept = "text/xml; charset=utf-8";
			request.Method = "POST";
			return request;
		}
		
		protected override string createRequestContent(string methodName,
		                                               Dictionary<string, object> parameters) {
			XmlDocument document = new XmlDocument();
			XmlElement rootElement;
			XmlElement headerElement;
			XmlElement bodyElement;
			XmlElement methodElement;

			rootElement = document.CreateElement(xml_prefix, "Envelope", xml_namespace);
			addAttribute(document, rootElement, "xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
			addAttribute(document, rootElement, "xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
			document.AppendChild(rootElement);
			
			headerElement = document.CreateElement(xml_prefix, "Header", xml_namespace);
			foreach (WsHeader header in headers) {
				addHeader(document, headerElement, header);
			}
			rootElement.AppendChild(headerElement);
			
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
			bodyElement = document.SelectNodes("/soap:Envelope[1]/soap:Body[1]", xmlNamespaceManager);
			return bodyElement[0][methodName + "Response"][methodName + "Result"].InnerXml;
		}
		
		private void addHeader(XmlDocument document,
		                       XmlElement headerSectionElement,
		                       WsHeader header) {
			XmlElement headerElement = document.CreateElement(header.Name, service_namespace);
			
			addAttribute(document, headerElement, "custom-attribute", "value");
			
			foreach (KeyValuePair<string, object> headerAttribute in header.Attributes) {
				XmlElement headerAttributeElement = document.CreateElement(headerAttribute.Key, service_namespace);
				
				headerAttributeElement.InnerText = headerAttribute.Value.ToString();
				headerElement.AppendChild(headerAttributeElement);
			}
			headerSectionElement.AppendChild(headerElement);
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
