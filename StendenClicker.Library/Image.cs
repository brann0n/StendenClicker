using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StendenClicker.Library
{
    public class Image
    {
        private string resourceName;
        public Image(string filename)
        {
            resourceName = filename;
        }

        public string GetFileLocation()
        {
            return resourceName;
        }
    }
}
