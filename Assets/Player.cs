using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class Player
    {
        public Guid Id { get; private set; }
            = Guid.NewGuid();
        public Interactable Focus { get; private set; }

        public void SetFocus(Interactable focus)
        {
            Focus = focus;
        }
    }
}
