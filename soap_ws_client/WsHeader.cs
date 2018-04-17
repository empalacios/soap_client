/*
 * Created by SharpDevelop.
 * User: user
 * Date: 17/04/2018
 * Time: 11:30 a.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace soap_ws_client
{
	public class WsHeader
	{
		public string Name { get; set; }
		public Dictionary<string, object> Attributes { get; set; }
		
		public WsHeader(string name, Dictionary<string, object> attributes)
		{
			Name = name;
			Attributes = attributes;
		}
		
		public WsHeader(string name)
		{
			Name = name;
			Attributes = new Dictionary<string, object>();
		}
		
		public void addAttribute(string key, object value) {
			Attributes.Add(key, value);
		}
	}
}
