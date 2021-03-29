using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StendenClickerGame.Data
{
	public class abilities
	{
		public String AbilitieName { get; set; }
		public int Cooldown { get; set; }
		public String Image { get; set; }

		public ICommand OnExecute { get; set; }
	}
}
