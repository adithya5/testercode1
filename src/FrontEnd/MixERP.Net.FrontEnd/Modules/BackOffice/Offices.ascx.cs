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

using MixERP.Net.Common.Helpers;
using MixERP.Net.Core.Modules.BackOffice.Resources;
using MixERP.Net.FrontEnd.Base;
using MixERP.Net.WebControls.ScrudFactory;
using System;
using System.Collections.Generic;
using System.Reflection;
using MixERP.Net.Common.Domains;

namespace MixERP.Net.Core.Modules.BackOffice
{
    public partial class Offices : MixERPUserControl
    {
        public override AccessLevel AccessLevel
        {
            get
            {
                return AccessLevel.AdminOnly;
            }
        }

        public override void OnControlLoad(object sender, EventArgs e)
        {
            using (ScrudForm scrud = new ScrudForm())
            {
                scrud.KeyColumn = "office_id";

                scrud.TableSchema = "office";
                scrud.Table = "offices";
                scrud.ViewSchema = "office";
                scrud.View = "office_scrud_view";

                scrud.DisplayFields = GetDisplayFields();
                scrud.DisplayViews = GetDisplayViews();

                scrud.Text = Titles.OfficeSetup;
                scrud.ResourceAssembly = Assembly.GetAssembly(typeof(Offices));

                this.ScrudPlaceholder.Controls.Add(scrud);
            }

            
        }

        private static string GetDisplayFields()
        {
            List<string> displayFields = new List<string>();
            ScrudHelper.AddDisplayField(displayFields, "office.offices.office_id", ConfigurationHelper.GetDbParameter("OfficeDisplayField"));
            ScrudHelper.AddDisplayField(displayFields, "core.currencies.currency_code", ConfigurationHelper.GetDbParameter("CurrencyDisplayField"));
            return string.Join(",", displayFields);
        }

        private static string GetDisplayViews()
        {
            List<string> displayViews = new List<string>();
            ScrudHelper.AddDisplayView(displayViews, "office.offices.office_id", "office.office_scrud_view");
            ScrudHelper.AddDisplayView(displayViews, "core.currencies.currency_code", "core.currency_scrud_view");
            return string.Join(",", displayViews);
        }
    }
}