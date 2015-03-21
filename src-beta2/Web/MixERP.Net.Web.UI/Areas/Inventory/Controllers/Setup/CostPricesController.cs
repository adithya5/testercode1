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
using MixERP.Net.UI.ScrudFactory;
using MixERP.Net.Web.UI.Inventory.Resources;
using MixERP.Net.Web.UI.Providers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MixERP.Net.Web.UI.Inventory.Controllers.Setup
{
    [RouteArea("Inventory")]
    [RoutePrefix("setup/cost-prices")]
    [Route("{action=index}")]
    public class CostPricesController : ScrudController
    {
        public ActionResult Index()
        {
            const string view = "~/Areas/Inventory/Views/Setup/CostPrices.cshtml";
            return View(view, this.GetConfig());
        }

        public override Config GetConfig()
        {
            Config config = ScrudProvider.GetScrudConfig();
            {
                config.KeyColumn = "item_cost_price_id";

                config.TableSchema = "core";
                config.Table = "item_cost_prices";
                config.ViewSchema = "core";
                config.View = "item_cost_price_scrud_view";

                config.DisplayFields = GetDisplayFields();
                config.DisplayViews = GetDisplayViews();

                config.Text = Titles.ItemCostPrices;
                config.ResourceAssembly = typeof(CostPricesController).Assembly;

                return config;
            }
        }

        private static string GetDisplayFields()
        {
            List<string> displayFields = new List<string>();
            ScrudHelper.AddDisplayField(displayFields, "core.items.item_id",
                ConfigurationHelper.GetDbParameter("ItemDisplayField"));
            ScrudHelper.AddDisplayField(displayFields, "core.parties.party_id",
                ConfigurationHelper.GetDbParameter("PartyDisplayField"));
            ScrudHelper.AddDisplayField(displayFields, "core.units.unit_id",
                ConfigurationHelper.GetDbParameter("UnitDisplayField"));
            return string.Join(",", displayFields);
        }

        private static string GetDisplayViews()
        {
            List<string> displayViews = new List<string>();
            ScrudHelper.AddDisplayView(displayViews, "core.items.item_id", "core.item_scrud_view");
            ScrudHelper.AddDisplayView(displayViews, "core.parties.party_id", "core.party_scrud_view");
            ScrudHelper.AddDisplayView(displayViews, "core.units.unit_id", "core.unit_scrud_view");
            return string.Join(",", displayViews);
        }
    }
}