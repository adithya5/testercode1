﻿using MixERP.Net.Common.Helpers;
using MixERP.Net.Common.Models.Core;
using MixERP.Net.Common.Models.Transactions;
using System;
using System.Collections.ObjectModel;

namespace MixERP.Net.Core.Modules.Sales.Data.Helpers
{
    public static class Delivery
    {
        public static long Add(DateTime valueDate, int storeId, string partyCode, int priceTypeId, Collection<StockMasterDetailModel> details, int shipperId, string shippingAddressCode, decimal shippingCharge, int costCenterId, string referenceNumber, int agentId, string statementReference, Collection<int> transactionIdCollection, Collection<Attachment> attachments)
        {
            StockMasterModel stockMaster = new StockMasterModel();

            stockMaster.PartyCode = partyCode;
            stockMaster.StoreId = storeId;
            stockMaster.PriceTypeId = priceTypeId;
            stockMaster.ShipperId = shipperId;
            stockMaster.ShippingAddressCode = shippingAddressCode;
            stockMaster.ShippingCharge = shippingCharge;
            stockMaster.AgentId = agentId;

            long transactionMasterId = DatabaseLayer.Transactions.SalesDelivery.Add(valueDate, SessionHelper.GetOfficeId(), SessionHelper.GetUserId(), SessionHelper.GetLogOnId(), costCenterId, referenceNumber, statementReference, stockMaster, details, transactionIdCollection, attachments);
            DatabaseLayer.Transactions.Verification.CallAutoVerification(transactionMasterId);
            return transactionMasterId;
        }
    }
}