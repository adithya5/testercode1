﻿<%-- 
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
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/ContentMaster.Master" AutoEventWireup="true" CodeBehind="Delivery.aspx.cs" Inherits="MixERP.Net.FrontEnd.Sales.Entry.Delivery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptContentPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StyleSheetContentPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContentPlaceholder" runat="server">
    <mixerp:Product runat="server"
        ID="SalesDeliveryControl"
        Book="Sales"
        SubBook="Delivery"
        Text="<%$Resources:Titles, DeliveryWithoutSalesOrder %>"
        ShowPriceTypes="True"
        ShowShippingInformation="True"
        ShowSalesAgents="True"
        ShowStore="True"
        ShowCostCenter="True"
        VerifyStock="True"
        TopPanelWidth="750" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomScriptContentPlaceholder" runat="server">
    <script type="text/javascript">
        saveButton.click(function () {
            if (validateProductControl()) {
                save();
            };
        });

        var save = function () {
            var ajaxSalesDelivery = saveSalesDelivery(valueDate, storeId, partyCode, priceTypeId, referenceNumber, data, statementReference, transactionType, agentId, shipperId, shippingAddressCode, shippingCharge, cashRepositoryId, costCenterId, transactionIds, attachments);

            ajaxSalesDelivery.done(function (response) {
                var id = response.d;
                window.location = "/Sales/Confirmation/Delivery.aspx?TranId=" + id;
            });

            ajaxSalesDelivery.fail(function (jqXHR) {
                var errorMessage = JSON.parse(jqXHR.responseText).Message;
                errorLabelBottom.html(errorMessage);
                logError(errorMessage);
            });

        };

        var saveSalesDelivery = function (valueDate, storeId, partyCode, priceTypeId, referenceNumber, data, statementReference, transactionType, agentId, shipperId, shippingAddressCode, shippingCharge, cashRepositoryId, costCenterId, transactionIds, attachments) {
            var d = "";
            d = appendParameter(d, "valueDate", valueDate);
            d = appendParameter(d, "storeId", storeId);
            d = appendParameter(d, "partyCode", partyCode);
            d = appendParameter(d, "priceTypeId", priceTypeId);
            d = appendParameter(d, "referenceNumber", referenceNumber);
            d = appendParameter(d, "data", data);
            d = appendParameter(d, "statementReference", statementReference);
            d = appendParameter(d, "transactionType", transactionType);
            d = appendParameter(d, "agentId", agentId);
            d = appendParameter(d, "shipperId", shipperId);
            d = appendParameter(d, "shippingAddressCode", shippingAddressCode);
            d = appendParameter(d, "shippingCharge", shippingCharge);
            d = appendParameter(d, "cashRepositoryId", cashRepositoryId);
            d = appendParameter(d, "costCenterId", costCenterId);
            d = appendParameter(d, "transactionIds", transactionIds);
            d = appendParameter(d, "attachmentsJSON", attachments);

            d = "{" + d + "}";

            return $.ajax({
                type: "POST",
                url: "/Services/Sales/Delivery.asmx/Save",
                data: d,
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            });
        };

    </script>
</asp:Content>
