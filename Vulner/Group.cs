using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulner
{
    class CommandGroup
    {
        public Dictionary<string, Command> C = null;
        public Command Fallback = null;
        public string Name = null;
        public CommandGroup(string Name, Main m)
        {
            this.Name = Name;
            C = new Dictionary<string, Command>();
            m.AddGroup(this, this.Name);
        }
        public void SetFallback( Command c )
        {
            this.Fallback = c;
        }
    }
}
