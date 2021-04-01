using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace StendenClicker.Library.Models
{
	public class InviteModel
	{
		public string UserGuid { get; set; }	
		public string UserName { get; set; }
		public ICommand OnAccept { get; set; }

		public ICommand OnDecline { get; set; }
	}
}
