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

/*jshint -W032, -W098*/
/*global getAjax, getAjaxErrorMessage, logError, ajaxDataBind, ajaxUpdateVal, appendParameter, exchangeRateLocalized, getData, parseFloat2, parseInt2, partyDropDownList, repaint*/

var dueAmountInputText = $("#DueAmountInputText");
var costCenterSelect = $("#CostCenterSelect");
var currencyInputText = $("#CurrencyInputText");
var currencySelect = $("#CurrencySelect");
var amountInputText = $("#AmountInputText");
var amountInHomeCurrencyInputText = $("#AmountInHomeCurrencyInputText");
var debitExchangeRateInputText = $("#DebitExchangeRateInputText");
var debitExchangeRateInputTextLabel = $("label[for='DebitExchangeRateInputText']");

var creditExchangeRateInputText = $("#CreditExchangeRateInputText");
var creditExchangeRateInputTextLabel = $("label[for='CreditExchangeRateInputText']");

var baseAmountInputText = $("#BaseAmountInputText");
var finalDueAmountInputText = $("#FinalDueAmountInputText");

var instrumentCodeInputText = $("#InstrumentCodeInputText");

var cashButton = $("#CashButton");
var bankButton = $("#BankButton");
var cashRepositorySelect = $("#CashRepositorySelect");
var bankSelect = $("#BankSelect");
var postedDateTextBox = $("#PostedDateTextBox");
var referenceNumberInputText = $("#ReferenceNumberInputText");

var transactionCodeInputText = $("#TransactionCodeInputText");
var statementReferenceTextArea = $("#StatementReferenceTextArea");

var saveButton = $("#SaveButton");
var receiptTypeDiv = $("#ReceiptType");

//Variables
var homeCurrency = "";
var url = "";
var data = "";

receiptTypeDiv.find(".button").click(function () {
    toggleTransactionType($(this));
    repaint();
});

//Define a new callback function to subscribe to
//GoButton's click event
//in PartyControl.
var goButtonCallBack = function () {
    var totalDueAmountHidden = $("#TotalDueAmountHidden");
    var defaultCurrencySpan = $("#DefaultCurrencySpan");

    dueAmountInputText.val(totalDueAmountHidden.val());
    currencyInputText.val(defaultCurrencySpan.html());

    var ajaxGetHomeCurrency = getHomeCurrency();

    ajaxGetHomeCurrency.done(function (msg) {
        homeCurrency = msg.d;
        getExchangeRates();
        amountInputText.focus();
    });

    ajaxGetHomeCurrency.fail(function (xhr) {
        logAjaxErrorMessage(xhr);
    });
};

$(document).ready(function () {
    $("#receipt").appendTo("#home");
    loadCurrencies();
    loadCashRepositories();
    loadBankAccounts();
    loadCostCenters();
});

//Control Events

saveButton.click(function () {
    var partyCode = partyDropDownList.getSelectedValue();
    var currencyCode = currencySelect.getSelectedValue();
    var amount = parseFloat2(amountInputText.val());
    var debitExchangeRate = parseFloat2(debitExchangeRateInputText.val());
    var creditExchangeRate = parseFloat2(creditExchangeRateInputText.val());
    var referenceNumber = referenceNumberInputText.val();
    var statementReference = statementReferenceTextArea.val();
    var costCenterId = parseInt2(costCenterSelect.getSelectedValue());
    var cashRepositoryId = parseInt2(cashRepositorySelect.getSelectedValue());
    var postedDate = Date.parseExact(postedDateTextBox.val(), window.shortDateFormat);
    var bankAccountId = parseInt2(bankSelect.getSelectedValue());
    var bankInstrumentCode = instrumentCodeInputText.val();
    var bankTransactionCode = transactionCodeInputText.val();

    var ajaxSaveReceipt = saveReceipt(partyCode, currencyCode, amount, debitExchangeRate, creditExchangeRate, referenceNumber, statementReference, costCenterId, cashRepositoryId, postedDate, bankAccountId, bankInstrumentCode, bankTransactionCode);

    ajaxSaveReceipt.success(function (msg) {
        var id = msg.d;
        window.location = "/Modules/Sales/Confirmation/Receipt.mix?TranId=" + id;
    });

    ajaxSaveReceipt.fail(function (xhr) {
        logAjaxErrorMessage(xhr);
    });
});

