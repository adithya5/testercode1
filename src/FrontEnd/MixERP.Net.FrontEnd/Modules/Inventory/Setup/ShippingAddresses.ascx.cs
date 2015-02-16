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
using System.Reflection;
using MixERP.Net.Common.Helpers;
using MixERP.Net.Core.Modules.Inventory.Resources;
using MixERP.Net.FrontEnd.Base;
using MixERP.Net.FrontEnd.Controls;

namespace MixERP.Net.Core.Modules.Inventory.Setup
{
    public partial class ShippingAddresses : MixERPUserControl
    {
        public override void OnControlLoad(object sender, EventArgs e)
        {
            using (Scrud scrud = new Scrud())
            {
                scrud.KeyColumn = "shipping_address_id";
                scrud.TableSchema = "core";
                scrud.Table = "shipping_addresses";
                scrud.ViewSchema = "core";
                scrud.View = "shipping_address_scrud_view";

                //Shipping address code will be automatically generated on the database.
                scrud.Exclude = "shipping_address_code";

                scrud.DisplayFields = GetDisplayFields();
                scrud.DisplayViews = GetDisplayViews();

                scrud.Text = Titles.ShippingAddressMaintenance;
                scrud.ResourceAssembly = Assembly.GetAssembly(typeof (ShippingAddresses));

                this.ScrudPlaceholder.Controls.Add(scrud);
            }
        }

        private static string GetDisplayFields()
        {
            List<string> displayFields = new List<string>();
            ScrudHelper.AddDisplayField(displayFields, "core.parties.party_id", ConfigurationHelper.GetDbParameter("PartyDisplayField"));
            ScrudHelper.AddDisplayField(displayFields, "core.countries.country_id", "country_name");
            ScrudHelper.AddDisplayField(displayFields, "core.states.state_id", "state_name");
            return string.Join(",", displayFields);
        }

        private static string GetDisplayViews()
        {
            List<string> displayViews = new List<string>();
            ScrudHelper.AddDisplayView(displayViews, "core.parties.party_id", "core.party_scrud_view");
            ScrudHelper.AddDisplayView(displayViews, "core.countries.country_id", "core.country_scrud_view");
            ScrudHelper.AddDisplayView(displayViews, "core.states.state_id", "core.state_scrud_view");
            return string.Join(",", displayViews);
        }
    }
}