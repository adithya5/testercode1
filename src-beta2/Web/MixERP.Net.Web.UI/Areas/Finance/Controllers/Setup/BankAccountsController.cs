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

using System.Collections.Generic;
using System.Web.Mvc;
using MixERP.Net.Common.Helpers;
using MixERP.Net.UI.ScrudFactory;
using MixERP.Net.Web.UI.Finance.Resources;
using MixERP.Net.Web.UI.Providers;

namespace MixERP.Net.Web.UI.Finance.Controllers.Setup
{
    [RouteArea("Finance")]
    [RoutePrefix("setup/bank-accounts")]
    [Route("{action=index}")]
    public class BankAccountsController : ScrudController
    {
       public ActionResult Index()
        {
            const string view = "~/Areas/Finance/Views/Setup/BankAccounts.cshtml";
            return View(view, this.GetConfig());
        }

        public override Config GetConfig()
        {
            Config config = ScrudProvider.GetScrudConfig();
            {
                config.KeyColumn = "account_id";

                config.TableSchema = "core";
                config.Table = "bank_accounts";
                config.ViewSchema = "core";
                config.View = "bank_accounts";

                config.DisplayFields = GetDisplayFields();
                config.DisplayViews = GetDisplayViews();

                config.Text = Titles.BankAccounts;
                config.ResourceAssembly = (typeof(BankAccountsController)).Assembly;
                return config;

            }
        }

        private static string GetDisplayFields()
        {
            List<string> displayFields = new List<string>();
            ScrudHelper.AddDisplayField(displayFields, "office.users.user_id", ConfigurationHelper.GetDbParameter("UserDisplayField"));
            ScrudHelper.AddDisplayField(displayFields, "core.accounts.account_id", ConfigurationHelper.GetDbParameter("AccountDisplayField"));
            ScrudHelper.AddDisplayField(displayFields, "office.offices.office_id", ConfigurationHelper.GetDbParameter("OfficeDisplayField"));
            return string.Join(",", displayFields);
        }

        private static string GetDisplayViews()
        {
            List<string> displayViews = new List<string>();
            ScrudHelper.AddDisplayView(displayViews, "office.users.user_id", "office.user_selector_view");
            ScrudHelper.AddDisplayView(displayViews, "core.accounts.account_id", "core.account_scrud_view");
            ScrudHelper.AddDisplayView(displayViews, "office.offices.office_id", "office.office_scrud_view");
            return string.Join(",", displayViews);
        }
    }
}