using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StendenClickerGame.Data
{
	public class Abilities
	{
		public string AbilitieName { get; set; }
		public string AbilitieDescription { get; set; }
		public bool IsOffCooldown { get; set; }
		public string Image { get; set; }

		public ICommand OnExecute { get; set; }
	}
}
