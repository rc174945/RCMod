using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utility
{
    class BaseCSVRowItem: BaseCSVObject
    {
        protected override char Delimiter => '|';
    }
}
