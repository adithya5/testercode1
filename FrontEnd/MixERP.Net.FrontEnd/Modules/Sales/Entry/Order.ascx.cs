﻿using MixERP.Net.BusinessLayer;
using MixERP.Net.Core.Modules.Sales.Resources;
using System;

namespace MixERP.Net.Core.Modules.Sales.Entry
{
    public partial class Order : MixERPUserControl
    {
        public override void OnControlLoad(object sender, EventArgs e)
        {
            SalesOrderControl.Text = Titles.SalesOrder;
            base.OnControlLoad(sender, e);
        }
    }
}