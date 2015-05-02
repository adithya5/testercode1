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
using MixERP.Net.Entities;
using MixERP.Net.Entities.Transactions;

namespace MixERP.Net.WebControls.StockTransactionViewFactory.Data.Helpers
{
    public static class GLStockTransaction
    {
        public static IEnumerable<DbGetProductViewResult> GetView(int userId, string book, int officeId, DateTime dateFrom, DateTime dateTo, string office, string party, string priceType, string user, string referenceNumber, string statementReference)
        {
            return Factory.Get<DbGetProductViewResult>("SELECT * FROM transactions.get_product_view(@0::integer, @1::text, @2::integer, @3::date, @4::date, @5::national character varying(12), @6::text, @7::text, @8::national character varying(50), @9::national character varying(24), @10::text);", userId, book, officeId, dateFrom, dateTo, office, party, priceType, user, referenceNumber, statementReference);
        }
    }
}