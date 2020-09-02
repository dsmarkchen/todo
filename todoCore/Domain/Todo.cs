using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todoCore.Domain
{
    public class Todo
    {
        public virtual int Id
        {
            get;
            set;
        }
        public virtual string Name
        {
            get;
            set;
        }
        public virtual string Description
        {
            get;
            set;
        }

        public virtual DateTime DateTime
        {
            get;
            set;
        }


        public virtual bool Done
        {
            get;
            set;
        }
    }
}