function saveReceipt(partyCode, currencyCode, amount, debitExchangeRate, creditExchangeRate, referenceNumber, statementReference, costCenterId, cashRepositoryId, postedDate, bankAccountId, bankInstrumentCode, bankTransactionCode) {
    url = "/Modules/Sales/Services/Receipt/TransactionPosting.asmx/Save";
    data = appendParameter("", "partyCode", partyCode);
    data = appendParameter(data, "currencyCode", currencyCode);
    data = appendParameter(data, "amount", amount);
    data = appendParameter(data, "debitExchangeRate", debitExchangeRate);
    data = appendParameter(data, "creditExchangeRate", creditExchangeRate);
    data = appendParameter(data, "referenceNumber", referenceNumber);
    data = appendParameter(data, "statementReference", statementReference);
    data = appendParameter(data, "costCenterId", costCenterId);
    data = appendParameter(data, "cashRepositoryId", cashRepositoryId);
    data = appendParameter(data, "postedDate", postedDate);
    data = appendParameter(data, "bankAccountId", bankAccountId);
    data = appendParameter(data, "bankInstrumentCode", bankInstrumentCode);
    data = appendParameter(data, "bankTransactionCode", bankTransactionCode);
    data = getData(data);

    return getAjax(url, data);
};

currencySelect.blur(function () {
    getExchangeRates();
});

function getExchangeRates() {
    if (exchangeRateLocalized) {
        debitExchangeRateInputTextLabel.html(String.format("{0} ({1} - {2})", exchangeRateLocalized, currencySelect.getSelectedValue(), homeCurrency));
        creditExchangeRateInputTextLabel.html(String.format("{0} ({1} - {2})", exchangeRateLocalized, homeCurrency, currencyInputText.val()));
    };

    getER(debitExchangeRateInputText, currencySelect.getSelectedValue(), homeCurrency);
    getER(creditExchangeRateInputText, homeCurrency, currencyInputText.val());
}

function getER(associatedControl, sourceCurrencyCode, destinationCurrencyCode) {
    url = "/Modules/Sales/Services/Receipt/Currencies.asmx/GetExchangeRate";
    data = appendParameter("", "sourceCurrencyCode", sourceCurrencyCode);
    data = appendParameter(data, "destinationCurrencyCode", destinationCurrencyCode);
    data = getData(data);

    ajaxUpdateVal(url, data, associatedControl);
};

amountInputText.keyup(function () {
    updateTotal();
});

debitExchangeRateInputText.keyup(function () {
    updateTotal();
});

function updateTotal() {
    var due = parseFloat2(dueAmountInputText.val());
    var amount = parseFloat2(amountInputText.val());
    var er = parseFloat2(debitExchangeRateInputText.val());
    var er2 = parseFloat2(creditExchangeRateInputText.val());
    var toHomeCurrency = amount * er;

    amountInHomeCurrencyInputText.val(toHomeCurrency);

    var toBase = toHomeCurrency * er2;

    var remainingDue = due - toBase;

    baseAmountInputText.val(toBase);

    finalDueAmountInputText.val(remainingDue);

    finalDueAmountInputText.removeClass("alert-danger");

    if (remainingDue < 0) {
        finalDueAmountInputText.addClass("alert-danger");
    };
};

var toggleTransactionType = function (e) {
    if (e.attr("id") === "BankButton") {
        if (!$("#BankFormGroup").is(":visible")) {
            $("#BankFormGroup").show(500);
            $("#CashFormGroup").hide();
            return;
        };
    };

    if (e.attr("id") === "CashButton") {
        if (!$("#CashFormGroup").is(":visible")) {
            $("#CashFormGroup").show(500);
            $("#BankFormGroup").hide();
            return;
        };
    };
};

function loadCostCenters() {
    url = "/Modules/Sales/Services/Receipt/Accounts.asmx/GetCostCenters";
    ajaxDataBind(url, costCenterSelect);
};

function loadCurrencies() {
    url = "/Modules/Sales/Services/Receipt/Currencies.asmx/GetCurrencies";
    ajaxDataBind(url, currencySelect);
};

function loadCashRepositories() {
    url = "/Modules/Sales/Services/Receipt/Accounts.asmx/GetCashRepositories";
    ajaxDataBind(url, cashRepositorySelect);
};

function loadBankAccounts() {
    url = "/Modules/Sales/Services/Receipt/Accounts.asmx/GetBankAccounts";
    ajaxDataBind(url, bankSelect);
};

//Ajax Requests

function getHomeCurrency() {
    url = "/Modules/Sales/Services/Receipt/Currencies.asmx/GetHomeCurrency";
    return getAjax(url);
};