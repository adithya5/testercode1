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
using System.Globalization;
using System.Reflection;
using MixERP.Net.Common;
using MixERP.Net.Common.Extensions;
using MixERP.Net.Core.Modules.Inventory.Resources;
using MixERP.Net.FrontEnd.Base;
using MixERP.Net.FrontEnd.Cache;
using MixERP.Net.WebControls.ReportEngine;

namespace MixERP.Net.Core.Modules.Inventory.Reports
{
    public partial class AccountStatementReport : MixERPUserControl
    {
        public override void OnControlLoad(object sender, EventArgs e)
        {
            string itemCode = this.Page.Request["ItemCode"];
            DateTime from = Conversion.TryCastDate(this.Page.Request["From"]);
            DateTime to = Conversion.TryCastDate(this.Page.Request["To"]);
            int storeId = Conversion.TryCastInteger(this.Page.Request["StoreId"]);

            int userId = CurrentUser.GetSignInView().UserId.ToInt();


            Collection<KeyValuePair<string, object>> parameter1 = new Collection<KeyValuePair<string, object>>();
            parameter1.Add(new KeyValuePair<string, object>("@ItemCode", itemCode));

            Collection<KeyValuePair<string, object>> parameter2 = new Collection<KeyValuePair<string, object>>();
            parameter2.Add(new KeyValuePair<string, object>("@From", from));
            parameter2.Add(new KeyValuePair<string, object>("@To", to));
            parameter2.Add(new KeyValuePair<string, object>("@UserId", userId.ToString(CultureInfo.InvariantCulture)));
            parameter2.Add(new KeyValuePair<string, object>("@ItemCode", itemCode));
            parameter2.Add(new KeyValuePair<string, object>("@StoreId", storeId.ToString(CultureInfo.InvariantCulture)));

            using (Report report = new Report())
            {
                report.AddParameterToCollection(parameter1);
                report.AddParameterToCollection(parameter2);
                report.RunningTotalText = Titles.RunningTotal;
                report.ResourceAssembly = Assembly.GetAssembly(typeof (AccountStatementReport));
                report.Path = "~/Modules/Inventory/Reports/Source/Inventory.AccountStatement.xml";
                report.AutoInitialize = true;

                this.Controls.Add(report);
            }
        }
    }
}