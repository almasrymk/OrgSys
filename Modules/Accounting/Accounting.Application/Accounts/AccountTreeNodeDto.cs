using System;
using System.Collections.Generic;
using System.Text;

namespace Accounting.Application
{
    public class AccountTreeNodeDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }

        public List<AccountTreeNodeDto> Children { get; set; } = new();
    }
}
