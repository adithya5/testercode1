﻿/********************************************************************************
Copyright (C) Binod Nepal, Mix Open Foundation (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MixERP.Net.FrontEnd.Base;
using MixERP.Net.WebControls.ReportEngine;

namespace MixERP.Net.Core.Modules.Inventory.Reports
{
    public partial class InventoryAdvice : MixERPUserControl
    {
        public override void OnControlLoad(object sender, EventArgs e)
        {
            Collection<KeyValuePair<string, object>> list = new Collection<KeyValuePair<string, object>>();
            list.Add(new KeyValuePair<string, object>("@TranCode", this.Page.Request["TranCode"]));

            using (Report report = new Report())
            {
                report.AddParameterToCollection(list);
                report.AddParameterToCollection(list);
                report.AutoInitialize = true;
                report.ResourceAssembly = Assembly.GetAssembly(typeof(InventoryAdvice));
                report.Path = "~/Modules/Inventory/Reports/Source/Inventory.Advice.xml";

                this.Controls.Add(report);
            }
        }
    }
}