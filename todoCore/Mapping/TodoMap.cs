using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using todoCore.Domain;

namespace todoCore.Mapping
{
    public class TodoMap : ClassMap<Todo>
    {
        public TodoMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.DateTime);
        }
    }
}
