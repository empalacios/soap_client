/*
 * Created by SharpDevelop.
 * User: user
 * Date: 09/04/2018
 * Time: 10:19 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace soap_ws_client
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void Button1Click(object sender, EventArgs e)
		{
			Dictionary<string, object> parametros = new Dictionary<string, object>();
			parametros.Add("Fahrenheit", "10");
			
			a("https://www.w3schools.com/xml/tempconvert.asmx",
			  "https://www.w3schools.com/xml/",
			  "FahrenheitToCelsius",
			  parametros);
		}
		
		private void a(string service_uri, string service_namespace,
		               string methodName, Dictionary<string, object> parameters) {
			Soap1_1WsClient client = new Soap1_1WsClient(service_uri, service_namespace);
			
			textBox3.Text = client.callService(methodName, parameters);
		}
		
		private void b(string service_uri, string service_namespace,
		               string methodName, Dictionary<string, object> parameters) {
			Soap1_2WsClient client = new Soap1_2WsClient(service_uri, service_namespace);
			
			textBox3.Text = client.callService(methodName, parameters);
		}
	}

}
