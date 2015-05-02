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

namespace MixERP.Net.Core.Modules.Inventory.Data.Reports
{
    public static class StockItems
    { 
        public static IEnumerable<DbListClosingStockResult> ListClosingStock(int storeId)
        {
            return Factory.Get<DbListClosingStockResult>("SELECT * FROM transactions.list_closing_stock(@0::integer);", storeId);
        }

        public static IEnumerable<DbGetStockAccountStatementResult> GetAccountStatement(DateTime @from, DateTime to, int userId, string itemCode, int storeId)
        {
            return Factory.Get<DbGetStockAccountStatementResult>("SELECT * FROM transactions.get_stock_account_statement(@0::date, @1::date, @2::integer, @3::text, @4::integer);", from, to, userId, itemCode, storeId);
        }
    }
}